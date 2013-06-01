using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Star.Game.Level;
using Star.GameManagement;
using StarEdit.Editor;
using Microsoft.Xna.Framework;
using Star.Game.Level.InteractiveObjects;
using Star.Game.Enemy;

namespace StarEdit.LevelEditor
{
	public delegate void LayerFXDataChangedEventHandler(LayerFX layer,LayerFXData data);

    public delegate void DecoObjectDataChangedEventHandler(Microsoft.Xna.Framework.Rectangle orgRect,Vector2 origin,float rotation,float layerDepth);

    public partial class LevelMDIChild : Form
    {
        System.Drawing.Point formpos;
        public delegate void RemoveViewButton(ToolStripMenuItem item);
		public ToolManager toolManager;
        RemoveViewButton menuItemRemoveMethod;
        ToolStripMenuItem menuItem;
        StyleChangeForm stylechangeform;
        MapBoundsForm mapBoundsForm;
		bool changinvalues;
		bool drawAndUpdating;
		Dictionary<LayerFX, LayerFXData> layerColorData;
		List<LayerObject> rearCollection;
		List<LayerObject> frontCollection;
		List<Enemy> placedEnemies;
        Vector2 origin;
        float rotation;
		float layerDepth;
        Microsoft.Xna.Framework.Rectangle rect;

		public event LayerFXDataChangedEventHandler LayerFXDataChanged;

        public event DecoObjectDataChangedEventHandler DecoObjectDataChanged;

        public LevelControl Control
        {
            get { return levelControl1; }
            set { levelControl1 = value; }
        }

        public LevelMDIChild(string Title)
        {
			placedEnemies = new List<Enemy>();
			layerDepth = 0;
			drawAndUpdating = true;
            InitializeComponent();
			rearCollection = new List<LayerObject>();
			frontCollection = new List<LayerObject>();
			levelControl1.DecoAdded += new DecoChangedEventHandler(levelControl1_DecoAdded);
			levelControl1.DecoRotationChanged += new RotationScaleChangedEventHandler(levelControl1_DecoRotationChanged);
			levelControl1.DecoTextureChanged += new DecoTextureChangedEventHandler(levelControl1_DecoTextureChanged);
			levelControl1.EnemyPlaced += new EnemyPlacedEventHandler(levelControl1_EnemyPlaced);
            DecoObjectDataChanged += new DecoObjectDataChangedEventHandler(LevelMDIChild_DecoObjectDataChanged);
            DecoObjectDataChanged += new DecoObjectDataChangedEventHandler(levelControl1.DecoObjectDataChanged);
            Text = Title;
            SaveMapDialog.Filter = "Mapfile (*.map)|*.map";
			layerColorData = new Dictionary<LayerFX, LayerFXData>();
			foreach (LayerFX layer in Enum.GetValues(typeof(LayerFX)))
			{
				layerColorData.Add(layer, LayerFXData.Default);
			}
            Locationchanged();
			LayerFXDataChanged += new LayerFXDataChangedEventHandler(LevelMDIChild_LayerFXDataChanged);
			LayerFXDataChanged += new LayerFXDataChangedEventHandler(levelControl1.LayerDataChanged);
			foreach (LayerFX layer in Enum.GetValues(typeof(LayerFX)))
			{
				comboBoxLayer.Items.Add(layer);
			}
			comboBoxLayer.SelectedIndex = 0;
			levelControl1.IgnoreFocus = true;
			levelControl1.Activated = drawAndUpdating;

			toolManager = new ToolManager();
			toolManager.CurrentTileToolTypeChanged += new CurrentTileToolTypeChangedEventHandler(toolManager_CurrentTileToolTypeChanged);
			toolManager.SelectedEnemyChanged += new ToolManager.CurrentEnemyChangedEventHandler(toolManager_SelectedEnemyChanged);
			toolManager.SelectedToolChanged += new ToolManager.CurrentToolChangedEventHandler(toolManager_SelectedToolChanged);
			toolManager.CurrentTileTypeChanged += new CurrentTileTypeChangedEventHandler(toolManager_CurrentTileTypeChanged);
			toolManager.CurrentDecorationChanged += new CurrentDecorationChangedEventHandler(toolManager_CurrentDecorationChanged);
			toolManager.CurrentDecorationsLayerChanged += new CurrentDecorationLayerChangedEventHandler(toolManager_CurrentDecorationsLayerChanged);
			toolManager.CurrentIObjectChanged += new ToolManager.CurrentIObjectChangedEventHandler(toolManager_CurrentIObjectChanged);
			
			foreach (TileType tile in Enum.GetValues(typeof(TileType)))
			{
				if (tile != TileType.Error)
				{
					listBoxTiles.Items.Add(tile);
				}
			}

			listBoxTiles.SelectedIndex = 0;
			foreach (InteractiveObjectsIDs iD in Enum.GetValues(typeof(InteractiveObjectsIDs)))
			{
				listBoxIObjects.Items.Add(iD);
			}
			listBoxIObjects.SelectedIndex = 0;

			foreach(DecorationsLayer layer in Enum.GetValues(typeof(DecorationsLayer)))
			{
				comboBoxDecoLayer.Items.Add(layer);
			}
			comboBoxDecoLayer.SelectedIndex = 0;
		}

