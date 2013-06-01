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
using System.IO;
using Star.GameManagement;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics;

namespace StarEdit.EnemyEditor
{
    public delegate void RectangleTransformedEventHandler(string rectangle, FrameRectangle[] tranformedRectangles);

    public partial class EnemyEditor : Form
    {
        AddRectangleForm rectForm;
        LinearInterpolationForm linearForm;
        ParentForm parentForm;
        OffsetForm offsetForm;
		List<string> deletableFilesonSave;

        public EnemyEditor()
        {
            InitializeComponent();
			enemyControl.PlayControl = new PlayControl();
            OpenImg.Filter = "Image Files(*.BMP;*.JPG;*.PNG)|*.BMP;*.JPG;*.PNG|All files (*.*)|*.*";
			deletableFilesonSave = new List<string>();
            enemyControl.IgnoreFocus = true;
        }

        private void openImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (OpenImg.ShowDialog() == DialogResult.OK)
            {
                //enemyControl1.LoadTexture(OpenImg.FileName);
            }
        }

        public void FormLocationChanged(System.Drawing.Point location)
        {
            enemyControl.SetEditorLocation(location);
        }

        private void EnemyEditor_LocationChanged(object sender, EventArgs e)
        {
            enemyControl.SetFormLocation(Location);
        }

        private void EnemyEditor_Load(object sender, EventArgs e)
        {
            enemyControl.PlayControl.PlayControlClicked += new PlayControlClickEvent.PlayControlEventHandler(PlayControl_PlayControlClicked);
            foreach (Anims animation in Enum.GetValues(typeof(Anims)))
            {
                AnimationComboBox.Items.Add(animation);
            }
            AnimationComboBox.SelectedItem = Anims.Walk;
			SetRectComboBox();
			rectangleComboBox.SelectedIndex = 0;
			enemyControl.CurrentRectangle = (string)rectangleComboBox.SelectedItem;
            LoadRectangleData();
            EnemyEditor_LocationChanged(this, new EventArgs());
			enemyControl.CurrentRectangleChanged += new EnemyControl.RectangleChangedEventHandler(enemyControl_CurrentRectangleChanged);
			//liveEditCheckBoxList.ItemCheck += new ItemCheckEventHandler(enemyControl.liveEditCheckBoxList_ItemCheck);
            foreach (Star.Game.Enemy.Enemy.CollisionRects rect in Enum.GetValues(typeof(Star.Game.Enemy.Enemy.CollisionRects)))
            {
                rectCollisionBox.Items.Add(rect);
            }
        }

		void enemyControl_CurrentRectangleChanged(FrameRectangle newrect)
		{
			LoadRectangleData();
		}

		private void SetRectComboBox()
		{
			rectangleComboBox.Items.Clear();
			string[] rects = enemyControl.Enemy.Variables[Enemy.EnemyVariables.AnimRectangles].Split(',');
			foreach (string rect in rects)
			{
				rectangleComboBox.Items.Add(rect);
			}
		}

		public void LoadEnemy(string name)
		{
			enemyControl.Enemy = new Enemy();
			enemyControl.Enemy.Initialize(name, enemyControl.Services,0, new Options());
			enemyControl.LoadTextures();
			LoadRectangleData();
			SetKeyFrameList(enemyControl.Enemy.Animations.CurrentAnimationKeyframes);
			SetRectComboBox();
			try
			{
				rectangleComboBox.SelectedItem = enemyControl.Enemy.Animations.CurrentAnimationKeyframe.GetRectangles.Keys.ToArray()[0];
			}
			catch (Exception)
			{

				rectangleComboBox.SelectedItem = Anims.Walk;
			}
			
			enemyControl.CurrentRectangle = (string)rectangleComboBox.SelectedItem;
		}

