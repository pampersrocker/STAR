using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Star.GameManagement;
using WinFormsGraphicsDevice;
using System.Windows.Forms;
using Star.Game;
using Star.Game.Level;
using System.Runtime.InteropServices;
using Star.Game.Enemy;
using StarEdit.Editor;
using StarEdit.EnemyEditor;
using Star.Graphics.Effects.PostProcessEffects;
using Microsoft.Xna.Framework.Input;
using Star.Game.Level.InteractiveObjects;
using System.Globalization;
using Star;


namespace StarEdit.LevelEditor
{
	//TODO IMPLEMENT Texture Size
	public delegate void DecoTextureChangedEventHandler(Texture2D tex);

	public delegate void DecoChangedEventHandler(LayerObject layerObject,DecorationsLayer layer);

	public delegate void RotationScaleChangedEventHandler(float rotation,bool rotate);

	public delegate void EnemyPlacedEventHandler(List<Enemy> enemies);

	public class LevelControl : GraphicsDeviceControl
	{
		[DllImport("user32.dll")]
		static extern int ShowCursor(bool bShow);

		public event DecoChangedEventHandler DecoAdded;

		public event DecoTextureChangedEventHandler DecoTextureChanged;

		public event RotationScaleChangedEventHandler DecoRotationChanged;

		public event EnemyPlacedEventHandler EnemyPlaced;

		#region Variables

		SpriteBatch spritebatch;
		SpriteFont arial;
		ContentManager editorcontent;
		ContentManager gamecontent;
		Level level;
		EnemyManager enemymanager;

		public EnemyManager Enemymanager
		{
			get { return enemymanager; }
		}
		SingleEnemyManager placeEnemy;

		#region Layer
		ParallaxLayer rearparallaxLayer;
		ParallaxLayer frontparallaxLayer;
		CloudLayer cloudlayer;
		DecoLayer reardecoLayer;
		DecoLayer frontDecoLayer;
		InteractiveObjectManager iObjectManager;
		#endregion

		Rectangle bg_rect;
		Texture2D bg_tex;
		Options options;
		Camera camera;
		bool moving = false;
		bool placing = false;
		bool focusclick = false;
		bool rotating;
		bool scaling;
		float rotationValue;
		Vector2 rotationOrgPos;
		Point oldmousepos;
		Point newmousepos;
		Point oldscreensize;
		Tile mousetile;
		Texture2D mouseTex;
		Vector2 mouseoffset = new Vector2();
		Vector2 FormLocation = new Vector2();
		TileType mousetiletype;

		#region BorderVariables
		Rectangle leftDeadEndRect;
		Rectangle topDeadEndRect;
		Rectangle rightDeadEndRect;
		Rectangle bottomDeadEndRect;
		Rectangle rightborder;
		Rectangle leftBorder;
		Rectangle topBorder;
		Rectangle bottomBorder;
		#endregion

		Vector2 rectangle_start;
		Vector2 rectangle_end;
		Rectangle recttool;
		Color recttoolcolor = new Color(0,0,255,(byte) 127);
		Texture2D blanktex;
		const int borderSize = 10;
		Color deadendcolor = new Color(200,0,0, (byte)100);
		Color borderColor = new Color(0,0,0, (byte)255);
		TileToolType tooltype = TileToolType.Single;
		Tool tool = Tool.Tiles;
		string selectedEnemy = "";
		//string currentDecoration;
		DecorationsLayer currentDecoLayer;
		LayerObject mouseDeco; //Deco Object for Mouse Deco Tool
		InteractiveObject iObject;
		string iObjectName;
		float[] iObjectParameters;

		//LayerFX Variables
		Dictionary<LayerFX, LayerFXData> layerData;
		//LayerFX currentLayer;
		//Colorize colorizeBackground;
		//Colorize colorizePost;
		ColorizeLUT colorizeBackground;
		ColorizeLUT colorizePost;
		//DecoVariables
		Vector2 origin;
		float rotation;
		float layerDepth;
		Microsoft.Xna.Framework.Rectangle rect;

		RenderTarget2D target;

		#endregion

		#region Attributes

		public LayerObject MouseDeco
		{
			get { return mouseDeco; }
		}

		public Level Level
		{
			get { return level; }
			set { level = value; }
		}

		public bool Moving
		{
			get { return moving; }
			set 
			{
				moving = value;
				oldmousepos = new Point(MousePosition.X,MousePosition.Y);
				newmousepos = new Point(MousePosition.X, MousePosition.Y);
			}
		}

		private Point GetMousePosition
		{
			get { return new Point(MousePosition.X, MousePosition.Y); }
		}

