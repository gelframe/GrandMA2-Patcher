namespace GelFrame
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            this.loadFixtureTypesButton = new System.Windows.Forms.Button();
            this.loadExcelButton = new System.Windows.Forms.Button();
            this.patchFileLabel = new System.Windows.Forms.Label();
            this.patchFileTextBox = new System.Windows.Forms.TextBox();
            this.fixtureDataGrid = new System.Windows.Forms.DataGridView();
            this.dataColumName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataColumnProfile = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.browseButton = new System.Windows.Forms.Button();
            this.processButton = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.fixtureDataGrid)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // loadFixtureTypesButton
            // 
            this.loadFixtureTypesButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.loadFixtureTypesButton.Location = new System.Drawing.Point(818, 134);
            this.loadFixtureTypesButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.loadFixtureTypesButton.Name = "loadFixtureTypesButton";
            this.loadFixtureTypesButton.Size = new System.Drawing.Size(153, 63);
            this.loadFixtureTypesButton.TabIndex = 14;
            this.loadFixtureTypesButton.Text = "Load Fixture Types";
            this.loadFixtureTypesButton.UseVisualStyleBackColor = true;
            this.loadFixtureTypesButton.Click += new System.EventHandler(this.LoadFixtureTypesButton_Click);
            // 
            // loadExcelButton
            // 
            this.loadExcelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.loadExcelButton.Location = new System.Drawing.Point(818, 84);
            this.loadExcelButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.loadExcelButton.Name = "loadExcelButton";
            this.loadExcelButton.Size = new System.Drawing.Size(153, 45);
            this.loadExcelButton.TabIndex = 13;
            this.loadExcelButton.Text = "Load Excel Data";
            this.loadExcelButton.UseVisualStyleBackColor = true;
            this.loadExcelButton.Click += new System.EventHandler(this.LoadExcelButton_Click);
            // 
            // patchFileLabel
            // 
            this.patchFileLabel.AutoSize = true;
            this.patchFileLabel.Location = new System.Drawing.Point(8, 46);
            this.patchFileLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.patchFileLabel.Name = "patchFileLabel";
            this.patchFileLabel.Size = new System.Drawing.Size(83, 20);
            this.patchFileLabel.TabIndex = 18;
            this.patchFileLabel.Text = "Patch File:";
            // 
            // patchFileTextBox
            // 
            this.patchFileTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.patchFileTextBox.Location = new System.Drawing.Point(93, 44);
            this.patchFileTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.patchFileTextBox.Name = "patchFileTextBox";
            this.patchFileTextBox.Size = new System.Drawing.Size(716, 26);
            this.patchFileTextBox.TabIndex = 17;
            // 
            // fixtureDataGrid
            // 
            this.fixtureDataGrid.AllowDrop = true;
            this.fixtureDataGrid.AllowUserToAddRows = false;
            this.fixtureDataGrid.AllowUserToDeleteRows = false;
            this.fixtureDataGrid.AllowUserToResizeRows = false;
            this.fixtureDataGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fixtureDataGrid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.fixtureDataGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.fixtureDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.fixtureDataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataColumName,
            this.dataColumnProfile});
            this.fixtureDataGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.fixtureDataGrid.Location = new System.Drawing.Point(12, 82);
            this.fixtureDataGrid.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.fixtureDataGrid.MultiSelect = false;
            this.fixtureDataGrid.Name = "fixtureDataGrid";
            this.fixtureDataGrid.RowHeadersVisible = false;
            this.fixtureDataGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.fixtureDataGrid.Size = new System.Drawing.Size(798, 417);
            this.fixtureDataGrid.TabIndex = 0;
            this.fixtureDataGrid.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.FixtureDataGrid_CellEnter);
            // 
            // dataColumName
            // 
            this.dataColumName.HeaderText = "Fixture Name";
            this.dataColumName.MinimumWidth = 100;
            this.dataColumName.Name = "dataColumName";
            this.dataColumName.ReadOnly = true;
            this.dataColumName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataColumName.Width = 230;
            // 
            // dataColumnProfile
            // 
            this.dataColumnProfile.HeaderText = "Profile";
            this.dataColumnProfile.MinimumWidth = 100;
            this.dataColumnProfile.Name = "dataColumnProfile";
            this.dataColumnProfile.Width = 300;
            // 
            // browseButton
            // 
            this.browseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.browseButton.Location = new System.Drawing.Point(818, 34);
            this.browseButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new System.Drawing.Size(153, 45);
            this.browseButton.TabIndex = 23;
            this.browseButton.Text = "Browse";
            this.browseButton.UseVisualStyleBackColor = true;
            this.browseButton.Click += new System.EventHandler(this.BrowseButton_Click);
            // 
            // processButton
            // 
            this.processButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.processButton.Location = new System.Drawing.Point(818, 200);
            this.processButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.processButton.Name = "processButton";
            this.processButton.Size = new System.Drawing.Size(153, 45);
            this.processButton.TabIndex = 24;
            this.processButton.Text = "Process";
            this.processButton.UseVisualStyleBackColor = true;
            this.processButton.Click += new System.EventHandler(this.ProcessButton_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(988, 33);
            this.menuStrip1.TabIndex = 30;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(50, 29);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(195, 30);
            this.exitToolStripMenuItem.Text = "Quit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(88, 29);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.SettingsButton_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(61, 29);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(252, 30);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.AboutToolStripMenuItem_Click);
            // 
            // statusBox
            // 
            this.statusBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.statusBox.Location = new System.Drawing.Point(12, 508);
            this.statusBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.statusBox.Multiline = true;
            this.statusBox.Name = "statusBox";
            this.statusBox.ReadOnly = true;
            this.statusBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.statusBox.Size = new System.Drawing.Size(956, 126);
            this.statusBox.TabIndex = 31;
            this.statusBox.TextChanged += new System.EventHandler(this.StatusBox_TextChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(988, 652);
            this.Controls.Add(this.statusBox);
            this.Controls.Add(this.fixtureDataGrid);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.processButton);
            this.Controls.Add(this.browseButton);
            this.Controls.Add(this.patchFileLabel);
            this.Controls.Add(this.patchFileTextBox);
            this.Controls.Add(this.loadFixtureTypesButton);
            this.Controls.Add(this.loadExcelButton);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(1002, 602);
            this.Name = "MainForm";
            this.Text = "GelFrame GrandMA2 Patcher - Beta V0.1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.fixtureDataGrid)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button loadFixtureTypesButton;
        private System.Windows.Forms.Button loadExcelButton;
        private System.Windows.Forms.Label patchFileLabel;
        private System.Windows.Forms.TextBox patchFileTextBox;
        private System.Windows.Forms.Button browseButton;
        private System.Windows.Forms.Button processButton;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.DataGridView fixtureDataGrid;
        private System.Windows.Forms.TextBox statusBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataColumName;
        private System.Windows.Forms.DataGridViewComboBoxColumn dataColumnProfile;
    }
}