        public void NewEnemy(string name)
        {
			Directory.CreateDirectory("Data/" + GameConstants.EnemiesPath + name);
            enemyControl.Enemy = new Enemy();
            enemyControl.Enemy.Initialize(enemyControl.Services, new Options());
			enemyControl.Enemy.SetName(name);
            LoadRectangleData();
			SetKeyFrameList(enemyControl.Enemy.Animations.CurrentAnimationKeyframes);
			enemyControl.CurrentRectangle = (string)rectangleComboBox.SelectedItem;
        }

		private void LoadRectangleData()
		{
			if (rectangleComboBox.SelectedItem != null && (string)rectangleComboBox.SelectedItem != "")
			{
				try
				{
					FrameRectangle rect = enemyControl.Enemy.Animations.CurrentAnimationKeyframe.GetRectangles[(string)rectangleComboBox.SelectedItem];
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
					xOriginNumericUpDown.Value = (decimal)rect.Origin.X;
					yOriginNumericUpDown.Value = (decimal)rect.Origin.Y;
				}
				catch (Exception)
				{

					//throw;
				}
			}
		}

        private void SetRectangle()
        {
            FrameRectangle rect = new FrameRectangle();
            Vector2 origin;
			origin = new Vector2((float)xOriginNumericUpDown.Value, (float)yOriginNumericUpDown.Value);
            Microsoft.Xna.Framework.Rectangle pos;
            Microsoft.Xna.Framework.Color color = new Microsoft.Xna.Framework.Color(
                (byte)rColorNumericUpDown.Value, 
                (byte)gColorNumericUpDown.Value, 
                (byte)bColorNumericUpDown.Value, 
                (byte)aColorNumericUpDown.Value);
            pos = new Microsoft.Xna.Framework.Rectangle(
                (int)xPositionNumericUpDown.Value,
                (int)yPositionNumericUpDown.Value,
                (int)widthNumericUpDown.Value,
                (int)heightNumericUpDown.Value);
            float rotation = (float)rotationNumericUpDown.Value;
            float layer = (float)layerNumericUpDown.Value;
            rect.Load(pos, rotation, layer, color, origin);
            enemyControl.Enemy.Animations.CurrentAnimationKeyframe.GetRectangles[(string)rectangleComboBox.SelectedItem] = rect;

        }

        void PlayControl_PlayControlClicked(object sender, PlayControls pressedControls)
        {
            switch (pressedControls)
            { 
                case PlayControls.Play:
					EnableDisableControl(!EnemyToolPanel.Enabled);
                    break;
                case PlayControls.Pause:
                    EnableDisableControl(true);
                    break;
            }
            KeyframeListBox.SelectedIndex = enemyControl.Enemy.Animations.CurrentAnimationKeyFrameValue;
            LoadRectangleData();

        }

        private void EnableDisableControl(bool enable)
        {
            EnemyToolPanel.Enabled = enable;
        }

