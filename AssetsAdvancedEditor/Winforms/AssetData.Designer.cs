namespace AssetsAdvancedEditor.Winforms
{
    partial class AssetData
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AssetData));
            this.tcDataView = new System.Windows.Forms.TabControl();
            this.tpTreeView = new System.Windows.Forms.TabPage();
            this.rawViewTree = new System.Windows.Forms.TreeView();
            this.tpDumpView = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.prettifiedViewTree = new System.Windows.Forms.TreeView();
            this.btnCloseDown = new System.Windows.Forms.Button();
            this.btnOpenDown = new System.Windows.Forms.Button();
            this.btnCloseAll = new System.Windows.Forms.Button();
            this.btnOpenAll = new System.Windows.Forms.Button();
            this.tcDataView.SuspendLayout();
            this.tpTreeView.SuspendLayout();
            this.tpDumpView.SuspendLayout();
            this.SuspendLayout();
            // 
            // tcDataView
            // 
            this.tcDataView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tcDataView.Controls.Add(this.tpTreeView);
            this.tcDataView.Controls.Add(this.tpDumpView);
            this.tcDataView.Location = new System.Drawing.Point(14, 14);
            this.tcDataView.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tcDataView.Name = "tcDataView";
            this.tcDataView.SelectedIndex = 0;
            this.tcDataView.Size = new System.Drawing.Size(406, 430);
            this.tcDataView.TabIndex = 0;
            // 
            // tpTreeView
            // 
            this.tpTreeView.Controls.Add(this.rawViewTree);
            this.tpTreeView.Location = new System.Drawing.Point(4, 24);
            this.tpTreeView.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tpTreeView.Name = "tpTreeView";
            this.tpTreeView.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tpTreeView.Size = new System.Drawing.Size(398, 402);
            this.tpTreeView.TabIndex = 0;
            this.tpTreeView.Text = "Tree View";
            this.tpTreeView.UseVisualStyleBackColor = true;
            // 
            // rawViewTree
            // 
            this.rawViewTree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rawViewTree.Location = new System.Drawing.Point(0, 0);
            this.rawViewTree.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.rawViewTree.Name = "rawViewTree";
            this.rawViewTree.Size = new System.Drawing.Size(398, 402);
            this.rawViewTree.TabIndex = 0;
            // 
            // tpDumpView
            // 
            this.tpDumpView.Controls.Add(this.label1);
            this.tpDumpView.Controls.Add(this.prettifiedViewTree);
            this.tpDumpView.Location = new System.Drawing.Point(4, 24);
            this.tpDumpView.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tpDumpView.Name = "tpDumpView";
            this.tpDumpView.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tpDumpView.Size = new System.Drawing.Size(398, 402);
            this.tpDumpView.TabIndex = 1;
            this.tpDumpView.Text = "Dump View";
            this.tpDumpView.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(7, 3);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(383, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "This feature is not implemented yet.";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // prettifiedViewTree
            // 
            this.prettifiedViewTree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.prettifiedViewTree.Location = new System.Drawing.Point(7, 22);
            this.prettifiedViewTree.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.prettifiedViewTree.Name = "prettifiedViewTree";
            this.prettifiedViewTree.Size = new System.Drawing.Size(382, 371);
            this.prettifiedViewTree.TabIndex = 1;
            // 
            // btnCloseDown
            // 
            this.btnCloseDown.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCloseDown.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnCloseDown.Location = new System.Drawing.Point(329, 450);
            this.btnCloseDown.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnCloseDown.Name = "btnCloseDown";
            this.btnCloseDown.Size = new System.Drawing.Size(91, 24);
            this.btnCloseDown.TabIndex = 3;
            this.btnCloseDown.Text = "Close Down";
            this.btnCloseDown.UseVisualStyleBackColor = true;
            this.btnCloseDown.Click += new System.EventHandler(this.closeDown_Click);
            // 
            // btnOpenDown
            // 
            this.btnOpenDown.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnOpenDown.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnOpenDown.Location = new System.Drawing.Point(224, 450);
            this.btnOpenDown.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnOpenDown.Name = "btnOpenDown";
            this.btnOpenDown.Size = new System.Drawing.Size(91, 24);
            this.btnOpenDown.TabIndex = 2;
            this.btnOpenDown.Text = "Open Down";
            this.btnOpenDown.UseVisualStyleBackColor = true;
            this.btnOpenDown.Click += new System.EventHandler(this.openDown_Click);
            // 
            // btnCloseAll
            // 
            this.btnCloseAll.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCloseAll.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnCloseAll.Location = new System.Drawing.Point(119, 450);
            this.btnCloseAll.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnCloseAll.Name = "btnCloseAll";
            this.btnCloseAll.Size = new System.Drawing.Size(91, 24);
            this.btnCloseAll.TabIndex = 1;
            this.btnCloseAll.Text = "Close all";
            this.btnCloseAll.UseVisualStyleBackColor = true;
            this.btnCloseAll.Click += new System.EventHandler(this.closeAll_Click);
            // 
            // btnOpenAll
            // 
            this.btnOpenAll.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnOpenAll.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnOpenAll.Location = new System.Drawing.Point(14, 450);
            this.btnOpenAll.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnOpenAll.Name = "btnOpenAll";
            this.btnOpenAll.Size = new System.Drawing.Size(91, 24);
            this.btnOpenAll.TabIndex = 0;
            this.btnOpenAll.Text = "Open all";
            this.btnOpenAll.UseVisualStyleBackColor = true;
            this.btnOpenAll.Click += new System.EventHandler(this.openAll_Click);
            // 
            // AssetData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ClientSize = new System.Drawing.Size(434, 481);
            this.Controls.Add(this.btnOpenAll);
            this.Controls.Add(this.btnCloseAll);
            this.Controls.Add(this.btnCloseDown);
            this.Controls.Add(this.btnOpenDown);
            this.Controls.Add(this.tcDataView);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AssetData";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Asset Data";
            this.tcDataView.ResumeLayout(false);
            this.tpTreeView.ResumeLayout(false);
            this.tpDumpView.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tcDataView;
        private System.Windows.Forms.TabPage tpTreeView;
        private System.Windows.Forms.TabPage tpDumpView;
        private System.Windows.Forms.TreeView rawViewTree;
        private System.Windows.Forms.TreeView prettifiedViewTree;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCloseDown;
        private System.Windows.Forms.Button btnOpenDown;
        private System.Windows.Forms.Button btnCloseAll;
        private System.Windows.Forms.Button btnOpenAll;
    }
}