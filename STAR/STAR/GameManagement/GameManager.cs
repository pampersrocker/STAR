using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Star.Input;
using Star.GameManagement.Gamestates;
using Star.Graphics.Effects.PostProcessEffects;
using Star.Graphics;
using Star.Audio;

namespace Star.GameManagement
{
	public enum GameAction
	{ 
		Nothing,
		Exit
	}

	class GameManager
	{
		IServiceProvider serviceprovider;
		SpriteBatch spritebatch;
		Inputhandler inputhandler;
		EGameState cur_state = EGameState.Intro;
		Options options;
		EGameState oldState;
		GraphicsDeviceManager graphicsDeviceManager;
		RenderTarget2D currentTarget;
		RenderTarget2D oldStateTarget;
		Rectangle oldStateRectangle;
		Rectangle curStateRectangle;
		ContentManager content;
		float oldStateAlpha;
		bool drawnOldState;
		Matrix oldStateMatrix,curStateMatrix;
		float oldStateScale,curStateScale;
		bool disposed;

		Music music;
		
		SGame game;
		SMenu menu;
		SLoadingScreen loadingscreen;
		SCredits credits;

		readonly int numberOfTimes = 500;
		TimeSpan[] gameTimes;
		int currentPosTime;

		ParticleThreadManager pthreadManager;

		public GameManager(EGameState state,IServiceProvider ServiceProvider)
		{
			cur_state = state;
			options = new Options();
			options.DisplayModeChanged += new DisplayModeChangedEventHandler(options_DisplayModeChanged);
			options.ResolutionChanged += new ResolutionChangedEventHandler(options_ResolutionChanged);
			game = new SGame(ServiceProvider);
			inputhandler = new Inputhandler(Vector2.Zero,options);
			menu = new SMenu(ServiceProvider);
			serviceprovider = ServiceProvider;
			content = new ContentManager(serviceprovider, "Data");
			music = new Music();
			credits = new SCredits(serviceprovider);
			pthreadManager = new ParticleThreadManager();
			gameTimes = new TimeSpan[numberOfTimes];
			for (int i = 0; i < numberOfTimes; i++)
			{
				gameTimes[i] = new TimeSpan(0, 0, 0, 0, 16);
			}
			
		}

		TimeSpan GetAverageTimeSpan()
		{
			TimeSpan average = new TimeSpan();
			int count=0;
			for (int i = 0; i < numberOfTimes; i++)
			{
				if (gameTimes[i] != null)
				{
					count++;
					average += gameTimes[i];
				}
			}
			average = new TimeSpan(0, 0, 0, 0, (int)(average.TotalMilliseconds / count));
			return average;
		}

		void options_ResolutionChanged(Options options, Resolution resolution)
		{
			graphicsDeviceManager.PreferredBackBufferWidth = options.ScreenWidth;
			graphicsDeviceManager.PreferredBackBufferHeight = options.ScreenHeight;
			graphicsDeviceManager.ApplyChanges();
			GraphicsChanged();
		}

		void options_DisplayModeChanged(Options options, DisplayMode mode)
		{
			graphicsDeviceManager.IsFullScreen = options.IsFullScreen;
			graphicsDeviceManager.ApplyChanges();

		}

