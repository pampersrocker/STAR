using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Star.GameManagement;
using WinFormsGraphicsDevice;
using System.Windows.Forms;
using Star.Game;
using Star.Game.Level;
//using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using Star.Game.Enemy;
using Microsoft.Xna.Framework.Input;

namespace StarEdit.EnemyEditor
{
    public partial class EnemyControl : GraphicsDeviceControl
    {
        //Timer timer;
        SpriteBatch spriteBatch;
        ContentManager content;
        Enemy enemy;
        Texture2D tex;
		SpriteFont font;
        PlayControl playcontrol;
		Vector2 formlocation;
		Vector2 editorlocation;
		Vector2 offset;
        bool playing = false;
		bool moving = false;
		bool rotating = false;
		Vector2 rotatingStart;
		bool moveDataXY;
		bool moveDataWidthHeight;
		MouseState oldstate;
		Vector2 mousepos;
		Vector2 oldMousePos;
		string currentRect;
		bool playSlow = false;
		int slowPlayer = 0;
		Dictionary<string, Texture2D> textures;
		EditMode editMode = EditMode.Animation;
        Enemy.CollisionRects colRect = Enemy.CollisionRects.BoundingBox;
		bool[] modifyCollision;
		

		public delegate void RectangleChangedEventHandler(FrameRectangle newrect);

		public event RectangleChangedEventHandler CurrentRectangleChanged;

		public EnemyControl()
		{
			playcontrol = new PlayControl();
		}

        public Enemy Enemy
        {
            get { return enemy; }
            set { enemy = value; }
        }

        public PlayControl PlayControl
        {
            get { return playcontrol; }
            set { playcontrol = value; }
        }

		public string CurrentRectangle
		{
			set { currentRect = value; }
		}

        public void LoadEnemy(string name)
        {
            enemy = new Star.Game.Enemy.Enemy();
            enemy.Alive = true;
            enemy.Initialize(name, Services, 0,new Options());
			LoadTextures();
        }

        public void LoadTexture(string filename)
        {
            WinFormsContentLoading.ContentBuilder builder = new WinFormsContentLoading.ContentBuilder();
            builder.Add(filename, "Test", null, "TextureProcessor");

            string error = builder.Build();

            content.RootDirectory = builder.OutputDirectory;

            tex = content.Load<Texture2D>("Test");
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            this.Focus();
            base.OnMouseClick(e);
        }

		protected override void OnMouseDown(MouseEventArgs e)
		{
			Vector2 difference = DetermineMousePos() - new Vector2(this.Size.Width / 2, this.Size.Height / 2);

			if (difference.Length() > 0 && !string.IsNullOrEmpty(currentRect))
			{

				FrameRectangle rect = Enemy.Animations.CurrentAnimationKeyframe.GetRectangles[currentRect];
				//if (e.Button == MouseButtons.Left)
				{
					switch (editMode)
					{
						case EditMode.Animation:
							if (rect.Rect.Contains(new Point((int)difference.X + rect.Rect.Width / 2, (int)difference.Y + rect.Rect.Height / 2)) && e.Button == MouseButtons.Left)
								moving = true;
                            if (e.Button == MouseButtons.Right)
                            {
                                if (rect.Rect.Contains(new Point((int)difference.X + rect.Rect.Width / 2, (int)difference.Y + rect.Rect.Height / 2)))
                                {
                                    rotating = true;
                                    rotatingStart = DetermineMousePos();
                                }
                            }
							break;
						case EditMode.Data:
							if (enemy.Collisionrect.Contains((int)DetermineMousePos().X - Size.Width / 2, (int)DetermineMousePos().Y - Size.Height / 2))
							{
								if (e.Button == MouseButtons.Left)
									moveDataXY = true;
								else if (e.Button == MouseButtons.Right)
									moveDataWidthHeight = true;
							}
							break;
					}
				}
				
			}
		
			oldstate = Mouse.GetState();
			base.OnMouseDown(e);
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{


			if (e.Button == MouseButtons.Left)
			{
				moveDataXY = false;
				moving = false;
			}
			else if (e.Button == MouseButtons.Right)
			{
				moveDataWidthHeight = false;
				rotating = false;
			}
			base.OnMouseUp(e);
		}

		private Vector2 DetermineMousePos()
		{
			Vector2 pos;

			pos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
			pos -= formlocation;
			pos -= offset;
			pos -= editorlocation;
			return pos;
		}

        protected override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
			MouseState state = Mouse.GetState();
			mousepos = DetermineMousePos();
			switch (editMode)
			{
				case EditMode.Animation:
					UpdateAnimationMouse(state);
					break;
				case EditMode.Data:
					UpdateDataMouse(state);
					break;
			}
            if (playing || playSlow)
            {
				if (playSlow)
				{
					slowPlayer++;
					if (slowPlayer >= 3)
					{
						slowPlayer = 0;
						enemy.Update(gameTime);
					}

				}
				else
					enemy.Update(gameTime);
                playcontrol.Value = enemy.Animations.CurrentAnimationKeyFrameValue;
                playcontrol.Max = enemy.Animations.CurrentAnimationKeyFrameCount - 1;
                //playcontrol.CurrentKeyFrame = enemy.Animations.CurrentAnimationKeyFrameValue;
            }
            enemy.Pos = new Vector2(this.Size.Width / 2, this.Size.Height / 2);
            playcontrol.Width = this.Size.Width;
            playcontrol.Position = new Vector2(0, this.Size.Height - 100);
            playcontrol.Update(gameTime);
			oldMousePos = mousepos;
			oldstate = state;
        }

