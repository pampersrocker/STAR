using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace AStarPathFinding
{
	public class Path : IEnumerable
	{
		readonly PathNode[] path;

		public Path()
		{
			path = new PathNode[1];
			path[0] = new PathNode(0, 0, Walkable.Walkable);
		}

		public Path(List<PathNode> nodes)
		{
			//path = new PathNode[nodes.Count];
			path = nodes.ToArray();
		}

		public PathNode this[int index]
		{
			get { return path[index]; }
		}

		public int Length
		{
			get { return path.Length; }
		}

		public float Cost
		{
			get { return path[path.Length - 1].FCost; }
		}

		#region IDisposable Member

		public void Dispose()
		{
		}

		#endregion

		#region IEnumerable Member

		IEnumerator IEnumerable.GetEnumerator()
		{
			return new PathEnumerator(path);
		}

		#endregion
	}

	public class PathEnumerator : IEnumerator
	{
		PathNode[] nodes;
		int position;
		public PathEnumerator(PathNode[] nodes)
		{
			this.nodes = nodes;
			position = -1;
		}



		#region IEnumerator Member

		public object Current
		{
			get 
			{
				try
				{
					return nodes[position];
				}
				catch (IndexOutOfRangeException)
				{
					throw new InvalidOperationException();
				}
			}
		}

		public bool MoveNext()
		{
			position++;
			return position < nodes.Length;
		}

		public void Reset()
		{
			position = -1;
		}

		#endregion
	}

}
