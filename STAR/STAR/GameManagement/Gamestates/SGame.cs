using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Star.Game;
using Star.Game.Enemy;
using Star.Game.GameEvents;
using Star.Game.Level;
using Star.Game.Level.InteractiveObjects;
using Star.Graphics.Effects.PostProcessEffects;
using Star.Input;
using Star.Menu;
using XNAParticleSystem;

namespace Star.GameManagement.Gamestates
{

	public delegate void LevelLoadEnhancedEventHandler(byte percentage, string action);

	public delegate void LevelLoadedEventHandler();

	public class SGame : IGameState
	{
		public event LevelLoadedEventHandler LevelLoaded;

		public event LevelLoadEnhancedEventHandler LevelLoadEnhanced;



		Player player;
		Level level;
		Camera camera;

		public Player Player
		{
			get { return player; }
		}

		public Camera Camera
		{
			get { return camera; }
			set { camera = value; }
		}
		DebugScreen debugscreen;
		BasicEffect effect;
		RunDirection rundirection;
		CloudLayer cloudlayer;
		ParallaxLayer frontparallaxLayer;
		ParallaxLayer rearparallaxLayer;
		DecoLayer rearDecoLayer;
		DecoLayer frontDecoLayer;
		Texture2D bg_tex;
		Rectangle bg_rect;
		Effect waether;
		Effect motionblur;
		Effect blur;
		//Vector2 pos1 = new Vector2();
		//Vector2 pos2 = new Vector2();
		GameLogic gamelogic;
		//ResolveTexture2D resolvedtexture;
		RenderTarget2D target;
		RenderTarget2D basicTarget;
		Texture2D tex;
		EnemyManager enemyManager;
		InteractiveObjectManager iObjectManager;
		private ContentManager content;
		DateTime start;
		ToonShader toonShader;

		bool firstFrame = true;

		ColorizeLUT colorizeBackground;
		ColorizeLUT colorizePost;
		

		#region Properties
		public ContentManager Content
		{
			get { return this.content; }
		}

		public Level Level
		{
			get { return level; }
		}

		#endregion


		public SGame(IServiceProvider ServiceProvider)
		{
			//Filling LevelLoadEnhanced Event that is is not null
			LevelLoadEnhanced += new LevelLoadEnhancedEventHandler(SGame_LevelLoadEnhanced);
			content = new ContentManager(ServiceProvider);
			content.RootDirectory = "Data";
			enemyManager = new EnemyManager(ServiceProvider,new Options());
			start = DateTime.Now;
			iObjectManager = new InteractiveObjectManager();
		}

		void SGame_LevelLoadEnhanced(byte percentage, string action)
		{
			//this is just a dummy that the LevelLoadEnanced event is not null ;)
		}

		#region IGameState Member

		protected void Initialize(Options options,GraphicsDevice graphics)
		{
			basicTarget = new RenderTarget2D(graphics,
				graphics.PresentationParameters.BackBufferWidth,
				graphics.PresentationParameters.BackBufferHeight,
				false,
				SurfaceFormat.Color,
				DepthFormat.None,
				graphics.PresentationParameters.MultiSampleCount,
				RenderTargetUsage.PreserveContents);
			player = new Player(new Vector2(100, 100), Content);
			blur = content.Load<Effect>("Stuff/Effects/Blur");
			waether = content.Load<Effect>("Stuff/Effects/Weather");
			waether.CurrentTechnique = waether.Techniques["Technique1"];
			motionblur = content.Load<Effect>("Stuff/Effects/MotionBlur");
			waether.Parameters["effectTexture"].SetValue(content.Load<Texture2D>("Stuff/Effects/Rain"));
			level = new Level();
			level.LoadLevel(content.ServiceProvider,graphics,options);
			InitializeLayer(options,graphics);
			player.Pos = level.Startpos;
			bg_rect = new Rectangle(0, 0, options.ScreenWidth, options.ScreenHeight);

			gamelogic = new GameLogic();
			gamelogic.Initialize(content.ServiceProvider,graphics, level.LevelVariables, options);
			camera = new Camera(options.ScreenWidth, 
				options.ScreenHeight, 
				level.Startpos, 
				(float)options.ScaleFactor);
			debugscreen = new DebugScreen(Content, level.Startpos);
			Matrix viewMatrix = Matrix.CreateLookAt(
				   new Vector3(0.0f, 0.0f, 1.0f),
				   Vector3.Zero,
				   Vector3.Up
				   );

			Matrix projectionMatrix = Matrix.CreateOrthographicOffCenter(
				0,
				(float)options.ScreenWidth,
				(float)options.ScreenHeight,
				0,
				1.0f, 1000.0f);

			effect = new BasicEffect(graphics);
			effect.Alpha = 1.0f;
			effect.View = viewMatrix;
			effect.Projection = projectionMatrix;
			effect.VertexColorEnabled = true;

			colorizePost = new ColorizeLUT();
			colorizeBackground = new ColorizeLUT();
			JumpPad pad = new JumpPad();
			pad.Initialize(content.ServiceProvider, options, graphics, "0,0,32,32,0,-5000,0.95");
			iObjectManager.AddObject(pad);
			target = new RenderTarget2D(graphics,
				graphics.PresentationParameters.BackBufferWidth,
				graphics.PresentationParameters.BackBufferHeight,
				false,
				SurfaceFormat.Color,
				DepthFormat.None,
				graphics.PresentationParameters.MultiSampleCount,
				RenderTargetUsage.PreserveContents);

			toonShader = new ToonShader();
			toonShader.Initialize(Content.ServiceProvider, graphics, options);
			toonShader.Enabled = true;

		}

