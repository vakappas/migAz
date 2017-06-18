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
using MigAz.Azure.Interface;

namespace MigAz.Azure.UserControls
{
    public partial class NetworkSelectionControl : UserControl
    {
        private IVirtualNetworkTarget _NetworkInterfaceTarget;
        private AzureContext _AzureContext;
        private List<Azure.MigrationTarget.VirtualNetwork> _TargetVirualNetworksInMigration = new List<MigrationTarget.VirtualNetwork>();

        public delegate void AfterPropertyChanged();
        public event AfterPropertyChanged PropertyChanged;

        public NetworkSelectionControl()
        {
            InitializeComponent();
        }

        public async Task Bind(AzureContext azureContext, List<Azure.MigrationTarget.VirtualNetwork> virtualNetworks)
        {
            _AzureContext = azureContext;
            _TargetVirualNetworksInMigration = virtualNetworks;

            try
            {
                List<Azure.Arm.VirtualNetwork> a = await _AzureContext.AzureRetriever.GetAzureARMVirtualNetworks();
                this.ExistingARMVNetEnabled = a.Count() > 0;
            }
            catch (Exception exc)
            {
                _AzureContext.LogProvider.WriteLog("VirtualMachineProperties.Bind", exc.Message);
                this.ExistingARMVNetEnabled = false;
            }


        }

        public IVirtualNetworkTarget VirtualNetworkTarget
        {
            get { return _NetworkInterfaceTarget; }
            set
            {
                _NetworkInterfaceTarget = value;

                if (this.ExistingARMVNetEnabled == false ||
                    _NetworkInterfaceTarget == null ||
                    _NetworkInterfaceTarget.TargetSubnet == null ||
                    _NetworkInterfaceTarget.TargetSubnet.GetType() == typeof(Azure.MigrationTarget.Subnet)
                    )
                {
                    this.SelectVNetInMigration();
                }
                else
                {
                    this.SelectExistingARMVNet();
                }
            }
        }

        public bool ExistingARMVNetEnabled
        {
            get { return rbExistingARMVNet.Enabled; }
            set { rbExistingARMVNet.Enabled = value; }
        }

        public void SelectVNetInMigration()
        {
            rbVNetInMigration.Checked = true;
        }

        public void SelectExistingARMVNet()
        {
            rbExistingARMVNet.Checked = true;
        }

        private async void cmbExistingArmVNets_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbExistingArmSubnet.Items.Clear();
            if (cmbExistingArmVNets.SelectedItem != null)
            {
                if (cmbExistingArmVNets.SelectedItem.GetType() == typeof(Azure.MigrationTarget.VirtualNetwork))
                {
                    Azure.MigrationTarget.VirtualNetwork selectedNetwork = (Azure.MigrationTarget.VirtualNetwork)cmbExistingArmVNets.SelectedItem;

                    foreach (Azure.MigrationTarget.Subnet subnet in selectedNetwork.TargetSubnets)
                    {
                        if (!subnet.IsGatewaySubnet)
                            cmbExistingArmSubnet.Items.Add(subnet);
                    }
                }
                else if (cmbExistingArmVNets.SelectedItem.GetType() == typeof(Azure.Arm.VirtualNetwork))
                {
                    Azure.Arm.VirtualNetwork selectedNetwork = (Azure.Arm.VirtualNetwork)cmbExistingArmVNets.SelectedItem;

                    foreach (Azure.Arm.Subnet subnet in selectedNetwork.Subnets)
                    {
                        if (!subnet.IsGatewaySubnet)
                            cmbExistingArmSubnet.Items.Add(subnet);
                    }
                }
            }

            PropertyChanged();
        }

        private async void rbVNetInMigration_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;

