using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Star.Game.Level;
using Star.GameManagement;

namespace Star.Graphics.Effects
{
	public enum EffectType
	{ 
		ScreenEffect,
		RelativeToPosition,
		OnTexture,
	}

	public enum AvailableEffects
	{
		Wave,
		KillEffect,
		Grayscale,
		Colorize,
		GaussianBlur,
		ToonShader,
		ColorizeLUT,
		None
	}

	public abstract class GraphicEffect : IDisposable
	{
		ContentManager content;
		protected Effect effect;
		protected Vector2 position;
		AvailableEffects type;
		bool initialized = false;
		bool enabled = false;
		bool isPostProcessEffect;
		static RenderTarget2D resolvedTex;
		Texture2D tex;
		float alpha = 1;

		public float Alpha
		{
			get { return alpha; }
			set { alpha = MathHelper.Clamp(value, 0, 1); }
		}

		static RenderTarget2D currentTarget;
		static RenderTarget2D target1, target2;
		static GraphicsDevice device;
		static Texture2D resolvedMultiEffectTex;
		static bool multiEffectEnabled;
		static SpriteBatch spriteBatch;

		

		public static RenderTarget2D RenderTarget
		{
			get { return currentTarget; }
		}

		public static void BeginMultiEffect()
		{
			if (!multiEffectEnabled)
			{
				PresentationParameters pp = device.PresentationParameters;
				if (currentTarget != null)
					currentTarget.Dispose();
				currentTarget = new RenderTarget2D(device, pp.BackBufferWidth, pp.BackBufferHeight,false , pp.BackBufferFormat, DepthFormat.Depth24);
				device.SetRenderTarget(currentTarget);
				if (resolvedMultiEffectTex == null)
					resolvedMultiEffectTex = new Texture2D(device, device.PresentationParameters.BackBufferWidth, device.PresentationParameters.BackBufferHeight);
				multiEffectEnabled = true;
				if (spriteBatch == null)
					spriteBatch = new SpriteBatch(device);
				if (target1 != null)
					target1.Dispose();
				target1 = new RenderTarget2D(device, pp.BackBufferWidth, pp.BackBufferHeight,false, pp.BackBufferFormat, DepthFormat.Depth24);
				if (target2 != null)
					target2.Dispose();
				target2 = new RenderTarget2D(device, pp.BackBufferWidth, pp.BackBufferHeight, false, pp.BackBufferFormat, DepthFormat.Depth24);
				//Cleaning up...
				GC.Collect();
			}
			else
				throw new InvalidOperationException("EndMultiEffect must be Called before it can be started again.");
		}

		public static void EndMultiEffect(List<GraphicEffect> effects)
		{
			if (multiEffectEnabled)
			{
				//int switcher = 0;
				////Get Texture to draw multiple Effects
				////device.ResolveBackBuffer(resolvedMultiEffectTex);
				//device.GetBackBufferData<Texture2D>(resolvedMultiEffectTex);
				
				//foreach (GraphicEffect effect in effects)
				//{
				//    //Get last Drawn Texture
				//    device.ResolveBackBuffer(resolvedMultiEffectTex);
				//    //Swapping Targets between target1 and target2 
				//    //To Prevent unwanted Alpha Overlays
				//    device.SetRenderTarget(0, switcher++ % 2 == 0 ? target1 : target2);
				//    if (!effect.IsPostProcessEffect)
				//    {
				//        spriteBatch.Begin();
				//        effect.Begin();
				//        spriteBatch.Draw(resolvedMultiEffectTex, Vector2.Zero, Color.White);
				//        effect.End();
				//        spriteBatch.End();
				//    }
				//    else
				//        effect.DrawPostProcess(spriteBatch, resolvedMultiEffectTex);
				//}
				//device.ResolveBackBuffer(resolvedMultiEffectTex);
				////Set RenderTarget back to Screen
				//device.SetRenderTarget(0, null);
				//spriteBatch.Begin();
				//spriteBatch.Draw(resolvedMultiEffectTex, Vector2.Zero, Color.White);
				//spriteBatch.End();
				//multiEffectEnabled = false;
			}
			else
				throw new InvalidOperationException("BeginMultiEffect must be called before you can stop it.");
		}


		/// <summary>
		/// Returns the ContentManager
		/// </summary>
		protected ContentManager Content
		{
			get { return content; }
		}

		protected bool IsPostProcessEffect
		{
			get { return isPostProcessEffect; }
			set { isPostProcessEffect = value; }
		}

		/// <summary>
		/// Gets or sets the State of the Effect
		/// Enabled can only be cahnged if it was Initialized
		/// </summary>
		public bool Enabled
		{
			get { return enabled; }
			set
			{
				if (initialized)
				{
					enabled = value;
				}
			}
		}

		/// <summary>
		/// Gets or sets the Effect
		/// </summary>
		public Effect Effect
		{
			get { return effect; }
			set { effect = value; }
		}

