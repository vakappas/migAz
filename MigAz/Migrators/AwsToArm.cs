using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;
using MigAz.Core.Interface;
using MigAz.AWS.Providers;
using MigAz.Core;
using MigAz.Azure;
using MigAz.Azure.UserControls;
using MigAz.Azure.Generator.AsmToArm;
using MigAz.AWS.Generator;

namespace MigAz.Migrators
{
    public partial class AwsToArm : Azure.UserControls.IMigratorUserControl
    {
        private AwsToArmSaveSelectionProvider _saveSelectionProvider;
        private AwsToArmSaveSelectionProvider _telemetryProvider;
        private PropertyPanel _PropertyPanel;
        private ImageList _AzureResourceImageList;

        //private EC2Operation ec2 = null;
        // TODO WHERE?? static IAmazonEC2 service;
        private AzureContext _AzureContextTargetARM;
        private TreeNode _SourceAwsNode = null;

        public AwsToArm() : base(null, null) { }

        public AwsToArm(IStatusProvider statusProvider, ILogProvider logProvider, PropertyPanel propertyPanel) 
            : base (statusProvider, logProvider)
        {
            InitializeComponent();

            _PropertyPanel = propertyPanel;
            _saveSelectionProvider = new AwsToArmSaveSelectionProvider();
            _telemetryProvider = new AwsToArmSaveSelectionProvider();

            _AzureContextTargetARM = new AzureContext(LogProvider, StatusProvider, null); // _appSettingsProvider);
            _AzureContextTargetARM.AfterAzureSubscriptionChange += _AzureContextTargetARM_AfterAzureSubscriptionChange;

            azureLoginContextViewer21.Bind(_AzureContextTargetARM);

            this.TemplateGenerator = new AwsGenerator(LogProvider, StatusProvider);

            this.treeTargetARM.LogProvider = this.LogProvider;
            this.treeTargetARM.StatusProvider = this.StatusProvider;

            if (_AzureContextTargetARM != null && _AzureContextTargetARM.SettingsProvider != null)
                this.treeTargetARM.SettingsProvider = _AzureContextTargetARM.SettingsProvider;

            this._PropertyPanel.LogProvider = this.LogProvider;
            this._PropertyPanel.StatusProvider = this.StatusProvider;
            this._PropertyPanel.AzureContext = _AzureContextTargetARM;
            this._PropertyPanel.TargetTreeView = treeTargetARM;
            this._PropertyPanel.PropertyChanged += _PropertyPanel_PropertyChanged;
        }

        private async Task _PropertyPanel_PropertyChanged()
        {
            if (_SourceAwsNode == null) // we are not going to update on every property bind during TreeView updates
                await this.TemplateGenerator.UpdateArtifacts(treeTargetARM.GetExportArtifacts());
        }


        private async Task _AzureContextTargetARM_AfterAzureSubscriptionChange(AzureContext sender)
        {

        }

        public ImageList AzureResourceImageList
        {
            get { return _AzureResourceImageList; }
            set
            {
                _AzureResourceImageList = value;

                if (treeTargetARM != null)
                    treeTargetARM.ImageList = _AzureResourceImageList;
            }
        }


        private void AwsToArm_Load(object sender, EventArgs e)
        {
            if (LogProvider != null)
                LogProvider.WriteLog("Window_Load", "Program start");
            // TODO instResponse = new DescribeInstancesResponse();
            // this.Text = "migAz AWS (" + Assembly.GetEntryAssembly().GetName().Version.ToString() + ")";
            AlertIfNewVersionAvailable();
        }

        #region New Version Check

