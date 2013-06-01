using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Star.Menu;
using Star.GameManagement;
using Star.Graphics.Effects.PostProcessEffects;
using AStarPathFinding;


namespace Star.Game.Level
{
	public delegate void LevelLoadLineEnhancedEventHandler(int cur_line,int max_line);

	/// <summary>
	/// Contains the Complete LevelData
	/// </summary>
	public class Level : IDisposable
	{
		public event LevelLoadLineEnhancedEventHandler LevelLoadLineEnhanced;

		ContentManager content;
		bool successfull = false;
		readonly int AMOUNT_TILE_TYPES = Enum.GetValues(typeof(TileType)).GetLength(0);
		public int width;
		Tile[,] tiles;
		List<string> error_messages;
		//Texture2D bg_tex;
		//Rectangle bg_rect;
		Vector2 startpos;
		Texture2D[] textures;
		Texture2D[] grass_tex;
		Quadtree quadtree;
		Effect alphablur;
		LevelVariables levelvariables;
		ColorizeLUT colorize;
		LayerFXData fxData;
		Texture2D nodeTex;



		#region Attributes

		public LayerFXData FXData
		{
			get { return fxData; }
			set 
			{ 
				fxData = value;
				ApplyFXData(fxData);
			}
		}

		public Vector2 Startpos
		{
			get { return startpos; }
		}

		public Texture2D[] Textures
		{
			get { return textures; }
		}

		public Texture2D[] Grass_Tex
		{
			get { return grass_tex; }
		}

		public bool Level_Loaded
		{
			get { return successfull; }
		}

		public List<string> Error_Message
		{
			get { return error_messages; }
		}

		public Tile[,] Tiles
		{
			get { return tiles; }
			set { tiles = value; }
		}

		//public Texture2D GetBGTex
		//{
		//    get { return bg_tex; }
		//}

		public Quadtree GetQuadtree
		{
			get { return quadtree; }
		}

		public LevelVariables LevelVariables
		{
			get { return levelvariables; }
			set { levelvariables = value; }
		}

		#endregion

		/// <summary>
		/// Creates a new Custom or Story Level
		/// </summary>
		/// <param name="serviceProvider">For the Content</param>
		/// <param name="level_name">The Level Name which will be loaded</param>
		/// <param name="leveltype">Custom or Story Level</param>
		public void LoadLevel(IServiceProvider serviceProvider,GraphicsDevice graphicsDevice,Options options, string level_name, LevelType leveltype)
		{

			content = new ContentManager(serviceProvider);
			content.RootDirectory = "Data";
			textures = new Texture2D[AMOUNT_TILE_TYPES];
			successfull = LoadCustomOrStoryLevel(level_name, leveltype);
			error_messages = new List<string>();
			InitializeEffects(serviceProvider, graphicsDevice, options);
			reLoadTextures();
		}

		/// <summary>
		/// Load a Level from a Path (Normally used by the Editor)
		/// </summary>
		/// <param name="serviceProvider">For the Content</param>
		/// <param name="levelPath">The Path to the LevelFile</param>
		public void LoadLevel(IServiceProvider serviceProvider, GraphicsDevice graphicsDevice, Options options, string levelPath)
		{
			content = new ContentManager(serviceProvider);
			content.RootDirectory = "Data";
			textures = new Texture2D[AMOUNT_TILE_TYPES];
			successfull = LoadLevelPath(levelPath);
			error_messages = new List<string>();
			InitializeEffects(serviceProvider, graphicsDevice, options);
			reLoadTextures();
		}

		/// <summary>
		/// Generates an Empty Level with default Values
		/// </summary>
		/// <param name="serviceProvider">For The Content</param>
		public void LoadLevel(IServiceProvider serviceProvider, GraphicsDevice graphicsDevice, Options options)
		{
			content = new ContentManager(serviceProvider);
			content.RootDirectory = "Data";
			textures = new Texture2D[AMOUNT_TILE_TYPES];
			startpos = Vector2.Zero;
			levelvariables = new LevelVariables();
			error_messages = new List<string>();
			tiles = new Tile[1, 1];
			tiles[0, 0] = new Tile(0, 0);
			tiles[0, 0].load_tile((int)TileType.Empty,null);
			InitializeEffects(serviceProvider, graphicsDevice, options);
			reLoadTextures();
		}

