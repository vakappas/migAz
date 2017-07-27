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
    public partial class PropertyPanel : UserControl
    {
        private TargetTreeView _TargetTreeView;
        private TreeNode _BoundTreeNode;

        public delegate Task AfterPropertyChanged();
        public event AfterPropertyChanged PropertyChanged;

        public PropertyPanel()
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

        public AzureContext AzureContext
        {
            get; set;
        }

        public TargetTreeView TargetTreeView
        {
            get { return _TargetTreeView; }
            set
            {
                _TargetTreeView = value;

                if (_TargetTreeView != null)
                    if (_TargetTreeView.PropertyPanel != this)
                        _TargetTreeView.PropertyPanel = this;
            }
        }

        public Image ResourceImage
        {
            get { return pictureBox1.Image; }
            private set { pictureBox1.Image = value; }
        }

        public string ResourceText
        {
            get { return lblAzureObjectName.Text; }
            private set { lblAzureObjectName.Text = value; }
        }

        public Control PropertyDetailControl
        {
            get
            {
                if (this.pnlProperties.Controls.Count == 0)
                    return null;
                else
                    return this.pnlProperties.Controls[0];
            }
            private set
            {
                this.pnlProperties.Controls.Clear();

                if (value != null)
                {
                    this.pnlProperties.Controls.Add(value);
                }
            }
        }

        public void Clear()
        {
            this._BoundTreeNode = null;
            this.ResourceImage = null;
            this.ResourceText = String.Empty;
            this.pnlProperties.Controls.Clear();
        }

        private void PropertyPanel_Resize(object sender, EventArgs e)
        {
            groupBox1.Width = this.Width - 5;
            groupBox1.Height = this.Height - 5;
        }

        private void groupBox1_Resize(object sender, EventArgs e)
        {
            pnlProperties.Width = groupBox1.Width - 15;
            pnlProperties.Height = groupBox1.Height - 90;
        }

        public async Task Bind(TreeNode migrationTargetNode)
        {
            this.LogProvider.WriteLog("PropertyPanel Bind", "Start");

            this.Clear();
            this._BoundTreeNode = migrationTargetNode;

            if (this.LogProvider == null)
                throw new ArgumentException("LogProvider Property must be set on Property Panel before Binding.");

            if (this.AzureContext == null)
                this.LogProvider.WriteLog("PropertyPanel Bind", "AzureContext Property must be set on Property Panel before Binding.");
            else
            {
                if (migrationTargetNode != null && migrationTargetNode.Tag != null)
                {
                    this.ResourceText = migrationTargetNode.Tag.ToString();

                    if (migrationTargetNode.Tag.GetType() == typeof(Azure.MigrationTarget.VirtualMachine))
                    {
                        this.ResourceImage = imageList1.Images["VirtualMachine"];

                        VirtualMachineProperties properties = new VirtualMachineProperties();
                        properties.AllowManangedDisk = false;
                        properties.PropertyChanged += Properties_PropertyChanged;
                        await properties.Bind(this.AzureContext, _TargetTreeView, (Azure.MigrationTarget.VirtualMachine)migrationTargetNode.Tag);
                        this.PropertyDetailControl = properties;
                    }
                    else if (migrationTargetNode.Tag.GetType() == typeof(Azure.MigrationTarget.NetworkSecurityGroup))
                    {
                        this.ResourceImage = imageList1.Images["NetworkSecurityGroup"];

                        NetworkSecurityGroupProperties properties = new NetworkSecurityGroupProperties();
                        properties.PropertyChanged += Properties_PropertyChanged;
                        properties.Bind((Azure.MigrationTarget.NetworkSecurityGroup)migrationTargetNode.Tag);
                        this.PropertyDetailControl = properties;
                    }
                    if (migrationTargetNode.Tag.GetType() == typeof(Azure.MigrationTarget.VirtualNetwork))
                    {
                        this.ResourceImage = imageList1.Images["VirtualNetwork"];

                        VirtualNetworkProperties properties = new VirtualNetworkProperties();
                        properties.PropertyChanged += Properties_PropertyChanged;
                        properties.Bind((Azure.MigrationTarget.VirtualNetwork)migrationTargetNode.Tag);
                        this.PropertyDetailControl = properties;
                    }
                    else if (migrationTargetNode.Tag.GetType() == typeof(Azure.MigrationTarget.Subnet))
                    {
                        this.ResourceImage = imageList1.Images["VirtualNetwork"];

                        SubnetProperties properties = new SubnetProperties();
                        properties.PropertyChanged += Properties_PropertyChanged;
                        properties.Bind((Azure.MigrationTarget.Subnet)migrationTargetNode.Tag);
                        this.PropertyDetailControl = properties;
                    }
                    else if (migrationTargetNode.Tag.GetType() == typeof(Azure.MigrationTarget.StorageAccount))
                    {
                        this.ResourceImage = imageList1.Images["StorageAccount"];

                        StorageAccountProperties properties = new StorageAccountProperties();
                        properties.PropertyChanged += Properties_PropertyChanged;
                        properties.Bind(this.AzureContext, (Azure.MigrationTarget.StorageAccount)migrationTargetNode.Tag);
                        this.PropertyDetailControl = properties;
                    }
                    else if (migrationTargetNode.Tag.GetType() == typeof(Azure.MigrationTarget.Disk))
                    {
                        Azure.MigrationTarget.Disk migrationDisk = (Azure.MigrationTarget.Disk)migrationTargetNode.Tag;

                        this.ResourceImage = imageList1.Images["Disk"];

                        DiskProperties properties = new DiskProperties();
                        properties.AllowManangedDisk = false;
                        properties.PropertyChanged += Properties_PropertyChanged;
                        properties.Bind(this.AzureContext, _TargetTreeView, (Azure.MigrationTarget.Disk)migrationTargetNode.Tag);
                        this.PropertyDetailControl = properties;
                    }
                    else if (migrationTargetNode.Tag.GetType() == typeof(Azure.MigrationTarget.AvailabilitySet))
                    {
                        this.ResourceImage = imageList1.Images["AvailabilitySet"];

                        AvailabilitySetProperties properties = new AvailabilitySetProperties();
                        properties.PropertyChanged += Properties_PropertyChanged;
                        properties.Bind(migrationTargetNode);
                        this.PropertyDetailControl = properties;
                    }
                    else if (migrationTargetNode.Tag.GetType() == typeof(Azure.MigrationTarget.NetworkInterface))
                    {
                        this.ResourceImage = imageList1.Images["NetworkInterface"];

                        NetworkInterfaceProperties properties = new NetworkInterfaceProperties();
                        properties.PropertyChanged += Properties_PropertyChanged;
                        await properties.Bind(this.AzureContext, _TargetTreeView, (Azure.MigrationTarget.NetworkInterface)migrationTargetNode.Tag);
                        this.PropertyDetailControl = properties;
                    }
                    else if (migrationTargetNode.Tag.GetType() == typeof(Azure.MigrationTarget.ResourceGroup))
                    {
                        this.ResourceImage = imageList1.Images["ResourceGroup"];

                        ResourceGroupProperties properties = new ResourceGroupProperties();
                        properties.PropertyChanged += Properties_PropertyChanged;
                        await properties.Bind(AzureContext, (Azure.MigrationTarget.ResourceGroup)migrationTargetNode.Tag);
                        this.PropertyDetailControl = properties;
                    }
                    else if (migrationTargetNode.Tag.GetType() == typeof(Azure.MigrationTarget.LoadBalancer))
                    {
                        this.ResourceImage = imageList1.Images["LoadBalancer"];

                        LoadBalancerProperties properties = new LoadBalancerProperties();
                        properties.PropertyChanged += Properties_PropertyChanged;
                        await properties.Bind(this.AzureContext, _TargetTreeView);
                        properties.LoadBalancer = (Azure.MigrationTarget.LoadBalancer)migrationTargetNode.Tag;
                        this.PropertyDetailControl = properties;
                    }
                    else if (migrationTargetNode.Tag.GetType() == typeof(Azure.MigrationTarget.PublicIp))
                    {
                        this.ResourceImage = imageList1.Images["PublicIp"];

                        PublicIpProperties properties = new PublicIpProperties();
                        properties.PropertyChanged += Properties_PropertyChanged;
                        properties.Bind((Azure.MigrationTarget.PublicIp)migrationTargetNode.Tag);
                        this.PropertyDetailControl = properties;
                    }
                }
            }

            this.LogProvider.WriteLog("PropertyPanel Bind", "End");
        }

        private async Task Properties_PropertyChanged()
        {
            if (_BoundTreeNode != null && _BoundTreeNode.Tag != null)
            {
                _BoundTreeNode.Text = _BoundTreeNode.Tag.ToString();

                if (_BoundTreeNode.Tag.GetType() == typeof(Azure.MigrationTarget.ResourceGroup))
                    _BoundTreeNode.Name = _BoundTreeNode.Tag.ToString();
            }

            await PropertyChanged();
        }
    }
}