		#endregion

		#region GraphicsDeviceControl Members

		protected override void Initialize()
		{
			//Initilaize Layer Data
			layerData = new Dictionary<LayerFX, LayerFXData>();
			
			//Fill Dictionary with Default Values
			foreach (LayerFX layer in Enum.GetValues(typeof(LayerFX)))
			{
				layerData.Add(layer, LayerFXData.Default);
			}

			editorcontent = new ContentManager(Services, "StarEditData");
			gamecontent = new ContentManager(Services, "Data");
			blanktex = gamecontent.Load<Texture2D>("Stuff/Blank");
			
			recttool = new Rectangle();
			options = new Options();
			options.ScreenHeight = DisplayRectangle.Height;
			options.ScreenWidth = DisplayRectangle.Width;
			options.ScreenWidth = GraphicsDevice.PresentationParameters.BackBufferWidth;
			options.ScreenHeight = GraphicsDevice.PresentationParameters.BackBufferHeight;
			options.InitObjectHolder.graphics = GraphicsDevice;

			options.InitObjectHolder.serviceProvider = Services;
			oldscreensize = new Point(DisplayRectangle.Width, DisplayRectangle.Height);
			try
			{
				arial = editorcontent.Load<SpriteFont>("Arial");
				spritebatch = new SpriteBatch(GraphicsDevice);
			}
			catch (Exception e)
			{
				string error = e.Message;
			}
			level = new Level();
			level.LoadLevel(Services,GraphicsDevice,options);
			//level = new Level(Services);
			iObjectManager = new InteractiveObjectManager();
			iObjectManager.Initialize(Services, level.LevelVariables, GraphicsDevice, options);
			rearparallaxLayer = new ParallaxLayer();
			frontparallaxLayer = new ParallaxLayer();
			cloudlayer = new CloudLayer();
			reardecoLayer = new DecoLayer();
			frontDecoLayer = new DecoLayer();
			SetBGRect();
			camera = new Camera(DisplayRectangle.Width, DisplayRectangle.Height, Vector2.Zero, DisplayRectangle.Height / 600f);
			mousetile = new Tile(0, 0);
			mousetile.load_tile((int)TileType.Wall,null);
			mouseTex = editorcontent.Load<Texture2D>("mousetile");
			if (level.Tiles != null)
			{
				mousetile.LoadGrass();
			}
			SetBorders(1, 1);
			enemymanager = new EnemyManager(Services, new Options());
			try
			{
				placeEnemy = new SingleEnemyManager(Services);
				placeEnemy.LoadEnemy(selectedEnemy);
			}
			catch (Exception e)
			{
				FileManager.WriteInErrorLog(this, e.Message, e.GetType());
			}
			
			bg_tex = new Texture2D(GraphicsDevice, 1, 1);
			colorizePost = new ColorizeLUT();
			colorizePost.Initialize(Services,GraphicsDevice,new Options());
			colorizePost.Enabled = true;
			colorizePost.StartResetEffect();
			colorizePost.FxData = LayerFXData.Default;
			colorizeBackground = new ColorizeLUT();
			colorizeBackground.Initialize(Services, GraphicsDevice, new Options());
			colorizeBackground.Enabled = true;
			colorizeBackground.StartResetEffect();

			target = new RenderTarget2D(GraphicsDevice,
				GraphicsDevice.PresentationParameters.BackBufferWidth,
				GraphicsDevice.PresentationParameters.BackBufferHeight,
				false,
				SurfaceFormat.Color,
				DepthFormat.None,

				GraphicsDevice.PresentationParameters.MultiSampleCount,
				RenderTargetUsage.PreserveContents);
			
		}

