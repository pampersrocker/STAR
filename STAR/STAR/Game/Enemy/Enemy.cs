using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Star.Game.Level;
using Star.GameManagement;
using Star.Input;
using System.Globalization;
using XNAParticleSystem;
using Star.Game.Debug;
using Star.Graphics.Effects.PostProcessEffects;

namespace Star.Game.Enemy
{

	public partial class Enemy : IDisposable
	{
		Vector2 pos;
		Vector2 gravity;
		Vector2 speed;
		Rectangle rect;
		bool initialized = false;
		Dictionary<EnemyVariables, string> enemyvariables;
		string errorMessage;
		string eType;
		int myNumber;
		AnimationManager animations;
		EnemyCollision collision;
		MovementType movementType;
		StandardDirection rundirection;
		StandardDirection standardirection;
		ContentManager Content;
		Texture2D tex;
		ParticleSystem particleManager;
		Wave wave;
		float maxSpeed;
		bool alive;
		byte numThread;
		float timeTillDeath;
		bool removable;
		List<Tile> Collisions;

		static int number;

		public bool Removable
		{
			get { return removable; }
			set { removable = value; }
		}
		

		#region Properties

		public byte ThreadNumber
		{
			get { return numThread; }
		}

		public bool Alive
		{
			get { return alive; }
			set { alive = value; }
		}

		public StandardDirection RunDirection
		{
			get { return rundirection; }
			set { rundirection = value; }
		}

		public Vector2 Pos
		{
			get { return pos; }
			set { pos = value; }
		}

		public Dictionary<EnemyVariables,string> Variables
		{
			get { return enemyvariables; }
			set { enemyvariables = value; }
		}

		public string Type
		{
			get { return eType; }
		}

		public AnimationManager Animations
		{
			get { return animations; }
			set { animations = value; }
		}

		#endregion

		public Enemy()
		{
			Random rand = new Random((int)DateTime.Now.Ticks);
			timeFromLastSearch = (float)rand.NextDouble() * MaxTimeFromLastSearch;
			particleManager = new ParticleSystem();
			number++;
			myNumber = number;
		}

		public bool Initialize(string type,IServiceProvider serviceProvider,byte threadNumber,Options options)
		{
			bool success = true;
			try
			{
				eType = type;
				InitializeEnemy(type,serviceProvider,options);
				numThread = threadNumber;

				
			}
			catch (Exception e)
			{
				DebugManager.AddItem("Failed to Initialize Enemy", "Enemy #" + number, new System.Diagnostics.StackTrace(), System.Drawing.Color.Red);
				
				success = false;
				FileManager.WriteInErrorLog(this, "Fail to Initialize Enemy: \n" + e.Message,e.GetType());
				errorMessage =("Failed To Initialize Enemy: " + e.Message);
			}
			initialized = success;
			return success;
		}

		public bool Initialize(IServiceProvider serviceProvider,Options options) 
		{
			bool succes = true;
			enemyvariables = new Dictionary<EnemyVariables, string>();
			//eType = "";
			InitializeEnemy(null, serviceProvider,options);
			return succes;
		}

		public void SetName(string name)
		{
			eType = name;
		}

		public void Update(GameTime gameTime,Quadtree quadtree,Vector2 playerPos,int numThread,Tile[,] map)
		{
			//try
			{
				UpdateEnemy(gameTime, quadtree,playerPos,numThread,map);
			}
			//catch (Exception e)
			{
				//errorMessage = ("Couldn't Update Enemy: " + e.Message);
			}
		}

		public void Update(GameTime gameTime)
		{
			try
			{
				UpdateEnemy(gameTime);
			}
			catch (Exception e)
			{
				DebugManager.AddItem("Failed to Update Enemx", "Enemy #" + number, new System.Diagnostics.StackTrace(), System.Drawing.Color.Red);
				
				FileManager.WriteInErrorLog(this, "Couln't Update Enemy " + Type + ":\n" + e.Message,e.GetType());
				errorMessage = ("Couldn't Update Enemy: " + e.Message);
			}
		}

		public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Matrix matrix, Dictionary<string, Texture2D> tex, RenderTarget2D target, RenderTarget2D resolvedTex)
		{
			try
			{
				DrawEnemy(gameTime, spriteBatch, matrix,tex,target,resolvedTex);
			}
			catch (Exception e)
			{
				DebugManager.AddItem("Failed to Draw Enemx", "Enemy #" + number, new System.Diagnostics.StackTrace(), System.Drawing.Color.Red);
			 
				FileManager.WriteInErrorLog(this, "Couln't Draw Enemy " + Type + ":\n" + e.Message, e.GetType());
				errorMessage = "Couldn't Draw Enemy: " +e.Message;
				spriteBatch.End();
			}
		}

