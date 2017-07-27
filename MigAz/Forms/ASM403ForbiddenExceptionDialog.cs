using MigAz.Core.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MigAz.Forms
{
    public partial class ASM403ForbiddenExceptionDialog : Form
    {

        Exception _UnhandledException;
        ILogProvider _LogProvider;

        private ASM403ForbiddenExceptionDialog() { }

        public ASM403ForbiddenExceptionDialog(ILogProvider fileLogProvider, Exception e)
        {
            InitializeComponent();
            _UnhandledException = e;
            _LogProvider = fileLogProvider;

            if (e.GetType() == typeof(System.Net.WebException))
            {
                System.Net.WebException webException = (System.Net.WebException)e;
                Stream responseStream = webException.Response.GetResponseStream();
                responseStream.Position = 0;
                StreamReader sr = new StreamReader(responseStream);
                string responseBody = sr.ReadToEnd();
                textBox1.Text = responseBody + Environment.NewLine + Environment.NewLine + webException.Message + Environment.NewLine + Environment.NewLine + webException.StackTrace;
            }
            else
                textBox1.Text = e.Message + Environment.NewLine + e.StackTrace;

            _LogProvider.WriteLog("ASM403ForbiddenExceptionDialog", textBox1.Text);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://docs.microsoft.com/en-us/azure/billing/billing-add-change-azure-subscription-administrator");
        }
    }
}