		private void UpdateDataMouse(MouseState state)
		{
			Vector2 difference = mousepos - oldMousePos;
            Rectangle rect;
            switch (colRect)
            { 
                case Enemy.CollisionRects.BoundingBox:
                    rect = enemy.Collisionrect;
                    break;
                case Enemy.CollisionRects.DieRect:
                    rect = enemy.Animations.CurrentAnimationKeyframe.DieRect.OriginalRectangle;
                    break;
                case Enemy.CollisionRects.KillingRect:
                    rect = enemy.Animations.CurrentAnimationKeyframe.KillingRect.OriginalRectangle;
                    break;
                default:
                    rect = new Rectangle();
                    break;
            }
			

			if (playing || playSlow)
			{
				if (moveDataXY)
				{
					rect.X = (int)mousepos.X-this.Size.Width/2;
					rect.Y = (int)mousepos.Y-this.Size.Height/2;
				}
				else if (moveDataWidthHeight)
				{
					rect.Width = (int)mousepos.X - rect.X - this.Size.Width/2;
					rect.Height = (int)mousepos.Y - rect.Y - this.Size.Height/2;
				}
			}
			else
			{
				if (moveDataXY)
				{
					rect.X += (int)difference.X;
					rect.Y += (int)difference.Y;
				}
				if (moveDataWidthHeight)
				{
					rect.Width += (int)difference.X;
					rect.Height += (int)difference.Y;
				}
			}

            switch (colRect)
            {
                case Enemy.CollisionRects.BoundingBox:
                    enemy.Collisionrect = rect;
                    break;
                case Enemy.CollisionRects.DieRect:
                    enemy.Animations.CurrentAnimationKeyframe.DieRect = new SpecialRect(rect);
                    break;
                case Enemy.CollisionRects.KillingRect:
                    enemy.Animations.CurrentAnimationKeyframe.KillingRect = new SpecialRect(rect);
                    break;
            }
			
		}

		private void UpdateAnimationMouse(MouseState state)
		{
			if (moving)
			{

				Vector2 oldpos = new Vector2(oldstate.X, oldstate.Y);
				Vector2 newpos = new Vector2(state.X, state.Y);
				Vector2 difference = DetermineMousePos() - new Vector2(this.Size.Width / 2, this.Size.Height / 2);

				if (difference.Length() > 0 && !string.IsNullOrEmpty(currentRect))
				{

					FrameRectangle rect = Enemy.Animations.CurrentAnimationKeyframe.GetRectangles[currentRect];

					{
						rect.Rect.X = (int)difference.X;
						rect.Rect.Y = (int)difference.Y;
						Enemy.Animations.CurrentAnimationKeyframe.GetRectangles[currentRect] = rect;
						CurrentRectangleChanged(rect);
					}
				}
			}
			else if (rotating)
			{
				FrameRectangle rect = Enemy.Animations.CurrentAnimationKeyframe.GetRectangles[currentRect];
				rect.Rotation = ((rotatingStart - DetermineMousePos()) / 30f).Y;
				Enemy.Animations.CurrentAnimationKeyframe.GetRectangles[currentRect] = rect;
				
				CurrentRectangleChanged(rect);
			}
		}

