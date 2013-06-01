using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AStarPathFinding
{
	public struct Node
	{
		private Rectangle rect;

		public Rectangle Rectangle
		{
			get { return rect; }
		}
		private Walkable walkable;

		public Walkable Walkable
		{
			get { return walkable; }
		}
		private int mapXPosition;

		public int MapXPosition
		{
			get { return mapXPosition; }
		}
		private int mapYPosition;

		public int MapYPosition
		{
			get { return mapYPosition; }
		}

		public Node(int x, int y, Walkable walkable)
			: this(x, y, 0, walkable) { }

		public Node(int x, int y, int size, Walkable walkable)
		{
			mapXPosition = x;
			mapYPosition = y;
			this.walkable = walkable;
			rect = new Rectangle(x*size, y*size, size, size);
		}

		public PathNode ToPathNode()
		{
			PathNode node;

			node = new PathNode(mapXPosition, mapYPosition,rect.Width, walkable);

			return node;
		}
	}
}
