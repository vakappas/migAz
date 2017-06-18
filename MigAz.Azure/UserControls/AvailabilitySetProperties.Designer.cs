namespace MigAz.Azure.UserControls
{
    partial class AvailabilitySetProperties
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
            this.txtTargetName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtTargetName
            // 
            this.txtTargetName.Location = new System.Drawing.Point(165, 10);
            this.txtTargetName.Name = "txtTargetName";
            this.txtTargetName.Size = new System.Drawing.Size(328, 31);
            this.txtTargetName.TabIndex = 1;
            this.txtTargetName.TextChanged += new System.EventHandler(this.txtTargetName_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(142, 25);
            this.label2.TabIndex = 2;
            this.label2.Text = "Target Name:";
            // 
            // AvailabilitySetProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtTargetName);
            this.Name = "AvailabilitySetProperties";
            this.Size = new System.Drawing.Size(640, 62);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox txtTargetName;
        private System.Windows.Forms.Label label2;
    }
}