        protected override void Initialize()
        {
            Initialize(null);
        }

        protected void Initialize(string name)
        {
			
			mousepos = new Vector2();
			oldMousePos = new Vector2();
			offset = new Vector2(12,128);
			formlocation = new Vector2();
			editorlocation = new Vector2();
			ContentManager content2 = new ContentManager(Services, "StarEditData");
			font = content2.Load<SpriteFont>("Arial");
            content = new ContentManager(Services);
            content.RootDirectory = "Data";
            spriteBatch = new SpriteBatch(GraphicsDevice);
            enemy = new Enemy();
            enemy.Alive = true;
			textures = new Dictionary<string, Texture2D>();
			tex = content.Load<Texture2D>("Stuff/Blank");
            if (name != null)
            {
                enemy.Initialize(name, Services,0,new Options());
				LoadTextures();
            }
            else
            {
                enemy.Initialize( Services, new Options());
            }
			playcontrol.Initialize(Services);
            playcontrol.Max = enemy.Animations.CurrentAnimationKeyFrameCount;
            //playcontrol.TotalKeyFrames = enemy.Animations.CurrentAnimationKeyFrameCount;
            playcontrol.Value = enemy.Animations.CurrentAnimationKeyFrameValue;
            playcontrol.Width = this.Size.Width;
            playcontrol.PlayControlClicked += new PlayControlClickEvent.PlayControlEventHandler(playcontrol_PlayControlClicked);
            //playcontrol.Offset = new Vector2(0, 50) + new Vector2(this.Location.X,this.Location.Y);
			modifyCollision = new bool[Enum.GetValues(typeof(LiveEdit)).Length];
        }

		public void LoadTextures()
		{
			string[] rects = enemy.Variables[Enemy.EnemyVariables.AnimRectangles].Split(',');
			foreach (string rect in rects)
			{
				try
				{ 
					textures.Add(rect, content.Load<Texture2D>(GameConstants.EnemiesPath + enemy.Type + "/" + rect));
				}
				catch
				{
					textures.Add(rect,content.Load<Texture2D>("Stuff/Error"));
				}
			}
		}

        void playcontrol_PlayControlClicked(object sender, PlayControls pressedControls)
        {
            switch (pressedControls)
            { 
                case PlayControls.Play:
                    playing = !playing;
					playSlow = false;
                    break;
				case PlayControls.Slow:
					playSlow = !playSlow;
					break;
                case PlayControls.Pause:
                    playing = false;
					playSlow = false;
                    break;
                case PlayControls.Start:
                    enemy.Animations.CurrentAnimationKeyFrameValue = 0;
                    //playcontrol.CurrentKeyFrame = 0;
                    playcontrol.Value = 0;
                    break;
                case PlayControls.End:
                    enemy.Animations.CurrentAnimationKeyFrameValue = enemy.Animations.CurrentAnimationKeyFrameCount-1;
                    playcontrol.Value = enemy.Animations.CurrentAnimationKeyFrameValue;
                    //playcontrol.CurrentKeyFrame = enemy.Animations.CurrentAnimationKeyFrameValue;
                    break;
                case  PlayControls.StepForward:
                    int newvalue = enemy.Animations.CurrentAnimationKeyFrameValue +1 ;
                    if (newvalue >= enemy.Animations.CurrentAnimationKeyFrameCount)
                        newvalue = 0;
                    enemy.Animations.CurrentAnimationKeyFrameValue = newvalue;
                    //playcontrol.CurrentKeyFrame = newvalue;
                    playcontrol.Value = newvalue;
                    break;
                case PlayControls.StepBack:
                    int decrement = enemy.Animations.CurrentAnimationKeyFrameValue - 1;
                    if (decrement < 0)
                        decrement = enemy.Animations.CurrentAnimationKeyFrameCount - 1;
                    enemy.Animations.CurrentAnimationKeyFrameValue = decrement;
                    //playcontrol.CurrentKeyFrame = decrement;
                    playcontrol.Value = decrement;
                    break;
                case PlayControls.FastBack:
                    int fastback = enemy.Animations.CurrentAnimationKeyFrameValue - 10;
                    if (fastback < 0)
                        fastback = enemy.Animations.CurrentAnimationKeyFrameCount - 1;
                    enemy.Animations.CurrentAnimationKeyFrameValue = fastback;
                    playcontrol.Value = fastback;
                    break;
                case PlayControls.FastForward:
                    int fastforward = enemy.Animations.CurrentAnimationKeyFrameValue + 10;
                    if (fastforward >= enemy.Animations.CurrentAnimationKeyFrameCount)
                        fastforward = 0;
                    enemy.Animations.CurrentAnimationKeyFrameValue = fastforward;
                    playcontrol.Value = fastforward;
                    break;
            }
            playcontrol.Max = enemy.Animations.CurrentAnimationKeyFrameCount - 1;
        }

