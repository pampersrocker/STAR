using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace StarEdit.EnemyEditor
{
    public enum PlayControls
    { 
        Play,
		Slow,
        Pause,
        Start,
        FastBack,
        StepBack,
        StepForward,
        FastForward,
        End
    }

    public class PlayControl : PlayControlClickEvent
    {
        ContentManager content;
        SpriteFont font;
        Texture2D blank;
        Rectangle background;
        Rectangle[] controlrectangles;
        Texture2D[] controlTextures;
        Rectangle playbar;
        float barvalue = 0;
        //int totalkeyframes;
        //int currentframe;
        float maxX = 100;
        float minX;
        //float height;
        float leftdistance = 50;
        float rightdistance = 50;
        Vector2 position;
        Matrix matrix;
        float width = 150;
        Vector2 mousepos;
        Color[] controlColor;
        Vector2 formlocation;
        Vector2 offset;
        Vector2 editorlocation;
        MouseState oldstate;
        MouseState state;
		KeyboardState oldKeyState;
		KeyboardState keyState;

        

        public float Min
        {
            get { return minX; }
            set 
            { 
                minX = value;
                maxX = MathHelper.Clamp(maxX, minX, float.MaxValue);
            }
        }

        public float Max
        {
            get { return maxX; }
            set 
            { 
                maxX = value;
                minX = MathHelper.Clamp(minX, float.MinValue, maxX);
            }
        }

        public float Width
        {
            get { return width; }
            set 
            { 
                width = value;
                background = new Rectangle(0, 0, (int)width, 100);
            }
        }

        public float Value
        {
            get { return barvalue; }
            set { barvalue = MathHelper.Clamp(value,minX,maxX); }
        }

        public Vector2 Position
        {
            get { return position; }
            set 
            { 
                position = value;
                matrix = Matrix.CreateTranslation(Vector3.Zero);
            }
        }

        public Vector2 EditorLocation
        {
            get { return editorlocation; }
            set { editorlocation = value; }
        }

        public Vector2 FormLocation
        {
            get { return formlocation; }
            set { formlocation = value; }
        }



        public void Initialize(IServiceProvider serviceProvider)
        {
			content = new ContentManager(serviceProvider, "StarEditData");
            font = content.Load<SpriteFont>("Arial");
            blank = content.Load<Texture2D>("Blank");
            controlTextures = new Texture2D[Enum.GetValues(typeof(PlayControls)).Length];
            controlrectangles = new Rectangle[Enum.GetValues(typeof(PlayControls)).Length];
            controlColor = new Color[Enum.GetValues(typeof(PlayControls)).Length];
            foreach (PlayControls control in Enum.GetValues(typeof(PlayControls)))
            {
                controlColor[(int)control] = Color.White;
                controlTextures[(int)control] = content.Load<Texture2D>(control.ToString());
                controlrectangles[(int)control] = new Rectangle((((int)control)) * 25 + (int)leftdistance, 20, 20, 20);
                
            }
            //formlocation = new Vector2();
            //editorlocation = new Vector2();
            offset = new Vector2(12,129);
            background = new Rectangle(0, 0, (int)width, 100);
            oldstate = Mouse.GetState();
        }

        private void UpdateBarValue()
        {
            playbar = new Rectangle((int)leftdistance, 50, (int)((((barvalue) / (maxX - minX) + minX) * (width-rightdistance-leftdistance)) ), 10);
        }

        public void Update(GameTime gameTime)
        {
            matrix = Matrix.CreateTranslation(new Vector3(position, 0));
            UpdateBarValue();
            state = Mouse.GetState();
            mousepos = DetermineMousePos();
			CheckKeys();
            CheckControlIntersection();

        }

		private void CheckKeys()
		{
			if (oldstate == null)
				oldKeyState = Keyboard.GetState();
			keyState = Keyboard.GetState();

			if (keyState.GetPressedKeys().Contains(Keys.Space) && !oldKeyState.GetPressedKeys().Contains(Keys.Space))
			{
				RaisePlayControlClicked(PlayControls.Play);
			}

			oldKeyState = Keyboard.GetState();
		}

        private void CheckControlIntersection()
        {
            Point mouspoint = new Point((int)mousepos.X-(int)position.X,(int)mousepos.Y-(int)position.Y);
            foreach (PlayControls control in Enum.GetValues(typeof(PlayControls)))
            {
                if (controlrectangles[(int)control].Contains(mouspoint))
                {
                    controlColor[(int)control] = new Color(255, 0, 0);
                    if (state.LeftButton == ButtonState.Pressed && oldstate.LeftButton == ButtonState.Released)
                    {
                        //Fire PlayControlClicked Event
                        RaisePlayControlClicked(control);
                    }
                }
                else
                    controlColor[(int)control] = Color.White;
            }
            oldstate = new MouseState(state.X,state.Y,state.ScrollWheelValue,state.LeftButton,state.MiddleButton,state.RightButton,state.XButton1,state.XButton2);
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

        public void Draw(SpriteBatch spriteBatch)
        {
            //background = new Rectangle(0, 0, (int)width, 100);
            //3_1
			//spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState, matrix);
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, matrix);
			spriteBatch.Draw(blank,background, new Color(0,0,0,127));
            spriteBatch.Draw(blank, playbar, Color.White);
            foreach (PlayControls control in Enum.GetValues(typeof(PlayControls)))
            {
                spriteBatch.Draw(controlTextures[(int)control], controlrectangles[(int)control], controlColor[(int)control]);
            }
            spriteBatch.DrawString(font, "Frame #" + barvalue.ToString("000"), Vector2.Zero, Color.White);
            spriteBatch.End();
        }

    }
}
