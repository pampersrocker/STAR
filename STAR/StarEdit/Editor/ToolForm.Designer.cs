namespace StarEdit.Editor
{
    partial class ToolForm
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
			this.listBox1 = new System.Windows.Forms.ListBox();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.enemyListBox = new System.Windows.Forms.ListBox();
			this.toolControl = new System.Windows.Forms.TabControl();
			this.ToolPanel = new System.Windows.Forms.Panel();
			this.rescanButton = new System.Windows.Forms.Button();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.toolControl.SuspendLayout();
			this.ToolPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// listBox1
			// 
			this.listBox1.Dock = System.Windows.Forms.DockStyle.Top;
			this.listBox1.FormattingEnabled = true;
			this.listBox1.Location = new System.Drawing.Point(3, 3);
			this.listBox1.Name = "listBox1";
			this.listBox1.Size = new System.Drawing.Size(102, 134);
			this.listBox1.TabIndex = 0;
			this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.listBox1);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(108, 146);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Tiles";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.rescanButton);
			this.tabPage2.Controls.Add(this.enemyListBox);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(108, 146);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Enemies";
			this.tabPage2.UseVisualStyleBackColor = true;
			this.tabPage2.Visible = false;
			// 
			// enemyListBox
			// 
			this.enemyListBox.FormattingEnabled = true;
			this.enemyListBox.Location = new System.Drawing.Point(3, 6);
			this.enemyListBox.Name = "enemyListBox";
			this.enemyListBox.Size = new System.Drawing.Size(99, 95);
			this.enemyListBox.TabIndex = 0;
			this.enemyListBox.SelectedIndexChanged += new System.EventHandler(this.enemyListBox_SelectedIndexChanged);
			// 
			// toolControl
			// 
			this.toolControl.Controls.Add(this.tabPage1);
			this.toolControl.Controls.Add(this.tabPage2);
			this.toolControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.toolControl.Location = new System.Drawing.Point(0, 0);
			this.toolControl.Name = "toolControl";
			this.toolControl.SelectedIndex = 0;
			this.toolControl.Size = new System.Drawing.Size(116, 172);
			this.toolControl.TabIndex = 0;
			this.toolControl.SelectedIndexChanged += new System.EventHandler(this.toolControl_SelectedIndexChanged);
			// 
			// ToolPanel
			// 
			this.ToolPanel.Controls.Add(this.toolControl);
			this.ToolPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ToolPanel.Location = new System.Drawing.Point(0, 0);
			this.ToolPanel.Name = "ToolPanel";
			this.ToolPanel.Size = new System.Drawing.Size(116, 172);
			this.ToolPanel.TabIndex = 1;
			// 
			// rescanButton
			// 
			this.rescanButton.Location = new System.Drawing.Point(6, 115);
			this.rescanButton.Name = "rescanButton";
			this.rescanButton.Size = new System.Drawing.Size(92, 23);
			this.rescanButton.TabIndex = 2;
			this.rescanButton.Text = "Rescan";
			this.rescanButton.UseVisualStyleBackColor = true;
			// 
			// ToolForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(116, 172);
			this.Controls.Add(this.ToolPanel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ToolForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "ToolBox";
			this.TopMost = true;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ToolForm_FormClosing);
			this.tabPage1.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			this.toolControl.ResumeLayout(false);
			this.ToolPanel.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabControl toolControl;
        private System.Windows.Forms.Panel ToolPanel;
		private System.Windows.Forms.ListBox enemyListBox;
		private System.Windows.Forms.Button rescanButton;
    }
}