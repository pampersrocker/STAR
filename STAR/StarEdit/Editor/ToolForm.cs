using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Star.Game.Level;
using StarEdit.LevelEditor;

namespace StarEdit.Editor
{

    public partial class ToolForm : Form
    {
		Tool selectedTool = Tool.Tiles;
        bool hiding = false;
        public delegate void HidingChanged(bool phiding);
        HidingChanged hidedelegate;
        public delegate void SelectedEnemyChangedEventHandler(string newEnemy);

        public event SelectedEnemyChangedEventHandler SelectedEnemyChanged;

		public delegate void SelectedToolChangedEventHandler(Tool tool);

		public event SelectedToolChangedEventHandler SelectedToolChanged;

        public ToolForm(HidingChanged hideMethod)
        {
            InitializeComponent();
            hidedelegate = hideMethod;
        }

		public Tool SelectedTool
		{
			get { return selectedTool; }
		}

        public bool IsHiding
        {
            get { return hiding; }
            set { hiding = value; }
        }

        public ListBox TileBox
        {
            get { return listBox1; }
            set { listBox1 = value; }
        }

        public int TileSelectedIndex
        {
            get { return listBox1.SelectedIndex; }
        }

        public string SelectedTileString
        {
            get { return listBox1.SelectedItem.ToString(); }
        }

		public string SelectedEnemy
		{
			get { return enemyListBox.SelectedItem.ToString(); }
		}

        public void SetTileListBox(List<TileType> Tiles)
        {
            foreach (TileType tile in Enum.GetValues(typeof(TileType)))
            {
                if (tile != TileType.Error)
                {
                    listBox1.Items.Add(tile);
                }
            }
            listBox1.SelectedIndex = 0;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
        }

        private void ToolForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            hiding = true;
            hidedelegate(hiding);
            Hide();
        }

        public void SetEnemies(string[] enemies)
        {
            if (enemies != null)
            {
                if (enemies.Length >= 1)
                {
                    enemyListBox.Items.Clear();
                    foreach (string enemy in enemies)
                    {
                        enemyListBox.Items.Add(enemy);
                    }
                    enemyListBox.SelectedIndex = 0;
                }
            }
        }

        private void rescanButton_Click(object sender, EventArgs e)
        {

        }

		private void enemyListBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (SelectedEnemyChanged != null)
			{
				SelectedEnemyChanged(enemyListBox.SelectedItem.ToString());
			}
		}



		private void toolControl_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (SelectedToolChanged != null)
			{
				switch (toolControl.SelectedIndex)
				{
					case 0:
						SelectedToolChanged(Tool.Tiles);
						break;
					case 1:
						SelectedToolChanged(Tool.Enemies);
						break;
				}
			}
		}
    }
}
