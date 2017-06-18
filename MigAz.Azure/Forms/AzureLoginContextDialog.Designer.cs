namespace MigAz.Azure.Forms
{
    partial class AzureLoginContextDialog
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
            this.label4 = new System.Windows.Forms.Label();
            this.btnCloseDialog = new System.Windows.Forms.Button();
            this.azureArmLoginControl = new MigAz.Azure.UserControls.AzureArmLoginControl();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(1181, 25);
            this.label4.TabIndex = 54;
            this.label4.Text = "The following Azure Subscription context will be utilized to retrieve Azure Class" +
    "ic (ASM) resource information for migration.";
            // 
            // btnCloseDialog
            // 
            this.btnCloseDialog.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnCloseDialog.Location = new System.Drawing.Point(970, 341);
            this.btnCloseDialog.Name = "btnCloseDialog";
            this.btnCloseDialog.Size = new System.Drawing.Size(223, 52);
            this.btnCloseDialog.TabIndex = 1;
            this.btnCloseDialog.Text = "&Close";
            this.btnCloseDialog.UseVisualStyleBackColor = true;
            this.btnCloseDialog.Click += new System.EventHandler(this.btnCloseDialog_Click);
            // 
            // azureArmLoginControl
            // 
            this.azureArmLoginControl.Location = new System.Drawing.Point(22, 68);
            this.azureArmLoginControl.Name = "azureArmLoginControl";
            this.azureArmLoginControl.Size = new System.Drawing.Size(1171, 267);
            this.azureArmLoginControl.TabIndex = 55;
            // 
            // AzureLoginContextDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1212, 417);
            this.Controls.Add(this.azureArmLoginControl);
            this.Controls.Add(this.btnCloseDialog);
            this.Controls.Add(this.label4);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AzureLoginContextDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Azure Authentication";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnCloseDialog;
        private UserControls.AzureArmLoginControl azureArmLoginControl;
    }
}
