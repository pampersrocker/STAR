using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Runtime.InteropServices;

namespace XNAParticleSystem
{
	//[StructLayout(LayoutKind.Sequential), Serializable]
	//public struct ParticleOptions
	//{
		
	//    float totalLifeTime;
	//    float elapsedGameTime;
	//    [MarshalAsAttribute( UnmanagedType.SysInt)]
	//    GravityType gravType;
	//    [MarshalAsAttribute(UnmanagedType.Struct)]
	//    Vector2 gravity;
	//    [MarshalAsAttribute(UnmanagedType.Struct)]
	//    NewtonMass[] mass;
	//    int newtonMassSize;
	//    [MarshalAsAttribute(UnmanagedType.SysInt)]
	//    CollisionType collisionType;
	//    [MarshalAsAttribute(UnmanagedType.Struct)]
	//    Rectangle[] collisionRects;
	//    int collisionRectSize;
	//    int size;
	//    float AirFriction;
	//    float Friction;
	//}

	[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
	public struct ParticleOptions
	{

		/// float
		public float totalLifeTime;

		/// float
		public float elapsedGameTime;

		/// GravType
		public GravityType gravType;

		/// Vector2
		public Vector2 gravity;

		/// NewtonMass*
		public System.IntPtr mass;

		/// int
		public int newtonMassSize;

		/// CollisionType
		public CollisionType collisionType;

		/// Rectangle*
		public IntPtr collisionRects;

		/// int
		public int collisionRectSize;

		/// int
		public int size;

		/// float
		public float AirFriction;

		/// float
		public float Friction;
	}

}
