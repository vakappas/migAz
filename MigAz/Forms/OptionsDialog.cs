using System;
using System.Windows.Forms;

namespace MigAz.Forms
{
    public partial class OptionsDialog : Form
    {
        public OptionsDialog()
        {
            InitializeComponent();
        }

        private void chkAllowTelemetry_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAllowTelemetry.Checked == true)
            {
                string message = String.Empty + "\n";
                message = "\n" + "Tool telemetry data is important for us to keep improving it. Data collected is for tool development usage only and will not be shared, by any reason, out of the tool development team or scope.";
                message += "\n";
                message += "\n" + "Tool telemetry DOES send:";
                message += "\n" + ". TenantId";
                message += "\n" + ". SubscriptionId";
                message += "\n" + ". Processed resources type";
                message += "\n" + ". Processed resources location";
                message += "\n" + ". Execution date";
                message += "\n";
                message += "\n" + "Tool telemetry DOES NOT send:";
                message += "\n" + ". Resources names";
                message += "\n" + ". Any resources configuration or caracteristics";
                message += "\n" + ". Any local computer information";
                message += "\n" + ". Any other information not stated on the \"Tool telemetry DOES send\" section";
                message += "\n";
                message += "\n" + "Do you authorize the tool to send telemetry data?";
                DialogResult dialogresult = MessageBox.Show(message, "Authorization Request", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogresult == DialogResult.No)
                {
                    chkAllowTelemetry.Checked = false;
                }
            }
        }

        private void txtStorageAccountSuffix_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetterOrDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            app.Default.ResourceGroupSuffix = txtResourceGroupSuffix.Text.Trim();
            app.Default.VirtualNetworkSuffix = txtVirtualNetworkSuffix.Text.Trim();
            app.Default.VirtualNetworkGatewaySuffix = txtVirtualNetworkGatewaySuffix.Text.Trim();
            app.Default.NetworkSecurityGroupSuffix = txtNetworkSecurityGroupSuffix.Text.Trim();
            app.Default.StorageAccountSuffix = txtStorageAccountSuffix.Text.Trim();
            app.Default.PublicIPSuffix = txtPublicIPSuffix.Text.Trim();
            app.Default.LoadBalancerSuffix = txtLoadBalancerSuffix.Text.Trim();
            app.Default.AvailabilitySetSuffix = txtAvailabilitySetSuffix.Text.Trim();
            app.Default.VirtualMachineSuffix = txtVirtualMachineSuffix.Text.Trim();
            app.Default.NetworkInterfaceCardSuffix = txtNetworkInterfaceCardSuffix.Text.Trim();
            app.Default.AutoSelectDependencies = chkAutoSelectDependencies.Checked;
            app.Default.SaveSelection = chkSaveSelection.Checked;
            app.Default.BuildEmpty = chkBuildEmpty.Checked;
            app.Default.AllowTelemetry = chkAllowTelemetry.Checked;
            app.Default.Save();
        }

        private void formOptions_Load(object sender, EventArgs e)
        {
            txtResourceGroupSuffix.Text = app.Default.ResourceGroupSuffix;
            txtVirtualNetworkSuffix.Text = app.Default.VirtualNetworkSuffix;
            txtVirtualNetworkGatewaySuffix.Text = app.Default.VirtualNetworkGatewaySuffix;
            txtNetworkSecurityGroupSuffix.Text = app.Default.NetworkSecurityGroupSuffix;
            txtStorageAccountSuffix.Text = app.Default.StorageAccountSuffix;
            txtPublicIPSuffix.Text = app.Default.PublicIPSuffix;
            txtLoadBalancerSuffix.Text = app.Default.LoadBalancerSuffix;
            txtAvailabilitySetSuffix.Text = app.Default.AvailabilitySetSuffix;
            txtVirtualMachineSuffix.Text = app.Default.VirtualMachineSuffix;
            txtNetworkInterfaceCardSuffix.Text = app.Default.NetworkInterfaceCardSuffix;
            chkAutoSelectDependencies.Checked = app.Default.AutoSelectDependencies;
            chkSaveSelection.Checked = app.Default.SaveSelection;
            chkBuildEmpty.Checked = app.Default.BuildEmpty;
            chkAllowTelemetry.Checked = app.Default.AllowTelemetry;
        }

        private void btnApplyDefaultNaming_Click(object sender, EventArgs e)
        {
            txtResourceGroupSuffix.Text = "-rg";
            txtVirtualNetworkSuffix.Text = "-vnet";
            txtVirtualNetworkGatewaySuffix.Text = "-gw";
            txtNetworkSecurityGroupSuffix.Text = "-nsg";
            txtStorageAccountSuffix.Text = "v2";
            txtPublicIPSuffix.Text = "-pip";
            txtLoadBalancerSuffix.Text = "-lb";
            txtAvailabilitySetSuffix.Text = "-as";
            txtVirtualMachineSuffix.Text = "-vm";
            txtNetworkInterfaceCardSuffix.Text = "-nic";
        }
    }
}
