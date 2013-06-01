using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Star.Game.Level;
using Star.Game;
using StarEdit.LevelEditor;
using StarEdit.Editor;
using StarEdit.EnemyEditor;
using System.IO;
using Star.GameManagement;

namespace StarEdit
{

    public partial class StarEditWindows : Form
    {
        public delegate void SetToolTypeDelegate(TileToolType type);
        List<SetToolTypeDelegate> settooltypedelegate;
        TileToolType tooltype = TileToolType.Single;
        ToolForm toolform;
        LevelMDIChild lastactivelevel;
        NewMapForm newmapform;
        int newlevelnumber = 1;
        EnemyEditor.EnemyEditor enemyeditor;
        public StarEditWindows()
        {
            InitializeComponent();
            toolform = new ToolForm(ToolBoxHideChanged);

            //toolform.MdiParent = this;
            //toolform.TopMost = true;

            SaveMapDialog.Filter = "Mapfile (*.map)|*.map";
            OpenMapDialog.Filter = "Mapfile (*.map)|*.map|All Files (*.*)|*.*";
            settooltypedelegate = new List<SetToolTypeDelegate>();
			LoadExistingEnemies();
        }

		private void LoadExistingEnemies()
		{
			string[] existingEnemies = Directory.GetDirectories("Data/" + GameConstants.EnemiesPath, "*", SearchOption.TopDirectoryOnly);
			for (int i = 0; i < existingEnemies.Length; i++)
			{
				string[] data = (existingEnemies[i].Split('/'));
				existingEnemies[i] = data[data.Length - 1];
				ToolStripMenuItem item = new ToolStripMenuItem(existingEnemies[i]);
				item.Click += new EventHandler(ExisingEnemyitem_Click);
				enemyToolStripMenuItem.DropDownItems.Add(item);
			}
            toolform.SetEnemies(existingEnemies);

		}

		void ExisingEnemyitem_Click(object sender, EventArgs e)
		{
			ToolStripMenuItem item = (ToolStripMenuItem)sender;
			LoadEnemy(item.Text);
			
		}

		private void OpenMapDialog_FileOk(object sender, CancelEventArgs e)
        {
            try
            {
                LevelMDIChild child = NewLevelMDI(OpenMapDialog.FileName);
                AddLevelViewButton(child);
                child.LoadLevel(OpenMapDialog.FileName);
                toolform.TileBox.SelectedIndexChanged += new EventHandler(child.SetMouseTile);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this,"Die Datei enthält keine oder falsche Map Daten\nAdditional Info:" + ex.Message,"Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private LevelMDIChild NewLevelMDI(string path)
        {
            string[] filename = path.Split('\\');
            
            LevelMDIChild level = new LevelMDIChild(filename[filename.Length-1]);
			SetLevelEvents(level);
            level.MdiParent = this;
            level.Show();
            return level;
        }

        private LevelMDIChild NewDefaultMDI(string name,int x,int y)
        {
            LevelMDIChild level = new LevelMDIChild(name);
            level.MdiParent = this;
            //level.SetFormLocation(Location);
            level.Show();
            level.NewLevel(x, y);
            //Adding SelectedIndexChanged Event from the TileBox in the Toolform
            //to the LevelMDIChildForm
			SetLevelEvents(level);
            return level;
        }

		private void SetLevelEvents(LevelMDIChild level)
		{
			//toolform.SelectedEnemyChanged += level.SelectedEnemyChanged;
			settooltypedelegate.Add(level.toolManager.TileToolTypeChanged);
			//toolform.TileBox.SelectedIndexChanged += new EventHandler(level.SetMouseTile);
			//toolform.SelectedToolChanged +=new ToolManager.CurrentToolChangedEventHandler(level.ToolChanged);
			//level.ToolChanged(toolform.SelectedTool);
			level.SetTool(tooltype);
			level.SelectedEnemyChanged(toolform.SelectedEnemy);
		}

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenMapDialog.ShowDialog();
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void StarEditWindows_LocationChanged(object sender, EventArgs e)
        {
            if (ActiveMdiChild != null)
            {
                if (ActiveMdiChild.GetType() == (typeof(LevelMDIChild)))
                {
                    LevelMDIChild child = (LevelMDIChild)ActiveMdiChild;
                    child.SetFormLocation(Location);
                }
                if (ActiveMdiChild.GetType() == (typeof(EnemyEditor.EnemyEditor)))
                {
                    EnemyEditor.EnemyEditor editor = (EnemyEditor.EnemyEditor)ActiveMdiChild;
                    editor.FormLocationChanged(Location);
                }
            }
        }

        private void TileListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //LevelControl.SetMouseTileType(TileListBox.Items[(int)TileListBox.SelectedIndex].ToString());
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SaveMapDialog.ShowDialog() == DialogResult.OK)
            {
                lastactivelevel.SaveLevel(SaveMapDialog.FileName);
            }

        }

