namespace MigAz.Azure.UserControls
{
    partial class VirtualNetworkProperties
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
            this.lblVNetName = new System.Windows.Forms.Label();
            this.lblASMVirtualNetworkName = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtVirtualNetworkName = new System.Windows.Forms.TextBox();
            this.lblARMVirtualNetworkName = new System.Windows.Forms.Label();
            this.dgvAddressSpaces = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAddressSpaces)).BeginInit();
            this.SuspendLayout();
            // 
            // lblVNetName
            // 
            this.lblVNetName.AutoSize = true;
            this.lblVNetName.Location = new System.Drawing.Point(201, 24);
            this.lblVNetName.Name = "lblVNetName";
            this.lblVNetName.Size = new System.Drawing.Size(121, 25);
            this.lblVNetName.TabIndex = 7;
            this.lblVNetName.Text = "VNet Name";
            // 
            // lblASMVirtualNetworkName
            // 
            this.lblASMVirtualNetworkName.AutoSize = true;
            this.lblASMVirtualNetworkName.Location = new System.Drawing.Point(16, 24);
            this.lblASMVirtualNetworkName.Name = "lblASMVirtualNetworkName";
            this.lblASMVirtualNetworkName.Size = new System.Drawing.Size(148, 25);
            this.lblASMVirtualNetworkName.TabIndex = 6;
            this.lblASMVirtualNetworkName.Text = "Source Name:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 112);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(164, 25);
            this.label3.TabIndex = 8;
            this.label3.Text = "Address Space:";
            // 
            // txtVirtualNetworkName
            // 
            this.txtVirtualNetworkName.Location = new System.Drawing.Point(206, 61);
            this.txtVirtualNetworkName.Name = "txtVirtualNetworkName";
            this.txtVirtualNetworkName.Size = new System.Drawing.Size(352, 31);
            this.txtVirtualNetworkName.TabIndex = 10;
            this.txtVirtualNetworkName.TextChanged += new System.EventHandler(this.txtTargetName_TextChanged);
            this.txtVirtualNetworkName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTargetName_KeyPress);
            // 
            // lblARMVirtualNetworkName
            // 
            this.lblARMVirtualNetworkName.AutoSize = true;
            this.lblARMVirtualNetworkName.Location = new System.Drawing.Point(16, 64);
            this.lblARMVirtualNetworkName.Name = "lblARMVirtualNetworkName";
            this.lblARMVirtualNetworkName.Size = new System.Drawing.Size(142, 25);
            this.lblARMVirtualNetworkName.TabIndex = 11;
            this.lblARMVirtualNetworkName.Text = "Target Name:";
            // 
            // dgvAddressSpaces
            // 
            this.dgvAddressSpaces.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAddressSpaces.Location = new System.Drawing.Point(206, 112);
            this.dgvAddressSpaces.Name = "dgvAddressSpaces";
            this.dgvAddressSpaces.RowTemplate.Height = 20;
            this.dgvAddressSpaces.Size = new System.Drawing.Size(352, 136);
            this.dgvAddressSpaces.TabIndex = 12;
            // 
            // VirtualNetworkProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dgvAddressSpaces);
            this.Controls.Add(this.lblARMVirtualNetworkName);
            this.Controls.Add(this.txtVirtualNetworkName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblVNetName);
            this.Controls.Add(this.lblASMVirtualNetworkName);
            this.Name = "VirtualNetworkProperties";
            this.Size = new System.Drawing.Size(625, 262);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAddressSpaces)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblVNetName;
        private System.Windows.Forms.Label lblASMVirtualNetworkName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtVirtualNetworkName;
        private System.Windows.Forms.Label lblARMVirtualNetworkName;
        private System.Windows.Forms.DataGridView dgvAddressSpaces;
    }
}
