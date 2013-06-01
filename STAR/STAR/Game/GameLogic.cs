using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Star.Game.Level;
using Star.Input;
using Star.Game.GameEvents;
using Star.GameManagement;
using Star.GameManagement.Gamestates;

namespace Star.Game
{
	public class GameLogic : IDisposable
	{
		int totalTime;
		int timeLeft;
		List<GameEvent> events;
		public GameLogic()
		{
			totalTime = 9999;
			timeLeft = totalTime;
			events = new List<GameEvent>();
		}

		public void Initialize(IServiceProvider serviceProvider,GraphicsDevice device, LevelVariables levelvariables, Options options)
		{
			GameEvent gameevent;
			gameevent = new ExitGameEvent();
			gameevent.Initialize(serviceProvider,device, levelvariables, options);
			gameevent.Enabled = false;
			events.Add(gameevent);
			gameevent = new KillEvent();
			gameevent.Initialize(serviceProvider,device, levelvariables, options);
			gameevent.Enabled = false;
			events.Add(gameevent);
			gameevent = new StartLevelCountdown();
			gameevent.Initialize(serviceProvider, device, levelvariables, options);
			events.Add(gameevent);
		}

		public List<EventAction> Update(GameTime gameTime, Rectangle playerBoundingBox, Quadtree quadtree, List<Enemy.Enemy> enemies, SGame stateGame)
		{
			List<EventAction> actions = new List<EventAction>();

			ChecEventActivation(gameTime, playerBoundingBox,quadtree,stateGame);
			UpdateEvents(gameTime, playerBoundingBox, ref actions, enemies);

			return actions;
		}

		private void ChecEventActivation(GameTime gameTime, Rectangle playerBoundingBox, Quadtree quadtree, SGame stateGame)
		{
			foreach (GameEvent gamevent in events)
			{
				gamevent.CheckActivation(gameTime, playerBoundingBox, timeLeft,quadtree,stateGame);
			}
		}

		private void UpdateEvents(GameTime gameTime, Rectangle playerBoundingBox,ref List<EventAction> actions,List<Enemy.Enemy> enemies)
		{ 
			foreach(GameEvent gameevent in events)
			{
				actions.Add(gameevent.Update(gameTime,enemies));
			}
		}

		public void Draw(DrawPosition position, GameTime gameTime, SpriteBatch spritebatch,Matrix matrix)
		{
			foreach (GameEvent ge in events)
			{
				if (ge.DrawPosition == position)
				{
					ge.Draw(gameTime, spritebatch, matrix);
				}
			}
		}

		public void PlayerKilled(object sender, Enemy.Enemy killer)
		{
			foreach (GameEvent killevent in events)
			{
				if (killevent.GetType() == typeof(KillEvent))
				{
					((KillEvent)killevent).PlayerPos = killer.Pos;
					killevent.Enabled = true;
					killevent.Start();
				}
			}
		}



		#region IDisposable Member

		public void Dispose()
		{
			foreach (GameEvent myevent in events)
				myevent.Dispose();
		}

		#endregion
	}
}