		public void Initialize(GraphicsDeviceManager graphics)
		{
			options = Options.FromSettingsFile();
			options.DisplayModeChanged += new DisplayModeChangedEventHandler(options_DisplayModeChanged);
			options.ResolutionChanged += new ResolutionChangedEventHandler(options_ResolutionChanged);
			inputhandler = new Inputhandler(Vector2.Zero, options);
			graphicsDeviceManager = graphics;
			spritebatch = new SpriteBatch(graphics.GraphicsDevice);
			/*options.ScreenHeight = graphics.GraphicsDevice.Viewport.Height;
			options.ScreenWidth = graphics.GraphicsDevice.Viewport.Width;*/
			graphics.PreferredBackBufferHeight = options.ScreenHeight;
			graphics.PreferredBackBufferWidth = options.ScreenWidth;
			graphicsDeviceManager.IsFullScreen = options.IsFullScreen;
			graphics.ApplyChanges();
			options.ScreenWidth = graphics.GraphicsDevice.PresentationParameters.BackBufferWidth;
			options.ScreenHeight = graphics.GraphicsDevice.PresentationParameters.BackBufferHeight;
			options.InitObjectHolder.graphics = graphics.GraphicsDevice;

			options.InitObjectHolder.serviceProvider = serviceprovider;
			menu.Initialize(options);
			game.Initialize(options);
			loadingscreen = new SLoadingScreen(serviceprovider, options);
			loadingscreen.Initialize(options, graphics.GraphicsDevice);
			credits.Initialize(options);
			GraphicsManager.AddItem(credits);
			GraphicsManager.AddItem(game);
			GraphicsManager.AddItem(menu);
			GraphicsManager.AddItem(loadingscreen);
			graphics.DeviceDisposing += new EventHandler<EventArgs>(graphics_DeviceDisposing);
			graphics.DeviceCreated += new EventHandler<EventArgs>(graphics_DeviceCreated);
			graphics.Disposed += new EventHandler<EventArgs>(graphics_Disposed);
			PresentationParameters pp = graphics.GraphicsDevice.PresentationParameters;

			currentTarget = new RenderTarget2D(
				graphics.GraphicsDevice,
				pp.BackBufferWidth, pp.BackBufferHeight, false,
				SurfaceFormat.Color, DepthFormat.None, pp.MultiSampleCount, RenderTargetUsage.PreserveContents);
			oldStateTarget = new RenderTarget2D(
				graphics.GraphicsDevice,
				pp.BackBufferWidth, pp.BackBufferHeight, false,
				SurfaceFormat.Color, DepthFormat.None, pp.MultiSampleCount, RenderTargetUsage.PreserveContents);
			oldStateRectangle = new Rectangle(0, 0, options.ScreenWidth, options.ScreenHeight);
			curStateRectangle = new Rectangle(0, 0, options.ScreenWidth, options.ScreenHeight);
			curStateScale = .7f;
			oldStateScale = 1f;
			music.Initialize(serviceprovider,options);
			
		}

		void graphics_Disposed(object sender, EventArgs e)
		{
			disposed = true;
		}

		void graphics_DeviceCreated(object sender, EventArgs e)
		{
			disposed = false;
		}

		void graphics_DeviceDisposing(object sender, EventArgs e)
		{
			disposed = true;
		}


