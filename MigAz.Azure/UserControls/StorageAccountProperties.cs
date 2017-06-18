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
using MigAz.Azure;

namespace MigAz.Azure.UserControls
{
    public partial class StorageAccountProperties : UserControl
    {
        private MigrationTarget.StorageAccount _StorageAccount;
        private AzureContext _AzureContext;

        public delegate Task AfterPropertyChanged();
        public event AfterPropertyChanged PropertyChanged;

        public StorageAccountProperties()
        {
            InitializeComponent();
        }

        public void Bind(AzureContext azureContext, MigrationTarget.StorageAccount storageAccount)
        {
            _AzureContext = azureContext;
            txtTargetName.MaxLength = 24 - azureContext.SettingsProvider.StorageAccountSuffix.Length;

            _StorageAccount = storageAccount;
            lblAccountType.Text = storageAccount.SourceAccount.AccountType;
            lblSourceASMName.Text = storageAccount.SourceAccount.Name;

            if (storageAccount.TargetName != null)
            {
                if (storageAccount.TargetName.Length > txtTargetName.MaxLength)
                    txtTargetName.Text = storageAccount.TargetName.Substring(0, txtTargetName.MaxLength);
                else
                    txtTargetName.Text = storageAccount.TargetName;
            }

            cmbAccountType.SelectedIndex = cmbAccountType.FindString(storageAccount.AccountType);
        }

        private void txtTargetName_TextChanged(object sender, EventArgs e)
        {
            TextBox txtSender = (TextBox)sender;

            _StorageAccount.TargetName = txtSender.Text;

            PropertyChanged();
        }

        private void txtTargetName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetterOrDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void cmbAccountType_SelectedIndexChanged(object sender, EventArgs e)
        {
            _StorageAccount.AccountType = cmbAccountType.SelectedItem.ToString();
            PropertyChanged();
        }
    }
}
