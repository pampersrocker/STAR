using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Star.Game.Level;
using Microsoft.Xna.Framework.Graphics;
using Star.GameManagement;
using Microsoft.Xna.Framework.Content;
using Star.Input;
using System.Globalization;

namespace Star.Game.Enemy
{
	public delegate void PlayerKillEventHandler(object sender, Enemy killer);

	public class EnemyManager : IDisposable
	{
		List<Enemy> allenemies;
		List<Enemy>[] enemies;
		IServiceProvider serviceProvider;
		Thread[] updater;
		int currentThreadtoAdd;
		int numThreads;
		bool finished;
		bool running;
		GameTime gameTime;
		Quadtree quadtree;
		Vector2 playerPos;
		Vector2 updatelength;
		Dictionary<string,Dictionary<string,Texture2D>> textures;
		ContentManager content;
		List<Enemy> deletingEnemies;
		Tile[,] map;

		RenderTarget2D target;
		RenderTarget2D resolvedTex;
		//ResolveTexture2D resolvedTex;
		//ResolveTexture2D resolvedEnemy;

		public event PlayerKillEventHandler PlayerKilled;

		public EnemyManager(IServiceProvider serviceProvider,Options options)
		{
			allenemies = new List<Enemy>();
			deletingEnemies = new List<Enemy>();
			updatelength = new Vector2(options.ScreenWidth, options.ScreenHeight);
			numThreads = Environment.ProcessorCount+1;
			if (numThreads > 1)
				numThreads--;
			this.serviceProvider = serviceProvider;
			enemies = new List<Enemy>[numThreads];
			updater = new Thread[numThreads];
			running = true;
			for (int i = 0; i < numThreads; i++)
			{
				enemies[i] = new List<Enemy>();
				updater[i] = new Thread(new ParameterizedThreadStart(UpdateThread));
				updater[i].Name = "EnemyThread#" + i.ToString();
				updater[i].Priority = ThreadPriority.BelowNormal;
			}
			textures = new Dictionary<string, Dictionary<string, Texture2D>>();
			content = new ContentManager(serviceProvider, "Data");
		}

		public List<Enemy> Enemies
		{
			get 
			{
				return allenemies;
			}
			
		}

		public void AddEnemy(string type, Vector2 position,Star.Game.Enemy.Enemy.StandardDirection startdirection,Options options)
		{
			Enemy newenemy;
			
			
			newenemy = new Enemy();
			newenemy.Initialize(type, serviceProvider,(byte)currentThreadtoAdd,options);
			//preloadedEnemies.Add(newenemy);
			newenemy.Pos = position;
			//newenemy.RunDirection = startdirection;
			if (!textures.ContainsKey(type))
			{
				LoadTextures(content, newenemy.Variables, type);
			}
			newenemy.Alive = true;
			enemies[currentThreadtoAdd].Add(newenemy);
			currentThreadtoAdd++;
			if (currentThreadtoAdd >= numThreads)
				currentThreadtoAdd = 0;
			refreshAllEnemies();
		}

		public void RemoveEnemy(Enemy enemy)
		{
			enemy.Dispose();
			foreach (List<Enemy> enemyList in enemies)
			{
				enemyList.Remove(enemy);
			}

			refreshAllEnemies();
		}

		public void Clear()
		{
			while (!IsFinished())
			{
				Thread.Sleep(0);
			}
			foreach (List<Enemy> listenemies in enemies)
			{
				listenemies.Clear();
			}
			allenemies.Clear();
		}

		private void LoadTextures(ContentManager content,Dictionary<Star.Game.Enemy.Enemy.EnemyVariables, string> dict, string name)
		{
			textures.Add(name, new Dictionary<string, Texture2D>());
			string path = GameConstants.EnemiesPath + name + "/";
			string[] rectangles;
			rectangles = dict[Enemy.EnemyVariables.AnimRectangles].Split(',');

			for (int i = 0; i < rectangles.Length; i++)
			{
				rectangles[i] = rectangles[i].Trim();
			}
			textures[name] = new Dictionary<string, Texture2D>();
			foreach (string rect in rectangles)
			{
				try
				{
					textures[name].Add(rect, content.Load<Texture2D>(path + rect));
				}
				catch
				{
					textures[name].Add(rect, content.Load<Texture2D>("Stuff/Error"));
				}
			}
		}

		public bool IsFinished()
		{
			finished = true;
			foreach (Thread thread in updater)
			{
				if (thread.ThreadState == ThreadState.Running)
					finished = false;
			}
			return finished;
		}

		public void Stop()
		{
			running = false;
			foreach (Thread thread in updater)
			{
				if (thread.ThreadState == ThreadState.Suspended)
					thread.Resume();
			}
		}

		private void UpdateThread(object number)
		{
			int num = (int)number;
			while (running)
			{
				
				foreach (Enemy enemy in enemies[num])
				{
					if((playerPos-enemy.Pos).Length() < updatelength.Length())
						enemy.Update(gameTime, quadtree, playerPos,num,map);
				}
				
				Thread.CurrentThread.Suspend();
			}
		}