		protected override void Draw(GameTime gameTime)
		{
			try
			{
				PresentationParameters pp = GraphicsDevice.PresentationParameters;
				if (target.Width != GraphicsDevice.PresentationParameters.BackBufferWidth ||
					target.Height != pp.BackBufferHeight)
					target = new RenderTarget2D(GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight);
				GraphicsDevice.SetRenderTarget(target);
				GraphicsDevice.Clear(Color.Black);
				colorizeBackground.ApplyParameters();
				spritebatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, null, null, null, colorizeBackground.Effect);
				spritebatch.Draw(bg_tex, bg_rect, Color.White);
				spritebatch.End();
				rearparallaxLayer.Draw(spritebatch);
				frontparallaxLayer.Draw(spritebatch);
				cloudlayer.Draw(spritebatch);
				reardecoLayer.Draw(spritebatch, camera.getMatrix);


				level.DrawLevel(spritebatch, camera.getMatrix, null);
				iObjectManager.Draw(spritebatch, camera.getMatrix);

				frontDecoLayer.Draw(spritebatch, camera.getMatrix);

				switch (tool)
				{
					case Tool.Enemies:
						placeEnemy.Draw(gameTime, spritebatch, camera.getMatrix);
						enemymanager.Draw(gameTime, spritebatch, camera.getMatrix, true);
						break;
					case Tool.Tiles:
						switch (tooltype)
						{
							case TileToolType.Single:
								DrawMouseTile();
								break;
							case TileToolType.Rectangle:
								DrawRectTool();

								break;
						}
						break;
					case Tool.Decoration:
						DrawDecoTool();
						break;
					case Tool.IObject:
						DrawIObject();
						break;
				}
				colorizePost.DrawPostProcess(spritebatch);
				DrawBorders();

				GraphicsDevice.SetRenderTarget(null);
				spritebatch.Begin();
				spritebatch.Draw(target, Vector2.Zero, Color.White);
				spritebatch.End();
			}
			catch
			{ }

		}

		protected override void Update(GameTime gameTime)
		{
			//if (Focused)
			{
				if(Keyboard.GetState().GetPressedKeys().Contains(Microsoft.Xna.Framework.Input.Keys.RightAlt) || (Keyboard.GetState().GetPressedKeys().Contains(Microsoft.Xna.Framework.Input.Keys.LeftAlt)))
					scaling = true;
				else
					scaling = false;
				Point size = new Point(DisplayRectangle.Width, DisplayRectangle.Height);
				if (oldscreensize != size)
				{
					rearparallaxLayer.ScreenSize = size;
					frontparallaxLayer.ScreenSize = size;
					oldscreensize = size;
					SetBGRect();
				}
				Vector2 difference = new Vector2();
				newmousepos = GetMousePosition;
				
				if (moving)
				{

					difference.X = -(1 / camera.Scale) * (newmousepos.X - oldmousepos.X);
					difference.Y = -(1 / camera.Scale) * (newmousepos.Y - oldmousepos.Y);
					camera.update(gameTime, difference, Vector2.Zero, true);
				}
				if (rotating)
				{
					
					Vector2 rotationDifference = new Vector2(newmousepos.X - oldmousepos.X,newmousepos.Y-oldmousepos.Y);
						rotationValue = rotationDifference.X / 50;
					DecoRotationChanged(rotationValue,rotating);
					//Mouse.SetPosition(oldmousepos.X, oldmousepos.Y);
				}
				options.ScreenWidth = DisplayRectangle.Width;
				options.ScreenHeight = DisplayRectangle.Height;
				Vector2 relativePosition = new Vector2(-camera.Position.X / (level.Tiles.GetLength(1) * Tile.TILE_SIZE), -camera.Position.Y / ((level.Tiles.GetLength(0) * Tile.TILE_SIZE) - options.ScreenHeight / 2));
				
				rearparallaxLayer.Update(gameTime, difference, options,relativePosition);
				frontparallaxLayer.Update(gameTime, difference, options,relativePosition);
				reardecoLayer.Update(gameTime, difference, options,relativePosition);
				frontDecoLayer.Update(gameTime, difference, options,relativePosition);
				cloudlayer.Update(gameTime, difference, options,relativePosition);
				if (iObject != null)
					iObject.Update(gameTime, Vector2.Zero, new Rectangle());
				if (iObjectManager != null)
					iObjectManager.Update(gameTime, null);
				switch (tool)
				{
					case Tool.Enemies:
						placeEnemy.Update(DetermineLevelMousePos());
						break;
					case Tool.Tiles:
						switch (tooltype)
						{
							case TileToolType.Single:
								UpdateMouseTile();
								break;
							case TileToolType.Rectangle:
								UpdateRectanglePlacing();
								break;
						}
						break;
					case Tool.Decoration:
						UpdateDecoTool();
						break;
					case Tool.IObject:
						UpdateIObject();
						break;
				}
				colorizeBackground.Update(gameTime, Vector2.Zero);
				colorizePost.Update(gameTime, Vector2.Zero);
				oldmousepos = newmousepos;
			}

		}

		#endregion

		#region Public Members

		public LevelControl()
		{
			DecoAdded += new DecoChangedEventHandler(LevelControl_DecoAdded);
			DecoTextureChanged += new DecoTextureChangedEventHandler(LevelControl_DecoTextureChanged);
			DecoRotationChanged += new RotationScaleChangedEventHandler(LevelControl_DecoRotationChanged);
			EnemyPlaced += new EnemyPlacedEventHandler(LevelControl_EnemyPlaced);
		}

