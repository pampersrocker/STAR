using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNAParticleSystem
{

	public enum SpawnSource
	{ 
		Point,
		Rectangle
	}

	public enum SpawnType
	{ 
		Explosion,
		Fontaine
	}

	public enum SpawnDirections
	{ 
		Normal,
		XPlusMinus,
		YPlusMinus,
		AllWays,
		Angle
	}

	public enum GravityType
	{ 
		OverallForce,
		Point,
		Newton
	}

	public enum CollisionType
	{ 
		Collision,
		None
	}

	public struct NewtonMass
	{
		public Vector2 center;
		public Vector2 velocity;
		public float weight;
	}

	public class ParticleSystem
	{
		Vector2[] positions;
		Vector2[] velocities;
		float[] lifetimes;
		Rectangle[] rects;
		float[] Rotations;
		byte[] alphas;
		float[] floatalphas;

		float particleMass=1;

		
 
		//Particle[] particles;
		List<Rectangle> collisionrectangles;
		Vector2 spawn;
		Vector2 gravity = new Vector2(100,100f);
		List<NewtonMass> masses;
		GravityType gravtype;
		SpawnType spawntype;
		CollisionType collisiontype;

		
		SpawnDirections spawndirections;

		
		SpawnSource spawnsource;
		Vector2 spawnend;
		bool fixedMasses = true;

		

		
		Texture2D particletex;
		Random rand;
		Thread[] updatethread;
		public Barrier barrier;
		public ManualResetEvent[] resetEvents;
		public string debugPrefix="";
		GameTime gametime;
		Vector2 maxspeed;
		ParticleCUDA particleCuda;
		ParticleOptions options;

		object _locker;
		public TimeSpan[] duration;
		int size = 3;
		float spawnperS;
		float newSpawners;
		long newSpawnCount;
		float totallifetime = 5;
		float spawnAngle;
		Color stdcolor = Color.White;
		float friction = 2f;

		
		bool running = false;
		int numThreads = 3;
		int numParticles;
		bool[] newframe;
		bool initialized;

		
		bool timeAlpha = true;
		float alpha;
		float airFriction;
		Vector2 minSpeed;
		bool useminSpeed;

		bool useCuda;
		bool enabled;

		bool BetweenParticleCollision = false;


		

		#region Getters,Setters

		public bool BetweenParticleCollision1
		{
			get { return BetweenParticleCollision; }
			set { BetweenParticleCollision = value; }
		}

		public bool Enabled
		{
			get { return enabled; }
			set
			{
				if (value)
				{
					if (initialized)
					{

						enabled = value;
					}
				}
				else
				{
					enabled = value;
				}
			}
		}

		public float ParticleMass
		{
			get { return particleMass; }
			set { particleMass = value; }
		}

		public CollisionType Collisiontype
		{
			get { return collisiontype; }
			set { collisiontype = value; }
		}

		public bool FixedMasses
		{
			get { return fixedMasses; }
			set { fixedMasses = value; }
		}

		public bool Initialized
		{
			get { return initialized; }
		}

		public float Friction
		{
			get { return friction; }
			set { friction = value; }
		}

		public bool CudaEnabled
		{
			get { return useCuda; }
		}

		/// <summary>
		/// Gets or Sets the ParticleLifetime
		///  (only by Explosion)
		/// </summary>
		public float ParticleLifetime
		{
			get { return totallifetime; }
			set { totallifetime = value; }
		}

		public SpawnDirections Spawndirections
		{
			get { return spawndirections; }
			set { spawndirections = value; }
		}

		public float SpawnAngle
		{
			get { return spawnAngle; }
			set { spawnAngle = value; }
		}

		public bool UseMinSpeed
		{
			get { return useminSpeed; }
			set { useminSpeed = value; }
		}

		public Vector2 MinSpeed
		{
			get { return minSpeed; }
			set
			{
				if (value != null)
					minSpeed = value;
			}
		}

		public Vector2 MaxSpeed
		{
			get { return maxspeed; }
			set { maxspeed = value; }
		}

		public TimeSpan[] CalcTime
		{
			get { return duration; }
		}

		public bool IsRunning
		{
			get { return running; }
			set { running = value; }
		}

		public bool UseTimedAlpha
		{
			get { return timeAlpha; }
			set { timeAlpha = value; }
		}

		public float Alpha
		{
			get { return alpha; }
			set { alpha = (float)MathHelper.Clamp(value, 0, 1); }
		}

		public float AirFriction
		{
			get { return airFriction; }
			set { airFriction = MathHelper.Clamp(value, 0, float.MaxValue); }
		}

		public int NrParticlesAlive
		{
			get
			{
				int particlesAlive = 0;
				for (int i = 0; i < numParticles; i++)
				{
					if (lifetimes[i] < totallifetime)
					{
						particlesAlive++;
					}
				}
				return particlesAlive;

			}
		}

		public Vector2 SpawnPos
		{
			get { return spawn; }
			set { spawn = value; }
		}

		public Vector2 Spawnend
		{
			get { return spawnend; }
			set { spawnend = value; }
		}

		public Vector2 Gravity
		{
			get { return gravity; }
			set { gravity = value; }
		}

		public GravityType Type
		{
			get { return gravtype; }
			set { gravtype = value; }
		} 
		#endregion

		public ParticleSystem()
		{
			_locker = new object();
			duration = new TimeSpan[1];
			duration[0] = new TimeSpan();
			masses = new List<NewtonMass>();
			collisionrectangles = new List<Rectangle>();
			collisionrectangles.Add(new Rectangle(-500000000, 0, 0, 0));
			if (Environment.ProcessorCount > 1)
				numThreads = Environment.ProcessorCount;
			else
				numThreads = 1;
			updatethread = new Thread[numThreads];
			newframe = new bool[numThreads];
			resetEvents = new ManualResetEvent[numThreads];
			for (int i = 0; i < numThreads; i++)
			{
				updatethread[i] = new Thread((new ParameterizedThreadStart(UpdateThread)));
				updatethread[i].Name = "ParticleThread#" + i.ToString();
				updatethread[i].Priority = ThreadPriority.Lowest;
				resetEvents[i] = new ManualResetEvent(false);
			}
			barrier = new Barrier(numThreads + 1);
			
			//particles = new Particle[1];
			positions = new Vector2[1];
			velocities = new Vector2[1];
			lifetimes = new float[1];
			rects = new  Rectangle[1];
			Rotations = new float[1];
			alphas = new byte[1];
			floatalphas = new float[1];
			minSpeed = new Vector2();
			InitalizeCUDA();
		}

		#region Initialize
		/// <summary>
		/// Initializes the particle System. 
		/// The Particle will spawn at the specified POINT with the defined SpawnType, 
		/// with the specified Directions, speed and the specified Gravity
		/// </summary>
		/// <param name="fontaine">Determines the spawnType:
		/// Fontain: The particles will spawn continously at the specified Point
		/// Explosion: The particles will spawn all at once at the specified Point</param>
		/// <param name="pnumParticles">Maximum particle number</param>
		/// <param name="particle_s">Spawntype:
		/// Fontain: The Particle per second (must be >= 60 to spawn any particles), also controls the particle lifetime = numParticles/particle_s
		/// Explosion: The particles Lifetime</param>
		/// <param name="Size">The Size of the Particles</param>
		/// <param name="pStdColor">The Color of the Particles</param>
		/// <param name="src">The Point where the particles are going to spawn</param>
		/// <param name="pdirections">Determines the Directions in which the particles are going to go</param>
		/// <param name="maxSpeed">The Maxmimum x and y speed</param>
		/// <param name="tex">The Texture of the particles</param>
		/// <param name="Gravity">The Gravity Type.
		/// OverallForce: all particles will be forced in that direction
		/// Point: All particles are going to move to that direction
		/// Newton: All particles are going to have planetary Gravity, Masses can be defined with the AddNewtonMass() method</param>
		/// <param name="gravAim">
		/// Gravity Type:
		/// Overall Force: The force which will be applied.
		/// Point: The Point, the particles are heading to.
		/// Newton: No affect
		/// </param>
		/// <param name="pfriction">Particles speed will bei divided by this when they collided with sth.</param>
		/// <param name="collison">Defines if the particles can collide or not</param>
		public void Initialize(bool bla, SpawnType fontaine, int pnumParticles, float particle_s, int Size, Color pStdColor, Vector2 src,
			SpawnDirections pdirections, Vector2 maxSpeed, Texture2D tex, GravityType Gravity, Vector2 gravAim,
			float pfriction, CollisionType collison)
		{
			Initialize(false,
				fontaine,
				pnumParticles,
				particle_s,
				Size,
				pStdColor,
				src,
				src,
				pdirections,
				maxSpeed,
				tex,
				Gravity,
				gravAim,
				pfriction,
				collison);
		}

		/// <summary>
		/// Initializes a Default Particle System with <c>numParticles</c> Particles and no Startspeed or Gravity
		/// </summary>
		/// <param name="numParticles">The Number of Particles</param>
		/// <param name="lifeTime"><c>type</c>=Explosion: LifeTime in seconds
		/// <c>type</c> =Fontaine: Particle spawning per second</param>
		/// <param name="type"></param>
		/// <param name="Size"></param>
		/// <param name="tex"></param>
		/// <param name="src"></param>
		public void Initialize(bool bla, int numParticles, float lifeTime, SpawnType type, int Size, Texture2D tex, Vector2 src)
		{
			Initialize(false,
				type,
				numParticles,
				lifeTime,
				Size,
				Color.White,
				src,
				SpawnDirections.Normal,
				Vector2.Zero,
				tex,
				GravityType.OverallForce,
				Vector2.Zero,
				1,
				CollisionType.None);
		}

		/// <summary>
		/// Initializes the particle System. 
		/// The Particle will spawn at the specified RECTANGLE with the defined SpawnType, 
		/// with the specified Directions, speed and the specified Gravity
		/// </summary>
		/// <param name="fontaine">Determines the spawnType:
		/// Fontain: The particles will spawn continously at the specified Point
		/// Explosion: The particles will spawn all at once at the specified Point</param>
		/// <param name="pnumParticles">Maximum particle number</param>
		/// <param name="particle_s">Spawntype:
		/// Fontain: The Particle per second (must be >= 60 to spawn any particles), also controls the particle lifetime = numParticles/particle_s
		/// Explosion: The particles Lifetime</param>
		/// <param name="Size">The Size of the Particles</param>
		/// <param name="pStdColor">The Color of the Particles</param>
		/// <param name="startpos">The Startpoint of the Rectangle where the particles are going to spawn</param>
		/// <param name="endpos">The Endpos of the Rectangle where the particles are going to spawn</param>
		/// <param name="pdirections">Determines the Directions in which the particles are going to go</param>
		/// <param name="maxSpeed">The Maxmimum x and y speed</param>
		/// <param name="tex">The Texture of the particles</param>
		/// <param name="Gravity">The Gravity Type.
		/// OverallForce: all particles will be forced in that direction
		/// Point: All particles are going to move to that direction
		/// Newton: All particles are going to have planetary Gravity, Masses can be defined with the AddNewtonMass() method</param>
		/// <param name="gravAim">
		/// Gravity Type:
		/// Overall Force: The force which will be applied.
		/// Point: The Point, the particles are heading to.
		/// Newton: No affect
		/// </param>
		/// <param name="pfriction">Particles speed will bei divided by this when they collided with sth.</param>
		/// <param name="collison">Defines if the particles can collide or not</param>
		public void Initialize(bool bla, SpawnType fontaine, int pnumParticles, float particle_s, int Size, Color pStdColor, Vector2 startpos, Vector2 endpos,
	SpawnDirections pdirections, Vector2 maxSpeed, Texture2D tex, GravityType Gravity, Vector2 gravAim,
	float pfriction, CollisionType collison)
		{
			if (startpos != endpos)
				spawnsource = SpawnSource.Rectangle;
			else
				spawnsource = SpawnSource.Point;

			masses = new List<NewtonMass>();
			collisiontype = collison;
			spawntype = fontaine;
			gravtype = Gravity;
			spawndirections = pdirections;

			size = Size;

			rand = new Random((int)DateTime.Now.Ticks);
			stdcolor = pStdColor;
			spawn = startpos;
			spawnend = endpos;
			maxspeed = maxSpeed;
			spawnperS = particle_s;
			numParticles = pnumParticles;
			positions = new Vector2[numParticles];
			velocities = new Vector2[numParticles];
			lifetimes = new float[numParticles];
			rects = new Rectangle[numParticles];
			Rotations = new float[numParticles];
			alphas = new byte[numParticles];
			floatalphas = new float[numParticles];
			collisionrectangles = new List<Rectangle>();
			collisionrectangles.Add(new Rectangle(-500000000, 0, 0, 0));
			particletex = tex;
			gravtype = Gravity;
			friction = MathHelper.Clamp(pfriction, float.Epsilon, float.MaxValue);
			gravity = gravAim;
			duration = new TimeSpan[numThreads];
			if (fontaine == SpawnType.Fontaine)
			{
				totallifetime = pnumParticles / particle_s;
			}
			else
			{
				totallifetime = particle_s;
			}
			for (int i = 0; i < pnumParticles; i++)
			{
				ResetParticle(i);
				lifetimes[i] = totallifetime + 1;
			}
			running = true;
			updatethread = new Thread[numThreads];
			newframe = new bool[numThreads];
			for (int i = 0; i < numThreads; i++)
			{
				updatethread[i] = new Thread((new ParameterizedThreadStart(UpdateThread)));
				updatethread[i].Name = "ParticleThread#" + i.ToString();
			}
			initialized = true;
			airFriction = 1;

			newSpawners = 0;
			InitalizeCUDA();
		} 
		#endregion

		private void InitalizeCUDA()
		{
			particleCuda = new ParticleCUDA();
			options = new ParticleOptions();
			particleCuda.Initalize(CreateHolder(), CreateOptions());
		}

		private unsafe ParticleOptions CreateOptions(float elapsed)
		{

			ParticleOptions options = new ParticleOptions();
			options.AirFriction = AirFriction;
			options.collisionRectSize = collisionrectangles.Count;
			options.collisionType = collisiontype;
			options.elapsedGameTime = elapsed;
			options.Friction = friction;
			options.gravity = gravity;
			options.gravType = gravtype;
			fixed (NewtonMass* fmasses = masses.ToArray())
			{
				options.mass = new IntPtr((void*)fmasses);
			}
			options.newtonMassSize = masses.Count;
			options.size = size;
			options.totalLifeTime = totallifetime;
			return options;
		}

		private ParticleArrayHolder CreateHolder()
		{
			ParticleArrayHolder holder = new ParticleArrayHolder();
			holder.alphas = alphas;
			holder.lifetimes = lifetimes;
			holder.positions = positions;
			holder.rects = rects;
			holder.Rotations = Rotations;
			holder.velocities = velocities;
			return holder;
		}

		private unsafe ParticleOptions CreateOptions()
		{
			return CreateOptions(0);
		}

		/// <summary>
		/// Adds a newton mass to the particle system
		/// </summary>
		/// <param name="masscenter">The Center of the Mass</param>
		/// <param name="weight">The weight of the mass</param>
		public void AddNewtonMass(Vector2 masscenter,float weight)
		{
			bool running = false;
			foreach (Thread thread in updatethread)
				if (thread.ThreadState == ThreadState.Running)
					running = true;
			if (!running)
			{
				NewtonMass newMass;
				newMass.center = masscenter;
				newMass.weight = weight;
				newMass.velocity = Vector2.Zero;
				masses.Add(newMass);
			}
		}

		/// <summary>
		/// Resets all particles (also Starts the Explosion)
		/// </summary>
		public void Reset()
		{
			if (initialized)
			{
				for (int i = 0; i < numParticles; i++)
				{
					ResetParticle(i);
					if (spawntype == SpawnType.Fontaine)
					{
						lifetimes[i] = totallifetime;
					}
				}
				enabled = true;
				particleCuda.Reset(CreateHolder(), options); 
			}
		}

		/// <summary>
		/// Starts the Explosion
		/// </summary>
		public void StartExplosion()
		{
			if (initialized)
			{
				if (spawntype == SpawnType.Explosion)
				{
					Reset();
				}
				enabled = true;
			}
		}

		public void Update(GameTime gameTime)
		{
			if (initialized && enabled)
			{
				if (!fixedMasses)
				{
					for (int i = 0; i < masses.Count; i++)
					{
						masses[i] = new NewtonMass() { center = masses[i].center + (masses[i].velocity), velocity = masses[i].velocity, weight = masses[i].weight };

					}
				}
				
				gametime = gameTime;

				if (spawntype == SpawnType.Fontaine)
				{
					newSpawners -= newSpawners - newSpawnCount;
					newSpawners += (spawnperS * (float)gametime.ElapsedGameTime.TotalSeconds);
					newSpawnCount = (long)newSpawners;
				}


				if (!useCuda)
				{
					for (int i = 0; i < numThreads; i++)
					{
						if (updatethread[i].ThreadState == ThreadState.Unstarted)
							updatethread[i].Start(i);
						newframe[i] = true;
					}
				}
				else
				{
					DateTime start = DateTime.Now;
					ParticleOptions options = CreateOptions((float)gametime.ElapsedGameTime.TotalSeconds);
					particleCuda.Run(CreateHolder(),options);
					duration[0] = DateTime.Now - start;
				}
			}
			
		}

		private void ResetParticle(int number)
		{
			lock (_locker)
			{
				if (initialized)
				{
					#region ResetParticleLogic
					if (spawnsource == SpawnSource.Point)
					{
						positions[number] = spawn;
					}
					else
					{
						positions[number] = new Vector2(
							(float)rand.NextDouble() * (spawnend.X - spawn.X) + spawn.X,
							(float)rand.NextDouble() * (spawnend.Y - spawn.Y) + spawn.Y);

					}
					alphas[number] = 255;
					lifetimes[number] = 0.01f;



					int random;
					if (spawndirections == SpawnDirections.Angle)
					{
						velocities[number] = new Vector2(maxspeed.X, maxspeed.Y);
						velocities[number].Normalize();
						if (useminSpeed)
							velocities[number] *= (maxspeed.Length() - minSpeed.Length()) * (float)rand.NextDouble() + minSpeed.Length();
						else
							velocities[number] *= maxspeed.Length() * (float)rand.NextDouble();
						float angle = (float)rand.NextDouble() * spawnAngle;
						if (rand.Next(100) >= 50)
							angle *= -1;
						velocities[number] = velocities[number].Rotate(angle);
					}
					else
					{
						velocities[number] = new Vector2((float)rand.NextDouble(), (float)rand.NextDouble());
						velocities[number].Normalize();
						if (useminSpeed)
							velocities[number] *= (maxspeed - minSpeed) * (float)rand.NextDouble() + minSpeed;
						else
							velocities[number] *= maxspeed * (float)rand.NextDouble();

						if (spawndirections == SpawnDirections.AllWays || spawndirections == SpawnDirections.XPlusMinus)
						{
							random = rand.Next(100);
							if (random >= 50)
							{
								velocities[number].X *= -1;
							}
						}
						if (spawndirections == SpawnDirections.AllWays || spawndirections == SpawnDirections.YPlusMinus)
						{
							random = rand.Next(100);
							if (random >= 50)
							{
								velocities[number].Y *= -1;
							}
						}
					}

					positions[number] += velocities[number] * 0.016f;
					rects[number] = new Rectangle((int)positions[number].X, (int)positions[number].Y, size, size);

					#endregion
				}
			}
		}

		private void UpdateThread(object numThread)
		{
			float lifetime;
			Vector2 position;
			Vector2 velocity;
			float rotation;
			Rectangle rect;
			byte alpha;
			float floatAlpha;

			int numthread = (int)numThread;
			int particleperthread = (int)((float)numParticles / numThreads)+1;
			int startparticle = particleperthread * numthread;
			int endparticle = (int)MathHelper.Clamp( startparticle + particleperthread,0,positions.GetLength(0));
			DateTime last= DateTime.Now;
			DateTime start;
			Vector2 grav;

			while (running)
			{
				if (newframe[numthread] == true)
				{
#if DEBUG
					start = DateTime.Now;
#endif	
					Vector2 newpos;
					//(float)gametime.ElapsedGameTime.TotalSeconds;
					float elapsed = (float)gametime.ElapsedGameTime.TotalSeconds;
					//float elapsed = 0.003f;
					//int length = particles.GetLength(0);
					for (int i = startparticle; i < endparticle; i++)
					{
						// Save Particle information into a single Variable to avoid Array Bounds checks.
						lifetime = lifetimes[i];
						velocity = velocities[i];
						position = positions[i];
						rotation = Rotations[i];
						rect = rects[i];
						alpha = alphas[i];
						floatAlpha = floatalphas[i];

						#region SingleParticleUpdateLogic

						lifetime += elapsed;
						if (lifetime < totallifetime)
						{
							if (gravtype == GravityType.OverallForce)
							{
								grav = gravity;
							}
							else if (gravtype == GravityType.Newton)
							{
								grav = new Vector2();
								if (fixedMasses)
								{
									foreach (NewtonMass mass in masses)
									{
										grav += (mass.center - position) * (mass.weight * particleMass / (mass.center - position).LengthSquared());

									}
								}
								else
								{
									for (int massIndex = 0; massIndex < masses.Count; massIndex++)
									{
										Vector2 Tempgrav = (masses[massIndex].center - position) * (masses[massIndex].weight * particleMass / (masses[massIndex].center - position).LengthSquared());
										grav += Tempgrav / particleMass;
										masses[massIndex] = new NewtonMass() { center = masses[massIndex].center, velocity = masses[massIndex].velocity - Tempgrav / masses[massIndex].weight * elapsed * elapsed, weight = masses[massIndex].weight };
									}
								}
								//grav /= 2;
							}
							else
							{
								grav = (gravity - position) * (1000 / (gravity - position).Length());
							}
							velocity += grav * elapsed;
							velocity *= airFriction;
							newpos = position + velocity * elapsed;
							if (collisiontype == CollisionType.Collision)
							{
								#region ObjectCollision
								foreach (Rectangle colliderect in collisionrectangles)
								{
									//RightCollision
									if (position.X + size <= colliderect.X && newpos.X + size >= colliderect.X)
									{
										if (newpos.Y + size >= colliderect.Y && newpos.Y <= colliderect.Y + size)
										{
											velocity.X /= -1;//+ (float)(rand.NextDouble() / 10);
											velocity /= friction;
											newpos = position + velocity * elapsed;
											continue;
										}
									}

									//LeftCollision
									else if (position.X > colliderect.Right && newpos.X <= colliderect.Right)
									{
										if (newpos.Y + size >= colliderect.Y && newpos.Y <= colliderect.Y + size)
										{
											velocity.X /= -2;//+ (float)(rand.NextDouble() / 10);
											velocity /= friction;
											newpos = position + velocity * elapsed;
											continue;

										}
									}

									//BottomCollision
									else if (position.Y + size <= colliderect.Y && newpos.Y + size >= colliderect.Y)
									{
										if (position.X + size >= colliderect.X && position.X <= colliderect.Right)
										{
											velocity.Y /= -1;//+ (float)(rand.NextDouble() / 10);
											velocity /= friction;
											newpos = position + velocity * elapsed;
											continue;
										}
									}
									//UpCollision
									else if (position.Y >= colliderect.Y + size && newpos.Y <= colliderect.Y + size)
									{
										if (newpos.X + size >= colliderect.X && newpos.X <= colliderect.Right)
										{
											velocity.Y /= -1;//+ (float)(rand.NextDouble() / 10);
											velocity /= friction;
											newpos = position + velocity * elapsed;
											continue;
										}
									}



								}
								#endregion
							}

							if (BetweenParticleCollision)
							{
								for (int betweenParticlesIndex = 0; betweenParticlesIndex < positions.Length; betweenParticlesIndex++)
								{
									Vector2 tempVel;
									if (lifetimes[betweenParticlesIndex]<totallifetime)
									{
										if (position.X + size <= positions[betweenParticlesIndex].X && newpos.X + size >= positions[betweenParticlesIndex].X)
										{
											if (newpos.Y + size >= positions[betweenParticlesIndex].Y && newpos.Y <= positions[betweenParticlesIndex].Y + size)
											{
												//velocity.X /= -1;//+ (float)(rand.NextDouble() / 10);
												tempVel = velocity;
												velocity = velocities[betweenParticlesIndex];
												velocities[betweenParticlesIndex] = tempVel;
												velocity /= friction;
												continue;
											}
										}

																	//LeftCollision
										else if (position.X > positions[betweenParticlesIndex].X + size && newpos.X <= positions[betweenParticlesIndex].X + size)
										{
											if (newpos.Y + size >= positions[betweenParticlesIndex].Y && newpos.Y <= positions[betweenParticlesIndex].Y + size)
											{
												tempVel = velocity;
												velocity = velocities[betweenParticlesIndex];
												velocities[betweenParticlesIndex] = tempVel;
												velocity /= friction;
												continue;

											}
										}

										//BottomCollision
										else if (position.Y + size <= positions[betweenParticlesIndex].Y && newpos.Y + size >= positions[betweenParticlesIndex].Y)
										{
											if (position.X + size >= positions[betweenParticlesIndex].X && position.X <= positions[betweenParticlesIndex].X + size)
											{
												tempVel = velocity;
												velocity = velocities[betweenParticlesIndex];
												velocities[betweenParticlesIndex] = tempVel;
												velocity /= friction;
												continue;
											}
										}
										//UpCollision
										else if (position.Y >= positions[betweenParticlesIndex].Y + size && newpos.Y <= positions[betweenParticlesIndex].Y + size)
										{
											if (newpos.X + size >= positions[betweenParticlesIndex].X && newpos.X <= positions[betweenParticlesIndex].X + size)
											{
												tempVel = velocity;
												velocity = velocities[betweenParticlesIndex];
												velocities[betweenParticlesIndex] = tempVel;
												velocity /= friction;
												continue;
											}
										} 
									}
								}
							}


							Vector2 newVelocity = velocity * elapsed;
							position += newVelocity;
							rect.X = (int)position.X + size / 2;
							rect.Y = (int)position.Y + size / 2;
							if ((newVelocity).LengthSquared() > 1)
							{
								rotation = (float)Math.Acos(velocity.X / (velocity.Length()));
								if (velocity.Y < 0)
									rotation *= -1;
							}
							alpha = (byte)(255 - (255 * (lifetime / totallifetime)));
							floatAlpha = lifetime / totallifetime;
							
						}
						else if (Interlocked.Read(ref newSpawnCount) >= 0)
						{
							Interlocked.Decrement(ref newSpawnCount);
							ResetParticle(i);
							lifetime = lifetimes[i];
							velocity = velocities[i];
							position = positions[i];
							rotation = Rotations[i];
							rect = rects[i];
							alpha = alphas[i];
							floatAlpha = floatalphas[i];
						}
						#endregion

						lifetimes[i] = lifetime;
						velocities[i] = velocity;
						positions[i] = position;
						Rotations[i] = rotation;
						rects[i] = rect;
						alphas[i] = alpha;
						floatalphas[i] = floatAlpha;
					}
#if DEBUG
					duration[numthread] = DateTime.Now - start;
					last = start;
#endif
					newframe[numthread] = false;
				}
				resetEvents[numthread].Set(); //Tell that this thread is ready
				barrier.SignalAndWait(); //Wait for Barrier to go on.
				
				
			}

		}

		/// <summary>
		/// Adds a rectangle which the particles can collide with
		/// </summary>
		/// <param name="rect"></param>
		public void AddCollisionRect(Rectangle rect)
		{
			collisionrectangles.Add(rect);
			particleCuda.CollisionRectanglesChanged(collisionrectangles.ToArray());
		}

		/// <summary>
		/// Stops the particle Calculation
		/// Must be called before games End, 
		/// else the the particleThreads will never be stopped
		/// </summary>
		public void Dispose()
		{
			for (int i = 0; i < numThreads; i++)
			{
				IsRunning = false;
				try
				{
					updatethread[i].Abort();
				}
				catch (ThreadAbortException) { }
			}
			particleCuda.Dispose();
			enabled = false;
			initialized = false;
		}

		public bool UseCuda(bool use)
		{
			if (ParticleCUDA.CudaUsable() && use)
			{
				useCuda = true;
				particleCuda.Dispose();
				InitalizeCUDA();
			}
			else
				useCuda = false;
			return useCuda;
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			Draw(spriteBatch, Matrix.CreateTranslation(Vector3.Zero));
		}

		public void Draw(SpriteBatch spriteBatch, Matrix offsetmatrix)
		{
			if (initialized)
			{
				if (timeAlpha)
				{
					Rectangle drawrect = new Rectangle(0, 0, size, size);
					Color drawcolor = stdcolor;
					Rectangle src = new Rectangle(0, 0, particletex.Width, particletex.Height);
					Vector2 origin = new Vector2((float)size / 2 / (float)size * particletex.Width, (float)size / 2 / (float)size * particletex.Height);
					spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, null, offsetmatrix);
					for (int i =0; i< numParticles;i++)
					{
						if (lifetimes[i] < totallifetime)
						{
							drawcolor.A = alphas[i];
							spriteBatch.Draw(particletex, rects[i], null, drawcolor, Rotations[i], origin, SpriteEffects.None, 0);
						}
					}

					spriteBatch.End();
				}
				else
				{
					Rectangle drawrect = new Rectangle(0, 0, size, size);
					Color drawcolor = stdcolor;
					Rectangle src = new Rectangle(0, 0, particletex.Width, particletex.Height);
					Vector2 origin = new Vector2(src.Width / 2, src.Height / 2);
					spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, null, offsetmatrix);
					for (int i = 0; i < numParticles; i++)
					{
						if (lifetimes[i] < totallifetime)
						{
							drawcolor.A = (byte)(alpha*255);
							spriteBatch.Draw(particletex, rects[i], null, drawcolor, Rotations[i], origin, SpriteEffects.None, 0);
						}
					}

					spriteBatch.End();
				}
			}
		}

		public void DrawMasses(SpriteBatch spriteBatch, Texture2D tex, int Size,Matrix matrix)
		{
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, null, matrix);
			for (int i = 0; i < masses.Count; i++)
			{
				spriteBatch.Draw(tex, new Rectangle((int)masses[i].center.X - Size / 2, (int)masses[i].center.Y - Size / 2, Size, Size), Color.Green);
			}
			spriteBatch.End();
		}
	}

	
}