		/// <summary>
		/// Initializes The Effect
		/// </summary>
		/// <param name="serviceProvider">The Service Provider for the Content</param>
		/// <param name="device">GraphicsDevice</param>
		/// <param name="options">For The Screenresolution</param>
		public void Initialize(IServiceProvider serviceProvider, GraphicsDevice device, Options options)
		{
			PresentationParameters pp = device.PresentationParameters;
			resolvedTex = new RenderTarget2D(
				device,
				pp.BackBufferWidth,
				pp.BackBufferHeight,
				false,
				SurfaceFormat.Color,
				DepthFormat.None,
				pp.MultiSampleCount,
				RenderTargetUsage.PreserveContents);
			content = new ContentManager(serviceProvider, "Data");
			position = new Vector2();
			type = InitializeEffect(device, options);
			if (type != AvailableEffects.None)
				effect = content.Load<Effect>(GameConstants.EffectsPath + type.ToString());
			SetEffectParameters();
			initialized = true;
			GraphicEffect.device = device;
		}

		/// <summary>
		/// Updates the Effect
		/// Only when Effect is initialized && Enabled
		/// </summary>
		/// <param name="gameTime">GameTime</param>
		/// <param name="pos">dummy</param>
		public void Update(GameTime gameTime,Vector2 pos)
		{
			if (initialized && enabled)
			{
				UpdateEffect(gameTime, pos);
			}
		}

		/// <summary>
		/// Starts or Resets the Effect, only when initialized
		/// </summary>
		public void StartResetEffect()
		{
			if (initialized)
			{
				enabled = true;
				ResetEffect();
			}
		}

		public void ApplyParameters()
		{
			SetEffectParameters();
		}

		public virtual void Begin()
		{
			//3_1
			//effect.Begin();
			//effect.CurrentTechnique.Passes["Pass1"].Begin();
			throw new InvalidOperationException("This has to be fixed due to Upgrade to XNA 4.0");
		}

		public virtual void End()
		{
			//3_1
			//effect.CurrentTechnique.Passes["Pass1"].End();
			//effect.End();
			throw new InvalidOperationException("This hat to be fixed due to Upgrade to XNA 4.0");
		}

		/// <summary>
		/// Draws The Effect
		/// </summary>
		/// <param name="spriteBatch"></param>
		/// 
		public void DrawPostProcess(SpriteBatch spriteBatch)
		{
			GraphicsDevice device = spriteBatch.GraphicsDevice;
			if (initialized && enabled)
			{
				SetEffectParameters();
				if (device.PresentationParameters.BackBufferWidth != resolvedTex.Width ||
					device.PresentationParameters.BackBufferHeight != resolvedTex.Height)
				{
					ResolutionChanged(device.PresentationParameters.BackBufferWidth, device.PresentationParameters.BackBufferHeight);
				}
				tex = (RenderTarget2D)device.GetRenderTargets()[0].RenderTarget;
				RenderTarget2D target = (RenderTarget2D)device.GetRenderTargets()[0].RenderTarget;
				device.SetRenderTarget(resolvedTex);
				DrawPostProcess(spriteBatch, tex);
				device.SetRenderTarget(target);
				spriteBatch.Begin( SpriteSortMode.Immediate, BlendState.NonPremultiplied);
				spriteBatch.Draw(resolvedTex, Vector2.Zero, new Color(1f, 1f, 1f,Alpha));
				spriteBatch.End();
			}
		}

		public void DrawPostProcess(SpriteBatch spriteBatch, Texture2D tex)
		{
			DrawEffect(tex, spriteBatch, spriteBatch.GraphicsDevice);
		}

		public void ResolutionChanged(int width, int height)
		{
			
			int levelCount = resolvedTex.LevelCount;
			SurfaceFormat format = resolvedTex.GraphicsDevice.PresentationParameters.BackBufferFormat;
			GraphicsDevice device = resolvedTex.GraphicsDevice;
			resolvedTex.Dispose();
			resolvedTex = new RenderTarget2D(device, width, height, false, format, DepthFormat.None, device.PresentationParameters.MultiSampleCount, RenderTargetUsage.PreserveContents);
			//resolvedTex = new ResolveTexture2D(
			//    device, 
			//    width, 
			//    height, 
			//    1, 
			//    format);
		}

		#region AbstractMethods

		/// <summary>
		/// Initializes the specific Effect
		/// </summary>
		/// <param name="device"></param>
		/// <param name="options"></param>
		/// <returns></returns>
		protected abstract AvailableEffects InitializeEffect(GraphicsDevice device, Options options);
		
		/// <summary>
		/// Updates the Specified Effect
		/// </summary>
		/// <param name="gameTime"></param>
		/// <param name="pos"></param>
		protected virtual void UpdateEffect(GameTime gameTime, Vector2 pos)
		{ 
		
		}
		
		/// <summary>
		/// Draws The Specified Effect
		/// </summary>
		/// <param name="tex">ScreenTexture</param>
		/// <param name="spriteBatch"></param>
		/// <param name="device"></param>
		protected virtual void DrawEffect(Texture2D tex, SpriteBatch spriteBatch, GraphicsDevice device)
		{
			spriteBatch.Begin(SpriteSortMode.Immediate,
				BlendState.NonPremultiplied,
				null,
				null,
				null,
				effect);
			spriteBatch.Draw(tex, Vector2.Zero, new Color(1f, 1f, 1f, alpha));
			spriteBatch.End();
		}
		
		/// <summary>
		/// Resets The specified Effect
		/// </summary>
		protected abstract void ResetEffect();

		/// <summary>
		/// Initializes the Effect parameters
		/// </summary>
		protected abstract void SetEffectParameters();

		#endregion

		#region IDisposable Member

		public virtual void Dispose()
		{
			//if (tex != null)
			//    tex.Dispose();
		}

		#endregion
	}
}
