
#include "cuda_runtime.h"
#include "device_launch_parameters.h"
//#include <cuda.h>
#include <math.h>
#include <stdio.h>
//#include "xnaTypes.h"
#include "particleSystemTypes.h"
void __syncthreads();
void checkCUDAError(const char *msg);

__global__ void doParticleCalculations(Vector2 *positions,Vector2 *velocities,float *lifetimes,struct Rectangle* rects,unsigned char *alphas,float *Rotations,int N,struct ParticleOptions options,struct Rectangle* collisionRects,int nColRects)
{

    Vector2 grav;
    Vector2 newpos;
    //(float)gametime.ElapsedGameTime.TotalSeconds;
	float elapsed = options.elapsedGameTime;
    //float elapsed = 0.003f;
    //int length = particles.GetLength(0);
	//particles[0].lifetime = thread_start_idx;
	int inOffset  = blockDim.x * blockIdx.x;
	int i = inOffset + threadIdx.x;
	//for (int i = blockDim.x * blockIdx.x; i < thread_end_idx; i+=STRIDE)
	if(i<N)
    {
		__syncthreads();
		//device_particles[i].lifetime = elapsed;
		//particles[i].lifetime += 10;
		//if (lifetimes[i] < options.totalLifeTime)
        {
			
            if (options.gravityType == OverallForce)
            {
                grav = options.gravity;
            }
			else if (options.gravityType == Newton)
            {
				grav= Vector2();
				for(int massIndex = 0; massIndex < options.newtonMassSize;massIndex++)
                {
					grav += ((options.mass[massIndex].center - positions[i]) * (options.mass[massIndex].weight / ((options.mass[massIndex].center - positions[i]).LengthSquared())));
                }
                //grav /= 2;
            }
            else
            {
                grav = (options.gravity - positions[i]) * (1000 / (options.gravity - positions[i]).Length());
            }
            velocities[i] += grav * elapsed;
            velocities[i] *= options.AirFriction;
            newpos = positions[i] + velocities[i] * elapsed;
            if (options.collisionType == Collision)
            {
				for (int collisionIndex = 0; collisionIndex< nColRects;collisionIndex++)
                {
                    //RightCollision
                    if (positions[i].X + options.size <= collisionRects[collisionIndex].X && newpos.X + options.size >= collisionRects[collisionIndex].X)
                    {
                        if (newpos.Y + options.size >= collisionRects[collisionIndex].Y && newpos.Y <= collisionRects[collisionIndex].Bottom())
                        {
                            velocities[i].X /= -1;//+ (float)(rand.NextDouble() / 10);
                            velocities[i] /= options.Friction;
                            newpos = positions[i] + velocities[i] * elapsed;
                            continue;
                        }
                    }

                    //LeftCollision
                    else if (positions[i].X > collisionRects[collisionIndex].Right() && newpos.X <= collisionRects[collisionIndex].Right())
                    {
                        if (newpos.Y + options.size >= collisionRects[collisionIndex].Y && newpos.Y <= collisionRects[collisionIndex].Bottom())
                        {
                            velocities[i].X /= -1;//+ (float)(rand.NextDouble() / 10);
                            velocities[i] /= options.Friction;
                            newpos = positions[i] + velocities[i] * elapsed;
                            continue;

                        }
                    }

                    //BottomCollision
                    else if (positions[i].Y + options.size <= collisionRects[collisionIndex].Y && newpos.Y + options.size >= collisionRects[collisionIndex].Y)
                    {
                        if (positions[i].X + options.size >= collisionRects[collisionIndex].X && positions[i].X <= collisionRects[collisionIndex].Right())
                        {
                            velocities[i].Y /= -1;//+ (float)(rand.NextDouble() / 10);
                            velocities[i] /= options.Friction;
                            newpos = positions[i] + velocities[i] * elapsed;
                            continue;
                        }
                    }
                    //UpCollision
                    else if (positions[i].Y >= collisionRects[collisionIndex].Bottom() && newpos.Y <= collisionRects[collisionIndex].Bottom())
                    {
                        if (newpos.X + options.size >= collisionRects[collisionIndex].X && newpos.X <= collisionRects[collisionIndex].Right())
                        {
                            velocities[i].Y /= -1;//+ (float)(rand.NextDouble() / 10);
                            velocities[i] /= options.Friction;
                            newpos = positions[i] + velocities[i] * elapsed;
                            continue;
                        }
                    }


                }
            }

			lifetimes[i] = lifetimes[i] + options.elapsedGameTime;
            Vector2 newVelocity =velocities[i] * elapsed;
			positions[i] += newVelocity;
			rects[i].X = (int)positions[i].X + options.size/2;
			rects[i].Y = (int)positions[i].Y + options.size / 2;
            //if ((newVelocity).LengthSquared() > 1)
            
			Rotations[i] = (float)acos(velocities[i].X / (velocities[i].Length()));
                if (velocities[i].Y < 0)
                    Rotations[i] *= -1;
            
			alphas[i] = (char)(255 - (255 * (lifetimes[i] / options.totalLifeTime)));

        }
		
    }
            
            //updatethread.Abort();
        
        //Thread.Sleep(0);
}