		private void ClearEnemies()
		{
			if (IsFinished())
			{
				foreach (Enemy enemy in deletingEnemies)
				{
					enemy.Dispose();
					enemies[enemy.ThreadNumber].Remove(enemy);
				}
				deletingEnemies.Clear();
			}
		}

		public void Update(GameTime gameTime, Quadtree quadtree, Vector2 playerPos,Rectangle playerRect,Tile[,] map,Matrix matrix)
		{
			this.map = map;
			this.gameTime = gameTime;
			this.quadtree = quadtree;
			this.playerPos = playerPos;
			CheckIntersection(playerRect,matrix);
			ClearEnemies();
			for (int i = 0; i < updater.Length;i++)
			{
				//thread = new Thread(new ParameterizedThreadStart(UpdateThread));
				if (updater[i].ThreadState == ThreadState.Unstarted)
					updater[i].Start(i);
				if (updater[i].ThreadState == ThreadState.Suspended)
					updater[i].Resume();
			}
		}

		private void CheckIntersection(Rectangle rect,Matrix matrix)
		{
			
			foreach (List<Enemy> enemies1 in enemies)
			{
				foreach (Enemy enemy in enemies1)
				{
					if (enemy.Alive)
					{
						if (enemy.Animations.CurrentAnimationKeyframe.KillingRect.Rectangle.Intersects(rect))
						{
							//Throw killEvent
							if (PlayerKilled != null)
								PlayerKilled(this, enemy);
						}
						else
							if (enemy.Animations.CurrentAnimationKeyframe.DieRect.Rectangle.Intersects(rect))
							{
								enemy.Alive = false;
								enemy.Kill(quadtree,matrix);
								//enemies[enemy.ThreadNumber].Remove(enemy);
								//deletingEnemies.Add(enemy);
							}
						
					}
					if (enemy.Removable)
						deletingEnemies.Add(enemy);
				}
			}
		}

		private void refreshAllEnemies()
		{ 
			allenemies = new List<Enemy>();
			foreach (List<Enemy> enemies1 in enemies)
			{
				foreach (Enemy enemy in enemies1)
				{
					allenemies.Add(enemy);
				}
			}
		}

		public void Draw(GameTime gameTime,SpriteBatch spriteBatch, Matrix matrix,bool editor)
		{
			if (target == null)
				//target = new RenderTarget2D(
				//    spriteBatch.GraphicsDevice,
				//    370,//Editor Window Size
				//    500,// "		"	  "
				//    spriteBatch.GraphicsDevice.PresentationParameters.BackBufferCount,
				//    SurfaceFormat.Color,
				//    spriteBatch.GraphicsDevice.PresentationParameters.MultiSampleType,
				//    spriteBatch.GraphicsDevice.PresentationParameters.MultiSampleQuality
				//    );
			target = new RenderTarget2D(
				spriteBatch.GraphicsDevice,
				370,
				500,
				false,
				SurfaceFormat.Color,
				DepthFormat.Depth24,
				spriteBatch.GraphicsDevice.PresentationParameters.MultiSampleCount,
				RenderTargetUsage.DiscardContents);
				

			if (resolvedTex == null)
			//    resolvedTex = new ResolveTexture2D(spriteBatch.GraphicsDevice,
			//        spriteBatch.GraphicsDevice.PresentationParameters.BackBufferWidth,
			//        spriteBatch.GraphicsDevice.PresentationParameters.BackBufferHeight,
			//        spriteBatch.GraphicsDevice.PresentationParameters.BackBufferCount,
			//        spriteBatch.GraphicsDevice.PresentationParameters.BackBufferFormat);
				resolvedTex = new RenderTarget2D(spriteBatch.GraphicsDevice,
					spriteBatch.GraphicsDevice.PresentationParameters.BackBufferWidth,
					spriteBatch.GraphicsDevice.PresentationParameters.BackBufferHeight,
					false,
					spriteBatch.GraphicsDevice.PresentationParameters.BackBufferFormat,
					DepthFormat.Depth24,
					spriteBatch.GraphicsDevice.PresentationParameters.MultiSampleCount,
					RenderTargetUsage.DiscardContents);

			for (int i = 0; i < enemies.Length; i++)
			{
				foreach (Enemy enemy in enemies[i])
				{
					if ((playerPos - enemy.Pos).Length() < updatelength.Length() || editor)
						enemy.Draw(gameTime, spriteBatch, matrix,textures[enemy.Type],target,resolvedTex);
				}
			}
		}

		public string GetDataString()
		{
			string data = "";
			refreshAllEnemies();
			foreach (Enemy enemy in allenemies)
			{
				data += enemy.Type + "," + (int)enemy.Pos.X + "," + (int)enemy.Pos.Y + "," + (int)enemy.RunDirection + ":";
			}
			return data;
		}

		~EnemyManager()
		{
			content.Dispose();
			Stop();
		}



		#region IDisposable Member

		public void Dispose()
		{
			refreshAllEnemies();
			foreach (Enemy enemy in allenemies)
			{
				enemy.Dispose();
			}
			content.Dispose();
			Stop();
		}

		#endregion
	}
}
