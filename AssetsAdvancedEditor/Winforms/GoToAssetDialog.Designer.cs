
namespace AssetsAdvancedEditor.Winforms
{
    partial class GoToAssetDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GoToAssetDialog));
            this.lblFileID = new System.Windows.Forms.Label();
            this.cboxFileID = new System.Windows.Forms.ComboBox();
            this.lblPathID = new System.Windows.Forms.Label();
            this.boxPathID = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblFileID
            // 
            this.lblFileID.AutoSize = true;
            this.lblFileID.Location = new System.Drawing.Point(16, 14);
            this.lblFileID.Name = "lblFileID";
            this.lblFileID.Size = new System.Drawing.Size(39, 15);
            this.lblFileID.TabIndex = 0;
            this.lblFileID.Text = "File ID";
            // 
            // cboxFileID
            // 
            this.cboxFileID.FormattingEnabled = true;
            this.cboxFileID.Location = new System.Drawing.Point(84, 11);
            this.cboxFileID.Name = "cboxFileID";
            this.cboxFileID.Size = new System.Drawing.Size(158, 23);
            this.cboxFileID.TabIndex = 1;
            // 
            // lblPathID
            // 
            this.lblPathID.AutoSize = true;
            this.lblPathID.Location = new System.Drawing.Point(16, 40);
            this.lblPathID.Name = "lblPathID";
            this.lblPathID.Size = new System.Drawing.Size(45, 15);
            this.lblPathID.TabIndex = 2;
            this.lblPathID.Text = "Path ID";
            // 
            // boxPathID
            // 
            this.boxPathID.Location = new System.Drawing.Point(84, 37);
            this.boxPathID.Name = "boxPathID";
            this.boxPathID.Size = new System.Drawing.Size(158, 23);
            this.boxPathID.TabIndex = 3;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(163, 77);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(84, 25);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(12, 77);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(84, 25);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // GoToAssetDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(259, 114);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.boxPathID);
            this.Controls.Add(this.lblPathID);
            this.Controls.Add(this.cboxFileID);
            this.Controls.Add(this.lblFileID);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "GoToAssetDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Go to";
            this.Load += new System.EventHandler(this.GoToAssetDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblFileID;
        private System.Windows.Forms.ComboBox cboxFileID;
        private System.Windows.Forms.Label lblPathID;
        private System.Windows.Forms.TextBox boxPathID;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
    }
}