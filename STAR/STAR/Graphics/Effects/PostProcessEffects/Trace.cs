using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Star.Graphics.Effects.PostProcessEffects
{
    class Trace : GraphicEffect
    {
        const int frames = 30;
        Texture2D[] traces;
        float[] alphas;
        //ResolveTexture2D resolvedTex;
		RenderTarget2D resolvedTex;
		RenderTarget2D target;
        int frameCount;
		float baseAlpha = 0.5f;
		

        float alphaReductionPerFrame;

        public float AlphaReductionPerFrame
        {
            get { return alphaReductionPerFrame; }
            set { alphaReductionPerFrame = value; }
        }
        protected override AvailableEffects InitializeEffect(Microsoft.Xna.Framework.Graphics.GraphicsDevice device, Star.GameManagement.Options options)
        {
			resolvedTex = new RenderTarget2D(
				device,
				device.PresentationParameters.BackBufferWidth,
				device.PresentationParameters.BackBufferHeight,
				false,
				device.PresentationParameters.BackBufferFormat,
				 DepthFormat.None);
            traces = new Texture2D[frames];
            alphas = new float[frames];
			for (int i = 0; i < traces.Length; i++)
			{
				traces[i] = new Texture2D(device, device.PresentationParameters.BackBufferWidth, device.PresentationParameters.BackBufferHeight, false, SurfaceFormat.Color);
			}
			//target = new RenderTarget2D(
			//    device,
			//    device.PresentationParameters.BackBufferWidth,
			//    device.PresentationParameters.BackBufferHeight,
			//    device.PresentationParameters.BackBufferCount,
			//    device.PresentationParameters.BackBufferFormat,
			//    device.PresentationParameters.MultiSampleType,
			//    device.PresentationParameters.MultiSampleQuality);
			target = new RenderTarget2D(
				device,
				device.PresentationParameters.BackBufferWidth,
				device.PresentationParameters.BackBufferHeight,
				false,
				device.PresentationParameters.BackBufferFormat,
				 DepthFormat.Depth24);
			alphaReductionPerFrame = baseAlpha / (frames * 0.5f);
            return AvailableEffects.None;
            //throw new NotImplementedException();
        }

        protected override void UpdateEffect(Microsoft.Xna.Framework.GameTime gameTime, Microsoft.Xna.Framework.Vector2 pos)
        {
            //throw new NotImplementedException();
        }

        protected override void DrawEffect(Microsoft.Xna.Framework.Graphics.Texture2D tex, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Microsoft.Xna.Framework.Graphics.GraphicsDevice device)
        {
            frameCount++;
            //if (frameCount % 3 == 0)
            {
				
                //device.ResolveBackBuffer(resolvedTex);
                //3_1
				//resolvedTex = new ResolveTexture2D(device, device.PresentationParameters.BackBufferWidth, device.PresentationParameters.BackBufferHeight, device.PresentationParameters.BackBufferCount, device.PresentationParameters.BackBufferFormat);
                //device.ResolveBackBuffer(resolvedTex);
				resolvedTex = (RenderTarget2D)device.GetRenderTargets()[0].RenderTarget;
				//if (traces[0] != null)
				//    traces[0].Dispose();
				Color[] data=new Color[traces[0].Width*traces[0].Height];
                for (int i = 0; i < traces.Length - 1; i++)
                {
					//3_1
                    //traces[i] = traces[i + 1];
					//traces[i+1].GetData(data);
					//traces[i].SetData(data);
                }
                for (int i = 0; i < alphas.Length - 1; i++)
                {
                    alphas[i] = alphas[i + 1];
                    alphas[i] -= alphaReductionPerFrame;
                }
				//device.SetRenderTarget(null);
				
				//resolvedTex.GetData(data);
				//device.SetRenderTarget(resolvedTex);
				//traces[traces.Length - 1].SetData(data);
                //traces[traces.Length - 1] = resolvedTex;
                alphas[alphas.Length - 1] = baseAlpha-alphaReductionPerFrame;
            }

			//device.SetRenderTarget(0, target);
			//device.DepthStencilBuffer = GraphicEffect.CreateDepthStencilBuffer(target);
			
			//3_1
			//spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState);
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
			//spriteBatch.Draw(tex, Vector2.Zero, Color.White);
			
			for (int i = traces.Length -1; i >= 0; i--)
            {
                if (traces[i] != null)
                    spriteBatch.Draw(traces[i], new Vector2(0), new Color(1-alphas[i],0f, 0f, alphas[i]*Alpha));
            }
			spriteBatch.End();
			//device.DepthStencilBuffer = GraphicEffect.CreateDepthStencilBuffer(RenderTarget);
			//device.SetRenderTarget(0, RenderTarget);
			
			//blurTex = target.GetTexture();
			//spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState);
			//spriteBatch.Draw(tex, Vector2.Zero, Color.White);
			//spriteBatch.Draw(blurTex, new Vector2(100), new Color(Color.White, 1f));
			//spriteBatch.End();
            
        }

        protected override void ResetEffect()
        {
            for (int i = 0; i < traces.Length; i++ )
            {
                traces[i] = null;
            }
        }

        protected override void SetEffectParameters()
        {
        }
    }
}