		void levelControl1_EnemyPlaced(List<Star.Game.Enemy.Enemy> enemies)
		{
			listViewPlacedEnemies.Items.Clear();
			placedEnemies.Clear();
			placedEnemies.AddRange(levelControl1.Enemymanager.Enemies);
			foreach (Enemy enemy in placedEnemies)
			{
				ListViewItem item = new ListViewItem(enemy.Type);
				item.SubItems.Add(enemy.Pos.ToString());
				listViewPlacedEnemies.Items.Add(item);
			}
		}

		void toolManager_CurrentIObjectChanged(string iObject, float[] parameters)
		{
			levelControl1.IObjectDataChanged(iObject, parameters);	
		}

		void levelControl1_DecoRotationChanged(float rotation,bool rotate)
		{
			if (rotate)
				rotationNumericUpDown.Value += (decimal)rotation;
			else
			{
				//xOriginNumericUpDown.Value *= (decimal)rotation;
				//yOriginNumericUpDown.Value *= (decimal)rotation;
				widthNumericUpDown.Value *=(decimal)rotation*(decimal)rotation;
				heightNumericUpDown.Value *=(decimal)rotation * (decimal)rotation;
				xPositionNumericUpDown.Value *= (decimal)rotation;
				yPositionNumericUpDown.Value *= (decimal)rotation;



			}
			//throw new NotImplementedException();
		}

		void levelControl1_DecoTextureChanged(Microsoft.Xna.Framework.Graphics.Texture2D tex)
		{
			widthNumericUpDown.Value = tex.Width;
			heightNumericUpDown.Value = tex.Height;
			xOriginNumericUpDown.Value = tex.Width / 2;
			yOriginNumericUpDown.Value = tex.Height / 2;
			xPositionNumericUpDown.Value = - tex.Width / 2;
			yPositionNumericUpDown.Value = - tex.Height / 2;
		}

        void LevelMDIChild_DecoObjectDataChanged(Microsoft.Xna.Framework.Rectangle orgRect, Vector2 origin, float rotation, float layerDepth)
        {
            //dummy...
            //Avoid null Reference on Event
            //throw new NotImplementedException();
        }

		void levelControl1_DecoAdded(LayerObject layerObject, DecorationsLayer layer)
		{
			ListViewItem item;
			switch (toolManager.CurrentLayer)
			{
				case DecorationsLayer.Front:
					frontCollection.Add(layerObject);
					item = new ListViewItem(layerObject.Texture.Name);
					item.SubItems.Add(layerObject.ExtendedRectangle.Translation.ToString());
					listViewPlacedDecos.Items.Add(item);
					break;
				case DecorationsLayer.Rear:
					rearCollection.Add(layerObject);
					item = new ListViewItem(layerObject.Texture.Name);
					item.SubItems.Add(layerObject.ExtendedRectangle.Translation.ToString());
					listViewPlacedDecos.Items.Add(item);
					break;
			}
		}

