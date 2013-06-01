using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using XNAParticleSystem;
using System.Windows.Forms;

namespace XNAParticleSystem
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class Game1 : Microsoft.Xna.Framework.Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		ParticleSystem system1;
		ParticleSystem system2;
		ParticleManager Manager;
		Texture2D blank;
		Rectangle testrect;
		Rectangle rect2;
		Rectangle rect3;
		Rectangle rect4;
		Rectangle rect5;
		Rectangle[,] rects;
		SpriteFont font;
		List<Rectangle> collisionrects;
		float total=0;
		float elapsedTime;
		string output="";
		float refreshtime=0;
		bool usingCuda;
		RenderTarget2D target;
		InitializeForm form;
		MouseState oldState;
		bool single = true;
		List<ParticleSystem> particleSystems;
		List<double> averageCalcTime;
		List<double> averageDrawTime;
		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			this.IsFixedTimeStep = false;
			graphics.PreferredBackBufferHeight = 600;
		}

		public void InitializeNEW(bool BetweenParticleCollision)
		{
			Manager.Stop();
			Manager.Clear();
			collisionrects.Clear();
			if (system1 != null)
				system1.Dispose();
			system1 = new XNAParticleSystem.ParticleSystem();
			if (system2 != null)
				system2.Dispose();
			system2 = new XNAParticleSystem.ParticleSystem();
			system1.BetweenParticleCollision1 = BetweenParticleCollision;
			system2.BetweenParticleCollision1 = BetweenParticleCollision;
		}

		public void AddSystemsToManager()
		{
			Manager.Add(system1);
			Manager.Add(system2);
			Manager.StartAllExplosions();
			Manager.ResetAllSystems();
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			averageCalcTime = new List<double>();
			averageDrawTime = new List<double>();
			particleSystems = new List<ParticleSystem>();
			DebugForm.Initialize();
			Manager = new ParticleManager();
			form = new InitializeForm(this);
			form.Show();
			this.IsMouseVisible = true;
			collisionrects = new List<Rectangle>();
			blank = Content.Load<Texture2D>("Blank");
			font = Content.Load<SpriteFont>("Arial");
			rects = new Rectangle[10, 10];
			rect2 = new Rectangle(-1000,0 , 1000, 800);
			rect3 = new Rectangle(800, 0, 1000, 800);
			rect4 = new Rectangle(0, -1000, 800, 1000);
			rect5 = new Rectangle(0, 600, 1000, 1000);
			testrect = new Rectangle(350,250, 100, 100);

			system1 = new XNAParticleSystem.ParticleSystem();
			system2 = new XNAParticleSystem.ParticleSystem();
			system1.debugPrefix = "System1";
			system2.debugPrefix = "System2";

			target = new RenderTarget2D(GraphicsDevice, GraphicsDevice.PresentationParameters.BackBufferWidth,
				GraphicsDevice.PresentationParameters.BackBufferHeight,
				false,
				SurfaceFormat.Color,
				DepthFormat.None,
				GraphicsDevice.PresentationParameters.MultiSampleCount,
				RenderTargetUsage.PreserveContents);
			//InitializeNewParticleMangersTest();
			Manager.StartAllExplosions();
			//InitializeNewtonAtom();
			//InitializeSnow();
			InitializeMASSCollision();

			//InitializeBloodCollision();
			//if (system1 !=null)
			{
				Manager.Add(system1);
			} //if (system2!=null)
			{

				Manager.Add(system2); 
			}
			//InitializeNewton();
			//InitializeExplosion();
			//InitializeBiene();
			
			//InitializeCircle();
			//InitializeWaterfall();

			//manager2.AddNewtonMass(new Vector2(400, 300), 5000);

			//manager2.AddCollisionRect(testrect);
			//manager2.AddCollisionRect(testrect);
			system1.AddCollisionRect(rect5);
			system1.AddCollisionRect(rect2);
			system1.AddCollisionRect(rect3);
			system2.AddCollisionRect(rect4);
			system1.AddCollisionRect(rect4);
			system2.AddCollisionRect(rect5);
			system2.AddCollisionRect(rect2);
			system2.AddCollisionRect(rect3);
			system1.AddCollisionRect(new Rectangle(-1000, 500, 50, 50));
			collisionrects.Add(new Rectangle(-1000, 500,50, 50));
			base.Initialize();
			//manager.UseTimedAlpha = false;
			//manager.Alpha = 0.1f;
		}

		public void InitializeNewParticleMangersTest()
		{
			for (int i = 0; i < 5; i++)
			{
				for (int k = 0; k < 5; k++)
				{
					ParticleSystem system = new ParticleSystem();
					system.Initialize(false,1000, 5, SpawnType.Explosion, 2, Content.Load<Texture2D>("blank"), new Vector2(i * 100 + 100, k * 100 + 100));
					system.Gravity = new Vector2(0, 200);
					system.MaxSpeed = new Vector2(100, 100);
					system.Type = GravityType.OverallForce;
					system.Spawndirections = SpawnDirections.AllWays;
					system.debugPrefix = i.ToString() + "x" + k.ToString() + " System";
					system.Collisiontype = CollisionType.Collision;
					system.AddCollisionRect(rect2);
					system.AddCollisionRect(rect3);
					system.AddCollisionRect(rect4);
					system.AddCollisionRect(rect5);
					
					if (single)
					{
						Manager.Add(system); 
					}
					else
					{
						//particleSystems.Add(system);
					}
					
				}
			}
		}


		public void InitializeNewtonComplete()
		{
			system1.Initialize(false,
					SpawnType.Fontaine,
					1000,
					10f,
					8,
					Color.White,
					new Vector2(350, 200),
					SpawnDirections.AllWays,
					new Vector2(10, 10),
					Content.Load<Texture2D>("blank"),
					GravityType.Newton,
					new Vector2(100, 0),
					1f,
					CollisionType.Collision);
			system1.FixedMasses = false;
			system1.SpawnAngle = 0.1f;
			system1.UseMinSpeed = true;
			system1.MinSpeed = new Vector2(100, 0);
			//manager.AddNewtonMass(new Vector2(400, 300), 500);
			system1.ParticleMass = 5;
		}

		public void InitializeWaterfall()
		{
			system1.Initialize(false,
						SpawnType.Fontaine,
						1000,
						10f,
						10,
						new Color(0.5f, 0.5f, 0.8f),
						new Vector2(100, 50),
						new Vector2(700,150),
						SpawnDirections.XPlusMinus,
						new Vector2(500,0),
						Content.Load<Texture2D>("blank"),
				//blank,
						GravityType.OverallForce,
						new Vector2(0, 1000),
						1.1f,
						CollisionType.Collision);
			system1.AirFriction = 0.95f;
			system1.AddCollisionRect(rect5);
			system1.AddCollisionRect(rect2);
			system1.AddCollisionRect(rect3);
			system2.AddCollisionRect(rect4);
			system1.AddCollisionRect(rect4);
			system2.AddCollisionRect(rect5);
			system2.AddCollisionRect(rect2);
			system2.AddCollisionRect(rect3);
		}

		public void InitializeCircle()
		{
			system1.Initialize(false, SpawnType.Explosion,
				10000,
				1000f,
				2,
				Color.White,
				new Vector2(400, 300),
				SpawnDirections.AllWays,
				new Vector2(800),
				Content.Load<Texture2D>("blank"),
				GravityType.Point,
				new Vector2(400, 300),
				2,
				CollisionType.None);
			system1.AirFriction = 0.999f;
			system1.UseMinSpeed = true;
			system1.MinSpeed = new Vector2(800);
			usingCuda =  system1.UseCuda(true);
		}

		public void InitializeMASSCollision()
		{
			system1.Initialize(false,
					SpawnType.Explosion,
					10000,
					10000f,
					2,
					new Color(0.5f, 1f, 0.1f),
					new Vector2(75, 275),
					SpawnDirections.AllWays,
					new Vector2(300),
					Content.Load<Texture2D>("blank"),
				//blank,
					GravityType.OverallForce,
					new Vector2(2, 2),
					2f,
					CollisionType.Collision);
			system1.UseMinSpeed = true;
			system1.MinSpeed = new Vector2(100);
			usingCuda = system1.UseCuda(true);
			//manager.AirFriction = 0.9f;
			system1.AddNewtonMass(new Vector2(100, 0), 5000);
			system1.AddNewtonMass(new Vector2(500, 500), 5000);
			system1.UseTimedAlpha = true;
			system1.Alpha = 0.1f;
			int xmax = 20;
			int ymax = 20;
			rects = new Rectangle[xmax, ymax];
			for (int x = 0; x < xmax; x++)
			{
				for (int y = 0; y < ymax; y++)
				{
					rects[x, y] = new Rectangle(25 * x, 25 * y, 15, 15);
					system1.AddCollisionRect(rects[x, y]);
					collisionrects.Add(rects[x, y]);
					//manager2.AddCollisionRect(rects[x,y]);
				}

			}
			system2.Initialize(false,
					SpawnType.Explosion,
					10000,
					10000f,
					2,
					new Color(1f, 0f, 0f),
					new Vector2(275, 275),
					SpawnDirections.AllWays,
					new Vector2(300),
					Content.Load<Texture2D>("blank"),
				//blank,
					GravityType.OverallForce,
					new Vector2(2, 2),
					2f,
					CollisionType.Collision);
			system2.UseMinSpeed = true;
			system2.MinSpeed = new Vector2(100);
			usingCuda = system2.UseCuda(true);
			//manager.AirFriction = 0.9f;
			system2.AddNewtonMass(new Vector2(100, 0), 5000);
			system2.AddNewtonMass(new Vector2(500, 500), 5000);
			system2.UseTimedAlpha = true;
			system2.Alpha = 0.1f;
			rects = new Rectangle[xmax, ymax];
			for (int x = 0; x < xmax; x++)
			{
				for (int y = 0; y < ymax; y++)
				{
					rects[x, y] = new Rectangle(25 * x, 25 * y, 15, 15);
					system2.AddCollisionRect(rects[x, y]);
					//collisionrects.Add(rects[x, y]);
					//manager2.AddCollisionRect(rects[x,y]);
				}

			}
		}

		public void InitializeMASSCollisionNOCUDA()
		{
			system1.Initialize(false,
					SpawnType.Explosion,
					10000,
					10000f,
					2,
					new Color(0.5f,1f,0.1f),
					new Vector2(75,275),
					SpawnDirections.AllWays,
					new Vector2(300),
					Content.Load<Texture2D>("blank"),
				//blank,
					GravityType.OverallForce,
					new Vector2(2,2),
					2f,
					CollisionType.Collision);
			system1.UseMinSpeed = true;
			system1.MinSpeed = new Vector2(100);
			//usingCuda = manager.UseCuda(true);
			//manager.AirFriction = 0.9f;
			system1.AddNewtonMass(new Vector2(100,0),5000);
			system1.AddNewtonMass(new Vector2(500,500),5000);
			system1.UseTimedAlpha = true;
			system1.Alpha = 0.1f;
			int xmax = 20;
			int ymax = 20;
			rects = new Rectangle[xmax, ymax];
			for (int x = 0; x < xmax; x++)
			{
				for (int y = 0; y < ymax; y++)
				{
					rects[x, y] = new Rectangle(25 * x, 25 * y, 15, 15);
					system1.AddCollisionRect(rects[x, y]);
					collisionrects.Add(rects[x, y]);
					//manager2.AddCollisionRect(rects[x,y]);
				}

			}
			//manager.AirFriction = 1.0001f;
			//for (int i = 0; i < 100; i++)
			//{
			//    Vector2 pos = new Vector2((float)rand.NextDouble() * 800, (float)rand.NextDouble() * 600);
			//    manager.AddNewtonMass(pos, 100000 * (float)rand.NextDouble());
			//    collisionrects.Add(new Rectangle((int)pos.X, (int)pos.Y, 5, 5));
			//}

			//manager2.Initialize(
			//        SpawnType.Explosion,
			//        12500,
			//        100,
			//        2,
			//        new Color(1f,0f, 0f),
			//        new Vector2(400, 300),
			//        SpawnDirections.AllWays,
			//        new Vector2(500),
			//        Content.Load<Texture2D>("blank"),
			//    //blank,
			//        GravityType.Point,
			//        new Vector2(399, 299),
			//        2f,
			//        CollisionType.Collision);
			//manager2.UseCuda(true);
			//manager2.UseMinSpeed = true;
			//manager2.MinSpeed = new Vector2(500);
			////manager.AirFriction = 0.9f;
			//manager2.AddNewtonMass(new Vector2(100, 0), 5000);
			//manager2.AddNewtonMass(new Vector2(500, 500), 5000);
		}

		public void InitializeSnow()
		{
			system1.Initialize(false,
				SpawnType.Fontaine,
				2000,
				500,
				8,
				Color.WhiteSmoke,
				new Vector2(800, 0), new Vector2(800, 600),
				SpawnDirections.Normal,
				new Vector2(300, 000),
				Content.Load<Texture2D>("blank"),
							//blank,
				GravityType.OverallForce,
				new Vector2(-400, 100),
				10f,
				CollisionType.Collision);
			system2.Initialize(false,
				SpawnType.Fontaine,
				1000,
				100,
				8,
				Color.WhiteSmoke,
				new Vector2(0, 0), new Vector2(800, 0),
				SpawnDirections.Normal,
				new Vector2(000, 100),
				Content.Load<Texture2D>("blank"),
				//blank,
				GravityType.OverallForce,
				new Vector2(-400, 100),
				10f,
				CollisionType.Collision);
		}

		public void InitializeBloodCollision()
		{
			system1.Initialize(false, SpawnType.Fontaine,
				2000,
				20f,
				20,
				Color.White,
				new Vector2(200, 250),
				 SpawnDirections.AllWays,
				 new Vector2(50),
				 Content.Load<Texture2D>("arrow"),
				  GravityType.OverallForce,
				  new Vector2(400,200),
				 1,
				CollisionType.Collision);
			//manager.AddNewtonMass(new Vector2(400,300), 5000);
			//manager.AddCollisionRect(new Rectangle(395, 295, 10, 10));
			//collisionrects.Add(new Rectangle(395, 295, 10, 10));
			//system1.AddNewtonMass(new Vector2(400,200), 5000);
			system1.AddCollisionRect(new Rectangle(395, 195, 10, 10));
			collisionrects.Add(new Rectangle(395, 195, 10, 10));
			//manager.AirFriction = 0.999f;
		}

		public void InitializeBiene()
		{
			system1.Initialize(false, SpawnType.Explosion,
				100,
				100f,
				50,
				Color.White,
				new Vector2(200, 250),
				 SpawnDirections.AllWays,
				 new Vector2(200),
				 Content.Load<Texture2D>("biene"),
				  GravityType.Newton,
				  new Vector2(400, 200),
				 1,
				CollisionType.Collision);
			system1.UseTimedAlpha = true;
			system1.Alpha = 1;
			//manager.AirFriction = 0.5f;
			//manager.AddNewtonMass(new Vector2(400,300), 5000);
			//manager.AddCollisionRect(new Rectangle(395, 295, 10, 10));
			//collisionrects.Add(new Rectangle(395, 295, 10, 10));
			system1.AddNewtonMass(new Vector2(400, 200), 100000);
			system1.AddCollisionRect(new Rectangle(395, 195, 10, 10));
			collisionrects.Add(new Rectangle(395, 195, 10, 10));
			//manager.AirFriction = 0.999f;
		}

		public void InitializeNewtonAtom()
		{
			system2.Initialize(false,
				SpawnType.Fontaine,
				50000,
				250,
				8,
				Color.Blue,
				new Vector2(200, 100),
				SpawnDirections.AllWays,
				new Vector2(0, 0),
				Content.Load<Texture2D>("blood"),
						//blank,
				GravityType.Point,
				new Vector2(400, 300),
				1f,
				CollisionType.Collision);
			system1.Initialize(false,
				SpawnType.Fontaine,
				50000,
				500,
				8,
				Color.Red,
				new Vector2(400, 200),
				SpawnDirections.Normal,
				new Vector2(500, 000),
				Content.Load<Texture2D>("blood"),
				//blank,
				GravityType.Point,
				new Vector2(400, 300),
				1f,
				CollisionType.None);

			system2.AddCollisionRect(testrect);
			collisionrects.Add(testrect);
			//usingCuda = manager2.UseCuda(true);
		}

		public void InitializeNewton()
		{
			system1.Initialize(false,
				SpawnType.Fontaine,
				10000,
				100,
				8,
				Color.White,
				new Vector2(350, 200),
				SpawnDirections.Angle,
				new Vector2(100, 10),
				Content.Load<Texture2D>("blank"),
				GravityType.Newton,
				new Vector2(100,0),
				1f,
				CollisionType.Collision);
			system1.SpawnAngle = 0.1f;
			system1.UseMinSpeed = true;
			system1.MinSpeed = new Vector2(100, 0);
			system1.AddNewtonMass(new Vector2(400, 300), 10000);
			//manager.AddNewtonMass(new Vector2(400, 325), 5000);
			//manager.UseCuda(true);
			//manager.AddCollisionRect(new Rectangle(350,250,100,100));
			//collisionrects.Add(new Rectangle(350, 250, 100, 100));
			
		}

		public void InitializeExplosion()
		{
			Vector2 maxSpeed = new Vector2(500);
			system1.Initialize(false, SpawnType.Explosion,
				2000,
				20,
				20,
				Color.Red,
				new Vector2(400, 275),
				SpawnDirections.AllWays,
				new Vector2(-2000,-2000),
				Content.Load<Texture2D>("blood"),
				GravityType.OverallForce,
				new Vector2(0,0),
				2,
				CollisionType.Collision);
			system1.AirFriction = 0.995f;
			//manager.MinSpeed = maxSpeed - new Vector2(350);
			system1.UseMinSpeed = true;
			//manager.ParticleLifetime = 0.4f;

			//for (int x = 0; x < 10; x++)
			//{
			//    for (int y = 0; y < 8; y++)
			//    {
			//        rects[x, y] = new Rectangle(75 * x, 75 * y, 20, 20);
			//        manager.AddCollisionRect(rects[x, y]);
			//        collisionrects.Add(rects[x, y]);
			//        //manager2.AddCollisionRect(rects[x,y]);
			//    }

			//}
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
			Manager.Stop();
		}

		protected override void OnExiting(object sender, EventArgs args)
		{
			MessageBox.Show(output);
			base.OnExiting(sender, args);
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			if (Mouse.GetState().RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && oldState.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
			{
				if (!system1.FixedMasses)
				{
					system1.AddNewtonMass(new Vector2(Mouse.GetState().X, Mouse.GetState().Y), 500);
				}
				else
				{
					system1.Type = GravityType.Point;
					system1.Gravity = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
					system2.Type = GravityType.Point;
					system2.Gravity = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
				}
			}
			else if(Mouse.GetState().MiddleButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
			{
				system1.Type = GravityType.OverallForce;
				system1.Gravity = new Vector2(0, 1000);
			}
			elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
			Random rand = new Random();
			if (elapsedTime > 2)
			{
				elapsedTime = 0;
				Vector2 pos = new Vector2((float)rand.NextDouble() * 800, (float)rand.NextDouble() * 600);
			}
			// Allows the game to exit
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
				this.Exit();
			if (single)
			{
				Manager.Update(gameTime); 
			}
			else
			{
				for (int i = 0; i < particleSystems.Count; i++)
				{
					particleSystems[i].Update(gameTime);
					particleSystems[i].barrier.SignalAndWait();
				}
			}

			if (Mouse.GetState().LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
			{
				Manager.ResetAllSystems();
				system1.Reset();
				system2.Reset();
			}
			base.Update(gameTime);
			averageCalcTime.Add(Manager.milliseconds_needed);
			refreshtime += (float)gameTime.ElapsedGameTime.TotalSeconds;
			if (refreshtime > 0.25f)
			{
				double avgCalcTime =0;
				double avgDrawTime =0;
				for (int i = 0; i < averageCalcTime.Count; i++)
			{
					avgCalcTime+= averageCalcTime[i];
				}
				avgCalcTime/=averageCalcTime.Count;
				for (int i = 0; i < averageDrawTime.Count; i++)
			{
					avgDrawTime+= averageDrawTime[i];
				}
				avgDrawTime/=averageDrawTime.Count;

				refreshtime = 0;
				output = "Rendertime:";
				output += "\nCuda is " + (ParticleCUDA.CudaUsable() ? (usingCuda ? "enabled" : "disabled") : "not available");
				total = 0;
				if (Environment.ProcessorCount <= system1.CalcTime.Length)
					total /= Environment.ProcessorCount;
				output += "\nTotal time needed: " + Manager.milliseconds_needed.ToString("00.000") + "ms";
				output += "\nDraw Time: " + Manager.drawMilliseconds_needed.ToString("00.000") + "ms";
				output += "\nAverage Calc Time: " +avgCalcTime.ToString("00.000")+ "ms";
				output += "\nAverage Draw Time: " + avgDrawTime.ToString("00.000") + "ms";
			}
			oldState = Mouse.GetState();
			//timesRun++;
			if (gameTime.TotalGameTime.TotalSeconds > 30 && gameTime.TotalGameTime.TotalSeconds < 31)
			{
				//MessageBox.Show(output);
			}
			
		}

		

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{

		   
			GraphicsDevice.SetRenderTarget(target);
			GraphicsDevice.Clear(Color.Black);
			if (single)
			{
				Manager.Draw(spriteBatch); 
			}
			else
			{
				for (int i = 0; i < particleSystems.Count; i++)
				{
					particleSystems[i].Draw(spriteBatch);
				}
			}
			averageDrawTime.Add(Manager.drawMilliseconds_needed);
			//system1.Draw(spriteBatch);
			if (!system1.FixedMasses)
			{
				system1.DrawMasses(spriteBatch, blank, 10, Matrix.CreateTranslation(Vector3.Zero));
			}
			//system2.Draw(spriteBatch);
			
			// TODO: Add your drawing code here
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
			//spriteBatch.Draw(blank, new Rectangle(0, 0, 800, 600), new Color(0,0,0, 0.2f));
			foreach (Rectangle rect in collisionrects)
			{
				spriteBatch.Draw(blank, rect, new Color(255, 255, 255, 127));
			}
			
			//spriteBatch.Draw(blank, testrect, new Color(255,255,255,127));

			spriteBatch.DrawString(font, output, new Vector2(10, 10), Color.LightGreen);
			spriteBatch.DrawString(font, system1.NrParticlesAlive.ToString() + " Particles Alive", new Vector2(200, 10), Color.LightGreen);
			spriteBatch.End();
			base.Draw(gameTime);
			GraphicsDevice.SetRenderTarget(null);
			GraphicsDevice.Clear(Color.Black);
			spriteBatch.Begin();
			spriteBatch.Draw(target, Vector2.Zero, Color.White);
			spriteBatch.End();
		}
	}
}