		public void AddRectangle(string name,Texture2D tex, FrameRectangle newrect)
		{
			if (enemyvariables[EnemyVariables.AnimRectangles] == "")
				enemyvariables[EnemyVariables.AnimRectangles] = name;
			else
				enemyvariables[EnemyVariables.AnimRectangles] += "," + name;
			animations.AddRectangle(name, tex, newrect);
		}

		public void RemoveRectangle(string name)
		{
			enemyvariables[EnemyVariables.AnimRectangles] = enemyvariables[EnemyVariables.AnimRectangles].Replace(name + ",", "");
			enemyvariables[EnemyVariables.AnimRectangles] = enemyvariables[EnemyVariables.AnimRectangles].Replace("," + name, "");
			animations.RemoveRectangle(name);
		}

		public void ToFile()
		{
			try
			{
				string data = "";
				foreach (EnemyVariables key in enemyvariables.Keys)
				{
					data += key.ToString() + "=";
					data += enemyvariables[key] + ";\n";
				}
				FileManager.WriteFile(data, "Data/" + GameConstants.EnemiesPath + Type + "/" + Type + ".enemy");
			}
			catch (Exception e)
			{
				DebugManager.AddItem("Failed to write Enemx", "Enemy #" + number, new System.Diagnostics.StackTrace(), System.Drawing.Color.Red);
				FileManager.WriteInErrorLog(this, "Failed to Write Enemy:\n" + e.Message);   
			}
		}

		private void InitializeVariables()
		{
			if (!string.IsNullOrEmpty(enemyvariables[EnemyVariables.MovementType]))
				movementType = (MovementType)int.Parse(enemyvariables[EnemyVariables.MovementType]);
			else
				movementType = MovementType.Normal;
			if (!string.IsNullOrEmpty(enemyvariables[EnemyVariables.MaxSpeed]))
				maxSpeed = float.Parse(enemyvariables[EnemyVariables.MaxSpeed], CultureInfo.CreateSpecificCulture("en-us"));
			else
				maxSpeed = 0;
			if (!string.IsNullOrEmpty(enemyvariables[EnemyVariables.EnemyCollision]))
				collision = (EnemyCollision)int.Parse(enemyvariables[EnemyVariables.EnemyCollision]);
			else
				collision = EnemyCollision.Normal;
			if (!string.IsNullOrEmpty(enemyvariables[EnemyVariables.StandardDirection]))
				standardirection = (StandardDirection)int.Parse(enemyvariables[EnemyVariables.StandardDirection]);
			else
				standardirection = StandardDirection.Left;
			if (!string.IsNullOrEmpty(enemyvariables[EnemyVariables.StandardDirection]))
				rundirection = (StandardDirection)int.Parse(enemyvariables[EnemyVariables.StandardDirection]);
			else
				rundirection = StandardDirection.Left;
			string[] boundingBox = enemyvariables[EnemyVariables.BoundingBox].Split(',');
			if (boundingBox.Length >= 4)
			{
				int x = int.Parse(boundingBox[0]);
				int y = int.Parse(boundingBox[1]);
				int width = int.Parse(boundingBox[2]);
				int height = int.Parse(boundingBox[3]);
				orgcollisionrect = new Rectangle(x, y, width, height);
			}
			else
			{
				orgcollisionrect = new Rectangle();
			}

		}

		#region protected Methods

		protected void InitializeEnemy(string name,IServiceProvider serviceProvider,Options options)
		{
			alive = true;
			Content = new ContentManager(serviceProvider, "Data");
			tex = Content.Load<Texture2D>("Stuff/Blank");
			animations = new AnimationManager(serviceProvider);
			enemyvariables = new Dictionary<EnemyVariables, string>();
			int numParticles = 0;
			switch (options.QualitySettings[OptionsID.ParticleQuality])
			{ 
				case QualitySetting.High:
					numParticles = 100;
					break;
				case QualitySetting.Middle:
					numParticles = 50;
					break;
			}
			if (options.QualitySettings[OptionsID.ParticleQuality] != QualitySetting.Low)
			{
				particleManager.Initialize(false,SpawnType.Explosion,
					numParticles,
					5,
					10,
					Color.Red,
					Vector2.Zero,
					SpawnDirections.YPlusMinus,
					new Vector2(500),
					Content.Load<Texture2D>("Stuff/Blank"),
					GravityType.OverallForce,
					new Vector2(0, 1000),
					3,
					XNAParticleSystem.CollisionType.Collision);
				particleManager.debugPrefix = "Enemy Part.Sys. Name: " + name + " Nr. " + myNumber + " "; 
				options.InitObjectHolder.particleManagers[EGameState.Game].Add(particleManager);
			}
			//particleManager.UseCuda(true);
			wave = new Wave();
			wave.Initialize(serviceProvider, tex.GraphicsDevice, options);
			foreach (EnemyVariables variable in Enum.GetValues(typeof(EnemyVariables)))
			{
				enemyvariables.Add(variable, "");
			}
			if (name != null)
			{
				enemyvariables = FileManager.GetFileDict<EnemyVariables>("Data/" + GameConstants.EnemiesPath + name + "/" + name + ".enemy");
				Dictionary<Anims, string> anims;
				anims = FileManager.GetFileDict<Anims>("Data/" + GameConstants.EnemiesPath + name + "/" + "animation.anim");

				//Dictionary<CollisionAnims,string> canims = FileManager.GetFileDict<CollisionAnims>("Data/" + GameConstants.EnemiesPath + name + "/" + "animation.anim");
				animations.Initialize(anims, enemyvariables, name);
				InitializeVariables();
				gravity = new Vector2();
				InitializeKI();
			}
			else
				animations.Initialize();
		}

