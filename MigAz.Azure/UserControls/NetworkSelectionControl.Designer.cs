namespace MigAz.Azure.UserControls
{
    partial class NetworkSelectionControl
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
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.rbExistingARMVNet = new System.Windows.Forms.RadioButton();
            this.rbVNetInMigration = new System.Windows.Forms.RadioButton();
            this.cmbExistingArmSubnet = new System.Windows.Forms.ComboBox();
            this.cmbExistingArmVNets = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(2, 82);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(78, 13);
            this.label8.TabIndex = 36;
            this.label8.Text = "Target Subnet:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(2, 54);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(68, 13);
            this.label7.TabIndex = 35;
            this.label7.Text = "Target VNet:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(2, 2);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 34;
            this.label2.Text = "Migrate To:";
            // 
            // rbExistingARMVNet
            // 
            this.rbExistingARMVNet.AutoSize = true;
            this.rbExistingARMVNet.Location = new System.Drawing.Point(87, 25);
            this.rbExistingARMVNet.Margin = new System.Windows.Forms.Padding(2);
            this.rbExistingARMVNet.Name = "rbExistingARMVNet";
            this.rbExistingARMVNet.Size = new System.Drawing.Size(160, 17);
            this.rbExistingARMVNet.TabIndex = 31;
            this.rbExistingARMVNet.Text = "Existing ARM VNet in Target";
            this.rbExistingARMVNet.UseVisualStyleBackColor = true;
            this.rbExistingARMVNet.CheckedChanged += new System.EventHandler(this.rbExistingARMVNet_CheckedChanged);
            // 
            // rbVNetInMigration
            // 
            this.rbVNetInMigration.AutoSize = true;
            this.rbVNetInMigration.Location = new System.Drawing.Point(87, 2);
            this.rbVNetInMigration.Margin = new System.Windows.Forms.Padding(2);
            this.rbVNetInMigration.Name = "rbVNetInMigration";
            this.rbVNetInMigration.Size = new System.Drawing.Size(138, 17);
            this.rbVNetInMigration.TabIndex = 30;
            this.rbVNetInMigration.Text = "VNet in MigAz Migration";
            this.rbVNetInMigration.UseVisualStyleBackColor = true;
            this.rbVNetInMigration.CheckedChanged += new System.EventHandler(this.rbVNetInMigration_CheckedChanged);
            // 
            // cmbExistingArmSubnet
            // 
            this.cmbExistingArmSubnet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbExistingArmSubnet.FormattingEnabled = true;
            this.cmbExistingArmSubnet.Location = new System.Drawing.Point(87, 78);
            this.cmbExistingArmSubnet.Margin = new System.Windows.Forms.Padding(2);
            this.cmbExistingArmSubnet.Name = "cmbExistingArmSubnet";
            this.cmbExistingArmSubnet.Size = new System.Drawing.Size(216, 21);
            this.cmbExistingArmSubnet.TabIndex = 33;
            this.cmbExistingArmSubnet.SelectedIndexChanged += new System.EventHandler(this.cmbExistingArmSubnet_SelectedIndexChanged);
            // 
            // cmbExistingArmVNets
            // 
            this.cmbExistingArmVNets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbExistingArmVNets.FormattingEnabled = true;
            this.cmbExistingArmVNets.Location = new System.Drawing.Point(87, 52);
            this.cmbExistingArmVNets.Margin = new System.Windows.Forms.Padding(2);
            this.cmbExistingArmVNets.Name = "cmbExistingArmVNets";
            this.cmbExistingArmVNets.Size = new System.Drawing.Size(216, 21);
            this.cmbExistingArmVNets.TabIndex = 32;
            this.cmbExistingArmVNets.SelectedIndexChanged += new System.EventHandler(this.cmbExistingArmVNets_SelectedIndexChanged);
            // 
            // NetworkSelectionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.rbExistingARMVNet);
            this.Controls.Add(this.rbVNetInMigration);
            this.Controls.Add(this.cmbExistingArmSubnet);
            this.Controls.Add(this.cmbExistingArmVNets);
            this.Name = "NetworkSelectionControl";
            this.Size = new System.Drawing.Size(310, 105);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rbExistingARMVNet;
        private System.Windows.Forms.RadioButton rbVNetInMigration;
        private System.Windows.Forms.ComboBox cmbExistingArmSubnet;
        private System.Windows.Forms.ComboBox cmbExistingArmVNets;
    }
}
