using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Star.Game.Level;
using Star.GameManagement;

namespace Star.Graphics.Effects.PostProcessEffects
{
	/// <summary>
	/// This Class is based on the Description and Code from here: 
	/// http://www.messy-mind.net/2008/fast-gpu-color-transforms/
	/// </summary>
	public class ColorizeLUT : GraphicEffect, IGraphicsChange
	{
		LayerFXData fxData;
		GraphicsDevice device;
		public LayerFXData FxData
		{
			get { return fxData; }
			set 
			{
				fxData = value;
				CreateLUT(device);
			}
		}
		int size = 32;
		Texture3D lookupTable;
		protected override AvailableEffects InitializeEffect(GraphicsDevice device, GameManagement.Options options)
		{
			GraphicsManager.AddItem(this);

			this.device = device;
			fxData = LayerFXData.Default;
			
			switch (options.QualitySettings[GameManagement.OptionsID.ShaderQuality])
			{ 
				case GameManagement.QualitySetting.High:
					size = 32;
					break;
				case GameManagement.QualitySetting.Middle:
					size = 16;
					break;
				case GameManagement.QualitySetting.Low:
					size = 8;
					break;
			}
			CreateLUT(device);
			return AvailableEffects.ColorizeLUT;
		}

		protected override void ResetEffect()
		{
			fxData = LayerFXData.Default;
		}

		protected override void SetEffectParameters()
		{
			effect.Parameters["cubeTex"].SetValue(lookupTable);
			effect.Parameters["Size"].SetValue(size);
		}

		private float __min_channel(Vector3 v)
		{
			float t = (v.X < v.Y) ? v.X : v.Y;
			t = (t < v.Z) ? t : v.Z;
			return t;
		}

		private float __max_channel(Vector3 v)
		{
			float t = (v.X > v.Y) ? v.X : v.Y;
			t = (t > v.Z) ? t : v.Z;
			return t;
		}

		/// <summary>
		/// Adapted from color_spaces.fxh HLSL Code
		/// </summary>
		/// <param name="HSV"></param>
		/// <returns></returns>
		private Vector3 hsv_to_rgb(Vector3 HSV)
		{
			Vector3 RGB = new Vector3(HSV.Z);
			if (HSV.Y != 0)
			{
				float var_h = HSV.X * 6;
				float var_i = (float)Math.Floor(var_h);   // Or ... var_i = floor( var_h )
				float var_1 = HSV.Z * (1.0f - HSV.Y);
				float var_2 = HSV.Z * (1.0f - HSV.Y * (var_h - var_i));
				float var_3 = HSV.Z * (1.0f - HSV.Y * (1 - (var_h - var_i)));
				if (var_i == 0) { RGB = new Vector3(HSV.Z, var_3, var_1); }
				else if (var_i == 1) { RGB = new Vector3(var_2, HSV.Z, var_1); }
				else if (var_i == 2) { RGB = new Vector3(var_1, HSV.Z, var_3); }
				else if (var_i == 3) { RGB = new Vector3(var_1, var_2, HSV.Z); }
				else if (var_i == 4) { RGB = new Vector3(var_3, var_1, HSV.Z); }
				else { RGB = new Vector3(HSV.Z, var_1, var_2); }
			}
			return (RGB);
		}

		/// <summary>
		/// Adapted from color_spaces.fxh HLSL Code
		/// </summary>
		/// <param name="HSV"></param>
		/// <returns></returns>
		private Vector3 rgb_to_hsv(Vector3 RGB)
		{
			Vector3 HSV = new Vector3();
			float minVal = __min_channel(RGB);
			float maxVal = __max_channel(RGB);
			float delta = maxVal - minVal;             //Delta RGB value 
			HSV.Z = maxVal;
			if (delta != 0)
			{                    // If gray, leave H & S at zero
				HSV.Y = delta / maxVal;
				Vector3 delRGB;
				delRGB = (((new Vector3(maxVal) - RGB) / 6.0f) + new Vector3(delta / 2.0f)) / delta;
				if (RGB.X == maxVal) HSV.X = delRGB.Z - delRGB.Y;
				else if (RGB.Y == maxVal) HSV.X = (1.0f / 3.0f) + delRGB.X - delRGB.Z;
				else if (RGB.Z == maxVal) HSV.X = (2.0f / 3.0f) + delRGB.Y - delRGB.X;
				if (HSV.X < 0.0) { HSV.X += 1.0f; }
				if (HSV.X > 1.0) { HSV.X -= 1.0f; }
			}
			return (HSV);
		}

		private void CreateLUT(GraphicsDevice device)
		{
			if (lookupTable != null)
				lookupTable.Dispose();
			lookupTable = new Texture3D(device, size, size, size, false, SurfaceFormat.Color);
			Color[] colors = new Color[size * size * size];
			Vector3 baseColor;
			for (int r = 0; r < size; r++)
			{
				for (int g = 0; g < size; g++)
				{
					for (int b = 0; b < size; b++)
					{
						baseColor = new Vector3((float)r / size, (float)g / size, (float)b / size);

						#region HSV Transformation
						baseColor = rgb_to_hsv(baseColor);
						if (fxData.HueEnabled)
						{
							if (!fxData.Colorize)
							{
								baseColor.X += fxData.Hue;
								if (baseColor.X >= 360)
								{
									baseColor.X -= 360;
								}
								else if (baseColor.X < 0)
								{
									baseColor.X += 360;
								}
							}
							else
								baseColor.X = fxData.Hue;
						}

						if (fxData.SaturationEnabled)
						{
							baseColor.Y *= fxData.Saturation; 
						}
						if (fxData.ContrastEnabled)
						{
							baseColor.Z = baseColor.Z * fxData.Contrast + fxData.Brightness;
						}
						baseColor = hsv_to_rgb(baseColor);

						#endregion						

						colors[r + g * size + b * size * size] = new Color(baseColor);
					}
				}
			}

			lookupTable.SetData<Color>(colors);
		}

		public override void Dispose()
		{
			try
			{
				UnloadGraphicsChanged();
				lookupTable.Dispose();
				base.Dispose();
			}
			catch (Exception)
			{
				
			}
		}

		#region IGraphicsChange Member

		public void GraphicsChanged(GraphicsDevice device, GameManagement.Options options)
		{
			int tempSize = size;
			switch (options.QualitySettings[GameManagement.OptionsID.ShaderQuality])
			{
				case GameManagement.QualitySetting.High:
					tempSize = 32;
					break;
				case GameManagement.QualitySetting.Middle:
					tempSize = 16;
					break;
				case GameManagement.QualitySetting.Low:
					tempSize = 8;
					break;
			}
			if (tempSize != size)
			{
				size = tempSize;
				CreateLUT(device);
			}
		}

		public void UnloadGraphicsChanged()
		{
			GraphicsManager.RemoveItem(this);
		}

		#endregion
	}
}
