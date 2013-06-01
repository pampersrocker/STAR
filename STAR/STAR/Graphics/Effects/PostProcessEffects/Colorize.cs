using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Star.Game.Level;

namespace Star.Graphics.Effects.PostProcessEffects
{
	[Flags]
	public enum ColorizationFlags
	{ 
		None = 0x00,
		Hue = 0x01,
		Sat = 0x02,
		Contrast = 0x04
	}

    public class Colorize : GraphicEffect
    {
		private LayerFXData fxData;
		string effectColorization;
		private ColorizationFlags colorization;

		public ColorizationFlags Colorization
		{
			get { return colorization; }
			set 
			{ 
				colorization = value;
				parseColorization();
			}
		}

		public LayerFXData LayerFXData
		{
			get { return fxData; }
			set 
			{
				fxData = value;
				colorization = ColorizationFlags.None;

				if (value.HueEnabled)
					colorization |= ColorizationFlags.Hue;
				if (value.SaturationEnabled)
					colorization |= ColorizationFlags.Sat;
				if (value.ContrastEnabled)
					colorization |= ColorizationFlags.Contrast;

				brightness = value.Brightness;
				contrast = value.Contrast;
				coloration = value.Hue;
				saturation = value.Saturation;
				parseColorization();
				SetEffectParameters();
			}
		}


		private float coloration;

		/// <summary>
		/// Gets or sets the Coloration
		/// </summary>
		public float Coloration
		{
			get { return coloration; }
			set { coloration = MathHelper.Clamp(value, 0f, 1f); }
		}
		private float saturation =1;

		/// <summary>
		/// Gets or sets the Saturation
		/// </summary>
		public float Saturation
		{
			get { return saturation; }
			set { saturation = MathHelper.Clamp(value, 0.0f, 1.0f); }
		}
		private float brightness;

		public float Brightness
		{
			get { return brightness; }
			set { brightness = MathHelper.Clamp(value, -1f, 1.0f); }
		}

		private float contrast;

		public float Contrast
		{
			get { return contrast; }
			set { contrast = MathHelper.Clamp(value, 0.0f, 5f); }
		}

		

        protected override AvailableEffects InitializeEffect(Microsoft.Xna.Framework.Graphics.GraphicsDevice device, Star.GameManagement.Options options)
        {
			colorization = ColorizationFlags.None;
            return AvailableEffects.Colorize;
        }

        protected override void UpdateEffect(Microsoft.Xna.Framework.GameTime gameTime, Microsoft.Xna.Framework.Vector2 pos)
        {
            SetEffectParameters();
        }

        protected override void DrawEffect(Microsoft.Xna.Framework.Graphics.Texture2D tex, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Microsoft.Xna.Framework.Graphics.GraphicsDevice device)
        {
			if (colorization != ColorizationFlags.None)
			{
				//3_1
				//spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState);
				//effect.Begin();
				//foreach (EffectPass pass in effect.CurrentTechnique.Passes)
				//{
				//	pass.Begin();
				spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, effect);
				spriteBatch.Draw(tex, Vector2.Zero, Color.White);
				//	pass.End();
				//}
				//effect.End();
				spriteBatch.End();
			}
        }

        protected override void ResetEffect()
        {
            //throw new NotImplementedException();
			colorization = ColorizationFlags.None;
			coloration = 0;
			saturation = 1;
			brightness = 0;
			contrast = 1;
        }

        protected override void SetEffectParameters()
        {
            effect.Parameters["offset"].SetValue(coloration);
			//saturation = 0;
			effect.Parameters["saturation"].SetValue(saturation);
			effect.Parameters["a"].SetValue(contrast);
			effect.Parameters["b"].SetValue(brightness);
        }

		public override void Begin()
		{
			if (colorization != ColorizationFlags.None)
				base.Begin();
		}

		public override void End()
		{
			if (colorization != ColorizationFlags.None)
				base.End();
		}

		private void parseColorization()
		{
			if (colorization != ColorizationFlags.None)
			{
				effectColorization = "";
				if (colorization.Contains(ColorizationFlags.Hue))
					effectColorization += ColorizationFlags.Hue.ToString();
				if (colorization.Contains(ColorizationFlags.Sat))
					effectColorization += ColorizationFlags.Sat.ToString();
				if (colorization.Contains(ColorizationFlags.Contrast))
					effectColorization += ColorizationFlags.Contrast;
				effect.CurrentTechnique = Effect.Techniques[effectColorization];
			}

		}
    }
}
