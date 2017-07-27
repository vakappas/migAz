namespace MigAz.Migrators
{
    partial class AwsToArm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AwsToArm));
            this.azureLoginContextViewer21 = new MigAz.Azure.UserControls.AzureLoginContextViewer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.secretKeyTextBox = new System.Windows.Forms.TextBox();
            this.accessKeyTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.treeSourceAWS = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.treeTargetARM = new MigAz.Azure.UserControls.TargetTreeView();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // azureLoginContextViewer21
            // 
            this.azureLoginContextViewer21.ChangeType = MigAz.Azure.UserControls.AzureLoginChangeType.Full;
            this.azureLoginContextViewer21.Location = new System.Drawing.Point(370, 8);
            this.azureLoginContextViewer21.Margin = new System.Windows.Forms.Padding(1);
            this.azureLoginContextViewer21.Name = "azureLoginContextViewer21";
            this.azureLoginContextViewer21.Size = new System.Drawing.Size(447, 110);
            this.azureLoginContextViewer21.TabIndex = 53;
            this.azureLoginContextViewer21.Title = "Azure Subscription";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnLogin);
            this.groupBox1.Controls.Add(this.secretKeyTextBox);
            this.groupBox1.Controls.Add(this.accessKeyTextBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Location = new System.Drawing.Point(2, 2);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(360, 107);
            this.groupBox1.TabIndex = 57;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "AWS Account";
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(150, 80);
            this.btnLogin.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(86, 23);
            this.btnLogin.TabIndex = 22;
            this.btnLogin.Text = "Log In";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // secretKeyTextBox
            // 
            this.secretKeyTextBox.Location = new System.Drawing.Point(150, 51);
            this.secretKeyTextBox.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.secretKeyTextBox.Name = "secretKeyTextBox";
            this.secretKeyTextBox.PasswordChar = '*';
            this.secretKeyTextBox.Size = new System.Drawing.Size(200, 20);
            this.secretKeyTextBox.TabIndex = 26;
            // 
            // accessKeyTextBox
            // 
            this.accessKeyTextBox.AllowDrop = true;
            this.accessKeyTextBox.Location = new System.Drawing.Point(150, 24);
            this.accessKeyTextBox.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.accessKeyTextBox.Name = "accessKeyTextBox";
            this.accessKeyTextBox.Size = new System.Drawing.Size(200, 20);
            this.accessKeyTextBox.TabIndex = 25;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 51);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(125, 13);
            this.label2.TabIndex = 24;
            this.label2.Text = "AWS Secret Access Key";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(22, 24);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(105, 13);
            this.label5.TabIndex = 23;
            this.label5.Text = "AWS Access Key ID";
            // 
            // treeSourceAWS
            // 
            this.treeSourceAWS.CheckBoxes = true;
            this.treeSourceAWS.ImageIndex = 0;
            this.treeSourceAWS.ImageList = this.imageList1;
            this.treeSourceAWS.Location = new System.Drawing.Point(2, 125);
            this.treeSourceAWS.Margin = new System.Windows.Forms.Padding(2);
            this.treeSourceAWS.Name = "treeSourceAWS";
            this.treeSourceAWS.SelectedImageIndex = 0;
            this.treeSourceAWS.Size = new System.Drawing.Size(362, 251);
            this.treeSourceAWS.TabIndex = 58;
            this.treeSourceAWS.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeSourceAWS_AfterCheck);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Disk");
            this.imageList1.Images.SetKeyName(1, "LoadBalancer");
            this.imageList1.Images.SetKeyName(2, "NetworkInterface");
            this.imageList1.Images.SetKeyName(3, "NetworkSecurityGroup");
            this.imageList1.Images.SetKeyName(4, "PublicIp");
            this.imageList1.Images.SetKeyName(5, "StorageAccount");
            this.imageList1.Images.SetKeyName(6, "VirtualMachine");
            this.imageList1.Images.SetKeyName(7, "VirtualNetwork");
            this.imageList1.Images.SetKeyName(8, "Subscription");
            this.imageList1.Images.SetKeyName(9, "ResourceGroup");
            this.imageList1.Images.SetKeyName(10, "AvailabilitySet");
            // 
            // treeTargetARM
            // 
            this.treeTargetARM.ImageList = null;
            this.treeTargetARM.Location = new System.Drawing.Point(370, 125);
            this.treeTargetARM.LogProvider = null;
            this.treeTargetARM.Name = "treeTargetARM";
            this.treeTargetARM.PropertyPanel = null;
            this.treeTargetARM.SelectedNode = null;
            this.treeTargetARM.SettingsProvider = null;
            this.treeTargetARM.Size = new System.Drawing.Size(447, 300);
            this.treeTargetARM.StatusProvider = null;
            this.treeTargetARM.TabIndex = 59;
            // 
            // AwsToArm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.treeTargetARM);
            this.Controls.Add(this.treeSourceAWS);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.azureLoginContextViewer21);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "AwsToArm";
            this.Size = new System.Drawing.Size(909, 537);
            this.Load += new System.EventHandler(this.AwsToArm_Load);
            this.Resize += new System.EventHandler(this.AwsToArm_Resize);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private Azure.UserControls.AzureLoginContextViewer azureLoginContextViewer21;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.TextBox secretKeyTextBox;
        private System.Windows.Forms.TextBox accessKeyTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TreeView treeSourceAWS;
        private System.Windows.Forms.ImageList imageList1;
        private Azure.UserControls.TargetTreeView treeTargetARM;
    }
}
