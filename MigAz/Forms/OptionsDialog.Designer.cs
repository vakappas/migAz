namespace MigAz.Forms
{
    partial class OptionsDialog
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionsDialog));
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtPublicIPSuffix = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtLoadBalancerSuffix = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.lblSuffix = new System.Windows.Forms.Label();
            this.txtStorageAccountSuffix = new System.Windows.Forms.TextBox();
            this.txtVirtualNetworkSuffix = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtVirtualNetworkGatewaySuffix = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtNetworkSecurityGroupSuffix = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtResourceGroupSuffix = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnApplyDefaultNaming = new System.Windows.Forms.Button();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.txtAvailabilitySetSuffix = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtVirtualMachineSuffix = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtNetworkInterfaceCardSuffix = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkSaveSelection = new System.Windows.Forms.CheckBox();
            this.chkAutoSelectDependencies = new System.Windows.Forms.CheckBox();
            this.chkBuildEmpty = new System.Windows.Forms.CheckBox();
            this.chkAllowTelemetry = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(842, 754);
            this.btnOK.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(149, 44);
            this.btnOK.TabIndex = 15;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(1016, 754);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(149, 44);
            this.btnCancel.TabIndex = 16;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtPublicIPSuffix);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.txtLoadBalancerSuffix);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.lblSuffix);
            this.groupBox1.Controls.Add(this.txtStorageAccountSuffix);
            this.groupBox1.Controls.Add(this.txtVirtualNetworkSuffix);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtVirtualNetworkGatewaySuffix);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtNetworkSecurityGroupSuffix);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.txtResourceGroupSuffix);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.btnApplyDefaultNaming);
            this.groupBox1.Controls.Add(this.linkLabel1);
            this.groupBox1.Controls.Add(this.txtAvailabilitySetSuffix);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtVirtualMachineSuffix);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtNetworkInterfaceCardSuffix);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(26, 314);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1139, 416);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Azure Resource Manager (ARM) Object Naming Suffixes";
            // 
            // txtPublicIPSuffix
            // 
            this.txtPublicIPSuffix.Location = new System.Drawing.Point(325, 326);
            this.txtPublicIPSuffix.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.txtPublicIPSuffix.MaxLength = 10;
            this.txtPublicIPSuffix.Name = "txtPublicIPSuffix";
            this.txtPublicIPSuffix.Size = new System.Drawing.Size(76, 31);
            this.txtPublicIPSuffix.TabIndex = 9;
            this.txtPublicIPSuffix.Text = "-pip";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(62, 326);
            this.label9.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(102, 25);
            this.label9.TabIndex = 23;
            this.label9.Text = "Public IP:";
            // 
            // txtLoadBalancerSuffix
            // 
            this.txtLoadBalancerSuffix.Location = new System.Drawing.Point(948, 127);
            this.txtLoadBalancerSuffix.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.txtLoadBalancerSuffix.MaxLength = 10;
            this.txtLoadBalancerSuffix.Name = "txtLoadBalancerSuffix";
            this.txtLoadBalancerSuffix.Size = new System.Drawing.Size(76, 31);
            this.txtLoadBalancerSuffix.TabIndex = 10;
            this.txtLoadBalancerSuffix.Text = "-lb";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(701, 127);
            this.label8.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(157, 25);
            this.label8.TabIndex = 21;
            this.label8.Text = "Load Balancer:";
            // 
            // lblSuffix
            // 
            this.lblSuffix.AutoSize = true;
            this.lblSuffix.Location = new System.Drawing.Point(62, 285);
            this.lblSuffix.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblSuffix.Name = "lblSuffix";
            this.lblSuffix.Size = new System.Drawing.Size(177, 25);
            this.lblSuffix.TabIndex = 19;
            this.lblSuffix.Text = "Storage Account:";
            // 
            // txtStorageAccountSuffix
            // 
            this.txtStorageAccountSuffix.CharacterCasing = System.Windows.Forms.CharacterCasing.Lower;
            this.txtStorageAccountSuffix.Location = new System.Drawing.Point(325, 282);
            this.txtStorageAccountSuffix.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.txtStorageAccountSuffix.MaxLength = 10;
            this.txtStorageAccountSuffix.Name = "txtStorageAccountSuffix";
            this.txtStorageAccountSuffix.Size = new System.Drawing.Size(76, 31);
            this.txtStorageAccountSuffix.TabIndex = 8;
            this.txtStorageAccountSuffix.Text = "v2";
            // 
            // txtVirtualNetworkSuffix
            // 
            this.txtVirtualNetworkSuffix.Location = new System.Drawing.Point(325, 166);
            this.txtVirtualNetworkSuffix.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.txtVirtualNetworkSuffix.MaxLength = 10;
            this.txtVirtualNetworkSuffix.Name = "txtVirtualNetworkSuffix";
            this.txtVirtualNetworkSuffix.Size = new System.Drawing.Size(76, 31);
            this.txtVirtualNetworkSuffix.TabIndex = 5;
            this.txtVirtualNetworkSuffix.Text = "-vnet";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(62, 168);
            this.label5.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(163, 25);
            this.label5.TabIndex = 18;
            this.label5.Text = "Virtual Network:";
            // 
            // txtVirtualNetworkGatewaySuffix
            // 
            this.txtVirtualNetworkGatewaySuffix.Location = new System.Drawing.Point(325, 206);
            this.txtVirtualNetworkGatewaySuffix.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.txtVirtualNetworkGatewaySuffix.MaxLength = 10;
            this.txtVirtualNetworkGatewaySuffix.Name = "txtVirtualNetworkGatewaySuffix";
            this.txtVirtualNetworkGatewaySuffix.Size = new System.Drawing.Size(76, 31);
            this.txtVirtualNetworkGatewaySuffix.TabIndex = 6;
            this.txtVirtualNetworkGatewaySuffix.Text = "-gw";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(62, 206);
            this.label6.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(253, 25);
            this.label6.TabIndex = 16;
            this.label6.Text = "Virtual Network Gateway:";
            // 
            // txtNetworkSecurityGroupSuffix
            // 
            this.txtNetworkSecurityGroupSuffix.Location = new System.Drawing.Point(325, 244);
            this.txtNetworkSecurityGroupSuffix.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.txtNetworkSecurityGroupSuffix.MaxLength = 10;
            this.txtNetworkSecurityGroupSuffix.Name = "txtNetworkSecurityGroupSuffix";
            this.txtNetworkSecurityGroupSuffix.Size = new System.Drawing.Size(76, 31);
            this.txtNetworkSecurityGroupSuffix.TabIndex = 7;
            this.txtNetworkSecurityGroupSuffix.Text = "-nsg";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(62, 244);
            this.label7.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(245, 25);
            this.label7.TabIndex = 14;
            this.label7.Text = "Network Security Group:";
            // 
            // txtResourceGroupSuffix
            // 
            this.txtResourceGroupSuffix.Location = new System.Drawing.Point(325, 123);
            this.txtResourceGroupSuffix.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.txtResourceGroupSuffix.MaxLength = 10;
            this.txtResourceGroupSuffix.Name = "txtResourceGroupSuffix";
            this.txtResourceGroupSuffix.Size = new System.Drawing.Size(76, 31);
            this.txtResourceGroupSuffix.TabIndex = 4;
            this.txtResourceGroupSuffix.Text = "-rg";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(60, 128);
            this.label4.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(175, 25);
            this.label4.TabIndex = 11;
            this.label4.Text = "Resource Group:";
            // 
            // btnApplyDefaultNaming
            // 
            this.btnApplyDefaultNaming.Location = new System.Drawing.Point(706, 339);
            this.btnApplyDefaultNaming.Name = "btnApplyDefaultNaming";
            this.btnApplyDefaultNaming.Size = new System.Drawing.Size(414, 50);
            this.btnApplyDefaultNaming.TabIndex = 14;
            this.btnApplyDefaultNaming.Text = "Apply Default Naming Conventions";
            this.btnApplyDefaultNaming.UseVisualStyleBackColor = true;
            this.btnApplyDefaultNaming.Click += new System.EventHandler(this.btnApplyDefaultNaming_Click);
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(137, 51);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(770, 25);
            this.linkLabel1.TabIndex = 4;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "https://docs.microsoft.com/en-us/azure/guidance/guidance-naming-conventions";
            // 
            // txtAvailabilitySetSuffix
            // 
            this.txtAvailabilitySetSuffix.Location = new System.Drawing.Point(948, 164);
            this.txtAvailabilitySetSuffix.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.txtAvailabilitySetSuffix.MaxLength = 10;
            this.txtAvailabilitySetSuffix.Name = "txtAvailabilitySetSuffix";
            this.txtAvailabilitySetSuffix.Size = new System.Drawing.Size(76, 31);
            this.txtAvailabilitySetSuffix.TabIndex = 11;
            this.txtAvailabilitySetSuffix.Text = "-as";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(701, 166);
            this.label3.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(159, 25);
            this.label3.TabIndex = 10;
            this.label3.Text = "Availability Set:";
            // 
            // txtVirtualMachineSuffix
            // 
            this.txtVirtualMachineSuffix.Location = new System.Drawing.Point(948, 204);
            this.txtVirtualMachineSuffix.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.txtVirtualMachineSuffix.MaxLength = 10;
            this.txtVirtualMachineSuffix.Name = "txtVirtualMachineSuffix";
            this.txtVirtualMachineSuffix.Size = new System.Drawing.Size(76, 31);
            this.txtVirtualMachineSuffix.TabIndex = 12;
            this.txtVirtualMachineSuffix.Text = "-vm";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(701, 204);
            this.label2.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(167, 25);
            this.label2.TabIndex = 8;
            this.label2.Text = "Virtual Machine:";
            // 
            // txtNetworkInterfaceCardSuffix
            // 
            this.txtNetworkInterfaceCardSuffix.Location = new System.Drawing.Point(948, 244);
            this.txtNetworkInterfaceCardSuffix.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.txtNetworkInterfaceCardSuffix.MaxLength = 10;
            this.txtNetworkInterfaceCardSuffix.Name = "txtNetworkInterfaceCardSuffix";
            this.txtNetworkInterfaceCardSuffix.Size = new System.Drawing.Size(76, 31);
            this.txtNetworkInterfaceCardSuffix.TabIndex = 13;
            this.txtNetworkInterfaceCardSuffix.Text = "-nic";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(701, 244);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(237, 25);
            this.label1.TabIndex = 6;
            this.label1.Text = "Network Interface Card:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkSaveSelection);
            this.groupBox2.Controls.Add(this.chkAutoSelectDependencies);
            this.groupBox2.Controls.Add(this.chkBuildEmpty);
            this.groupBox2.Controls.Add(this.chkAllowTelemetry);
            this.groupBox2.Location = new System.Drawing.Point(26, 32);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1139, 257);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "MigAz Options";
            // 
            // chkSaveSelection
            // 
            this.chkSaveSelection.AutoSize = true;
            this.chkSaveSelection.Location = new System.Drawing.Point(52, 94);
            this.chkSaveSelection.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.chkSaveSelection.Name = "chkSaveSelection";
            this.chkSaveSelection.Size = new System.Drawing.Size(185, 29);
            this.chkSaveSelection.TabIndex = 2;
            this.chkSaveSelection.Text = "Save selection";
            this.chkSaveSelection.UseVisualStyleBackColor = true;
            // 
            // chkAutoSelectDependencies
            // 
            this.chkAutoSelectDependencies.AutoSize = true;
            this.chkAutoSelectDependencies.Location = new System.Drawing.Point(52, 54);
            this.chkAutoSelectDependencies.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.chkAutoSelectDependencies.Name = "chkAutoSelectDependencies";
            this.chkAutoSelectDependencies.Size = new System.Drawing.Size(432, 29);
            this.chkAutoSelectDependencies.TabIndex = 1;
            this.chkAutoSelectDependencies.Text = "Auto select dependencies (for VMs only)";
            this.chkAutoSelectDependencies.UseVisualStyleBackColor = true;
            // 
            // chkBuildEmpty
            // 
            this.chkBuildEmpty.AutoSize = true;
            this.chkBuildEmpty.Checked = true;
            this.chkBuildEmpty.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBuildEmpty.Location = new System.Drawing.Point(52, 134);
            this.chkBuildEmpty.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.chkBuildEmpty.Name = "chkBuildEmpty";
            this.chkBuildEmpty.Size = new System.Drawing.Size(280, 29);
            this.chkBuildEmpty.TabIndex = 0;
            this.chkBuildEmpty.Text = "Build empty environment";
            this.chkBuildEmpty.UseVisualStyleBackColor = true;
            // 
            // chkAllowTelemetry
            // 
            this.chkAllowTelemetry.AutoSize = true;
            this.chkAllowTelemetry.Checked = true;
            this.chkAllowTelemetry.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAllowTelemetry.Location = new System.Drawing.Point(52, 174);
            this.chkAllowTelemetry.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.chkAllowTelemetry.Name = "chkAllowTelemetry";
            this.chkAllowTelemetry.Size = new System.Drawing.Size(286, 29);
            this.chkAllowTelemetry.TabIndex = 3;
            this.chkAllowTelemetry.Text = "Allow telemetry collection";
            this.chkAllowTelemetry.UseVisualStyleBackColor = true;
            this.chkAllowTelemetry.CheckedChanged += new System.EventHandler(this.chkAllowTelemetry_CheckedChanged);
            // 
            // OptionsDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(1180, 822);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionsDialog";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.Load += new System.EventHandler(this.formOptions_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtNetworkInterfaceCardSuffix;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkSaveSelection;
        private System.Windows.Forms.CheckBox chkAutoSelectDependencies;
        private System.Windows.Forms.CheckBox chkBuildEmpty;
        private System.Windows.Forms.CheckBox chkAllowTelemetry;
        private System.Windows.Forms.TextBox txtVirtualMachineSuffix;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtAvailabilitySetSuffix;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnApplyDefaultNaming;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.TextBox txtPublicIPSuffix;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtLoadBalancerSuffix;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblSuffix;
        private System.Windows.Forms.TextBox txtStorageAccountSuffix;
        private System.Windows.Forms.TextBox txtVirtualNetworkSuffix;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtVirtualNetworkGatewaySuffix;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtNetworkSecurityGroupSuffix;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtResourceGroupSuffix;
        private System.Windows.Forms.Label label4;
    }
}