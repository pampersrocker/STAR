#include <stdio.h>

struct Rectangle
{
	int X;
	int Y;
	int Width;
	int Height;

	__host__ __device__ int Right()
	{
		return X+Width;

	}

	__host__ __device__ int Bottom()
	{
		return Y+Height;
	}
};

struct Vector2
{
	float X;
	float Y;

	__host__ __device__ float Length()
	{
		return sqrt(X*X + Y*Y);
	}

	__host__ __device__ float LengthSquared()
	{
		return X*X + Y*Y;
	}

	__device__ Vector2()
	{
		X=0;
		Y=0;
	}

#pragma region "Operators"

	__host__ __device__ Vector2 operator+(Vector2 v)
	{
		Vector2 temp;
		temp.X = X;
		temp.Y = Y;
		temp.X+=v.X;
		temp.Y+=v.Y;
		return temp;
	}

	__host__ __device__ Vector2 operator-(Vector2 v)
	{
		Vector2 temp;
		temp.X = X;
		temp.Y = Y;
		temp.X-=v.X;
		temp.Y-=v.Y;
		return temp;
	}

	__host__ __device__ Vector2& operator +=(Vector2 &v)
	{
		X+=v.X;
		Y+=v.Y;
		return *this;
	}

	__host__ __device__ Vector2& operator -=(Vector2 &v)
	{
		X-=v.X;
		Y-=v.Y;
		return *this;
	}

	__host__ __device__ float operator *(Vector2 v)
	{
		return (X*v.X+Y*v.Y);
	}

	__host__ __device__ Vector2& operator *=(float f)
	{
		X*=f;
		Y*=f;
		return *this;
	}

	__host__ __device__ Vector2 operator *(float f)
	{
		Vector2 temp;
		temp.X = X;
		temp.Y = Y;
		temp.X*=f;
		temp.Y*=f;
		return temp;
	}

	__host__ __device__ Vector2 operator /(float f)
	{
		Vector2 temp;
		temp.X = X/f;
		temp.Y = Y/f;
		return temp;
	}

	__host__ __device__ Vector2& operator /=(float f)
	{
		X = X/f;
		Y = Y/f;
		return *this;
	}

#pragma endregion

};