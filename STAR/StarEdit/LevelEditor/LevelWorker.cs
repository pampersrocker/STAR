using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Star.Game.Level;

namespace StarEdit.LevelEditor
{
    public partial class LevelWorker : Form
    {
        Thread worker;
        Tile[,] oldTiles;
        Point zeroPointModification;
        Point newSize;
        float percentage=0;
        public delegate void TileOverLoad(Tile[,] newtiles);
        TileOverLoad overloader;
        
        public LevelWorker()
        {
            InitializeComponent();
        }

        public void ModifyTiles(Tile[,] poldTiles,Point pzeroPointModification,Point pnewSize,TileOverLoad poverloader)
        {
            percentage = 0;
            overloader = poverloader;
            oldTiles = poldTiles;
            zeroPointModification = pzeroPointModification;
            newSize = pnewSize;
            progessTimer.Enabled = true;
            worker = new Thread(new ThreadStart(ThreadModifiyTiles));
            worker.Start();
        }

        private void ThreadModifiyTiles()
        {
            Tile[,] newtiles;

            Point oldSize = new Point(
                oldTiles.GetLength(1),
                oldTiles.GetLength(0));

            Point totalSize = new Point(
                newSize.X - zeroPointModification.X,
                newSize.Y - zeroPointModification.Y);

            newtiles = new Tile[totalSize.Y, totalSize.X];
            int y;
            int x;
            int newx;
            int newy;
            for (y = zeroPointModification.Y,newy=0; y < newSize.Y && newy<totalSize.Y; y++,newy++)
            { 
                for(x=zeroPointModification.X,newx = 0;x< newSize.X && newx<totalSize.X;x++,newx++)
                {
                    newtiles[newy, newx] = new Tile(newx, newy);
                    if (x < 0 || y < 0 || x >= oldSize.X || y >= oldSize.Y)
                    {
                        newtiles[newy, newx].load_tile((int)TileType.Empty,null);
                    }
                    else 
                    {
                        newtiles[newy, newx].load_tile((int)oldTiles[y, x].TileType,null);
                    }
                }
                percentage = ((float)newy / (float)totalSize.Y) *100;
            }
            foreach (Tile tile in newtiles)
            {
                tile.loadGrass(newtiles);
            }
            percentage = 100;
            overloader(newtiles);
        }

        private void progessTimer_Tick(object sender, EventArgs e)
        {
            progressBar.Value = (int)percentage;
            progressBar.Refresh();
            progressLabel.Text = (int)percentage + "%";
            if (percentage == 100)
            {
                progessTimer.Enabled = false;
                CloseTimer.Enabled = true;
            }

        }

        private void CloseTimer_Tick(object sender, EventArgs e)
        {
            CloseTimer.Enabled = false;
            Close();
        }

        private void LevelWorker_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (worker.ThreadState == ThreadState.Running)
            {
                worker.Abort();
            }
        }
    }
}
