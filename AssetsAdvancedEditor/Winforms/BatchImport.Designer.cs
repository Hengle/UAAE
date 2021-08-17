
namespace AssetsAdvancedEditor.Winforms
{
    partial class BatchImport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BatchImport));
            this.lblAssetsToImport = new System.Windows.Forms.Label();
            this.lblMatchingFiles = new System.Windows.Forms.Label();
            this.affectedAssetsList = new System.Windows.Forms.ListView();
            this.columnDescription = new System.Windows.Forms.ColumnHeader();
            this.columnFile = new System.Windows.Forms.ColumnHeader();
            this.columnPathID = new System.Windows.Forms.ColumnHeader();
            this.lboxMatchingFiles = new System.Windows.Forms.ListBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblAssetsToImport
            // 
            this.lblAssetsToImport.AutoSize = true;
            this.lblAssetsToImport.Location = new System.Drawing.Point(173, 9);
            this.lblAssetsToImport.Name = "lblAssetsToImport";
            this.lblAssetsToImport.Size = new System.Drawing.Size(93, 15);
            this.lblAssetsToImport.TabIndex = 0;
            this.lblAssetsToImport.Text = "Assets to import";
            // 
            // lblMatchingFiles
            // 
            this.lblMatchingFiles.AutoSize = true;
            this.lblMatchingFiles.Location = new System.Drawing.Point(532, 9);
            this.lblMatchingFiles.Name = "lblMatchingFiles";
            this.lblMatchingFiles.Size = new System.Drawing.Size(82, 15);
            this.lblMatchingFiles.TabIndex = 1;
            this.lblMatchingFiles.Text = "Matching files";
            // 
            // affectedAssetsList
            // 
            this.affectedAssetsList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnDescription,
            this.columnFile,
            this.columnPathID});
            this.affectedAssetsList.HideSelection = false;
            this.affectedAssetsList.Location = new System.Drawing.Point(12, 32);
            this.affectedAssetsList.MultiSelect = false;
            this.affectedAssetsList.Name = "affectedAssetsList";
            this.affectedAssetsList.Size = new System.Drawing.Size(430, 319);
            this.affectedAssetsList.TabIndex = 2;
            this.affectedAssetsList.UseCompatibleStateImageBehavior = false;
            this.affectedAssetsList.View = System.Windows.Forms.View.Details;
            this.affectedAssetsList.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.affectedAssetsList_ItemSelectionChanged);
            // 
            // columnDescription
            // 
            this.columnDescription.Text = "Description";
            this.columnDescription.Width = 200;
            // 
            // columnFile
            // 
            this.columnFile.Text = "File";
            this.columnFile.Width = 130;
            // 
            // columnPathID
            // 
            this.columnPathID.Text = "Path ID";
            this.columnPathID.Width = 80;
            // 
            // lboxMatchingFiles
            // 
            this.lboxMatchingFiles.FormattingEnabled = true;
            this.lboxMatchingFiles.HorizontalScrollbar = true;
            this.lboxMatchingFiles.ItemHeight = 15;
            this.lboxMatchingFiles.Location = new System.Drawing.Point(455, 32);
            this.lboxMatchingFiles.Name = "lboxMatchingFiles";
            this.lboxMatchingFiles.Size = new System.Drawing.Size(235, 319);
            this.lboxMatchingFiles.TabIndex = 3;
            this.lboxMatchingFiles.SelectedIndexChanged += new System.EventHandler(this.lboxMatchingFiles_SelectedIndexChanged);
            this.lboxMatchingFiles.MouseHover += new System.EventHandler(this.lboxMatchingFiles_MouseHover);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(616, 357);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 27);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(454, 357);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 27);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(11, 357);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(75, 27);
            this.btnEdit.TabIndex = 8;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // BatchImport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ClientSize = new System.Drawing.Size(702, 396);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lboxMatchingFiles);
            this.Controls.Add(this.affectedAssetsList);
            this.Controls.Add(this.lblMatchingFiles);
            this.Controls.Add(this.lblAssetsToImport);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "BatchImport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Batch Import";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblAssetsToImport;
        private System.Windows.Forms.Label lblMatchingFiles;
        private System.Windows.Forms.ListView affectedAssetsList;
        private System.Windows.Forms.ListBox lboxMatchingFiles;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.ColumnHeader columnDescription;
        private System.Windows.Forms.ColumnHeader columnFile;
        private System.Windows.Forms.ColumnHeader columnPathID;
    }
}