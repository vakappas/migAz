namespace MigAz.Azure.UserControls
{
    partial class SubnetProperties
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
            this.lblSourceName = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblAddressSpace = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(142, 25);
            this.label2.TabIndex = 4;
            this.label2.Text = "Target Name:";
            // 
            // txtTargetName
            // 
            this.txtTargetName.Location = new System.Drawing.Point(233, 43);
            this.txtTargetName.Name = "txtTargetName";
            this.txtTargetName.Size = new System.Drawing.Size(318, 31);
            this.txtTargetName.TabIndex = 3;
            this.txtTargetName.TextChanged += new System.EventHandler(this.txtTargetName_TextChanged);
            this.txtTargetName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTargetName_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(148, 25);
            this.label1.TabIndex = 5;
            this.label1.Text = "Source Name:";
            // 
            // lblSourceName
            // 
            this.lblSourceName.AutoSize = true;
            this.lblSourceName.Location = new System.Drawing.Point(228, 9);
            this.lblSourceName.Name = "lblSourceName";
            this.lblSourceName.Size = new System.Drawing.Size(70, 25);
            this.lblSourceName.TabIndex = 6;
            this.lblSourceName.Text = "label3";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(164, 25);
            this.label3.TabIndex = 7;
            this.label3.Text = "Address Space:";
            // 
            // lblAddressSpace
            // 
            this.lblAddressSpace.AutoSize = true;
            this.lblAddressSpace.Location = new System.Drawing.Point(228, 82);
            this.lblAddressSpace.Name = "lblAddressSpace";
            this.lblAddressSpace.Size = new System.Drawing.Size(120, 25);
            this.lblAddressSpace.TabIndex = 8;
            this.lblAddressSpace.Text = "10.0.0.0/24";
            // 
            // SubnetProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblAddressSpace);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblSourceName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtTargetName);
            this.Name = "SubnetProperties";
            this.Size = new System.Drawing.Size(625, 122);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTargetName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblSourceName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblAddressSpace;
    }
}
