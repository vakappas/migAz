using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MigAz.Core.Interface;

namespace MigAz.Azure.UserControls
{
    public partial class TargetTreeView : UserControl
    {
        private Azure.MigrationTarget.ResourceGroup _TargetResourceGroup = new MigrationTarget.ResourceGroup();
        private TreeNode _EventSourceNode;
        private PropertyPanel _PropertyPanel;

        public TargetTreeView()
        {
            InitializeComponent();
        }

        public ILogProvider LogProvider
        {
            get; set;
        }

        public IStatusProvider StatusProvider
        {
            get; set;
        }

        public ImageList ImageList
        {
            get { return this.treeTargetARM.ImageList; }
            set { this.treeTargetARM.ImageList = value; }
        }

        public PropertyPanel PropertyPanel
        {
            get { return _PropertyPanel; }
            set
            {
                _PropertyPanel = value;

                if (_PropertyPanel != null)
                    if (_PropertyPanel.TargetTreeView != this)
                        _PropertyPanel.TargetTreeView = this;
            }
        }

        public TreeNodeCollection Nodes
        {
            get { return this.treeTargetARM.Nodes; }
        }

        public TreeNode EventSourceNode
        {
            get { return _EventSourceNode; }
        }

        public TreeNode SelectedNode
        {
            get { return this.treeTargetARM.SelectedNode; }
            set { this.treeTargetARM.SelectedNode = value; }
        }

        public TreeNode ResourceGroupNode
        {
            get
            {
                if (this.Nodes.Count == 1)
                    return this.Nodes[0];
                else
                    return null;
            }
        }

        public ISettingsProvider SettingsProvider { get; set; }

        private void GetExportArtifactsRecursive(TreeNode parentTreeNode, ref ExportArtifacts exportArtifacts)
        {
            foreach (TreeNode selectedNode in parentTreeNode.Nodes)
            {
                Type tagType = selectedNode.Tag.GetType();

                if (tagType == typeof(Azure.MigrationTarget.VirtualNetwork))
                {
                    exportArtifacts.VirtualNetworks.Add((Azure.MigrationTarget.VirtualNetwork)selectedNode.Tag);
                }
                else if (tagType == typeof(Azure.MigrationTarget.StorageAccount))
                {
                    exportArtifacts.StorageAccounts.Add((Azure.MigrationTarget.StorageAccount)selectedNode.Tag);
                }
                else if (tagType == typeof(Azure.MigrationTarget.NetworkSecurityGroup))
                {
                    exportArtifacts.NetworkSecurityGroups.Add((Azure.MigrationTarget.NetworkSecurityGroup)selectedNode.Tag);
                }
                else if (tagType == typeof(Azure.MigrationTarget.VirtualMachine))
                {
                    exportArtifacts.VirtualMachines.Add((Azure.MigrationTarget.VirtualMachine)selectedNode.Tag);
                }
                else if (tagType == typeof(Azure.MigrationTarget.LoadBalancer))
                {
                    exportArtifacts.LoadBalancers.Add((Azure.MigrationTarget.LoadBalancer)selectedNode.Tag);
                }
                else if (tagType == typeof(Azure.MigrationTarget.PublicIp))
                {
                    exportArtifacts.PublicIPs.Add((Azure.MigrationTarget.PublicIp)selectedNode.Tag);
                }
            }

            foreach (TreeNode treeNode in parentTreeNode.Nodes)
            {
                GetExportArtifactsRecursive(treeNode, ref exportArtifacts);
            }
        }

        public ExportArtifacts GetExportArtifacts()
        {
            ExportArtifacts exportArtifacts = new ExportArtifacts();

            if (this.Nodes.Count == 1 && this.Nodes[0].Tag != null)
                exportArtifacts.ResourceGroup = (MigrationTarget.ResourceGroup)this.Nodes[0].Tag;

            foreach (TreeNode treeNode in treeTargetARM.Nodes)
            {
                GetExportArtifactsRecursive(treeNode, ref exportArtifacts);
            }

            return exportArtifacts;
        }

