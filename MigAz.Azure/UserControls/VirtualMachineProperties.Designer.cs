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
            this.cbRoleSizes = new System.Windows.Forms.ComboBox();
            this.lblTargetNumberOfCores = new System.Windows.Forms.Label();
            this.lblTargetMemoryInGb = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblTargetMaxDataDisks = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblTargetLocationRequired = new System.Windows.Forms.Label();
            this.lblSourceCPUCores = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lblSourceMaxDataDisks = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.lblSourceMemoryInGb = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblRoleSize
            // 
            this.lblRoleSize.AutoSize = true;
            this.lblRoleSize.Location = new System.Drawing.Point(89, 36);
            this.lblRoleSize.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblRoleSize.Name = "lblRoleSize";
            this.lblRoleSize.Size = new System.Drawing.Size(53, 13);
            this.lblRoleSize.TabIndex = 5;
            this.lblRoleSize.Text = "Unknown";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 36);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(30, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Size:";
            // 
            // lblOS
            // 
            this.lblOS.AutoSize = true;
            this.lblOS.Location = new System.Drawing.Point(89, 19);
            this.lblOS.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblOS.Name = "lblOS";
            this.lblOS.Size = new System.Drawing.Size(22, 13);
            this.lblOS.TabIndex = 7;
            this.lblOS.Text = "OS";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 19);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(25, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "OS:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(10, 252);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(53, 13);
            this.label10.TabIndex = 18;
            this.label10.Text = "OS Disk";
            // 
            // diskProperties1
            // 
            this.diskProperties1.AllowManangedDisk = true;
            this.diskProperties1.Location = new System.Drawing.Point(8, 273);
            this.diskProperties1.Margin = new System.Windows.Forms.Padding(1);
            this.diskProperties1.Name = "diskProperties1";
            this.diskProperties1.ShowSizeInGb = true;
            this.diskProperties1.Size = new System.Drawing.Size(320, 350);
            this.diskProperties1.TabIndex = 2;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(20, 133);
            this.label12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(38, 13);
            this.label12.TabIndex = 22;
            this.label12.Text = "Name:";
            // 
            // lblASMVMName
            // 
            this.lblASMVMName.AutoSize = true;
            this.lblASMVMName.Location = new System.Drawing.Point(88, 0);
            this.lblASMVMName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblASMVMName.Name = "lblASMVMName";
            this.lblASMVMName.Size = new System.Drawing.Size(80, 13);
            this.lblASMVMName.TabIndex = 21;
            this.lblASMVMName.Text = "ASM VM Name";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(6, 0);
            this.label14.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(38, 13);
            this.label14.TabIndex = 20;
            this.label14.Text = "Name:";
            // 
            // txtTargetName
            // 
            this.txtTargetName.Location = new System.Drawing.Point(104, 130);
            this.txtTargetName.Margin = new System.Windows.Forms.Padding(2);
            this.txtTargetName.Name = "txtTargetName";
            this.txtTargetName.Size = new System.Drawing.Size(204, 20);
            this.txtTargetName.TabIndex = 0;
            this.txtTargetName.TextChanged += new System.EventHandler(this.txtTargetName_TextChanged);
            this.txtTargetName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTargetName_KeyPress);
            // 
            // cbRoleSizes
            // 
            this.cbRoleSizes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRoleSizes.FormattingEnabled = true;
            this.cbRoleSizes.Location = new System.Drawing.Point(105, 158);
            this.cbRoleSizes.Name = "cbRoleSizes";
            this.cbRoleSizes.Size = new System.Drawing.Size(203, 21);
            this.cbRoleSizes.TabIndex = 1;
            this.cbRoleSizes.SelectedIndexChanged += new System.EventHandler(this.cbRoleSizes_SelectedIndexChanged);
            // 
            // lblTargetNumberOfCores
            // 
            this.lblTargetNumberOfCores.AutoSize = true;
            this.lblTargetNumberOfCores.Location = new System.Drawing.Point(104, 187);
            this.lblTargetNumberOfCores.Name = "lblTargetNumberOfCores";
            this.lblTargetNumberOfCores.Size = new System.Drawing.Size(13, 13);
            this.lblTargetNumberOfCores.TabIndex = 25;
            this.lblTargetNumberOfCores.Text = "0";
            // 
            // lblTargetMemoryInGb
            // 
            this.lblTargetMemoryInGb.AutoSize = true;
            this.lblTargetMemoryInGb.Location = new System.Drawing.Point(104, 207);
            this.lblTargetMemoryInGb.Name = "lblTargetMemoryInGb";
            this.lblTargetMemoryInGb.Size = new System.Drawing.Size(13, 13);
            this.lblTargetMemoryInGb.TabIndex = 26;
            this.lblTargetMemoryInGb.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 161);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 27;
            this.label1.Text = "Size:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 187);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 28;
            this.label2.Text = "CPU Cores:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 207);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 13);
            this.label3.TabIndex = 29;
            this.label3.Text = "Memory (In GB):";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(20, 227);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(85, 13);
            this.label5.TabIndex = 31;
            this.label5.Text = "Max Data Disks:";
            // 
            // lblTargetMaxDataDisks
            // 
            this.lblTargetMaxDataDisks.AutoSize = true;
            this.lblTargetMaxDataDisks.Location = new System.Drawing.Point(104, 227);
            this.lblTargetMaxDataDisks.Name = "lblTargetMaxDataDisks";
            this.lblTargetMaxDataDisks.Size = new System.Drawing.Size(13, 13);
            this.lblTargetMaxDataDisks.TabIndex = 30;
            this.lblTargetMaxDataDisks.Text = "0";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(6, 109);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(44, 13);
            this.label7.TabIndex = 32;
            this.label7.Text = "Target";
            // 
            // lblTargetLocationRequired
            // 
            this.lblTargetLocationRequired.AutoSize = true;
            this.lblTargetLocationRequired.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTargetLocationRequired.Location = new System.Drawing.Point(104, 161);
            this.lblTargetLocationRequired.Name = "lblTargetLocationRequired";
            this.lblTargetLocationRequired.Size = new System.Drawing.Size(189, 13);
            this.lblTargetLocationRequired.TabIndex = 33;
            this.lblTargetLocationRequired.Text = "<Set Resource Group Location>";
            // 
            // lblSourceCPUCores
            // 
            this.lblSourceCPUCores.AutoSize = true;
            this.lblSourceCPUCores.Location = new System.Drawing.Point(89, 53);
            this.lblSourceCPUCores.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSourceCPUCores.Name = "lblSourceCPUCores";
            this.lblSourceCPUCores.Size = new System.Drawing.Size(13, 13);
            this.lblSourceCPUCores.TabIndex = 35;
            this.lblSourceCPUCores.Text = "0";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(7, 53);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(62, 13);
            this.label9.TabIndex = 34;
            this.label9.Text = "CPU Cores:";
            // 
            // lblSourceMaxDataDisks
            // 
            this.lblSourceMaxDataDisks.AutoSize = true;
            this.lblSourceMaxDataDisks.Location = new System.Drawing.Point(89, 87);
            this.lblSourceMaxDataDisks.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSourceMaxDataDisks.Name = "lblSourceMaxDataDisks";
            this.lblSourceMaxDataDisks.Size = new System.Drawing.Size(13, 13);
            this.lblSourceMaxDataDisks.TabIndex = 39;
            this.lblSourceMaxDataDisks.Text = "0";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(7, 87);
            this.label13.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(85, 13);
            this.label13.TabIndex = 38;
            this.label13.Text = "Max Data Disks:";
            // 
            // lblSourceMemoryInGb
            // 
            this.lblSourceMemoryInGb.AutoSize = true;
            this.lblSourceMemoryInGb.Location = new System.Drawing.Point(89, 70);
            this.lblSourceMemoryInGb.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSourceMemoryInGb.Name = "lblSourceMemoryInGb";
            this.lblSourceMemoryInGb.Size = new System.Drawing.Size(13, 13);
            this.lblSourceMemoryInGb.TabIndex = 37;
            this.lblSourceMemoryInGb.Text = "0";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(7, 70);
            this.label16.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(83, 13);
            this.label16.TabIndex = 36;
            this.label16.Text = "Memory (In GB):";
            // 
            // VirtualMachineProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblSourceMaxDataDisks);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.lblSourceMemoryInGb);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.lblSourceCPUCores);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.lblTargetLocationRequired);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lblTargetMaxDataDisks);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblTargetMemoryInGb);
            this.Controls.Add(this.lblTargetNumberOfCores);
            this.Controls.Add(this.cbRoleSizes);
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
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "VirtualMachineProperties";
            this.Size = new System.Drawing.Size(312, 687);
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
        private System.Windows.Forms.ComboBox cbRoleSizes;
        private System.Windows.Forms.Label lblTargetNumberOfCores;
        private System.Windows.Forms.Label lblTargetMemoryInGb;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblTargetMaxDataDisks;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblTargetLocationRequired;
        private System.Windows.Forms.Label lblSourceCPUCores;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lblSourceMaxDataDisks;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label lblSourceMemoryInGb;
        private System.Windows.Forms.Label label16;
    }
}
