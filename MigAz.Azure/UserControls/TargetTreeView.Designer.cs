namespace MigAz.Azure.UserControls
{
    partial class TargetTreeView
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
            this.treeTargetARM = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // treeTargetARM
            // 
            this.treeTargetARM.Location = new System.Drawing.Point(2, 2);
            this.treeTargetARM.Margin = new System.Windows.Forms.Padding(2);
            this.treeTargetARM.Name = "treeTargetARM";
            this.treeTargetARM.Size = new System.Drawing.Size(87, 85);
            this.treeTargetARM.TabIndex = 58;
            this.treeTargetARM.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TargetTreeView_AfterSelect);
            // 
            // TargetTreeView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.treeTargetARM);
            this.Name = "TargetTreeView";
            this.Size = new System.Drawing.Size(132, 123);
            this.Resize += new System.EventHandler(this.TargetTreeView_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeTargetARM;
    }
}
