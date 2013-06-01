using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;


namespace Star.GameManagement.Gamestates
{
	class SCredits : IGameState
	{
		int left;
		List<string> credits;
		float speed=50;
		float xPos = 0.05f;
		SpriteFont font;
		ContentManager content;
		Vector2 pos;
		Rectangle bgrect;
		Texture2D bgTex;
		bool initialized = false;

		public SCredits(IServiceProvider serviceProvider)
		{
			content = new ContentManager(serviceProvider, "Data");
		}

		#region IGameState Member

		protected void Initialize(Options options, GraphicsDevice graphics)
		{
			bgTex = content.Load<Texture2D>("Img/Menu/StdBG");
			bgrect = new Rectangle(0, 0, options.ScreenWidth, options.ScreenHeight);
			credits = new List<string>();
			left = (int)(graphics.PresentationParameters.BackBufferWidth * xPos);
			pos = new Vector2(left, graphics.PresentationParameters.BackBufferHeight);
			font = content.Load<SpriteFont>("Stuff\\Arial");
			credits.Add("Credits:");
			credits.Add("");
			credits.Add("");
			credits.Add("");
			credits.Add("");
			credits.Add("Directed By:");
			credits.Add("Marvin Pohl");
			credits.Add("");
			credits.Add("Executive Producer:");
			credits.Add("Marvin Pohl");
			credits.Add("");
			credits.Add("Supervisor:");
			credits.Add("Marvin Pohl");
			credits.Add("");
			credits.Add("Project Leader:");
			credits.Add("Marvin Pohl");
			credits.Add("");
			credits.Add("Leading Engineer:");
			credits.Add("Marvin Pohl");
			credits.Add("");
			credits.Add("Chief Technology Engineer:");
			credits.Add("Marvin Pohl");
			credits.Add("");
			credits.Add("Art Director:");
			credits.Add("Marvin Pohl");
			credits.Add("");
			credits.Add("Build Manager:");
			credits.Add("Marvin Pohl");
			credits.Add("");
			credits.Add("Lead Programmer:");
			credits.Add("Marvin Pohl");
			credits.Add("");
			credits.Add("Graphics & Design:");
			credits.Add("Marvin Pohl");
			credits.Add("");
			credits.Add("Lead Effect Artist:");
			credits.Add("Marvin Pohl");
			credits.Add("");
			credits.Add("Lead Environment Artist:");
			credits.Add("Marvin Pohl");
			credits.Add("");
			credits.Add("Lead Technical Artist:");
			credits.Add("Marvin Pohl");
			credits.Add("");
			credits.Add("Lead Animator:");
			credits.Add("Marvin Pohl");
			credits.Add("");
			credits.Add("Lead Designer:");
			credits.Add("Marvin Pohl");
			credits.Add("");
			credits.Add("Level Builder:");
			credits.Add("Marvin Pohl");
			credits.Add("");
			credits.Add("Game Design:");
			credits.Add("Marvin Pohl");
			credits.Add("");
			credits.Add("Senior IT Manager:");
			credits.Add("Marvin Pohl");
			credits.Add("");
			credits.Add("GUI & Menu:");
			credits.Add("Marvin Pohl");
			credits.Add("");
			credits.Add("");
			credits.Add("");
			credits.Add("");
			credits.Add("Music:");
			credits.Add("Marvin Pohl - Industrial");
			credits.Add("");
			credits.Add("Following music is under creative commons 3.0 by-nc-sa");
			credits.Add("http://creativecommons.org/licenses/by-nc-sa/3.0/");
			credits.Add("Legal Code:");
			credits.Add("http://creativecommons.org/licenses/by-nc-sa/3.0/legalcode");
			
			credits.Add(""); 
			credits.Add("");
			credits.Add("Artist: SquattingDog88101");
			credits.Add("Title: Shave While I Rave");
			credits.Add("Website: http://squattingdog88101.newgrounds.com/");
			credits.Add("");

			credits.Add("Artist: Bafana");
			credits.Add("Title: Rock-tronic");
			credits.Add("Website: http://bafana.newgrounds.com/");
			credits.Add("");

			credits.Add("Artist: DJ-Delinquent");
			credits.Add("Title: Halcyonic Falcon X");
			credits.Add("Website: http://squattingdog88101.newgrounds.com/");
			credits.Add("");

			credits.Add("Artist: robothair");
			credits.Add("Title: Serious Business");
			credits.Add("Website: http://robothair.newgrounds.com/");
			credits.Add("");

			credits.Add("Artist: YanX");
			credits.Add("Title: No Time To Cry");
			credits.Add("Website: http://yanx.newgrounds.com/");
			credits.Add("");

			credits.Add("Artist: MordiNor");
			credits.Add("Title: Poison");
			credits.Add("Website: http://mordinor.newgrounds.com/");
			credits.Add("");

			credits.Add("Artist: Nijg");
			credits.Add("Title: Space Oblivion");
			credits.Add("Website: http://nijg.newgrounds.com/");
			credits.Add("");

			credits.Add("Artist: Pakerman1700");
			credits.Add("Title: Tetris Remix");
			credits.Add("Website: http://parkerman1700.newgrounds.com/");
			credits.Add("");

			credits.Add("Artist: Slug-Salt");
			credits.Add("Title: Thundersocks");
			credits.Add("Website: http://slug-salt.newgrounds.com/");
			credits.Add("");

			credits.Add("Artist: SoundReaper");
			credits.Add("Title: The Jungle");
			credits.Add("Website: http://soundreaper.newgrounds.com/");
			credits.Add("");

			credits.Add("");
			credits.Add("");
			credits.Add("");
			credits.Add("");
			credits.Add("");
			credits.Add("");
			credits.Add("");
			credits.Add("");
			credits.Add("");
			credits.Add("");
			credits.Add("");
			credits.Add("");
			credits.Add("THORN!");
			

		}

