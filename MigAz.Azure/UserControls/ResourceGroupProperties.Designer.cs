namespace MigAz.Azure.UserControls
{
    partial class ResourceGroupProperties
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTargetName = new System.Windows.Forms.TextBox();
            this.cboTargetLocation = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 25);
            this.label2.TabIndex = 1;
            this.label2.Text = "Location:";
            // 
            // txtTargetName
            // 
            this.txtTargetName.Location = new System.Drawing.Point(130, 20);
            this.txtTargetName.Name = "txtTargetName";
            this.txtTargetName.Size = new System.Drawing.Size(389, 31);
            this.txtTargetName.TabIndex = 0;
            this.txtTargetName.TextChanged += new System.EventHandler(this.txtTargetName_TextChanged);
            this.txtTargetName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTargetName_KeyPress);
            // 
            // cboTargetLocation
            // 
            this.cboTargetLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTargetLocation.FormattingEnabled = true;
            this.cboTargetLocation.Location = new System.Drawing.Point(130, 65);
            this.cboTargetLocation.Margin = new System.Windows.Forms.Padding(4);
            this.cboTargetLocation.Name = "cboTargetLocation";
            this.cboTargetLocation.Size = new System.Drawing.Size(389, 33);
            this.cboTargetLocation.TabIndex = 1;
            this.cboTargetLocation.SelectedIndexChanged += new System.EventHandler(this.cboTargetLocation_SelectedIndexChanged);
            // 
            // ResourceGroupProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cboTargetLocation);
            this.Controls.Add(this.txtTargetName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "ResourceGroupProperties";
            this.Size = new System.Drawing.Size(625, 126);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTargetName;
        private System.Windows.Forms.ComboBox cboTargetLocation;
    }
}
