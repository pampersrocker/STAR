using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace AStarPathFinding
{
	public class AStarMap
	{
		static Node[,] mapnodes;

		public static Node[,] Mapnodes
		{
			get { return AStarMap.mapnodes; }
		}

		public AStarMap()
		{
			if (mapnodes == null)
			{
				mapnodes = new Node[1, 1];
				mapnodes[0, 0] = new Node(0,0,Walkable.Walkable);
			}
		}

        public static void InitializeMap(Node[,] nodes)
        {
            if (nodes != null)

                mapnodes = nodes;

        }

		public static int GetLength(int dimension)
		{
			if (dimension >= 0 && dimension < 2)
				return mapnodes.GetLength(dimension);
			else
				throw new ArgumentOutOfRangeException();
		}

		public Node this[int y,int x]
		{
			get
			{
				try
				{
					return mapnodes[y, x]; 
				}
				catch (IndexOutOfRangeException e)
				{
					string error = "";
					foreach (DictionaryEntry entry in e.Data)
					{
						error += "Key: " + entry.Key + ", Value: " + entry.Value + "\n";
					}
					error += "X Max Value: " + (mapnodes.GetLength(0) -1) + "\n";
					error += "Y Max Value: " + (mapnodes.GetLength(1) - 1) + "\n";
					throw new ArgumentException(error);
				}
				
			}
		}
	}
}
