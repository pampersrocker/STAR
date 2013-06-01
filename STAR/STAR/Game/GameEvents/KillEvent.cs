using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Star.Game.Level;
using Star.GameManagement;
using Star.Graphics.Effects.PostProcessEffects;
using Star.GameManagement.Gamestates;
using XNAParticleSystem;
using Star.Components;

namespace Star.Game.GameEvents
{
	class KillEvent : GameEvent
	{
		float time;
		KillEffect killeffect;
		Trace traceEffect;
		int screenwidth;
		Texture2D blank;
		Rectangle rect;
		Rectangle frontrect;
		Vector2 playerPos;
		Camera camera;
		Vector2 startCameraOffset;
		ParticleSystem manager;
		DrawableText gameOverText;
		const string gameOver = "Game Over!";
		const float LetterTime = 0.1f;
		float shakeDivider;
		float gameOveralpha = 0;
		float blackScreenAlpha;
		Color blackScreenColor;
		Vector2 coord;
		ToonShader toonShader;
		Options options;

		public Vector2 PlayerPos
		{
			get { return playerPos; }
			set { playerPos = value; }
		}


		protected override bool CheckActvationEvent(GameTime gameTime, Rectangle playerBoundingBox, int timeLeft, Quadtree quadtree, SGame stateGame)
		{
			camera = stateGame.Camera;
			playerPos = new Vector2(playerBoundingBox.Center.X, playerBoundingBox.Center.Y);
			bool activation = Enabled;
			List<Tile> killtiles = quadtree.GetCollision(playerBoundingBox);
			foreach (Tile tile in killtiles)
			{
				if (tile.TileType == TileType.Spike)
				{
					Rectangle rect = tile.get_rect;
					rect.Y += 12;
					rect.Height -= 12;
					if (playerBoundingBox.Intersects(rect))
					{
						activation = true;
						
						if (!traceEffect.Enabled)
						{
							ActivateEffect();
						}
						break;
					}
				}
			}
			return activation;
		}

		private void ActivateEffect()
		{
			startCameraOffset = camera.Offset;
			killeffect.StartResetEffect();
			traceEffect.Enabled = true;
			int numParticles = 0;
			switch (options.QualitySettings[OptionsID.ParticleQuality])
			{ 
				case QualitySetting.High:
					numParticles = 2000;
					break;
				case QualitySetting.Middle:
					numParticles = 1000;
					break;
			}
			if (options.QualitySettings[OptionsID.ParticleQuality] != QualitySetting.Low)
			{
				manager.Initialize(false, SpawnType.Fontaine,
					numParticles,
					numParticles / 20,
					10,
					Color.Red,
					PlayerPos,
					SpawnDirections.AllWays,
					new Vector2(300),
					Content.Load<Texture2D>("Stuff\\Blank"),
					GravityType.OverallForce,
					Vector2.Zero,
					1,
					XNAParticleSystem.CollisionType.None);
				manager.debugPrefix = "Player Particle System";
				manager.Enabled = true;
				manager.Reset();
				options.InitObjectHolder.particleManagers[EGameState.Game].Add(manager);
				
			}
			toonShader.Enabled = true;
			
			
			toonShader.Alpha = 0;
			//gameOverText.Text = "Game Over!";
		}

		protected override DrawPosition InitializeEvent(IServiceProvider serviceProvider,GraphicsDevice device,LevelVariables levelVariables, Options options)
		{
			this.options = options;
			blank = Content.Load<Texture2D>("Stuff/Blank");
			killeffect = new KillEffect();
			killeffect.Initialize(serviceProvider, device, options);
			traceEffect = new Trace();
			traceEffect.Initialize(serviceProvider, device, options);
			rect = new Rectangle(0, 0, options.ScreenWidth, options.ScreenHeight);
			screenwidth = options.ScreenWidth;
			coord = new Vector2(options.ScreenWidth / 2, options.ScreenHeight / 2 + options.ScreenHeight * 0.35f);
			frontrect = new Rectangle(0, 0, options.ScreenWidth, options.ScreenHeight);
			shakeDivider = 10;
			manager = new ParticleSystem();
			gameOverText.font = Content.Load<SpriteFont>("Stuff\\Font");
			gameOverText.Text = gameOver;
			gameOverText.color = new Color(1f,1f,1f, 0);
			blackScreenColor = new Color(0f,0f,0f, 0);
			toonShader = new ToonShader();
			toonShader.Initialize(serviceProvider, device, options);
			toonShader.CurrentTechnique = ToonShaderEffects.BrightEdgesColored;
			return DrawPosition.Post;
		}

