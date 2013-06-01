using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace XNAParticleSystem
{
	[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct Particle
    {
        public byte alpha;
        public Vector2 pos;
        public Vector2 velocity;
        public float lifetime;
		public float Rotation;
		public Rectangle rect;
    }

	public struct ParticleArrayHolder
	{
		public Vector2[] positions;
		public Vector2[] velocities;
		public float[] lifetimes;
		public Rectangle[] rects;
		public float[] Rotations;
		public byte[] alphas;
	}
}
