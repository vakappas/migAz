namespace MigAz.Forms
{
    partial class ExportResultsDialog
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnGenerateInstructions = new System.Windows.Forms.Button();
            this.btnViewTemplate = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(16, 15);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(894, 248);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnGenerateInstructions);
            this.tabPage1.Controls.Add(this.btnViewTemplate);
            this.tabPage1.Controls.Add(this.pictureBox1);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(8, 39);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage1.Size = new System.Drawing.Size(878, 201);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Status";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnGenerateInstructions
            // 
            this.btnGenerateInstructions.Location = new System.Drawing.Point(188, 95);
            this.btnGenerateInstructions.Margin = new System.Windows.Forms.Padding(4);
            this.btnGenerateInstructions.Name = "btnGenerateInstructions";
            this.btnGenerateInstructions.Size = new System.Drawing.Size(357, 59);
            this.btnGenerateInstructions.TabIndex = 0;
            this.btnGenerateInstructions.Text = "Show Deployment Instructions";
            this.btnGenerateInstructions.UseVisualStyleBackColor = true;
            this.btnGenerateInstructions.Click += new System.EventHandler(this.btnGenerateInstructions_Click);
            // 
            // btnViewTemplate
            // 
            this.btnViewTemplate.Location = new System.Drawing.Point(590, 95);
            this.btnViewTemplate.Margin = new System.Windows.Forms.Padding(4);
            this.btnViewTemplate.Name = "btnViewTemplate";
            this.btnViewTemplate.Size = new System.Drawing.Size(225, 59);
            this.btnViewTemplate.TabIndex = 1;
            this.btnViewTemplate.Text = "View Template";
            this.btnViewTemplate.UseVisualStyleBackColor = true;
            this.btnViewTemplate.Click += new System.EventHandler(this.btnViewTemplate_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::MigAz.Properties.Resources.Resource_group;
            this.pictureBox1.Location = new System.Drawing.Point(23, 19);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(137, 135);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(183, 29);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(486, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Your template has been generated successfully!  ";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(714, 289);
            this.btnOK.Margin = new System.Windows.Forms.Padding(4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(188, 51);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // ExportResultsDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(923, 361);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExportResultsDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Export Complete";
            this.Load += new System.EventHandler(this.ExportResults_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnViewTemplate;
        private System.Windows.Forms.Button btnGenerateInstructions;
    }
}