        private void SeekAlertSourceRecursive(object sourceObject, TreeNodeCollection nodes)
        {
            foreach (TreeNode treeNode in nodes)
            {
                if (treeNode.Tag != null)
                {
                    object nodeObject = null;

                    if (treeNode.Tag.GetType() == typeof(TreeNode))
                    {
                        TreeNode asmTreeNode = (TreeNode)treeNode.Tag;
                        nodeObject = asmTreeNode.Tag;
                    }
                    else
                    {
                        nodeObject = treeNode.Tag;
                    }

                    // Note, this could probably be object compares, but was written this was to get it done.  Possible future change to object compares
                    if (nodeObject != null && sourceObject != null)
                    {
                        if (nodeObject.GetType() == sourceObject.GetType())
                        {
                            if (sourceObject.GetType() == typeof(Azure.MigrationTarget.ResourceGroup))
                                treeTargetARM.SelectedNode = treeNode;
                            else if (sourceObject.GetType() == typeof(Azure.MigrationTarget.VirtualMachine) && sourceObject.ToString() == nodeObject.ToString())
                                treeTargetARM.SelectedNode = treeNode;
                            else if (sourceObject.GetType() == typeof(Azure.MigrationTarget.Disk) && sourceObject.ToString() == nodeObject.ToString())
                                treeTargetARM.SelectedNode = treeNode;
                            else if (sourceObject.GetType() == typeof(Azure.MigrationTarget.NetworkInterface) && sourceObject.ToString() == nodeObject.ToString())
                                treeTargetARM.SelectedNode = treeNode;
                            else if (sourceObject.GetType() == typeof(Azure.MigrationTarget.LoadBalancer) && sourceObject.ToString() == nodeObject.ToString())
                                treeTargetARM.SelectedNode = treeNode;
                        }
                    }
                }
                SeekAlertSourceRecursive(sourceObject, treeNode.Nodes);
            }
        }

        public TreeNode SeekARMChildTreeNode(string name, string text, object tag, bool allowCreated = false)
        {
            return SeekARMChildTreeNode(this.treeTargetARM.Nodes, name, text, tag, allowCreated);
        }

        public TreeNode SeekARMChildTreeNode(TreeNodeCollection nodeCollection, string name, string text, object tag, bool allowCreated = false)
        {
            TreeNode[] childNodeMatch = nodeCollection.Find(name, false);

            foreach (TreeNode matchedNode in childNodeMatch)
            {
                if (matchedNode.Tag != null)
                {
                    if (matchedNode.Tag.GetType() == tag.GetType() && matchedNode.Text == text && matchedNode.Name == name)
                        return matchedNode;
                }
            }

            TreeNode childNode = null;
            if (allowCreated)
            {
                childNode = new TreeNode(text);
                childNode.Name = name;
                childNode.Tag = tag;
                if (tag.GetType() == typeof(Azure.MigrationTarget.ResourceGroup))
                {
                    childNode.ImageKey = "ResourceGroup";
                    childNode.SelectedImageKey = "ResourceGroup";
                }
                else if (tag.GetType() == typeof(Azure.MigrationTarget.StorageAccount))
                {
                    childNode.ImageKey = "StorageAccount";
                    childNode.SelectedImageKey = "StorageAccount";
                }
                else if (tag.GetType() == typeof(Azure.MigrationTarget.AvailabilitySet))
                {
                    childNode.ImageKey = "AvailabilitySet";
                    childNode.SelectedImageKey = "AvailabilitySet";
                }
                else if (tag.GetType() == typeof(Azure.MigrationTarget.VirtualMachine))
                {
                    childNode.ImageKey = "VirtualMachine";
                    childNode.SelectedImageKey = "VirtualMachine";
                }
                else if (tag.GetType() == typeof(Azure.MigrationTarget.VirtualNetwork))
                {
                    childNode.ImageKey = "VirtualNetwork";
                    childNode.SelectedImageKey = "VirtualNetwork";
                }
                else if (tag.GetType() == typeof(Azure.MigrationTarget.Subnet))
                {
                    childNode.ImageKey = "VirtualNetwork";
                    childNode.SelectedImageKey = "VirtualNetwork";
                }
                else if (tag.GetType() == typeof(Azure.MigrationTarget.NetworkSecurityGroup))
                {
                    childNode.ImageKey = "NetworkSecurityGroup";
                    childNode.SelectedImageKey = "NetworkSecurityGroup";
                }
                else if (tag.GetType() == typeof(Azure.MigrationTarget.Disk))
                {
                    childNode.ImageKey = "Disk";
                    childNode.SelectedImageKey = "Disk";
                }
                else if (tag.GetType() == typeof(Azure.MigrationTarget.NetworkInterface))
                {
                    childNode.ImageKey = "NetworkInterface";
                    childNode.SelectedImageKey = "NetworkInterface";
                }
                else if (tag.GetType() == typeof(Azure.MigrationTarget.LoadBalancer))
                {
                    childNode.ImageKey = "LoadBalancer";
                    childNode.SelectedImageKey = "LoadBalancer";
                }
                else if (tag.GetType() == typeof(Azure.MigrationTarget.PublicIp))
                {
                    childNode.ImageKey = "PublicIp";
                    childNode.SelectedImageKey = "PublicIp";
                }
                else
                    throw new ArgumentException("Unknown node tag type: " + tag.GetType().ToString());

                nodeCollection.Add(childNode);
                childNode.ExpandAll();
                return childNode;
            }
            return null;
        }