		public GameAction Update(GameTime gametime,GraphicsDevice device)
		{
			if (gametime.GetElapsedTotalSecondsFloat() < 0.1)
			{

				gameTimes[currentPosTime] = gametime.ElapsedGameTime;
				currentPosTime++;
				currentPosTime = currentPosTime % numberOfTimes; 
			}
			GameTime newGameTime = new GameTime(gametime.TotalGameTime, GetAverageTimeSpan());
			gametime = newGameTime;
			GameAction action = GameAction.Nothing;
			inputhandler.Update(gametime,game.Player.Pos);
			EGameState tempState;
			//options.InitObjectHolder.particleManagers[cur_state].Update(gametime);
			pthreadManager.Update(gametime, options.InitObjectHolder.particleManagers[cur_state]);
			switch (cur_state)
			{ 
				case EGameState.Intro:
					break;
				case EGameState.Game:
					tempState = game.Update(gametime,inputhandler,options);
					CheckGameStateChange(tempState);
					if (cur_state == EGameState.Menu)
					{
						menu.CurrentMenu = Star.Menu.CurrentMenu.MainMenu;
					}
					break;
				case EGameState.Loading:
					tempState = loadingscreen.Update(gametime, inputhandler, options);
					CheckGameStateChange(tempState);
					if (cur_state == EGameState.Menu)
					{
						menu.CurrentMenu = Star.Menu.CurrentMenu.MainMenu;
					}
					if(cur_state == EGameState.Game)
						inputhandler.Pos = game.Level.Startpos;
					break;
				case EGameState.Menu:
					tempState = menu.Update(gametime, inputhandler,options);
					CheckGameStateChange(tempState);
					if (cur_state == EGameState.Loading)
					{
						//game.LoadLevel(serviceprovider,device, menu.GetLevelName, menu.GetLevelType, options);
						loadingscreen.LoadLevel(game, serviceprovider, device, menu.GetLevelName, menu.GetLevelType, options);
						
						//cur_state = EGameState.Game;
						//game.Update(gametime, inputhandler);
					}
					break;
				case EGameState.Quit:
					action = GameAction.Exit;
					break;
				case EGameState.Credits:
					tempState = credits.Update(gametime, inputhandler, options);
					CheckGameStateChange(tempState);
					break;
			}

			music.Update(gametime);
			oldStateScale += (float)gametime.ElapsedGameTime.TotalSeconds;
			oldStateScale = MathHelper.Clamp(oldStateScale, 1, 1.3f);
			oldStateAlpha = 1 - ((oldStateScale - 1) / 0.3f);
			curStateScale += (float)gametime.ElapsedGameTime.TotalSeconds;
			curStateScale = MathHelper.Clamp(curStateScale, 0, 1);
			
			curStateMatrix = Matrix.CreateTranslation(new Vector3(options.ScreenWidth / 2 /curStateScale, options.ScreenHeight / 2 / curStateScale, 0)) *
				Matrix.CreateScale(curStateScale) *
				Matrix.CreateTranslation(new Vector3(-options.ScreenWidth / (2/curStateScale), -options.ScreenHeight / (2/curStateScale), 0));
			oldStateMatrix = Matrix.CreateTranslation(new Vector3(options.ScreenWidth / 2 /oldStateScale, options.ScreenHeight / 2 / oldStateScale, 0)) *
				Matrix.CreateScale(oldStateScale) *
				Matrix.CreateTranslation(new Vector3(-options.ScreenWidth / (2 / oldStateScale), -options.ScreenHeight / (2 / oldStateScale), 0));
			return action;
		}

		public void Draw(GameTime gametime, GraphicsDeviceManager graphics)
		{
			if (!drawnOldState)
			{
				graphics.GraphicsDevice.SetRenderTarget(oldStateTarget);
				graphics.GraphicsDevice.Clear(Color.Transparent);
				switch (oldState)
				{
					case EGameState.Intro:
						break;
					case EGameState.Game:
						game.Draw(gametime, spritebatch, graphics.GraphicsDevice);
						break;
					case EGameState.Loading:
						loadingscreen.Draw(gametime, spritebatch, graphics.GraphicsDevice);
						break;
					case EGameState.Menu:
						menu.Draw(gametime, spritebatch, graphics.GraphicsDevice);
						break;
					case EGameState.Quit:
						break;
					case EGameState.Credits:
						credits.Draw(gametime, spritebatch, graphics.GraphicsDevice);
						break;
				}
				drawnOldState = true;
			}
			try
			{
				graphics.GraphicsDevice.SetRenderTarget(currentTarget);
			}
			catch (Exception)
			{
				currentTarget = new RenderTarget2D(graphicsDeviceManager.GraphicsDevice,
					graphicsDeviceManager.GraphicsDevice.PresentationParameters.BackBufferWidth,
					graphicsDeviceManager.GraphicsDevice.PresentationParameters.BackBufferHeight,
					false,
					SurfaceFormat.Color,
					DepthFormat.None,
					graphicsDeviceManager.GraphicsDevice.PresentationParameters.MultiSampleCount,
					RenderTargetUsage.PreserveContents);
				graphics.GraphicsDevice.SetRenderTarget(currentTarget);
			}
			
			graphics.GraphicsDevice.Clear(Color.Black);
			switch (cur_state)
			{
				case EGameState.Intro:
					break;
				case EGameState.Game:
					game.Draw(gametime, spritebatch, graphics.GraphicsDevice);
					break;
				case EGameState.Loading:
					loadingscreen.Draw(gametime, spritebatch, graphics.GraphicsDevice);
					break;
				case EGameState.Menu:
					menu.Draw(gametime, spritebatch, graphics.GraphicsDevice);
					break;
				case EGameState.Quit:
					break;
				case EGameState.Credits:
					credits.Draw(gametime, spritebatch, graphics.GraphicsDevice);
					break;
			}
			graphics.GraphicsDevice.SetRenderTarget(null);
			graphics.GraphicsDevice.Clear(Color.Black);
			spritebatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied,null,null,null,null,curStateMatrix);
			spritebatch.Draw(currentTarget, curStateRectangle, Color.White);
			
