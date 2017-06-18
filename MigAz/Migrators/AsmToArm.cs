using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MigAz.Providers;
using MigAz.Azure;
using MigAz.Azure.Arm;
using MigAz.Core.Interface;
using MigAz.Azure.Generator.AsmToArm;
using MigAz.Core;
using MigAz.Forms;
using MigAz.Azure.UserControls;
using System.Xml;

namespace MigAz.Migrators
{
    public partial class AsmToArm : IMigratorUserControl
    {
        #region Variables

        private UISaveSelectionProvider _saveSelectionProvider;
        private TreeNode _SourceAsmNode;
        private List<TreeNode> _SelectedNodes = new List<TreeNode>();
        private AzureTelemetryProvider _telemetryProvider;
        private AppSettingsProvider _appSettingsProvider;
        private AzureContext _AzureContextSourceASM;
        private AzureContext _AzureContextTargetARM;
        private List<Azure.MigrationTarget.NetworkSecurityGroup> _AsmTargetNetworkSecurityGroups;
        private List<Azure.MigrationTarget.StorageAccount> _AsmTargetStorageAccounts;
        private List<Azure.MigrationTarget.VirtualNetwork> _AsmTargetVirtualNetworks;
        private List<Azure.MigrationTarget.VirtualMachine> _AsmTargetVirtualMachines;
        private List<Azure.MigrationTarget.StorageAccount> _ArmTargetStorageAccounts;
        private List<Azure.MigrationTarget.VirtualNetwork> _ArmTargetVirtualNetworks;
        private List<Azure.MigrationTarget.VirtualMachine> _ArmTargetVirtualMachines;
        private List<Azure.MigrationTarget.AvailabilitySet> _ArmTargetAvailabilitySets;
        private List<Azure.MigrationTarget.Disk> _ArmTargetManagedDisks;
        private List<Azure.MigrationTarget.LoadBalancer> _ArmTargetLoadBalancers;
        private List<Azure.MigrationTarget.NetworkSecurityGroup> _ArmTargetNetworkSecurityGroups;
        private List<Azure.MigrationTarget.PublicIp> _ArmTargetPublicIPs;
        private PropertyPanel _PropertyPanel;
        private ImageList _AzureResourceImageList;
        private bool _IsAsmLoaded = false;
        private bool _IsArmLoaded = false;

        #endregion

        #region Constructors

        public AsmToArm() : base(null, null) { }

        public AsmToArm(IStatusProvider statusProvider, ILogProvider logProvider, PropertyPanel propertyPanel) 
            : base (statusProvider, logProvider)
        {
            InitializeComponent();

            _saveSelectionProvider = new UISaveSelectionProvider();
            _telemetryProvider = new AzureTelemetryProvider();
            _appSettingsProvider = new AppSettingsProvider();
            _PropertyPanel = propertyPanel;

            _AzureContextSourceASM = new AzureContext(LogProvider, StatusProvider, _appSettingsProvider);
            _AzureContextSourceASM.AzureEnvironmentChanged += _AzureContextSourceASM_AzureEnvironmentChanged;
            _AzureContextSourceASM.UserAuthenticated += _AzureContextSourceASM_UserAuthenticated;
            _AzureContextSourceASM.BeforeAzureSubscriptionChange += _AzureContextSourceASM_BeforeAzureSubscriptionChange;
            _AzureContextSourceASM.AfterAzureSubscriptionChange += _AzureContextSourceASM_AfterAzureSubscriptionChange;
            _AzureContextSourceASM.BeforeUserSignOut += _AzureContextSourceASM_BeforeUserSignOut;
            _AzureContextSourceASM.AfterUserSignOut += _AzureContextSourceASM_AfterUserSignOut;
            _AzureContextSourceASM.AfterAzureTenantChange += _AzureContextSourceASM_AfterAzureTenantChange;

            _AzureContextTargetARM = new AzureContext(LogProvider, StatusProvider, _appSettingsProvider);
            _AzureContextTargetARM.AfterAzureSubscriptionChange += _AzureContextTargetARM_AfterAzureSubscriptionChange;


            azureLoginContextViewerASM.Bind(_AzureContextSourceASM);
            azureLoginContextViewerARM.Bind(_AzureContextTargetARM);

            this.TemplateGenerator = new AzureGenerator(_AzureContextSourceASM.AzureSubscription, _AzureContextTargetARM.AzureSubscription, LogProvider, StatusProvider, _telemetryProvider, _appSettingsProvider);

            this.treeTargetARM.LogProvider = this.LogProvider;
            this.treeTargetARM.StatusProvider = this.StatusProvider;
            this.treeTargetARM.SettingsProvider = this.AzureContextSourceASM.SettingsProvider;

            this._PropertyPanel.LogProvider = this.LogProvider;
            this._PropertyPanel.StatusProvider = this.StatusProvider;
            this._PropertyPanel.AzureContext = _AzureContextTargetARM;
            this._PropertyPanel.TargetTreeView = treeTargetARM;
            this._PropertyPanel.PropertyChanged += _PropertyPanel_PropertyChanged;
        }

        public ImageList AzureResourceImageList
        {
            get { return _AzureResourceImageList; }
            set
            {
                _AzureResourceImageList = value;

                if (treeTargetARM != null)
                    treeTargetARM.ImageList = _AzureResourceImageList;

                if (treeSourceARM != null)
                    treeSourceARM.ImageList = _AzureResourceImageList;
            }
        }

        private async Task _PropertyPanel_PropertyChanged()
        {
            if (_SourceAsmNode == null && treeTargetARM.EventSourceNode == null) // we are not going to update on every property bind during TreeView updates
                await this.TemplateGenerator.UpdateArtifacts(treeTargetARM.GetExportArtifacts());
        }

        private async Task _AzureContextTargetARM_AfterAzureSubscriptionChange(AzureContext sender)
        {
            this.TemplateGenerator.TargetSubscription = sender.AzureSubscription;
        }

        private async Task _AzureContextSourceASM_AfterAzureTenantChange(AzureContext sender)
        {
            await _AzureContextTargetARM.CopyContext(_AzureContextSourceASM);
        }

        #endregion

        private async Task _AzureContextSourceASM_BeforeAzureSubscriptionChange(AzureContext sender)
        {
            await SaveSubscriptionSettings(sender.AzureSubscription);
            await _AzureContextTargetARM.SetSubscriptionContext(null);
        }

        private async Task _AzureContextSourceASM_AzureEnvironmentChanged(AzureContext sender)
        {
            app.Default.AzureEnvironment = sender.AzureEnvironment.ToString();
            app.Default.Save();

            if (_AzureContextTargetARM.TokenProvider == null)
                _AzureContextTargetARM.AzureEnvironment = sender.AzureEnvironment;
        }


        private async Task _AzureContextSourceASM_UserAuthenticated(AzureContext sender)
        {
            if (_AzureContextTargetARM.TokenProvider.AuthenticationResult == null)
            {
                await _AzureContextTargetARM.CopyContext(_AzureContextSourceASM);
            }
        }

        private async Task _AzureContextSourceASM_BeforeUserSignOut()
        {
            await SaveSubscriptionSettings(this._AzureContextSourceASM.AzureSubscription);
        }

        private async Task _AzureContextSourceASM_AfterUserSignOut()
        {
            ResetForm();
            await _AzureContextTargetARM.SetSubscriptionContext(null);
            await _AzureContextTargetARM.Logout();
            azureLoginContextViewerARM.Enabled = false;
            azureLoginContextViewerARM.Refresh();
        }

        private async Task _AzureContextSourceASM_AfterAzureSubscriptionChange(AzureContext sender)
        {
            ResetForm();

            try
            {
                if (sender.AzureSubscription != null)
                {
                    if (_AzureContextTargetARM.AzureSubscription == null)
                    {
                        await _AzureContextTargetARM.SetSubscriptionContext(_AzureContextSourceASM.AzureSubscription);
                    }

                    azureLoginContextViewerARM.Enabled = true;

                    this.TemplateGenerator.SourceSubscription = _AzureContextSourceASM.AzureSubscription;
                    this.TemplateGenerator.TargetSubscription = _AzureContextTargetARM.AzureSubscription;

                    switch (tabSourceResources.SelectedTab.Name)
                    {
                        case "tabPageAsm":
                            await BindAsmResources();
                            break;
                        case "tabPageArm":
                            await BindArmResources();
                            break;
                        default:
                            throw new ArgumentException("Unexpected Source Resource Tab: " + tabSourceResources.SelectedTab.Name);
                    }

                    _AzureContextSourceASM.AzureRetriever.SaveRestCache();
                    await ReadSubscriptionSettings(sender.AzureSubscription);

                    treeTargetARM.Enabled = true;
                }
            }
            catch (Exception exc)
            {
                UnhandledExceptionDialog unhandledException = new UnhandledExceptionDialog(LogProvider, exc);
                unhandledException.ShowDialog();
            }
            
            StatusProvider.UpdateStatus("Ready");
        }

