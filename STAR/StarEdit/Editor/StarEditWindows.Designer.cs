namespace StarEdit
{
    partial class StarEditWindows
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StarEditWindows));
			this.UpdateWorker = new System.ComponentModel.BackgroundWorker();
			this.statusStrip = new System.Windows.Forms.StatusStrip();
			this.BottomPanel = new System.Windows.Forms.Panel();
			this.menuStrip = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.editorsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.enemyEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.enemyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.newEnemyToolMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.OpenMapDialog = new System.Windows.Forms.OpenFileDialog();
			this.SaveMapDialog = new System.Windows.Forms.SaveFileDialog();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
			this.SingleSetTool = new System.Windows.Forms.ToolStripButton();
			this.SetRectangleTool = new System.Windows.Forms.ToolStripButton();
			this.panel1 = new System.Windows.Forms.Panel();
			this.BottomPanel.SuspendLayout();
			this.menuStrip.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// statusStrip
			// 
			this.statusStrip.Dock = System.Windows.Forms.DockStyle.Fill;
			this.statusStrip.Location = new System.Drawing.Point(0, 0);
			this.statusStrip.Name = "statusStrip";
			this.statusStrip.Size = new System.Drawing.Size(742, 25);
			this.statusStrip.TabIndex = 0;
			// 
			// BottomPanel
			// 
			this.BottomPanel.Controls.Add(this.statusStrip);
			this.BottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.BottomPanel.Location = new System.Drawing.Point(0, 494);
			this.BottomPanel.Name = "BottomPanel";
			this.BottomPanel.Size = new System.Drawing.Size(742, 25);
			this.BottomPanel.TabIndex = 2;
			// 
			// menuStrip
			// 
			this.menuStrip.Dock = System.Windows.Forms.DockStyle.Fill;
			this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editorsToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.openToolStripMenuItem1});
			this.menuStrip.Location = new System.Drawing.Point(0, 0);
			this.menuStrip.Name = "menuStrip";
			this.menuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.menuStrip.Size = new System.Drawing.Size(742, 24);
			this.menuStrip.TabIndex = 0;
			this.menuStrip.Text = "MainMenu";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.quitToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
			this.fileToolStripMenuItem.Text = "Map";
			// 
			// newToolStripMenuItem
			// 
			this.newToolStripMenuItem.Name = "newToolStripMenuItem";
			this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
			this.newToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
			this.newToolStripMenuItem.Text = "New";
			this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
			// 
			// openToolStripMenuItem
			// 
			this.openToolStripMenuItem.Name = "openToolStripMenuItem";
			this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.openToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
			this.openToolStripMenuItem.Text = "Open";
			this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
			// 
			// quitToolStripMenuItem
			// 
			this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
			this.quitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.Q)));
			this.quitToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
			this.quitToolStripMenuItem.Text = "Quit";
			this.quitToolStripMenuItem.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
			// 
			// editorsToolStripMenuItem
			// 
			this.editorsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.enemyEditorToolStripMenuItem});
			this.editorsToolStripMenuItem.Name = "editorsToolStripMenuItem";
			this.editorsToolStripMenuItem.Size = new System.Drawing.Size(55, 20);
			this.editorsToolStripMenuItem.Text = "Editors";
			// 
			// enemyEditorToolStripMenuItem
			// 
			this.enemyEditorToolStripMenuItem.Name = "enemyEditorToolStripMenuItem";
			this.enemyEditorToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
			this.enemyEditorToolStripMenuItem.Text = "EnemyEditor";
			this.enemyEditorToolStripMenuItem.Click += new System.EventHandler(this.enemyEditorToolStripMenuItem_Click);
			// 
			// viewToolStripMenuItem
			// 
			this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
			this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
			this.viewToolStripMenuItem.Text = "View";
			// 
			// openToolStripMenuItem1
			// 
			this.openToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.enemyToolStripMenuItem});
			this.openToolStripMenuItem1.Name = "openToolStripMenuItem1";
			this.openToolStripMenuItem1.Size = new System.Drawing.Size(48, 20);
			this.openToolStripMenuItem1.Text = "Open";
			// 
			// enemyToolStripMenuItem
			// 
			this.enemyToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newEnemyToolMenuItem,
            this.toolStripSeparator2});
			this.enemyToolStripMenuItem.Name = "enemyToolStripMenuItem";
			this.enemyToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
			this.enemyToolStripMenuItem.Text = "Enemy";
			// 
			// newEnemyToolMenuItem
			// 
			this.newEnemyToolMenuItem.Name = "newEnemyToolMenuItem";
			this.newEnemyToolMenuItem.Size = new System.Drawing.Size(107, 22);
			this.newEnemyToolMenuItem.Text = "New...";
			this.newEnemyToolMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem1_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(104, 6);
			// 
			// OpenMapDialog
			// 
			this.OpenMapDialog.DefaultExt = "map";
			this.OpenMapDialog.FileName = "Level";
			this.OpenMapDialog.ReadOnlyChecked = true;
			this.OpenMapDialog.SupportMultiDottedExtensions = true;
			this.OpenMapDialog.Title = "Open Map";
			this.OpenMapDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.OpenMapDialog_FileOk);
			// 
			// SaveMapDialog
			// 
			this.SaveMapDialog.DefaultExt = "map";
			// 
			// toolStrip1
			// 
			this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.SingleSetTool,
            this.SetRectangleTool});
			this.toolStrip1.Location = new System.Drawing.Point(0, 24);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.toolStrip1.Size = new System.Drawing.Size(742, 25);
			this.toolStrip1.TabIndex = 5;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// toolStripLabel1
			// 
			this.toolStripLabel1.Name = "toolStripLabel1";
			this.toolStripLabel1.Size = new System.Drawing.Size(36, 22);
			this.toolStripLabel1.Text = "Tools";
			// 
			// SingleSetTool
			// 
			this.SingleSetTool.Checked = true;
			this.SingleSetTool.CheckOnClick = true;
			this.SingleSetTool.CheckState = System.Windows.Forms.CheckState.Checked;
			this.SingleSetTool.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.SingleSetTool.Image = global::StarEdit.Properties.Resources.arrow;
			this.SingleSetTool.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.SingleSetTool.Name = "SingleSetTool";
			this.SingleSetTool.Size = new System.Drawing.Size(23, 22);
			this.SingleSetTool.Text = "Single";
			this.SingleSetTool.Click += new System.EventHandler(this.Tool_Click);
			// 
			// SetRectangleTool
			// 
			this.SetRectangleTool.CheckOnClick = true;
			this.SetRectangleTool.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.SetRectangleTool.Image = global::StarEdit.Properties.Resources.rectangle;
			this.SetRectangleTool.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.SetRectangleTool.Name = "SetRectangleTool";
			this.SetRectangleTool.Size = new System.Drawing.Size(23, 22);
			this.SetRectangleTool.Text = "Rectangle";
			this.SetRectangleTool.Click += new System.EventHandler(this.Tool_Click);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.menuStrip);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(742, 24);
			this.panel1.TabIndex = 4;
			// 
			// StarEditWindows
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(742, 519);
			this.Controls.Add(this.toolStrip1);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.BottomPanel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.IsMdiContainer = true;
			this.MainMenuStrip = this.menuStrip;
			this.Name = "StarEditWindows";
			this.Text = "StarEdit";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.Load += new System.EventHandler(this.StarEditWindows_Load);
			this.MdiChildActivate += new System.EventHandler(this.StarEditWindows_MdiChildActivate);
			this.LocationChanged += new System.EventHandler(this.StarEditWindows_LocationChanged);
			this.Resize += new System.EventHandler(this.StarEditWindows_Resize);
			this.BottomPanel.ResumeLayout(false);
			this.BottomPanel.PerformLayout();
			this.menuStrip.ResumeLayout(false);
			this.menuStrip.PerformLayout();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.ComponentModel.BackgroundWorker UpdateWorker;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog OpenMapDialog;
        private System.Windows.Forms.Panel BottomPanel;
        private System.Windows.Forms.SaveFileDialog SaveMapDialog;
        private System.Windows.Forms.ToolStripMenuItem editorsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem enemyEditorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripButton SingleSetTool;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripButton SetRectangleTool;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem enemyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newEnemyToolMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    }
}