		public void LoadLevel(IServiceProvider ServiceProvider,GraphicsDevice device,string level_name, LevelType leveltype,Options options)
		{
			level.Dispose();
			level = new Level();
			level.LevelLoadLineEnhanced += new LevelLoadLineEnhancedEventHandler(level_LevelLoadLineEnhanced);
			level.LoadLevel(ServiceProvider,device,options, level_name, leveltype);
			//level = new Level(ServiceProvider, level_name, leveltype);
			GameParameters.CurrentGraphXPack = level.LevelVariables.Dictionary[LV.GraphXPack];
			colorizeBackground.Dispose();
			colorizeBackground = new ColorizeLUT();
			colorizeBackground.Initialize(ServiceProvider, device, options);
			colorizeBackground.Enabled = true;
			colorizeBackground.StartResetEffect();
			colorizeBackground.FxData = LayerFXData.FromString(level.LevelVariables.Dictionary[LV.BackgroundFX]);
			colorizePost.Dispose();
			colorizePost = new ColorizeLUT();
			colorizePost.Initialize(ServiceProvider, device, options);
			colorizePost.Enabled = true;
			colorizePost.StartResetEffect();
			colorizePost.FxData = LayerFXData.FromString(level.LevelVariables.Dictionary[LV.PostFX]);
			iObjectManager.Dispose();
			iObjectManager = new InteractiveObjectManager();
			iObjectManager.Initialize(content.ServiceProvider, level.LevelVariables,device, options);
			
			LevelLoadEnhanced(50, "Level Loaded...");
			player = new Player(level.Startpos, Content);
			InitializeLayer(options,device);
			LevelLoadEnhanced(60, "Layers Initialized");
			gamelogic.Dispose();
			gamelogic = new GameLogic();
			gamelogic.Initialize(content.ServiceProvider,device, level.LevelVariables, options);
			camera = new Camera(options.ScreenWidth,
				options.ScreenHeight,
				level.Startpos,
				(float) options.ScaleFactor);
			debugscreen = new DebugScreen(Content, level.Startpos);
			LevelLoadEnhanced(75, "Game Logic Loaded");
			bg_tex = content.Load<Texture2D>(GameConstants.GraphXPacksPath + level.LevelVariables.Dictionary[LV.GraphXPack] + "/Backgrounds/" + level.LevelVariables.Dictionary[LV.BackgroundImg]);
			enemyManager.Dispose();
			enemyManager = new EnemyManager(ServiceProvider,options);
			LoadEnemies(options);
			
			enemyManager.PlayerKilled += gamelogic.PlayerKilled;
			
			//Invoke LevelLoaded Event
			LevelLoaded();
			//Cleaning UP...
			GC.Collect();

			firstFrame = true;
		}

		void level_LevelLoadLineEnhanced(int cur_line, int max_line)
		{
			LevelLoadEnhanced((byte)((float)cur_line/(float)max_line * 50),"["+cur_line+"|"+max_line+"] Level Lines Loaded");
		}