        private async Task LoadARMManagedDisks(TreeNode tnResourceGroup, ResourceGroup armResourceGroup)
        {
            foreach (Azure.Arm.ManagedDisk armManagedDisk in await _AzureContextSourceASM.AzureRetriever.GetAzureARMManagedDisks(armResourceGroup))
            {
                Azure.MigrationTarget.Disk targetManagedDisk = new Azure.MigrationTarget.Disk(armManagedDisk);
                _ArmTargetManagedDisks.Add(targetManagedDisk);
            }
        }
        

        private async Task LoadARMPublicIPs(TreeNode tnResourceGroup, ResourceGroup armResourceGroup)
        {
            foreach (Azure.Arm.PublicIP armPublicIp in await _AzureContextSourceASM.AzureRetriever.GetAzureARMPublicIPs(armResourceGroup))
            {
                TreeNode publicIpParentNode = tnResourceGroup;

                Azure.MigrationTarget.PublicIp targetPublicIP = new Azure.MigrationTarget.PublicIp(armPublicIp);
                _ArmTargetPublicIPs.Add(targetPublicIP);

                TreeNode tnPublicIP = new TreeNode(targetPublicIP.SourceName);
                tnPublicIP.Name = targetPublicIP.SourceName;
                tnPublicIP.Tag = targetPublicIP;
                tnPublicIP.ImageKey = "PublicIp";
                tnPublicIP.SelectedImageKey = "PublicIp";
                publicIpParentNode.Nodes.Add(tnPublicIP);
            }
        }

        private async Task LoadARMLoadBalancers(TreeNode tnResourceGroup, ResourceGroup armResourceGroup)
        {
            foreach (Azure.Arm.LoadBalancer armLoadBalancer in await _AzureContextSourceASM.AzureRetriever.GetAzureARMLoadBalancers(armResourceGroup))
            {
                TreeNode networkSecurityGroupParentNode = tnResourceGroup;

                Azure.MigrationTarget.LoadBalancer targetLoadBalancer = new Azure.MigrationTarget.LoadBalancer(armLoadBalancer);
                foreach (Azure.MigrationTarget.FrontEndIpConfiguration targetFrontEndIpConfiguration in targetLoadBalancer.FrontEndIpConfigurations)
                {
                    if (targetFrontEndIpConfiguration.Source != null && targetFrontEndIpConfiguration.Source.PublicIP != null)
                    {
                        foreach (Azure.MigrationTarget.PublicIp targetPublicIp in _ArmTargetPublicIPs)
                        {
                            if (targetPublicIp.SourceName == targetFrontEndIpConfiguration.Source.PublicIP.Name)
                            {
                                targetFrontEndIpConfiguration.PublicIp = targetPublicIp;
                            }
                        }
                    }
                }
                _ArmTargetLoadBalancers.Add(targetLoadBalancer);

                TreeNode tnNetworkSecurityGroup = new TreeNode(targetLoadBalancer.SourceName);
                tnNetworkSecurityGroup.Name = targetLoadBalancer.SourceName;
                tnNetworkSecurityGroup.Tag = targetLoadBalancer;
                tnNetworkSecurityGroup.ImageKey = "LoadBalancer";
                tnNetworkSecurityGroup.SelectedImageKey = "LoadBalancer";
                networkSecurityGroupParentNode.Nodes.Add(tnNetworkSecurityGroup);
            }
        }
        private async Task LoadARMAvailabilitySets(TreeNode tnResourceGroup, ResourceGroup armResourceGroup)
        {
            foreach (Azure.Arm.AvailabilitySet armAvailabilitySet in await _AzureContextSourceASM.AzureRetriever.GetAzureARMAvailabilitySets(armResourceGroup))
            {
                TreeNode availabilitySetParentNode = tnResourceGroup;

                Azure.MigrationTarget.AvailabilitySet targetAvailabilitySet = new Azure.MigrationTarget.AvailabilitySet(_AzureContextTargetARM, armAvailabilitySet);
                _ArmTargetAvailabilitySets.Add(targetAvailabilitySet);

                TreeNode tnAvailabilitySet = new TreeNode(targetAvailabilitySet.SourceName);
                tnAvailabilitySet.Name = targetAvailabilitySet.SourceName;
                tnAvailabilitySet.Tag = targetAvailabilitySet;
                tnAvailabilitySet.ImageKey = "AvailabilitySet";
                tnAvailabilitySet.SelectedImageKey = "AvailabilitySet";
                availabilitySetParentNode.Nodes.Add(tnAvailabilitySet);
            }
        }

        private async Task LoadARMVirtualMachines(TreeNode tnResourceGroup, ResourceGroup armResourceGroup)
        {
            foreach (Azure.Arm.VirtualMachine armVirtualMachine in await _AzureContextSourceASM.AzureRetriever.GetAzureArmVirtualMachines(armResourceGroup))
            {
                TreeNode virtualMachineParentNode = tnResourceGroup;

                Azure.MigrationTarget.VirtualMachine targetVirtualMachine = new Azure.MigrationTarget.VirtualMachine(this.AzureContextTargetARM, armVirtualMachine, _ArmTargetVirtualNetworks, _ArmTargetStorageAccounts, _ArmTargetNetworkSecurityGroups);
                _ArmTargetVirtualMachines.Add(targetVirtualMachine);

                if (armVirtualMachine.AvailabilitySet != null)
                {
                    foreach (Azure.MigrationTarget.AvailabilitySet targetAvailabilitySet in _ArmTargetAvailabilitySets)
                    {
                        if (targetAvailabilitySet.SourceAvailabilitySet != null)
                        {
                            Azure.Arm.AvailabilitySet sourceAvailabilitySet = (Azure.Arm.AvailabilitySet)targetAvailabilitySet.SourceAvailabilitySet;
                            if (sourceAvailabilitySet.Id == armVirtualMachine.AvailabilitySet.Id)
                            {
                                targetVirtualMachine.TargetAvailabilitySet = targetAvailabilitySet;

                                TreeNode tnAvailabilitySet = GetAvailabilitySetTreeNode(virtualMachineParentNode, targetAvailabilitySet);
                                virtualMachineParentNode = tnAvailabilitySet;

                            }
                        }
                    }
                }

                TreeNode tnVirtualMachine = new TreeNode(targetVirtualMachine.SourceName);
                tnVirtualMachine.Name = targetVirtualMachine.SourceName;
                tnVirtualMachine.Tag = targetVirtualMachine;
                tnVirtualMachine.ImageKey = "VirtualMachine";
                tnVirtualMachine.SelectedImageKey = "VirtualMachine";
                virtualMachineParentNode.Nodes.Add(tnVirtualMachine);
            }
        }

        private async Task LoadARMStorageAccounts(TreeNode tnResourceGroup, ResourceGroup armResourceGroup)
        {
            foreach (Azure.Arm.StorageAccount armStorageAccount in await _AzureContextSourceASM.AzureRetriever.GetAzureARMStorageAccounts(armResourceGroup))
            {
                TreeNode storageAccountParentNode = tnResourceGroup;

                Azure.MigrationTarget.StorageAccount targetStorageAccount = new Azure.MigrationTarget.StorageAccount(_AzureContextTargetARM, armStorageAccount);
                _ArmTargetStorageAccounts.Add(targetStorageAccount);

                TreeNode tnStorageAccount = new TreeNode(targetStorageAccount.SourceName);
                tnStorageAccount.Name = targetStorageAccount.SourceName;
                tnStorageAccount.Tag = targetStorageAccount;
                tnStorageAccount.ImageKey = "StorageAccount";
                tnStorageAccount.SelectedImageKey = "StorageAccount";
                storageAccountParentNode.Nodes.Add(tnStorageAccount);
            }
        }

