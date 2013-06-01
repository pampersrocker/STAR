using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Star.GameManagement;
using Star.Input;
using Star.GameManagement.Gamestates.LoadingScreen;
using Star.Menu;
using System.Threading;
using Star.Game.Debug;
using System.Diagnostics;

namespace Star.GameManagement.Gamestates
{
	struct LevelLoadData
	{
		public IServiceProvider serviceProvider;
		public GraphicsDevice device;
		public string level_name;
		public LevelType type;
		public Options options;
	}


	public class SLoadingScreen: IGameState
	{
		PlayerBackground pBackground;
		List<LoadingString> loadingstrings;
		ContentManager content;
		LoadingRotation loadingrotation;
		Texture2D background;
		SpriteFont font;
		Vector2 fontpos;
		float alphaloading;
		float alphatime;
		SGame game;
		Thread loadthread;
		LevelLoadData data;
		Rectangle bgRect;
		Options options;
		bool loaded = false;
		string loadstring="";
		#region IGameState Member
		public SLoadingScreen(IServiceProvider serviceProvider,Options options)
		{
			content = new ContentManager(serviceProvider, "Data");
			loadingrotation = new LoadingRotation(serviceProvider,options);
			loadingstrings = new List<LoadingString>();
			pBackground = new Menu.PlayerBackground(serviceProvider);
		}

		public void Initialize(Options options, GraphicsDevice graphics)
		{
			this.options = options;
			loadingrotation.Initialize();
			background = content.Load<Texture2D>("Img/Menu/StdBG");
			font = content.Load<SpriteFont>("Stuff/Arial");
			fontpos = new Vector2(125, options.ScreenHeight / options.ScaleFactor - 75);
			//bgRect = new Rectangle(0, 0, options.ScreenWidth, options.ScreenHeight);
			int height = (int)(((float)options.ScreenWidth / background.Width) * background.Height);
			if (height >= options.ScreenHeight)
			{
				bgRect = new Rectangle(0, options.ScreenHeight - height, options.ScreenWidth, height);
			}
			else
			{
				int width = (int)(((float)options.ScreenHeight / background.Height) * background.Width);
				bgRect = new Rectangle(options.ScreenWidth - width, 0, width, options.ScreenHeight);
			} 
			pBackground.Initialize(options);
		}

		public EGameState Update(GameTime gameTime, Inputhandler inputhandler, Options options)
		{
			alphatime += 2*(float)gameTime.ElapsedGameTime.TotalSeconds;
			alphaloading = 0.25f * (float)Math.Sin(alphatime) + 0.75f;
			loadingrotation.Update(gameTime);
			for (int i = 0; i < loadingstrings.Count;i++)
			{
				loadingstrings[i].alpha -= (float)gameTime.ElapsedGameTime.TotalSeconds / 10;
			}
			if (loaded)
				return EGameState.Game;
			else if (loaded && inputhandler.GetNewPressedMenuKeys.Contains(MenuKeys.Back))
				return EGameState.Menu;
			return EGameState.Loading;
		}

		public void Draw(GameTime gametime, SpriteBatch spritebatch, GraphicsDevice graphics)
		{
			spritebatch.Begin();
			spritebatch.Draw(background, bgRect, Color.White);
			spritebatch.End();
			pBackground.Draw(spritebatch);
			//3_1
			//spritebatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState, Matrix.CreateScale(options.ScaleFactor));
			spritebatch.Begin(SpriteSortMode.Immediate,
				BlendState.AlphaBlend,
				null, null, null, null,
				Matrix.CreateScale(options.ScaleFactor));
			spritebatch.DrawString(font, loadstring, fontpos, new Color(1,1,1, alphaloading));
			lock (loadingstrings)
			{
				for(int i = 0;i< loadingstrings.Count;i++)
				{
					spritebatch.DrawString(font, loadingstrings[i].Text, loadingstrings[i].pos, new Color(1,1,1, loadingstrings[i].alpha));
				}
			}
			spritebatch.End();
			loadingrotation.Draw(spritebatch, Matrix.CreateScale(options.ScaleFactor));

		}

		public void LoadLevel(SGame game, IServiceProvider serviceProvider,GraphicsDevice device,string level_name, LevelType type,Options options)
		{
			options.InitObjectHolder.particleManagers[EGameState.Game].Stop();
			options.InitObjectHolder.particleManagers[EGameState.Game].Clear();
			loadingstrings.Clear();
			loadstring = "Loading... 0%";
			loaded = false;
			this.game = game;
			data.device = device;
			data.level_name = level_name;
			data.options = options;
			data.serviceProvider = serviceProvider;
			data.type = type;
			game.LevelLoaded += new LevelLoadedEventHandler(game_LevelLoaded);
			game.LevelLoadEnhanced += new LevelLoadEnhancedEventHandler(game_LevelLoadEnhanced);
			loadthread = new Thread(new ThreadStart(LoadLevelThread));
			loadthread.Start();
		}

		void game_LevelLoadEnhanced(byte percentage, string action)
		{
			//throw new NotImplementedException();
			//loadstring = percentage.ToString() + "% " + action;
			DebugManager.AddItem("Level Load enhanced:" + percentage, this.ToString(), new StackTrace());
			loadstring = "Loading... " + percentage + "%";
			foreach (LoadingString lstring in loadingstrings)
			{
				lstring.pos.Y -= 20;
				lstring.alpha -= 0.05f;
			}
			loadingstrings.Add(new LoadingString() { pos = new Vector2(125, fontpos.Y - 50), alpha = 1, Text = action });
		}

		void game_LevelLoaded()
		{
			StackTrace trace;
			trace = new StackTrace();

			DebugManager.AddItem("Level Loaded", this.ToString(), trace, System.Drawing.Color.Green);
			loaded = true;
			loadstring = "Loaded... ";
		}

		public void LoadLevelThread()
		{
			game.LoadLevel(data.serviceProvider, data.device, data.level_name, data.type, data.options);
			game.LevelLoadEnhanced -= game_LevelLoadEnhanced;
		}

		public void Unload()
		{
			loadingrotation = null;
			background.Dispose();
			content.Dispose();
			UnloadGraphicsChanged();
		}

		#endregion


		#region IDisposable Member

		public void Dispose()
		{
			Unload();
		}

		#endregion

		#region IGraphicsChange Member

		public void GraphicsChanged(GraphicsDevice device, Options options)
		{
			int height = (int)(((float)options.ScreenWidth / background.Width) * background.Height);
			if (height >= options.ScreenHeight)
			{
				bgRect = new Rectangle(0, options.ScreenHeight - height, options.ScreenWidth, height);
			}
			else
			{
				int width = (int)(((float)options.ScreenHeight / background.Height) * background.Width);
				bgRect = new Rectangle(options.ScreenWidth - width, 0, width, options.ScreenHeight);
			} 
			//bgRect = new Rectangle(0, 0, options.ScreenWidth, options.ScreenHeight);
		}

		#endregion

		#region IGraphicsChange Member


		public void UnloadGraphicsChanged()
		{
			GraphicsManager.RemoveItem(this);
		}

		#endregion

		#region IInitializeable Member

		bool initialized;

		public bool Initialized
		{
			get { return initialized; }
		}

		public void Initialize(Options options)
		{
			Initialize(options, options.InitObjectHolder.graphics);
			initialized = true;
		}

		#endregion
	}
}