		/// <summary>
		/// Generates an new default Level with px rows an py coloumns
		/// </summary>
		/// <param name="serviceProvider"></param>
		/// <param name="px"></param>
		/// <param name="py"></param>
		public void LoadLevel(IServiceProvider serviceProvider, GraphicsDevice graphicsDevice, Options options, int px, int py)
		{
			content = new ContentManager(serviceProvider);
			content.RootDirectory = "Data";
			textures = new Texture2D[AMOUNT_TILE_TYPES];
			startpos = Vector2.Zero;
			levelvariables = new LevelVariables();
			error_messages = new List<string>();
			tiles = new Tile[py, px];

			for (int x = 0; x < px; x++)
			{
				for (int y = 0; y < py; y++)
				{
					tiles[y, x] = new Tile(x, y);
					if (y - 1 >= 0)
						tiles[y, x].load_tile(((int)TileType.Empty), tiles[y - 1, x]);
					else
						tiles[y, x].load_tile(((int)TileType.Empty), null);
				}
			}
			InitializeEffects(serviceProvider, graphicsDevice, options);
			reLoadTextures();
		}

		

		/// <summary>
		/// Loads the Textures of the current GraphXPack specifies in the LevelVariables
		/// </summary>
		public void reLoadTextures()
		{
			//Load Every Tile Tex
			foreach (TileType type in Enum.GetValues(typeof(TileType)))
			{
				textures[(int)type] = content.Load<Texture2D>(GameConstants.GraphXPacksPath + levelvariables.Dictionary[LV.GraphXPack] + "/Level/" + type.ToString());
			}
			alphablur = content.Load<Effect>("Stuff/Effects/AlphaBlur");
			//Initalize an load GrassTextures
			grass_tex = new Texture2D[4];
			grass_tex[(int)GrassType.Left] = content.Load<Texture2D>(GameConstants.GraphXPacksPath + levelvariables.Dictionary[LV.GraphXPack] + "/Level/Grass/" + GrassType.Left.ToString());
			grass_tex[(int)GrassType.Right] = content.Load<Texture2D>(GameConstants.GraphXPacksPath + levelvariables.Dictionary[LV.GraphXPack] + "/Level/Grass/" + GrassType.Right.ToString());
			grass_tex[(int)GrassType.Top] = content.Load<Texture2D>(GameConstants.GraphXPacksPath + levelvariables.Dictionary[LV.GraphXPack] + "/Level/Grass/" + GrassType.Top.ToString());
			grass_tex[(int)GrassType.Bottom] = content.Load<Texture2D>(GameConstants.GraphXPacksPath + levelvariables.Dictionary[LV.GraphXPack] + "/Level/Grass/" + GrassType.Bottom.ToString());
			nodeTex = content.Load<Texture2D>("Stuff/Node");
		}

		/// <summary>
		/// Reads Data out of a Custom or Story Level
		/// </summary>
		/// <param name="level_name">The LevelName</param>
		/// <param name="leveltype">Custom or Story level</param>
		/// <returns>If it was successfull</returns>
		bool LoadCustomOrStoryLevel(string level_name,LevelType leveltype)
		{
			bool level_loaded = false;
			error_messages = new List<string>();
			string daten;
			
			//StreamReader stream = new StreamReader("Data/Levels/"+leveltype.ToString()+"/" + level_name + ".map");
			//daten = stream.ReadToEnd();
			//stream.Close();
			daten = FileManager.ReadFile("Data/Levels/" + leveltype.ToString() + "/" + level_name + ".map");
			level_loaded = LoadLevel(daten);

			return level_loaded;
		}

		private void ApplyFXData(LayerFXData data)
		{
			colorize.FxData = data;
			//if (colorize.Colorization != ColorizationFlags.None)
			//    colorize.Enabled = true;
			colorize.Enabled = true;
			//else
			//    colorize.Enabled = false;
		}

		private void InitializeEffects(IServiceProvider serviceProvider,GraphicsDevice graphicsDevice,Options options)
		{
			colorize = new ColorizeLUT();
			colorize.Initialize(serviceProvider, graphicsDevice, options);
			colorize.FxData = LayerFXData.FromString(levelvariables.Dictionary[LV.LevelFX]);
			//colorize.StartResetEffect();
			colorize.Enabled = true;
		}

