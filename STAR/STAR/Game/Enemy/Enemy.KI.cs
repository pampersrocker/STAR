using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AStarPathFinding;
using Microsoft.Xna.Framework;
using Star.Game.Level;
using Microsoft.Xna.Framework.Graphics;
using Star.Game.Debug;

namespace Star.Game.Enemy
{
	public partial class Enemy
	{
		readonly float MaxTimePathSearch = 1;
		readonly float MaxTimeFromLastSearch = .2f;
		AStar astar;
		Path path;
		PlayerTracking tracking;
		bool usesKI;
		bool pathfound;
		bool pathSearching;
		bool canJump;
		float timePathSearching;
		Texture2D nodeTex;
		Node start;
		Node end;
		Vector2 jumpSpeed;
		bool jumping;
		bool standXStill;
		float timeFromLastSearch;
		float jumpheight = 1;
		SpriteFont font;
		int pathPosition = 0;

		public void Jump(float elapsed,bool small,float height)
		{
			if (!jumping && canJump)
			{
				jumpSpeed = new Vector2(0, (small ? -15 : (-30 * ((float)height / 4f))));
				jumpSpeed.Y *=gravity.Y;
				jumping = true;
			}
			else
			{
				if (jumpSpeed.Y < 0)
				{
					jumpSpeed.Y += 100 * elapsed;
				}
				else
					jumpSpeed.Y = 0;
			}
		}

		private void InitializeKI()
		{
			astar = new AStar();
			astar.PathFound += new PathFoundEventHandler(astar_PathFound);
			nodeTex = Content.Load<Texture2D>("Stuff/Node");
			font = Content.Load<SpriteFont>("Stuff/Arial");
			if (enemyvariables[EnemyVariables.PlayerTracking].Trim() == PlayerTracking.Tracking.ToString())
			{
				tracking = PlayerTracking.Tracking;
				usesKI = true;
			}
			else
				tracking = PlayerTracking.NotTracking;
			//timeFromLastSearch = 0;


		}

		void astar_PathFound(Path path, double MillisecondsNeeded)
		{
			if (MillisecondsNeeded >= 0)
			{
				pathSearching = false;
				this.path = path;
				pathfound = true;
				pathPosition = 0;
				//DebugManager.AddItem("Found Path", myNumber.ToString() + " Enemy, Type: " + eType, new System.Diagnostics.StackTrace());
			}
			else
			{

				DebugManager.AddItem("Invalid Path, Error:" +MillisecondsNeeded, myNumber.ToString() + " Enemy, Type: " + eType, new System.Diagnostics.StackTrace(), System.Drawing.Color.Yellow);
				pathSearching = false;
			}
			timeFromLastSearch = 0;
		}

		private void DoEnemyKI(GameTime gameTime, Quadtree quadtree, Vector2 playerPos,Tile[,] map)
		{
			switch (tracking)
			{ 
				case PlayerTracking.NotTracking:
					break;
				case PlayerTracking.Tracking:
					TrackPlayer((float)gameTime.ElapsedGameTime.TotalSeconds,playerPos);
					CliffJump((float)gameTime.ElapsedGameTime.TotalSeconds, map);
					break;
			}
		}

		private void CliffJump(float elapsed,Tile[,] map)
		{
			int xpos = (int)pos.X / Tile.TILE_SIZE;
			int ypos = (int)pos.Y / Tile.TILE_SIZE;
			if (xpos + 1 < map.GetLength(1) && xpos - 1 >= 0 && ypos + 1 < map.GetLength(0))
			{
				if (map[ypos + 1, xpos].Standable)
					if (rundirection == StandardDirection.Left)
					{
						if (map[ypos + 1, xpos - 1].TileColission_Type == TileCollision.Passable)
							Jump(elapsed, false,4);
					}
					else if (map[ypos + 1, xpos + 1].TileColission_Type == TileCollision.Passable && rundirection == StandardDirection.Right)
						Jump(elapsed, false,4);
						

			}
		}

		private void TrackPlayer(float elapsedTime,Vector2 playerPos)
		{
			if (pathfound)
			{
				standXStill = false;
				TrackPath(elapsedTime);
				timeFromLastSearch += elapsedTime;
				if (timeFromLastSearch > MaxTimeFromLastSearch*2)
				{
					timeFromLastSearch = 0;
					StartNewPathSearch(playerPos);
				}
			}
			else
			{
				//standXStill = true;
				timeFromLastSearch += elapsedTime;
				if (!pathSearching && timeFromLastSearch > MaxTimeFromLastSearch)
				{
					timeFromLastSearch = 0;

					StartNewPathSearch(playerPos);
				}
				else
				{
					timePathSearching += elapsedTime;
					if (timePathSearching > MaxTimePathSearch)
					{
						pathSearching = false;
						astar.Abort();
					}
				}
			}
				

		}

		private void StartNewPathSearch(Vector2 playerPos)
		{
			int startX, starty, aimX, aimY;
			startX = (int)Math.Round((pos.X) / (float)Tile.TILE_SIZE);
			starty = (int)Math.Round(pos.Y / (float)Tile.TILE_SIZE);
			aimX = (int)playerPos.X / Tile.TILE_SIZE;
			aimY = (int)playerPos.Y / Tile.TILE_SIZE;
			start = new Node(startX, starty, Tile.TILE_SIZE, Walkable.Walkable);
			end = new Node(aimX, aimY, Tile.TILE_SIZE, Walkable.Walkable);
			astar.Abort();
			astar.FindWay(startX, starty, aimX, aimY);
			pathSearching = true;
		}