        private TreeNode SeekResourceGroupTreeNode()
        {
            TreeNode targetResourceGroupNode = SeekARMChildTreeNode(treeTargetARM.Nodes, _TargetResourceGroup.ToString(), _TargetResourceGroup.ToString(), _TargetResourceGroup, true);
            return targetResourceGroupNode;
        }

        public async Task<TreeNode> AddMigrationTargetToTargetTree(IMigrationTarget parentNode)
        {
            if (parentNode == null)
                throw new ArgumentNullException("Migration Target cannot be null.");

            TreeNode targetResourceGroupNode = SeekResourceGroupTreeNode();

            if (parentNode.GetType() == typeof(Azure.MigrationTarget.VirtualNetwork))
            {
                Azure.MigrationTarget.VirtualNetwork targetVirtualNetwork = (Azure.MigrationTarget.VirtualNetwork)parentNode;
                TreeNode virtualNetworkNode = SeekARMChildTreeNode(targetResourceGroupNode.Nodes, targetVirtualNetwork.SourceName, targetVirtualNetwork.ToString(), targetVirtualNetwork, true);

                foreach (Azure.MigrationTarget.Subnet targetSubnet in targetVirtualNetwork.TargetSubnets)
                {
                    TreeNode subnetNode = SeekARMChildTreeNode(virtualNetworkNode.Nodes, targetVirtualNetwork.ToString(), targetSubnet.ToString(), targetSubnet, true);
                }

                targetResourceGroupNode.ExpandAll();
                return virtualNetworkNode;
            }
            else if (parentNode.GetType() == typeof(Azure.MigrationTarget.StorageAccount))
            {
                Azure.MigrationTarget.StorageAccount targetStorageAccount = (Azure.MigrationTarget.StorageAccount)parentNode;

                TreeNode storageAccountNode = SeekARMChildTreeNode(targetResourceGroupNode.Nodes, targetStorageAccount.SourceName, targetStorageAccount.ToString(), targetStorageAccount, true);

                targetResourceGroupNode.ExpandAll();
                return storageAccountNode;
            }
            else if (parentNode.GetType() == typeof(Azure.MigrationTarget.NetworkSecurityGroup))
            {
                Azure.MigrationTarget.NetworkSecurityGroup targetNetworkSecurityGroup = (Azure.MigrationTarget.NetworkSecurityGroup)parentNode;
                TreeNode networkSecurityGroupNode = SeekARMChildTreeNode(targetResourceGroupNode.Nodes, targetNetworkSecurityGroup.SourceName, targetNetworkSecurityGroup.ToString(), targetNetworkSecurityGroup, true);

                targetResourceGroupNode.ExpandAll();
                return networkSecurityGroupNode;
            }
            else if (parentNode.GetType() == typeof(Azure.MigrationTarget.LoadBalancer))
            {
                Azure.MigrationTarget.LoadBalancer targetLoadBalancer = (Azure.MigrationTarget.LoadBalancer)parentNode;
                TreeNode targetLoadBalancerNode = SeekARMChildTreeNode(targetResourceGroupNode.Nodes, targetLoadBalancer.SourceName, targetLoadBalancer.ToString(), targetLoadBalancer, true);

                targetResourceGroupNode.ExpandAll();
                return targetLoadBalancerNode;
            }
            else if (parentNode.GetType() == typeof(Azure.MigrationTarget.PublicIp))
            {
                Azure.MigrationTarget.PublicIp targetPublicIp = (Azure.MigrationTarget.PublicIp)parentNode;
                TreeNode targetPublicIpNode = SeekARMChildTreeNode(targetResourceGroupNode.Nodes, targetPublicIp.SourceName, targetPublicIp.ToString(), targetPublicIp, true);

                targetResourceGroupNode.ExpandAll();
                return targetPublicIpNode;
            }
            else if (parentNode.GetType() == typeof(Azure.MigrationTarget.VirtualMachine))
            {
                Azure.MigrationTarget.VirtualMachine targetVirtualMachine = (Azure.MigrationTarget.VirtualMachine)parentNode;

                TreeNode virtualMachineParentNode = targetResourceGroupNode;
                TreeNode targetAvailabilitySetNode = null;

                // https://docs.microsoft.com/en-us/azure/virtual-machines/windows/manage-availability
                if (targetVirtualMachine.TargetAvailabilitySet != null)
                {
                    targetAvailabilitySetNode = GetTargetAvailabilitySetNode(targetResourceGroupNode, targetVirtualMachine.TargetAvailabilitySet);
                    virtualMachineParentNode = targetAvailabilitySetNode;
                }

                TreeNode virtualMachineNode = SeekARMChildTreeNode(virtualMachineParentNode.Nodes, targetVirtualMachine.SourceName, targetVirtualMachine.ToString(), targetVirtualMachine, true);

                foreach (Azure.MigrationTarget.Disk targetDisk in targetVirtualMachine.DataDisks)
                {
                    TreeNode dataDiskNode = SeekARMChildTreeNode(virtualMachineNode.Nodes, targetDisk.ToString(), targetDisk.ToString(), targetDisk, true);
                }

                foreach (Azure.MigrationTarget.NetworkInterface targetNetworkInterface in targetVirtualMachine.NetworkInterfaces)
                {
                    TreeNode networkInterfaceNode = SeekARMChildTreeNode(virtualMachineNode.Nodes, targetNetworkInterface.ToString(), targetNetworkInterface.ToString(), targetNetworkInterface, true);
                }

                targetResourceGroupNode.ExpandAll();
                return virtualMachineNode;
            }
            else
                throw new Exception("Unhandled Node Type in AddMigrationTargetToTargetTree: " + parentNode.GetType());

        }

