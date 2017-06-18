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
            networkSelectionControl1.PropertyChanged += NetworkSelectionControl1_PropertyChanged;
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

            if (_TargetNetworkInterface.TargetNetworkInterfaceIpConfigurations.Count > 0)
            {
                await networkSelectionControl1.Bind(azureContext, targetTreeView.GetVirtualNetworksInMigration());
                networkSelectionControl1.VirtualNetworkTarget = _TargetNetworkInterface.TargetNetworkInterfaceIpConfigurations[0];
            }

            lblSourceName.Text = _TargetNetworkInterface.SourceName;
            txtTargetName.Text = _TargetNetworkInterface.TargetName;


            if (_TargetNetworkInterface.TargetNetworkInterfaceIpConfigurations.Count > 0)
            {
                cmbAllocationMethod.SelectedIndex = cmbAllocationMethod.FindString(_TargetNetworkInterface.TargetNetworkInterfaceIpConfigurations[0].TargetPrivateIPAllocationMethod);
            }

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

        private void cmbAllocationMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_TargetNetworkInterface.TargetNetworkInterfaceIpConfigurations.Count > 0)
            {
                _TargetNetworkInterface.TargetNetworkInterfaceIpConfigurations[0].TargetPrivateIPAllocationMethod = cmbAllocationMethod.SelectedItem.ToString();
                txtStaticIp.Enabled = cmbAllocationMethod.SelectedItem.ToString() == "Static";
                if (txtStaticIp.Enabled)
                    txtStaticIp.Text = _TargetNetworkInterface.TargetNetworkInterfaceIpConfigurations[0].TargetPrivateIpAddress;
                else
                    txtStaticIp.Text = String.Empty;
            }

            PropertyChanged();
        }

        private void txtStaticIp_TextChanged(object sender, EventArgs e)
        {
            if (cmbAllocationMethod.SelectedItem.ToString() == "Static")
            {
                if (_TargetNetworkInterface.TargetNetworkInterfaceIpConfigurations.Count > 0)
                    _TargetNetworkInterface.TargetNetworkInterfaceIpConfigurations[0].TargetPrivateIpAddress = txtStaticIp.Text.Trim();

                PropertyChanged();
            }
        }
    }
}
