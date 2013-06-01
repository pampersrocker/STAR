using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Star.Game.Level;

namespace StarEdit.LevelEditor
{
    public partial class MapBoundsForm : Form
    {
        StarEdit.LevelEditor.LevelWorker.TileOverLoad overloader;
        LevelWorker worker;
        Tile[,] oldtiles;
        public MapBoundsForm(StarEdit.LevelEditor.LevelWorker.TileOverLoad poverloader,Tile[,] oldTiles)
        {
            InitializeComponent();
            overloader = poverloader;
            oldtiles = oldTiles;
            SizeXBox.Value = oldtiles.GetLength(1);
            SizeYBox.Value = oldtiles.GetLength(0);
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            Point zeroPointModification = new Point(
                (int)PositionXBox.Value,
                (int)PositionYBox.Value);

            Point newSize = new Point(
                (int)SizeXBox.Value,
                (int)SizeYBox.Value);
            worker = new LevelWorker();
            worker.Show();
            worker.ModifyTiles(oldtiles, zeroPointModification, newSize, overloader);
            Close();

        }

        private void PositionXBox_ValueChanged(object sender, EventArgs e)
        {
            SizeXBox.Minimum = PositionXBox.Value + 1;
        }

        private void PositionYBox_ValueChanged(object sender, EventArgs e)
        {
            SizeYBox.Minimum = PositionYBox.Value + 1;
        }
    }
}
