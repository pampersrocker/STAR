using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Star.Input;
using Star.Game;
using Star.Graphics;
using Star.GameManagement;

namespace Star.Game.Level
{
	public class CloudLayer : Layer
	{
		const string GraphXPath = "/Backgrounds/Layers/CloudLayer/";
		const float PercentageOfScreen = 0.15f;
		Random rand = new Random(DateTime.Now.Millisecond);

		protected override void InitializeGraphX(Star.GameManagement.Options options,LevelVariables levelvariables)
		{
			string[] clouds = levelvariables.Dictionary[LV.CloudLayerImgs].Split(',');
			LayerObjects = new LayerObject[clouds.Length];
			for (int i = 0; i < LayerObjects.Length; i++)
			{
				LayerObjects[i] = new LayerObject(Content.Load<Texture2D>(GameManagement.GameConstants.GraphXPacksPath + levelvariables.Dictionary[LV.GraphXPack] + GraphXPath + clouds[i]),
					new Rectangle(getRandom(0, options.ScreenWidth), 
						getRandom(0, (int)(options.ScreenHeight * PercentageOfScreen)), 
						(int)(100*options.ScaleFactor), 
						(int)(80*options.ScaleFactor)));
				LayerObjects[i].SpecialRect = new Rectangle(LayerObjects[i].Rectangle.X,LayerObjects[i].Rectangle.Y,LayerObjects[i].Rectangle.Width,LayerObjects[i].Rectangle.Height);
				LayerObjects[i].SpecialValue = getRandom(20,80);
				LayerObjects[i].SpecialValue2 = (float)getRandom(0, 6000) / 3000f;
				LayerObjects[i].SpecialValue3 = (float)getRandom(500, 3000) / 1000f;
			}
		}

		private int getRandom(int min,int max)
		{ 
			return rand.Next(min,max);
		}

		protected override void UpdateLayer(GameTime gametime, Vector2 player_difference,Star.GameManagement.Options options,Vector2 relativePosition)
		{
			foreach (LayerObject lo in LayerObjects)
			{
				lo.Position -= new Vector2(lo.SpecialValue * (float)gametime.ElapsedGameTime.TotalSeconds,0);
				lo.Position -= new Vector2(player_difference.X, 0);
				
				if (lo.Rectangle.Right < 0)
				{ 
					lo.Position = new Vector2(options.ScreenWidth,
						getRandom(0,(int)(options.ScreenHeight * PercentageOfScreen)));
					lo.SpecialValue = getRandom(20, 80);
					lo.SpecialValue2 = (float)getRandom(0, 6000) / 3000f;
					lo.SpecialValue3 = (float)getRandom(500, 3000) / 1000f;
				}
				else if (lo.Rectangle.Left > options.ScreenWidth)
				{
					lo.Position = new Vector2(-lo.Rectangle.Width, 
						getRandom(0,(int)(options.ScreenHeight * PercentageOfScreen)));
					lo.SpecialValue = getRandom(20, 80);
					lo.SpecialValue2 = (float)getRandom(0, 6000) / 3000f;
					lo.SpecialValue3 = (float)getRandom(500, 3000) / 1000f;
				}
				lo.SpecialValue2 += lo.SpecialValue3* (float)gametime.ElapsedGameTime.TotalSeconds;
				if (lo.SpecialValue2 >= 2 * MathHelper.Pi)
				{
					lo.SpecialValue2 = 0;
				}
				lo.Rectangle = new Rectangle(lo.SpecialRect.X, (int)(lo.SpecialRect.Y + Math.Sin(lo.SpecialValue2) * 10), lo.SpecialRect.Width, (int)(lo.SpecialRect.Height +10 * -Math.Sin(lo.SpecialValue2)));
			}
		}

		protected override void DrawLayer(SpriteBatch spritebatch,Matrix matrix)
		{
			//colorizeEffect.Effect.Begin();
			//colorizeEffect.Effect.CurrentTechnique.Passes[0].Begin();
			//spritebatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState);
			//colorizeEffect.Begin();
			
				foreach (LayerObject lo in LayerObjects)
				{
					
					spritebatch.Draw(lo.Texture, lo.Rectangle, new Color(1,1,1, 1.0f));
					
				}
				
			//colorizeEffect.End();
			
			//spritebatch.End();
			//colorizeEffect.Effect.CurrentTechnique.Passes[0].End();


			//colorizeEffect.Effect.End();
			//colorizeEffect.DrawPostProcess(spritebatch, spritebatch.GraphicsDevice);
		}

		#region IGraphicsChange Member

		public override void GraphicsChanged(GraphicsDevice device, Star.GameManagement.Options options)
		{
			for (int i = 0; i < LayerObjects.Length; i++)
			{
				LayerObjects[i].Rectangle = new Rectangle(LayerObjects[i].Rectangle.X, LayerObjects[i].Rectangle.Y,
					(int)(100 * options.ScaleFactor),
					(int)(80 * options.ScaleFactor));
				LayerObjects[i].SpecialRect = new Rectangle(LayerObjects[i].Rectangle.X, LayerObjects[i].Rectangle.Y,
					(int)(100 * options.ScaleFactor),
					(int)(80 * options.ScaleFactor));
			}
		}

		#endregion
	}
}