		private void InitializeLayer(Options pOptions, GraphicsDevice graphicsDevice)
		{
			if (cloudlayer != null)
				cloudlayer.Dispose();
			cloudlayer = new CloudLayer();
			cloudlayer.Initialize(pOptions, level.LevelVariables);
			cloudlayer.FXData = LayerFXData.FromString(level.LevelVariables.Dictionary[LV.CloudsFX]);
			LevelLoadEnhanced(53, "Clouds Loaded...");
			if (frontparallaxLayer != null)
				frontparallaxLayer.Dispose();
			frontparallaxLayer = new ParallaxLayer();
			frontparallaxLayer.GraphXName = level.LevelVariables.Dictionary[LV.FrontParallaxLayerImage];

			pOptions.InitObjectHolder.dataHolder.PutData(Layer.Data_Layer.NumLayerObjects.GetKey(), 2);
			frontparallaxLayer.Initialize(pOptions, level.LevelVariables);
			frontparallaxLayer.SpeedDivider = float.Parse(level.LevelVariables.Dictionary[LV.FrontParallaxLayerSpeedDivider]);
			frontparallaxLayer.FXData = LayerFXData.FromString(level.LevelVariables.Dictionary[LV.FrontParallaxFX]);
			LevelLoadEnhanced(56, "Front Parallax Layer loaded...");
			if (rearparallaxLayer != null)
				rearparallaxLayer.Dispose();
			rearparallaxLayer = new ParallaxLayer();
			rearparallaxLayer.GraphXName = level.LevelVariables.Dictionary[LV.RearParallaxLayerImage];
			rearparallaxLayer.Initialize(pOptions, level.LevelVariables);
			rearparallaxLayer.SpeedDivider = float.Parse(level.LevelVariables.Dictionary[LV.RearParallaxLayerSpeedDivider]);
			rearparallaxLayer.FXData = LayerFXData.FromString(level.LevelVariables.Dictionary[LV.RearParallaxFX]);
			LevelLoadEnhanced(59, "Rear Parallax Layer loaded...");
			if (rearDecoLayer != null)
				rearDecoLayer.Dispose();
			rearDecoLayer = new DecoLayer();
			rearDecoLayer.LayerName = DecorationsLayer.Rear.ToString();

			pOptions.InitObjectHolder.dataHolder.PutData(Layer.Data_Layer.NumLayerObjects.GetKey(), 1);
			rearDecoLayer.Initialize(pOptions, level.LevelVariables);
			rearDecoLayer.FXData = LayerFXData.FromString(level.LevelVariables.Dictionary[LV.LevelFX]);
			if (frontDecoLayer != null)
				frontDecoLayer.Dispose();
			frontDecoLayer = new DecoLayer();
			frontDecoLayer.LayerName = DecorationsLayer.Front.ToString();
			frontDecoLayer.Initialize(pOptions, level.LevelVariables);
			frontDecoLayer.FXData = LayerFXData.FromString(level.LevelVariables.Dictionary[LV.LevelFX]);
			Texture2D tex = content.Load<Texture2D>("Img\\Game\\Exit");
			Rectangle exitRect = level.LevelVariables.ExitRectangle;
			exitRect.Width *= 2;
			exitRect.Height *= 2;
			exitRect.Y -= exitRect.Height / 2;
			ExtendedRectangle extRect = ExtendedRectangle.Transform(exitRect, new Vector2(tex.Width, tex.Height), null, Vector2.Zero, 0);
			LayerObject layerObject = new LayerObject(tex, extRect);
			rearDecoLayer.AddItem(layerObject);

		}