		private void DrawKI(GameTime gameTime, SpriteBatch spriteBatch,Matrix matrix)
		{
			//3_1
			//spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState, matrix);
			spriteBatch.Begin(
				SpriteSortMode.Immediate,
				BlendState.AlphaBlend,
				null, null, null, null,
				matrix);
			if (pathfound)
			{
				Color color = Color.Green;
				foreach (PathNode node in path)
					spriteBatch.Draw(nodeTex, node.Rectangle, color);
			}
			//for (int i = 0; i < AStarMap.Mapnodes.GetLength(0); i++)
			//{
			//    for (int k = 0; k < AStarMap.Mapnodes.GetLength(1); k++)
			//    {
			//        Node node = AStarMap.Mapnodes[i, k];
			//        Color color;
			//        switch (node.Walkable)
			//        {
			//            case Walkable.Walkable:
			//                color = Color.Transparent;
			//                break;
			//            case Walkable.Blocked:
			//                color = Color.Red;
			//                break;
			//            case Walkable.Platform:
			//                color = Color.Green;
			//                break;
			//            case Walkable.NotReachable:
			//                color = Color.Blue;
			//                break;
			//            default:
			//                color = Color.White;
			//                break;
							
			//        }
			//        spriteBatch.Draw(nodeTex, node.Rectangle, color);
			//    }
			//}
			//if (start != null)
			spriteBatch.DrawString(font, jumpheight.ToString("0") + "\nX:" + speed.X.ToSpacedString("0") + "Y:"+ speed.Y.ToSpacedString("0") , pos, Color.White);
				spriteBatch.Draw(nodeTex, start.Rectangle, Color.Green);
			//if (end != null)
				spriteBatch.Draw(nodeTex, end.Rectangle, Color.Black);
			spriteBatch.End();
			
		}

		private void TrackPath(float elapsedTime)
		{
			Rectangle collisionrect = new Rectangle(
				orgcollisionrect.X + (int)pos.X,
				orgcollisionrect.Y + (int)pos.Y,
				orgcollisionrect.Width,
				orgcollisionrect.Height);
			//collisionrect.Y = collisionrect.Bottom - 1;
			//collisionrect.Height = 1;
			//collisionrect.X = collisionrect.Center.X;
			//collisionrect.Width = 1;

			pathPosition = 0;
			bool pathPositionFound = false;
			PathNode currentNode=null, nextNode=null;
			//Go Through Entire Path to Find the current Position
			for (; pathPosition < path.Length; pathPosition++)
			{
				//if (currentNode == null)
				//    pathPositionFound = collisionrect.Intersects(path[pathPosition].Rectangle);
				if (collisionrect.Intersects(path[pathPosition].Rectangle))
				{
					pathPositionFound = true;
					currentNode = path[pathPosition];
					if (pathPosition + 1 < path.Length)
						nextNode = path[pathPosition + 1];
					else
						nextNode = currentNode;
				}
			}

			//Check if we walked the whole path
			if (collisionrect.Intersects(path[path.Length - 1].Rectangle))
			{
				astar.Abort();
				//Implicit request a new Path by saying we don't got a path
				pathfound = false;
			}
			//Or if we found the pathPosition go on with walking on it
			else if (pathPositionFound)
			{
				standXStill = false;
				if (nextNode.MapXPosition < currentNode.MapXPosition)
				{
					rundirection = StandardDirection.Left;
					standXStill = false;
				}
				else if (nextNode.MapXPosition > currentNode.MapXPosition)
				{
					rundirection = StandardDirection.Right;
					standXStill = false;
				}
				if (nextNode.MapYPosition < currentNode.MapYPosition)
				{
					//standXStill = true;
					PathNode curNode = currentNode;
					jumpheight = 2;
					int sideways = 0;
					bool foundDirection = false;
					try
					{
						
						while (sideways < 4 && jumpheight < 5)
						{
							if (!foundDirection)
							{

								if (curNode.MapXPosition < curNode.RootNode.MapXPosition)
								{
									foundDirection = true;
									rundirection = StandardDirection.Right;
								}
								else if (curNode.MapXPosition > curNode.RootNode.MapXPosition)
								{
									rundirection = StandardDirection.Left;
									foundDirection = true;
								}

							} 
							if (curNode.MapXPosition < curNode.RootNode.MapXPosition || curNode.MapXPosition > curNode.RootNode.MapXPosition)
							{
								sideways++;
							}
							else if (curNode.MapYPosition < curNode.RootNode.MapYPosition)
							{
								jumpheight++;
								jumpheight++;
							}
							
							curNode = curNode.RootNode;
						}
					}
					catch (Exception)
					{

					}
					Jump(elapsedTime,false,jumpheight);
				}
			}
			//If we don't reached the end of the path, 
			//and we're not on the Path anymore
			//we've lost the Path and have to request a new One 
			else
				pathfound = false;
			

			
		}
	}
}
