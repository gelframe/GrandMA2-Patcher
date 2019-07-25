namespace GelFrame.Settings
{
    partial class IpForm
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
            this.ipListBox = new System.Windows.Forms.ListBox();
            this.selectButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ipListBox
            // 
            this.ipListBox.FormattingEnabled = true;
            this.ipListBox.ItemHeight = 20;
            this.ipListBox.Location = new System.Drawing.Point(12, 16);
            this.ipListBox.Name = "ipListBox";
            this.ipListBox.Size = new System.Drawing.Size(378, 284);
            this.ipListBox.TabIndex = 0;
            this.ipListBox.DoubleClick += new System.EventHandler(this.SelectButton_Click);
            // 
            // selectButton
            // 
            this.selectButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.selectButton.Location = new System.Drawing.Point(255, 309);
            this.selectButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.selectButton.Name = "selectButton";
            this.selectButton.Size = new System.Drawing.Size(135, 41);
            this.selectButton.TabIndex = 17;
            this.selectButton.Text = "Select";
            this.selectButton.UseVisualStyleBackColor = true;
            this.selectButton.Click += new System.EventHandler(this.SelectButton_Click);
            // 
            // IpForm
            // 
            this.AcceptButton = this.selectButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(402, 361);
            this.Controls.Add(this.selectButton);
            this.Controls.Add(this.ipListBox);
            this.MaximumSize = new System.Drawing.Size(424, 417);
            this.MinimumSize = new System.Drawing.Size(424, 417);
            this.Name = "IpForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select IP Address";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox ipListBox;
        private System.Windows.Forms.Button selectButton;
    }
}