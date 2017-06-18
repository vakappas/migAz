using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MigAz.Azure.Asm;
using MigAz.Core.ArmTemplate;

namespace MigAz.Azure.UserControls
{
    public partial class SubnetProperties : UserControl
    {
        private MigrationTarget.Subnet _Subnet;

        public delegate Task AfterPropertyChanged();
        public event AfterPropertyChanged PropertyChanged;

        public SubnetProperties()
        {
            InitializeComponent();
        }

        internal void Bind(MigrationTarget.Subnet targetSubnet)
        {
            _Subnet = targetSubnet;

            txtTargetName.Text = targetSubnet.TargetName;

            if (targetSubnet.SourceSubnet != null)
            {
                if (targetSubnet.SourceSubnet.GetType() == typeof(Azure.Asm.Subnet))
                {
                    Azure.Asm.Subnet asmSubnet = (Azure.Asm.Subnet)targetSubnet.SourceSubnet;

                    lblSourceName.Text = asmSubnet.Name;
                    lblAddressSpace.Text = asmSubnet.AddressPrefix;
                }
                else if (targetSubnet.SourceSubnet.GetType() == typeof(Azure.Arm.Subnet))
                {
                    Azure.Arm.Subnet armSubnet = (Azure.Arm.Subnet)targetSubnet.SourceSubnet;

                    lblSourceName.Text = armSubnet.Name;
                    lblAddressSpace.Text = armSubnet.AddressPrefix;
                }
            }

            if (String.Compare(txtTargetName.Text, ArmConst.GatewaySubnetName, true) == 0)
            {
                // if gateway subnet, the name can't be changed
                txtTargetName.Enabled = false;
            }
        }

        private void txtTargetName_TextChanged(object sender, EventArgs e)
        {
            TextBox txtSender = (TextBox)sender;

            _Subnet.TargetName = txtSender.Text;

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