        private void enemyEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            enemyeditor = new StarEdit.EnemyEditor.EnemyEditor();
            enemyeditor.MdiParent = this;
            enemyeditor.Show();
        }

        private void StarEditWindows_Load(object sender, EventArgs e)
        {
            List<TileType> tiles = new List<TileType>();
			this.Text = "StarEdit v." + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            //toolform.Show();
            //toolform.Location = new Point(
            //    Size.Width - toolform.Size.Width-50,
            //    100);
            //toolform.TopLevel = true;
            foreach (TileType type in Enum.GetValues(typeof(TileType)))
            {
                tiles.Add(type);
            }
            toolform.SetTileListBox(tiles);
        }

        private void ButtonMouseTile_Click(object sender, EventArgs e)
        {
        }

        private void StarEditWindows_MdiChildActivate(object sender, EventArgs e)
        {
			if(ActiveMdiChild != null)
				if (ActiveMdiChild.GetType() == (typeof(LevelMDIChild)))
				{
					LevelMDIChild child = (LevelMDIChild)ActiveMdiChild;
					child.SetMouseTile(toolform.SelectedTileString);
					lastactivelevel = child;
				}
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //LevelMDIChild child = NewDefaultMDI("Map"+newlevelnumber.ToString());
            //newlevelnumber++;
            newmapform = new NewMapForm();
            newmapform.Controls["NewButton"].Click += new EventHandler(NewClick);
            newmapform.Show();
        }

        private void NewClick(object sender, EventArgs e)
        {
            LevelMDIChild child = NewDefaultMDI("Map"+newlevelnumber.ToString(),newmapform.XSize,newmapform.YSize);
            newlevelnumber++;
            AddLevelViewButton(child);
        }

        private void StarEditWindows_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                //toolform.Hide();
            }
            else if(toolform.IsHiding == false)
            {
                //toolform.Show();
            }
        }

        void ToolBoxHideChanged(bool phiding)
        {
            //ToolBoxViewButton.Checked = !phiding;
        }

        private void toolBoxToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void AddLevelViewButton(LevelMDIChild child)
        {
            ToolStripMenuItem item = new ToolStripMenuItem(child.Text);
            item.Click += new EventHandler(child.FocusClick);
            child.SetToolStipMenuItem(item, RemoLevelViewButton);
            viewToolStripMenuItem.DropDownItems.Add(item);
        }

        private void RemoLevelViewButton(ToolStripMenuItem item)
        {
            viewToolStripMenuItem.DropDownItems.Remove(item);
        }

        private void Tool_Click(object sender, EventArgs e)
        {
            ToolStripButton button = (ToolStripButton)sender;
            if (button.Checked == false)
            {
                button.Checked = true;
            }
            DeselectOtherToolButtons(button);

            foreach(TileToolType type in Enum.GetValues(typeof(TileToolType)))
            {
                if (type.ToString() == button.Text)
                {
                    tooltype = type;
                }
            }

            foreach (SetToolTypeDelegate deleg in settooltypedelegate)
            {
                if (deleg.Target != null)
                {
                    deleg(tooltype);
                }
            }
        }

        private void DeselectOtherToolButtons(ToolStripButton button)
        {
            foreach (ToolStripItem item in toolStrip1.Items)
            {
                try
                {
                    ToolStripButton menuitem = (ToolStripButton)item;
                    if (menuitem != button)
                    {
                        menuitem.Checked = false;
                    }
                }
                catch { }
            }
        }

        private void newToolStripMenuItem1_Click(object sender, EventArgs e)
        {
			NewEnemyForm enemyform = new NewEnemyForm();
			enemyform.EnemyCreate += new NewEnemyForm.EnemyCreateEventHandler(enemyform_EnemyCreate);
			enemyform.ShowDialog(this);

        }

		private void LoadEnemy(string name)
		{
			EnemyEditor.EnemyEditor editor = new StarEdit.EnemyEditor.EnemyEditor();
			editor.MdiParent = this;
			editor.Show();
			editor.LoadEnemy(name);
		}

		private void enemyform_EnemyCreate(string name)
		{
			EnemyEditor.EnemyEditor editor = new StarEdit.EnemyEditor.EnemyEditor();
			
			editor.MdiParent = this;
			editor.Show();
			editor.NewEnemy(name);
		}
    }
}
