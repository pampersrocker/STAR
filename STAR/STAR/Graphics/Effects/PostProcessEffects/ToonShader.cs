using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Star.Graphics.Effects.PostProcessEffects
{
	public enum ToonShaderEffects
	{ 
		Outline,
		DrawEffect,
		BrightEdges,
		BrightEdgesColored
	}

	public class ToonShader : GraphicEffect
	{
		RenderTarget2D target;
		ToonShaderEffects currentTechnique;

		public ToonShaderEffects CurrentTechnique
		{
			get { return currentTechnique; }
			set { currentTechnique = value; }
		}

		protected override AvailableEffects InitializeEffect(GraphicsDevice device, GameManagement.Options options)
		{
			PresentationParameters pp = device.PresentationParameters;
			target = new RenderTarget2D(device, pp.BackBufferWidth, pp.BackBufferHeight, false, SurfaceFormat.Color, DepthFormat.None, pp.MultiSampleCount, RenderTargetUsage.PreserveContents);
			return AvailableEffects.ToonShader;
		}

		protected override void UpdateEffect(GameTime gameTime, Vector2 pos)
		{
			//
		}

		protected override void DrawEffect(Texture2D tex, SpriteBatch spriteBatch, GraphicsDevice device)
		{
			RenderTarget2D curTarget = device.GetCurrentRenderTarget();
			//device.SetRenderTarget(target);
			effect.Parameters["ScreenSize"].SetValue(new Vector2(tex.Width, tex.Height));
			spriteBatch.Begin( SpriteSortMode.Immediate, BlendState.NonPremultiplied,null,null,null,effect);
			spriteBatch.Draw(tex, Vector2.Zero, Color.White);
			spriteBatch.End();
			//device.SetRenderTarget(curTarget);
			//spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
			//spriteBatch.Draw(target, Vector2.Zero, Color.White);
			//spriteBatch.End();
		}

		protected override void ResetEffect()
		{
			//throw new NotImplementedException();
		}

		protected override void SetEffectParameters()
		{
			effect.CurrentTechnique = effect.Techniques[currentTechnique.ToString()];
		}
	}
}
