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
    public partial class NetworkInterfaceProperties : UserControl
    {
        private AzureContext _AzureContext;
        private TargetTreeView _TargetTreeView;
        private Azure.MigrationTarget.NetworkInterface _TargetNetworkInterface;

        public delegate Task AfterPropertyChanged();
        public event AfterPropertyChanged PropertyChanged;

        public NetworkInterfaceProperties()
        {
            InitializeComponent();
        }

        private void NetworkSelectionControl1_PropertyChanged()
        {
            PropertyChanged(); // bubble event
        }

        internal async Task Bind(AzureContext azureContext, TargetTreeView targetTreeView, Azure.MigrationTarget.NetworkInterface targetNetworkInterface)
        {
            _AzureContext = azureContext;
            _TargetTreeView = targetTreeView;
            _TargetNetworkInterface = targetNetworkInterface;
            networkSelectionControl1.PropertyChanged += NetworkSelectionControl1_PropertyChanged;


            if (_TargetNetworkInterface.TargetNetworkInterfaceIpConfigurations.Count > 0)
            {
                await networkSelectionControl1.Bind(azureContext, targetTreeView, targetTreeView.GetVirtualNetworksInMigration());
                networkSelectionControl1.VirtualNetworkTarget = _TargetNetworkInterface.TargetNetworkInterfaceIpConfigurations[0];
            }

            lblSourceName.Text = _TargetNetworkInterface.SourceName;
            txtTargetName.Text = _TargetNetworkInterface.TargetName;

            if (_TargetNetworkInterface.SourceNetworkInterface != null)
            {
                if (_TargetNetworkInterface.SourceNetworkInterface.GetType() == typeof(Azure.Asm.NetworkInterface))
                {
                    Azure.Asm.NetworkInterface asmNetworkInterface = (Azure.Asm.NetworkInterface)_TargetNetworkInterface.SourceNetworkInterface;

                    lblVirtualNetworkName.Text = asmNetworkInterface.NetworkInterfaceIpConfigurations[0].VirtualNetworkName;
                    lblSubnetName.Text = asmNetworkInterface.NetworkInterfaceIpConfigurations[0].SubnetName;
                    lblStaticIpAddress.Text = asmNetworkInterface.NetworkInterfaceIpConfigurations[0].PrivateIpAddress;
                }
                else if (_TargetNetworkInterface.SourceNetworkInterface.GetType() == typeof(Azure.Arm.NetworkInterface))
                {
                    Azure.Arm.NetworkInterface armNetworkInterface = (Azure.Arm.NetworkInterface)_TargetNetworkInterface.SourceNetworkInterface;

                    lblVirtualNetworkName.Text = armNetworkInterface.NetworkInterfaceIpConfigurations[0].VirtualNetworkName;
                    lblSubnetName.Text = armNetworkInterface.NetworkInterfaceIpConfigurations[0].SubnetName;
                    lblStaticIpAddress.Text = armNetworkInterface.NetworkInterfaceIpConfigurations[0].PrivateIpAddress;
                }
            }
        }
        
       
        private void txtTargetName_TextChanged(object sender, EventArgs e)
        {
            TextBox txtSender = (TextBox)sender;

            _TargetNetworkInterface.TargetName = txtSender.Text.Trim();

            PropertyChanged();
        }

        private void txtTargetName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
