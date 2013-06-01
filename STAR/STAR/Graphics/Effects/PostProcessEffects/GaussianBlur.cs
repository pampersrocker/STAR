using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Star.Graphics.Effects.PostProcessEffects
{
	class GaussianBlur : GraphicEffect
	{
		//RenderTarget2D target1;

		

		protected override AvailableEffects InitializeEffect(Microsoft.Xna.Framework.Graphics.GraphicsDevice device, Star.GameManagement.Options options)
		{
			PresentationParameters pp = device.PresentationParameters;
			//3_1
			//target1 = new RenderTarget2D(device, pp.BackBufferWidth, pp.BackBufferHeight, pp.BackBufferCount, pp.BackBufferFormat);
			
			return AvailableEffects.GaussianBlur;
		}

		protected override void UpdateEffect(Microsoft.Xna.Framework.GameTime gameTime, Microsoft.Xna.Framework.Vector2 pos)
		{
			throw new NotImplementedException();
		}

		protected override void DrawEffect(Microsoft.Xna.Framework.Graphics.Texture2D tex, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Microsoft.Xna.Framework.Graphics.GraphicsDevice device)
		{
			throw new NotImplementedException();
		}

		protected override void ResetEffect()
		{
			throw new NotImplementedException();
		}

		protected override void SetEffectParameters()
		{
			throw new NotImplementedException();
		}
	}
}