		protected override void DrawEvent(GameTime gameTime, SpriteBatch spriteBatch, Matrix matrix)
		{
			//spriteBatch.Begin();
			//spriteBatch.Draw(blank, frontrect, new Color(Color.Red, alpha));
			//spriteBatch.Draw(blank,rect,new Color(Color.Black,(byte)255));
			//spriteBatch.Draw(blank, rect2, Color.Black);
			
			//spriteBatch.End();
			////wave.Draw(spriteBatch, spriteBatch.GraphicsDevice);
			//killeffect.DrawPostProcess(spriteBatch, spriteBatch.GraphicsDevice);
			manager.Draw(spriteBatch,matrix);
			//traceEffect.DrawPostProcess(spriteBatch, spriteBatch.GraphicsDevice);
			toonShader.DrawPostProcess(spriteBatch);
			gameOverText.CenterText(new Vector2(
				spriteBatch.GraphicsDevice.PresentationParameters.BackBufferWidth/2,
				spriteBatch.GraphicsDevice.PresentationParameters.BackBufferHeight/2));
			if (time >= 1.5f)
			{
				spriteBatch.Begin( SpriteSortMode.Texture, BlendState.NonPremultiplied);
				if (time >= 3)
					spriteBatch.Draw(blank, rect, blackScreenColor);
				spriteBatch.DrawString(gameOverText.font, gameOverText.Text, gameOverText.pos, gameOverText.color);
				
				spriteBatch.End();
			}
			
			
		}

		protected override EventAction UpdateEvent(GameTime gameTime,List<Enemy.Enemy> enemies)
		{
			time += (float)gameTime.ElapsedGameTime.TotalSeconds;
			if (traceEffect.Alpha < 1)
				traceEffect.Alpha += (float)gameTime.ElapsedGameTime.TotalSeconds * 1;
			else
				traceEffect.Alpha = 1;
			camera.Offset = startCameraOffset + new Vector2((float)Math.Sin(time * 50),(float) Math.Cos(time * 30)) * 10/shakeDivider;
			camera.Rotation = (float)Math.Sin(time * 10) * 0.1f;
			shakeDivider -= shakeDivider * (float)gameTime.ElapsedGameTime.TotalSeconds*10;
			if (shakeDivider < 1)
				shakeDivider = 1;
			//manager.Update(gameTime);
			//killeffect.Update(gameTime, Vector2.Zero);
			//alpha += 1 * (float)gameTime.ElapsedGameTime.TotalSeconds;
			//alpha = MathHelper.Clamp(alpha, 0, 0.5f);
			//rect.X -= (int)((rect.X) * (float)gameTime.ElapsedGameTime.TotalSeconds)/2;
			//rect2.X += (int)((-rect2.X) * (float)gameTime.ElapsedGameTime.TotalSeconds) / 2;
			toonShader.Alpha += (float)gameTime.ElapsedGameTime.TotalSeconds*2;
			if (time > 1.5f)
			{
				
				gameOveralpha += (float)gameTime.ElapsedGameTime.TotalSeconds;
				gameOverText.color.A = (byte)MathHelper.Clamp(gameOveralpha*255, 0, 255);
			}
			if (time > 3)
			{
				blackScreenAlpha += (float)gameTime.ElapsedGameTime.TotalSeconds;
				blackScreenAlpha = MathHelper.Clamp(blackScreenAlpha, 0,1);
				blackScreenColor.A = (byte)(blackScreenAlpha * 255);
			}
			if (time > 5)
			{
				return EventAction.Exit;
			}
			return EventAction.BlockInput;
		}

		public override void Start()
		{
			if (!killeffect.Enabled)
				killeffect.StartResetEffect();
			if (!traceEffect.Enabled)
			{
				ActivateEffect();
			}
			Enabled = true;

		}

		public override void Dispose()
		{
			manager.Dispose();
		}
	}
}
