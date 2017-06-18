namespace MigAz.Azure.UserControls
{
    partial class PropertyPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PropertyPanel));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pnlProperties = new System.Windows.Forms.Panel();
            this.lblAzureObjectName = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.imageList1 = new System.Windows.Forms.ImageList();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.pnlProperties);
            this.groupBox1.Controls.Add(this.lblAzureObjectName);
            this.groupBox1.Controls.Add(this.pictureBox1);
            this.groupBox1.Location = new System.Drawing.Point(2, 2);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(362, 246);
            this.groupBox1.TabIndex = 59;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Target Properties";
            this.groupBox1.Resize += new System.EventHandler(this.groupBox1_Resize);
            // 
            // pnlProperties
            // 
            this.pnlProperties.AutoScroll = true;
            this.pnlProperties.Location = new System.Drawing.Point(12, 84);
            this.pnlProperties.Margin = new System.Windows.Forms.Padding(2);
            this.pnlProperties.Name = "pnlProperties";
            this.pnlProperties.Size = new System.Drawing.Size(332, 148);
            this.pnlProperties.TabIndex = 63;
            // 
            // lblAzureObjectName
            // 
            this.lblAzureObjectName.AutoSize = true;
            this.lblAzureObjectName.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAzureObjectName.Location = new System.Drawing.Point(65, 35);
            this.lblAzureObjectName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblAzureObjectName.Name = "lblAzureObjectName";
            this.lblAzureObjectName.Size = new System.Drawing.Size(170, 21);
            this.lblAzureObjectName.TabIndex = 1;
            this.lblAzureObjectName.Text = "lblAzureObjectName";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 24);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(50, 52);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "ResourceGroup");
            this.imageList1.Images.SetKeyName(1, "Disk");
            this.imageList1.Images.SetKeyName(2, "LoadBalancer");
            this.imageList1.Images.SetKeyName(3, "NetworkInterface");
            this.imageList1.Images.SetKeyName(4, "NetworkSecurityGroup");
            this.imageList1.Images.SetKeyName(5, "PublicIp");
            this.imageList1.Images.SetKeyName(6, "StorageAccount");
            this.imageList1.Images.SetKeyName(7, "VirtualMachine");
            this.imageList1.Images.SetKeyName(8, "AvailabilitySet");
            this.imageList1.Images.SetKeyName(9, "VirtualNetwork");
            // 
            // PropertyPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "PropertyPanel";
            this.Size = new System.Drawing.Size(368, 306);
            this.Resize += new System.EventHandler(this.PropertyPanel_Resize);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblAzureObjectName;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel pnlProperties;
        private System.Windows.Forms.ImageList imageList1;
    }
}