			spritebatch.End();
			spritebatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, null, null, null, null, oldStateMatrix);

			spritebatch.Draw(oldStateTarget, oldStateRectangle, new Color(1f, 1f, 1f, oldStateAlpha));
			spritebatch.End();

			music.Draw(spritebatch);
		}

		private void CheckGameStateChange(EGameState tempState)
		{
			if (tempState != cur_state)
			{
				oldState = cur_state;
				cur_state = tempState;
				drawnOldState = false;
				oldStateRectangle = new Rectangle(0, 0, options.ScreenWidth, options.ScreenHeight);
				curStateRectangle = new Rectangle(0, 0, options.ScreenWidth, options.ScreenHeight);
				curStateScale = .7f;
				oldStateScale = 1f;
				if (cur_state == EGameState.Game)
				{
					music.GraphXPackChanged(game.Level.LevelVariables.Dictionary[Game.Level.LV.GraphXPack]);
					music.StartCategoryTransition(true);
					inputhandler = new Inputhandler(game.Player.Pos, options);
				}
				else if (cur_state == EGameState.Menu && oldState == EGameState.Game)
				{
					music.StartCategoryTransition(false);
				}
				else if (cur_state == EGameState.Credits)
				{
					credits.ResetCredits(options.Resolution);
				}
			}
		}

		public void Unload()
		{
			options.ToFile();
			game.Unload();
			loadingscreen.Dispose();
			menu.Dispose();
			foreach (EGameState curGameState in Enum.GetValues(typeof(EGameState)))
			{
				options.InitObjectHolder.particleManagers[curGameState].Dispose();
			}
			pthreadManager.Dispose();
			//menu.Unload();
		}

		#region IGraphicsChange Member

		public void GraphicsChanged()
		{
			
			options.ScreenWidth = (int)MathHelper.Clamp(graphicsDeviceManager.GraphicsDevice.PresentationParameters.BackBufferWidth,320,float.MaxValue);
			options.ScreenHeight = (int)MathHelper.Clamp(graphicsDeviceManager.GraphicsDevice.PresentationParameters.BackBufferHeight, 240, float.MaxValue);
			graphicsDeviceManager.PreferredBackBufferWidth = options.ScreenWidth;
			graphicsDeviceManager.PreferredBackBufferHeight = options.ScreenHeight;
			graphicsDeviceManager.ApplyChanges();
			PresentationParameters pp = graphicsDeviceManager.GraphicsDevice.PresentationParameters;
			currentTarget = new RenderTarget2D(
				graphicsDeviceManager.GraphicsDevice,
				pp.BackBufferWidth, pp.BackBufferHeight, false,
				SurfaceFormat.Color, DepthFormat.None, pp.MultiSampleCount, RenderTargetUsage.PreserveContents);
			oldStateTarget = new RenderTarget2D(
				graphicsDeviceManager.GraphicsDevice,
				pp.BackBufferWidth, pp.BackBufferHeight, false,
				SurfaceFormat.Color, DepthFormat.None, pp.MultiSampleCount, RenderTargetUsage.PreserveContents);
			drawnOldState = false;
			oldStateRectangle = new Rectangle(0, 0, pp.BackBufferWidth, pp.BackBufferHeight);

			curStateRectangle = new Rectangle(0, 0, pp.BackBufferWidth, pp.BackBufferHeight);
			GraphicsManager.GraphicsChange(graphicsDeviceManager.GraphicsDevice,options);
		}

		#endregion
	}
}
