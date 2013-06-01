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

namespace StarEdit.EnemyEditor
{
   

    public partial class ParentForm : Form
    {
        Animation currentAnimation;

        public ParentForm(Animation animation)
        {
            InitializeComponent();
            foreach (string value in animation.CurrentFrame.GetRectangles.Keys)
            {
                comboBox1.Items.Add(value);
                comboBox2.Items.Add(value);
            }
            currentAnimation = animation;
            
            ChildTransformed += new RectangleTransformedEventHandler(ParentForm_ChildTransformed);
        }

        void ParentForm_ChildTransformed(string child, FrameRectangle[] tranformedRectangles)
        {
            //throw new NotImplementedException();
        }

        public event RectangleTransformedEventHandler ChildTransformed;

        private void buttonAbort_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Abort;
            Close();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            int startX, startY;

            if (comboBox1.SelectedItem != null && comboBox2.SelectedItem != null)
                if (comboBox1.SelectedItem != comboBox2.SelectedItem)
                {
                    FrameRectangle[] parentRectangles = new FrameRectangle[currentAnimation.Frames.Length];
                    FrameRectangle[] childRectangles = new FrameRectangle[currentAnimation.Frames.Length];
                    Vector2 parentRotationVector;
					float startRotationOffset;
					Vector2 transformedVector;


                    for (int i = 0; i < currentAnimation.Frames.Length; i++)
                    {
                        parentRectangles[i] = currentAnimation.Frames[i].GetRectangles[comboBox1.SelectedItem.ToString()];
                        childRectangles[i] = currentAnimation.Frames[i].GetRectangles[comboBox2.SelectedItem.ToString()];
                    }

                    parentRotationVector = new Vector2(childRectangles[0].Rect.X, childRectangles[0].Rect.Y) - new Vector2(parentRectangles[0].Rect.X, parentRectangles[0].Rect.Y);
                   
                    startRotationOffset = parentRectangles[0].Rotation;
					
                    startX = childRectangles[0].Rect.X;
                    startY = childRectangles[0].Rect.Y;

                    for (int i = 1; i < currentAnimation.Frames.Length; i++)
                    {
                        
						if (checkBoxRotation.Checked)
						{
							transformedVector = Vector2.Transform(parentRotationVector, Matrix.CreateRotationZ(parentRectangles[i].Rotation - startRotationOffset));
							childRectangles[i].Rect.X = parentRectangles[i].Rect.X + (int)transformedVector.X;
							childRectangles[i].Rect.Y = parentRectangles[i].Rect.Y + (int)transformedVector.Y;
						}
						else
						{
							int offsetX, offsetY;
							offsetX = parentRectangles[i].Rect.X - parentRectangles[i - 1].Rect.X;
							offsetY = parentRectangles[i].Rect.Y - parentRectangles[i - 1].Rect.Y;
							childRectangles[i].Rect.X = startX + offsetX;
							childRectangles[i].Rect.Y = startY + offsetY;
							startX += offsetX;
							startY += offsetY;
						}
                    }

                    ChildTransformed(comboBox2.SelectedItem.ToString(), childRectangles);
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                    MessageBox.Show(this, "Parent und Child dürfen nicht gleich sein!", "Fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
                MessageBox.Show(this, "Du musst Parent und Child angeben", "Fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
