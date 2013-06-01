using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Star.Game.Level;
using Star.Game;

namespace StarEdit.Editor
{
    public partial class StyleChangeForm : Form
    {

        LevelVariables variables = new LevelVariables();
        public delegate void LVSetDelegate(LevelVariables pLV);
        private LVSetDelegate closing;

        public LVSetDelegate SetLVOnClose
        {
            set { closing = value; }
        }

        public StyleChangeForm(LevelVariables lv)
        {
            InitializeComponent();
            variables = lv;
            LoadBackgroundLists();
            SelectCurrentSettings();
            //GraphXPackBox.SelectedItem = variables.Dictionary[LV.GraphXPack];
            RefreshGraphXPackBox();
        }

        private void LoadBackgroundLists()
        {
            string graphXPackpath = "Data/GraphXPacks/" + variables.Dictionary[LV.GraphXPack] + "/";
            bool backgroundexists = Directory.Exists(graphXPackpath + "Backgrounds/");
            bool parallaxexists = Directory.Exists(graphXPackpath + "Backgrounds/Layers/ParallaxLayer/");
            if (backgroundexists && parallaxexists)
            {
                string[] backgroundimgs = Directory.GetFiles(graphXPackpath + "Backgrounds/", "*.xnb", SearchOption.TopDirectoryOnly);
                string[] parallaximgs = Directory.GetFiles(graphXPackpath + "Backgrounds/Layers/ParallaxLayer/", "*.xnb", SearchOption.TopDirectoryOnly);

                foreach (string img in backgroundimgs)
                {
                    BackgroundImgBox.Items.Add(CutNameOut(img));
                    //BackgroundImgBox.SelectedItem = img;
                }

                foreach (string img in parallaximgs)
                {
                    RearparallaxBox.Items.Add(CutNameOut(img));
                    FrontParallaxBox.Items.Add(CutNameOut(img));
                }
            }

        }

        private string CutNameOut(string path)
        {
            string[] data = path.Split('/');
            data = data[data.Length - 1].Split('.');
            string name = data[data.Length - 2].Trim();
            return name;
        }

        private void SelectCurrentSettings()
        {
            BackgroundImgBox.SelectedItem = variables.Dictionary[LV.BackgroundImg];
            
            RearparallaxBox.SelectedItem = variables.Dictionary[LV.RearParallaxLayerImage];
            RearparallaxSpeedNumericUpDown.Value = decimal.Parse(variables.Dictionary[LV.RearParallaxLayerSpeedDivider]);
            FrontParallaxBox.SelectedItem = variables.Dictionary[LV.FrontParallaxLayerImage];
            FrontParallaxSpeedUpDown.Value = decimal.Parse(variables.Dictionary[LV.FrontParallaxLayerSpeedDivider]);
        }

        private void loadbutton_Click(object sender, EventArgs e)
        {
            try
            {
                ReadOutBoxes();
                if (closing != null)
                {
                    closing(variables);
                }
                Close();
            }
            catch (NullReferenceException)
            {
                MessageBox.Show(this,"All Parameters must be Set","Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ReadOutBoxes()
        {
            variables.Dictionary[LV.GraphXPack] = GraphXPackBox.SelectedItem.ToString();
            variables.Dictionary[LV.BackgroundImg] = BackgroundImgBox.SelectedItem.ToString();
            variables.Dictionary[LV.RearParallaxLayerImage] = RearparallaxBox.SelectedItem.ToString();
            variables.Dictionary[LV.FrontParallaxLayerImage] = FrontParallaxBox.SelectedItem.ToString();

            variables.Dictionary[LV.RearParallaxLayerSpeedDivider] = RearparallaxSpeedNumericUpDown.Value.ToString();
            variables.Dictionary[LV.FrontParallaxLayerSpeedDivider] = FrontParallaxSpeedUpDown.Value.ToString();
        }

        private void ClearLists()
        {
            //GraphXPackBox.Items.Clear();
            BackgroundImgBox.Items.Clear();
            RearparallaxBox.Items.Clear();
            FrontParallaxBox.Items.Clear();
        }

        private void RefreshGraphXPackBox()
        {
            GraphXPackBox.Items.Clear();
            string graphXPacks = FileManager.ReadFile("Data/GraphXPacks/installed.packs");
            string[] packs = graphXPacks.Split(',');
            foreach (string installedpacks in packs)
            {
                if (installedpacks != "" && installedpacks != null)
                {
                    GraphXPackBox.Items.Add(installedpacks);
                }
            }
            GraphXPackBox.SelectedItem = variables.Dictionary[LV.GraphXPack];
        }

        private void GraphXPackBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            variables.Dictionary[LV.GraphXPack] = GraphXPackBox.SelectedItem.ToString();
            //ReadOutBoxes();
            ClearLists();
            LoadBackgroundLists();
            SelectCurrentSettings();
        }

        private void cancelbutton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
