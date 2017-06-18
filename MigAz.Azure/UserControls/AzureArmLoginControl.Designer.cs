namespace MigAz.Azure.UserControls
{
    partial class AzureArmLoginControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblAuthenticatedUser = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblSubscriptions = new System.Windows.Forms.Label();
            this.cmbSubscriptions = new System.Windows.Forms.ComboBox();
            this.btnAuthenticate = new System.Windows.Forms.Button();
            this.cboAzureEnvironment = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cboTenant = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // lblAuthenticatedUser
            // 
            this.lblAuthenticatedUser.AutoSize = true;
            this.lblAuthenticatedUser.Location = new System.Drawing.Point(236, 77);
            this.lblAuthenticatedUser.Name = "lblAuthenticatedUser";
            this.lblAuthenticatedUser.Size = new System.Drawing.Size(207, 25);
            this.lblAuthenticatedUser.TabIndex = 60;
            this.lblAuthenticatedUser.Text = "<Not Authenticated>";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 77);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(160, 25);
            this.label2.TabIndex = 59;
            this.label2.Text = "Azure AD User:";
            // 
            // lblSubscriptions
            // 
            this.lblSubscriptions.AutoSize = true;
            this.lblSubscriptions.Location = new System.Drawing.Point(8, 220);
            this.lblSubscriptions.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblSubscriptions.Name = "lblSubscriptions";
            this.lblSubscriptions.Size = new System.Drawing.Size(137, 25);
            this.lblSubscriptions.TabIndex = 56;
            this.lblSubscriptions.Text = "Subscription:";
            // 
            // cmbSubscriptions
            // 
            this.cmbSubscriptions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSubscriptions.Enabled = false;
            this.cmbSubscriptions.FormattingEnabled = true;
            this.cmbSubscriptions.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.cmbSubscriptions.Location = new System.Drawing.Point(241, 212);
            this.cmbSubscriptions.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.cmbSubscriptions.MaxDropDownItems = 15;
            this.cmbSubscriptions.Name = "cmbSubscriptions";
            this.cmbSubscriptions.Size = new System.Drawing.Size(921, 33);
            this.cmbSubscriptions.Sorted = true;
            this.cmbSubscriptions.TabIndex = 2;
            this.cmbSubscriptions.SelectedIndexChanged += new System.EventHandler(this.cmbSubscriptions_SelectedIndexChanged);
            // 
            // btnAuthenticate
            // 
            this.btnAuthenticate.Location = new System.Drawing.Point(241, 108);
            this.btnAuthenticate.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.btnAuthenticate.Name = "btnAuthenticate";
            this.btnAuthenticate.Size = new System.Drawing.Size(202, 44);
            this.btnAuthenticate.TabIndex = 1;
            this.btnAuthenticate.Text = "Sign In";
            this.btnAuthenticate.UseVisualStyleBackColor = true;
            this.btnAuthenticate.Click += new System.EventHandler(this.btnAuthenticate_Click);
            // 
            // cboAzureEnvironment
            // 
            this.cboAzureEnvironment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAzureEnvironment.FormattingEnabled = true;
            this.cboAzureEnvironment.Items.AddRange(new object[] {
            "AzureCloud",
            "AzureChinaCloud",
            "AzureGermanCloud",
            "AzureUSGovernment"});
            this.cboAzureEnvironment.Location = new System.Drawing.Point(241, 13);
            this.cboAzureEnvironment.Margin = new System.Windows.Forms.Padding(4);
            this.cboAzureEnvironment.Name = "cboAzureEnvironment";
            this.cboAzureEnvironment.Size = new System.Drawing.Size(323, 33);
            this.cboAzureEnvironment.TabIndex = 0;
            this.cboAzureEnvironment.SelectedIndexChanged += new System.EventHandler(this.cboAzureEnvironment_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 16);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(200, 25);
            this.label1.TabIndex = 57;
            this.label1.Text = "Azure Environment:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 172);
            this.label3.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 25);
            this.label3.TabIndex = 62;
            this.label3.Text = "Tenant:";
            // 
            // cboTenant
            // 
            this.cboTenant.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTenant.Enabled = false;
            this.cboTenant.FormattingEnabled = true;
            this.cboTenant.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.cboTenant.Location = new System.Drawing.Point(241, 164);
            this.cboTenant.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.cboTenant.MaxDropDownItems = 15;
            this.cboTenant.Name = "cboTenant";
            this.cboTenant.Size = new System.Drawing.Size(921, 33);
            this.cboTenant.Sorted = true;
            this.cboTenant.TabIndex = 61;
            this.cboTenant.SelectedIndexChanged += new System.EventHandler(this.cboTenant_SelectedIndexChanged);
            // 
            // AzureArmLoginControl2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cboTenant);
            this.Controls.Add(this.lblAuthenticatedUser);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblSubscriptions);
            this.Controls.Add(this.cmbSubscriptions);
            this.Controls.Add(this.btnAuthenticate);
            this.Controls.Add(this.cboAzureEnvironment);
            this.Controls.Add(this.label1);
            this.Name = "AzureArmLoginControl2";
            this.Size = new System.Drawing.Size(1171, 267);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblAuthenticatedUser;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblSubscriptions;
        private System.Windows.Forms.ComboBox cmbSubscriptions;
        private System.Windows.Forms.Button btnAuthenticate;
        private System.Windows.Forms.ComboBox cboAzureEnvironment;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboTenant;
    }
}
