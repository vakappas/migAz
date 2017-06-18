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
    public partial class PublicIpProperties : UserControl
    {
        MigrationTarget.PublicIp _PublicIp;

        public delegate Task AfterPropertyChanged();
        public event AfterPropertyChanged PropertyChanged;

        public PublicIpProperties()
        {
            InitializeComponent();
        }

        internal void Bind(MigrationTarget.PublicIp publicIp)
        {
            _PublicIp = publicIp;

            txtTargetName.Text = _PublicIp.Name;
            txtDomainNameLabel.Text = _PublicIp.DomainNameLabel;
        }

        private void txtTargetName_TextChanged(object sender, EventArgs e)
        {
            TextBox txtSender = (TextBox)sender;

            _PublicIp.Name = txtSender.Text;

            PropertyChanged();
        }

        private void txtDomainNameLabel_TextChanged(object sender, EventArgs e)
        {
            TextBox txtSender = (TextBox)sender;

            _PublicIp.DomainNameLabel = txtSender.Text;

            PropertyChanged();
        }
    }
}
