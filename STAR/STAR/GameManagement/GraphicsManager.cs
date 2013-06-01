using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Star.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace Star.GameManagement
{
	public static class GraphicsManager
	{
		static List<IGraphicsChange> graphicsChanger = new List<IGraphicsChange>();

		public static void AddItem(IGraphicsChange item)
		{
			graphicsChanger.Add(item);
		}

		public static void Clear()
		{
			graphicsChanger.Clear();
		}

		public static void RemoveItem(IGraphicsChange item)
		{
			graphicsChanger.Remove(item);
		}

		public static void GraphicsChange(GraphicsDevice device,Options options)
		{
			foreach (IGraphicsChange changeItem in graphicsChanger)
				changeItem.GraphicsChanged(device,options);
		}
	}
}