        private void AnimationComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            enemyControl.Enemy.Animations.CurrentAnimation = (Anims)AnimationComboBox.SelectedItem;
            SetKeyFrameList(enemyControl.Enemy.Animations.CurrentAnimationKeyframes);
            LoadRectangleData();
        }

        private void SetKeyFrameList(Keyframe[] newframes)
        {
            KeyframeListBox.Items.Clear();
            foreach (Keyframe frame in newframes)
            {
                KeyframeListBox.Items.Add(frame);
            }
            KeyframeListBox.SelectedIndex = (int)MathHelper.Clamp(enemyControl.Enemy.Animations.CurrentAnimationKeyFrameValue,0,newframes.Length-1);
        }

        private void KeyframeListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            enemyControl.Enemy.Animations.CurrentAnimationKeyFrameValue = KeyframeListBox.SelectedIndex;
            LoadRectangleData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SetRectangle();
        }

        private void saveToXNBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            enemyControl.Enemy.Animations.ToFile(enemyControl.Enemy.Type);
            enemyControl.Enemy.ToFile();
			foreach (string file in deletableFilesonSave)
			{
				try
				{
					File.Delete(file);
				}
				catch (Exception ex)
				{
					MessageBox.Show(this, "Couldn't delete file: \n" + ex.Message, ex.GetType().ToString(), MessageBoxButtons.OK, MessageBoxIcon.Warning); 
				}
			}
            deletableFilesonSave.Clear();
			MessageBox.Show(this, "Enemy saved succesfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void rectangleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //rectForm.Show();

            rectForm = new AddRectangleForm(enemyControl.Enemy.Type,enemyControl.Enemy.Animations.CurrentAnimationKeyframe.GetRectangles.Keys.ToArray());
            rectForm.RectangleLoaded += new AddRectangleForm.RectLoadEventhandler(rectForm_RectangleLoaded);
            rectForm.ShowDialog(this);
            
        }

        void rectForm_RectangleLoaded(string enemyName, string rectangleName, FrameRectangle rect)
        {
            enemyControl.AddAnimationRectangle(rectangleName, rect);
            //rectangleComboBox.Items.Add(rectangleName);
			
			SetKeyFrameList(enemyControl.Enemy.Animations.CurrentAnimationKeyframes);
			SetRectComboBox();
			rectangleComboBox.SelectedItem = rectangleName;
			enemyControl.CurrentRectangle = (string)rectangleComboBox.SelectedItem;
        }

		private void removeRectangleToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show(
				this,
				"Do you really want to delete " + rectangleComboBox.SelectedItem.ToString() + " ?",
				"Are you Sure?",
				MessageBoxButtons.YesNo,
				MessageBoxIcon.Asterisk) == DialogResult.Yes)
			{
				enemyControl.RemoveAnimationRectangle(rectangleComboBox.SelectedItem.ToString());
				deletableFilesonSave.Add(@"Data\Enemies\" + enemyControl.Enemy.Type + @"\" + rectangleComboBox.SelectedItem.ToString() + ".xnb");
				
				rectangleComboBox.Items.Remove(rectangleComboBox.SelectedItem);
				try
				{
					rectangleComboBox.SelectedIndex = 0;
				}
				catch (Exception)
				{
					
					//throw;
				}
				
			}
		}

        private void copyButton_Click(object sender, EventArgs e)
        {
            Keyframe originframe = (Keyframe)KeyframeListBox.SelectedItem;
            Keyframe copyframe = new Keyframe();
            foreach (string rect in originframe.GetRectangles.Keys)
            {
                copyframe.GetRectangles.Add(rect, originframe.GetRectangles[rect].Copy());
            }
            //copyframe.GetRectangles = originframe.GetRectangles;
            copyframe.KeyFrameNumber = enemyControl.Enemy.Animations.CurrentAnimationKeyFrameCount;
            enemyControl.Enemy.Animations.Animations[(int)enemyControl.Enemy.Animations.CurrentAnimation].AddKeyframe(copyframe);
            SetKeyFrameList(enemyControl.Enemy.Animations.CurrentAnimationKeyframes);
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            enemyControl.Enemy.Animations.Animations[(int)enemyControl.Enemy.Animations.CurrentAnimation].RemoveKeyframe(KeyframeListBox.SelectedIndex);
            SetKeyFrameList(enemyControl.Enemy.Animations.CurrentAnimationKeyframes);
        }

        private void rectangleComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadRectangleData();
			enemyControl.CurrentRectangle = (string)rectangleComboBox.SelectedItem;
        }

        private void linearToolButton_Click(object sender, EventArgs e)
        {
			if (!string.IsNullOrEmpty((string)rectangleComboBox.SelectedItem))
			{
				FrameRectangle[] orgFrames = new FrameRectangle[enemyControl.Enemy.Animations.CurrentAnimationKeyFrameCount];
				for (int i = 0; i < enemyControl.Enemy.Animations.CurrentAnimationKeyFrameCount; i++)
					orgFrames[i] = enemyControl.Enemy.Animations.CurrentAnimationKeyframes[i].GetRectangles[(string)rectangleComboBox.SelectedItem];

				linearForm = new LinearInterpolationForm(
					enemyControl.Enemy.Animations.CurrentAnimationKeyFrameCount,
					enemyControl.Enemy.Animations.CurrentAnimationKeyframe.GetRectangles[(string)rectangleComboBox.SelectedItem],
					orgFrames);
				linearForm.LinearInterpolated += new LinearInterpolationForm.LinearRectangleEventHandler(linearForm_LinearInterpolated);
				linearForm.ShowDialog(this);
			}
        }

        void linearForm_LinearInterpolated(FrameRectangle[] frameRects, int startPos, int endPos)
        {
            for (int i = startPos; i < endPos; i++)
            {
                enemyControl.Enemy.Animations.CurrentAnimationKeyframes[i].GetRectangles[(string)rectangleComboBox.SelectedItem] = frameRects[i-startPos];
            }
            LoadRectangleData();
        }

		private void animationTabControl_Selected(object sender, TabControlEventArgs e)
		{
			EditMode mode = (EditMode)Enum.Parse(typeof(EditMode), e.TabPage.Text);
			enemyControl.SetEditMode(mode);
		}

        private void scaleButton_Click(object sender, EventArgs e)
        {
            float scale = (float)scaleUpDown.Value;
            enemyControl.Enemy.Animations.Scale((float)scaleUpDown.Value);
            enemyControl.Enemy.Collisionrect = new Microsoft.Xna.Framework.Rectangle(
                (int)(enemyControl.Enemy.Collisionrect.X * scale),
                (int)(enemyControl.Enemy.Collisionrect.Y * scale),
                (int)(enemyControl.Enemy.Collisionrect.Width * scale),
                (int)(enemyControl.Enemy.Collisionrect.Height * scale));
            
        }

        private void rectCollisionBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Enemy.CollisionRects rect = (Enemy.CollisionRects)rectCollisionBox.SelectedItem;
            enemyControl.SetModifyRectangle(rect);
        }

        private void Buttonparent_Click(object sender, EventArgs e)
        {
            try
            {
                parentForm.ChildTransformed -= parentForm_ChildTransformed;
            }
            catch (Exception)
            {
                
                //throw;
            }
            parentForm = new ParentForm(enemyControl.Enemy.Animations.Animations[(int)enemyControl.Enemy.Animations.CurrentAnimation]);
            parentForm.ChildTransformed += new RectangleTransformedEventHandler(parentForm_ChildTransformed);
            parentForm.ShowDialog(this);
        }

        void parentForm_ChildTransformed(string child, FrameRectangle[] tranformedRectangles)
        {
            for (int i = 0; i < tranformedRectangles.Length; i++)
            {
                enemyControl.Enemy.Animations.Animations[(int)enemyControl.Enemy.Animations.CurrentAnimation].Frames[i].GetRectangles[child] = tranformedRectangles[i];
            }
            //throw new NotImplementedException();
        }

        private void buttonOffset_Click(object sender, EventArgs e)
        {
            offsetForm = new OffsetForm(enemyControl.Enemy.Animations.Animations[(int)enemyControl.Enemy.Animations.CurrentAnimation]);
            offsetForm.RectangleOffset += new RectangleTransformedEventHandler(offsetForm_RectangleOffset);
            offsetForm.ShowDialog(this);
        }

        void offsetForm_RectangleOffset(string rectangle, FrameRectangle[] tranformedRectangles)
        {
            for (int i = 0; i < tranformedRectangles.Length; i++)
            {
                enemyControl.Enemy.Animations.Animations[(int)enemyControl.Enemy.Animations.CurrentAnimation].Frames[i].GetRectangles[rectangle] = tranformedRectangles[i];
            }
        }


    }
}