		void LevelControl_EnemyPlaced(List<Enemy> enemy)
		{
			//throw new NotImplementedException();
		}

		void LevelControl_DecoRotationChanged(float rotation,bool rotate)
		{
			//throw new NotImplementedException();
		}

		void LevelControl_DecoTextureChanged(Texture2D tex)
		{
			//throw new NotImplementedException();
		}

		void LevelControl_DecoAdded(LayerObject layerObject, DecorationsLayer layer)
		{
			//throw new NotImplementedException();
		}

		public void SetBorders(int px,int py)
		{
			leftDeadEndRect = new Rectangle(-400000000, -200000000, 400000000, 400000000);
			topDeadEndRect = new Rectangle(0, -400000000, 400000000, 400000000);
			rightDeadEndRect = new Rectangle((px ) * Tile.TILE_SIZE, 0, 400000000, (py ) * Tile.TILE_SIZE);
			bottomDeadEndRect = new Rectangle(0, (py ) * Tile.TILE_SIZE, (px ) * Tile.TILE_SIZE, 400000000);
			leftBorder = new Rectangle(-borderSize, -borderSize, borderSize, rightDeadEndRect.Height+2*borderSize);
			topBorder = new Rectangle(0, -borderSize, bottomDeadEndRect.Width, borderSize);
			rightborder = rightDeadEndRect;
			rightborder.Y -= borderSize;
			rightborder.Width = borderSize;
			rightborder.Height +=2* borderSize;
			bottomBorder = bottomDeadEndRect;
			bottomBorder.Height = borderSize;
			bottomDeadEndRect.Width = 400000000;
		}

		public void SaveFile(string path)
		{
			level.LevelVariables.Dictionary[LV.Enemies] = enemymanager.GetDataString();
			level.LevelVariables.Dictionary[LV.RearDecorationsLayerData] = reardecoLayer.GetItemDataString();
			level.LevelVariables.Dictionary[LV.FrontDecorationsLayerData] = frontDecoLayer.GetItemDataString();
			level.LevelVariables.Dictionary[LV.InteractiveObjects] = iObjectManager.GetDataString();
			FileManager.WriteFile(FileManager.ConvertLevelToString(level), path);
		}

		public void ReloadTextures()
		{
			rearparallaxLayer = new ParallaxLayer();
			frontparallaxLayer = new ParallaxLayer();
			cloudlayer = new CloudLayer();

			Level.reLoadTextures();
			InitializeLayer(options);
		}

		public void LoadLevel(string levelPath)
		{
			SetBGRect();
			level = new Level();
			level.LoadLevel(Services,GraphicsDevice,options, levelPath);
			//level = new Level(Services, levelPath);
			SetBorders(level.Tiles.GetLength(1), level.Tiles.GetLength(0));
			camera = new Camera(DisplayRectangle.Width, DisplayRectangle.Height, level.Startpos, DisplayRectangle.Height / 1200f);
			mouseoffset = new Vector2(DisplayRectangle.Width/2, DisplayRectangle.Height/2);
			InitializeLayer(options);
			LoadEnemies();
			iObjectManager.Dispose();
			iObjectManager = new InteractiveObjectManager();
			iObjectManager.Initialize(Services, level.LevelVariables, GraphicsDevice, options);
			bg_tex = gamecontent.Load<Texture2D>(GameConstants.GraphXPacksPath + level.LevelVariables.Dictionary[LV.GraphXPack] + "/Backgrounds/" + level.LevelVariables.Dictionary[LV.BackgroundImg]);
		}

		public void NewLevel(int x, int y)
		{
			SetBGRect();
			level = new Level();
			level.LoadLevel(Services,GraphicsDevice,options, x, y);
			//level = new Level(Services,x,y);

			SetBorders(x, y);
			camera = new Camera(DisplayRectangle.Width, DisplayRectangle.Height, new Vector2(DisplayRectangle.Width*1.5f, y * Tile.TILE_SIZE - DisplayRectangle.Height ), DisplayRectangle.Height / 2000f);
			mouseoffset = new Vector2(DisplayRectangle.Width / 2, DisplayRectangle.Height / 2);
			InitializeLayer(options);
			bg_tex = gamecontent.Load<Texture2D>(GameConstants.GraphXPacksPath + level.LevelVariables.Dictionary[LV.GraphXPack] + "/Backgrounds/" + level.LevelVariables.Dictionary[LV.BackgroundImg]);
		}

		#endregion

		#region Private Members