        private async Task AlertIfNewVersionAvailable()
        {
            string currentVersion = "2.2.12.0";
            VersionCheck versionCheck = new VersionCheck(this.LogProvider);
            string newVersionNumber = await versionCheck.GetAvailableVersion("https://api.migaz.tools/v1/version/AWStoARM", currentVersion);
            if (versionCheck.IsVersionNewer(currentVersion, newVersionNumber))
            {
                DialogResult dialogresult = MessageBox.Show("New version " + newVersionNumber + " is available at http://aka.ms/MigAz", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        #endregion

        //TODO CHECK
        private void Window_FormClosing(object sender, FormClosingEventArgs e)
        {
            // If save selection option is enabled
            //if (app.Default.SaveSelection)
            //{
            //    _saveSelectionProvider.Save(cmbRegion.Text, lvwVirtualNetworks, lvwVirtualMachines);
            //}
        }

        private void btnGetToken_Click(object sender, EventArgs e)
        {
            LogProvider.WriteLog("GetToken_Click", "Start");

            try
            {
                //Load Items
                Load_Items();

                //lblSignInText.Text = $"Signed in as {accessKeyID}";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }


        private void Load_Items()
        {
            LogProvider.WriteLog("Load_Items", "Start");

            treeSourceAWS.Nodes.Clear();
            treeTargetARM.Nodes.Clear();

            TreeNode subscriptionNode = new TreeNode("AWS Subscription");
            treeSourceAWS.Nodes.Add(subscriptionNode);

            AWSRetriever awsRetriever = new AWSRetriever(this.LogProvider, this.StatusProvider);
            awsRetriever.toname(accessKeyTextBox.Text.Trim(), secretKeyTextBox.Text.Trim(), subscriptionNode);

            subscriptionNode.ExpandAll();

            StatusProvider.UpdateStatus("Ready");
        }

        public override void SeekAlertSource(object sourceObject)
        {
            treeTargetARM.SeekAlertSource(sourceObject);
        }














































        private void AutoSelectDependencies(ItemCheckedEventArgs listViewRow)
        {
            // TODO
            //    string InstanceId = listViewRow.Item.ListView.Items[listViewRow.Item.Index].SubItems[0].Text;

            //    var availableInstances = _awsObjectRetriever.Instances;
            //    //var selectedVolumes;
            //    if (InstanceId != null)
            //    {

            //        //var selectedInstances = availableInstances.Reservations[0].Instances.Find(x => x.InstanceId == InstanceId);
            //        var selectedInstances = _awsObjectRetriever.getInstancebyId(InstanceId);

            //        //foreach (ListViewItem virtualNetwork in lvwVirtualNetworks.Items)
            //        //{
            //        //    if (selectedInstances.Instances[0].VpcId == virtualNetwork.SubItems[0].Text)
            //        //    {
            //        //        virtualNetwork.Checked = true;
            //        //        virtualNetwork.Selected = true;
            //        //    }

            //        //}

            //    }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            Load_Items();
        }

        private void AwsToArm_Resize(object sender, EventArgs e)
        {
            treeSourceAWS.Height = this.Height - 150;
            treeTargetARM.Height = this.Height - 150;
        }

        private async void treeSourceAWS_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (_SourceAwsNode == null)
            {
                _SourceAwsNode = e.Node;
            }

            //if (e.Node.Checked)
            //    await AutoSelectDependencies(e.Node);

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

            if (_SourceAwsNode != null && _SourceAwsNode == e.Node)
            {
                if (e.Node.Checked)
                {
                    await RecursiveCheckToggleDown(e.Node, e.Node.Checked);
                    FillUpIfFullDown(e.Node);
                    treeSourceAWS.SelectedNode = e.Node;
                }
                else
                {
                    await RecursiveCheckToggleUp(e.Node, e.Node.Checked);
                    await RecursiveCheckToggleDown(e.Node, e.Node.Checked);
                }

                //_SelectedNodes = this.GetSelectedNodes(treeSourceASM);
                //UpdateExportItemsCount();
                await this.TemplateGenerator.UpdateArtifacts(treeTargetARM.GetExportArtifacts());

                _SourceAwsNode = null;

                if (resultUpdateARMTree != null)
                    treeTargetARM.SelectedNode = resultUpdateARMTree;
            }
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

      
    }
}
