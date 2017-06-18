namespace MigAz.Azure.UserControls
{
    partial class AzureLoginContextViewer
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
            this.groupSubscription = new System.Windows.Forms.GroupBox();
            this.lblTenantName = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblSourceSubscriptionId = new System.Windows.Forms.Label();
            this.lblSourceSubscriptionName = new System.Windows.Forms.Label();
            this.lblSourceUser = new System.Windows.Forms.Label();
            this.lblSourceEnvironment = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnAzureContext = new System.Windows.Forms.Button();
            this.groupSubscription.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupSubscription
            // 
            this.groupSubscription.Controls.Add(this.lblTenantName);
            this.groupSubscription.Controls.Add(this.label6);
            this.groupSubscription.Controls.Add(this.lblSourceSubscriptionId);
            this.groupSubscription.Controls.Add(this.lblSourceSubscriptionName);
            this.groupSubscription.Controls.Add(this.lblSourceUser);
            this.groupSubscription.Controls.Add(this.lblSourceEnvironment);
            this.groupSubscription.Controls.Add(this.label4);
            this.groupSubscription.Controls.Add(this.label3);
            this.groupSubscription.Controls.Add(this.label2);
            this.groupSubscription.Controls.Add(this.label1);
            this.groupSubscription.Controls.Add(this.btnAzureContext);
            this.groupSubscription.Location = new System.Drawing.Point(3, 3);
            this.groupSubscription.Name = "groupSubscription";
            this.groupSubscription.Size = new System.Drawing.Size(883, 190);
            this.groupSubscription.TabIndex = 1;
            this.groupSubscription.TabStop = false;
            this.groupSubscription.Text = "Azure Subscription";
            // 
            // lblTenantName
            // 
            this.lblTenantName.AutoSize = true;
            this.lblTenantName.Location = new System.Drawing.Point(224, 92);
            this.lblTenantName.Name = "lblTenantName";
            this.lblTenantName.Size = new System.Drawing.Size(19, 25);
            this.lblTenantName.TabIndex = 10;
            this.lblTenantName.Text = "-";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(22, 92);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(85, 25);
            this.label6.TabIndex = 9;
            this.label6.Text = "Tenant:";
            // 
            // lblSourceSubscriptionId
            // 
            this.lblSourceSubscriptionId.AutoSize = true;
            this.lblSourceSubscriptionId.Location = new System.Drawing.Point(224, 146);
            this.lblSourceSubscriptionId.Name = "lblSourceSubscriptionId";
            this.lblSourceSubscriptionId.Size = new System.Drawing.Size(19, 25);
            this.lblSourceSubscriptionId.TabIndex = 8;
            this.lblSourceSubscriptionId.Text = "-";
            // 
            // lblSourceSubscriptionName
            // 
            this.lblSourceSubscriptionName.AutoSize = true;
            this.lblSourceSubscriptionName.Location = new System.Drawing.Point(224, 119);
            this.lblSourceSubscriptionName.Name = "lblSourceSubscriptionName";
            this.lblSourceSubscriptionName.Size = new System.Drawing.Size(19, 25);
            this.lblSourceSubscriptionName.TabIndex = 7;
            this.lblSourceSubscriptionName.Text = "-";
            // 
            // lblSourceUser
            // 
            this.lblSourceUser.AutoSize = true;
            this.lblSourceUser.Location = new System.Drawing.Point(224, 65);
            this.lblSourceUser.Name = "lblSourceUser";
            this.lblSourceUser.Size = new System.Drawing.Size(19, 25);
            this.lblSourceUser.TabIndex = 6;
            this.lblSourceUser.Text = "-";
            // 
            // lblSourceEnvironment
            // 
            this.lblSourceEnvironment.AutoSize = true;
            this.lblSourceEnvironment.Location = new System.Drawing.Point(224, 38);
            this.lblSourceEnvironment.Name = "lblSourceEnvironment";
            this.lblSourceEnvironment.Size = new System.Drawing.Size(19, 25);
            this.lblSourceEnvironment.TabIndex = 5;
            this.lblSourceEnvironment.Text = "-";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(22, 146);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(160, 25);
            this.label4.TabIndex = 4;
            this.label4.Text = "Subscription Id:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 119);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(199, 25);
            this.label3.TabIndex = 3;
            this.label3.Text = "Subscription Name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 25);
            this.label2.TabIndex = 2;
            this.label2.Text = "User:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(138, 25);
            this.label1.TabIndex = 1;
            this.label1.Text = "Environment:";
            // 
            // btnAzureContext
            // 
            this.btnAzureContext.Location = new System.Drawing.Point(736, 23);
            this.btnAzureContext.Name = "btnAzureContext";
            this.btnAzureContext.Size = new System.Drawing.Size(141, 40);
            this.btnAzureContext.TabIndex = 0;
            this.btnAzureContext.Text = "Change";
            this.btnAzureContext.UseVisualStyleBackColor = true;
            this.btnAzureContext.Click += new System.EventHandler(this.btnAzureContext_Click);
            // 
            // AzureLoginContextViewer2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupSubscription);
            this.Name = "AzureLoginContextViewer2";
            this.Size = new System.Drawing.Size(894, 211);
            this.EnabledChanged += new System.EventHandler(this.AzureLoginContextViewer_EnabledChanged);
            this.groupSubscription.ResumeLayout(false);
            this.groupSubscription.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupSubscription;
        private System.Windows.Forms.Label lblSourceSubscriptionId;
        private System.Windows.Forms.Label lblSourceSubscriptionName;
        private System.Windows.Forms.Label lblSourceUser;
        private System.Windows.Forms.Label lblSourceEnvironment;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnAzureContext;
        private System.Windows.Forms.Label lblTenantName;
        private System.Windows.Forms.Label label6;
    }
}