		private void InitializeLayer(Options pOptions)
		{
			pOptions.InitObjectHolder.dataHolder.PutData(Layer.Data_Layer.NumLayerObjects.GetKey(), 4);
			cloudlayer = new CloudLayer();
			cloudlayer.Initialize(pOptions, level.LevelVariables);
			frontparallaxLayer = new ParallaxLayer();
			frontparallaxLayer.GraphXName = level.LevelVariables.Dictionary[LV.FrontParallaxLayerImage];
			pOptions.InitObjectHolder.dataHolder.PutData(Layer.Data_Layer.NumLayerObjects.GetKey(), 2);
			frontparallaxLayer.Initialize(pOptions,level.LevelVariables);
			frontparallaxLayer.SpeedDivider = float.Parse(level.LevelVariables.Dictionary[LV.FrontParallaxLayerSpeedDivider]);
			rearparallaxLayer = new ParallaxLayer();
			rearparallaxLayer.GraphXName = level.LevelVariables.Dictionary[LV.RearParallaxLayerImage];
			rearparallaxLayer.Initialize(pOptions,level.LevelVariables);
			rearparallaxLayer.SpeedDivider = float.Parse(level.LevelVariables.Dictionary[LV.RearParallaxLayerSpeedDivider]);
			reardecoLayer = new DecoLayer();
			reardecoLayer.LayerName = DecorationsLayer.Rear.ToString();

			pOptions.InitObjectHolder.dataHolder.PutData(Layer.Data_Layer.NumLayerObjects.GetKey(), 1);
			reardecoLayer.Initialize(pOptions,level.LevelVariables);
			frontDecoLayer = new DecoLayer();
			frontDecoLayer.LayerName = DecorationsLayer.Front.ToString();
			frontDecoLayer.Initialize(pOptions,level.LevelVariables);

		}

		private void SetBGRect()
		{
			bg_rect = new Rectangle(DisplayRectangle.X,
				DisplayRectangle.Y,
				Math.Max(DisplayRectangle.Width, 1),
				Math.Max(DisplayRectangle.Height, 1));
		}

		private void DrawRectTool()
		{
			if (placing)
			{
				//3_1
				//spritebatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState, camera.getMatrix);
				spritebatch.Begin(SpriteSortMode.Immediate,
					BlendState.AlphaBlend,
					null,
					null,
					null,
					null,
					camera.getMatrix);
				spritebatch.Draw(blanktex, recttool, recttoolcolor);
				spritebatch.End();
			}
		}

		private void DrawIObject()
		{
			iObject.Draw(spritebatch, camera.getMatrix);
		}

		private void DrawMouseTile()
		{
			//3_1
			//spritebatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState, camera.getMatrix);
			spritebatch.Begin(SpriteSortMode.Immediate,
				BlendState.AlphaBlend,
				null,
				null,
				null,
				null,
				camera.getMatrix); 
			spritebatch.Draw(level.Textures[(int)mousetile.TileType], mousetile.get_rect, Color.White);
			if(mousetile.TileType == TileType.Wall)
				foreach (Grass grass in mousetile.GetGrass)
				{
					if (grass.type != GrassType.Empty)
						spritebatch.Draw(level.Grass_Tex[(int)grass.type], grass.rect, Color.White);
				}
			spritebatch.Draw(mouseTex, mousetile.get_rect, Color.White);
			spritebatch.End();
		}

		private void DrawDecoTool()
		{
			if (mouseDeco != null)
			{
				//3_1
				//spritebatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState, camera.getMatrix);
				spritebatch.Begin(SpriteSortMode.Immediate,
					BlendState.AlphaBlend,
					null,
					null,
					null,
					null,
					camera.getMatrix); 
				spritebatch.Draw(mouseDeco.Texture, mouseDeco.ExtendedRectangle.TransformedRectangle, null, Color.White, mouseDeco.ExtendedRectangle.Rotation, mouseDeco.ExtendedRectangle.Origin, SpriteEffects.None, 0);
				spritebatch.End();
			}
		}

		private void DrawBorders()
		{
			//3_1
			//spritebatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState, camera.getMatrix);
			spritebatch.Begin(SpriteSortMode.Immediate,
				BlendState.NonPremultiplied,
				null,
				null,
				null,
				null,
				camera.getMatrix); 
			spritebatch.Draw(blanktex, leftDeadEndRect, deadendcolor);
			spritebatch.Draw(blanktex, topDeadEndRect, deadendcolor);
			spritebatch.Draw(blanktex, rightDeadEndRect, deadendcolor);
			spritebatch.Draw(blanktex, bottomDeadEndRect, deadendcolor);
			spritebatch.Draw(blanktex, rightborder, borderColor);
			spritebatch.Draw(blanktex, bottomBorder, borderColor);
			spritebatch.Draw(blanktex, leftBorder, borderColor);
			spritebatch.Draw(blanktex, topBorder, borderColor);
			spritebatch.End();
		}

