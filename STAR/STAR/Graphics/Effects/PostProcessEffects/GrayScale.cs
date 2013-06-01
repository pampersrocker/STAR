using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Star.Graphics.Effects.PostProcessEffects
{
	class GrayScale : GraphicEffect
	{
		float rFactor = 1;
		float gFactor = 1;
		float bFactor = 1;

		public float RFactor
		{
			get { return rFactor; }
			set { rFactor = MathHelper.Clamp(value, 0, float.MaxValue); }
		}

		public float GFactor
		{
			get { return gFactor; }
			set { gFactor = MathHelper.Clamp(value, 0, float.MaxValue); }
		}

		public float BFactor
		{
			get { return bFactor; }
			set { bFactor = MathHelper.Clamp(value, 0, float.MaxValue); }
		}

		protected override AvailableEffects InitializeEffect(GraphicsDevice device, GameManagement.Options options)
		{
			return AvailableEffects.Grayscale;
		}

		protected override void ResetEffect()
		{
			rFactor = 1;
			bFactor = 1;
			gFactor = 1;
		}

		protected override void SetEffectParameters()
		{
			effect.Parameters["rFactor"].SetValue(rFactor);
			effect.Parameters["gFactor"].SetValue(gFactor);
			effect.Parameters["bFactor"].SetValue(bFactor);
		}
	}
}
