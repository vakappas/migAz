using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MigAz.Azure.Forms
{
    public partial class AzureLoginContextDialog : Form
    {
        public AzureLoginContextDialog()
        {
            InitializeComponent();
        }

        public async Task InitializeDialog(AzureContext azureContext)
        {
            await this.azureArmLoginControl.BindContext(azureContext);
            azureContext.AfterAzureSubscriptionChange += AzureContextSourceASM_AfterAzureSubscriptionChange;
        }

        private async Task AzureContextSourceASM_AfterAzureSubscriptionChange(AzureContext sender)
        {
            this.Close();
        }

        private void btnCloseDialog_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}