        public void SetEditorLocation(System.Drawing.Point location)
        {
            playcontrol.EditorLocation = new Vector2(location.X, location.Y);
			editorlocation = new Vector2(location.X, location.Y);
        }

        public void SetFormLocation(System.Drawing.Point location)
        {
            playcontrol.FormLocation = new Vector2(location.X, location.Y);
			formlocation = new Vector2(location.X, location.Y);
        }

        protected override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
			
            SpecialRect killRect;
			if (enemy.Animations.CurrentAnimationKeyframe.KillingRect != null)
				killRect = new SpecialRect(enemy.Animations.CurrentAnimationKeyframe.KillingRect.Rectangle);
			else
				killRect = new SpecialRect(new Rectangle());
            Rectangle temp = new Rectangle(
                killRect.OriginalRectangle.X + (int)enemy.Pos.X, 
                killRect.OriginalRectangle.Y + (int)enemy.Pos.Y,
                killRect.OriginalRectangle.Width,
                killRect.OriginalRectangle.Height);
            killRect.Rectangle = temp;
			SpecialRect dieRect;
			if (enemy.Animations.CurrentAnimationKeyframe.DieRect != null)

				dieRect = new SpecialRect(enemy.Animations.CurrentAnimationKeyframe.DieRect.Rectangle);
			else
				dieRect = new SpecialRect(new Rectangle());
            temp = new Rectangle(
                dieRect.OriginalRectangle.X + (int)enemy.Pos.X,
                dieRect.OriginalRectangle.Y + (int)enemy.Pos.Y,
                dieRect.OriginalRectangle.Width,
                dieRect.OriginalRectangle.Height);
            dieRect.Rectangle = temp;
			Rectangle collision = new Rectangle(enemy.Collisionrect.X,
				enemy.Collisionrect.Y,
				enemy.Collisionrect.Width,
				enemy.Collisionrect.Height);
			collision.Location = new Point((int)enemy.Pos.X + collision.X, (int)enemy.Pos.Y + collision.Y);
            GraphicsDevice.Clear(Color.CornflowerBlue);
			spriteBatch.Begin();
			for (int i = 0; i*20 < Size.Height; i++)
			{
				spriteBatch.Draw(tex, new Rectangle(Size.Width / 2, i*20, 2, 10), new Color(255,255,255, (byte)127));
			}
			spriteBatch.End();
            enemy.Draw(gameTime, spriteBatch, Matrix.CreateTranslation(Vector3.Zero),textures,null,null);
            if (editMode == EditMode.Data)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(tex, collision, new Color(0, 0, 255, 127));
                spriteBatch.Draw(tex, killRect.Rectangle, new Color(255,0,0, 127));
                spriteBatch.Draw(tex, dieRect.Rectangle, new Color(0,255,0, 127));
                //spriteBatch.DrawString(font,DetermineMousePos().ToString(), Vector2.Zero, Color.Red);
                spriteBatch.End();
            }
            playcontrol.Draw(spriteBatch);
            
        }

        public void AddAnimationRectangle(string rectName, FrameRectangle newrect)
        {
            Texture2D tex = content.Load<Texture2D>(GameConstants.EnemiesPath + Enemy.Type + "/" + rectName);
            Enemy.AddRectangle(rectName, tex, newrect);
			textures.Add(rectName, tex);
        }

		public void RemoveAnimationRectangle(string rectName)
		{
			textures.Remove(rectName);
			enemy.RemoveRectangle(rectName);
			
			

		}

		public void SetEditMode(EditMode mode)
		{
			editMode = mode;
		}

        public void SetModifyRectangle(Enemy.CollisionRects colRects)
        {
            this.colRect = colRects;
        }

        
    }
}
