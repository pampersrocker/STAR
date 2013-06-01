using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Collections;

namespace AStarPathFinding
{
	public class PathNode
	{
		#region Node
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

		//public Node(int x, int y, Walkable walkable)
		//    : this(x, y, 0, walkable) { }

		//public Node(int x, int y, int size, Walkable walkable)
		//{
		//    mapXPosition = x;
		//    mapYPosition = y;
		//    this.walkable = walkable;
		//    rect = new Rectangle(x*size, y*size, size, size);
		//}

		public PathNode ToPathNode()
		{
			PathNode node;

			node = new PathNode(mapXPosition, mapYPosition,rect.Width, walkable);

			return node;
		}

		public void FromNode(Node node)
		{
			mapXPosition = node.MapXPosition;
			mapYPosition = node.MapYPosition;
			this.walkable = node.Walkable;
			int size = node.Rectangle.Width;
			rect = new Rectangle(mapXPosition * size, mapYPosition * size, size, size);
			switch (walkable)
			{
				case Walkable.Walkable:
					state = NodeState.Unknown;
					break;
				case Walkable.Blocked:
					state = NodeState.Closed;
					break;
			}
		}
		#endregion
		public float hCost;
		float fCost;
		float fCostNoAim = -1;

		public float FCostNoAim
		{
			get { return fCostNoAim; }
		}

		public float FCost
		{
			get { return fCost; }
		}

		public float HCost
		{
			get { return hCost; }
		}
		NodeState state;
		PathNode rootNode;

		public PathNode RootNode
		{
			get { return rootNode; }
			set { rootNode = value; }
		}

		public NodeState State
		{
			get { return state; }
			set { state = value; }
		}

		public PathNode(int x, int y, Walkable walkable)
			: this(x, y,10, walkable)
		{
		}

		public PathNode(int x, int y, int size, Walkable walkable)
		{
			mapXPosition = x;
			mapYPosition = y;
			this.walkable = walkable;
				rect = new Rectangle(x*size, y*size, size, size);
			switch (walkable)
			{
				case Walkable.Walkable:
					state = NodeState.Unknown;
					break;
				case Walkable.Blocked:
					state = NodeState.Closed;
					break;
			}
		}

		public float CalculateHCost(Vector2 aim)
		{
			hCost = (aim - Rectangle.CenterToVector2()).Length();
			return hCost;
		}

		public float CalculateFCost(Vector2 aim,PathNode root)
		{
			if (rootNode == null)
				return CalculateHCost(aim);
			else
				return CalculateFCost(this,root) + CalculateHCost(aim);
		}

		public float CalculateFCost(Vector2 aim)
		{
			//fCost = CalculateFCost(aim, rootNode);
			if (rootNode == null)
				fCost = CalculateHCost(aim);
			else
			{
				fCostNoAim = CalculateFCost(this, rootNode);
				fCost = fCostNoAim + CalculateHCost(aim);
			}
			return fCost;
		}

		protected float CalculateFCost(PathNode next,PathNode root)
		{
			float h=0;
			if (root != null)
				if (fCostNoAim != -1)
					h += fCostNoAim;
			else
				h += root.CalculateFCost(this,rootNode);
			h += (next.Rectangle.CenterToVector2() - Rectangle.CenterToVector2()).Length()/2f;
			return h;
		}

		public override string ToString()
		{
			return base.ToString() + "X: " + MapXPosition + " Y: " + MapYPosition;
		}

	}

	public class PathNodeComparer : IComparer<PathNode>
	{

		#region IComparer<PathNode> Member

		public int Compare(PathNode x, PathNode y)
		{
			//if (y != null && x != null)
			{
				return (int)(x.FCost - y.FCost);
			}
			//else if (x == null && y == null)
			//    return 0;
			//else if (y == null)
			//    return 1;
			//else
			//    return -1;

		}

		#endregion
	}

	public class PathNodeComparerIndex : IEqualityComparer<PathNode>
	{

		#region IComparer<PathNode> Member

		public int Compare(PathNode x, PathNode y)
		{
			//if (y != null && x != null)
			{
				return (int)(x.MapXPosition-y.MapXPosition + x.MapYPosition-y.MapYPosition);
			}
			//else if (x == null && y == null)
			//    return 0;
			//else if (y == null)
			//    return 1;
			//else
			//    return -1;

		}

		#endregion

		#region IEqualityComparer<PathNode> Member

		public bool Equals(PathNode x, PathNode y)
		{
			return (x.MapXPosition == y.MapXPosition && x.MapYPosition == y.MapYPosition) ? true : false;
		}

		public int GetHashCode(PathNode obj)
		{
			return obj.MapXPosition << 16 + obj.MapYPosition;
		}

		#endregion
	}

}