		void toolManager_CurrentDecorationsLayerChanged(DecorationsLayer newLayer)
		{
			levelControl1.SetDecorationLayer(newLayer);
		}

		void toolManager_CurrentDecorationChanged(string currentDecoration)
		{
			levelControl1.SetDecorationType(currentDecoration);
		}

		void toolManager_CurrentTileTypeChanged(TileType type)
		{
			SetMouseTile(type.ToString());
		}

		void toolManager_SelectedToolChanged(Tool tool)
		{
			ToolChanged(tool);		
		}

		void toolManager_SelectedEnemyChanged(string newEnemy)
		{
			SelectedEnemyChanged(newEnemy);
		}

		void toolManager_CurrentTileToolTypeChanged(TileToolType newType)
		{
			SetTool(newType);
		}

		void LevelMDIChild_LayerFXDataChanged(LayerFX layer, LayerFXData data)
		{
			//throw new NotImplementedException();
			levelControl1.Level.LevelVariables.Dictionary
				[(LV)Enum.Parse(typeof(LV),
					layer.ToString())] = data.DataString();
			layerColorData[layer] = data;
		}

        public void SetLevel(Tile[,] newtiles)
        {
            levelControl1.Level.Tiles = newtiles;
            levelControl1.SetBorders(newtiles.GetLength(1), newtiles.GetLength(0));
            //Focus();
        }

        /// <summary>
        /// Sets the View ToolStripMenuItem for this Form, 
        /// and the delegate to remove this when its get closed
        /// </summary>
        /// <param name="item">The MenuItem to destroy when it gets closed</param>
        /// <param name="removemethod">Will be called when the Forms is closed</param>
        public void SetToolStipMenuItem(ToolStripMenuItem item, RemoveViewButton removemethod)
        {
            menuItem = item;
            menuItemRemoveMethod = removemethod;
        }

        public void LoadLevel(string filename)
        {
            levelControl1.LoadLevel(filename);
			ParseLevelVariables(levelControl1.Level.LevelVariables.Dictionary);
			placedEnemies.AddRange(levelControl1.Enemymanager.Enemies);
			foreach (Enemy enemy in placedEnemies)
			{
				ListViewItem item = new ListViewItem(enemy.Type);
				item.SubItems.Add(enemy.Pos.ToString());
				listViewPlacedEnemies.Items.Add(item);
			}
        }

        public void SetFormLocation(System.Drawing.Point location)
        {
            System.Drawing.Point pos = location;
            formpos = location;
            pos.X += Location.X;
            pos.Y += Location.Y;
            Locationchanged();
            levelControl1.SetFormLocation(pos);
        }

        public void SetMouseTile(string tile)
        {
            levelControl1.SetMouseTileType(tile);
        }

        /// <summary>
        /// Used for the SelectedIndexChanged Event in the Toolform
        /// </summary>
        /// <param name="sender">The TileBox in the Toolform</param>
        /// <param name="e"></param>
        public void SetMouseTile(object sender, EventArgs e)
        {
            ListBox box = (ListBox)sender;
            levelControl1.SetMouseTileType(box.SelectedItem.ToString());
        }

        public void NewLevel(int x, int y)
        {
            levelControl1.NewLevel(x, y);
        }

        private void LevelMDIChild_LocationChanged(object sender, EventArgs e)
        {
            Locationchanged();
        }

        private void Locationchanged()
        {
            System.Drawing.Point pos = Location;
            pos.X += formpos.X;
            pos.Y += formpos.Y + 75;
            levelControl1.SetFormLocation(pos);
        }

		private void ParseLevelVariables(Dictionary<LV, string> levelVariables)
		{
			foreach (LayerFX layer in Enum.GetValues(typeof(LayerFX)))
			{
				layerColorData[layer] = LayerFXData.FromString(
					levelVariables[(LV)Enum.Parse(typeof(LV), 
					layer.ToString())]);
				if (layer == LayerFX.BackgroundFX)
					LoadFXData(layer);
				LayerFXDataChanged(layer, layerColorData[layer]);
			}
		}

        public void SaveLevel(string filename)
        {
            levelControl1.SaveFile(filename);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveMapDialog.ShowDialog();
        }

