using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Star.GameManagement.Gamestates.LoadingScreen
{
	class LoadingRotation
	{
		Rectangle screen;
		ContentManager content;
		Circle innercircle;
		Circle outercircle;
		Options options;
		public LoadingRotation(IServiceProvider serviceProvider,Options options)
		{
			this.options = options;
			screen = new Rectangle(0, 0, options.ScreenWidth, options.ScreenHeight);
			content = new ContentManager(serviceProvider, "Data");
		}

		public void Initialize()
		{
			int size = 75;
			innercircle = new Circle(new Vector2(100 , screen.Height / options.ScaleFactor - 25), content.Load<Texture2D>("Img/Game/LSIC"), size, size);
			System.Threading.Thread.Sleep(10);
			outercircle = new Circle(new Vector2(100, screen.Height / options.ScaleFactor - 25), content.Load<Texture2D>("Img/Game/LSOC"), size, size);
		}

		public void Update(GameTime gameTime)
		{
			innercircle.Update(gameTime);
			outercircle.Update(gameTime);
		}

		public void Draw(SpriteBatch spriteBatch, Matrix matrix)
		{
			innercircle.Draw(spriteBatch, matrix);
			outercircle.Draw(spriteBatch, matrix);
		}
	}
}
