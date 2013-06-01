using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Star.Graphics.Effects.PostProcessEffects
{
	/// <summary>
	/// Extends Flagged Enums with a Contains Method.
	/// Code from: http://dotnet-snippets.de/dns/enum-extension---flag-in-einem-bitfeld-gesetzt-SID1290.aspx
	/// </summary>
	public static class FlaggedEnumExtensions
	{
		/// <summary>
		/// Überprüft, ob in dem Bitfeld en1 das Bitfeld en2 enthalten ist.
		/// </summary>
		/// <param name="en1">Bitfeld 1</param>
		/// <param name="en2">Bitfeld 2</param>
		/// <returns><c>True</c>, wenn en2 in en1 enthalten ist, sonst <c>False</c>.</returns>
		/// <remarks>Bei dieser überlagerten Version der Methode wird nicht überprüft, ob die beiden Bitfelder gleichen Typs sind.
		/// <para>Verwenden Sie daher bitte die generische Version dieser Methode, wenn Sie sicherstellen müssen, dass beide Bitfelder gleichen Typs sind.</para></remarks>
		public static bool Contains(this Enum en1, Enum en2)
		{
			return ((Convert.ToInt32(en1) & Convert.ToInt32(en2)) == Convert.ToInt32(en2));
		}

		/// <summary>
		/// Überprüft, ob in dem Bitfeld en1 das Bitfeld en2 enthalten ist.
		/// </summary>
		/// <typeparam name="TFlaggedEnum">Parameter, der den Typ der Bitfelder angibt.</typeparam>
		/// <param name="en1">Bitfeld 1</param>
		/// <param name="en2">Bitfeld 2</param>
		/// <returns><c>True</c>, wenn en2 in en1 enthalten ist, sonst <c>False</c>.</returns>
		public static bool Contains<TFlaggedEnum>(this Enum en1, Enum en2)
		{
			if (!(Attribute.IsDefined(typeof(TFlaggedEnum), typeof(FlagsAttribute)))) return false;
			if ((en1.GetType().ToString() != en2.GetType().ToString())) return false;
			if ((en1.GetType().ToString() != typeof(TFlaggedEnum).ToString())) return false;
			return ((Convert.ToInt32(en1) & Convert.ToInt32(en2)) == Convert.ToInt32(en2));
		}

	}
}