		protected void UpdateEnemy(GameTime gameTime, Quadtree quadtree,Vector2 playerPos,int numThread,Tile[,] map)
		{
			
			if (!alive)
			{
				timeTillDeath += (float)gameTime.ElapsedGameTime.TotalSeconds;
				if (timeTillDeath > 10)
					removable = true;
				//particleManager.Update(gameTime);
				wave.Update(gameTime, pos);
			}
			else
			{
				
				speed = new Vector2();
				gravity.Y += GameParameters.Gravity * (float)gameTime.ElapsedGameTime.TotalSeconds;
				UpdateRectangles(pos);
				speed.Y += gravity.Y;
				DoEnemyKI(gameTime, quadtree, playerPos, map);
				if (movementType == MovementType.Normal && !standXStill)
				{
					if (rundirection == StandardDirection.Left)
						speed.X = -maxSpeed;
					else
						speed.X = maxSpeed;
				}
				speed.Y += jumpSpeed.Y;
				if (jumping)
					animations.CurrentAnimation = Anims.Jump;
				else
					animations.CurrentAnimation = Anims.Walk;
				//#TODO : Speed
				Collision(quadtree, (float)gameTime.ElapsedGameTime.TotalSeconds, numThread);
				animations.Update(gameTime, !(rundirection == standardirection));
				pos += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
				UpdateRectangles(pos);
			}
		}
		public void UpdateEnemy(GameTime gameTime)
		{
			animations.Update(gameTime);
		}

		protected void DrawEnemy(GameTime gameTime, SpriteBatch spriteBatch, Matrix matrix, Dictionary<string, Texture2D> tex, RenderTarget2D target,RenderTarget2D resolvedTex)
		{
			if (!alive)
			{
				particleManager.Draw(spriteBatch, matrix);
				wave.CenterCoord = Vector2.Transform(pos, matrix);
				wave.DrawPostProcess(spriteBatch);
			}
			else
				animations.Draw(spriteBatch, matrix, pos, rundirection, standardirection, tex,target,resolvedTex);
			DrawKI(gameTime, spriteBatch,matrix);
		}

		/// <summary>
		/// Initializes the KillAnimation
		/// </summary>
		public void Kill(Quadtree quadtree,Matrix matrix)
		{
			alive = false;
			particleManager.SpawnPos = pos;
			//particleManager.UseTimedAlpha = true;
			//particleManager.Alpha = 1;
			DebugManager.AddItem("Enemy #" + myNumber + " killed", this.ToString(), new System.Diagnostics.StackTrace());
			particleManager.UseMinSpeed = true;
			particleManager.MinSpeed = new Vector2(300);
			if (speed.X < 0)
				particleManager.MaxSpeed = new Vector2(500, 500);
			else
				particleManager.MaxSpeed = new Vector2(-500, 500);
			Rectangle particlerect = new Rectangle((int)pos.X - 150, (int)pos.Y - 150, (int)pos.X + 300, (int)pos.Y + 300);
			List<Tile> collisions = quadtree.GetEnemyCollision(particlerect, numThread);
			Collisions = collisions;
			foreach (Tile collision in collisions)
				if (collision.TileColission_Type != TileCollision.Passable && collision.Standable)
					particleManager.AddCollisionRect(collision.get_rect);
			particleManager.StartExplosion();
			wave.Enabled = true;

			wave.StartResetEffect();
			wave.CenterCoord = Vector2.Transform(pos, matrix);
			wave.Disortion = 0.1f;
			wave.DistortionChange = -0.2f;
			wave.RadiusChange = 0.5f;
			wave.SizeChange = 0f;
			wave.MaxRadius = 0.2f;
			wave.Size = 0.02f;
		}

		#endregion

		#region ICloneable Member

		public object Clone()
		{
			//#TODO
			Enemy clone = (Enemy)this.MemberwiseClone();
			//throw new NotImplementedException();
			clone.Animations = (AnimationManager)animations.Clone();

			return clone;
		}

		#endregion

		#region IDisposable Member

		public void Dispose()
		{
			particleManager.Dispose();
			try
			{
				astar.Dispose();
			}
			catch (Exception)
			{
				
				//throw;
			}
			
		}

		#endregion
	}
}
