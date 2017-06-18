namespace MigAz.Azure.UserControls
{
    partial class StorageAccountProperties
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
            this.lblAccountType = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblSourceASMName = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbAccountType = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtTargetName
            // 
            this.txtTargetName.CharacterCasing = System.Windows.Forms.CharacterCasing.Lower;
            this.txtTargetName.Location = new System.Drawing.Point(115, 49);
            this.txtTargetName.Margin = new System.Windows.Forms.Padding(2);
            this.txtTargetName.MaxLength = 24;
            this.txtTargetName.Name = "txtTargetName";
            this.txtTargetName.Size = new System.Drawing.Size(197, 20);
            this.txtTargetName.TabIndex = 7;
            this.txtTargetName.TextChanged += new System.EventHandler(this.txtTargetName_TextChanged);
            this.txtTargetName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTargetName_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(2, 52);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Target Name:";
            // 
            // lblAccountType
            // 
            this.lblAccountType.AutoSize = true;
            this.lblAccountType.Location = new System.Drawing.Point(115, 26);
            this.lblAccountType.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblAccountType.Name = "lblAccountType";
            this.lblAccountType.Size = new System.Drawing.Size(35, 13);
            this.lblAccountType.TabIndex = 5;
            this.lblAccountType.Text = "label2";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 27);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Source Type:";
            // 
            // lblSourceASMName
            // 
            this.lblSourceASMName.AutoSize = true;
            this.lblSourceASMName.Location = new System.Drawing.Point(115, 6);
            this.lblSourceASMName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSourceASMName.Name = "lblSourceASMName";
            this.lblSourceASMName.Size = new System.Drawing.Size(58, 13);
            this.lblSourceASMName.TabIndex = 9;
            this.lblSourceASMName.Text = "ASMName";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(2, 6);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Source Name:";
            // 
            // cmbAccountType
            // 
            this.cmbAccountType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAccountType.FormattingEnabled = true;
            this.cmbAccountType.Items.AddRange(new object[] {
            "Standard_LRS",
            "Standard_ZRS",
            "Standard_GRS",
            "Standard_RAGRS",
            "Premium_LRS"});
            this.cmbAccountType.Location = new System.Drawing.Point(115, 74);
            this.cmbAccountType.Name = "cmbAccountType";
            this.cmbAccountType.Size = new System.Drawing.Size(197, 21);
            this.cmbAccountType.TabIndex = 10;
            this.cmbAccountType.SelectedIndexChanged += new System.EventHandler(this.cmbAccountType_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(2, 77);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Target Type:";
            // 
            // StorageAccountProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmbAccountType);
            this.Controls.Add(this.lblSourceASMName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtTargetName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblAccountType);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "StorageAccountProperties";
            this.Size = new System.Drawing.Size(312, 126);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtTargetName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblAccountType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblSourceASMName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbAccountType;
        private System.Windows.Forms.Label label3;
    }
}
