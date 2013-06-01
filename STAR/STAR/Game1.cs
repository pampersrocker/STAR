using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using Star.Input;
using Star.GameManagement;
using Star.Game;
using Star.Game.Level;
using Star.Game.Debug;

namespace Star
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class Game1 : Microsoft.Xna.Framework.Game
	{
		int ScreenWidth =1366;
		int ScreenHeight = 768;
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		GameManager gamemanager;
		bool focused;

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			Window.AllowUserResizing = true;
			//3_1
			//Window.ClientSizeChanged += new EventHandler(Window_ClientSizeChanged);
			Window.ClientSizeChanged +=new EventHandler<EventArgs>(Window_ClientSizeChanged);
			Content.RootDirectory = "Data";
			graphics.PreferredBackBufferHeight = ScreenHeight;
			graphics.PreferredBackBufferWidth = ScreenWidth;
			//graphics.IsFullScreen = true;
			focused = true;
			graphics.PreferMultiSampling = true;
			//this.TargetElapsedTime = new TimeSpan(0,0,0,0,16);
			this.IsFixedTimeStep = false;
			gamemanager = new GameManager(EGameState.Menu, Content.ServiceProvider);
			this.Window.Title = "S.T.A.R. v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
		}

		void Window_ClientSizeChanged(object sender, EventArgs e)
		{
			gamemanager.GraphicsChanged();
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			DebugManager.Initialize();
			//3_1
			//GraphicsDevice.VertexDeclaration = new VertexDeclaration(GraphicsDevice, VertexPositionColor.VertexElements);
			//graphics.GraphicsDevice.PresentationParameters.EnableAutoDepthStencil = true;
			gamemanager.Initialize(graphics);
			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		protected override void UnloadContent()
		{
			gamemanager.Unload();
		}

		protected override void OnActivated(object sender, EventArgs args)
		{
			focused = true;
			base.OnActivated(sender, args);
		}

		protected override void OnDeactivated(object sender, EventArgs args)
		{
			focused = false;
			base.OnDeactivated(sender, args);
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			// Allows the game to exit
			//if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
			//{
			//    this.Exit();
			//}
			if (focused)
			{
				GameAction action = gamemanager.Update(gameTime, graphics.GraphicsDevice);
				if (action == GameAction.Exit)
				{
					gamemanager.Unload();
					this.Exit();
				}
			}
			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			gamemanager.Draw(gameTime, graphics);
			base.Draw(gameTime);
		}
	}
}
