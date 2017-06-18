using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MigAz.Azure;
using MigAz.Azure;

namespace MigAz.Azure.Asm.UserControls
{
    public partial class AzureAsmLoginControl : UserControl
    {
        private AzureContext _AzureContext;

        public AzureAsmLoginControl()
        {
            InitializeComponent();
        }

        public async Task BindContext(AzureContext azureContext)
        {
            _AzureContext = azureContext;

            cboAzureEnvironment.SelectedItem = null;

            int environmentIndex = cboAzureEnvironment.FindStringExact(_AzureContext.AzureEnvironment.ToString());
            if (environmentIndex >= 0)
            {
                cboAzureEnvironment.SelectedIndex = environmentIndex;
            }
            else
                cboAzureEnvironment.SelectedIndex = 0;

            if (_AzureContext.TokenProvider == null)
            {
                lblAuthenticatedUser.Text = "-";
                cboAzureEnvironment.Enabled = true;
                btnAuthenticate.Text = "Sign In";
            }
            else
            {
                lblAuthenticatedUser.Text = _AzureContext.TokenProvider.AuthenticationResult.UserInfo.DisplayableId;
                cboAzureEnvironment.Enabled = false;
                btnAuthenticate.Text = "Sign Out";
            }

            cmbSubscriptions.Items.Clear();
            if (_AzureContext.AzureRetriever != null)
            {
                foreach (AzureSubscription azureSubscription in await _AzureContext.AzureRetriever.GetSubscriptions())
                {
                    cmbSubscriptions.Items.Add(azureSubscription);
                }
                cmbSubscriptions.Enabled = true;
            }

            if (_AzureContext.AzureSubscription != null)
            {
                foreach (AzureSubscription azureSubscription in cmbSubscriptions.Items)
                {
                    if (_AzureContext.AzureSubscription == azureSubscription)
                        cmbSubscriptions.SelectedItem = azureSubscription;
                }
            }

        }

        internal void RemoveEnvironment(AzureEnvironment azureEnvironment)
        {
            cboAzureEnvironment.Items.Remove(azureEnvironment);

            if (cboAzureEnvironment.SelectedItem == null)
                cboAzureEnvironment.SelectedIndex = 0;
        }

        private void cboAzureEnvironment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_AzureContext != null)
            {
                if (cboAzureEnvironment.SelectedItem == null)
                    _AzureContext.AzureEnvironment = _AzureContext.AzureEnvironment;
                else
                    _AzureContext.AzureEnvironment = (AzureEnvironment) Enum.Parse(typeof(AzureEnvironment), cboAzureEnvironment.SelectedItem.ToString());
            }
        }

        private async void btnAuthenticate_Click(object sender, EventArgs e)
        {
            _AzureContext.LogProvider.WriteLog("GetToken_Click", "Start");

            try
            {

                if (btnAuthenticate.Text == "Sign In")
                {
                    cmbSubscriptions.Enabled = false;
                    cmbSubscriptions.Items.Clear();

                    await _AzureContext.Login();

                    if (_AzureContext.TokenProvider != null)
                    {
                        lblAuthenticatedUser.Text = _AzureContext.TokenProvider.AuthenticationResult.UserInfo.DisplayableId;
                        btnAuthenticate.Text = "Sign Out";

                        cmbSubscriptions.Items.Clear();
                        foreach (AzureSubscription azureSubscription in await _AzureContext.AzureRetriever.GetSubscriptions())
                        {
                            cmbSubscriptions.Items.Add(azureSubscription);
                        }

                        cboAzureEnvironment.Enabled = false;
                        cmbSubscriptions.Enabled = true;

                        if (cmbSubscriptions.Items.Count == 0)
                        {
                            MessageBox.Show("This account does not have any Azure Subscriptions.  Logging out of Azure AD Account.");
                            btnAuthenticate_Click(this, null); // No subscriptions, logout
                        }
                        if (cmbSubscriptions.Items.Count == 1)
                        {
                            cmbSubscriptions.SelectedIndex = 0;
                        }
                        else if (cmbSubscriptions.Items.Count > 1)
                        {
                            _AzureContext.StatusProvider.UpdateStatus("WAIT: Awaiting user selection of Azure Subscription");
                        }

                        cmbSubscriptions.Enabled = true;
                    }
                    else
                    {
                        _AzureContext.LogProvider.WriteLog("GetToken_Click", "Failed to get token");
                    }
                }
                else
                {
                    await _AzureContext.Logout();
                    lblAuthenticatedUser.Text = "<Not Authenticated>";
                    btnAuthenticate.Text = "Sign In";
                    cmbSubscriptions.Items.Clear();
                    cmbSubscriptions.Enabled = false;
                    cboAzureEnvironment.Enabled = true;
                }
            }
            catch (Exception exc)
            {
                _AzureContext.LogProvider.WriteLog("Exception in GetToken_Click", exc.Message);
                MessageBox.Show(exc.Message);
            }

            _AzureContext.LogProvider.WriteLog("GetToken_Click", "End");
        }

        private async void cmbSubscriptions_SelectedIndexChanged(object sender, EventArgs e)
        {
            _AzureContext.LogProvider.WriteLog("cmbSubscriptions_SelectedIndexChanged", "Start");

            ComboBox cmbSender = (ComboBox)sender;

            if (cmbSender.SelectedItem != null)
            {
                AzureSubscription selectedSubscription = (AzureSubscription)cmbSender.SelectedItem;
                if (_AzureContext.AzureSubscription != selectedSubscription)
                {
                    await _AzureContext.SetSubscriptionContext((AzureSubscription)cmbSender.SelectedItem);
                }
            }

            _AzureContext.LogProvider.WriteLog("cmbSubscriptions_SelectedIndexChanged", "End");
        }
    }
}
