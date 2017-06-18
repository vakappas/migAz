namespace MigAz.Azure.UserControls
{
    partial class PublicIpProperties
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
            this.label2 = new System.Windows.Forms.Label();
            this.txtTargetName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDomainNameLabel = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(142, 25);
            this.label2.TabIndex = 4;
            this.label2.Text = "Target Name:";
            // 
            // txtTargetName
            // 
            this.txtTargetName.Location = new System.Drawing.Point(176, 25);
            this.txtTargetName.Name = "txtTargetName";
            this.txtTargetName.Size = new System.Drawing.Size(328, 31);
            this.txtTargetName.TabIndex = 3;
            this.txtTargetName.TextChanged += new System.EventHandler(this.txtTargetName_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 74);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(212, 25);
            this.label1.TabIndex = 6;
            this.label1.Text = "Domain Name Label:";
            // 
            // txtDomainNameLabel
            // 
            this.txtDomainNameLabel.Location = new System.Drawing.Point(234, 74);
            this.txtDomainNameLabel.Name = "txtDomainNameLabel";
            this.txtDomainNameLabel.Size = new System.Drawing.Size(270, 31);
            this.txtDomainNameLabel.TabIndex = 5;
            this.txtDomainNameLabel.TextChanged += new System.EventHandler(this.txtDomainNameLabel_TextChanged);
            // 
            // PublicIpProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtDomainNameLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtTargetName);
            this.Name = "PublicIpProperties";
            this.Size = new System.Drawing.Size(527, 203);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTargetName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDomainNameLabel;
    }
}
