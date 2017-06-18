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
using MigAz.Azure.Arm;

namespace MigAz.Azure.UserControls
{
    public partial class AvailabilitySetProperties : UserControl
    {
        TreeNode _armAvailabilitySetNode;

        public delegate Task AfterPropertyChanged();
        public event AfterPropertyChanged PropertyChanged;

        public AvailabilitySetProperties()
        {
            InitializeComponent();
        }

        internal void Bind(TreeNode availabilitySetNode)
        {
            _armAvailabilitySetNode = availabilitySetNode;

            Azure.MigrationTarget.AvailabilitySet armAvailabilitySet = (Azure.MigrationTarget.AvailabilitySet)_armAvailabilitySetNode.Tag;
            txtTargetName.Text = armAvailabilitySet.TargetName;
        }

        private void txtTargetName_TextChanged(object sender, EventArgs e)
        {
            TextBox txtSender = (TextBox)sender;

            Azure.MigrationTarget.AvailabilitySet targetAvailabilitySet = (Azure.MigrationTarget.AvailabilitySet)_armAvailabilitySetNode.Tag;

            targetAvailabilitySet.TargetName = txtSender.Text;
            _armAvailabilitySetNode.Text = targetAvailabilitySet.ToString();

            PropertyChanged();
        }
    }
}
