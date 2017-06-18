namespace MigAz.AWS.UserControls
{
    partial class AmazonWebServiceLoginViewer
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnChange = new System.Windows.Forms.Button();
            this.lblSignInText = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnChange);
            this.groupBox1.Controls.Add(this.lblSignInText);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(723, 136);
            this.groupBox1.TabIndex = 55;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Amazon Web Services Subscription";
            // 
            // btnChange
            // 
            this.btnChange.Location = new System.Drawing.Point(567, 23);
            this.btnChange.Margin = new System.Windows.Forms.Padding(6);
            this.btnChange.Name = "btnChange";
            this.btnChange.Size = new System.Drawing.Size(141, 40);
            this.btnChange.TabIndex = 55;
            this.btnChange.Text = "Change";
            this.btnChange.UseVisualStyleBackColor = true;
            // 
            // lblSignInText
            // 
            this.lblSignInText.AutoSize = true;
            this.lblSignInText.Location = new System.Drawing.Point(43, 67);
            this.lblSignInText.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSignInText.Name = "lblSignInText";
            this.lblSignInText.Size = new System.Drawing.Size(141, 25);
            this.lblSignInText.TabIndex = 54;
            this.lblSignInText.Text = "Not Signed In";
            // 
            // AmazonWebServiceLoginViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "AmazonWebServiceLoginViewer";
            this.Size = new System.Drawing.Size(744, 150);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnChange;
        private System.Windows.Forms.Label lblSignInText;
    }
}
