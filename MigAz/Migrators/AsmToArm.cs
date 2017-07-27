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
using System.Net;

namespace MigAz.Migrators
{
    public partial class AsmToArm : Azure.UserControls.IMigratorUserControl
    {
        #region Variables

        private UISaveSelectionProvider _saveSelectionProvider;
        private TreeNode _SourceAsmNode;
        private List<TreeNode> _SelectedNodes = new List<TreeNode>();
        private AzureTelemetryProvider _telemetryProvider;
        private AppSettingsProvider _appSettingsProvider;
        private AzureContext _AzureContextSourceASM;
        private AzureContext _AzureContextTargetARM;
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
            string currentVersion = "2.2.12.0";
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
                                if (networkInterface.NetworkSecurityGroup != null)
                                {
                                    foreach (TreeNode treeNode in treeSourceARM.Nodes.Find(networkInterface.NetworkSecurityGroup.Name, true))
                                    {
                                        if ((treeNode.Tag != null) && (treeNode.Tag.GetType() == typeof(Azure.MigrationTarget.NetworkSecurityGroup)))
                                        {
                                            if (!treeNode.Checked)
                                                treeNode.Checked = true;
                                        }
                                    }
                                }

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
                    await _AzureContextSourceASM.AzureRetriever.BindAsmResources();

