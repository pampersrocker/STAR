using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Star
{
	public static class Extensions
	{
		public static string ToSpacedString(this object e,string spacer)
		{
			return e.ToString().Replace(spacer, " ");
		}

		public static string ToSpacedString(this object e)
		{
			return ToSpacedString(e, "_");
		}

		/// <summary>
		/// Gets the next Value in its Enum list
		/// </summary>
		/// <typeparam name="T">The Enum Type</typeparam>
		/// <param name="e">The Current Enum Value</param>
		/// <exception cref="ArgumentException">T must be an Enum.</exception>
		/// <returns>The Next Value</returns>
		public static T GetNextInEnumList<T>(this T e)
			where T : struct, IConvertible, IComparable, IFormattable
		{
			return GetRelativeElement(e, +1);
		}

		/// <summary>
		/// Gets the previous Value in its Enum list
		/// </summary>
		/// <typeparam name="T">The Enum Type</typeparam>
		/// <param name="e">The Current Enum Value</param>
		/// <exception cref="ArgumentException">T must be an Enum.</exception>
		/// <returns>The prevous Value</returns>
		public static T GetPreviousInEnumList<T>(this T e)
			where T : struct, IConvertible, IComparable, IFormattable
		{
			return GetRelativeElement(e, -1);
		}

		/// <summary>
		/// Gets the relative Value positioned to <c>e</c> in its Enum list
		/// </summary>
		/// <typeparam name="T">The Enum Type</typeparam>
		/// <param name="e">The Current Enum Value</param>
		/// <exception cref="ArgumentException">T must be an Enum.</exception>
		/// <returns>The Next Value</returns>
		public static T GetRelativeElement<T>(this T e,int offset)
			where T : struct, IConvertible, IComparable, IFormattable
		{
			if (!typeof(T).IsEnum)
			{
				throw new ArgumentException("T must be an Enum!");
			}
			int position = e.ToInt32(System.Globalization.CultureInfo.CurrentCulture.NumberFormat);
			T[] values = (T[])Enum.GetValues(typeof(T));
			int length = values.Length;
			
			offset -= (offset / length) * length ;
			if (position + offset < 0)
				offset += length;
			else if(position+offset >= length)
				offset -= length;
			return values[position + offset];
		}

		public static T GetRelativeElement<T>(this List<T> e, int position, int offset)
		{
			int length = e.Count;

			offset -= (offset / length) * length;
			if (position + offset < 0)
				offset += length;
			else if (position + offset >= length)
				offset -= length;
			return e[position + offset];
		}

		public static T GetRelativeElement<T>(this List<T> e, T value, int offset)
		{
			return e.GetRelativeElement(e.IndexOf(value), offset);
		}

		public static string DictToString<Tkey,TValue>(this Dictionary<Tkey,TValue> e)
		{
			string data = "";
			foreach (Tkey key in e.Keys)
			{
				data += key + "=" + e[key] + ";\n";
			}

			return data;
		}

		public static RenderTarget2D GetCurrentRenderTarget(this GraphicsDevice device)
		{
			return (RenderTarget2D)device.GetRenderTargets()[0].RenderTarget;
		}

		/// <summary>
		/// Returns the Elapsed Total Seconds from the last Picture in float
		/// </summary>
		/// <param name="gameTime"></param>
		/// <returns></returns>
		public static float GetElapsedTotalSecondsFloat(this GameTime gameTime)
		{
			return (float)gameTime.ElapsedGameTime.TotalSeconds;
		}

		public static Vector2 ToVector2(this Point p)
		{
			return new Vector2(p.X, p.Y);
		}
	}
}
