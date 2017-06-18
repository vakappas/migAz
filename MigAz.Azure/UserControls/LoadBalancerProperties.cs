using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MigAz.Azure.UserControls
{
    public partial class LoadBalancerProperties : UserControl
    {
        private AzureContext _AzureContext;
        private TargetTreeView _TargetTreeView;
        private Azure.MigrationTarget.LoadBalancer _LoadBalancer;

        public delegate Task AfterPropertyChanged();
        public event AfterPropertyChanged PropertyChanged;

        public LoadBalancerProperties()
        {
            InitializeComponent();
            networkSelectionControl1.PropertyChanged += NetworkSelectionControl1_PropertyChanged;
        }

        private void NetworkSelectionControl1_PropertyChanged()
        {

            PropertyChanged();
        }

        internal async Task Bind(AzureContext azureContext, TargetTreeView targetTreeView)
        {
            _AzureContext = azureContext;
            _TargetTreeView = targetTreeView;
        }

        public MigrationTarget.LoadBalancer LoadBalancer
        {
            get { return _LoadBalancer; }
            set
            {
                _LoadBalancer = value;

                cmbLoadBalancerType.SelectedIndex = cmbLoadBalancerType.FindString(_LoadBalancer.LoadBalancerType.ToString());
                txtTargetName.Text = _LoadBalancer.Name;
            }
        }

        private void txtTargetName_TextChanged(object sender, EventArgs e)
        {
            TextBox txtSender = (TextBox)sender;

            _LoadBalancer.Name = txtSender.Text;

            PropertyChanged();
        }

        private async void cmbLoadBalancerType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbLoadBalancerType.SelectedItem.ToString() == "Public")
            {
                _LoadBalancer.LoadBalancerType = MigrationTarget.LoadBalancerType.Public;
                this.networkSelectionControl1.Enabled = false;
                this.networkSelectionControl1.Visible = false;
            }
            else
            {
                _LoadBalancer.LoadBalancerType = MigrationTarget.LoadBalancerType.Internal;
                this.networkSelectionControl1.Enabled = true;
                this.networkSelectionControl1.Visible = true;

                if (_LoadBalancer.FrontEndIpConfigurations.Count > 0)
                {
                    await networkSelectionControl1.Bind(_AzureContext, _TargetTreeView.GetVirtualNetworksInMigration());
                    networkSelectionControl1.VirtualNetworkTarget = _LoadBalancer.FrontEndIpConfigurations[0];
                }
            }

            await PropertyChanged();
        }
    }
}