		public void ResetCredits(Resolution res)
		{
			pos = new Vector2(res.ScreenWidth * xPos, res.ScreenHeight);
		}

		public EGameState Update(Microsoft.Xna.Framework.GameTime gameTime, Input.Inputhandler inputhandler, Options options)
		{
			string text = "";
			for (int i = 0; i < credits.Count; i++)
			{
				text += credits[i] + "\n";
			}
			pos.Y -= speed * gameTime.GetElapsedTotalSecondsFloat();
			if (inputhandler.GetMenuKeys.Contains(Input.MenuKeys.Down))
			{
				pos.Y -= 2*speed * gameTime.GetElapsedTotalSecondsFloat();
			}
			if (inputhandler.GetMenuKeys.Contains(Input.MenuKeys.Up))
			{
				pos.Y += 2*speed * gameTime.GetElapsedTotalSecondsFloat();
			}
			if (pos.Y + font.MeasureString(text).Y < 0 || inputhandler.GetNewPressedMenuKeys.Contains(Input.MenuKeys.Back))
			{
				return EGameState.Menu;
			}
			else
			{
				return EGameState.Credits;
			}
		}

		public void Draw(Microsoft.Xna.Framework.GameTime gametime, Microsoft.Xna.Framework.Graphics.SpriteBatch spritebatch, GraphicsDevice graphics)
		{
			string text = "";
			for (int i = 0; i < credits.Count; i++)
			{
				text += credits[i] + "\n";
			}
			spritebatch.Begin();
			spritebatch.Draw(bgTex, bgrect, Color.White);
			spritebatch.DrawString(font, text, pos, Color.White);
			spritebatch.End();
		}

		public void Unload()
		{
			font = null;
			credits = null;
			content.Dispose();
		}

		#endregion

		#region IDisposable Member

		public void Dispose()
		{
			Unload();
		}

		#endregion

		#region IGraphicsChange Member

		public void GraphicsChanged(Microsoft.Xna.Framework.Graphics.GraphicsDevice device, Options options)
		{
			left = (int)(options.Resolution.ScreenWidth * xPos);

			bgrect = new Rectangle(0, 0, options.ScreenWidth, options.ScreenHeight);
		}

		public void UnloadGraphicsChanged()
		{
			GraphicsManager.RemoveItem(this);
		}

		#endregion

		#region IInitializeable Member

		public bool Initialized
		{
			get { return initialized; }
		}

		public void Initialize(Options options)
		{
			Initialize(options,options.InitObjectHolder.graphics);
			initialized = true;
		}

		#endregion
	}
}
