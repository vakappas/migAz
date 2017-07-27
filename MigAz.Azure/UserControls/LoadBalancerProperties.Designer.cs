namespace MigAz.Azure.UserControls
{
    partial class LoadBalancerProperties
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
            this.networkSelectionControl1 = new MigAz.Azure.UserControls.NetworkSelectionControl();
            this.cmbLoadBalancerType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(2, 10);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Target Name:";
            // 
            // txtTargetName
            // 
            this.txtTargetName.Location = new System.Drawing.Point(88, 10);
            this.txtTargetName.Margin = new System.Windows.Forms.Padding(2);
            this.txtTargetName.Name = "txtTargetName";
            this.txtTargetName.Size = new System.Drawing.Size(166, 20);
            this.txtTargetName.TabIndex = 3;
            this.txtTargetName.TextChanged += new System.EventHandler(this.txtTargetName_TextChanged);
            // 
            // networkSelectionControl1
            // 
            this.networkSelectionControl1.ExistingARMVNetEnabled = true;
            this.networkSelectionControl1.Location = new System.Drawing.Point(0, 73);
            this.networkSelectionControl1.Name = "networkSelectionControl1";
            this.networkSelectionControl1.Size = new System.Drawing.Size(310, 150);
            this.networkSelectionControl1.TabIndex = 5;
            this.networkSelectionControl1.VirtualNetworkTarget = null;
            // 
            // cmbLoadBalancerType
            // 
            this.cmbLoadBalancerType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLoadBalancerType.FormattingEnabled = true;
            this.cmbLoadBalancerType.Items.AddRange(new object[] {
            "Public",
            "Internal"});
            this.cmbLoadBalancerType.Location = new System.Drawing.Point(88, 35);
            this.cmbLoadBalancerType.Name = "cmbLoadBalancerType";
            this.cmbLoadBalancerType.Size = new System.Drawing.Size(166, 21);
            this.cmbLoadBalancerType.TabIndex = 6;
            this.cmbLoadBalancerType.SelectedIndexChanged += new System.EventHandler(this.cmbLoadBalancerType_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 38);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Type:";
            // 
            // LoadBalancerProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbLoadBalancerType);
            this.Controls.Add(this.networkSelectionControl1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtTargetName);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "LoadBalancerProperties";
            this.Size = new System.Drawing.Size(310, 226);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTargetName;
        private NetworkSelectionControl networkSelectionControl1;
        private System.Windows.Forms.ComboBox cmbLoadBalancerType;
        private System.Windows.Forms.Label label1;
    }
}
