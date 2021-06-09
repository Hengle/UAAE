namespace AssetsAdvancedEditor.Winforms
{
    partial class AssetsViewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AssetsViewer));
            this.lblAssets = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.boxName = new System.Windows.Forms.TextBox();
            this.boxPathID = new System.Windows.Forms.TextBox();
            this.lblPathID = new System.Windows.Forms.Label();
            this.lblFileID = new System.Windows.Forms.Label();
            this.boxFileID = new System.Windows.Forms.TextBox();
            this.lblType = new System.Windows.Forms.Label();
            this.boxType = new System.Windows.Forms.TextBox();
            this.btnViewData = new System.Windows.Forms.Button();
            this.btnExportRaw = new System.Windows.Forms.Button();
            this.btnExportDump = new System.Windows.Forms.Button();
            this.btnImportRaw = new System.Windows.Forms.Button();
            this.btnImportDump = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.modMakerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createStandaloneexeInstallerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createInstallerPackageFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.searchByNameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.byNameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.binaryContentSearchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.monobehaviourSearchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.transformSearchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.continueSearchF3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.goToAssetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dependenciesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.windowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.assetPreviewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.getScriptInformationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.assetList = new System.Windows.Forms.ListView();
            this.nameColumn = new System.Windows.Forms.ColumnHeader();
            this.containerColumn = new System.Windows.Forms.ColumnHeader();
            this.typeColumn = new System.Windows.Forms.ColumnHeader();
            this.typeIDColumn = new System.Windows.Forms.ColumnHeader();
            this.fileIDColumn = new System.Windows.Forms.ColumnHeader();
            this.pathIDColumn = new System.Windows.Forms.ColumnHeader();
            this.sizeColumn = new System.Windows.Forms.ColumnHeader();
            this.modifiedColumn = new System.Windows.Forms.ColumnHeader();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblAssets
            // 
            this.lblAssets.AutoSize = true;
            this.lblAssets.Location = new System.Drawing.Point(14, 28);
            this.lblAssets.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAssets.Name = "lblAssets";
            this.lblAssets.Size = new System.Drawing.Size(40, 15);
            this.lblAssets.TabIndex = 1;
            this.lblAssets.Text = "Assets";
            // 
            // lblName
            // 
            this.lblName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(757, 46);
            this.lblName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(39, 15);
            this.lblName.TabIndex = 2;
            this.lblName.Text = "Name";
            // 
            // boxName
            // 
            this.boxName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.boxName.BackColor = System.Drawing.SystemColors.Window;
            this.boxName.Location = new System.Drawing.Point(760, 65);
            this.boxName.Margin = new System.Windows.Forms.Padding(16, 3, 16, 3);
            this.boxName.Name = "boxName";
            this.boxName.ReadOnly = true;
            this.boxName.Size = new System.Drawing.Size(210, 23);
            this.boxName.TabIndex = 3;
            // 
            // boxPathID
            // 
            this.boxPathID.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.boxPathID.BackColor = System.Drawing.SystemColors.Window;
            this.boxPathID.Location = new System.Drawing.Point(760, 110);
            this.boxPathID.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.boxPathID.Name = "boxPathID";
            this.boxPathID.ReadOnly = true;
            this.boxPathID.Size = new System.Drawing.Size(210, 23);
            this.boxPathID.TabIndex = 5;
            // 
            // lblPathID
            // 
            this.lblPathID.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPathID.AutoSize = true;
            this.lblPathID.Location = new System.Drawing.Point(757, 91);
            this.lblPathID.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPathID.Name = "lblPathID";
            this.lblPathID.Size = new System.Drawing.Size(45, 15);
            this.lblPathID.TabIndex = 4;
            this.lblPathID.Text = "Path ID";
            // 
            // lblFileID
            // 
            this.lblFileID.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFileID.AutoSize = true;
            this.lblFileID.Location = new System.Drawing.Point(757, 136);
            this.lblFileID.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFileID.Name = "lblFileID";
            this.lblFileID.Size = new System.Drawing.Size(39, 15);
            this.lblFileID.TabIndex = 6;
            this.lblFileID.Text = "File ID";
            // 
            // boxFileID
            // 
            this.boxFileID.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.boxFileID.BackColor = System.Drawing.SystemColors.Window;
            this.boxFileID.Location = new System.Drawing.Point(760, 155);
            this.boxFileID.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.boxFileID.Name = "boxFileID";
            this.boxFileID.ReadOnly = true;
            this.boxFileID.Size = new System.Drawing.Size(210, 23);
            this.boxFileID.TabIndex = 7;
            // 
            // lblType
            // 
            this.lblType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblType.AutoSize = true;
            this.lblType.Location = new System.Drawing.Point(757, 181);
            this.lblType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(31, 15);
            this.lblType.TabIndex = 8;
            this.lblType.Text = "Type";
            // 
            // boxType
            // 
            this.boxType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.boxType.BackColor = System.Drawing.SystemColors.Window;
            this.boxType.Location = new System.Drawing.Point(760, 200);
            this.boxType.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.boxType.Name = "boxType";
            this.boxType.ReadOnly = true;
            this.boxType.Size = new System.Drawing.Size(210, 23);
            this.boxType.TabIndex = 9;
            // 
            // btnViewData
            // 
            this.btnViewData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnViewData.Location = new System.Drawing.Point(759, 230);
            this.btnViewData.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnViewData.Name = "btnViewData";
            this.btnViewData.Size = new System.Drawing.Size(212, 27);
            this.btnViewData.TabIndex = 10;
            this.btnViewData.Text = "View Data";
            this.btnViewData.UseVisualStyleBackColor = true;
            this.btnViewData.Click += new System.EventHandler(this.btnViewData_Click);
            // 
            // btnExportRaw
            // 
            this.btnExportRaw.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExportRaw.Location = new System.Drawing.Point(759, 306);
            this.btnExportRaw.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnExportRaw.Name = "btnExportRaw";
            this.btnExportRaw.Size = new System.Drawing.Size(212, 27);
            this.btnExportRaw.TabIndex = 11;
            this.btnExportRaw.Text = "Export Raw";
            this.btnExportRaw.UseVisualStyleBackColor = true;
            this.btnExportRaw.Click += new System.EventHandler(this.btnExportRaw_Click);
            // 
            // btnExportDump
            // 
            this.btnExportDump.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExportDump.Location = new System.Drawing.Point(759, 344);
            this.btnExportDump.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnExportDump.Name = "btnExportDump";
            this.btnExportDump.Size = new System.Drawing.Size(212, 27);
            this.btnExportDump.TabIndex = 12;
            this.btnExportDump.Text = "Export Dump";
            this.btnExportDump.UseVisualStyleBackColor = true;
            this.btnExportDump.Click += new System.EventHandler(this.btnExportDump_Click);
            // 
            // btnImportRaw
            // 
            this.btnImportRaw.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImportRaw.Location = new System.Drawing.Point(759, 382);
            this.btnImportRaw.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnImportRaw.Name = "btnImportRaw";
            this.btnImportRaw.Size = new System.Drawing.Size(212, 27);
            this.btnImportRaw.TabIndex = 13;
            this.btnImportRaw.Text = "Import Raw";
            this.btnImportRaw.UseVisualStyleBackColor = true;
            this.btnImportRaw.Click += new System.EventHandler(this.btnImportRaw_Click);
            // 
            // btnImportDump
            // 
            this.btnImportDump.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImportDump.Location = new System.Drawing.Point(759, 420);
            this.btnImportDump.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnImportDump.Name = "btnImportDump";
            this.btnImportDump.Size = new System.Drawing.Size(212, 27);
            this.btnImportDump.TabIndex = 14;
            this.btnImportDump.Text = "Import Dump";
            this.btnImportDump.UseVisualStyleBackColor = true;
            this.btnImportDump.Click += new System.EventHandler(this.btnImportDump_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEdit.Location = new System.Drawing.Point(759, 268);
            this.btnEdit.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(212, 27);
            this.btnEdit.TabIndex = 15;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemove.Location = new System.Drawing.Point(759, 496);
            this.btnRemove.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(212, 27);
            this.btnRemove.TabIndex = 16;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.Location = new System.Drawing.Point(759, 458);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(212, 27);
            this.btnAdd.TabIndex = 17;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.toolsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(985, 24);
            this.menuStrip1.TabIndex = 18;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.modMakerToolStripMenuItem,
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.saveAsToolStripMenuItem.Text = "Save as...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // modMakerToolStripMenuItem
            // 
            this.modMakerToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createStandaloneexeInstallerToolStripMenuItem,
            this.createInstallerPackageFileToolStripMenuItem});
            this.modMakerToolStripMenuItem.Enabled = false;
            this.modMakerToolStripMenuItem.Name = "modMakerToolStripMenuItem";
            this.modMakerToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.modMakerToolStripMenuItem.Text = "Mod Maker";
            // 
            // createStandaloneexeInstallerToolStripMenuItem
            // 
            this.createStandaloneexeInstallerToolStripMenuItem.Name = "createStandaloneexeInstallerToolStripMenuItem";
            this.createStandaloneexeInstallerToolStripMenuItem.Size = new System.Drawing.Size(237, 22);
            this.createStandaloneexeInstallerToolStripMenuItem.Text = "Create standalone .exe installer";
            // 
            // createInstallerPackageFileToolStripMenuItem
            // 
            this.createInstallerPackageFileToolStripMenuItem.Name = "createInstallerPackageFileToolStripMenuItem";
            this.createInstallerPackageFileToolStripMenuItem.Size = new System.Drawing.Size(237, 22);
            this.createInstallerPackageFileToolStripMenuItem.Text = "Create installer package file";
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.searchByNameToolStripMenuItem,
            this.continueSearchF3ToolStripMenuItem,
            this.goToAssetToolStripMenuItem,
            this.dependenciesToolStripMenuItem,
            this.windowToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // searchByNameToolStripMenuItem
            // 
            this.searchByNameToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.byNameToolStripMenuItem,
            this.binaryContentSearchToolStripMenuItem,
            this.monobehaviourSearchToolStripMenuItem,
            this.transformSearchToolStripMenuItem});
            this.searchByNameToolStripMenuItem.Name = "searchByNameToolStripMenuItem";
            this.searchByNameToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.searchByNameToolStripMenuItem.Text = "Search...";
            // 
            // byNameToolStripMenuItem
            // 
            this.byNameToolStripMenuItem.Name = "byNameToolStripMenuItem";
            this.byNameToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.byNameToolStripMenuItem.Text = "Asset by name";
            this.byNameToolStripMenuItem.Click += new System.EventHandler(this.byNameToolStripMenuItem_Click);
            // 
            // binaryContentSearchToolStripMenuItem
            // 
            this.binaryContentSearchToolStripMenuItem.Name = "binaryContentSearchToolStripMenuItem";
            this.binaryContentSearchToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.binaryContentSearchToolStripMenuItem.Text = "Content Search";
            this.binaryContentSearchToolStripMenuItem.Click += new System.EventHandler(this.binaryContentSearchToolStripMenuItem_Click);
            // 
            // monobehaviourSearchToolStripMenuItem
            // 
            this.monobehaviourSearchToolStripMenuItem.Name = "monobehaviourSearchToolStripMenuItem";
            this.monobehaviourSearchToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.monobehaviourSearchToolStripMenuItem.Text = "Monobehaviour Search";
            this.monobehaviourSearchToolStripMenuItem.Click += new System.EventHandler(this.monobehaviourSearchToolStripMenuItem_Click);
            // 
            // transformSearchToolStripMenuItem
            // 
            this.transformSearchToolStripMenuItem.Name = "transformSearchToolStripMenuItem";
            this.transformSearchToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.transformSearchToolStripMenuItem.Text = "Transform Search";
            this.transformSearchToolStripMenuItem.Click += new System.EventHandler(this.transformSearchToolStripMenuItem_Click);
            // 
            // continueSearchF3ToolStripMenuItem
            // 
            this.continueSearchF3ToolStripMenuItem.Name = "continueSearchF3ToolStripMenuItem";
            this.continueSearchF3ToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.continueSearchF3ToolStripMenuItem.Text = "Continue search (F3)";
            // 
            // goToAssetToolStripMenuItem
            // 
            this.goToAssetToolStripMenuItem.Name = "goToAssetToolStripMenuItem";
            this.goToAssetToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.goToAssetToolStripMenuItem.Text = "Go to asset";
            // 
            // dependenciesToolStripMenuItem
            // 
            this.dependenciesToolStripMenuItem.Name = "dependenciesToolStripMenuItem";
            this.dependenciesToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.dependenciesToolStripMenuItem.Text = "Dependencies";
            this.dependenciesToolStripMenuItem.Click += new System.EventHandler(this.dependenciesToolStripMenuItem_Click);
            // 
            // windowToolStripMenuItem
            // 
            this.windowToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.assetPreviewToolStripMenuItem});
            this.windowToolStripMenuItem.Name = "windowToolStripMenuItem";
            this.windowToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.windowToolStripMenuItem.Text = "Window";
            // 
            // assetPreviewToolStripMenuItem
            // 
            this.assetPreviewToolStripMenuItem.Name = "assetPreviewToolStripMenuItem";
            this.assetPreviewToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.assetPreviewToolStripMenuItem.Text = "Asset Preview";
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.getScriptInformationToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // getScriptInformationToolStripMenuItem
            // 
            this.getScriptInformationToolStripMenuItem.Name = "getScriptInformationToolStripMenuItem";
            this.getScriptInformationToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.getScriptInformationToolStripMenuItem.Text = "Get script information";
            // 
            // assetList
            // 
            this.assetList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.assetList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.nameColumn,
            this.containerColumn,
            this.typeColumn,
            this.typeIDColumn,
            this.fileIDColumn,
            this.pathIDColumn,
            this.sizeColumn,
            this.modifiedColumn});
            this.assetList.HideSelection = false;
            this.assetList.Location = new System.Drawing.Point(14, 46);
            this.assetList.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.assetList.Name = "assetList";
            this.assetList.Size = new System.Drawing.Size(732, 476);
            this.assetList.TabIndex = 19;
            this.assetList.UseCompatibleStateImageBehavior = false;
            this.assetList.View = System.Windows.Forms.View.Details;
            this.assetList.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.assetList_ItemSelectionChanged);
            this.assetList.SelectedIndexChanged += new System.EventHandler(this.assetList_SelectedIndexChanged);
            // 
            // nameColumn
            // 
            this.nameColumn.Text = "Name";
            this.nameColumn.Width = 195;
            // 
            // containerColumn
            // 
            this.containerColumn.Text = "Container";
            this.containerColumn.Width = 100;
            // 
            // typeColumn
            // 
            this.typeColumn.Text = "Type";
            this.typeColumn.Width = 140;
            // 
            // typeIDColumn
            // 
            this.typeIDColumn.Text = "Type ID";
            this.typeIDColumn.Width = 53;
            // 
            // fileIDColumn
            // 
            this.fileIDColumn.Text = "File ID";
            this.fileIDColumn.Width = 47;
            // 
            // pathIDColumn
            // 
            this.pathIDColumn.Text = "Path ID";
            this.pathIDColumn.Width = 55;
            // 
            // sizeColumn
            // 
            this.sizeColumn.Text = "Size (bytes)";
            this.sizeColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.sizeColumn.Width = 71;
            // 
            // modifiedColumn
            // 
            this.modifiedColumn.Text = "Modified";
            // 
            // AssetsViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ClientSize = new System.Drawing.Size(985, 537);
            this.Controls.Add(this.assetList);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnImportDump);
            this.Controls.Add(this.btnImportRaw);
            this.Controls.Add(this.btnExportDump);
            this.Controls.Add(this.btnExportRaw);
            this.Controls.Add(this.btnViewData);
            this.Controls.Add(this.boxType);
            this.Controls.Add(this.lblType);
            this.Controls.Add(this.boxFileID);
            this.Controls.Add(this.lblFileID);
            this.Controls.Add(this.boxPathID);
            this.Controls.Add(this.lblPathID);
            this.Controls.Add(this.boxName);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.lblAssets);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "AssetsViewer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Assets Info";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AssetsViewer_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AssetsViewer_KeyDown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblAssets;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox boxName;
        private System.Windows.Forms.TextBox boxPathID;
        private System.Windows.Forms.Label lblPathID;
        private System.Windows.Forms.Label lblFileID;
        private System.Windows.Forms.TextBox boxFileID;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.TextBox boxType;
        private System.Windows.Forms.Button btnViewData;
        private System.Windows.Forms.Button btnExportRaw;
        private System.Windows.Forms.Button btnExportDump;
        private System.Windows.Forms.Button btnImportRaw;
        private System.Windows.Forms.Button btnImportDump;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem modMakerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createStandaloneexeInstallerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createInstallerPackageFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem searchByNameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem continueSearchF3ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem goToAssetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dependenciesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem windowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem assetPreviewToolStripMenuItem;
        private System.Windows.Forms.ListView assetList;
        private System.Windows.Forms.ColumnHeader nameColumn;
        private System.Windows.Forms.ColumnHeader typeColumn;
        private System.Windows.Forms.ColumnHeader fileIDColumn;
        private System.Windows.Forms.ColumnHeader pathIDColumn;
        private System.Windows.Forms.ColumnHeader sizeColumn;
        private System.Windows.Forms.ColumnHeader modifiedColumn;
        private System.Windows.Forms.ToolStripMenuItem byNameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem binaryContentSearchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem monobehaviourSearchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem transformSearchToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader typeIDColumn;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem getScriptInformationToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader containerColumn;
    }
}