using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using WinFormsContentLoading;
using Star.GameManagement;
using Star.Game.Enemy;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace StarEdit.EnemyEditor
{
    public partial class AddRectangleForm : Form
    {
        public delegate void RectLoadEventhandler(string enemyName,string rectangleName,FrameRectangle rect);

        ContentBuilder builder;
        string enemyname;
        string[] alreadyExsistingRectangles;
        public AddRectangleForm(string enemyname,string[] rectanglenames)
        {
            InitializeComponent();
            builder = new ContentBuilder();
            this.enemyname = enemyname;
            alreadyExsistingRectangles = rectanglenames;
            OpenImg.Filter = "Image Files(*.BMP;*.JPG;*.PNG)|*.BMP;*.JPG;*.PNG|All files (*.*)|*.*";
        }

        private void selectButton_Click(object sender, EventArgs e)
        {
            if (OpenImg.ShowDialog() == DialogResult.OK)
            {
                imagePathTextBox.Text = OpenImg.FileName;
                
            }
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(imagePathTextBox.Text))
            {
                if (!string.IsNullOrEmpty(nameTextBox.Text))
                {
                    if (!alreadyExsistingRectangles.Contains(nameTextBox.Text))
                    {
                        builder.Add(OpenImg.FileName, nameTextBox.Text, null, "TextureProcessor");
                        string error = builder.Build();
                        if (!string.IsNullOrEmpty(error))
                        {
                            MessageBox.Show(this, "Failed to Load the Rectangle:\n" + error, "An Error Occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            if (!File.Exists("Data/" + GameConstants.EnemiesPath + enemyname + "/" + nameTextBox.Text + ".xnb"))
                            {
                                File.Move(builder.OutputDirectory + "/" + nameTextBox.Text + ".xnb", "Data/" + GameConstants.EnemiesPath + enemyname + "/" + nameTextBox.Text + ".xnb");
                            }
                            else
                            {
                                if (MessageBox.Show(this, "File already exits. Override?", "File exists", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                                {
                                    File.Delete("Data/" + GameConstants.EnemiesPath + enemyname + "/" + nameTextBox.Text + ".xnb");
                                    File.Move(builder.OutputDirectory + "/" + nameTextBox.Text + ".xnb", "Data/" + GameConstants.EnemiesPath + enemyname + "/" + nameTextBox.Text + ".xnb");
                                }
                            }

                            //MessageBox.Show("Loaded");

                            //#Fire the RectangleLoaded Event
                            RectangleLoaded(enemyname, nameTextBox.Text, GenerateFrameRectangle());
                            this.Close();
                        }
                    }
                    else
                        MessageBox.Show(this, "Rectangle name already used, choose another one.", "Name already Used", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                    MessageBox.Show(this, "Rectangle Name is not Valid!", "Fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
                MessageBox.Show(this, "A picture must be selected", "Failed to load the Picture", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private FrameRectangle GenerateFrameRectangle()
        {
            FrameRectangle rect = new FrameRectangle();

            Microsoft.Xna.Framework.Rectangle pos = new Microsoft.Xna.Framework.Rectangle((int)xPositionNumericUpDown.Value, (int)yPositionNumericUpDown.Value, (int)widthNumericUpDown.Value, (int)heightNumericUpDown.Value);
            float rotation = (float)rotationNumericUpDown.Value;
            float layer = (float)layerNumericUpDown.Value;
            Vector2 origin = new Vector2((float)xOriginNumericUpDown.Value,(float)yOriginNumericUpDown.Value);
            Color color = new Color((byte)rColorNumericUpDown.Value, (byte)gColorNumericUpDown.Value, (byte)bColorNumericUpDown.Value, (byte)aColorNumericUpDown.Value);
            rect.Load(pos, rotation, layer, color, origin);
            return rect;
        }

        public event RectLoadEventhandler RectangleLoaded;
    }
}
