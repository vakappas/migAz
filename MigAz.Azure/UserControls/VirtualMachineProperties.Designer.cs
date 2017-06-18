namespace MigAz.Azure.UserControls
{
    partial class VirtualMachineProperties
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
            this.lblRoleSize = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblOS = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.diskProperties1 = new MigAz.Azure.UserControls.DiskProperties();
            this.label12 = new System.Windows.Forms.Label();
            this.lblASMVMName = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.txtTargetName = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lblRoleSize
            // 
            this.lblRoleSize.AutoSize = true;
            this.lblRoleSize.Location = new System.Drawing.Point(176, 68);
            this.lblRoleSize.Name = "lblRoleSize";
            this.lblRoleSize.Size = new System.Drawing.Size(56, 25);
            this.lblRoleSize.TabIndex = 5;
            this.lblRoleSize.Text = "Role";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 68);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 25);
            this.label4.TabIndex = 4;
            this.label4.Text = "Size:";
            // 
            // lblOS
            // 
            this.lblOS.AutoSize = true;
            this.lblOS.Location = new System.Drawing.Point(176, 103);
            this.lblOS.Name = "lblOS";
            this.lblOS.Size = new System.Drawing.Size(42, 25);
            this.lblOS.TabIndex = 7;
            this.lblOS.Text = "OS";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 103);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(48, 25);
            this.label6.TabIndex = 6;
            this.label6.Text = "OS:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(20, 209);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(97, 25);
            this.label10.TabIndex = 18;
            this.label10.Text = "OS Disk";
            // 
            // diskProperties1
            // 
            this.diskProperties1.AllowManangedDisk = false;
            this.diskProperties1.Location = new System.Drawing.Point(17, 250);
            this.diskProperties1.Name = "diskProperties1";
            this.diskProperties1.Size = new System.Drawing.Size(640, 528);
            this.diskProperties1.TabIndex = 19;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(12, 35);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(142, 25);
            this.label12.TabIndex = 22;
            this.label12.Text = "Target Name:";
            // 
            // lblASMVMName
            // 
            this.lblASMVMName.AutoSize = true;
            this.lblASMVMName.Location = new System.Drawing.Point(176, 0);
            this.lblASMVMName.Name = "lblASMVMName";
            this.lblASMVMName.Size = new System.Drawing.Size(158, 25);
            this.lblASMVMName.TabIndex = 21;
            this.lblASMVMName.Text = "ASM VM Name";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(12, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(148, 25);
            this.label14.TabIndex = 20;
            this.label14.Text = "Source Name:";
            // 
            // txtTargetName
            // 
            this.txtTargetName.Location = new System.Drawing.Point(181, 29);
            this.txtTargetName.Name = "txtTargetName";
            this.txtTargetName.Size = new System.Drawing.Size(403, 31);
            this.txtTargetName.TabIndex = 23;
            this.txtTargetName.TextChanged += new System.EventHandler(this.txtTargetName_TextChanged);
            this.txtTargetName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTargetName_KeyPress);
            // 
            // VirtualMachineProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtTargetName);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.lblASMVMName);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.diskProperties1);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.lblOS);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lblRoleSize);
            this.Controls.Add(this.label4);
            this.Name = "VirtualMachineProperties";
            this.Size = new System.Drawing.Size(625, 807);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblRoleSize;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblOS;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label10;
        private DiskProperties diskProperties1;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label lblASMVMName;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtTargetName;
    }
}
