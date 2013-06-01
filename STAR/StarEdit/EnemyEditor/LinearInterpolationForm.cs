using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Star.Game.Enemy;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StarEdit.EnemyEditor
{
    public partial class LinearInterpolationForm : Form
    {
        FrameRectangle[] orgFrames;

        public delegate void LinearRectangleEventHandler(FrameRectangle[] frameRects, int startPos, int endPos);

        public event LinearRectangleEventHandler LinearInterpolated;

        public LinearInterpolationForm(int frames,FrameRectangle standardRect,FrameRectangle[] origFrames)
        {
            InitializeComponent();
            endFrameUpDown.Maximum = frames;
            endFrameUpDown.Value = frames;
            startFrameUpDown.Maximum = endFrameUpDown.Value - 1M;
            LoadRectangleData(standardRect);
            orgFrames = (FrameRectangle[]) origFrames.Clone();
            InitializeCheckBox();
            
        }

        private void LoadRectangleData(FrameRectangle rect)
        {
            xPositionNumericUpDown.Value = rect.Rect.X;
			xPositionNumericUpDown.Value = rect.Rect.X;
            yPositionNumericUpDown.Value = rect.Rect.Y;
            widthNumericUpDown.Value = rect.Rect.Width;
            heightNumericUpDown.Value = rect.Rect.Height;
            rotationNumericUpDown.Value = (decimal)rect.Rotation;
            layerNumericUpDown.Value = (decimal)rect.DrawPosition;
            rColorNumericUpDown.Value = rect.Color.R;
            gColorNumericUpDown.Value = rect.Color.G;
            bColorNumericUpDown.Value = rect.Color.B;
            aColorNumericUpDown.Value = rect.Color.A;

            xStartPositionUpDown.Value = rect.Rect.X;
            yStartPositionUpDown.Value = rect.Rect.Y;
            widthStartPositionUpDown.Value = rect.Rect.Width;
            heightStartPositionUpDown.Value = rect.Rect.Height;
            rotationStartUpDown.Value = (decimal)rect.Rotation;
            layerStartUpDown.Value = (decimal)rect.DrawPosition;
            rColorStartUpDown.Value = rect.Color.R;
            gColorStartUpDown.Value = rect.Color.G;
            bColorStartUpDown.Value = rect.Color.B;
            aColorStartUpDown.Value = rect.Color.A;
        }

        private void InitializeCheckBox()
        {
            changeValuesCheckBox.Items.Add("Rectangle");
            changeValuesCheckBox.Items.Add("Color");
            changeValuesCheckBox.Items.Add("Origin");
            changeValuesCheckBox.Items.Add("Rotation");
            changeValuesCheckBox.Items.Add("Layer");
            changeValuesCheckBox.CheckOnClick = true;

            ResetEnabler();
        }

        private void ResetEnabler()
        {
            rectGB1.Enabled = false;
            colorGroupBox1.Enabled = false;
            origGB2.Enabled = false;
            rectGB2.Enabled = false;
            colorGB2.Enabled = false;
            originGB1.Enabled = false;
            rotationNumericUpDown.Enabled = false;
            rotationStartUpDown.Enabled = false;
            layerNumericUpDown.Enabled = false;
            layerStartUpDown.Enabled = false;
        }
        

        private void UpdateChangeEnable(ItemCheckEventArgs e)
        {
            //ResetEnabler();
            bool enabler = e.NewValue == CheckState.Checked;
            switch(e.Index)
            {
                case 0:
                    rectGB1.Enabled = enabler;
                    rectGB2.Enabled = enabler;
                    break;
                case 1:
                    colorGroupBox1.Enabled = enabler;
                    colorGB2.Enabled = enabler;
                    break;
                case 2:
                    origGB2.Enabled = enabler;
                    originGB1.Enabled = enabler;
                    break;
                case 3:
                    rotationNumericUpDown.Enabled = enabler;
                    rotationStartUpDown.Enabled = enabler;
                    break;
                case 4:
                    layerNumericUpDown.Enabled = enabler;
                    layerStartUpDown.Enabled = enabler;
                    break;
            }
            
            
        }

        private void abortButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void endFrameUpDown_ValueChanged(object sender, EventArgs e)
        {
            startFrameUpDown.Maximum = endFrameUpDown.Value - 1M;
        }

        private void startFrameUpDown_ValueChanged(object sender, EventArgs e)
        {
            endFrameUpDown.Minimum = startFrameUpDown.Value + 1M;
        }

        private FrameRectangle[] Interpolate()
        {
            int length = (int)(endFrameUpDown.Value - startFrameUpDown.Value);
            int start = (int)startFrameUpDown.Value;
            Microsoft.Xna.Framework.Vector2 origin;
            Microsoft.Xna.Framework.Color color;
            FrameRectangle[] interpolatedRects= new FrameRectangle[length];
            RectangleDifference differences = GetDifference(length);

            for (int i = 0; i < length; i++)
            {
                Microsoft.Xna.Framework.Rectangle rect;
                float rotation;
                float layer;
                if (rectGB1.Enabled)
                     rect = new Microsoft.Xna.Framework.Rectangle(
                        (int)xStartPositionUpDown.Value + (int)(i * differences.xPositionDifference),
                        (int)yStartPositionUpDown.Value + (int)(i * differences.yPositionDifference),
                        (int)widthStartPositionUpDown.Value + (int)(i * differences.widthPositionDifference),
                        (int)heightStartPositionUpDown.Value + (int)(i * differences.heightPositionDifference));
                else
                    rect = orgFrames[start + i].Rect;

                if (rotationNumericUpDown.Enabled)
                    rotation = (float)rotationStartUpDown.Value + (differences.rotationDifference * i);
                else
                    rotation = orgFrames[start + i].Rotation;

                if (layerNumericUpDown.Enabled)
                    layer = (float)layerStartUpDown.Value + (differences.layerDifference * i);
                else
                    layer = orgFrames[start + i].DrawPosition;

                if (origGB2.Enabled)
                    origin = new Microsoft.Xna.Framework.Vector2(
                        (float)xOriginStartUpDown.Value + differences.xOriginDifference * i,
                        (float)yOriginStartUpDown.Value + differences.yOriginDifference * i);
                else
                    origin = orgFrames[start + i].Origin;

                if (colorGB2.Enabled)
                    color = new Microsoft.Xna.Framework.Color(
                        (((float)rColorStartUpDown.Value) + differences.rColorDifference * i) / 255f,
                        (((float)gColorStartUpDown.Value) + differences.gColorDifference * i) / 255f,
                        (((float)bColorStartUpDown.Value) + differences.bColorDifference * i) / 255f,
                        (((float)(aColorStartUpDown.Value) + (differences.aColorDifference * i))) / 255f);
                else
                    color = orgFrames[start + i].Color;


                interpolatedRects[i].Load(rect, rotation, layer, color, origin);
            }


            return interpolatedRects;
        }

        private RectangleDifference GetDifference(int length)
        {
            RectangleDifference rect = new RectangleDifference();

            rect.xPositionDifference = (float)(xPositionNumericUpDown.Value - xStartPositionUpDown.Value) / length;
            rect.yPositionDifference = (float)(yPositionNumericUpDown.Value - yStartPositionUpDown.Value) / length;
            rect.widthPositionDifference = (float)(widthNumericUpDown.Value - widthStartPositionUpDown.Value) / length;
            rect.heightPositionDifference = (float)(heightNumericUpDown.Value - heightStartPositionUpDown.Value) / length;
            rect.rColorDifference = (float)(rColorNumericUpDown.Value - rColorStartUpDown.Value) / length;
            rect.bColorDifference = (float)(bColorNumericUpDown.Value - bColorStartUpDown.Value) / length;
            rect.gColorDifference = (float)(gColorNumericUpDown.Value - gColorStartUpDown.Value) / length;
            rect.aColorDifference = (float)(aColorNumericUpDown.Value - aColorStartUpDown.Value) / length;
            rect.rotationDifference = (float)(rotationNumericUpDown.Value - rotationStartUpDown.Value) / length;
            rect.layerDifference = (float)(layerNumericUpDown.Value - layerStartUpDown.Value) / length;
            rect.xOriginDifference = (float)(xOriginNumericUpDown.Value - xOriginStartUpDown.Value) / length;
            rect.yOriginDifference = (float)(yOriginNumericUpDown.Value - yOriginStartUpDown.Value) / length;

            return rect;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            FrameRectangle[] interpolatedRects = Interpolate();
            int start = (int)startFrameUpDown.Value;
            int end = (int)endFrameUpDown.Value;

            //Fire Interpolated Event
            LinearInterpolated(interpolatedRects, start, end);

            Close();
        }

        private void changeValuesCheckBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            UpdateChangeEnable(e);
            
        }
    }

    public struct RectangleDifference
    {
        public float xPositionDifference;
        public float yPositionDifference;
        public float widthPositionDifference;
        public float heightPositionDifference;
        public float rColorDifference;
        public float gColorDifference;
        public float bColorDifference;
        public float aColorDifference;
        public float xOriginDifference;
        public float yOriginDifference;
        public float rotationDifference;
        public float layerDifference;
    }
}
