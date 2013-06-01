using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AStarPathFinding
{
	public static class RectangleExtensions
	{
		public static Vector2 CenterToVector2(this Rectangle rect)
		{
			return new Vector2(rect.Center.X, rect.Center.Y);
		}
	}
}
