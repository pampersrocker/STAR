using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Threading;
using Microsoft.Xna.Framework.Graphics;

namespace XNAParticleSystem
{
	public class ParticleManager : IList<ParticleSystem>,IDisposable
	{
		List<ParticleSystem> particleSystems;
		bool enabled;
		object _locker;
		public double milliseconds_needed;
		public double drawMilliseconds_needed;
		public List<string> debugStrings;

		public ParticleManager()
		{
			particleSystems = new List<ParticleSystem>();
			enabled = true;
			_locker = new object();
			debugStrings = new List<string>();
		}

		public void Stop()
		{
			if (particleSystems != null)
			{
				foreach (ParticleSystem system in particleSystems)
				{
					while (system.IsRunning)
					{
						system.Dispose();
					}
				} 
			}
			//enabled = false;
		}

		public void Update(GameTime gameTime)
		{
			if (enabled)
			{
				milliseconds_needed = 0;
				DateTime start = DateTime.Now;
				int dbg = 1;
				foreach (ParticleSystem system in particleSystems)
				{
					
					if (system.Initialized && system.Enabled)
					{
						debugStrings.Add("PARTICLE SYSTEM: Updating " + system.debugPrefix + "(System " + dbg + ")");
						system.Update(gameTime);
						if (!system.CudaEnabled)
						{
							system.barrier.SignalAndWait();
						
							WaitHandle.WaitAll(system.resetEvents);
							
						}
					}
					//tempSecs = 0;
					//for (int i = 0; i < system.duration.Length; i++)
					//{
					//    tempSecs += system.duration[i].TotalMilliseconds;
					//}
					//tempSecs /= Environment.ProcessorCount;
					dbg++;
				}

				milliseconds_needed = (DateTime.Now - start).TotalMilliseconds; 
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
				DateTime start = DateTime.Now;
				foreach (ParticleSystem system in particleSystems)
				{
					system.Draw(spriteBatch);
				}
				drawMilliseconds_needed = (DateTime.Now - start).TotalMilliseconds; 
			
		}

		public void Draw(SpriteBatch spriteBatch,Matrix matrix)
		{
			DateTime start = DateTime.Now;
			foreach (ParticleSystem system in particleSystems)
			{
				system.Draw(spriteBatch,matrix);
			}
			drawMilliseconds_needed = (DateTime.Now - start).TotalMilliseconds;
		}

		public void ResetAllSystems()
		{
			for (int i = 0; i < particleSystems.Count; i++)
			{
				particleSystems[i].Reset();
			}
		}

		public void StartAllExplosions()
		{
			for (int i = 0; i < particleSystems.Count; i++)
			{
				particleSystems[i].StartExplosion();
			}
		}

		#region IList<ParticleSystem> Member

		public int IndexOf(ParticleSystem item)
		{
			return particleSystems.IndexOf(item);
		}

		public void Insert(int index, ParticleSystem item)
		{
			particleSystems.Insert(index, item);
		}

		public void RemoveAt(int index)
		{
			particleSystems.RemoveAt(index);
		}

		public ParticleSystem this[int index]
		{
			get
			{
				return particleSystems[index];
			}
			set
			{
				particleSystems[index] = value;
			}
		}

		#endregion

		#region ICollection<ParticleSystem> Member

		public void Add(ParticleSystem item)
		{
			particleSystems.Add(item);
		}

		public void Clear()
		{
			particleSystems.Clear();
		}

		public bool Contains(ParticleSystem item)
		{
			return particleSystems.Contains(item);
		}

		public void CopyTo(ParticleSystem[] array, int arrayIndex)
		{
			particleSystems.CopyTo(array, arrayIndex);
		}

		public int Count
		{
			get { return particleSystems.Count; }
		}

		public bool IsReadOnly
		{
			get { return false; }
		}

		public bool Remove(ParticleSystem item)
		{
			return particleSystems.Remove(item);
		}

		#endregion

		#region IEnumerable<ParticleSystem> Member

		public IEnumerator<ParticleSystem> GetEnumerator()
		{
			return particleSystems.GetEnumerator();
		}

		#endregion

		#region IEnumerable Member

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return particleSystems.GetEnumerator();
		}

		#endregion

		#region IDisposable Member

		public void Dispose()
		{
			Stop();
			if (particleSystems!=null)
			{

				particleSystems.Clear();

			} 
			particleSystems = null;
			_locker = null;

		}

		#endregion
	}
}