		private void LoadEnemies(Options options)
		{
			if (level.LevelVariables.Dictionary.ContainsKey(LV.Enemies))
				if (!string.IsNullOrEmpty(level.LevelVariables.Dictionary[LV.Enemies]))
				{
					string[] enemies = level.LevelVariables.Dictionary[LV.Enemies].Split(':');
					for (int i = 0; i < enemies.Length; i++)
					{
						if (!string.IsNullOrWhiteSpace(enemies[i]))
						{
							if ((i + 1) % 5 == 0 || i + 1 == enemies.Length)
								LevelLoadEnhanced((byte)((((float)i / enemies.Length) * 25) + 75), "[" + (i + 1) + "|" + enemies.Length + "] Enemies Loaded");
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

									enemyManager.AddEnemy(name, startpos, startdirection, options);
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
					LevelLoadEnhanced(100, "Enemies Loaded");
				}

		}

		public EGameState Update(Microsoft.Xna.Framework.GameTime gameTime,Input.Inputhandler inputhandler,Options options)
		{
			
			List<Star.Game.CollisionType> collission = new List<Star.Game.CollisionType>();
			List<EventAction> actions;
			Inputhandler input;
			actions = gamelogic.Update(gameTime, player.BoundingBox,level.GetQuadtree,enemyManager.Enemies,this);
			if (!actions.Contains(EventAction.Exit))
			{
				 if (actions.Contains(EventAction.BlockInput))
				{
					input = new Inputhandler(player.Pos,options);
				}
				else
				{
					input = inputhandler;
				}
				if (!actions.Contains(EventAction.BlockUpdate) || firstFrame)
				{
					firstFrame = false;
					enemyManager.Update(gameTime, level.GetQuadtree, player.Pos, player.BoundingBox, level.Tiles, camera.getMatrix);
			   
					

					collission = player.Update(gameTime, input.Pos, level.GetQuadtree, input.GetInputKeys, input.GetOldInputkeys, input.RunDirection, input.RunFactor, iObjectManager.GetPlayerInfluences());
					camera.update(gameTime, player.getDifference, player.Pos, false);

					

					debugscreen.update(start, player.Pos, player.speed, player.Gravity, player.Jump, input.Pos, collission, input.RunFactor, input.GetInputKeys);
				}
				iObjectManager.Update(gameTime, this);
				input.UpdateRunFactor(gameTime, collission);
				Vector2 relativePosition = new Vector2(-camera.Position.X / (level.Tiles.GetLength(1) * Tile.TILE_SIZE), -camera.Position.Y / ((level.Tiles.GetLength(0) * Tile.TILE_SIZE) - options.ScreenHeight / 2));
				relativePosition.Y = MathHelper.Clamp(relativePosition.Y, 0, 1);
				cloudlayer.Update(gameTime, camera.CamDifference, options,relativePosition);
				frontparallaxLayer.Update(gameTime, camera.CamDifference, options,relativePosition);
				rearparallaxLayer.Update(gameTime, camera.CamDifference, options,relativePosition);
				frontDecoLayer.Update(gameTime, camera.CamDifference, options,relativePosition);
				rearDecoLayer.Update(gameTime, camera.CamDifference, options,relativePosition);
				if (collission.Contains(Star.Game.CollisionType.WalksAgainstIt))
				{
					input.Pos = player.Pos;
				}

				if (inputhandler.GetMenuKeys.Contains(MenuKeys.Back))
				{
					return EGameState.Menu;
				}
				rundirection = input.RunDirection;
				while (!enemyManager.IsFinished())
				{
					Thread.Sleep(0);
				}
			}
			else
			{
				return EGameState.Menu;
			}
			return EGameState.Game;
		}

		public void Draw(GameTime gametime,SpriteBatch spritebatch, GraphicsDevice graphics)
		{
			//graphics.SetRenderTarget(basicTarget);
			graphics.Clear(new Color(255, 255, 255));
			
			//3_1
			//spritebatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState);
			colorizeBackground.ApplyParameters();
			spritebatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,null,null,null,colorizeBackground.Effect);
			//colorizeBackground.Begin();
			spritebatch.Draw(bg_tex, bg_rect, Color.White);
			//colorizeBackground.End();
			spritebatch.End();

			rearparallaxLayer.Draw(spritebatch);
			frontparallaxLayer.Draw(spritebatch);
			cloudlayer.Draw(spritebatch);
			//Raineffect(spritebatch, graphics);
			gamelogic.Draw(DrawPosition.BehindLevel, gametime, spritebatch, camera.getMatrix);
			rearDecoLayer.Draw(spritebatch, camera.getMatrix);
			level.DrawLevel(spritebatch, camera.getMatrix, new Rectangle((int)-camera.Position.X - spritebatch.GraphicsDevice.PresentationParameters.BackBufferWidth, (int)-camera.Position.Y - spritebatch.GraphicsDevice.PresentationParameters.BackBufferHeight, spritebatch.GraphicsDevice.PresentationParameters.BackBufferWidth*2, spritebatch.GraphicsDevice.PresentationParameters.BackBufferHeight*2));
			iObjectManager.Draw(spritebatch, camera.getMatrix);
			gamelogic.Draw(DrawPosition.InFrontOfLevel, gametime, spritebatch, camera.getMatrix);
			MotionBlur(spritebatch, graphics);
			enemyManager.Draw(gametime, spritebatch, camera.getMatrix,false);
			player.draw_player(ref spritebatch, camera.getMatrix, rundirection,graphics);
			frontDecoLayer.Draw(spritebatch, camera.getMatrix);

			colorizePost.DrawPostProcess(spritebatch);
			//colorizePost.DrawPostProcess(spritebatch, spritebatch);
			debugscreen.draw(spritebatch, graphics, effect, camera.getScaleMatrix);
			//Blur(spritebatch, graphics);
			gamelogic.Draw(DrawPosition.Post, gametime, spritebatch, camera.getMatrix);

			//toonShader.CurrentTechnique = ToonShaderEffects.Outline;
			//toonShader.DrawPostProcess(spritebatch);
			//graphics.SetRenderTarget(null);
			//spritebatch.Begin();
			//spritebatch.Draw(basicTarget, Vector2.Zero, Color.White);
			//spritebatch.End();
			start = DateTime.Now;
		}

		private void Blur(SpriteBatch spriteBatch, GraphicsDevice device)
		{

			RenderTargetBinding[] targets = device.GetRenderTargets();

			spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, blur);    
			spriteBatch.Draw((RenderTarget2D)targets[0].RenderTarget, Vector2.Zero, Color.White);
			spriteBatch.End();
		}

