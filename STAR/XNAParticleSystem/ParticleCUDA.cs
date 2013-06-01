using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;

namespace XNAParticleSystem
{
	public class ParticleCUDA
	{
		IntPtr cudaObject;

		[DllImport(@"cudaParticleEngine.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
		private unsafe static extern bool CudaAvailable();

		[DllImport(@"cudaParticleEngine.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
		unsafe static extern void CudaReset(IntPtr holder, [Out]Vector2* positions, [Out]Vector2* velocities, float* Rotations, float* lifetimes, Rectangle* rects, byte* alphas,
			 int Size, [In] ParticleOptions options);

		[DllImport(@"cudaParticleEngine.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
		unsafe static extern void SinglePArticleChanged(
			IntPtr holder,
			Vector2 positions,
			Vector2 velocities,
			float Rotations,
			float lifetimes,
			Rectangle rects,
			byte alphas,
			int number);

		[DllImport(@"cudaParticleEngine.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
		unsafe static extern void CudaCollisionRectanglesChanged(IntPtr holder, [In]Rectangle* collisionRectangles, int Size);

		[DllImport(@"cudaParticleEngine.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
		unsafe static extern IntPtr CudaInitialize([Out]Vector2* positions,[Out]Vector2 *velocities,float *Rotations,float *lifetimes,Rectangle * rects,byte *alphas,
			int Size);

		[DllImport(@"cudaParticleEngine.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
		unsafe static extern void CudaMain(IntPtr holder, [Out]Vector2* positions, [Out]Vector2* velocities, float* Rotations, float* lifetimes, Rectangle* rects, byte* alphas,
			int Size, ParticleOptions options);

		[DllImport(@"cudaParticleEngine.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
		unsafe static extern bool Clear(IntPtr holder);

		public static bool IsCudaAvailable()
		{
			bool available;
			try
			{
				available = CudaAvailable();
			}
			catch
			{
				available = false;                
			}

			return available;
		}

		public void Initalize(ParticleArrayHolder holder, ParticleOptions options)
		{
			if (IsCudaAvailable())
			{
				unsafe
				{
					fixed (Vector2* ppositions = holder.positions, pvel = holder.velocities)
					{
						fixed (float* pRotations = holder.Rotations, pLifeTimes = holder.lifetimes)
						{
							fixed (Rectangle* prect = holder.rects)
							{
								fixed (byte* palphas = holder.alphas)
								{
									//CudaInitialize(particlePTR, particles.Length, options);
									//TODO
									try
									{
										cudaObject = CudaInitialize(ppositions, pvel, pRotations, pLifeTimes, prect, palphas, holder.positions.Length);
									}
									catch (Exception)
									{
										
										//throw;
									}
									
								}
							}
						}
					}
				}
			}
			
		}

		public static bool CudaUsable()
		{
			return IsCudaAvailable();
		}

		public unsafe void Run(ParticleArrayHolder holder, ParticleOptions options)
		{
			if (IsCudaAvailable())
			{
				fixed (Vector2* ppositions = holder.positions, pvel = holder.velocities)
				{
					fixed (float* pRotations = holder.Rotations, pLifeTimes = holder.lifetimes)
					{
						fixed (Rectangle* prect = holder.rects)
						{
							fixed (byte* palphas = holder.alphas)
							{
								//CudaInitialize(particlePTR, particles.Length, options);

								CudaMain(cudaObject, ppositions, pvel, pRotations, pLifeTimes, prect, palphas, holder.positions.Length, options);

							}
						}
					}
				}
			}
		}
		public unsafe void Reset(ParticleArrayHolder holder, ParticleOptions options)
		{
			if (IsCudaAvailable())
			{
				fixed (Vector2* ppositions = holder.positions, pvel = holder.velocities)
				{
					fixed (float* pRotations = holder.Rotations, pLifeTimes = holder.lifetimes)
					{
						fixed (Rectangle* prect = holder.rects)
						{
							fixed (byte* palphas = holder.alphas)
							{
								//CudaInitialize(particlePTR, particles.Length, options);
								try
								{

								}
								catch
								{
									
									throw;
								}
								CudaReset(cudaObject, ppositions, pvel, pRotations, pLifeTimes, prect, palphas, holder.positions.Length, options);
							}
						}
					}
				}
			}
		}

		public unsafe void CollisionRectanglesChanged(Rectangle[] colRects)
		{
			if (IsCudaAvailable())
			{
				fixed (Rectangle* prec = colRects)
				{
					CudaCollisionRectanglesChanged(cudaObject, prec, colRects.Length);
				}
			}
		}

		public unsafe void SingleParticleChanged(ParticleArrayHolder holder, int number)
		{
			if (IsCudaAvailable())
			{
				//CudaInitialize(particlePTR, particles.Length, options);
				SinglePArticleChanged(cudaObject, holder.positions[number], holder.velocities[number], holder.Rotations[number], holder.lifetimes[number], holder.rects[number], holder.alphas[number], number);
			}		
		}

		public unsafe void Dispose()
		{
			if (IsCudaAvailable())
			{
				bool success = false;
				if (cudaObject != null && cudaObject != IntPtr.Zero)
					success = Clear(cudaObject);
			}
		}

		~ParticleCUDA()
		{
			Dispose();
		}
	}
}
