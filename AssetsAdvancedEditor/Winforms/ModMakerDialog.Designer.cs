
namespace AssetsAdvancedEditor.Winforms
{
    partial class ModMakerDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ModMakerDialog));
            this.lblModName = new System.Windows.Forms.Label();
            this.boxModName = new System.Windows.Forms.TextBox();
            this.boxModCreators = new System.Windows.Forms.TextBox();
            this.lblCredits = new System.Windows.Forms.Label();
            this.boxModDescription = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.boxModBaseFolderPath = new System.Windows.Forms.TextBox();
            this.lblBaseFolderTip = new System.Windows.Forms.Label();
            this.btnSelect = new System.Windows.Forms.Button();
            this.lblChanges = new System.Windows.Forms.Label();
            this.btnImportPackage = new System.Windows.Forms.Button();
            this.btnRemoveChange = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.affectedFilesList = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // lblModName
            // 
            this.lblModName.AutoSize = true;
            this.lblModName.Location = new System.Drawing.Point(12, 26);
            this.lblModName.Name = "lblModName";
            this.lblModName.Size = new System.Drawing.Size(67, 15);
            this.lblModName.TabIndex = 0;
            this.lblModName.Text = "Mod Name";
            // 
            // boxModName
            // 
            this.boxModName.Location = new System.Drawing.Point(94, 23);
            this.boxModName.Name = "boxModName";
            this.boxModName.Size = new System.Drawing.Size(147, 23);
            this.boxModName.TabIndex = 1;
            // 
            // boxModCreators
            // 
            this.boxModCreators.Location = new System.Drawing.Point(94, 52);
            this.boxModCreators.Name = "boxModCreators";
            this.boxModCreators.Size = new System.Drawing.Size(147, 23);
            this.boxModCreators.TabIndex = 3;
            // 
            // lblCredits
            // 
            this.lblCredits.AutoSize = true;
            this.lblCredits.Location = new System.Drawing.Point(12, 55);
            this.lblCredits.Name = "lblCredits";
            this.lblCredits.Size = new System.Drawing.Size(77, 15);
            this.lblCredits.TabIndex = 2;
            this.lblCredits.Text = "Credits (By...)";
            // 
            // boxModDescription
            // 
            this.boxModDescription.Location = new System.Drawing.Point(94, 81);
            this.boxModDescription.Multiline = true;
            this.boxModDescription.Name = "boxModDescription";
            this.boxModDescription.Size = new System.Drawing.Size(147, 179);
            this.boxModDescription.TabIndex = 5;
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(12, 84);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(78, 30);
            this.lblDescription.TabIndex = 4;
            this.lblDescription.Text = "Description / \nInstructions";
            // 
            // boxModBaseFolderPath
            // 
            this.boxModBaseFolderPath.Location = new System.Drawing.Point(252, 52);
            this.boxModBaseFolderPath.Name = "boxModBaseFolderPath";
            this.boxModBaseFolderPath.Size = new System.Drawing.Size(147, 23);
            this.boxModBaseFolderPath.TabIndex = 7;
            // 
            // lblBaseFolderTip
            // 
            this.lblBaseFolderTip.AutoSize = true;
            this.lblBaseFolderTip.Location = new System.Drawing.Point(250, 19);
            this.lblBaseFolderTip.Name = "lblBaseFolderTip";
            this.lblBaseFolderTip.Size = new System.Drawing.Size(155, 30);
            this.lblBaseFolderTip.TabIndex = 6;
            this.lblBaseFolderTip.Text = "Select a base folder \n(e.g. the upper game folder)";
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(405, 52);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(83, 23);
            this.btnSelect.TabIndex = 8;
            this.btnSelect.Text = "Select";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // lblChanges
            // 
            this.lblChanges.AutoSize = true;
            this.lblChanges.Location = new System.Drawing.Point(250, 84);
            this.lblChanges.Name = "lblChanges";
            this.lblChanges.Size = new System.Drawing.Size(147, 15);
            this.lblChanges.TabIndex = 9;
            this.lblChanges.Text = "Changes done to the files :";
            // 
            // btnImportPackage
            // 
            this.btnImportPackage.Location = new System.Drawing.Point(251, 262);
            this.btnImportPackage.Name = "btnImportPackage";
            this.btnImportPackage.Size = new System.Drawing.Size(110, 25);
            this.btnImportPackage.TabIndex = 11;
            this.btnImportPackage.Text = "Import package";
            this.btnImportPackage.UseVisualStyleBackColor = true;
            this.btnImportPackage.Click += new System.EventHandler(this.btnImportPackage_Click);
            // 
            // btnRemoveChange
            // 
            this.btnRemoveChange.Location = new System.Drawing.Point(378, 262);
            this.btnRemoveChange.Name = "btnRemoveChange";
            this.btnRemoveChange.Size = new System.Drawing.Size(110, 25);
            this.btnRemoveChange.TabIndex = 12;
            this.btnRemoveChange.Text = "Remove change";
            this.btnRemoveChange.UseVisualStyleBackColor = true;
            this.btnRemoveChange.Click += new System.EventHandler(this.btnRemoveChange_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(404, 293);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(84, 25);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(251, 293);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(84, 25);
            this.btnOK.TabIndex = 13;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // affectedFilesList
            // 
            this.affectedFilesList.Location = new System.Drawing.Point(252, 102);
            this.affectedFilesList.Name = "affectedFilesList";
            this.affectedFilesList.Size = new System.Drawing.Size(235, 158);
            this.affectedFilesList.TabIndex = 15;
            // 
            // ModMakerDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ClientSize = new System.Drawing.Size(500, 331);
            this.Controls.Add(this.affectedFilesList);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnRemoveChange);
            this.Controls.Add(this.btnImportPackage);
            this.Controls.Add(this.lblChanges);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.boxModBaseFolderPath);
            this.Controls.Add(this.lblBaseFolderTip);
            this.Controls.Add(this.boxModDescription);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.boxModCreators);
            this.Controls.Add(this.lblCredits);
            this.Controls.Add(this.boxModName);
            this.Controls.Add(this.lblModName);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ModMakerDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Create an installer package";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblModName;
        private System.Windows.Forms.TextBox boxModName;
        private System.Windows.Forms.TextBox boxModCreators;
        private System.Windows.Forms.Label lblCredits;
        private System.Windows.Forms.TextBox boxModDescription;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox boxModBaseFolderPath;
        private System.Windows.Forms.Label lblBaseFolderTip;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Label lblChanges;
        private System.Windows.Forms.Button btnImportPackage;
        private System.Windows.Forms.Button btnRemoveChange;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TreeView affectedFilesList;
    }
}