		private void SaveMapDialog_FileOk(object sender, CancelEventArgs e)
		{
			try
			{
				levelControl1.SaveFile(SaveMapDialog.FileName);
				MessageBox.Show(this, "Level saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, "Saving level failed:\n" + ex.Message, "Failes", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

        /// <summary>
        /// Used for the ViewButtonClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void FocusClick(object sender, EventArgs e)
        {
            this.Focus();
        }

        private void LevelMDIChild_FormClosed(object sender, FormClosedEventArgs e)
        {
            menuItemRemoveMethod(menuItem);
        }

        private void styleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stylechangeform = new StyleChangeForm(levelControl1.Level.LevelVariables);
            stylechangeform.SetLVOnClose = SetLVStyleChange;
            stylechangeform.Show();
        }

        private void SetLVStyleChange(LevelVariables pLV)
        {
            levelControl1.Level.LevelVariables = pLV;
            levelControl1.ReloadTextures();
        }

        private void levelControl1_Enter(object sender, EventArgs e)
        {

        }

        private void mapBoundsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mapBoundsForm = new MapBoundsForm(SetLevel, levelControl1.Level.Tiles);
            mapBoundsForm.Show();
        }

        public void SetTool(TileToolType type)
        {
            levelControl1.SetToolType(type);
        }

        public void SelectedEnemyChanged(string name)
        {
            levelControl1.SelectedEnemyChanged(name);
        }

		public void ToolChanged(Tool tool)
		{
			levelControl1.ToolChanged(tool);
		}

		private void trackBarColorizing_ValueChanged(object sender, EventArgs e)
		{
			if (!changinvalues)
			{
				changinvalues = true;
				TrackBar bar = (TrackBar)sender;
				int value = (int)bar.Value;
				if (bar.Equals(trackBarBrightness))
					numericBrightness.Value = value;
				else if (bar.Equals(trackBarContrast))
					numericContrast.Value = value;
				else if (bar.Equals(trackBarHue))
					numericHue.Value = value;
				else if (bar.Equals(trackBarSaturation))
					numericSaturation.Value = value;
				LayerFXDataChanged((LayerFX)comboBoxLayer.SelectedItem, GetFXData());
				changinvalues = false;
			}
		}

		private LayerFXData GetFXData()
		{
			LayerFXData data;
			data = LayerFXData.Default;

			data.ContrastEnabled = checkBoxContrast.Checked;
			data.Contrast = ((float)numericContrast.Value / 100f);
			data.Brightness = (float)numericBrightness.Value / 100f;
			data.HueEnabled = checkBoxColoration.Checked;
			data.Hue = (float)numericHue.Value / 360f;
			data.Saturation = (float)numericSaturation.Value / 100f;
			data.SaturationEnabled = checkBoxSaturation.Checked;

			return data;
		}

		private void LoadFXData(LayerFX layer)
		{
			LayerFXData data = layerColorData[layer];
			numericBrightness.Value = (decimal)(data.Brightness * 100f);
			numericContrast.Value = (decimal)(data.Contrast * 100f);
			numericHue.Value = (decimal)(data.Hue * 360f);
			numericSaturation.Value = (decimal)(data.Saturation * 100f);
			checkBoxColoration.Checked = data.HueEnabled;
			checkBoxContrast.Checked = data.ContrastEnabled;
			checkBoxSaturation.Checked = data.SaturationEnabled;
		}

		private void numericColorizing_ValueChanged(object sender, EventArgs e)
		{
			if (!changinvalues)
			{
				changinvalues = true;
				NumericUpDown numeric = (NumericUpDown)sender;
				int value = (int)numeric.Value;
				if (numeric.Equals(numericBrightness))
					trackBarBrightness.Value = value;
				else if (numeric.Equals(numericContrast))
					trackBarContrast.Value = value;
				else if (numeric.Equals(numericHue))
					trackBarHue.Value = value;
				else if (numeric.Equals(numericSaturation))
					trackBarSaturation.Value = value;
				LayerFXDataChanged((LayerFX)comboBoxLayer.SelectedItem, GetFXData());
				changinvalues = false;
			}
		}

		private void checkBoxColoration_CheckedChanged(object sender, EventArgs e)
		{
			CheckBox box = (CheckBox)sender;
			if (box.Equals(checkBoxColoration))
			{
				numericHue.Enabled = box.Checked;
				trackBarHue.Enabled = box.Checked;
			}
			else if (box.Equals(checkBoxContrast))
			{
				numericContrast.Enabled = box.Checked;
				trackBarContrast.Enabled = box.Checked;
				numericBrightness.Enabled = box.Checked;
				trackBarBrightness.Enabled = box.Checked;
			}
			else if (box.Equals(checkBoxSaturation))
			{
				numericSaturation.Enabled = box.Checked;
				trackBarSaturation.Enabled = box.Checked;
			}
			LayerFXDataChanged((LayerFX)comboBoxLayer.SelectedItem, GetFXData());
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			drawAndUpdating = false;
			levelControl1.Activated = drawAndUpdating;
			base.OnClosing(e);
		}

		private void comboBoxLayer_SelectedIndexChanged(object sender, EventArgs e)
		{
			LoadFXData((LayerFX)comboBoxLayer.SelectedItem);
		}

        private void LevelMDIChild_Load(object sender, EventArgs e)
        {
            //comboBoxDecoLayer.SelectedIndex = 0;
			LoadExistingEnemies();
			LoadExistingDecos();
			listBoxIObjects.Items.Clear();
			foreach (InteractiveObjectsIDs iD in Enum.GetValues(typeof(InteractiveObjectsIDs)))
			{
				listBoxIObjects.Items.Add(iD);
			}
			listBoxIObjects.SelectedIndex = 0;
        }

		private void tabControlTools_SelectedIndexChanged(object sender, EventArgs e)
		{
			toolManager.tabControlTools_TabIndexChanged(sender, e);
			numericUpDownJumpPadValues_ValueChanged(this, e);
		}

		private void enemyListBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			toolManager.enemyListBox_SelectedIndexChanged(sender, e);
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
			enemyListBox.Items.AddRange(existingEnemies);
			enemyListBox.SelectedIndex = 0;

		}

