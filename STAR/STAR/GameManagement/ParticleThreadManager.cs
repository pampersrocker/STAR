using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Star.Game.Debug;

namespace Star.GameManagement
{
	public class ParticleThreadManager: IExtendedDisposable
	{
		Thread particleThread;
		bool isRunning;
		bool running;
		List<List<string>> debugStrings;
		XNAParticleSystem.ParticleManager particleManager;
		GameTime gameTime;
		Barrier barrier;

		public ParticleThreadManager()
		{
			particleThread = new Thread(new ThreadStart(Run));
			running = true;
			barrier = new Barrier(2);
			debugStrings = new List<List<string>>();
		}


		protected void Run()
		{
			while (running)
			{
				particleManager.Update(gameTime);
				barrier.SignalAndWait();
			}
		}

		public void Update(GameTime gameTime, XNAParticleSystem.ParticleManager particleManager)
		{
			this.particleManager = particleManager;
			this.gameTime = gameTime;
			if (particleThread.ThreadState == ThreadState.Unstarted)
			{
				particleThread.Start();
			}
			else
			{
#if DEBUGPARTICLESYSTEM
				debugStrings.Add(particleManager.debugStrings);
				foreach (string item in debugStrings[debugStrings.Count-1])
				{

					DebugManager.AddItem(item, this.ToString(), new System.Diagnostics.StackTrace(), System.Drawing.Color.Aqua); 

				}
#endif
				particleManager.debugStrings = new List<string>();
				while (barrier.ParticipantsRemaining > 2)
				{
					barrier.RemoveParticipant();
				}
				barrier.SignalAndWait();
			}
			
		}



		#region IExtendedDisposable Member

		public bool Disposed
		{
			get { return !running; }
		}

		#endregion

		#region IDisposable Member

		public void Dispose()
		{
			running = false;
			try
			{
				particleThread.Abort();
			}
			catch (ThreadAbortException)
			{

				
			}
		}

		#endregion
	}
}
