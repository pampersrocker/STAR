using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Globalization;

namespace Star.Game.Level
{
	/// <summary>
	/// Defines an Rectangle with Rotation and Translation
	/// </summary>
	public struct ExtendedRectangle
	{
		private Rectangle transformedRectangle;


		private Vector2 transformedPos;
		private Vector2 size;
		public float Rotation;
		private Vector2 origin;
		Vector2 textureSize;
		private Rectangle originalRectangle;
		Vector2 translation;

		public static ExtendedRectangle CreateFromString(string data,Vector2 textureSize)
		{
			ExtendedRectangle rect = new ExtendedRectangle();

			string[] values = data.Split(',');
			if (values.Length >= 9)
			{
				Vector2 pos = new Vector2();
				Vector2 size = new Vector2();
				float Rotation = 0;
				Vector2 origin = new Vector2();
				Vector2 translation = new Vector2();

				try
				{
					CultureInfo info = CultureInfo.CreateSpecificCulture("en-us");
					int i = 0;
					pos.X = float.Parse(values[i], info);
					i++;
					pos.Y = float.Parse(values[i], info);
					i++;
					size.X = float.Parse(values[i], info);
					i++;
					size.Y = float.Parse(values[i], info);
					i++;
					rect.size = size;
					rect.originalRectangle = new Rectangle((int)pos.X, (int)pos.Y, (int)size.X, (int)size.Y);
					origin.X = float.Parse(values[i], info);
					i++;
					origin.Y = float.Parse(values[i], info);
					i++;
					rect.origin = origin;
					translation.X = float.Parse(values[i], info);
					i++;
					translation.Y = float.Parse(values[i], info);
					i++;
					rect.translation = translation;
					Rotation = float.Parse(values[i], info);
					rect.Rotation = Rotation;
				}
				catch (Exception)
				{

					throw;
				}
				finally
				{
					rect = ExtendedRectangle.Transform(rect.originalRectangle, textureSize, rect.Origin, rect.Translation, rect.Rotation);
				}
			}
			else
				Debug.DebugManager.AddItem("Failed to Create Extended Rectangle: Not enough Values", "ExtendedRectangleCreator", new System.Diagnostics.StackTrace(), System.Drawing.Color.Yellow);

			return rect;
		}

		public string GetDataString()
		{
			CultureInfo info = CultureInfo.CreateSpecificCulture("en-us");
			string data = "";
			data += originalRectangle.X.ToString() + ",";
			data += originalRectangle.Y.ToString() + ",";
			data += originalRectangle.Width.ToString() + ",";
			data += originalRectangle.Height.ToString() + ",";
			data += origin.X.ToString(info) + ",";
			data += origin.Y.ToString(info) + ",";
			data += translation.X.ToString(info) + ",";
			data += translation.Y.ToString(info) + ",";
			data += Rotation.ToString(info);
			return data;
		}

		/// <summary>
		/// The Origin Values used for the SpriteBatch
		/// </summary>
		public Vector2 DrawOrigin
		{
			get
			{
				return new Vector2(
				(origin.X / size.X) * textureSize.X,
				(origin.Y / size.Y) * textureSize.Y);
			}
		}

		/// <summary>
		/// Gets the Size of the Rectangle
		/// </summary>
		public Vector2 Size
		{
			get { return size; }
		}

		/// <summary>
		/// Gets the transformed Rectangle
		/// </summary>
		public Rectangle TransformedRectangle
		{
			get { return transformedRectangle; }
		}

		/// <summary>
		/// Gets the Origin
		/// </summary>
		public Vector2 Origin
		{
			get { return origin; }
		}

		/// <summary>
		/// Gets the Transformed Position of the Rectangle
		/// </summary>
		public Vector2 TransformedPos
		{
			get { return transformedPos; }
		}

		/// <summary>
		/// Gets the original Rectangle
		/// </summary>
		public Rectangle OrgRectangle
		{
			get { return originalRectangle; }
		}

		/// <summary>
		/// Gets the Translation
		/// </summary>
		public Vector2 Translation
		{
			get { return translation; }
		}

		/// <summary>
		/// Transforms the <c>orgRect</c> with the specified Origin, Translation and Rotation
		/// </summary>
		/// <param name="orgRect">the Rectangle which will be tranformed</param>
		/// <param name="textureSize">The Size of the Texture which will be drawn (needed for the correct Origin</param>
		/// <param name="origin">The new Origin</param>
		/// <param name="translation">The new Translation</param>
		/// <param name="rotate">The new Rotation</param>
		/// <returns>A Transformed Rectangle</returns>
		public static ExtendedRectangle Transform(Rectangle orgRect,Vector2 textureSize, 
			Vector2? origin, Vector2 translation, float rotate)
		{
			ExtendedRectangle rect = new ExtendedRectangle();
			rect.textureSize = textureSize;
			rect.Rotation = rotate;
			rect.transformedPos = new Vector2(orgRect.X, orgRect.Y);
			rect.size = new Vector2(orgRect.Width, orgRect.Height);
			rect.origin = origin.HasValue ? origin.Value : Vector2.Zero;

			Matrix matrix =
				Matrix.CreateTranslation(new Vector3(translation, 0));

			rect.translation = translation;
			rect.originalRectangle = orgRect;
			rect.transformedPos = Vector2.Transform(rect.transformedPos, matrix) + rect.origin;
			rect.transformedRectangle = new Rectangle(
				(int)rect.transformedPos.X, 
				(int)rect.transformedPos.Y, 
				(int)rect.size.X, 
				(int)rect.size.Y);
			return rect;
		}

	}
}
