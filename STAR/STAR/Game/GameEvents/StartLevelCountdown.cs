using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Star.Graphics.Effects.PostProcessEffects;

namespace Star.Game.GameEvents
{
	public class StartLevelCountdown : GameEvent
	{
		float elapsedTime;
		bool enabled;
		GrayScale grayScale;
		readonly float timeThreshold = 2;

		protected override bool CheckActvationEvent(GameTime gameTime, Rectangle playerBoundingBox, int timeLeft, Level.Quadtree quadtree, GameManagement.Gamestates.SGame stateGame)
		{
			if (!enabled)
			{
				enabled = true;
				return true;
			}
			else
				return enabled;
		}

		protected override DrawPosition InitializeEvent(IServiceProvider serviceProvider, GraphicsDevice device, Level.LevelVariables levelVariables, GameManagement.Options options)
		{
			elapsedTime = timeThreshold;
			grayScale = new GrayScale();
			grayScale.Initialize(serviceProvider, device, options);
			grayScale.StartResetEffect();
			grayScale.BFactor = 0.5f;
			grayScale.RFactor = 0.5f;
			grayScale.GFactor = 0.5f;
			return GameEvents.DrawPosition.Post;
		}

		protected override void DrawEvent(GameTime gameTime, SpriteBatch spriteBatch, Matrix matrix)
		{
			grayScale.DrawPostProcess(spriteBatch);
		}
			
		protected override EventAction UpdateEvent(GameTime gameTime, List<Enemy.Enemy> enemies)
		{
			EventAction action = EventAction.Nothing;
			elapsedTime -= gameTime.GetElapsedTotalSecondsFloat();
			if (elapsedTime < timeThreshold && elapsedTime > 0)
			{
				action = EventAction.BlockUpdate;
			}
			if (elapsedTime < timeThreshold/4)
			{
				grayScale.Alpha -= gameTime.GetElapsedTotalSecondsFloat()/(timeThreshold/4);
			}
			if (elapsedTime <=0)
			{
				Enabled = false;
			}

			return action;
		}

		public override void Start()
		{
		}

		public override void Dispose()
		{
			grayScale = null;
		}
	}
}