        private async Task LoadARMVirtualNetworks(TreeNode tnResourceGroup, ResourceGroup armResourceGroup)
        {
            foreach (Azure.Arm.VirtualNetwork armVirtualNetwork in await _AzureContextSourceASM.AzureRetriever.GetAzureARMVirtualNetworks(armResourceGroup))
            {
                TreeNode virtualNetworkParentNode = tnResourceGroup;

                Azure.MigrationTarget.VirtualNetwork targetVirtualNetwork = new Azure.MigrationTarget.VirtualNetwork(this.AzureContextTargetARM, armVirtualNetwork, _ArmTargetNetworkSecurityGroups);
                _ArmTargetVirtualNetworks.Add(targetVirtualNetwork);

                TreeNode tnVirtualNetwork = new TreeNode(targetVirtualNetwork.SourceName);
                tnVirtualNetwork.Name = targetVirtualNetwork.SourceName;
                tnVirtualNetwork.Tag = targetVirtualNetwork;
                tnVirtualNetwork.ImageKey = "VirtualNetwork";
                tnVirtualNetwork.SelectedImageKey = "VirtualNetwork";
                virtualNetworkParentNode.Nodes.Add(tnVirtualNetwork);
            }
        }

        private async Task LoadARMNetworkSecurityGroups(TreeNode tnResourceGroup, ResourceGroup armResourceGroup)
        {
            foreach (Azure.Arm.NetworkSecurityGroup armNetworkSecurityGroup in await _AzureContextSourceASM.AzureRetriever.GetAzureARMNetworkSecurityGroups(armResourceGroup))
            {
                TreeNode networkSecurityGroupParentNode = tnResourceGroup;

                Azure.MigrationTarget.NetworkSecurityGroup targetNetworkSecurityGroup = new Azure.MigrationTarget.NetworkSecurityGroup(this.AzureContextTargetARM, armNetworkSecurityGroup);
                _ArmTargetNetworkSecurityGroups.Add(targetNetworkSecurityGroup);

                TreeNode tnNetworkSecurityGroup = new TreeNode(targetNetworkSecurityGroup.SourceName);
                tnNetworkSecurityGroup.Name = targetNetworkSecurityGroup.SourceName;
                tnNetworkSecurityGroup.Tag = targetNetworkSecurityGroup;
                tnNetworkSecurityGroup.ImageKey = "NetworkSecurityGroup";
                tnNetworkSecurityGroup.SelectedImageKey = "NetworkSecurityGroup";
                networkSecurityGroupParentNode.Nodes.Add(tnNetworkSecurityGroup);
            }
        }

        internal void RemoveAsmTab()
        {
            tabSourceResources.TabPages.Remove(tabSourceResources.TabPages["tabPageAsm"]);
        }

        internal void RemoveArmTab()
        {
            tabSourceResources.TabPages.Remove(tabSourceResources.TabPages["tabPageArm"]);
        }

        internal void ChangeAzureContext()
        {
            azureLoginContextViewerASM.ChangeAzureContext();
        }

        private void ResetForm()
        {
            treeSourceASM.Nodes.Clear();
            treeSourceARM.Nodes.Clear();
            treeTargetARM.Nodes.Clear();
            _SelectedNodes.Clear();
            UpdateExportItemsCount();
            _PropertyPanel.Clear();
            treeSourceASM.Enabled = false;
            treeSourceARM.Enabled = false;
            treeTargetARM.Enabled = false;
            _IsAsmLoaded = false;
            _IsArmLoaded = false;
        }

        #region Properties


        public AzureContext AzureContextSourceASM
        {
            get { return _AzureContextSourceASM; }
        }

        public AzureContext AzureContextTargetARM
        {
            get { return _AzureContextTargetARM; }
        }

        public MigAz.Core.Interface.ITelemetryProvider TelemetryProvider
        {
            get { return _telemetryProvider; }
        }

        internal AppSettingsProvider AppSettingsProviders
        {
            get { return _appSettingsProvider; }
        }

        internal List<TreeNode> SelectedNodes
        {
            get { return _SelectedNodes; }
        }

        #endregion

        #region New Version Check

