namespace AssetsAdvancedEditor.Winforms
{
    partial class Menu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Menu));
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadPackageFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.compressToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.modMakerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createStandaloneexeInstallerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createPackageFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editTypeDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editTypePackageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblFileName = new System.Windows.Forms.Label();
            this.cboxBundleContents = new System.Windows.Forms.ComboBox();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnInfo = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Padding = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.menuStrip.Size = new System.Drawing.Size(385, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.loadPackageFileToolStripMenuItem,
            this.closeToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.toolStripSeparator1,
            this.compressToolStripMenuItem,
            this.modMakerToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.openToolStripMenuItem.Text = "&Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // loadPackageFileToolStripMenuItem
            // 
            this.loadPackageFileToolStripMenuItem.Enabled = false;
            this.loadPackageFileToolStripMenuItem.Name = "loadPackageFileToolStripMenuItem";
            this.loadPackageFileToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.loadPackageFileToolStripMenuItem.Text = "&Load Package File";
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.closeToolStripMenuItem.Text = "&Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.saveToolStripMenuItem.Text = "&Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(165, 6);
            // 
            // compressToolStripMenuItem
            // 
            this.compressToolStripMenuItem.Name = "compressToolStripMenuItem";
            this.compressToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.compressToolStripMenuItem.Text = "&Compress";
            // 
            // modMakerToolStripMenuItem
            // 
            this.modMakerToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createStandaloneexeInstallerToolStripMenuItem,
            this.createPackageFileToolStripMenuItem});
            this.modMakerToolStripMenuItem.Enabled = false;
            this.modMakerToolStripMenuItem.Name = "modMakerToolStripMenuItem";
            this.modMakerToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.modMakerToolStripMenuItem.Text = "Mod Maker";
            // 
            // createStandaloneexeInstallerToolStripMenuItem
            // 
            this.createStandaloneexeInstallerToolStripMenuItem.Name = "createStandaloneexeInstallerToolStripMenuItem";
            this.createStandaloneexeInstallerToolStripMenuItem.Size = new System.Drawing.Size(237, 22);
            this.createStandaloneexeInstallerToolStripMenuItem.Text = "Create standalone .exe installer";
            // 
            // createPackageFileToolStripMenuItem
            // 
            this.createPackageFileToolStripMenuItem.Name = "createPackageFileToolStripMenuItem";
            this.createPackageFileToolStripMenuItem.Size = new System.Drawing.Size(237, 22);
            this.createPackageFileToolStripMenuItem.Text = "Create package file";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.exitToolStripMenuItem.Text = "&Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editTypeDatabaseToolStripMenuItem,
            this.editTypePackageToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "&Options";
            // 
            // editTypeDatabaseToolStripMenuItem
            // 
            this.editTypeDatabaseToolStripMenuItem.Enabled = false;
            this.editTypeDatabaseToolStripMenuItem.Name = "editTypeDatabaseToolStripMenuItem";
            this.editTypeDatabaseToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.editTypeDatabaseToolStripMenuItem.Text = "Edit Type &Database";
            // 
            // editTypePackageToolStripMenuItem
            // 
            this.editTypePackageToolStripMenuItem.Enabled = false;
            this.editTypePackageToolStripMenuItem.Name = "editTypePackageToolStripMenuItem";
            this.editTypePackageToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.editTypePackageToolStripMenuItem.Text = "Edit Type &Package";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.aboutToolStripMenuItem.Text = "&About...";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // lblFileName
            // 
            this.lblFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFileName.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblFileName.Location = new System.Drawing.Point(14, 22);
            this.lblFileName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFileName.Name = "lblFileName";
            this.lblFileName.Size = new System.Drawing.Size(357, 19);
            this.lblFileName.TabIndex = 1;
            this.lblFileName.Text = "No file opened.";
            this.lblFileName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cboxBundleContents
            // 
            this.cboxBundleContents.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboxBundleContents.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxBundleContents.Enabled = false;
            this.cboxBundleContents.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cboxBundleContents.FormattingEnabled = true;
            this.cboxBundleContents.Location = new System.Drawing.Point(14, 47);
            this.cboxBundleContents.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cboxBundleContents.Name = "cboxBundleContents";
            this.cboxBundleContents.Size = new System.Drawing.Size(358, 25);
            this.cboxBundleContents.TabIndex = 2;
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.Enabled = false;
            this.btnExport.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnExport.Location = new System.Drawing.Point(13, 82);
            this.btnExport.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(87, 33);
            this.btnExport.TabIndex = 3;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport.Enabled = false;
            this.btnImport.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnImport.Location = new System.Drawing.Point(104, 82);
            this.btnImport.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(87, 33);
            this.btnImport.TabIndex = 4;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnInfo
            // 
            this.btnInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnInfo.Enabled = false;
            this.btnInfo.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnInfo.Location = new System.Drawing.Point(286, 82);
            this.btnInfo.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnInfo.Name = "btnInfo";
            this.btnInfo.Size = new System.Drawing.Size(87, 33);
            this.btnInfo.TabIndex = 5;
            this.btnInfo.Text = "Info";
            this.btnInfo.UseVisualStyleBackColor = true;
            this.btnInfo.Click += new System.EventHandler(this.btnInfo_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemove.Enabled = false;
            this.btnRemove.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnRemove.Location = new System.Drawing.Point(195, 82);
            this.btnRemove.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(87, 33);
            this.btnRemove.TabIndex = 6;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // Menu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ClientSize = new System.Drawing.Size(385, 125);
            this.Controls.Add(this.btnInfo);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.cboxBundleContents);
            this.Controls.Add(this.lblFileName);
            this.Controls.Add(this.menuStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.Name = "Menu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Unity Assets Advanced Editor";
            this.Load += new System.EventHandler(this.Menu_Load);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadPackageFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem compressToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem modMakerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createStandaloneexeInstallerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createPackageFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editTypeDatabaseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editTypePackageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.Label lblFileName;
        private System.Windows.Forms.ComboBox cboxBundleContents;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button btnInfo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.Button btnRemove;
    }
}