        private TreeNode GetTargetAvailabilitySetNode(TreeNode subscriptionNode, Azure.MigrationTarget.AvailabilitySet targetAvailabilitySet)
        {
            foreach (TreeNode treeNode in subscriptionNode.Nodes)
            {
                if (treeNode.Tag != null)
                {
                    if (treeNode.Tag.GetType() == typeof(Azure.MigrationTarget.AvailabilitySet) && treeNode.Text == targetAvailabilitySet.ToString())
                        return treeNode;
                }
            }

            TreeNode tnAvailabilitySet = new TreeNode(targetAvailabilitySet.ToString());
            tnAvailabilitySet.Text = targetAvailabilitySet.ToString();
            tnAvailabilitySet.Tag = targetAvailabilitySet;
            tnAvailabilitySet.ImageKey = "AvailabilitySet";
            tnAvailabilitySet.SelectedImageKey = "AvailabilitySet";

            subscriptionNode.Nodes.Add(tnAvailabilitySet);
            tnAvailabilitySet.Expand();
            return tnAvailabilitySet;
        }

        public async Task RemoveASMNodeFromARMTree(IMigrationTarget migrationTarget)
        {

            TreeNode targetResourceGroupNode = SeekResourceGroupTreeNode();
            if (targetResourceGroupNode != null)
            {
                TreeNode[] matchingNodes = targetResourceGroupNode.Nodes.Find(migrationTarget.SourceName, true);
                foreach (TreeNode matchingNode in matchingNodes)
                {
                    if (matchingNode.Tag.GetType() == migrationTarget.GetType())
                        await RemoveTreeNodeCascadeUp(matchingNode);
                    else if (matchingNode.Tag.GetType() == typeof(Azure.MigrationTarget.StorageAccount))
                    {
                        if (migrationTarget.GetType() == typeof(Azure.Asm.StorageAccount))
                        {
                            await RemoveTreeNodeCascadeUp(matchingNode);
                        }
                        else if (migrationTarget.GetType() == typeof(Azure.Arm.StorageAccount))
                        {
                            await RemoveTreeNodeCascadeUp(matchingNode);
                        }
                    }
                    else if (matchingNode.Tag.GetType() == typeof(Azure.MigrationTarget.VirtualMachine))
                    {
                        if (migrationTarget.GetType() == typeof(Azure.Asm.VirtualMachine))
                        {
                            await RemoveTreeNodeCascadeUp(matchingNode);
                        }
                        else if (migrationTarget.GetType() == typeof(Azure.Arm.VirtualMachine))
                        {
                            await RemoveTreeNodeCascadeUp(matchingNode);
                        }
                    }
                    else if (matchingNode.Tag.GetType() == typeof(Azure.MigrationTarget.NetworkSecurityGroup))
                    {
                        if (migrationTarget.GetType() == typeof(Azure.Asm.NetworkSecurityGroup))
                        {
                            await RemoveTreeNodeCascadeUp(matchingNode);
                        }
                        else if (migrationTarget.GetType() == typeof(Azure.Arm.NetworkSecurityGroup))
                        {
                            await RemoveTreeNodeCascadeUp(matchingNode);
                        }
                    }
                    else if (matchingNode.Tag.GetType() == typeof(Azure.MigrationTarget.VirtualNetwork))
                    {
                        if (migrationTarget.GetType() == typeof(Azure.Asm.VirtualNetwork))
                        {
                            await RemoveTreeNodeCascadeUp(matchingNode);
                        }
                        else if (migrationTarget.GetType() == typeof(Azure.Arm.VirtualNetwork))
                        {
                            await RemoveTreeNodeCascadeUp(matchingNode);
                        }
                    }
                    else if (matchingNode.Tag.GetType() == typeof(TreeNode))
                    {
                        TreeNode childTreeNode = (TreeNode)matchingNode.Tag;
                        if (migrationTarget.GetType() == childTreeNode.Tag.GetType())
                            await RemoveTreeNodeCascadeUp(matchingNode);
                    }
                }
            }
        }