		private void LoadExistingDecos()
		{
			string[] existingDecos= new string[1];
			string path = "Data/" +
				GameConstants.GraphXPacksPath +
				levelControl1.Level.LevelVariables.Dictionary[LV.GraphXPack] +
				GameConstants.DECORATIONS_PATH;
			try
			{
				existingDecos = Directory.GetFiles(
				path,
				"*",
				SearchOption.TopDirectoryOnly);
			}
			catch (Exception e)
			{
				string message = e.Message;
				
			}
			
			for (int i = 0; i < existingDecos.Length; i++)
			{
				string[] data = (existingDecos[i].Split('/'));
				existingDecos[i] = data[data.Length - 1];
				existingDecos[i] = existingDecos[i].Replace(".xnb", "");
				ToolStripMenuItem item = new ToolStripMenuItem(existingDecos[i]);
			}
			listBoxAvailableDecos.Items.AddRange(existingDecos);
			listBoxAvailableDecos.SelectedIndex = 0;

		}

		private void listBoxTiles_SelectedIndexChanged(object sender, EventArgs e)
		{
			toolManager.listBoxTiles_SelectedIndexChanged(sender, e);
		}

		private void comboBoxDecoLayer_SelectedIndexChanged(object sender, EventArgs e)
		{
			toolManager.comboBoxDecoLayer_SelectedIndexChanged(sender, e);
			ListViewItem item;
			switch (toolManager.CurrentLayer)
			{ 
				case DecorationsLayer.Front:
					listViewPlacedDecos.Items.Clear();

					for (int i = 0; i < frontCollection.Count; i++)
					{
						item = new ListViewItem(frontCollection[i].Texture.Name);
						item.SubItems.Add(frontCollection[i].ExtendedRectangle.Translation.ToString());
						listViewPlacedDecos.Items.Add(item);

					}
					break;
				case DecorationsLayer.Rear:
					listViewPlacedDecos.Items.Clear();
					for (int i = 0; i < rearCollection.Count; i++)
					{
						item = new ListViewItem(rearCollection[i].Texture.Name);
						item.SubItems.Add(rearCollection[i].ExtendedRectangle.Translation.ToString());
						listViewPlacedDecos.Items.Add(item);

					}
					break;
			}
		}