        private async Task AlertIfNewVersionAvailable()
        {
            string currentVersion = "2.2.10.0";
            VersionCheck versionCheck = new VersionCheck(this.LogProvider);
            string newVersionNumber = await versionCheck.GetAvailableVersion("https://api.migaz.tools/v1/version", currentVersion);
            if (versionCheck.IsVersionNewer(currentVersion, newVersionNumber))
            {
                DialogResult dialogresult = MessageBox.Show("New version " + newVersionNumber + " is available at http://aka.ms/MigAz", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        #endregion

        #region ASM TreeView Methods

        private async Task AutoSelectDependencies(TreeNode selectedNode)
        {
            // todo now asap russell, autoselect load balancers and public IPs

            if ((app.Default.AutoSelectDependencies) && (selectedNode.Checked) && (selectedNode.Tag != null))
            {
                if (selectedNode.Tag.GetType() == typeof(Azure.MigrationTarget.VirtualMachine))
                {
                    Azure.MigrationTarget.VirtualMachine targetVirtualMachine = (Azure.MigrationTarget.VirtualMachine)selectedNode.Tag;

                    if (targetVirtualMachine.Source != null)
                    {
                        if (targetVirtualMachine.Source.GetType() == typeof(Azure.Asm.VirtualMachine))
                        {
                            Azure.Asm.VirtualMachine asmVirtualMachine = (Azure.Asm.VirtualMachine)targetVirtualMachine.Source;

                            #region process virtual network

                            foreach (Azure.MigrationTarget.NetworkInterface networkInterface in targetVirtualMachine.NetworkInterfaces)
                            {
                                #region Auto Select Virtual Network from each IpConfiguration

                                foreach (Azure.MigrationTarget.NetworkInterfaceIpConfiguration ipConfiguration in networkInterface.TargetNetworkInterfaceIpConfigurations)
                                {
                                    if (ipConfiguration.TargetVirtualNetwork != null && ipConfiguration.TargetVirtualNetwork.GetType() == typeof(Azure.MigrationTarget.VirtualNetwork))
                                    {
                                        Azure.MigrationTarget.VirtualNetwork targetVirtualNetwork = (Azure.MigrationTarget.VirtualNetwork)ipConfiguration.TargetVirtualNetwork;
                                        foreach (TreeNode treeNode in treeSourceASM.Nodes.Find(targetVirtualNetwork.SourceName, true))
                                        {
                                            if ((treeNode.Tag != null) && (treeNode.Tag.GetType() == typeof(Azure.MigrationTarget.VirtualNetwork)))
                                            {
                                                if (!treeNode.Checked)
                                                    treeNode.Checked = true;
                                            }
                                        }
                                    }
                                }

                                #endregion

                                #region Auto Select Network Security Group

                                if (asmVirtualMachine.NetworkSecurityGroup != null)
                                {
                                    foreach (TreeNode treeNode in treeSourceASM.Nodes.Find(asmVirtualMachine.NetworkSecurityGroup.Name, true))
                                    {
                                        if ((treeNode.Tag != null) && (treeNode.Tag.GetType() == typeof(Azure.MigrationTarget.NetworkSecurityGroup)))
                                        {
                                            if (!treeNode.Checked)
                                                treeNode.Checked = true;
                                        }
                                    }
                                }
                                #endregion
                            }

                            #endregion

                            #region OS Disk Storage Account

                            foreach (TreeNode treeNode in treeSourceASM.Nodes.Find(asmVirtualMachine.OSVirtualHardDisk.StorageAccountName, true))
                            {
                                if ((treeNode.Tag != null) && (treeNode.Tag.GetType() == typeof(Azure.MigrationTarget.StorageAccount)))
                                {
                                    if (!treeNode.Checked)
                                        treeNode.Checked = true;
                                }
                            }

                            #endregion

                            #region Data Disk(s) Storage Account(s)

                            foreach (Azure.Asm.Disk dataDisk in asmVirtualMachine.DataDisks)
                            {
                                foreach (TreeNode treeNode in treeSourceASM.Nodes.Find(dataDisk.StorageAccountName, true))
                                {
                                    if ((treeNode.Tag != null) && (treeNode.Tag.GetType() == typeof(Azure.MigrationTarget.StorageAccount)))
                                    {
                                        if (!treeNode.Checked)
                                            treeNode.Checked = true;
                                    }
                                }
                            }

                            #endregion
                        }

                        else if (targetVirtualMachine.Source.GetType() == typeof(Azure.Arm.VirtualMachine))
                        {
                            Azure.Arm.VirtualMachine armVirtualMachine = (Azure.Arm.VirtualMachine)targetVirtualMachine.Source;

                            #region process virtual network

                            foreach (Azure.Arm.NetworkInterface networkInterface in armVirtualMachine.NetworkInterfaces)
                            {
                                foreach (Azure.Arm.NetworkInterfaceIpConfiguration ipConfiguration in networkInterface.NetworkInterfaceIpConfigurations)
                                {
                                    if (ipConfiguration.VirtualNetwork != null)
                                    {
                                        foreach (TreeNode treeNode in treeSourceARM.Nodes.Find(ipConfiguration.VirtualNetwork.Name, true))
                                        {
                                            if ((treeNode.Tag != null) && (treeNode.Tag.GetType() == typeof(Azure.MigrationTarget.VirtualNetwork)))
                                            {
                                                if (!treeNode.Checked)
                                                    treeNode.Checked = true;
                                            }
                                        }
                                    }
                                }
                            }

                            #endregion

                            #region OS Disk Storage Account

                            if (armVirtualMachine.OSVirtualHardDisk.GetType() == typeof(Azure.Arm.Disk)) // Disk in a Storage Account, not a Managed Disk
                            { 
                                foreach (TreeNode treeNode in treeSourceARM.Nodes.Find(((Azure.Arm.Disk)armVirtualMachine.OSVirtualHardDisk).StorageAccountName, true))
                                {
                                    if ((treeNode.Tag != null) && (treeNode.Tag.GetType() == typeof(Azure.MigrationTarget.StorageAccount)))
                                    {
                                        if (!treeNode.Checked)
                                            treeNode.Checked = true;
                                    }
                                }
                            }

                            #endregion

                            #region Data Disk(s) Storage Account(s)

                            foreach (Azure.Arm.Disk dataDisk in armVirtualMachine.DataDisks)
                            {
                                foreach (TreeNode treeNode in treeSourceARM.Nodes.Find(dataDisk.StorageAccountName, true))
                                {
                                    if ((treeNode.Tag != null) && (treeNode.Tag.GetType() == typeof(Azure.MigrationTarget.StorageAccount)))
                                    {
                                        if (!treeNode.Checked)
                                            treeNode.Checked = true;
                                    }
                                }
                            }

                            #endregion

                            #region Network Interface Card(s)

                            foreach (Azure.Arm.NetworkInterface networkInterface in armVirtualMachine.NetworkInterfaces)
                            {
                                foreach (Azure.Arm.NetworkInterfaceIpConfiguration ipConfiguration in networkInterface.NetworkInterfaceIpConfigurations)
                                {
                                    if (ipConfiguration.BackEndAddressPool != null && ipConfiguration.BackEndAddressPool.LoadBalancer != null)
                                    {
                                        foreach (TreeNode treeNode in treeSourceARM.Nodes.Find(ipConfiguration.BackEndAddressPool.LoadBalancer.Name, true))
                                        {
                                            if ((treeNode.Tag != null) && (treeNode.Tag.GetType() == typeof(Azure.MigrationTarget.LoadBalancer)))
                                            {
                                                if (!treeNode.Checked)
                                                    treeNode.Checked = true;
                                            }
                                        }
                                    }

                                    if (ipConfiguration.PublicIP != null)
                                    {
                                        foreach (TreeNode treeNode in treeSourceARM.Nodes.Find(ipConfiguration.PublicIP.Name, true))
                                        {
                                            if ((treeNode.Tag != null) && (treeNode.Tag.GetType() == typeof(Azure.MigrationTarget.PublicIp)))
                                            {
                                                if (!treeNode.Checked)
                                                    treeNode.Checked = true;
                                            }
                                        }
                                    }
                                }
                            }

                            #endregion
                        }
                    }
                }
                else if (selectedNode.Tag.GetType() == typeof(Azure.MigrationTarget.VirtualNetwork))
                {
                    Azure.MigrationTarget.VirtualNetwork targetVirtualNetwork = (Azure.MigrationTarget.VirtualNetwork)selectedNode.Tag;

                    foreach (Azure.MigrationTarget.Subnet targetSubnet in targetVirtualNetwork.TargetSubnets)
                    {
                        if (targetSubnet.NetworkSecurityGroup != null)
                        {
                            if (targetSubnet.NetworkSecurityGroup.SourceNetworkSecurityGroup != null)
                            {
                                if (targetSubnet.NetworkSecurityGroup.SourceNetworkSecurityGroup.GetType() == typeof(Azure.Asm.NetworkSecurityGroup))
                                {
                                    foreach (TreeNode treeNode in treeSourceASM.Nodes.Find(targetSubnet.NetworkSecurityGroup.SourceName, true))
                                    {
                                        if ((treeNode.Tag != null) && (treeNode.Tag.GetType() == typeof(Azure.MigrationTarget.NetworkSecurityGroup)))
                                        {
                                            if (!treeNode.Checked)
                                                treeNode.Checked = true;
                                        }
                                    }
                                }
                                else if (targetSubnet.NetworkSecurityGroup.SourceNetworkSecurityGroup.GetType() == typeof(Azure.Arm.NetworkSecurityGroup))
                                {
                                    foreach (TreeNode treeNode in treeSourceARM.Nodes.Find(targetSubnet.NetworkSecurityGroup.SourceName, true))
                                    {
                                        if ((treeNode.Tag != null) && (treeNode.Tag.GetType() == typeof(Azure.MigrationTarget.NetworkSecurityGroup)))
                                        {
                                            if (!treeNode.Checked)
                                                treeNode.Checked = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else if (selectedNode.Tag.GetType() == typeof(Azure.MigrationTarget.LoadBalancer))
                {
                    Azure.MigrationTarget.LoadBalancer targetLoadBalancer = (Azure.MigrationTarget.LoadBalancer)selectedNode.Tag;

                    if (targetLoadBalancer.Source != null)
                    {
                        if (targetLoadBalancer.Source.GetType() == typeof(Azure.Arm.LoadBalancer))
                        {
                            Azure.Arm.LoadBalancer armLoadBalaner = (Azure.Arm.LoadBalancer)targetLoadBalancer.Source;

                            foreach (Azure.Arm.FrontEndIpConfiguration frontEndIpConfiguration in armLoadBalaner.FrontEndIpConfigurations)
                            {
                                if (frontEndIpConfiguration.PublicIP != null)
                                {
                                    foreach (TreeNode treeNode in treeSourceARM.Nodes.Find(frontEndIpConfiguration.PublicIP.Name, true))
                                    {
                                        if ((treeNode.Tag != null) && (treeNode.Tag.GetType() == typeof(Azure.MigrationTarget.PublicIp)))
                                        {
                                            if (!treeNode.Checked)
                                                treeNode.Checked = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                StatusProvider.UpdateStatus("Ready");
            }
        }

        private List<TreeNode> GetSelectedNodes(TreeView treeView)
        {
            List<TreeNode> selectedNodes = new List<TreeNode>();
            foreach (TreeNode treeNode in treeView.Nodes)
            {
                RecursiveNodeSelectedAdd(ref selectedNodes, treeNode);
            }
            return selectedNodes;
        }

        private void RecursiveNodeSelectedAdd(ref List<TreeNode> selectedNodes, TreeNode parentNode)
        {
            if (parentNode.Checked && parentNode.Tag != null && 
                (parentNode.Tag.GetType() == typeof(Azure.MigrationTarget.NetworkSecurityGroup) || 
                parentNode.Tag.GetType() == typeof(Azure.MigrationTarget.VirtualNetwork) || 
                parentNode.Tag.GetType() == typeof(Azure.MigrationTarget.StorageAccount) ||
                parentNode.Tag.GetType() == typeof(Azure.MigrationTarget.VirtualMachine)
                ))
                selectedNodes.Add(parentNode);

            foreach (TreeNode childNode in parentNode.Nodes)
            {
                RecursiveNodeSelectedAdd(ref selectedNodes, childNode);
            }
        }
        #endregion

        #region ASM TreeView Events

        private void treeASM_AfterSelect(object sender, TreeViewEventArgs e)
        {
            _PropertyPanel.Clear();
        }

        private async void treeASM_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (_SourceAsmNode == null)
            {
                _SourceAsmNode = e.Node;
            }

            if (e.Node.Checked)
                await AutoSelectDependencies(e.Node);

            TreeNode resultUpdateARMTree = null;

            if (e.Node.Tag != null)
            { 
                Type tagType = e.Node.Tag.GetType();
                if ((tagType == typeof(Azure.MigrationTarget.VirtualNetwork)) ||
                    (tagType == typeof(Azure.MigrationTarget.StorageAccount)) ||
                    (tagType == typeof(Azure.MigrationTarget.VirtualMachine)) ||
                    (tagType == typeof(Azure.MigrationTarget.LoadBalancer)) ||
                    (tagType == typeof(Azure.MigrationTarget.PublicIp)) ||
                    (tagType == typeof(Azure.MigrationTarget.NetworkSecurityGroup)))
                {
                    if (e.Node.Checked)
                    {
                        resultUpdateARMTree = await treeTargetARM.AddMigrationTargetToTargetTree((MigAz.Core.Interface.IMigrationTarget)e.Node.Tag);
                    }
                    else
                    {
                        await treeTargetARM.RemoveASMNodeFromARMTree((MigAz.Core.Interface.IMigrationTarget)e.Node.Tag);
                    }
                }
            }

            if (_SourceAsmNode != null && _SourceAsmNode == e.Node)
            {
                if (e.Node.Checked)
                {
                    await RecursiveCheckToggleDown(e.Node, e.Node.Checked);
                    FillUpIfFullDown(e.Node);
                    treeSourceASM.SelectedNode = e.Node;
                }
                else
                {
                    await RecursiveCheckToggleUp(e.Node, e.Node.Checked);
                    await RecursiveCheckToggleDown(e.Node, e.Node.Checked);
                }

                _SelectedNodes = this.GetSelectedNodes(treeSourceASM);
                UpdateExportItemsCount();
                await this.TemplateGenerator.UpdateArtifacts(treeTargetARM.GetExportArtifacts());

                _SourceAsmNode = null;

                if (resultUpdateARMTree != null)
                    treeTargetARM.SelectedNode = resultUpdateARMTree;
            }
        }

        #endregion

        #region ARM TreeView Methods




        

        private TreeNode GetResourceGroupTreeNode(TreeNode subscriptionNode, ResourceGroup resourceGroup)
        {
            foreach (TreeNode treeNode in subscriptionNode.Nodes)
            {
                if (treeNode.Tag != null)
                {
                    if (treeNode.Tag.GetType() == resourceGroup.GetType() && treeNode.Text == resourceGroup.ToString())
                        return treeNode;
                }
            }

            TreeNode tnResourceGroup = new TreeNode(resourceGroup.ToString());
            tnResourceGroup.Text = resourceGroup.ToString();
            tnResourceGroup.Tag = resourceGroup;
            tnResourceGroup.ImageKey = "ResourceGroup";
            tnResourceGroup.SelectedImageKey = "ResourceGroup";

            subscriptionNode.Nodes.Add(tnResourceGroup);
            return tnResourceGroup;
        }

        private TreeNode GetAvailabilitySetTreeNode(TreeNode subscriptionNode, Azure.MigrationTarget.AvailabilitySet availabilitySet)
        {
            foreach (TreeNode treeNode in subscriptionNode.Nodes)
            {
                if (treeNode.Tag != null)
                {
                    if (treeNode.Tag.GetType() == availabilitySet.GetType() && treeNode.Text == availabilitySet.ToString())
                        return treeNode;
                }
            }

            TreeNode tnAvailabilitySet = new TreeNode(availabilitySet.ToString());
            tnAvailabilitySet.Text = availabilitySet.ToString();
            tnAvailabilitySet.Tag = availabilitySet;
            tnAvailabilitySet.ImageKey = "AvailabilitySet";
            tnAvailabilitySet.SelectedImageKey = "AvailabilitySet";

            subscriptionNode.Nodes.Add(tnAvailabilitySet);
            tnAvailabilitySet.Expand();
            return tnAvailabilitySet;
        }

        
        private TreeNode GetAvailabilitySetNode(TreeNode subscriptionNode, Azure.Asm.VirtualMachine virtualMachine)
        {
            string availabilitySetName = String.Empty;

            if (virtualMachine.AvailabilitySetName != String.Empty)
                availabilitySetName = virtualMachine.AvailabilitySetName;
            else
                availabilitySetName = virtualMachine.CloudServiceName;

            foreach (TreeNode treeNode in subscriptionNode.Nodes)
            {
                if (treeNode.Tag != null)
                {
                    if (treeNode.Tag.GetType() == typeof(Azure.MigrationTarget.AvailabilitySet) && treeNode.Text == availabilitySetName)
                        return treeNode;
                }
            }

            TreeNode tnAvailabilitySet = new TreeNode(availabilitySetName);
            tnAvailabilitySet.Text = availabilitySetName;
            tnAvailabilitySet.Tag = new Azure.MigrationTarget.AvailabilitySet(this.AzureContextTargetARM, availabilitySetName);
            tnAvailabilitySet.ImageKey = "AvailabilitySet";
            tnAvailabilitySet.SelectedImageKey = "AvailabilitySet";

            subscriptionNode.Nodes.Add(tnAvailabilitySet);
            tnAvailabilitySet.Expand();
            return tnAvailabilitySet;
        }

        private TreeNode GetDataCenterTreeViewNode(TreeNode subscriptionNode, string dataCenter, string containerName)
        {
            TreeNode dataCenterNode = null;

            foreach (TreeNode treeNode in subscriptionNode.Nodes)
            {
                if (treeNode.Text == dataCenter && treeNode.Tag.ToString() == "DataCenter")
                {
                    dataCenterNode = treeNode;

                    foreach (TreeNode dataCenterContainerNode in treeNode.Nodes)
                    {
                        if (dataCenterContainerNode.Text == containerName)
                            return dataCenterContainerNode;
                    }
                }
            }

            if (dataCenterNode == null)
            {
                dataCenterNode = new TreeNode(dataCenter);
                dataCenterNode.Tag = "DataCenter";
                subscriptionNode.Nodes.Add(dataCenterNode);
                dataCenterNode.Expand();
            }

            TreeNode containerNode = new TreeNode(containerName);
            dataCenterNode.Nodes.Add(containerNode);
            containerNode.Expand();

            return containerNode;
        }

        private void FillUpIfFullDown(TreeNode node)
        {
            if (IsSelectedFullDown(node) && (node.Parent != null))
            {
                node = node.Parent;

                while (node != null)
                {
                    if (AllChildrenChecked(node))
                    {
                        node.Checked = true;
                        node = node.Parent;
                    }
                    else
                        node = null;
                }
            }
        }

        private bool AllChildrenChecked(TreeNode node)
        {
            foreach (TreeNode childNode in node.Nodes)
                if (!childNode.Checked)
                    return false;

            return true;
        }

        private bool IsSelectedFullDown(TreeNode node)
        {
            if (!node.Checked)
                return false;

            foreach (TreeNode childNode in node.Nodes)
            {
                if (!IsSelectedFullDown(childNode))
                    return false;
            }

            return true;
        }

        private async Task RecursiveCheckToggleDown(TreeNode node, bool isChecked)
        {
            if (node.Checked != isChecked)
            {
                node.Checked = isChecked;
            }

            foreach (TreeNode subNode in node.Nodes)
            {
                await RecursiveCheckToggleDown(subNode, isChecked);
            }
        }
        private async Task RecursiveCheckToggleUp(TreeNode node, bool isChecked)
        {
            if (node.Checked != isChecked)
            {
                node.Checked = isChecked;
            }

            if (node.Parent != null)
                await RecursiveCheckToggleUp(node.Parent, isChecked);
        }

        #endregion

        #region Form Controls

        #region Export Button

        public async void Export()
        {
            await SaveSubscriptionSettings(_AzureContextSourceASM.AzureSubscription);
        }

        #endregion

        #endregion

        #region Form Events

        private async void AsmToArmForm_Load(object sender, EventArgs e)
        {
            LogProvider.WriteLog("AsmToArmForm_Load", "Program start");

            AsmToArmForm_Resize(null, null);
            ResetForm();

            try
            {
                _AzureContextSourceASM.AzureEnvironment = (AzureEnvironment)Enum.Parse(typeof(AzureEnvironment), app.Default.AzureEnvironment);
            }
            catch
            {
                _AzureContextSourceASM.AzureEnvironment = AzureEnvironment.AzureCloud;
            }

            await AlertIfNewVersionAvailable(); // check if there a new version of the app
        }

        private void AsmToArmForm_Resize(object sender, EventArgs e)
        {
            tabSourceResources.Height = this.Height - 130;
            treeSourceASM.Width = tabSourceResources.Width - 8;
            treeSourceASM.Height = tabSourceResources.Height - 26;
            treeSourceARM.Width = tabSourceResources.Width - 8;
            treeSourceARM.Height = tabSourceResources.Height - 26;
            treeTargetARM.Height = this.Height - 150;
        }

        #endregion

        #region Subscription Settings Read / Save Methods

        private async Task SaveSubscriptionSettings(AzureSubscription azureSubscription)
        {
            // If save selection option is enabled
            if (app.Default.SaveSelection && azureSubscription != null)
            {
                await _saveSelectionProvider.Save(azureSubscription.SubscriptionId, _SelectedNodes);
            }
        }

        private async Task ReadSubscriptionSettings(AzureSubscription azureSubscription)
        {
            // If save selection option is enabled
            if (app.Default.SaveSelection)
            {
                StatusProvider.UpdateStatus("BUSY: Reading saved selection");
                await _saveSelectionProvider.Read(azureSubscription.SubscriptionId, _AzureContextSourceASM.AzureRetriever, AzureContextTargetARM.AzureRetriever, treeSourceASM);
                UpdateExportItemsCount();
            }
        }

        #endregion

        private void UpdateExportItemsCount()
        {
            Int32 selectedExportCount = 0;

            if (_SelectedNodes != null)
            {
                selectedExportCount = _SelectedNodes.Count();
            }
        }

        public override void SeekAlertSource(object sourceObject)
        {
            treeTargetARM.SeekAlertSource(sourceObject);
        }

        public override void PostTelemetryRecord()
        {
            _telemetryProvider.PostTelemetryRecord((AzureGenerator) this.TemplateGenerator);
        }

        private async Task BindAsmResources()
        {
            if (!_IsAsmLoaded)
            {
                _IsAsmLoaded = true;
                treeSourceASM.Nodes.Clear();

                try
                {
                    _AsmTargetNetworkSecurityGroups = new List<Azure.MigrationTarget.NetworkSecurityGroup>();
                    _AsmTargetStorageAccounts = new List<Azure.MigrationTarget.StorageAccount>();
                    _AsmTargetVirtualNetworks = new List<Azure.MigrationTarget.VirtualNetwork>();
                    _AsmTargetVirtualMachines = new List<Azure.MigrationTarget.VirtualMachine>();

                    if (_AzureContextSourceASM != null && _AzureContextSourceASM.AzureSubscription != null)
                    {
                        TreeNode subscriptionNodeASM = new TreeNode(_AzureContextSourceASM.AzureSubscription.Name);
                        treeSourceASM.Nodes.Add(subscriptionNodeASM);
                        subscriptionNodeASM.Expand();

                        foreach (Azure.Asm.NetworkSecurityGroup asmNetworkSecurityGroup in await _AzureContextSourceASM.AzureRetriever.GetAzureAsmNetworkSecurityGroups())
                        {
                            TreeNode parentNode = GetDataCenterTreeViewNode(subscriptionNodeASM, asmNetworkSecurityGroup.Location, "Network Security Groups");

                            // Ensure we load the Full Details to get NSG Rules
                            Azure.Asm.NetworkSecurityGroup asmNetworkSecurityGroupFullDetail = await _AzureContextSourceASM.AzureRetriever.GetAzureAsmNetworkSecurityGroup(asmNetworkSecurityGroup.Name);

                            Azure.MigrationTarget.NetworkSecurityGroup targetNetworkSecurityGroup = new Azure.MigrationTarget.NetworkSecurityGroup(this.AzureContextTargetARM, asmNetworkSecurityGroupFullDetail);
                            _AsmTargetNetworkSecurityGroups.Add(targetNetworkSecurityGroup);

                            TreeNode tnNetworkSecurityGroup = new TreeNode(targetNetworkSecurityGroup.SourceName);
                            tnNetworkSecurityGroup.Name = targetNetworkSecurityGroup.SourceName;
                            tnNetworkSecurityGroup.Tag = targetNetworkSecurityGroup;
                            parentNode.Nodes.Add(tnNetworkSecurityGroup);
                            parentNode.Expand();
                        }

                        List<Azure.Asm.VirtualNetwork> asmVirtualNetworks = await _AzureContextSourceASM.AzureRetriever.GetAzureAsmVirtualNetworks();
                        foreach (Azure.Asm.VirtualNetwork asmVirtualNetwork in asmVirtualNetworks)
                        {
                            TreeNode parentNode = GetDataCenterTreeViewNode(subscriptionNodeASM, asmVirtualNetwork.Location, "Virtual Networks");

                            Azure.MigrationTarget.VirtualNetwork targetVirtualNetwork = new Azure.MigrationTarget.VirtualNetwork(this.AzureContextTargetARM, asmVirtualNetwork, _AsmTargetNetworkSecurityGroups);
                            _AsmTargetVirtualNetworks.Add(targetVirtualNetwork);

                            TreeNode tnVirtualNetwork = new TreeNode(targetVirtualNetwork.SourceName);
                            tnVirtualNetwork.Name = targetVirtualNetwork.SourceName;
                            tnVirtualNetwork.Text = targetVirtualNetwork.SourceName;
                            tnVirtualNetwork.Tag = targetVirtualNetwork;
                            parentNode.Nodes.Add(tnVirtualNetwork);
                            parentNode.Expand();
                        }

                        foreach (Azure.Asm.StorageAccount asmStorageAccount in await _AzureContextSourceASM.AzureRetriever.GetAzureAsmStorageAccounts())
                        {
                            TreeNode parentNode = GetDataCenterTreeViewNode(subscriptionNodeASM, asmStorageAccount.GeoPrimaryRegion, "Storage Accounts");

                            Azure.MigrationTarget.StorageAccount targetStorageAccount = new Azure.MigrationTarget.StorageAccount(_AzureContextTargetARM, asmStorageAccount);
                            _AsmTargetStorageAccounts.Add(targetStorageAccount);

                            TreeNode tnStorageAccount = new TreeNode(targetStorageAccount.SourceName);
                            tnStorageAccount.Name = targetStorageAccount.SourceName;
                            tnStorageAccount.Tag = targetStorageAccount;
                            parentNode.Nodes.Add(tnStorageAccount);
                            parentNode.Expand();
                        }

                        List<Azure.Asm.CloudService> asmCloudServices = await _AzureContextSourceASM.AzureRetriever.GetAzureAsmCloudServices();
                        foreach (Azure.Asm.CloudService asmCloudService in asmCloudServices)
                        {
                            List<Azure.MigrationTarget.VirtualMachine> cloudServiceTargetVirtualMachines = new List<Azure.MigrationTarget.VirtualMachine>();
                            Azure.MigrationTarget.AvailabilitySet targetAvailabilitySet = new Azure.MigrationTarget.AvailabilitySet(_AzureContextTargetARM, asmCloudService);

                            TreeNode parentNode = GetDataCenterTreeViewNode(subscriptionNodeASM, asmCloudService.Location, "Cloud Services");
                            TreeNode[] cloudServiceNodeSearch = parentNode.Nodes.Find(asmCloudService.Name, false);
                            TreeNode cloudServiceNode = null;
                            if (cloudServiceNodeSearch.Count() == 1)
                            {
                                cloudServiceNode = cloudServiceNodeSearch[0];
                            }

                            if (cloudServiceNode == null)
                            {
                                cloudServiceNode = new TreeNode(asmCloudService.Name);
                                cloudServiceNode.Name = asmCloudService.Name;
                                cloudServiceNode.Tag = targetAvailabilitySet;
                                parentNode.Nodes.Add(cloudServiceNode);
                                parentNode.Expand();
                            }

                            foreach (Azure.Asm.VirtualMachine asmVirtualMachine in asmCloudService.VirtualMachines)
                            {
                                Azure.MigrationTarget.VirtualMachine targetVirtualMachine = new Azure.MigrationTarget.VirtualMachine(this.AzureContextTargetARM, asmVirtualMachine, _AsmTargetVirtualNetworks, _AsmTargetStorageAccounts, _AsmTargetNetworkSecurityGroups);
                                targetVirtualMachine.TargetAvailabilitySet = targetAvailabilitySet;
                                cloudServiceTargetVirtualMachines.Add(targetVirtualMachine);
                                _AsmTargetVirtualMachines.Add(targetVirtualMachine);

                                TreeNode virtualMachineNode = new TreeNode(targetVirtualMachine.SourceName);
                                virtualMachineNode.Name = targetVirtualMachine.SourceName;
                                virtualMachineNode.Tag = targetVirtualMachine;
                                cloudServiceNode.Nodes.Add(virtualMachineNode);
                                cloudServiceNode.Expand();
                            }

                            Azure.MigrationTarget.LoadBalancer targetLoadBalancer = new Azure.MigrationTarget.LoadBalancer();
                            targetLoadBalancer.Name = asmCloudService.Name;
                            targetLoadBalancer.SourceName = asmCloudService.Name + "-LB";

                            TreeNode loadBalancerNode = new TreeNode(targetLoadBalancer.SourceName);
                            loadBalancerNode.Name = targetLoadBalancer.SourceName;
                            loadBalancerNode.Tag = targetLoadBalancer;
                            cloudServiceNode.Nodes.Add(loadBalancerNode);
                            cloudServiceNode.Expand();

                            Azure.MigrationTarget.FrontEndIpConfiguration frontEndIpConfiguration = new Azure.MigrationTarget.FrontEndIpConfiguration(targetLoadBalancer);

                            Azure.MigrationTarget.BackEndAddressPool backEndAddressPool = new Azure.MigrationTarget.BackEndAddressPool(targetLoadBalancer);

                            // if internal load balancer
                            if (asmCloudService.ResourceXml.SelectNodes("//Deployments/Deployment/LoadBalancers/LoadBalancer/FrontendIpConfiguration/Type").Count > 0)
                            {
                                string virtualnetworkname = asmCloudService.ResourceXml.SelectSingleNode("//Deployments/Deployment/VirtualNetworkName").InnerText;
                                string subnetname = asmCloudService.ResourceXml.SelectSingleNode("//Deployments/Deployment/LoadBalancers/LoadBalancer/FrontendIpConfiguration/SubnetName").InnerText.Replace(" ", "");

                                if (asmCloudService.ResourceXml.SelectNodes("//Deployments/Deployment/LoadBalancers/LoadBalancer/FrontendIpConfiguration/StaticVirtualNetworkIPAddress").Count > 0)
                                {
                                    frontEndIpConfiguration.PrivateIPAllocationMethod = "Static";
                                    frontEndIpConfiguration.PrivateIPAddress = asmCloudService.ResourceXml.SelectSingleNode("//Deployments/Deployment/LoadBalancers/LoadBalancer/FrontendIpConfiguration/StaticVirtualNetworkIPAddress").InnerText;
                                }

                                if (cloudServiceTargetVirtualMachines.Count > 0)
                                {
                                    if (cloudServiceTargetVirtualMachines[0].PrimaryNetworkInterface != null)
                                    {
                                        if (cloudServiceTargetVirtualMachines[0].PrimaryNetworkInterface.TargetNetworkInterfaceIpConfigurations.Count > 0)
                                        {
                                            frontEndIpConfiguration.TargetVirtualNetwork = cloudServiceTargetVirtualMachines[0].PrimaryNetworkInterface.TargetNetworkInterfaceIpConfigurations[0].TargetVirtualNetwork;
                                            frontEndIpConfiguration.TargetSubnet = cloudServiceTargetVirtualMachines[0].PrimaryNetworkInterface.TargetNetworkInterfaceIpConfigurations[0].TargetSubnet;
                                        }
                                    }
                                }
                            }
                            else // if external load balancer
                            {
                                Azure.MigrationTarget.PublicIp loadBalancerPublicIp = new Azure.MigrationTarget.PublicIp();
                                loadBalancerPublicIp.SourceName = asmCloudService.Name + "-PIP";
                                loadBalancerPublicIp.Name = asmCloudService.Name;
                                loadBalancerPublicIp.DomainNameLabel = asmCloudService.Name;
                                frontEndIpConfiguration.PublicIp = loadBalancerPublicIp;

                                TreeNode publicIPAddressNode = new TreeNode(loadBalancerPublicIp.SourceName);
                                publicIPAddressNode.Name = loadBalancerPublicIp.SourceName;
                                publicIPAddressNode.Tag = loadBalancerPublicIp;
                                cloudServiceNode.Nodes.Add(publicIPAddressNode);
                                cloudServiceNode.Expand();
                            }

                            foreach (Azure.MigrationTarget.VirtualMachine targetVirtualMachine in cloudServiceTargetVirtualMachines)
                            {
                                if (targetVirtualMachine.PrimaryNetworkInterface != null)
                                    targetVirtualMachine.PrimaryNetworkInterface.BackEndAddressPool = backEndAddressPool;

                                Azure.Asm.VirtualMachine asmVirtualMachine = (Azure.Asm.VirtualMachine)targetVirtualMachine.Source;
                                foreach (XmlNode inputendpoint in asmVirtualMachine.ResourceXml.SelectNodes("//ConfigurationSets/ConfigurationSet/InputEndpoints/InputEndpoint"))
                                {
                                    if (inputendpoint.SelectSingleNode("LoadBalancedEndpointSetName") == null) // if it's a inbound nat rule
                                    {
                                        Azure.MigrationTarget.InboundNatRule targetInboundNatRule = new Azure.MigrationTarget.InboundNatRule(targetLoadBalancer);
                                        targetInboundNatRule.Name = asmVirtualMachine.RoleName + "-" + inputendpoint.SelectSingleNode("Name").InnerText;
                                        targetInboundNatRule.FrontEndPort = Int32.Parse(inputendpoint.SelectSingleNode("Port").InnerText);
                                        targetInboundNatRule.BackEndPort = Int32.Parse(inputendpoint.SelectSingleNode("LocalPort").InnerText);
                                        targetInboundNatRule.Protocol = inputendpoint.SelectSingleNode("Protocol").InnerText;
                                        targetInboundNatRule.FrontEndIpConfiguration = frontEndIpConfiguration;

                                        if (targetVirtualMachine.PrimaryNetworkInterface != null)
                                            targetVirtualMachine.PrimaryNetworkInterface.InboundNatRules.Add(targetInboundNatRule);
                                    }
                                    else // if it's a load balancing rule
                                    {
                                        XmlNode probenode = inputendpoint.SelectSingleNode("LoadBalancerProbe");

                                        Azure.MigrationTarget.Probe targetProbe = null;
                                        foreach (Azure.MigrationTarget.Probe existingProbe in targetLoadBalancer.Probes)
                                        {
                                            if (existingProbe.Name == inputendpoint.SelectSingleNode("LoadBalancedEndpointSetName").InnerText)
                                            {
                                                targetProbe = existingProbe;
                                                break;
                                            }
                                        }

                                        if (targetProbe == null)
                                        {
                                            targetProbe = new Azure.MigrationTarget.Probe();
                                            targetProbe.Name = inputendpoint.SelectSingleNode("LoadBalancedEndpointSetName").InnerText;
                                            targetProbe.Port = Int32.Parse(probenode.SelectSingleNode("Port").InnerText);
                                            targetProbe.Protocol = probenode.SelectSingleNode("Protocol").InnerText;
                                            targetLoadBalancer.Probes.Add(targetProbe);
                                        }

                                        Azure.MigrationTarget.LoadBalancingRule targetLoadBalancingRule = null;
                                        foreach (Azure.MigrationTarget.LoadBalancingRule existingLoadBalancingRule in targetLoadBalancer.LoadBalancingRules)
                                        {
                                            if (existingLoadBalancingRule.Name == inputendpoint.SelectSingleNode("LoadBalancedEndpointSetName").InnerText)
                                            {
                                                targetLoadBalancingRule = existingLoadBalancingRule;
                                                break;
                                            }
                                        }

                                        if (targetLoadBalancingRule == null)
                                        {
                                            targetLoadBalancingRule = new Azure.MigrationTarget.LoadBalancingRule(targetLoadBalancer);
                                            targetLoadBalancingRule.Name = inputendpoint.SelectSingleNode("LoadBalancedEndpointSetName").InnerText;
                                            targetLoadBalancingRule.FrontEndIpConfiguration = frontEndIpConfiguration;
                                            targetLoadBalancingRule.BackEndAddressPool = targetLoadBalancer.BackEndAddressPools[0];
                                            targetLoadBalancingRule.Probe = targetProbe;
                                            targetLoadBalancingRule.FrontEndPort = Int32.Parse(inputendpoint.SelectSingleNode("Port").InnerText);
                                            targetLoadBalancingRule.BackEndPort = Int32.Parse(inputendpoint.SelectSingleNode("LocalPort").InnerText);
                                            targetLoadBalancingRule.Protocol = inputendpoint.SelectSingleNode("Protocol").InnerText;
                                        }
                                    }
                                }
                            }

                        }

                        subscriptionNodeASM.Expand();
                        treeSourceASM.Enabled = true;
                    }

                }
                catch (Exception exc)
                {
                    UnhandledExceptionDialog exceptionDialog = new UnhandledExceptionDialog(this.LogProvider, exc);
                    exceptionDialog.ShowDialog();
                }
            }
        }

        private async Task BindArmResources()
        {
            if (!_IsArmLoaded)
            {
                _IsArmLoaded = true;
                treeSourceASM.Nodes.Clear();

                try
                {
                    _ArmTargetStorageAccounts = new List<Azure.MigrationTarget.StorageAccount>();
                    _ArmTargetVirtualNetworks = new List<Azure.MigrationTarget.VirtualNetwork>();
                    _ArmTargetNetworkSecurityGroups = new List<Azure.MigrationTarget.NetworkSecurityGroup>();
                    _ArmTargetManagedDisks = new List<Azure.MigrationTarget.Disk>();
                    _ArmTargetVirtualMachines = new List<Azure.MigrationTarget.VirtualMachine>();
                    _ArmTargetLoadBalancers = new List<Azure.MigrationTarget.LoadBalancer>();
                    _ArmTargetAvailabilitySets = new List<Azure.MigrationTarget.AvailabilitySet>();
                    _ArmTargetPublicIPs = new List<Azure.MigrationTarget.PublicIp>();

                    if (_AzureContextSourceASM != null && _AzureContextSourceASM.AzureSubscription != null)
                    {
                        TreeNode subscriptionNodeARM = new TreeNode(_AzureContextSourceASM.AzureSubscription.Name);
                        subscriptionNodeARM.ImageKey = "Subscription";
                        subscriptionNodeARM.SelectedImageKey = "Subscription";
                        treeSourceARM.Nodes.Add(subscriptionNodeARM);
                        subscriptionNodeARM.Expand();

                        List<Task> armNetworkSecurityGroupTasks = new List<Task>();
                        foreach (Azure.Arm.ResourceGroup armResourceGroup in await _AzureContextSourceASM.AzureRetriever.GetAzureARMResourceGroups())
                        {
                            TreeNode tnResourceGroup = GetResourceGroupTreeNode(subscriptionNodeARM, armResourceGroup);

                            Task armNetworkSecurityGroupTask = LoadARMNetworkSecurityGroups(tnResourceGroup, armResourceGroup);
                            armNetworkSecurityGroupTasks.Add(armNetworkSecurityGroupTask);
                        }
                        await Task.WhenAll(armNetworkSecurityGroupTasks.ToArray());

                        List<Task> armPublicIPTasks = new List<Task>();
                        foreach (Azure.Arm.ResourceGroup armResourceGroup in await _AzureContextSourceASM.AzureRetriever.GetAzureARMResourceGroups())
                        {
                            TreeNode tnResourceGroup = GetResourceGroupTreeNode(subscriptionNodeARM, armResourceGroup);

                            Task armPublicIPTask = LoadARMPublicIPs(tnResourceGroup, armResourceGroup);
                            armPublicIPTasks.Add(armPublicIPTask);
                        }
                        await Task.WhenAll(armPublicIPTasks.ToArray());


                        List<Task> armVirtualNetworkTasks = new List<Task>();
                        foreach (Azure.Arm.ResourceGroup armResourceGroup in await _AzureContextSourceASM.AzureRetriever.GetAzureARMResourceGroups())
                        {
                            TreeNode tnResourceGroup = GetResourceGroupTreeNode(subscriptionNodeARM, armResourceGroup);

                            Task armVirtualNetworkTask = LoadARMVirtualNetworks(tnResourceGroup, armResourceGroup);
                            armVirtualNetworkTasks.Add(armVirtualNetworkTask);
                        }
                        await Task.WhenAll(armVirtualNetworkTasks.ToArray());

                        List<Task> armStorageAccountTasks = new List<Task>();
                        foreach (Azure.Arm.ResourceGroup armResourceGroup in await _AzureContextSourceASM.AzureRetriever.GetAzureARMResourceGroups())
                        {
                            TreeNode tnResourceGroup = GetResourceGroupTreeNode(subscriptionNodeARM, armResourceGroup);

                            Task armStorageAccountTask = LoadARMStorageAccounts(tnResourceGroup, armResourceGroup);
                            armStorageAccountTasks.Add(armStorageAccountTask);
                        }
                        await Task.WhenAll(armStorageAccountTasks.ToArray());

                        try
                        {
                            List<Task> armManagedDiskTasks = new List<Task>();
                            foreach (Azure.Arm.ResourceGroup armResourceGroup in await _AzureContextSourceASM.AzureRetriever.GetAzureARMResourceGroups())
                            {
                                TreeNode tnResourceGroup = GetResourceGroupTreeNode(subscriptionNodeARM, armResourceGroup);

                                Task armManagedDiskTask = LoadARMManagedDisks(tnResourceGroup, armResourceGroup);
                                armManagedDiskTasks.Add(armManagedDiskTask);
                            }
                            await Task.WhenAll(armManagedDiskTasks.ToArray());
                        }
                        catch (Exception exc) { }

                        List<Task> armAvailabilitySetTasks = new List<Task>();
                        foreach (Azure.Arm.ResourceGroup armResourceGroup in await _AzureContextSourceASM.AzureRetriever.GetAzureARMResourceGroups())
                        {
                            TreeNode tnResourceGroup = GetResourceGroupTreeNode(subscriptionNodeARM, armResourceGroup);

                            Task armAvailabilitySetTask = LoadARMAvailabilitySets(tnResourceGroup, armResourceGroup);
                            armAvailabilitySetTasks.Add(armAvailabilitySetTask);
                        }
                        await Task.WhenAll(armAvailabilitySetTasks.ToArray());

                        List<Task> armVirtualMachineTasks = new List<Task>();
                        foreach (Azure.Arm.ResourceGroup armResourceGroup in await _AzureContextSourceASM.AzureRetriever.GetAzureARMResourceGroups())
                        {
                            TreeNode tnResourceGroup = GetResourceGroupTreeNode(subscriptionNodeARM, armResourceGroup);

                            Task armVirtualMachineTask = LoadARMVirtualMachines(tnResourceGroup, armResourceGroup);
                            armVirtualMachineTasks.Add(armVirtualMachineTask);
                        }
                        await Task.WhenAll(armVirtualMachineTasks.ToArray());

                        List<Task> armLoadBalancerTasks = new List<Task>();
                        foreach (Azure.Arm.ResourceGroup armResourceGroup in await _AzureContextSourceASM.AzureRetriever.GetAzureARMResourceGroups())
                        {
                            TreeNode tnResourceGroup = GetResourceGroupTreeNode(subscriptionNodeARM, armResourceGroup);

                            Task armLoadBalancerTask = LoadARMLoadBalancers(tnResourceGroup, armResourceGroup);
                            armLoadBalancerTasks.Add(armLoadBalancerTask);
                        }
                        await Task.WhenAll(armLoadBalancerTasks.ToArray());

                        subscriptionNodeARM.Expand();
                        treeSourceARM.Enabled = true;
                    }
                }
                catch (Exception exc)
                {
                    UnhandledExceptionDialog exceptionDialog = new UnhandledExceptionDialog(this.LogProvider, exc);
                    exceptionDialog.ShowDialog();
                }
            }
        }


    }
}
