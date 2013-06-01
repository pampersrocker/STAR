using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace AStarPathFinding
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	class Game1 : Microsoft.Xna.Framework.Game
	{
		const bool drawCosts =false;
		const int mapsize =250;
		const int size = 500/mapsize;
		const int percentage = 38;
		//const int yaim = mapsize - mapsize / 10 - 1;
		//const int xaim = mapsize - mapsize / 10 - 1;
		const int yaim = mapsize - mapsize / 10 - 1;
		const int xaim = mapsize - mapsize / 10 - 1;

		Node[,] mapnodes;

		GraphicsDeviceManager graphics;
		KeyboardState oldstate, state;
		SpriteBatch spriteBatch;
		Texture2D tile;
		SpriteFont arial;
		AStar pathfinder;
		bool pathfound;
		Path path;
		double time;
		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			graphics.PreferredBackBufferHeight = 768;
			graphics.PreferredBackBufferWidth = 1024;
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{

			base.Initialize();
			tile = Content.Load<Texture2D>("tile2");
			arial = Content.Load<SpriteFont>("arial");
			//StreamReader reader = new StreamReader("AStarProgram\\map.txt");
			//string data = reader.ReadToEnd();
			//string[] lines = data.Split('\n');
			//mapnodes = new Node[lines.Length, lines[0].Length - 1];
			//for (int y = 0; y < lines.Length; y++)
			//{
			//    for (int x = 0; x < lines[0].Length - 1; x++)
			//    {
			//        mapnodes[y, x] = new Node(x, y, size, lines[y][x] == '1' ? Walkable.Blocked : Walkable.Walkable);
			//    }
			//}
			////Random rand = new Random((int)DateTime.Now.Ticks);
			////mapnodes = new Node[mapsize,mapsize];
			////for (int y = 0; y < mapsize; y++)
			////{
			////    for (int x = 0; x < mapsize ; x++)
			////    {
			////        mapnodes[y, x] = new Node(x, y, size, rand.Next(0,100) <percentage ? Walkable.Blocked : Walkable.Walkable);
			////    }
			////}
			//AStarMap.InitializeMap(mapnodes);
			Reinizialize();
			oldstate = Keyboard.GetState();
			state = oldstate;
		}

		public void Reinizialize()
		{
			pathfound = false;
			if (pathfinder != null)
			{
				pathfinder.PathFound -= pathfinder_PathFound;
				pathfinder.Abort();
				pathfinder.Stop();
			}
			Random rand = new Random((int)DateTime.Now.Ticks);
			mapnodes = new Node[mapsize, mapsize];
			for (int y = 0; y < mapsize; y++)
			{
				for (int x = 0; x < mapsize; x++)
				{


					mapnodes[y, x] = new Node(x, y, size, rand.Next(0, 100) < percentage ? Walkable.Blocked : Walkable.Walkable);


				}
			}
			mapnodes[yaim, xaim] = new Node(xaim, yaim, size, Walkable.Walkable);
			mapnodes[5, 2] = new Node(2, 5,size, Walkable.Walkable);
			AStarMap.InitializeMap(mapnodes);
			pathfinder = new AStar();
			pathfinder.PathFound += new PathFoundEventHandler(pathfinder_PathFound);

			pathfinder.FindWay(mapnodes[5, 2], mapnodes[yaim, xaim]);
			
		}

		void pathfinder_PathFound(Path path,double ml)
		{
			this.path = path;
			pathfound = true;
			time = ml;
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
			pathfinder.Abort();
			pathfinder.Stop();

		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			// Allows the game to exit
			state = Keyboard.GetState();
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
				this.Exit();
			if (state.IsKeyDown(Keys.Space) && !oldstate.IsKeyDown(Keys.Space))
				pathfinder.Resume();
			if (state.IsKeyDown(Keys.Enter) && !oldstate.IsKeyDown(Keys.Enter))
				Reinizialize();

			oldstate = state;
			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			try
			{
				Color color;
				GraphicsDevice.Clear(Color.CornflowerBlue);
				spriteBatch.Begin();
				
				if (!pathfound)
					foreach (PathNode node in pathfinder.searchMap)
					{
						color = Color.White;
						if ((node.State == NodeState.Closed))
							color = Color.Red;
						else if (node.State == NodeState.Known)
							color = Color.Green;
						spriteBatch.Draw(tile, node.Rectangle, color);
					}

				foreach (Node node in AStarMap.Mapnodes)
				{
					color = Color.White;
					if (!(node.Walkable == Walkable.Blocked))
						spriteBatch.Draw(tile, node.Rectangle, color);
				}
				if (!pathfound)
				{
					foreach (PathNode node in pathfinder.searchMap)
						if (node.State == NodeState.Known)
						{
							spriteBatch.Draw(tile, node.Rectangle, Color.Yellow);
							//spriteBatch.DrawString(arial, node.FCost.ToString() + "\n" + node.FCostNoAim.ToString() + "\n" + node.HCost.ToString(), node.Rectangle.CenterToVector2(), Color.Blue);
						}
						else if (node.State == NodeState.Closed)
							spriteBatch.Draw(tile, node.Rectangle, Color.Red);

					foreach (Node node in mapnodes)
						if (node.Walkable != Walkable.Walkable)
							spriteBatch.Draw(tile, node.Rectangle, Color.Black);
				}
				if (drawCosts)
				{
					foreach (PathNode node in pathfinder.searchMap)
					{
						color = Color.Blue;
						if (node.FCostNoAim != -1)
							spriteBatch.DrawString(arial, "FNA:" + node.FCostNoAim + "\nF:" + node.FCost.ToString("0"), new Vector2(node.Rectangle.Location.X, node.Rectangle.Location.Y), color);
					} 
				}
					if (pathfound)
					{
					foreach(PathNode node in path)
					{
						spriteBatch.Draw(tile, node.Rectangle, Color.Green);
						spriteBatch.DrawString(arial, time.ToString(), new Vector2(0, 600 - 100), Color.Red);
					}
					
				}
				Vector2 fontpos = new Vector2(500,20);
				spriteBatch.End();
				spriteBatch.Begin();
				//spriteBatch.DrawString(arial, "Aim\n" + path.Cost, pathfinder.searchMap[yaim, xaim].Rectangle.CenterToVector2(), Color.Red);
				spriteBatch.DrawString(arial, "Green - Path", fontpos, Color.Green);
				fontpos.Y += arial.LineSpacing;
				spriteBatch.DrawString(arial, "White - Walkable, Unknown", fontpos, Color.White);
				fontpos.Y += arial.LineSpacing; 
				spriteBatch.DrawString(arial, "Black - Blocked", fontpos, Color.Black);
				fontpos.Y += arial.LineSpacing;
				spriteBatch.DrawString(arial, "Red - Walkable,Closed", fontpos, Color.Red);
				fontpos.Y += arial.LineSpacing;
				spriteBatch.DrawString(arial, "Yellow - Walkable,Known", fontpos, Color.Yellow);
				fontpos.Y += arial.LineSpacing;
				
				
				spriteBatch.End();
				
				base.Draw(gameTime);
			}
			catch 
			{
				spriteBatch.End();
			}
		}
	}
}
