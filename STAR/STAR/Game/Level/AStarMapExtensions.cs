using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AStarPathFinding;

namespace Star.Game.Level
{
	public static class AStarMapExtensions
	{
		public static void InitializeMapFromLevel(this AStarMap map, Tile[,] maptiles)
		{
			int yLength = maptiles.GetLength(0);
			int xLength = maptiles.GetLength(1);
			Node[,] aStarNodes;
			aStarNodes = new Node[yLength, xLength];
			for (int y = 0; y < yLength; y++)
			{
				for (int x = 0; x < xLength; x++)
				{
					Walkable walkable = Walkable.Walkable;
					switch (maptiles[y, x].TileType)
					{
						case TileType.Wall:
						case TileType.Spike:
							walkable = Walkable.Blocked;
							break;

                        case TileType.Platform:
                            walkable = Walkable.Platform;
                            break;
					}
					if (walkable == Walkable.Walkable)
					{ 
						if (CheckForUnreachable(maptiles, y, x))
							walkable = Walkable.NotReachable;

					}

					aStarNodes[y, x] = new Node(x, y, Tile.TILE_SIZE, walkable);
				}
			}
			AStarMap.InitializeMap(aStarNodes);

		}

		/// <summary>
		/// Checks if the specified Tile is Unreachable for the AStar Algorithm.<para>(KI can not reach it by walking or jumping)</para>
		/// </summary>
		/// <param name="maptiles">The Level Tiles</param>
		/// <param name="tileX">x Coordinate of the Specified Tile</param>
		/// <param name="tileY">y Coordinate of the Specified Tile</param>
		/// <returns><para>True if tile is unreachable</para>, <para>Fals if tile is reachable.</para></returns>
		private static bool CheckForUnreachable(Tile[,] maptiles, int tileX, int tileY)
		{
			bool unreachable = true;

			int yOffset;
			int xOffset;
			int mapYMax = maptiles.GetLength(1);
			int mapXMax = maptiles.GetLength(0);
			//Build a Pyramide with height of 3 under the CurrentMaptile to check if it is Unreachable
			for (xOffset = 1; xOffset <= 4 && unreachable && xOffset + tileX < mapXMax; xOffset++)
			{
			    for (yOffset = -xOffset; yOffset <= xOffset && unreachable && yOffset + tileY < mapXMax && yOffset + tileY >= 0; yOffset++)
			    {
					unreachable = !maptiles[tileX + xOffset, tileY + yOffset].Standable;
			    }
			}
			return unreachable;
		}
	}
}
