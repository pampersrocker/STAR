using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Star.Input;
using Star.Game;
using Star.GameManagement;
using Star.Graphics;

namespace Star.Game.Level
{
    public class ParallaxLayer : Layer
    {
        const string graphXPath = "/Backgrounds/Layers/ParallaxLayer/";
        float speedDivider = 1;
        string graphXName = "Default";

        #region Properties
        public float SpeedDivider
        {
            get { return speedDivider; }
            set
            {
                if (value != 0f)
                {
                    speedDivider = value;
                }
                else
                {
                    speedDivider = 1f;
                }
            }
        }

        public Point ScreenSize
        {
            set 
            {
                if (Initalized)
                {
                    LayerObjects[0].Rectangle = new Rectangle(0, 0,
                        value.X, value.Y);
                    LayerObjects[1].Rectangle = new Rectangle(value.X, 0,
                        value.X, value.Y);
                    LayerObjects[0].Position = new Vector2(0);
                    LayerObjects[1].Position = new Vector2(value.X, 0);
                }
            }
        }

        public string GraphXName
        {
            get { return graphXName; }
            set { graphXName = value; }
        }

        #endregion

        #region Layer Abstract Members

        protected override void InitializeGraphX(Star.GameManagement.Options options,LevelVariables levelvariables)
        {
			//LayerObjects[0] = new LayerObject(Content.Load<Texture2D>(GameConstants.GraphXPacksPath + levelvariables.Dictionary[LV.GraphXPack] + graphXPath + graphXName),
			//        new Rectangle(0, 0, options.ScreenWidth, options.ScreenHeight));
			//LayerObjects[1] = new LayerObject(LayerObjects[0].Texture,
			//    new Rectangle(options.ScreenWidth, 0, options.ScreenWidth, options.ScreenHeight));
			Texture2D tex = Content.Load<Texture2D>(GameConstants.GraphXPacksPath + levelvariables.Dictionary[LV.GraphXPack] + graphXPath + graphXName);
			LayerObjects[0] = new LayerObject(tex,
					new Rectangle(0, 0, options.ScreenWidth, (int)(tex.Height / ((float)tex.Width/(float)options.ScreenWidth))));
			LayerObjects[1] = new LayerObject(LayerObjects[0].Texture,
				new Rectangle(options.ScreenWidth, 0, options.ScreenWidth, (int)(tex.Height / ((float)tex.Width / (float)options.ScreenWidth))));
        }

		protected override void UpdateLayer(GameTime gametime, Vector2 player_difference, Star.GameManagement.Options options, Vector2 playerRelativePositioninLevel)
		{
			Vector2 temp = player_difference;
			temp.Y = 0;
			temp.X /= speedDivider;
			LayerObjects[0].Position -= temp;
			LayerObjects[1].Position -= temp;
			if (LayerObjects[0].Position.X > options.ScreenWidth)
			{
				LayerObjects[0].Position = LayerObjects[1].Position - new Vector2(LayerObjects[0].Rectangle.Width, 0);
			}
			else if (LayerObjects[0].Rectangle.Right < 0)
			{
				LayerObjects[0].Position = new Vector2(LayerObjects[1].Rectangle.Right, 0);
			}

			if (LayerObjects[1].Position.X > options.ScreenWidth)
			{
				LayerObjects[1].Position = LayerObjects[0].Position - new Vector2(LayerObjects[1].Rectangle.Width, 0);
			}
			else if (LayerObjects[1].Rectangle.Right < 0)
			{
				LayerObjects[1].Position = new Vector2(LayerObjects[0].Rectangle.Right, 0);
			}
			LayerObjects[0].Position = new Vector2(LayerObjects[0].Position.X, options.ScreenHeight * (1 - playerRelativePositioninLevel.Y));
			LayerObjects[1].Position = new Vector2(LayerObjects[1].Position.X, options.ScreenHeight * (1 - playerRelativePositioninLevel.Y));
			if (LayerObjects[0].Position.Y + LayerObjects[0].Rectangle.Height < options.ScreenHeight)
			{
				LayerObjects[0].Position = new Vector2(LayerObjects[0].Position.X, options.ScreenHeight - LayerObjects[0].Rectangle.Height);
			}
			if (LayerObjects[1].Rectangle.Bottom < options.ScreenHeight)
			{
				LayerObjects[1].Position = new Vector2(LayerObjects[1].Position.X, options.ScreenHeight - LayerObjects[1].Rectangle.Height);
			}
		}

        protected override void DrawLayer(Microsoft.Xna.Framework.Graphics.SpriteBatch spritebatch, Microsoft.Xna.Framework.Matrix matrix)
        {
            //spritebatch.Begin();
            foreach (LayerObject lo in LayerObjects)
            {
                spritebatch.Draw(lo.Texture, lo.Rectangle, Color.White);
            }
            //spritebatch.End();
        }

        #endregion

		#region IGraphicsChange Member

		public override void GraphicsChanged(GraphicsDevice device, Options options)
		{
			LayerObjects[0].Rectangle = new Rectangle(
				0, 
				LayerObjects[0].Rectangle.Y, 
				options.ScreenWidth, 
				options.ScreenHeight);
			LayerObjects[0].Position = new Vector2(0, LayerObjects[0].Rectangle.Y);

			LayerObjects[1].Rectangle = new Rectangle(
				LayerObjects[1].Rectangle.X + options.ScreenWidth, 
				LayerObjects[1].Rectangle.Y, 
				options.ScreenWidth,
				options.ScreenHeight);
			LayerObjects[1].Position = new Vector2(options.ScreenWidth, LayerObjects[1].Rectangle.Y);
		}

		#endregion
	}
}