            if (rb.Checked)
            {
                #region Add "In MigAz Migration" Virtual Networks to cmbExistingArmVNets

                cmbExistingArmVNets.Items.Clear();
                cmbExistingArmSubnet.Items.Clear();

                foreach (Azure.MigrationTarget.VirtualNetwork targetVirtualNetwork in _TargetVirualNetworksInMigration)
                {
                    cmbExistingArmVNets.Items.Add(targetVirtualNetwork);
                }

                #endregion

                #region Seek Target VNet and Subnet as ComboBox SelectedItems

                if (_NetworkInterfaceTarget != null)
                {
                    if (_NetworkInterfaceTarget.TargetVirtualNetwork != null)
                    {
                        // Attempt to match target to list items
                        foreach (Azure.MigrationTarget.VirtualNetwork listVirtualNetwork in cmbExistingArmVNets.Items)
                        {
                            if (listVirtualNetwork.ToString() == _NetworkInterfaceTarget.TargetVirtualNetwork.ToString())
                            {
                                cmbExistingArmVNets.SelectedItem = listVirtualNetwork;
                                break;
                            }
                        }

                        if (cmbExistingArmVNets.SelectedItem != null && _NetworkInterfaceTarget.TargetSubnet != null)
                        {
                            foreach (Azure.MigrationTarget.Subnet listSubnet in cmbExistingArmSubnet.Items)
                            {
                                if (listSubnet.ToString() == _NetworkInterfaceTarget.TargetSubnet.ToString())
                                {
                                    cmbExistingArmSubnet.SelectedItem = listSubnet;
                                    break;
                                }
                            }
                        }
                    }
                }

                #endregion
            }

            PropertyChanged();
        }

        private async void rbExistingARMVNet_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;

            if (rb.Checked)
            {

                #region Add "In MigAz Migration" Virtual Networks to cmbExistingArmVNets

                cmbExistingArmVNets.Items.Clear();
                cmbExistingArmSubnet.Items.Clear();

                if (_AzureContext != null && _AzureContext.AzureRetriever != null)
                {
                    foreach (Azure.Arm.VirtualNetwork armVirtualNetwork in await _AzureContext.AzureRetriever.GetAzureARMVirtualNetworks())
                    {
                        if (armVirtualNetwork.HasNonGatewaySubnet)
                            cmbExistingArmVNets.Items.Add(armVirtualNetwork);
                    }
                }

                #endregion

                #region Seek Target VNet and Subnet as ComboBox SelectedItems

                if (_NetworkInterfaceTarget != null)
                {
                    if (_NetworkInterfaceTarget.TargetVirtualNetwork != null)
                    {
                        // Attempt to match target to list items
                        for (int i = 0; i < cmbExistingArmVNets.Items.Count; i++)
                        {
                            Azure.Arm.VirtualNetwork listVirtualNetwork = (Azure.Arm.VirtualNetwork)cmbExistingArmVNets.Items[i];
                            if (listVirtualNetwork.ToString() == _NetworkInterfaceTarget.TargetVirtualNetwork.ToString())
                            {
                                cmbExistingArmVNets.SelectedIndex = i;
                                break;
                            }
                        }
                    }

                    if (_NetworkInterfaceTarget.TargetSubnet != null)
                    {
                        // Attempt to match target to list items
                        for (int i = 0; i < cmbExistingArmSubnet.Items.Count; i++)
                        {
                            Azure.Arm.Subnet listSubnet = (Azure.Arm.Subnet)cmbExistingArmSubnet.Items[i];
                            if (listSubnet.ToString() == _NetworkInterfaceTarget.TargetSubnet.ToString())
                            {
                                cmbExistingArmSubnet.SelectedIndex = i;
                                break;
                            }
                        }
                    }
                }
                #endregion

            }

            PropertyChanged();
        }

        private void cmbExistingArmSubnet_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_NetworkInterfaceTarget != null)
            {
                if (cmbExistingArmSubnet.SelectedItem == null)
                {
                    _NetworkInterfaceTarget.TargetVirtualNetwork = null;
                    _NetworkInterfaceTarget.TargetSubnet = null;
                }
                else
                {
                    if (cmbExistingArmSubnet.SelectedItem.GetType() == typeof(Azure.MigrationTarget.Subnet))
                    {
                        _NetworkInterfaceTarget.TargetVirtualNetwork = (Azure.MigrationTarget.VirtualNetwork)cmbExistingArmVNets.SelectedItem;
                        _NetworkInterfaceTarget.TargetSubnet = (Azure.MigrationTarget.Subnet)cmbExistingArmSubnet.SelectedItem;
                    }
                    else if (cmbExistingArmSubnet.SelectedItem.GetType() == typeof(Azure.Arm.Subnet))
                    {
                        _NetworkInterfaceTarget.TargetVirtualNetwork = (Azure.Arm.VirtualNetwork)cmbExistingArmVNets.SelectedItem;
                        _NetworkInterfaceTarget.TargetSubnet = (Azure.Arm.Subnet)cmbExistingArmSubnet.SelectedItem;
                    }
                }
            }

            PropertyChanged();
        }

    }
}
