using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Star.Game.Level;
using Star.GameManagement;
using Star.Game.GameEvents;
using Star.GameManagement.Gamestates;

namespace Star.Game.GameEvents
{
    public class ExitGameEvent : GameEvent
    {
        Rectangle exit;
        Texture2D blank;
        Rectangle rect;
        Color backcolor;
        float alpha = 0;
        const float TotalTime = 5;

		protected override bool CheckActvationEvent(GameTime gameTime, Rectangle playerBoundingBox, int timeLeft, Quadtree quadtree, SGame stateGame)
        {
            bool activation;
            if (Enabled == false)
            {
                if (playerBoundingBox.Intersects(exit))
                {
                    activation = true;
                    backcolor = new Color(0, 0, 0, 0);
                }
                else
                {
                    activation = false;
                }
            }
            else
            {
                activation = Enabled;
            }
            return activation;
        }

        protected override DrawPosition InitializeEvent(IServiceProvider serviceProvider,GraphicsDevice device,LevelVariables levelVariables,Options options)
        {
            exit = levelVariables.ExitRectangle;
            blank = Content.Load<Texture2D>("Stuff/Blank");
            rect = new Rectangle(0, 0, options.ScreenWidth, options.ScreenHeight);
            return DrawPosition.InFrontOfLevel;
        }

        protected override void DrawEvent(GameTime gameTime, SpriteBatch spriteBatch, Matrix matrix)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(blank, rect, backcolor);
            spriteBatch.End();
        }

        protected override EventAction UpdateEvent(GameTime gameTime,List<Enemy.Enemy> enemies)
        {
            EventAction action = EventAction.BlockInput;
            alpha += (float)gameTime.ElapsedGameTime.TotalSeconds;
            alpha = MathHelper.Clamp(alpha, 0, 1);
            backcolor.A = (byte)(alpha*255);
            if (alpha >= 1)
            {
                action = EventAction.Exit;
            }

            return action;
        }

        public override void Start()
        {
            Enabled = true;
            backcolor = new Color(0, 0, 0, 0);
        }

		public override void Dispose()
		{
			//throw new NotImplementedException();
		}
	}
}