		/// <summary>
		/// Loads a level from a Path
		/// </summary>
		/// <param name="levelpath">The Filename</param>
		/// <returns>If it was Successfull</returns>
		bool LoadLevelPath(string levelpath)
		{
			bool level_loaded = false;
			error_messages = new List<string>();
			string daten;

			//StreamReader stream = new StreamReader(levelpath);
			//daten = stream.ReadToEnd();
			//stream.Close();

			//Read in File
			daten = FileManager.ReadFile(levelpath);

			level_loaded = LoadLevel(daten);

			return level_loaded;
		}

		/// <summary>
		/// Initializes a Level
		/// </summary>
		/// <param name="daten">The Content of the LevelFile</param>
		/// <returns>if it was successfull</returns>
		private bool LoadLevel(string daten)
		{
			bool level_loaded = false;
			//try
			{
		
				Dictionary<string, string> config = new Dictionary<string, string>();
				//Split Lines
				string[] zeilen = daten.Split(new char[] { ';' });
				foreach (string zeile in zeilen)
				{
					//Splite Key and Value
					string[] temp = zeile.Split(new char[] { '=' });
					if (temp.Length > 1)
					{
						string config_name = temp[0];
						string value = temp[1];

						config_name = config_name.Trim();
						value = value.Trim();

						config[config_name] = value;
					}
				}

				try
				{
					//Load the LevelTiles
					load_level_tiles(config["Level"]);
				}
				catch (Exception ex)
				{
					error_messages.Add(ex.Message);
				}

				//Load LevelVariables
				levelvariables = new LevelVariables(daten,tiles);

				//Old Method für Loading Level (ByChar)
				#region CharLevel
			   /*
				string[] temp_level_lines = config["Level"].Split(new char[] { ':' });
				level_lines = new string[temp_level_lines.Count()];
				for (int i = 0; i < temp_level_lines.Count(); i++)
				{
					level_lines[i] = temp_level_lines[i].Trim();
				}

				int max_width = level_lines[0].Length;
				foreach (string lines in level_lines)
				{
					if (lines.Length > max_width)
					{
						max_width = lines.Length;
					}
				}
				tiles = new Tile[level_lines.Count(), max_width];

				for (int x = 0; x < level_lines.Count(); x++)
				{
					for (int y = 0; y < max_width; y++)
					{
						tiles[x, y] = new Tile(y, x);
						if (y >= level_lines[x].Length)
						{
							tiles[x, y].load_tile('E');
						}
						else
						{
							tiles[x, y].load_tile(level_lines[x][y]);
							if (tiles[x, y].get_TileType == TileType.Start)
							{
								startpos.X = tiles[x, y].get_rect.Location.X + Tile.TILE_SIZE / 2;
								startpos.Y = tiles[x, y].get_rect.Location.Y + Tile.TILE_SIZE / 2;
							}
						}
					}
				}
				*/
			   #endregion

				quadtree = new Quadtree(tiles, int.Parse(levelvariables.Dictionary[LV.QuadtreeLayerDepth]));
				level_loaded = true;
			}
			//catch (Exception e)
			{
			  //  error_messages.Add(e.Message);
			}

			return level_loaded; 
		}