                    if (_AzureContextSourceASM != null && _AzureContextSourceASM.AzureSubscription != null)
                    {
                        TreeNode subscriptionNodeASM = new TreeNode(_AzureContextSourceASM.AzureSubscription.Name);
                        treeSourceASM.Nodes.Add(subscriptionNodeASM);
                        subscriptionNodeASM.Expand();

                        foreach (Azure.MigrationTarget.NetworkSecurityGroup targetNetworkSecurityGroup in _AzureContextSourceASM.AzureRetriever.AsmTargetNetworkSecurityGroups)
                        {
                            TreeNode parentNode = GetDataCenterTreeViewNode(subscriptionNodeASM, targetNetworkSecurityGroup.SourceNetworkSecurityGroup.Location, "Network Security Groups");

                            TreeNode tnNetworkSecurityGroup = new TreeNode(targetNetworkSecurityGroup.SourceName);
                            tnNetworkSecurityGroup.Name = targetNetworkSecurityGroup.SourceName;
                            tnNetworkSecurityGroup.Tag = targetNetworkSecurityGroup;
                            parentNode.Nodes.Add(tnNetworkSecurityGroup);
                            parentNode.Expand();
                        }

                        foreach (Azure.MigrationTarget.VirtualNetwork targetVirtualNetwork in _AzureContextSourceASM.AzureRetriever.AsmTargetVirtualNetworks)
                        {
                            TreeNode parentNode = GetDataCenterTreeViewNode(subscriptionNodeASM, targetVirtualNetwork.SourceVirtualNetwork.Location, "Virtual Networks");

                            TreeNode tnVirtualNetwork = new TreeNode(targetVirtualNetwork.SourceName);
                            tnVirtualNetwork.Name = targetVirtualNetwork.SourceName;
                            tnVirtualNetwork.Text = targetVirtualNetwork.SourceName;
                            tnVirtualNetwork.Tag = targetVirtualNetwork;
                            parentNode.Nodes.Add(tnVirtualNetwork);
                            parentNode.Expand();
                        }

                        foreach (Azure.MigrationTarget.StorageAccount targetStorageAccount in _AzureContextSourceASM.AzureRetriever.AsmTargetStorageAccounts)
                        {
                            TreeNode parentNode = GetDataCenterTreeViewNode(subscriptionNodeASM, targetStorageAccount.SourceAccount.PrimaryLocation, "Storage Accounts");

                            TreeNode tnStorageAccount = new TreeNode(targetStorageAccount.SourceName);
                            tnStorageAccount.Name = targetStorageAccount.SourceName;
                            tnStorageAccount.Tag = targetStorageAccount;
                            parentNode.Nodes.Add(tnStorageAccount);
                            parentNode.Expand();
                        }

                        foreach (Azure.MigrationTarget.VirtualMachine targetVirtualMachine in _AzureContextSourceASM.AzureRetriever.AsmTargetVirtualMachines)
                        {
                            TreeNode parentNode = GetDataCenterTreeViewNode(subscriptionNodeASM, targetVirtualMachine.Source.Location, "Cloud Services");
                            TreeNode[] cloudServiceNodeSearch = parentNode.Nodes.Find(targetVirtualMachine.TargetAvailabilitySet.TargetName, false);
                            TreeNode cloudServiceNode = null;
                            if (cloudServiceNodeSearch.Count() == 1)
                            {
                                cloudServiceNode = cloudServiceNodeSearch[0];
                            }

                            cloudServiceNode = new TreeNode(targetVirtualMachine.TargetAvailabilitySet.TargetName);
                            cloudServiceNode.Name = targetVirtualMachine.TargetAvailabilitySet.TargetName;
                            cloudServiceNode.Tag = targetVirtualMachine.TargetAvailabilitySet;
                            parentNode.Nodes.Add(cloudServiceNode);
                            parentNode.Expand();

                            TreeNode virtualMachineNode = new TreeNode(targetVirtualMachine.SourceName);
                            virtualMachineNode.Name = targetVirtualMachine.SourceName;
                            virtualMachineNode.Tag = targetVirtualMachine;
                            cloudServiceNode.Nodes.Add(virtualMachineNode);
                            cloudServiceNode.Expand();

                            foreach (Azure.MigrationTarget.NetworkInterface targetNetworkInterface in targetVirtualMachine.NetworkInterfaces)
                            {
                                if (targetNetworkInterface.BackEndAddressPool != null && targetNetworkInterface.BackEndAddressPool.LoadBalancer != null)
                                {
                                    TreeNode loadBalancerNode = new TreeNode(targetNetworkInterface.BackEndAddressPool.LoadBalancer.SourceName);
                                    loadBalancerNode.Name = targetNetworkInterface.BackEndAddressPool.LoadBalancer.SourceName;
                                    loadBalancerNode.Tag = targetNetworkInterface.BackEndAddressPool.LoadBalancer;
                                    cloudServiceNode.Nodes.Add(loadBalancerNode);
                                    cloudServiceNode.Expand();

                                    foreach (Azure.MigrationTarget.FrontEndIpConfiguration frontEnd in targetNetworkInterface.BackEndAddressPool.LoadBalancer.FrontEndIpConfigurations)
                                    {
                                        if (frontEnd.PublicIp != null) // if external load balancer
                                        {
                                            TreeNode publicIPAddressNode = new TreeNode(frontEnd.PublicIp.SourceName);
                                            publicIPAddressNode.Name = frontEnd.PublicIp.SourceName;
                                            publicIPAddressNode.Tag = frontEnd.PublicIp;
                                            cloudServiceNode.Nodes.Add(publicIPAddressNode);
                                            cloudServiceNode.Expand();
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
                    if (exc.GetType() == typeof(System.Net.WebException))
                    {
                        System.Net.WebException webException = (System.Net.WebException) exc;
                        if (webException.Response != null)
                        {
                            HttpWebResponse exceptionResponse = (HttpWebResponse)webException.Response;
                            if (exceptionResponse.StatusCode == HttpStatusCode.Forbidden)
                            {
                                ASM403ForbiddenExceptionDialog forbiddenDialog = new ASM403ForbiddenExceptionDialog(this.LogProvider, exc);
                                return;
                            }
                        }
                    }

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
                    await _AzureContextSourceASM.AzureRetriever.BindArmResources();

                    if (_AzureContextSourceASM != null && _AzureContextSourceASM.AzureSubscription != null)
                    {
                        TreeNode subscriptionNodeARM = new TreeNode(_AzureContextSourceASM.AzureSubscription.Name);
                        subscriptionNodeARM.ImageKey = "Subscription";
                        subscriptionNodeARM.SelectedImageKey = "Subscription";
                        treeSourceARM.Nodes.Add(subscriptionNodeARM);
                        subscriptionNodeARM.Expand();

                        foreach (Azure.MigrationTarget.NetworkSecurityGroup targetNetworkSecurityGroup in _AzureContextSourceASM.AzureRetriever.ArmTargetNetworkSecurityGroups)
                        {
                            TreeNode networkSecurityGroupParentNode = GetResourceGroupTreeNode(subscriptionNodeARM, ((Azure.Arm.NetworkSecurityGroup)targetNetworkSecurityGroup.SourceNetworkSecurityGroup).ResourceGroup);

                            TreeNode tnNetworkSecurityGroup = new TreeNode(targetNetworkSecurityGroup.SourceName);
                            tnNetworkSecurityGroup.Name = targetNetworkSecurityGroup.SourceName;
                            tnNetworkSecurityGroup.Tag = targetNetworkSecurityGroup;
                            tnNetworkSecurityGroup.ImageKey = "NetworkSecurityGroup";
                            tnNetworkSecurityGroup.SelectedImageKey = "NetworkSecurityGroup";
                            networkSecurityGroupParentNode.Nodes.Add(tnNetworkSecurityGroup);
                        }

                        foreach (Azure.MigrationTarget.PublicIp targetPublicIP in _AzureContextSourceASM.AzureRetriever.ArmTargetPublicIPs)
                        {
                            TreeNode publicIpParentNode = GetResourceGroupTreeNode(subscriptionNodeARM, ((Azure.Arm.PublicIP)targetPublicIP.Source).ResourceGroup); ;

                            TreeNode tnPublicIP = new TreeNode(targetPublicIP.SourceName);
                            tnPublicIP.Name = targetPublicIP.SourceName;
                            tnPublicIP.Tag = targetPublicIP;
                            tnPublicIP.ImageKey = "PublicIp";
                            tnPublicIP.SelectedImageKey = "PublicIp";
                            publicIpParentNode.Nodes.Add(tnPublicIP);
                        }

                        foreach (Azure.MigrationTarget.VirtualNetwork targetVirtualNetwork in _AzureContextSourceASM.AzureRetriever.ArmTargetVirtualNetworks)
                        {
                            TreeNode virtualNetworkParentNode = GetResourceGroupTreeNode(subscriptionNodeARM, ((Azure.Arm.VirtualNetwork)targetVirtualNetwork.SourceVirtualNetwork).ResourceGroup);

                            TreeNode tnVirtualNetwork = new TreeNode(targetVirtualNetwork.SourceName);
                            tnVirtualNetwork.Name = targetVirtualNetwork.SourceName;
                            tnVirtualNetwork.Tag = targetVirtualNetwork;
                            tnVirtualNetwork.ImageKey = "VirtualNetwork";
                            tnVirtualNetwork.SelectedImageKey = "VirtualNetwork";
                            virtualNetworkParentNode.Nodes.Add(tnVirtualNetwork);
                        }

                        foreach (Azure.MigrationTarget.StorageAccount targetStorageAccount in _AzureContextSourceASM.AzureRetriever.ArmTargetStorageAccounts)
                        {
                            TreeNode storageAccountParentNode = GetResourceGroupTreeNode(subscriptionNodeARM, ((Azure.Arm.StorageAccount)targetStorageAccount.SourceAccount).ResourceGroup);

                            TreeNode tnStorageAccount = new TreeNode(targetStorageAccount.SourceName);
                            tnStorageAccount.Name = targetStorageAccount.SourceName;
                            tnStorageAccount.Tag = targetStorageAccount;
                            tnStorageAccount.ImageKey = "StorageAccount";
                            tnStorageAccount.SelectedImageKey = "StorageAccount";
                            storageAccountParentNode.Nodes.Add(tnStorageAccount);
                        }


                        //foreach (Azure.MigrationTarget.ManagedDisk targetManagedDisk in _AzureContextSourceASM.AzureRetriever.ArmTargetManagedDisks)
                        //{
                        //}

                        foreach (Azure.MigrationTarget.AvailabilitySet targetAvailabilitySet in _AzureContextSourceASM.AzureRetriever.ArmTargetAvailabilitySets)
                        {
                            TreeNode availabilitySetParentNode = GetResourceGroupTreeNode(subscriptionNodeARM, ((Azure.Arm.AvailabilitySet)targetAvailabilitySet.SourceAvailabilitySet).ResourceGroup);

                            TreeNode tnAvailabilitySet = new TreeNode(targetAvailabilitySet.SourceName);
                            tnAvailabilitySet.Name = targetAvailabilitySet.SourceName;
                            tnAvailabilitySet.Tag = targetAvailabilitySet;
                            tnAvailabilitySet.ImageKey = "AvailabilitySet";
                            tnAvailabilitySet.SelectedImageKey = "AvailabilitySet";
                            availabilitySetParentNode.Nodes.Add(tnAvailabilitySet);
                        }

                        foreach (Azure.MigrationTarget.VirtualMachine targetVirtualMachine in _AzureContextSourceASM.AzureRetriever.ArmTargetVirtualMachines)
                        {
                            TreeNode tnResourceGroup = GetResourceGroupTreeNode(subscriptionNodeARM, ((Azure.Arm.VirtualMachine)targetVirtualMachine.Source).ResourceGroup);

                            TreeNode virtualMachineParentNode = tnResourceGroup;

                            if (targetVirtualMachine.TargetAvailabilitySet != null)
                            {
                                TreeNode tnAvailabilitySet = GetAvailabilitySetTreeNode(virtualMachineParentNode, targetVirtualMachine.TargetAvailabilitySet);
                                virtualMachineParentNode = tnAvailabilitySet;
                            }

                            TreeNode tnVirtualMachine = new TreeNode(targetVirtualMachine.SourceName);
                            tnVirtualMachine.Name = targetVirtualMachine.SourceName;
                            tnVirtualMachine.Tag = targetVirtualMachine;
                            tnVirtualMachine.ImageKey = "VirtualMachine";
                            tnVirtualMachine.SelectedImageKey = "VirtualMachine";
                            virtualMachineParentNode.Nodes.Add(tnVirtualMachine);
                        }

                        foreach (Azure.MigrationTarget.LoadBalancer targetLoadBalancer in _AzureContextSourceASM.AzureRetriever.ArmTargetLoadBalancers)
                        {
                            TreeNode networkSecurityGroupParentNode = GetResourceGroupTreeNode(subscriptionNodeARM, ((Azure.Arm.LoadBalancer)targetLoadBalancer.Source).ResourceGroup);

                            TreeNode tnNetworkSecurityGroup = new TreeNode(targetLoadBalancer.SourceName);
                            tnNetworkSecurityGroup.Name = targetLoadBalancer.SourceName;
                            tnNetworkSecurityGroup.Tag = targetLoadBalancer;
                            tnNetworkSecurityGroup.ImageKey = "LoadBalancer";
                            tnNetworkSecurityGroup.SelectedImageKey = "LoadBalancer";
                            networkSecurityGroupParentNode.Nodes.Add(tnNetworkSecurityGroup);
                        }

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
