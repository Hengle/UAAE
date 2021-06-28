
namespace AssetsAdvancedEditor.Winforms.AssetSearch
{
    partial class AssetNameSearch
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AssetNameSearch));
            this.boxName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.lblDirection = new System.Windows.Forms.Label();
            this.rbtnUp = new System.Windows.Forms.RadioButton();
            this.rbtnDown = new System.Windows.Forms.RadioButton();
            this.lblCaseSensitive = new System.Windows.Forms.Label();
            this.chboxCaseSensitive = new System.Windows.Forms.CheckBox();
            this.chboxStartAtSelection = new System.Windows.Forms.CheckBox();
            this.lblStartAtSelection = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // boxName
            // 
            this.boxName.Location = new System.Drawing.Point(108, 15);
            this.boxName.Name = "boxName";
            this.boxName.Size = new System.Drawing.Size(227, 23);
            this.boxName.TabIndex = 0;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(9, 18);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(39, 15);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "Name";
            // 
            // lblDirection
            // 
            this.lblDirection.AutoSize = true;
            this.lblDirection.Location = new System.Drawing.Point(9, 46);
            this.lblDirection.Name = "lblDirection";
            this.lblDirection.Size = new System.Drawing.Size(55, 15);
            this.lblDirection.TabIndex = 2;
            this.lblDirection.Text = "Direction";
            // 
            // rbtnUp
            // 
            this.rbtnUp.AutoSize = true;
            this.rbtnUp.Location = new System.Drawing.Point(108, 44);
            this.rbtnUp.Name = "rbtnUp";
            this.rbtnUp.Size = new System.Drawing.Size(40, 19);
            this.rbtnUp.TabIndex = 3;
            this.rbtnUp.Text = "Up";
            this.rbtnUp.UseVisualStyleBackColor = true;
            // 
            // rbtnDown
            // 
            this.rbtnDown.AutoSize = true;
            this.rbtnDown.Checked = true;
            this.rbtnDown.Location = new System.Drawing.Point(152, 44);
            this.rbtnDown.Name = "rbtnDown";
            this.rbtnDown.Size = new System.Drawing.Size(56, 19);
            this.rbtnDown.TabIndex = 4;
            this.rbtnDown.TabStop = true;
            this.rbtnDown.Text = "Down";
            this.rbtnDown.UseVisualStyleBackColor = true;
            // 
            // lblCaseSensitive
            // 
            this.lblCaseSensitive.AutoSize = true;
            this.lblCaseSensitive.Location = new System.Drawing.Point(9, 74);
            this.lblCaseSensitive.Name = "lblCaseSensitive";
            this.lblCaseSensitive.Size = new System.Drawing.Size(80, 15);
            this.lblCaseSensitive.TabIndex = 5;
            this.lblCaseSensitive.Text = "Case sensitive";
            // 
            // chboxCaseSensitive
            // 
            this.chboxCaseSensitive.AutoSize = true;
            this.chboxCaseSensitive.Location = new System.Drawing.Point(108, 75);
            this.chboxCaseSensitive.Name = "chboxCaseSensitive";
            this.chboxCaseSensitive.Size = new System.Drawing.Size(15, 14);
            this.chboxCaseSensitive.TabIndex = 6;
            this.chboxCaseSensitive.UseVisualStyleBackColor = true;
            // 
            // chboxStartAtSelection
            // 
            this.chboxStartAtSelection.AutoSize = true;
            this.chboxStartAtSelection.Location = new System.Drawing.Point(108, 103);
            this.chboxStartAtSelection.Name = "chboxStartAtSelection";
            this.chboxStartAtSelection.Size = new System.Drawing.Size(15, 14);
            this.chboxStartAtSelection.TabIndex = 8;
            this.chboxStartAtSelection.UseVisualStyleBackColor = true;
            // 
            // lblStartAtSelection
            // 
            this.lblStartAtSelection.AutoSize = true;
            this.lblStartAtSelection.Location = new System.Drawing.Point(9, 102);
            this.lblStartAtSelection.Name = "lblStartAtSelection";
            this.lblStartAtSelection.Size = new System.Drawing.Size(94, 15);
            this.lblStartAtSelection.TabIndex = 7;
            this.lblStartAtSelection.Text = "Start at selection";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(251, 129);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(84, 25);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(9, 129);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(84, 25);
            this.btnOK.TabIndex = 9;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // AssetNameSearch
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(347, 166);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.chboxStartAtSelection);
            this.Controls.Add(this.lblStartAtSelection);
            this.Controls.Add(this.chboxCaseSensitive);
            this.Controls.Add(this.lblCaseSensitive);
            this.Controls.Add(this.rbtnDown);
            this.Controls.Add(this.rbtnUp);
            this.Controls.Add(this.lblDirection);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.boxName);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "AssetNameSearch";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Search";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox boxName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblDirection;
        private System.Windows.Forms.RadioButton rbtnUp;
        private System.Windows.Forms.RadioButton rbtnDown;
        private System.Windows.Forms.Label lblCaseSensitive;
        private System.Windows.Forms.CheckBox chboxCaseSensitive;
        private System.Windows.Forms.CheckBox chboxStartAtSelection;
        private System.Windows.Forms.Label lblStartAtSelection;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
    }
}