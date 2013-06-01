using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Star.Game.Enemy;

namespace StarEdit.EnemyEditor
{
    public partial class OffsetForm : Form
    {
        public event RectangleTransformedEventHandler RectangleOffset;

        private Animation currentAnimation;

        public OffsetForm(Animation animation)
        {
            InitializeComponent();
            RectangleOffset += new RectangleTransformedEventHandler(OffsetForm_RectangleOffset);
            currentAnimation = animation;
            InitalizeComboBox();
            numericUpDownOffset.Maximum = currentAnimation.Frames.Length;
        }

        private void InitalizeComboBox()
        {
            foreach (string rectangle in currentAnimation.CurrentFrame.GetRectangles.Keys)
            {
                comboBoxRectangle.Items.Add(rectangle);
            }
        }

        void OffsetForm_RectangleOffset(string rectangle, Star.Game.Enemy.FrameRectangle[] tranformedRectangles)
        {
            //throw new NotImplementedException();
        }

		private void buttonOK_Click(object sender, EventArgs e)
		{
			if (comboBoxRectangle.SelectedItem != null && checkBoxFrameOffset.Checked)
			{
				int offset = (int)numericUpDownOffset.Value;
				FrameRectangle[] oldRectangles = new FrameRectangle[currentAnimation.Frames.Length];
				FrameRectangle[] newRectangles = new FrameRectangle[currentAnimation.Frames.Length];

				for (int i = 0; i < oldRectangles.Length; i++)
				{
					oldRectangles[i] = currentAnimation.Frames[i].GetRectangles[comboBoxRectangle.SelectedItem.ToString()];
				}

				for (int i = 0; i < oldRectangles.Length; i++)
				{
					newRectangles[i] = oldRectangles[i + offset < oldRectangles.Length ? i + offset : i + offset - oldRectangles.Length];
				}

				RectangleOffset(comboBoxRectangle.SelectedItem.ToString(), newRectangles);
			}
			else if (checkBox1.Checked)
			{
				List<string> rects = new List<string>();
				foreach (string key in currentAnimation.Frames[0].GetRectangles.Keys)
					rects.Add(key);
				foreach (string rect in rects)
				{
					FrameRectangle[] oldRectangles = new FrameRectangle[currentAnimation.Frames.Length];
					FrameRectangle[] newRectangles = new FrameRectangle[currentAnimation.Frames.Length];

					for (int i = 0; i < oldRectangles.Length; i++)
					{
						oldRectangles[i] = currentAnimation.Frames[i].GetRectangles[rect];
					}

					for (int i = 0; i < oldRectangles.Length; i++)
					{
						newRectangles[i] = oldRectangles[i];
						newRectangles[i].Rect.X += (int)numericUpDown1.Value;
						newRectangles[i].Rect.Y += (int)numericUpDown2.Value;

					}

					RectangleOffset(rect, newRectangles);
				}
				Close();
			}
			else
				MessageBox.Show(this, "A Rectangle must be Selected", "Fail", MessageBoxButtons.OK, MessageBoxIcon.Error);

		}

        private void buttonAbort_Click(object sender, EventArgs e)
        {
            Close();
        }

		private void comboBoxRectangle_SelectedIndexChanged(object sender, EventArgs e)
		{

		}

		private void checkBoxFrameOffset_CheckedChanged(object sender, EventArgs e)
		{
			groupBox1.Enabled = checkBoxFrameOffset.Checked;
		}

		private void checkBox1_CheckedChanged(object sender, EventArgs e)
		{
			groupBox2.Enabled = checkBox1.Checked;
		}
    }
}
