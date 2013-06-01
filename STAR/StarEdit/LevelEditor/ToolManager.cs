using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StarEdit.Editor;
using Star.Game.Level;
using System.Windows.Forms;
using System.IO;
using Star.GameManagement;
using Star.Game.Level.InteractiveObjects;

namespace StarEdit.LevelEditor
{
	public enum Tool
	{
		Tiles,
		Enemies,
		Decoration,
		IObject
	}

	public enum TileToolType
	{
		Single,
		Rectangle
	}

	public delegate void CurrentTileToolTypeChangedEventHandler(TileToolType newType);

	public delegate void CurrentTileTypeChangedEventHandler(TileType type);

	public delegate void CurrentDecorationChangedEventHandler(string currentDecoration);

	public delegate void CurrentDecorationLayerChangedEventHandler(DecorationsLayer newLayer);

	public class ToolManager
	{
		#region Fields

		List<string> enemies;
		TileType currentType = TileType.Empty;
		Tool selectedTool = Tool.Tiles;
		TileToolType currentTileType = TileToolType.Single;
		int selectedEnemyIndex = 0;
		string currentDecoration;
		DecorationsLayer currentLayer;

		float[] iObjectParameters = new float[10];
		string currentIObject = InteractiveObjectsIDs.JumpPad.ToString();

		#endregion

		#region Events

		public delegate void CurrentEnemyChangedEventHandler(string newEnemy);

        public event CurrentEnemyChangedEventHandler SelectedEnemyChanged;

		public delegate void CurrentToolChangedEventHandler(Tool tool);

		public delegate void CurrentIObjectChangedEventHandler(string iObject, float[] parameters);

		public event CurrentIObjectChangedEventHandler CurrentIObjectChanged;

		public event CurrentToolChangedEventHandler SelectedToolChanged;

		public event CurrentTileToolTypeChangedEventHandler CurrentTileToolTypeChanged;

		public event CurrentTileTypeChangedEventHandler CurrentTileTypeChanged;

		public event CurrentDecorationChangedEventHandler CurrentDecorationChanged;

		public event CurrentDecorationLayerChangedEventHandler CurrentDecorationsLayerChanged;

		#endregion

		#region Attributes

		public string CurrentDecoration
		{
			get { return currentDecoration; }
		}

		public DecorationsLayer CurrentLayer
		{
			get { return currentLayer; }
		}

		public Tool CurrentTool
		{
			get { return selectedTool; }
		}

		public ToolManager()
		{
			this.enemies = new List<string>();
			LoadExistingEnemies();
		}

		public string CurrentEnemy
		{
			get { return enemies[selectedEnemyIndex]; }
		}

		#endregion

		#region FormInteractions

		public void tabControlTools_TabIndexChanged(object sender, EventArgs e)
		{
			TabControl control = (TabControl)sender;
			switch (control.SelectedTab.Text)
			{ 
				case "Layers":
					selectedTool = Tool.Decoration;
					SelectedToolChanged(selectedTool);
					break;
				case "Level":
					selectedTool = Tool.Tiles;
					SelectedToolChanged(selectedTool);
					break;
				case "Enemies":
					selectedTool = Tool.Enemies;
					SelectedToolChanged(selectedTool);
					SelectedEnemyChanged(CurrentEnemy);
					break;
				case "IObjects":
					selectedTool = Tool.IObject;
					SelectedToolChanged(selectedTool);
					CurrentIObjectChanged(currentIObject, iObjectParameters);
					break;
			}
		}

		public void enemyListBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			ListBox box = (ListBox)sender;
			if (SelectedEnemyChanged != null)
			{
				SelectedEnemyChanged(box.SelectedItem.ToString());
			}
		}

		public void listBoxTiles_SelectedIndexChanged(object sender, EventArgs e)
		{
			ListBox box = (ListBox)sender;
			currentType = (TileType)box.SelectedItem;
			CurrentTileTypeChanged(currentType);

		}

		public void comboBoxDecoLayer_SelectedIndexChanged(object sender, EventArgs e)
		{
			ComboBox box = (ComboBox)sender;
			currentLayer = (DecorationsLayer)box.SelectedItem;
			CurrentDecorationsLayerChanged(currentLayer);
		}

		public void listBoxAvailableDecos_SelectedIndexChanged(object sender, EventArgs e)
		{
			ListBox box = (ListBox)sender;
			currentDecoration = box.SelectedItem.ToString();
			CurrentDecorationChanged(currentDecoration);
		}

		public void listBoxIObjects_SelectedIndexChanged(object sender, EventArgs e)
		{
			ListBox box = (ListBox)sender;
			currentIObject = box.SelectedItem.ToString();
			CurrentIObjectChanged(currentIObject, iObjectParameters);
		}

		public void numericUpDownJumpPadValues_ValueChanged(float rotation, float strength, float time)
		{
			iObjectParameters = new float[3];
			iObjectParameters[0] = rotation;
			iObjectParameters[1] = strength;
			iObjectParameters[2] = time;
		}

		#endregion

		public void TileToolTypeChanged(TileToolType type)
		{
			currentTileType = type;
			CurrentTileToolTypeChanged(currentTileType);
		}

		private void LoadExistingEnemies()
		{
			string[] existingEnemies = Directory.GetDirectories("Data/" + GameConstants.EnemiesPath, "*", SearchOption.TopDirectoryOnly);
			for (int i = 0; i < existingEnemies.Length; i++)
			{
				string[] data = (existingEnemies[i].Split('/'));
				existingEnemies[i] = data[data.Length - 1];
				ToolStripMenuItem item = new ToolStripMenuItem(existingEnemies[i]);
			}
			enemies.AddRange(existingEnemies);

		}


	}
}
