namespace MigAz.Azure.Forms
{
    partial class AzureSubscriptionContextDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbSubscriptions = new System.Windows.Forms.ComboBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblAzureEnvironment = new System.Windows.Forms.Label();
            this.lblAzureUsername = new System.Windows.Forms.Label();
            this.cboTenant = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(200, 25);
            this.label1.TabIndex = 59;
            this.label1.Text = "Azure Environment:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(160, 25);
            this.label2.TabIndex = 60;
            this.label2.Text = "Azure AD User:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 144);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(137, 25);
            this.label3.TabIndex = 61;
            this.label3.Text = "Subscription:";
            // 
            // cmbSubscriptions
            // 
            this.cmbSubscriptions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSubscriptions.Enabled = false;
            this.cmbSubscriptions.FormattingEnabled = true;
            this.cmbSubscriptions.Location = new System.Drawing.Point(250, 141);
            this.cmbSubscriptions.Name = "cmbSubscriptions";
            this.cmbSubscriptions.Size = new System.Drawing.Size(915, 33);
            this.cmbSubscriptions.Sorted = true;
            this.cmbSubscriptions.TabIndex = 1;
            this.cmbSubscriptions.SelectedIndexChanged += new System.EventHandler(this.cmbSubscriptions_SelectedIndexChanged);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnClose.Location = new System.Drawing.Point(952, 196);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(213, 47);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblAzureEnvironment
            // 
            this.lblAzureEnvironment.AutoSize = true;
            this.lblAzureEnvironment.Location = new System.Drawing.Point(250, 20);
            this.lblAzureEnvironment.Name = "lblAzureEnvironment";
            this.lblAzureEnvironment.Size = new System.Drawing.Size(19, 25);
            this.lblAzureEnvironment.TabIndex = 64;
            this.lblAzureEnvironment.Text = "-";
            // 
            // lblAzureUsername
            // 
            this.lblAzureUsername.AutoSize = true;
            this.lblAzureUsername.Location = new System.Drawing.Point(250, 60);
            this.lblAzureUsername.Name = "lblAzureUsername";
            this.lblAzureUsername.Size = new System.Drawing.Size(19, 25);
            this.lblAzureUsername.TabIndex = 65;
            this.lblAzureUsername.Text = "-";
            // 
            // cboTenant
            // 
            this.cboTenant.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTenant.Enabled = false;
            this.cboTenant.FormattingEnabled = true;
            this.cboTenant.Location = new System.Drawing.Point(250, 94);
            this.cboTenant.Name = "cboTenant";
            this.cboTenant.Size = new System.Drawing.Size(915, 33);
            this.cboTenant.Sorted = true;
            this.cboTenant.TabIndex = 66;
            this.cboTenant.SelectedIndexChanged += new System.EventHandler(this.cboTenant_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(25, 100);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(85, 25);
            this.label4.TabIndex = 67;
            this.label4.Text = "Tenant:";
            // 
            // AzureSubscriptionContextDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1188, 268);
            this.Controls.Add(this.cboTenant);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblAzureUsername);
            this.Controls.Add(this.lblAzureEnvironment);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.cmbSubscriptions);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "AzureSubscriptionContextDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Azure Subscription";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AzureContextARMDialog_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.FontDialog fontDialog1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbSubscriptions;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblAzureEnvironment;
        private System.Windows.Forms.Label lblAzureUsername;
        private System.Windows.Forms.ComboBox cboTenant;
        private System.Windows.Forms.Label label4;
    }
}