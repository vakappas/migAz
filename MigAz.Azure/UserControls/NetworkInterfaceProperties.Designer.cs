namespace MigAz.Azure.UserControls
{
    partial class NetworkInterfaceProperties
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
            this.lblVirtualNetworkName = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblStaticIpAddress = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblSubnetName = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblARMVirtualNetworkName = new System.Windows.Forms.Label();
            this.txtTargetName = new System.Windows.Forms.TextBox();
            this.lblSourceName = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbAllocationMethod = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtStaticIp = new System.Windows.Forms.TextBox();
            this.networkSelectionControl1 = new MigAz.Azure.UserControls.NetworkSelectionControl();
            this.SuspendLayout();
            // 
            // lblVirtualNetworkName
            // 
            this.lblVirtualNetworkName.AutoSize = true;
            this.lblVirtualNetworkName.Location = new System.Drawing.Point(88, 24);
            this.lblVirtualNetworkName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblVirtualNetworkName.Name = "lblVirtualNetworkName";
            this.lblVirtualNetworkName.Size = new System.Drawing.Size(62, 13);
            this.lblVirtualNetworkName.TabIndex = 26;
            this.lblVirtualNetworkName.Text = "VNet Name";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 24);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 13);
            this.label5.TabIndex = 25;
            this.label5.Text = "Source VNet:";
            // 
            // lblStaticIpAddress
            // 
            this.lblStaticIpAddress.AutoSize = true;
            this.lblStaticIpAddress.Location = new System.Drawing.Point(88, 70);
            this.lblStaticIpAddress.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblStaticIpAddress.Name = "lblStaticIpAddress";
            this.lblStaticIpAddress.Size = new System.Drawing.Size(40, 13);
            this.lblStaticIpAddress.TabIndex = 24;
            this.lblStaticIpAddress.Text = "0.0.0.0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 70);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 22;
            this.label3.Text = "Private IP:";
            // 
            // lblSubnetName
            // 
            this.lblSubnetName.AutoSize = true;
            this.lblSubnetName.Location = new System.Drawing.Point(88, 47);
            this.lblSubnetName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSubnetName.Name = "lblSubnetName";
            this.lblSubnetName.Size = new System.Drawing.Size(72, 13);
            this.lblSubnetName.TabIndex = 20;
            this.lblSubnetName.Text = "Subnet Name";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 47);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 13);
            this.label4.TabIndex = 18;
            this.label4.Text = "Source Subnet:";
            // 
            // lblARMVirtualNetworkName
            // 
            this.lblARMVirtualNetworkName.AutoSize = true;
            this.lblARMVirtualNetworkName.Location = new System.Drawing.Point(4, 122);
            this.lblARMVirtualNetworkName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblARMVirtualNetworkName.Name = "lblARMVirtualNetworkName";
            this.lblARMVirtualNetworkName.Size = new System.Drawing.Size(72, 13);
            this.lblARMVirtualNetworkName.TabIndex = 31;
            this.lblARMVirtualNetworkName.Text = "Target Name:";
            // 
            // txtTargetName
            // 
            this.txtTargetName.Location = new System.Drawing.Point(89, 122);
            this.txtTargetName.Margin = new System.Windows.Forms.Padding(2);
            this.txtTargetName.Name = "txtTargetName";
            this.txtTargetName.Size = new System.Drawing.Size(216, 20);
            this.txtTargetName.TabIndex = 30;
            this.txtTargetName.TextChanged += new System.EventHandler(this.txtTargetName_TextChanged);
            this.txtTargetName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTargetName_KeyPress);
            // 
            // lblSourceName
            // 
            this.lblSourceName.AutoSize = true;
            this.lblSourceName.Location = new System.Drawing.Point(88, 4);
            this.lblSourceName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSourceName.Name = "lblSourceName";
            this.lblSourceName.Size = new System.Drawing.Size(80, 13);
            this.lblSourceName.TabIndex = 33;
            this.lblSourceName.Text = "Interface Name";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 4);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(75, 13);
            this.label6.TabIndex = 32;
            this.label6.Text = "Source Name:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 252);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 35;
            this.label1.Text = "Allocation:";
            // 
            // cmbAllocationMethod
            // 
            this.cmbAllocationMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAllocationMethod.FormattingEnabled = true;
            this.cmbAllocationMethod.Items.AddRange(new object[] {
            "Dynamic",
            "Static"});
            this.cmbAllocationMethod.Location = new System.Drawing.Point(89, 248);
            this.cmbAllocationMethod.Margin = new System.Windows.Forms.Padding(2);
            this.cmbAllocationMethod.Name = "cmbAllocationMethod";
            this.cmbAllocationMethod.Size = new System.Drawing.Size(216, 21);
            this.cmbAllocationMethod.TabIndex = 34;
            this.cmbAllocationMethod.SelectedIndexChanged += new System.EventHandler(this.cmbAllocationMethod_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(2, 276);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(50, 13);
            this.label9.TabIndex = 37;
            this.label9.Text = "Static IP:";
            // 
            // txtStaticIp
            // 
            this.txtStaticIp.Enabled = false;
            this.txtStaticIp.Location = new System.Drawing.Point(89, 273);
            this.txtStaticIp.Margin = new System.Windows.Forms.Padding(2);
            this.txtStaticIp.MaxLength = 15;
            this.txtStaticIp.Name = "txtStaticIp";
            this.txtStaticIp.Size = new System.Drawing.Size(216, 20);
            this.txtStaticIp.TabIndex = 36;
            this.txtStaticIp.TextChanged += new System.EventHandler(this.txtStaticIp_TextChanged);
            // 
            // networkSelectionControl1
            // 
            this.networkSelectionControl1.Location = new System.Drawing.Point(2, 144);
            this.networkSelectionControl1.Name = "networkSelectionControl1";
            this.networkSelectionControl1.Size = new System.Drawing.Size(310, 105);
            this.networkSelectionControl1.TabIndex = 38;
            // 
            // NetworkInterfaceProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtStaticIp);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbAllocationMethod);
            this.Controls.Add(this.lblSourceName);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lblARMVirtualNetworkName);
            this.Controls.Add(this.txtTargetName);
            this.Controls.Add(this.lblVirtualNetworkName);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lblStaticIpAddress);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblSubnetName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.networkSelectionControl1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "NetworkInterfaceProperties";
            this.Size = new System.Drawing.Size(310, 306);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblVirtualNetworkName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblStaticIpAddress;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblSubnetName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblARMVirtualNetworkName;
        private System.Windows.Forms.TextBox txtTargetName;
        private System.Windows.Forms.Label lblSourceName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbAllocationMethod;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtStaticIp;
        private NetworkSelectionControl networkSelectionControl1;
    }
}
