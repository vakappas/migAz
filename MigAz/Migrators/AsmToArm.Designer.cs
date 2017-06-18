namespace MigAz.Migrators
{
    partial class AsmToArm
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
            this.azureLoginContextViewerARM = new MigAz.Azure.UserControls.AzureLoginContextViewer();
            this.label6 = new System.Windows.Forms.Label();
            this.treeSourceASM = new System.Windows.Forms.TreeView();
            this.azureLoginContextViewerASM = new MigAz.Azure.UserControls.AzureLoginContextViewer();
            this.tabSourceResources = new System.Windows.Forms.TabControl();
            this.tabPageAsm = new System.Windows.Forms.TabPage();
            this.tabPageArm = new System.Windows.Forms.TabPage();
            this.treeSourceARM = new System.Windows.Forms.TreeView();
            this.treeTargetARM = new MigAz.Azure.UserControls.TargetTreeView();
            this.tabSourceResources.SuspendLayout();
            this.tabPageAsm.SuspendLayout();
            this.tabPageArm.SuspendLayout();
            this.SuspendLayout();
            // 
            // azureLoginContextViewerARM
            // 
            this.azureLoginContextViewerARM.ChangeType = MigAz.Azure.UserControls.AzureLoginChangeType.SubscriptionOnly;
            this.azureLoginContextViewerARM.Enabled = false;
            this.azureLoginContextViewerARM.Location = new System.Drawing.Point(452, 2);
            this.azureLoginContextViewerARM.Margin = new System.Windows.Forms.Padding(1);
            this.azureLoginContextViewerARM.Name = "azureLoginContextViewerARM";
            this.azureLoginContextViewerARM.Size = new System.Drawing.Size(447, 106);
            this.azureLoginContextViewerARM.TabIndex = 63;
            this.azureLoginContextViewerARM.Title = "Azure ARM (Target) Subscription";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(449, 109);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(125, 13);
            this.label6.TabIndex = 62;
            this.label6.Text = "Target (ARM) Resources";
            // 
            // treeSourceASM
            // 
            this.treeSourceASM.CheckBoxes = true;
            this.treeSourceASM.Location = new System.Drawing.Point(0, 0);
            this.treeSourceASM.Margin = new System.Windows.Forms.Padding(2);
            this.treeSourceASM.Name = "treeSourceASM";
            this.treeSourceASM.Size = new System.Drawing.Size(423, 262);
            this.treeSourceASM.TabIndex = 56;
            this.treeSourceASM.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeASM_AfterCheck);
            this.treeSourceASM.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeASM_AfterSelect);
            // 
            // azureLoginContextViewerASM
            // 
            this.azureLoginContextViewerASM.ChangeType = MigAz.Azure.UserControls.AzureLoginChangeType.Full;
            this.azureLoginContextViewerASM.Location = new System.Drawing.Point(2, 2);
            this.azureLoginContextViewerASM.Margin = new System.Windows.Forms.Padding(1);
            this.azureLoginContextViewerASM.Name = "azureLoginContextViewerASM";
            this.azureLoginContextViewerASM.Size = new System.Drawing.Size(447, 110);
            this.azureLoginContextViewerASM.TabIndex = 64;
            this.azureLoginContextViewerASM.Title = "Azure Subscription";
            // 
            // tabSourceResources
            // 
            this.tabSourceResources.Controls.Add(this.tabPageAsm);
            this.tabSourceResources.Controls.Add(this.tabPageArm);
            this.tabSourceResources.HotTrack = true;
            this.tabSourceResources.ImeMode = System.Windows.Forms.ImeMode.On;
            this.tabSourceResources.Location = new System.Drawing.Point(2, 109);
            this.tabSourceResources.Margin = new System.Windows.Forms.Padding(2);
            this.tabSourceResources.Name = "tabSourceResources";
            this.tabSourceResources.SelectedIndex = 0;
            this.tabSourceResources.Size = new System.Drawing.Size(444, 300);
            this.tabSourceResources.TabIndex = 65;
            // 
            // tabPageAsm
            // 
            this.tabPageAsm.Controls.Add(this.treeSourceASM);
            this.tabPageAsm.Location = new System.Drawing.Point(4, 22);
            this.tabPageAsm.Margin = new System.Windows.Forms.Padding(2);
            this.tabPageAsm.Name = "tabPageAsm";
            this.tabPageAsm.Padding = new System.Windows.Forms.Padding(2);
            this.tabPageAsm.Size = new System.Drawing.Size(436, 274);
            this.tabPageAsm.TabIndex = 0;
            this.tabPageAsm.Text = "Source ASM Resources";
            this.tabPageAsm.UseVisualStyleBackColor = true;
            // 
            // tabPageArm
            // 
            this.tabPageArm.Controls.Add(this.treeSourceARM);
            this.tabPageArm.Location = new System.Drawing.Point(4, 22);
            this.tabPageArm.Margin = new System.Windows.Forms.Padding(2);
            this.tabPageArm.Name = "tabPageArm";
            this.tabPageArm.Padding = new System.Windows.Forms.Padding(2);
            this.tabPageArm.Size = new System.Drawing.Size(436, 274);
            this.tabPageArm.TabIndex = 1;
            this.tabPageArm.Text = "Source ARM Resources";
            this.tabPageArm.UseVisualStyleBackColor = true;
            // 
            // treeSourceARM
            // 
            this.treeSourceARM.CheckBoxes = true;
            this.treeSourceARM.Location = new System.Drawing.Point(0, 0);
            this.treeSourceARM.Margin = new System.Windows.Forms.Padding(2);
            this.treeSourceARM.Name = "treeSourceARM";
            this.treeSourceARM.Size = new System.Drawing.Size(423, 262);
            this.treeSourceARM.TabIndex = 57;
            this.treeSourceARM.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeASM_AfterCheck);
            this.treeSourceARM.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeASM_AfterSelect);
            // 
            // treeTargetARM
            // 
            this.treeTargetARM.ImageList = null;
            this.treeTargetARM.Location = new System.Drawing.Point(452, 131);
            this.treeTargetARM.LogProvider = null;
            this.treeTargetARM.Name = "treeTargetARM";
            this.treeTargetARM.PropertyPanel = null;
            this.treeTargetARM.SelectedNode = null;
            this.treeTargetARM.SettingsProvider = null;
            this.treeTargetARM.Size = new System.Drawing.Size(447, 262);
            this.treeTargetARM.StatusProvider = null;
            this.treeTargetARM.TabIndex = 66;
            // 
            // AsmToArm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.treeTargetARM);
            this.Controls.Add(this.tabSourceResources);
            this.Controls.Add(this.azureLoginContextViewerARM);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.azureLoginContextViewerASM);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "AsmToArm";
            this.Size = new System.Drawing.Size(904, 418);
            this.Load += new System.EventHandler(this.AsmToArmForm_Load);
            this.Resize += new System.EventHandler(this.AsmToArmForm_Resize);
            this.tabSourceResources.ResumeLayout(false);
            this.tabPageAsm.ResumeLayout(false);
            this.tabPageArm.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Azure.UserControls.AzureLoginContextViewer azureLoginContextViewerARM;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TreeView treeSourceASM;
        private Azure.UserControls.AzureLoginContextViewer azureLoginContextViewerASM;
        private System.Windows.Forms.TabControl tabSourceResources;
        private System.Windows.Forms.TabPage tabPageAsm;
        private System.Windows.Forms.TabPage tabPageArm;
        private System.Windows.Forms.TreeView treeSourceARM;
        private Azure.UserControls.TargetTreeView treeTargetARM;
    }
}