		private void LoadEnemies()
		{
			if (!string.IsNullOrEmpty(level.LevelVariables.Dictionary[LV.Enemies]))
			{
				string[] enemies = level.LevelVariables.Dictionary[LV.Enemies].Split(':');
				for (int i = 0; i < enemies.Length; i++)
				{
					string[] data = enemies[i].Split(',');
					if (data.Length >= 4)
					{
						try
						{
							string name;
							int x, y;
							Vector2 startpos;
							Star.Game.Enemy.Enemy.StandardDirection startdirection;

							name = data[0];
							x = int.Parse(data[1]);
							y = int.Parse(data[2]);
							startpos = new Vector2(x, y);
							startdirection = (Star.Game.Enemy.Enemy.StandardDirection)int.Parse(data[3]);

							enemymanager.AddEnemy(name, startpos, startdirection, new Options());
						}
						catch (Exception e)
						{
							FileManager.WriteInErrorLog(
								this,
								"Failed To Load Enemy# " + i.ToString() + " (Name = " + data[0] + ").",
								e.GetType());
						}
					}
					else
						FileManager.WriteInErrorLog(
							this,
							"Enemy #" + i.ToString() + " data is corrupted.");
				}
			}

		}

		private Vector2 DetermineLevelMousePos()
		{
			Vector2 pos;
			pos = new Vector2(MousePosition.X - 5, MousePosition.Y - 45) / camera.Scale;
			pos -= FormLocation / camera.Scale;
			pos -= camera.Position;
			pos -= camera.Offset;
			pos -= mouseoffset / camera.Scale;
			pos.X = (int)pos.X;
			pos.Y = (int)pos.Y;
			return pos;
		}

		private void UpdateRectanglePlacing()
		{
			rectangle_end = DetermineLevelMousePos();
			//rectangle_end.Y -= 70f / camera.Scale ;
			//rectangle_end = -rectangle_end;
			int height, width;
			height = (int)(rectangle_end.Y - rectangle_start.Y);
			width = (int)(rectangle_end.X - rectangle_start.X);
			//if (height < 0)
			//{
			//    rectangle_start.Y -= height;
			//    height = -height;
			//}
			//if (width < 0)
			//{
			//    rectangle_start.X -= width;
			//    width = -width;
			//}
			recttool = new Rectangle((int)rectangle_start.X, (int)rectangle_start.Y, width, height);
		}

		private void PlaceRectangle()
		{
			foreach (Tile tile in level.Tiles)
			{
				if (tile.get_rect.Intersects(recttool))
				{
					tile.load_tile((int)mousetiletype,null);
				}
			}

			foreach (Tile tile in level.Tiles)
			{
				tile.loadGrass(level.Tiles);
			}
		}

		private void UpdateMouseTile()
		{
			Vector2 tilepos = new Vector2();
			tilepos += new Vector2(MousePosition.X, MousePosition.Y) / camera.Scale;
			tilepos -= FormLocation / camera.Scale;
			tilepos -= camera.Position;
			tilepos -= camera.Offset;
			tilepos -= mouseoffset / camera.Scale;
			int tx = (int)(tilepos.X / 32 - 0.5f);
			int ty = (int)((tilepos.Y / 32)) - (int)(1 / camera.Scale);

			mousetile = new Tile(tx, ty);
			if (tx >= 0 && ty >= 0 && tx < level.Tiles.GetLength(1) && ty < level.Tiles.GetLength(0))
			{
				mousetile.load_tile((int)mousetiletype,null);
				if (level.Tiles != null && mousetile.TileType == TileType.Wall)
				{
					mousetile.LoadGrass();
				}
			}
			else
			{
				mousetile.load_tile((int)TileType.Error,null);
			}
			if (placing == true)
			{
				PlaceMouseTile();
			}
		}

		private void UpdateIObject()
		{
			if (iObject != null)
			{
				iObject.Pos = DetermineLevelMousePos();
			}
		}

		private void PlaceIObject()
		{
			iObjectManager.AddObject(Services, GraphicsDevice, options, iObjectName, iObjectParameters, DetermineLevelMousePos() - new Vector2(16));
		}

		private void UpdateDecoTool()
		{
			if (mouseDeco != null)
			{
				if (!rotating)
					rotationOrgPos = DetermineLevelMousePos();
				ExtendedRectangle extRect = ExtendedRectangle.Transform(
					rect,
					new Vector2(mouseDeco.Texture.Width, mouseDeco.Texture.Height),
					origin,
					rotationOrgPos,
					rotation);
				mouseDeco.ExtendedRectangle = extRect;


			}
		}

