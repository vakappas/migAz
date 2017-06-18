using System;
using System.Diagnostics;
using System.Windows.Forms;
using MigAz.Azure.Generator.AsmToArm;
using MigAz.Core.Generator;

namespace MigAz.Forms
{
    public partial class ExportResultsDialog : Form
    {
        private TemplateGenerator _TemplateGenerator;

        private ExportResultsDialog() { }

        public ExportResultsDialog(TemplateGenerator templateGenerator)
        {
            InitializeComponent();
            _TemplateGenerator = templateGenerator;
        }
        
        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        private void btnViewTemplate_Click(object sender, EventArgs e)
        {
            ProcessStartInfo pInfo = new ProcessStartInfo();
            pInfo.FileName = _TemplateGenerator.OutputDirectory + "export.json";
            pInfo.UseShellExecute = true;
            Process p = Process.Start(pInfo);
        }

        private void btnGenerateInstructions_Click(object sender, EventArgs e)
        {
            ProcessStartInfo pInfo = new ProcessStartInfo();
           pInfo.FileName = _TemplateGenerator.OutputDirectory + "DeployInstructions.html";
            pInfo.UseShellExecute = true;
            Process p = Process.Start(pInfo);
        }

        private void ExportResults_Load(object sender, EventArgs e)
        {
           
        }
    }
}