/*Particle *device_particles;
Vector2 *device_positions,*device_velocities;
struct Rectangle *device_rects,*device_collisionrects;
float *device_lifetimes,*device_Rotations;
unsigned char* device_alphas;
int numberColRects;*/
int main()
{

	cudaError_t cudaStatus;
	cudaStatus = cudaSetDevice(0);
    if (cudaStatus != cudaSuccess) {
        fprintf(stderr, "cudaSetDevice failed!  Do you have a CUDA-capable GPU installed?");
    }
	//if(CudaAvailable())
	{
		printf("success");
	}
	return 0;
}

extern "C"

{

	//from deviceQuery Example in CUDA Toolkit 3.2
	__declspec (dllexport) bool CudaAvailable()
	{
        
    int deviceCount = 0;
	if (cudaGetDeviceCount(&deviceCount) != cudaSuccess) {
		return false;
	}

    // This function call returns 0 if there are no CUDA capable devices.
    if (deviceCount == 0)
        return false;

    int dev;   
    for (dev = 0; dev < deviceCount; ++dev) {
        cudaDeviceProp deviceProp;
        cudaGetDeviceProperties(&deviceProp, dev);

        if (dev == 0) {
			// This function call returns 9999 for both major & minor fields, if no CUDA capable devices are present
            if (deviceProp.major == 9999 && deviceProp.minor == 9999)
                return false;
			else
                return true;
                
        }
	}
	return true;
	}

	__declspec(dllexport) struct ArrayHolder* CudaInitialize(Vector2 *positions,Vector2 *velocities,float *Rotations,float *lifetimes,struct Rectangle * rects,unsigned char *alphas, int N)
	{
		struct ArrayHolder *holder;
		holder = (struct ArrayHolder *) malloc(sizeof(ArrayHolder));
		size_t sizeVector = N*8;
		size_t sizeFloat = N * sizeof(float);
		size_t sizeRect = N * sizeof(struct Rectangle);
		size_t sizeChar = N* sizeof(unsigned char);


		if(cudaMalloc((void**) &(holder->device_Rotations), sizeFloat) != cudaSuccess)
		{
			printf("cudaMalloc Error!\n");
		}

		if(cudaMalloc((void**) &(holder->device_lifetimes), sizeFloat) != cudaSuccess)
		{
			printf("cudaMalloc Error!\n");
		}

		if(cudaMalloc((void**) &(holder->device_rects), sizeRect) != cudaSuccess)
		{
			printf("cudaMalloc Error!\n");
		}

		if(cudaMalloc((void**) &holder->device_alphas, sizeChar) != cudaSuccess)
		{
			printf("cudaMalloc Error!\n");
		}

		if(cudaMalloc((void**) &holder->device_positions, sizeVector) != cudaSuccess)
		{
			printf("cudaMalloc Error!\n");
		}

		if(cudaMalloc((void**) &holder->device_velocities, sizeVector) != cudaSuccess)
		{
			printf("cudaMalloc Error!\n");
		}

		if(cudaMemcpy(holder->device_lifetimes, lifetimes, sizeFloat, cudaMemcpyHostToDevice) != cudaSuccess)
		{
			printf("Error 11\n");
		}

		if(cudaMemcpy(holder->device_rects, rects, sizeRect, cudaMemcpyHostToDevice) != cudaSuccess)
		{
			printf("Error 1\n");
		}

		if(cudaMemcpy(holder->device_Rotations, Rotations, sizeFloat, cudaMemcpyHostToDevice) != cudaSuccess)
		{
			printf("Error 1\n");
		}

		if(cudaMemcpy(holder->device_alphas, alphas, sizeChar, cudaMemcpyHostToDevice) != cudaSuccess)
		{
			printf("Error 1\n");
		}

		if(cudaMemcpy(holder->device_positions, positions, sizeVector, cudaMemcpyHostToDevice) != cudaSuccess)
		{
			printf("Error 1\n");
		}


		if(cudaMemcpy(holder->device_velocities, velocities, sizeVector, cudaMemcpyHostToDevice) != cudaSuccess)
		{
			printf("Error 1\n");
		}

		return holder;

	}


	
	__declspec(dllexport) void CudaReset(struct ArrayHolder* holder,Vector2 *positions,Vector2 *velocities,float *Rotations,float *lifetimes,struct Rectangle * rects,unsigned char *alphas, int N)
	{
		size_t sizeVector = N*8;
		size_t sizeFloat = N * sizeof(float);
		size_t sizeRect = N * sizeof(struct Rectangle);
		size_t sizeChar = N* sizeof(unsigned char);

		if(cudaMemcpy(holder->device_lifetimes, lifetimes, sizeFloat, cudaMemcpyHostToDevice) != cudaSuccess)
		{
			printf("Error 2\n");
		}

		if(cudaMemcpy(holder->device_rects, rects, sizeRect, cudaMemcpyHostToDevice) != cudaSuccess)
		{
			printf("Error 1\n");
		}

		if(cudaMemcpy(holder->device_Rotations, Rotations, sizeFloat, cudaMemcpyHostToDevice) != cudaSuccess)
		{
			printf("Error 1\n");
		}

		if(cudaMemcpy(holder->device_alphas, alphas, sizeChar, cudaMemcpyHostToDevice) != cudaSuccess)
		{
			printf("Error 1\n");
		}

		if(cudaMemcpy(holder->device_positions, positions, sizeVector, cudaMemcpyHostToDevice) != cudaSuccess)
		{
			printf("Error 1\n");
		}


		if(cudaMemcpy(holder->device_velocities, velocities, sizeVector, cudaMemcpyHostToDevice) != cudaSuccess)
		{
			printf("Error 1\n");
		}

	}

	__declspec(dllexport) void CudaCollisionRectanglesChanged(struct ArrayHolder *holder,struct Rectangle* collisionRectangles,int Size)
	{
		size_t collisionRectsSize = Size * sizeof(struct Rectangle);
		//if(holder->device_collisionrects != NULL)
		//	cudaFree(holder->device_collisionrects);
		if(cudaMalloc((void**) &holder->device_collisionrects, collisionRectsSize) != cudaSuccess)
		{
			printf("cudaMalloc Error!\n");
		}
		if(cudaMemcpy(holder->device_collisionrects, collisionRectangles, collisionRectsSize, cudaMemcpyHostToDevice) != cudaSuccess)
		{
			printf("Error 1\n");
		}
		holder->numberColRects = Size;
	}

	__declspec(dllexport) void SinglePArticleChanged(struct ArrayHolder *holder,Vector2 positions,Vector2 velocities,float Rotations,float lifetimes,struct Rectangle  rects,unsigned char alphas,int number)
	{
		if(cudaMemcpy(&(holder->device_velocities[number]),&velocities,sizeof(Vector2),cudaMemcpyHostToDevice) != cudaSuccess)
		{
			cudaThreadSynchronize();
			checkCUDAError("Error Resetting Particle");
		}
		cudaMemcpy(&(holder->device_positions[number]),&positions,sizeof(Vector2),cudaMemcpyHostToDevice);
		cudaMemcpy(&(holder->device_Rotations[number]),&Rotations,sizeof(float),cudaMemcpyHostToDevice);
		cudaMemcpy(&(holder->device_lifetimes[number]),&lifetimes,sizeof(float),cudaMemcpyHostToDevice);
		cudaMemcpy(&(holder->device_alphas[number]),&alphas,sizeof(char),cudaMemcpyHostToDevice);
		cudaMemcpy(&(holder->device_rects[number]),&rects,sizeof(struct Rectangle),cudaMemcpyHostToDevice);
		/*[number] = positions;
		device_velocities[number] = velocities;
		device_Rotations[number] = Rotations;
		device_lifetimes[number] = lifetimes;
		device_alphas[number] = alphas;
		device_rects[number]= rects;*/

	}

	__declspec(dllexport) void CudaMain(struct ArrayHolder*holder,Vector2 *positions,Vector2 *velocities,float *Rotations,float *lifetimes,struct Rectangle * rects,unsigned char *alphas, int N,ParticleOptions options)

	{
		size_t sizeVector = N*8;
		size_t sizeFloat = N * sizeof(float);
		size_t sizeRect = N * sizeof(struct Rectangle);
		size_t sizeChar = N* sizeof(unsigned char);
		//if(cudaMemcpy(device_velocities, velocities, sizeVector, cudaMemcpyHostToDevice) != cudaSuccess)

		//{

		//	printf("Error 1\n");

		//}


		//if(cudaMemcpy(device_positions, positions, sizeVector, cudaMemcpyHostToDevice) != cudaSuccess)

		//{

		//	printf("Error 1\n");

		//}

		//particles[0].lifetime = 1337;
		//if(cudaMemcpy(device_particles, particles, size, cudaMemcpyHostToDevice) != cudaSuccess)

		//{

		//	printf("Error 2\n");

		//}
		//particles[0].lifetime = 1338;
		int numThreadsPerBlock = 256;
		int numBlocks = N/numThreadsPerBlock+ (N%numThreadsPerBlock == 0?0:1);
		dim3 dimGrid(numBlocks);
		dim3 dimBlock(numThreadsPerBlock);
		doParticleCalculations <<< dimGrid, dimBlock >>> (holder->device_positions,holder->device_velocities,holder->device_lifetimes,holder->device_rects,holder->device_alphas,holder->device_Rotations, N,options,holder->device_collisionrects,holder->numberColRects);
		cudaThreadSynchronize();
		//checkCUDAError("Error 2222222");
		if((cudaMemcpy(positions, holder->device_positions, sizeVector, cudaMemcpyDeviceToHost)) != cudaSuccess)
		{
			cudaThreadSynchronize();
			checkCUDAError("Error 333331");
		}

		if((cudaMemcpy(rects, holder->device_rects, sizeRect, cudaMemcpyDeviceToHost)) != cudaSuccess)
		{
			cudaThreadSynchronize();
			checkCUDAError("Error 333332");
		}

		if((cudaMemcpy(lifetimes, holder->device_lifetimes, sizeFloat, cudaMemcpyDeviceToHost)) != cudaSuccess)
		{
			cudaThreadSynchronize();
			checkCUDAError("Error 333333");
		}

		if((cudaMemcpy(Rotations, holder->device_Rotations, sizeFloat, cudaMemcpyDeviceToHost)) != cudaSuccess)
		{
			cudaThreadSynchronize();
			checkCUDAError("Error 333334");
		}

		if((cudaMemcpy(alphas, holder->device_alphas, sizeChar, cudaMemcpyDeviceToHost)) != cudaSuccess)
		{
			cudaThreadSynchronize();
			checkCUDAError("Error 333335");
		}

		if((cudaMemcpy(velocities, holder->device_velocities, sizeVector, cudaMemcpyDeviceToHost)) != cudaSuccess)

		{
			cudaThreadSynchronize();
			checkCUDAError("Error 333336");
		}

	}


	
	__declspec(dllexport) bool Clear(struct ArrayHolder* holder)
	{
		bool success = true;
		if(cudaFree(holder->device_positions)!=cudaSuccess)
			success=false;
		if(cudaFree(holder->device_velocities)!=cudaSuccess)
			success=false;
		if(cudaFree(holder->device_rects)!=cudaSuccess)
			success=false;
		if(cudaFree(holder->device_lifetimes)!=cudaSuccess)
			success=false;
		if(cudaFree(holder->device_Rotations)!=cudaSuccess)
			success=false;
		if(cudaFree(holder->device_collisionrects)!=cudaSuccess)
			success=false;
		if(cudaFree(holder->device_alphas)!=cudaSuccess)
			success=false;

		return success;
	}
}

void checkCUDAError(const char *msg)
{
    cudaError_t err = cudaGetLastError();
    if( cudaSuccess != err) 
    {
        fprintf(stderr, "Cuda error: %s: %s.\n", msg, cudaGetErrorString( err) );
        //exit(EXIT_FAILURE);
    }                         
}