		/// <summary>
		/// Loads the Level Tiles
		/// </summary>
		/// <param name="leveldata">the Content from Level = leveldata ;</param>
		/// <returns>if it was successfull</returns>
		private bool load_level_tiles(string leveldata)
		{
			bool successfull = true;

			try
			{
				//Split Lines
				string[] level_lines = leveldata.Split(':');
				int x_max = 0;
				int y_max = level_lines.Count();

				string[][] tiledata = new string[y_max][];

				//Split  each Line into Tiles
				for (int y = 0; y < y_max; y++)
				{
					tiledata[y] = level_lines[y].Split(',');
				}

				//Search for longest Line
				foreach (string[] temp in tiledata)
				{
					x_max = (int)MathHelper.Max(temp.Count(), x_max);
				}

				// Initializes the 2 dimensional Array
				tiles = new Tile[y_max, x_max];

				//Initialize every Tile
				for (int y = 0; y < y_max; y++)
				{
					//Invoke LevelLoadLineEnhanced Event
					if (LevelLoadLineEnhanced != null)
						if ((y + 1) % 20 == 0 || y + 1 == y_max)
						{
							LevelLoadLineEnhanced(y + 1, y_max);
							//System.Threading.Thread.Sleep(500);
						}
					for (int x = 0; x < x_max; x++)
					{
						//Set Tile Position
						tiles[y, x] = new Tile(x, y);

						//If this line is shorter than the longest Line,
						//then fill rest with Empty tiles
						if (x >= tiledata[y].Count() )
						{
							tiles[y, x].load_tile(((int)TileType.Empty),null);

						}
						else
						{
							//Load the Tile
							//FAIL
							bool parsing;
							int tileint;
							parsing = int.TryParse(tiledata[y][x],out tileint);
							if (parsing == true)
							{
								if(y-1>=0)
									tiles[y, x].load_tile(int.Parse(tiledata[y][x]), tiles[y - 1, x]);
								else
									tiles[y, x].load_tile(int.Parse(tiledata[y][x]),null);

								//Search for the Start of the Level
								if (tiles[y, x].TileType == TileType.Start)
								{
									startpos.X = tiles[y, x].get_rect.Location.X + Tile.TILE_SIZE / 2;
									startpos.Y = tiles[y, x].get_rect.Location.Y + Tile.TILE_SIZE / 2;
								}
							}
							else
							{
								
								tiles[y, x].load_tile((int)TileType.Empty,null);
							}
						}
					}
				}

				//If Tiles is wall then load grass
				foreach (Tile tile in tiles)
				{
					if (tile.TileType == TileType.Wall)
					{
						tile.loadGrass(tiles);
					}
				}
				AStarMap map = new AStarMap();
				map.InitializeMapFromLevel(tiles);
			}
			catch (Exception ex)
			{
				successfull = false;
				throw new Exception(ex.Message);
			}


			return successfull;
		}

		/// <summary>
		/// Draw The Level
		/// </summary>
		/// <param name="spritebatch">the SpriteBatch on which it will be drawn</param>
		/// <param name="translation">The Cameramatrix</param>
		public void DrawLevel(SpriteBatch spritebatch,Matrix translation,Rectangle? display)
		{
			//3_1
			//spritebatch.Begin(SpriteBlendMode.AlphaBlend,SpriteSortMode.Immediate,SaveStateMode.SaveState,translation);
			colorize.ApplyParameters();
			spritebatch.Begin(SpriteSortMode.Deferred,
				BlendState.NonPremultiplied,
				null, null, null, 
				colorize.Effect,
				translation);
			//Draw Tiles themselfes
			if (display.HasValue)
			{
				List<Tile> myTiles = quadtree.GetCollision(display.Value);
				for (int i = 0; i < myTiles.Count; i++)
				{
					spritebatch.Draw(textures[(int)myTiles[i].TileType], myTiles[i].get_rect, Color.White);
					if (myTiles[i].TileType == TileType.Wall)
					{
						int length = myTiles[i].GetGrass.Length;
						for (int grass = 0; grass < length; grass++)
						{
							if (myTiles[i].GetGrass[grass].type != GrassType.Empty)
							{
								spritebatch.Draw(grass_tex[(int)myTiles[i].GetGrass[grass].type], myTiles[i].GetGrass[grass].rect, Color.White);
							}
						}
					}
				}
			}
			else
			{
				for (int i = 0; i < tiles.GetLength(0); i++)
				{
					for (int k = 0; k < tiles.GetLength(1); k++)
					{
						if (tiles[i, k].TileType != TileType.Empty)
						{
							spritebatch.Draw(textures[(int)tiles[i, k].TileType], tiles[i, k].get_rect, Color.White);
							if (tiles[i, k].TileType == TileType.Wall)
							{
								int length = tiles[i, k].GetGrass.Length;
								for (int grass = 0; grass < length; grass++)
								{
									if (tiles[i, k].GetGrass[grass].type != GrassType.Empty)
									{
										spritebatch.Draw(grass_tex[(int)tiles[i, k].GetGrass[grass].type], tiles[i, k].GetGrass[grass].rect, Color.White);
									}
								}
							}
						}
					}
				}
			}
			spritebatch.End();
		}

		public void Dispose()
		{
			Dispose(true);
		}

		/// <summary>
		/// Clears the Level, releasing all loaded Data
		/// </summary>
		/// <param name="disposing">Dummy</param>
		protected virtual void Dispose(bool disposing)
		{
			content.Unload();
			content.Dispose();
			foreach (Tile tile in tiles)
			{
				tile.Clear();  
			}
		}
		
	}
}
