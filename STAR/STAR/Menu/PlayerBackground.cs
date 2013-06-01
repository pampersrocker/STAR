using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Star.GameManagement;
using Star.Graphics;

namespace Star.Menu
{

    public class PlayerBackground : IGraphicsChange
    {

        Texture2D textures;
        ContentManager content;
        Rectangle positions;

        public PlayerBackground(IServiceProvider serviceProvider)
        {
            content = new ContentManager(serviceProvider,"Data");
        }

        public void Initialize(Options options)
        {
            textures = content.Load<Texture2D>("Img\\Menu\\Player\\PlayerComplete");
            options.ResolutionChanged += new ResolutionChangedEventHandler(options_ResolutionChanged);
			int height = (int)(((float)options.ScreenWidth / textures.Width) * textures.Height);
			if (height >= options.ScreenHeight)
			{
				positions = new Rectangle(0, options.ScreenHeight - height, options.ScreenWidth, height);
			}
			else
			{
				int width = (int)(((float)options.ScreenHeight / textures.Height) * textures.Width);
				positions = new Rectangle(options.ScreenWidth - width, 0, width, options.ScreenHeight);
			}
			//positions = new Rectangle((int)(options.ScreenWidth - 420), 20, 600, 600);
            GraphicsManager.AddItem(this);
        }

        public void options_ResolutionChanged(Options options, Resolution resolution)
        {
			int height = (int)(((float)options.ScreenWidth / textures.Width) * textures.Height);
			if (height >= options.ScreenHeight)
			{
				positions = new Rectangle(0, options.ScreenHeight - height, options.ScreenWidth, height);
			}
			else
			{
				int width = (int)(((float)options.ScreenHeight / textures.Height) * textures.Width);
				positions = new Rectangle(options.ScreenWidth - width, 0, width, options.ScreenHeight);
			}
            //positions = new Rectangle((int)(options.ScreenWidth - 420*options.ScaleFactor), 20, 600, 600);
        }

		public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, null, null, null, null);
            spriteBatch.Draw(textures, positions, Color.White);
            spriteBatch.End();
        }

        #region IGraphicsChange Member

        public void GraphicsChanged(GraphicsDevice device, Options options)
        {
			int height = (int)(((float)options.ScreenWidth / textures.Width) * textures.Height);
			if (height >= options.ScreenHeight)
			{
				positions = new Rectangle(0, options.ScreenHeight - height, options.ScreenWidth, height);
			}
			else
			{
				int width = (int)(((float)options.ScreenHeight / textures.Height) * textures.Width);
				positions = new Rectangle(options.ScreenWidth - width, 0, width, options.ScreenHeight);
			}
			//positions = new Rectangle((int)(options.ScreenWidth - 420 * options.ScaleFactor), 20, 600, 600);
        }

        public void UnloadGraphicsChanged()
        {
            GraphicsManager.RemoveItem(this);
        }

        #endregion
    }
}