		private void Raineffect(SpriteBatch spritebatch,GraphicsDeviceManager graphics)
		{
			////3_1
			////graphics.ResolveBackBuffer(resolvedtexture);
			////tex = resolvedtexture;
			////graphics.Clear(Color.Black);
			//RenderTargetBinding[] target = graphics.GetRenderTargets();
			//pos1.Y -= 0.1f;
			//pos1.X += player.getDifference.X / 130;
			//pos2.X += player.getDifference.X / 130;
			//waether.Parameters["pos1x"].SetValue(pos1.X);
			//waether.Parameters["pos1y"].SetValue(pos1.Y);
			//pos2.Y -= 0.1f;
			//pos2.X += 0.01f;
			//waether.Parameters["pos2x"].SetValue(pos2.X);
			//waether.Parameters["pos2y"].SetValue(pos2.Y);
			//spritebatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState);
			//waether.Begin();
			//foreach (EffectPass pass in waether.CurrentTechnique.Passes)
			//{
			//    pass.Begin();
			//    spritebatch.Draw(tex, Vector2.Zero, Color.White);
			//    pass.End();
			//}
			//waether.End();
			//spritebatch.End();
		}

		private void MotionBlur(SpriteBatch spritebatch, GraphicsDevice graphics)
		{
			//3_1
			//graphics.ResolveBackBuffer(resolvedtexture);
			//tex = resolvedtexture;
			RenderTargetBinding[] targets = graphics.GetRenderTargets();
			tex = (RenderTarget2D)targets[0].RenderTarget;
			graphics.SetRenderTarget(target);
			float speedx;
			if (player.speed.X < 0)
			{
				speedx = MathHelper.Clamp(camera.CamOffset.X, -Player.MAX_SPEED, -60);
				speedx += 60;
			}
			else
			{
				speedx = MathHelper.Clamp(camera.CamOffset.X, 60, Player.MAX_SPEED);
				speedx -= 60;
			}
			speedx /= 600;
			motionblur.Parameters["speed"].SetValue(
				new Vector2(speedx, camera.CamOffset.Y / 5000));
			//graphics.Clear(Color.Black);
			//3_1
			//spritebatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState);
			//motionblur.Begin();
			//foreach (EffectPass pass in motionblur.CurrentTechnique.Passes)
			//{
			//    pass.Begin();
			spritebatch.Begin(SpriteSortMode.Immediate, null, null, null, null, motionblur);
			spritebatch.Draw(tex, Vector2.Zero, Color.White);
			//    pass.End();
			//}
			//motionblur.End();
			spritebatch.End();
			graphics.SetRenderTarget((RenderTarget2D)targets[0].RenderTarget);
			spritebatch.Begin();
			spritebatch.Draw(target, Vector2.Zero, Color.White);
			spritebatch.End();
		}

		public void Unload()
		{
			enemyManager.Dispose();
			gamelogic.Dispose();
			iObjectManager.Dispose();
			UnloadGraphicsChanged();
			basicTarget.Dispose();
		}

		#endregion



		#region IDisposable Member

		public void Dispose()
		{
			Unload();
		}

		#endregion

		#region IGraphicsChange Member

		public void GraphicsChanged(GraphicsDevice device,Options options)
		{
			//3_1
			//resolvedtexture.Dispose();
			//resolvedtexture = new ResolveTexture2D(device, options.ScreenWidth, options.ScreenHeight, device.PresentationParameters.BackBufferCount, SurfaceFormat.Color);
			PresentationParameters pp = device.PresentationParameters;
			basicTarget = new RenderTarget2D(
				device,
				pp.BackBufferWidth,
				pp.BackBufferHeight,
				false,
				SurfaceFormat.Color,
				DepthFormat.None,
				pp.MultiSampleCount,
				RenderTargetUsage.PreserveContents);
			target = new RenderTarget2D(
				device,
				pp.BackBufferWidth,
				pp.BackBufferHeight,
				false,
				SurfaceFormat.Color,
				DepthFormat.None,
				pp.MultiSampleCount,
				RenderTargetUsage.PreserveContents);
			bg_rect = new Rectangle(0, 0, options.ScreenWidth, options.ScreenHeight);
			camera = new Camera(options.ScreenWidth, options.ScreenHeight, Player.Pos, options.ScaleFactor);
		}

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