		private void PlaceDeco()
		{
			
			LayerObject newDeco = new LayerObject(mouseDeco.Texture, ExtendedRectangle.Transform(
						mouseDeco.ExtendedRectangle.OrgRectangle,
						new Vector2(mouseDeco.Texture.Width,mouseDeco.Texture.Height),
						mouseDeco.ExtendedRectangle.Origin,
						DetermineLevelMousePos(),
						mouseDeco.ExtendedRectangle.Rotation));
			switch (currentDecoLayer)
			{ 
				case DecorationsLayer.Front:
					
					frontDecoLayer.AddItem(newDeco);
					break;
				case DecorationsLayer.Rear:
					reardecoLayer.AddItem(newDeco);
					break;
			}
			DecoAdded(newDeco,currentDecoLayer);
			
		}

		private void PlaceMouseTile()
		{
			Tile[,] tiles;
			if (mousetile.TileType != TileType.Error)
			{
				tiles = level.Tiles;
				tiles[mousetile.TileCoord.Y, mousetile.TileCoord.X] = mousetile;
				for (int y = mousetile.TileCoord.Y - 5; y < mousetile.TileCoord.Y + 5; y++)
				{
					for (int x = mousetile.TileCoord.X - 5; x < mousetile.TileCoord.X + 5; x++)
					{
						if (y >= 0 && y < tiles.GetLength(0) && x >= 0 && x < tiles.GetLength(1))
						{
							if (y >= 1 && y < tiles.GetLength(0) - 1)
							{
								tiles[y - 1, x].loadGrass(tiles);
								tiles[y + 1, x].loadGrass(tiles);
							}
							if (x >= 1 && x < tiles.GetLength(1) - 1)
							{
								tiles[y, x - 1].loadGrass(tiles);
								tiles[y, x + 1].loadGrass(tiles);
							}
						}
					}
				}
			}
		}

		#endregion

		#region Protected Members
		protected override void Dispose(bool disposing)
		{
			//Cleanup is Required -> Destroying Threads, otherwise StarEdit.exe won't close
			if (iObject != null)
				iObject.Dispose();
			iObjectManager.Dispose();
			enemymanager.Dispose();
			placeEnemy.Enemy.Dispose();
			level.Dispose();
			reardecoLayer.Dispose();
			frontDecoLayer.Dispose();
			rearparallaxLayer.Dispose();
			frontparallaxLayer.Dispose();

			base.Dispose(disposing);
		}

		protected override void OnMouseWheel(MouseEventArgs e)
		{
			if (!scaling)
			{
				camera.Scale += 0.2f * camera.Scale * MathHelper.Clamp((e.Delta), -1, 1);
				camera.Scale = MathHelper.Clamp(camera.Scale, 0.1f, 10);
			}
			else
				DecoRotationChanged(e.Delta > 0 ? 1.1f:0.9f, false);
			//base.OnMouseWheel(e);
			
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			//ShowCursor(false);
			if (Focused)
			{
				switch (e.Button)
				{
					case MouseButtons.Right:
						Moving = true;
						break;
					case MouseButtons.Left:
						switch (tooltype)
						{
							case TileToolType.Single:
								if (focusclick == false)
								{
									PlaceMouseTile();
									placing = true;
								}
								break;
							case TileToolType.Rectangle:
								placing = true;
								rectangle_start = DetermineLevelMousePos();
								//rectangle_start.Y -= 70f / camera.Scale;
								break;
						}
						break;
				}
			}
			else
			{
				Focus();

			}
			switch (e.Button)
			{
				case MouseButtons.Middle:
					if (tool == Tool.Decoration)
							rotating = true;
					break;
			}
			

			base.OnMouseDown(e);
		}

		protected override void OnMouseClick(MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				if (tool == Tool.Enemies)
				{
					enemymanager.AddEnemy(placeEnemy.Type, placeEnemy.Position, placeEnemy.StandardDirection, new Options());
					EnemyPlaced(enemymanager.Enemies);
				}
				else if (tool == Tool.Decoration)
				{
					PlaceDeco();
				}
				else if (tool == Tool.IObject)
					PlaceIObject();
			}
			base.OnMouseClick(e);
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			if(tooltype == TileToolType.Single)
				ShowCursor(true);
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			Focus();
			if(tooltype == TileToolType.Single && tool == Tool.Tiles)
				ShowCursor(false);
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			focusclick = false;
			switch (e.Button)
			{
				case MouseButtons.Right:
					Moving = false;
					break;
				case MouseButtons.Left:
					placing = false;
					if (tooltype == TileToolType.Rectangle)
					{
						PlaceRectangle();
					}
					break;
				case MouseButtons.Middle:
					if (tool == Tool.Decoration)
						rotating = false;
					break;
			}
			
