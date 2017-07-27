namespace MigAz.Azure.UserControls
{
    partial class DiskProperties
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
            this.lblDiskName = new System.Windows.Forms.Label();
            this.lblHostCaching = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblLUN = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.rbExistingARMStorageAccount = new System.Windows.Forms.RadioButton();
            this.rbStorageAccountInMigration = new System.Windows.Forms.RadioButton();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbTargetStorage = new System.Windows.Forms.ComboBox();
            this.lblAsmStorageAccount = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtTargetDiskName = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.rbManagedDisk = new System.Windows.Forms.RadioButton();
            this.label10 = new System.Windows.Forms.Label();
            this.txtBlobName = new System.Windows.Forms.TextBox();
            this.lblSourceSizeGb = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.txtTargetSize = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 27);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Disk Name:";
            // 
            // lblDiskName
            // 
            this.lblDiskName.AutoSize = true;
            this.lblDiskName.Location = new System.Drawing.Point(103, 27);
            this.lblDiskName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblDiskName.Name = "lblDiskName";
            this.lblDiskName.Size = new System.Drawing.Size(77, 13);
            this.lblDiskName.TabIndex = 1;
            this.lblDiskName.Text = "DiskName.vhd";
            // 
            // lblHostCaching
            // 
            this.lblHostCaching.AutoSize = true;
            this.lblHostCaching.Location = new System.Drawing.Point(103, 46);
            this.lblHostCaching.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblHostCaching.Name = "lblHostCaching";
            this.lblHostCaching.Size = new System.Drawing.Size(33, 13);
            this.lblHostCaching.TabIndex = 3;
            this.lblHostCaching.Text = "None";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 46);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Host Caching:";
            // 
            // lblLUN
            // 
            this.lblLUN.AutoSize = true;
            this.lblLUN.Location = new System.Drawing.Point(103, 65);
            this.lblLUN.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblLUN.Name = "lblLUN";
            this.lblLUN.Size = new System.Drawing.Size(13, 13);
            this.lblLUN.TabIndex = 5;
            this.lblLUN.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 65);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "LUN:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 101);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Storage Account:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 250);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(61, 13);
            this.label5.TabIndex = 17;
            this.label5.Text = "Migrate To:";
            // 
            // rbExistingARMStorageAccount
            // 
            this.rbExistingARMStorageAccount.AutoSize = true;
            this.rbExistingARMStorageAccount.Enabled = false;
            this.rbExistingARMStorageAccount.Location = new System.Drawing.Point(103, 293);
            this.rbExistingARMStorageAccount.Margin = new System.Windows.Forms.Padding(2);
            this.rbExistingARMStorageAccount.Name = "rbExistingARMStorageAccount";
            this.rbExistingARMStorageAccount.Size = new System.Drawing.Size(173, 17);
            this.rbExistingARMStorageAccount.TabIndex = 2;
            this.rbExistingARMStorageAccount.Text = "Existing ARM Storage in Target";
            this.rbExistingARMStorageAccount.UseVisualStyleBackColor = true;
            this.rbExistingARMStorageAccount.CheckedChanged += new System.EventHandler(this.rbExistingARMStorageAccount_CheckedChanged);
            // 
            // rbStorageAccountInMigration
            // 
            this.rbStorageAccountInMigration.AutoSize = true;
            this.rbStorageAccountInMigration.Location = new System.Drawing.Point(103, 271);
            this.rbStorageAccountInMigration.Margin = new System.Windows.Forms.Padding(2);
            this.rbStorageAccountInMigration.Name = "rbStorageAccountInMigration";
            this.rbStorageAccountInMigration.Size = new System.Drawing.Size(151, 17);
            this.rbStorageAccountInMigration.TabIndex = 1;
            this.rbStorageAccountInMigration.Text = "Storage in MigAz Migration";
            this.rbStorageAccountInMigration.UseVisualStyleBackColor = true;
            this.rbStorageAccountInMigration.CheckedChanged += new System.EventHandler(this.rbStorageAccountInMigration_CheckedChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(11, 317);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(81, 13);
            this.label7.TabIndex = 19;
            this.label7.Text = "Target Storage:";
            // 
            // cmbTargetStorage
            // 
            this.cmbTargetStorage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTargetStorage.Enabled = false;
            this.cmbTargetStorage.FormattingEnabled = true;
            this.cmbTargetStorage.Location = new System.Drawing.Point(103, 315);
            this.cmbTargetStorage.Margin = new System.Windows.Forms.Padding(2);
            this.cmbTargetStorage.Name = "cmbTargetStorage";
            this.cmbTargetStorage.Size = new System.Drawing.Size(198, 21);
            this.cmbTargetStorage.Sorted = true;
            this.cmbTargetStorage.TabIndex = 3;
            this.cmbTargetStorage.SelectedIndexChanged += new System.EventHandler(this.cmbTargetStorage_SelectedIndexChanged);
            // 
            // lblAsmStorageAccount
            // 
            this.lblAsmStorageAccount.AutoSize = true;
            this.lblAsmStorageAccount.Location = new System.Drawing.Point(103, 101);
            this.lblAsmStorageAccount.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblAsmStorageAccount.Name = "lblAsmStorageAccount";
            this.lblAsmStorageAccount.Size = new System.Drawing.Size(43, 13);
            this.lblAsmStorageAccount.TabIndex = 20;
            this.lblAsmStorageAccount.Text = "abc123";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(2, 0);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(76, 13);
            this.label6.TabIndex = 21;
            this.label6.Text = "Source Disk";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(5, 131);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(73, 13);
            this.label8.TabIndex = 22;
            this.label8.Text = "Target Disk";
            // 
            // txtTargetDiskName
            // 
            this.txtTargetDiskName.Location = new System.Drawing.Point(103, 154);
            this.txtTargetDiskName.Margin = new System.Windows.Forms.Padding(2);
            this.txtTargetDiskName.Name = "txtTargetDiskName";
            this.txtTargetDiskName.Size = new System.Drawing.Size(196, 20);
            this.txtTargetDiskName.TabIndex = 0;
            this.txtTargetDiskName.TextChanged += new System.EventHandler(this.txtTargetDiskName_TextChanged);
            this.txtTargetDiskName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTargetDiskName_KeyPress);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(11, 155);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(62, 13);
            this.label9.TabIndex = 24;
            this.label9.Text = "Disk Name:";
            // 
            // rbManagedDisk
            // 
            this.rbManagedDisk.AutoSize = true;
            this.rbManagedDisk.Enabled = false;
            this.rbManagedDisk.Location = new System.Drawing.Point(103, 249);
            this.rbManagedDisk.Margin = new System.Windows.Forms.Padding(2);
            this.rbManagedDisk.Name = "rbManagedDisk";
            this.rbManagedDisk.Size = new System.Drawing.Size(94, 17);
            this.rbManagedDisk.TabIndex = 25;
            this.rbManagedDisk.Text = "Managed Disk";
            this.rbManagedDisk.UseVisualStyleBackColor = true;
            this.rbManagedDisk.CheckedChanged += new System.EventHandler(this.rbManagedDIsk_CheckedChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(11, 178);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(62, 13);
            this.label10.TabIndex = 27;
            this.label10.Text = "Blob Name:";
            // 
            // txtBlobName
            // 
            this.txtBlobName.Location = new System.Drawing.Point(103, 176);
            this.txtBlobName.Margin = new System.Windows.Forms.Padding(2);
            this.txtBlobName.Name = "txtBlobName";
            this.txtBlobName.Size = new System.Drawing.Size(196, 20);
            this.txtBlobName.TabIndex = 26;
            this.txtBlobName.TextChanged += new System.EventHandler(this.txtBlobName_TextChanged);
            this.txtBlobName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTargetDiskName_KeyPress);
            // 
            // lblSourceSizeGb
            // 
            this.lblSourceSizeGb.AutoSize = true;
            this.lblSourceSizeGb.Location = new System.Drawing.Point(103, 83);
            this.lblSourceSizeGb.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSourceSizeGb.Name = "lblSourceSizeGb";
            this.lblSourceSizeGb.Size = new System.Drawing.Size(13, 13);
            this.lblSourceSizeGb.TabIndex = 29;
            this.lblSourceSizeGb.Text = "0";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(11, 83);
            this.label12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(54, 13);
            this.label12.TabIndex = 28;
            this.label12.Text = "Size (GB):";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(11, 202);
            this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(54, 13);
            this.label11.TabIndex = 31;
            this.label11.Text = "Size (GB):";
            // 
            // txtTargetSize
            // 
            this.txtTargetSize.Location = new System.Drawing.Point(103, 200);
            this.txtTargetSize.Margin = new System.Windows.Forms.Padding(2);
            this.txtTargetSize.MaxLength = 4;
            this.txtTargetSize.Name = "txtTargetSize";
            this.txtTargetSize.Size = new System.Drawing.Size(53, 20);
            this.txtTargetSize.TabIndex = 30;
            this.txtTargetSize.TextChanged += new System.EventHandler(this.txtTargetSize_TextChanged);
            // 
            // DiskProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txtTargetSize);
            this.Controls.Add(this.lblSourceSizeGb);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtBlobName);
            this.Controls.Add(this.rbManagedDisk);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtTargetDiskName);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lblAsmStorageAccount);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.cmbTargetStorage);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.rbExistingARMStorageAccount);
            this.Controls.Add(this.rbStorageAccountInMigration);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblLUN);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblHostCaching);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblDiskName);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "DiskProperties";
            this.Size = new System.Drawing.Size(320, 411);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblDiskName;
        private System.Windows.Forms.Label lblHostCaching;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblLUN;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RadioButton rbExistingARMStorageAccount;
        private System.Windows.Forms.RadioButton rbStorageAccountInMigration;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cmbTargetStorage;
        private System.Windows.Forms.Label lblAsmStorageAccount;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtTargetDiskName;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.RadioButton rbManagedDisk;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtBlobName;
        private System.Windows.Forms.Label lblSourceSizeGb;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtTargetSize;
    }
}