		private void listBoxAvailableDecos_SelectedIndexChanged(object sender, EventArgs e)
		{
			toolManager.listBoxAvailableDecos_SelectedIndexChanged(sender, e);
		}

		private void buttonRemoveSelectedDeco_Click(object sender, EventArgs e)
		{
			int[] indices = new int[listViewPlacedDecos.SelectedIndices.Count];
			LayerObject[] items = new LayerObject[listViewPlacedDecos.SelectedIndices.Count];
			for (int i = 0; i < items.Length; i++)
			{
				indices[i] = listViewPlacedDecos.SelectedIndices[i];
				if (toolManager.CurrentLayer == DecorationsLayer.Front)
				{
					items[i] = frontCollection[listViewPlacedDecos.SelectedIndices[i]];
				}
				else
				{
					items[i] = rearCollection[listViewPlacedDecos.SelectedIndices[i]];
				}
			}
			for (int i = items.Length-1; i >= 0; i--)
			{
				if(toolManager.CurrentLayer == DecorationsLayer.Front)
				{
						frontCollection.Remove(items[i]);
				}
				else
				{
						rearCollection.Remove(items[i]);
				}
				levelControl1.RemoveItem(items[i], toolManager.CurrentLayer);
				listViewPlacedDecos.Items.RemoveAt(indices[i]);

			}
			
		}

        private void LayerNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            UpdateExtendedRectangleData();
            DecoObjectDataChanged(rect, origin, rotation, layerDepth);
        }

        private void UpdateExtendedRectangleData()
        {
            origin = new Vector2(
                (float)xOriginNumericUpDown.Value, 
                (float)yOriginNumericUpDown.Value);
            rotation = (float)rotationNumericUpDown.Value;
            //layerDepth = (float)layerNumericUpDown.Value;
            rect = new Microsoft.Xna.Framework.Rectangle(
                (int)xPositionNumericUpDown.Value, 
                (int)yPositionNumericUpDown.Value, 
                (int)widthNumericUpDown.Value, 
                (int)heightNumericUpDown.Value);
            
        }

		private void listBoxIObjects_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (levelControl1.Initialized)
			{
				if (listBoxIObjects.SelectedItem.ToString() == InteractiveObjectsIDs.JumpPad.ToString())
					numericUpDownJumpPadValues_ValueChanged(numericUpDownIObjectRotation, new EventArgs());
				toolManager.listBoxIObjects_SelectedIndexChanged(sender, e);
			}
		}

		private void numericUpDownJumpPadValues_ValueChanged(object sender, EventArgs e)
		{
			if (levelControl1.Initialized)
			{
				toolManager.numericUpDownJumpPadValues_ValueChanged(
					(float)numericUpDownIObjectRotation.Value,
					(float)numericUpDownIObjectStrength.Value,
					(float)numericUpDownIObjectTime.Value);
				toolManager.listBoxIObjects_SelectedIndexChanged(listBoxIObjects, e);
			}
		}

		private void buttonRemoveSelectedEnemies_Click(object sender, EventArgs e)
		{
			ListView.SelectedIndexCollection selectedItems = listViewPlacedEnemies.SelectedIndices;
			List<Enemy> oldEnemyList = placedEnemies;
			foreach (int index in selectedItems)
			{
				levelControl1.Enemymanager.RemoveEnemy(placedEnemies[index]);
			}
			listViewPlacedEnemies.Items.Clear();
			placedEnemies.Clear();
			placedEnemies.AddRange(levelControl1.Enemymanager.Enemies);
			foreach (Enemy enemy in placedEnemies)
			{
				ListViewItem item = new ListViewItem(enemy.Type);
				item.SubItems.Add(enemy.Pos.ToString());
				listViewPlacedEnemies.Items.Add(item);
			}
			
		}
    }
}