        private async Task RemoveTreeNodeCascadeUp(TreeNode treeNode)
        {
            TreeNode parentNode = treeNode.Parent;
            treeNode.Remove();
            await RemoveParentWhileNoChildren(parentNode);
        }

        private async Task RemoveParentWhileNoChildren(TreeNode treeNode)
        {
            if (treeNode != null)
            {
                if (treeNode.Nodes.Count == 0)
                {
                    TreeNode parentNode = treeNode.Parent;
                    treeNode.Remove();
                    await RemoveParentWhileNoChildren(parentNode);
                }
            }
        }

        public void SeekAlertSource(object sourceObject)
        {
            SeekAlertSourceRecursive(sourceObject, treeTargetARM.Nodes);
        }


        private async void TargetTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (this.LogProvider != null)
                LogProvider.WriteLog("treeARM_AfterSelect", "Start");

            _EventSourceNode = e.Node;
            await _PropertyPanel.Bind(e.Node);
            _EventSourceNode = null;

            if (this.LogProvider != null)
                LogProvider.WriteLog("treeARM_AfterSelect", "End");

            if (this.StatusProvider != null)
                StatusProvider.UpdateStatus("Ready");
        }

        private void TargetTreeView_Resize(object sender, EventArgs e)
        {
            treeTargetARM.Width = this.Width - 10;
            treeTargetARM.Height = this.Height - 10;
        }

        public List<Azure.MigrationTarget.VirtualNetwork> GetVirtualNetworksInMigration()
        {
            List<Azure.MigrationTarget.VirtualNetwork> _TargetVirtualNetworks = new List<MigrationTarget.VirtualNetwork>();

            TreeNode targetResourceGroupNode = this.ResourceGroupNode;

            if (targetResourceGroupNode != null)
            {
                foreach (TreeNode treeNode in targetResourceGroupNode.Nodes)
                {
                    if (treeNode.Tag != null && treeNode.Tag.GetType() == typeof(Azure.MigrationTarget.VirtualNetwork))
                    {
                        Azure.MigrationTarget.VirtualNetwork targetVirtualNetwork = (Azure.MigrationTarget.VirtualNetwork)treeNode.Tag;
                        _TargetVirtualNetworks.Add(targetVirtualNetwork);
                    }
                }
            }

            return _TargetVirtualNetworks;
        }
    }
}