			base.OnMouseUp(e);
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
		}

		#endregion

		#region FormSetters

		public void SetToolType(TileToolType type)
		{
			tooltype = type;
		}

		public void SelectedEnemyChanged(string enemy)
		{
			if (placeEnemy != null)
				placeEnemy.LoadEnemy(enemy);
			else
				selectedEnemy = enemy;
		}

		public void ToolChanged(Tool tool)
		{
			this.tool = tool;
		}

		public void SetDecorationType(string texture)
		{
			Texture2D tex;
			try
			{
				tex = gamecontent.Load<Texture2D>(
				GameConstants.GraphXPacksPath +
				level.LevelVariables.Dictionary[LV.GraphXPack] +
				GameConstants.DECORATIONS_PATH + "\\" + texture);
				tex.Name = texture;
			}
			catch (Exception e)
			{
				FileManager.WriteInErrorLog(this, e.Message, e.GetType());
				tex = gamecontent.Load<Texture2D>(
				"Stuff\\Blank");
			}
			ExtendedRectangle extRect = ExtendedRectangle.Transform(
				new Rectangle(0, 0, tex.Width, tex.Height), 
				new Vector2(tex.Width, tex.Height), 
				null, 
				DetermineLevelMousePos(), 
				0);
			mouseDeco = new LayerObject(tex, extRect);
			DecoTextureChanged(tex);
		}

		public void SetDecorationLayer(DecorationsLayer layer)
		{
			currentDecoLayer = layer;
			
		}

		public void SetFormLocation(System.Drawing.Point location)
		{
			FormLocation = new Vector2(location.X, location.Y);
		}

		public void SetMouseTileType(string indicator)
		{
			foreach (TileType type in Enum.GetValues(typeof(TileType)))
			{
				if (indicator == type.ToString())
				{
					mousetiletype = type;
					break;
				}

			}
		}

		public void LayerDataChanged(LayerFX layer,LayerFXData data)
		{
			layerData[layer] = data;
			switch (layer)
			{ 
				case LayerFX.BackgroundFX:
					colorizeBackground.FxData = data;
					break;
				case LayerFX.CloudsFX:
					cloudlayer.FXData = data;
					break;
				case LayerFX.FrontParallaxFX:
					frontparallaxLayer.FXData = data;
					break;
				case LayerFX.LevelFX:
					level.FXData = data;
					reardecoLayer.FXData = data;
					frontDecoLayer.FXData = data;
					break;
				case LayerFX.PostFX:
					colorizePost.FxData = data;
					break;
				case LayerFX.RearParallaxFX:
					rearparallaxLayer.FXData = data;
					break;
			}
			
		}

		public void RemoveItem(int index, DecorationsLayer layer)
		{
			switch (layer)
			{ 
				case DecorationsLayer.Front:
					frontDecoLayer.RemoveItem(index);
					break;
				case DecorationsLayer.Rear:
					reardecoLayer.RemoveItem(index);
					break;
			}
		}

		public void RemoveItem(LayerObject layerObject, DecorationsLayer layer)
		{
			switch (layer)
			{
				case DecorationsLayer.Front:
					frontDecoLayer.RemoveItem(layerObject);
					break;
				case DecorationsLayer.Rear:
					reardecoLayer.RemoveItem(layerObject);
					break;
			}
		}

		public void DecoObjectDataChanged(Microsoft.Xna.Framework.Rectangle orgRect, Vector2 origin, float rotation, float layerDepth)
		{
			this.rect = orgRect;
			this.origin = origin;
			this.rotation = rotation;
			this.layerDepth = layerDepth;
		}

		public void IObjectDataChanged(string iObject, float[] iObjectParameters)
		{
			CultureInfo info = CultureInfo.CreateSpecificCulture("en-us");
			iObjectName = iObject;
			this.iObjectParameters = iObjectParameters;
			switch (iObject)
			{ 
				case "JumpPad":
					Vector2 force = new Vector2(iObjectParameters[1],0).Rotate(iObjectParameters[0],true);
					if (this.iObject != null)
						this.iObject.Dispose();
					this.iObject = new JumpPad();
					this.iObject.Initialize(Services, options, GraphicsDevice,
						"0,0,32,32," +
						force.X.ToString(info) + "," +
						force.Y.ToString(info) + "," + 
						iObjectParameters[2].ToString(info));
				break;
					
			}
		}

		#endregion

	}
}
