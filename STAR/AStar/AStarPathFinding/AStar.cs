using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Microsoft.Xna.Framework;
using System.Threading;

namespace AStarPathFinding
{
	public delegate void PathFoundEventHandler(Path path,double MillisecondsNeeded);

	public class AStar : IDisposable
	{
		static int threadNumber=0;
		Path path;
		//List<PathNode> openList;
		List<PathNode> newOpenList;
		LinkedList<PathNode> openLinkedList;
		PathNode aimglobal;
		Node start;
		Node aim;
		bool searching;
		public Thread myThread;
		public PathNode[,] searchMap;
		DateTime starttime;
		bool abort;
		bool stop;

		public event PathFoundEventHandler PathFound;

		public AStar()
		{
			openLinkedList = new LinkedList<PathNode>();
			path = new Path();
			//openList = new List<PathNode>();
			newOpenList = new List<PathNode>();
			myThread = new Thread(new ThreadStart(FindWay));
			myThread.Priority = ThreadPriority.Lowest;
			threadNumber++;
			myThread.Name = "Enemy Thread #" + threadNumber.ToString();
			PathFound += new PathFoundEventHandler(AStar_PathFound);
			searchMap = new PathNode[AStarMap.Mapnodes.GetLength(0), AStarMap.Mapnodes.GetLength(1)];
			for (int i = 0; i < AStarMap.Mapnodes.GetLength(0); i++)
			{
				for (int k = 0; k < AStarMap.Mapnodes.GetLength(1); k++)
				{
					Node node = AStarMap.Mapnodes[i,k];
					searchMap[i, k] = new PathNode(node.MapXPosition, node.MapYPosition, node.Walkable);
				}
			}
		}

		void AStar_PathFound(Path path,double ml)
		{
			//throw new NotImplementedException();
		}

		public void Abort()
		{
			abort = true;
			//myThread.Abort();
		}

		public void Stop()
		{
			stop = true;
			if (myThread.ThreadState == ThreadState.Suspended)
				myThread.Resume();
		   
		}

		public void FindWay(Node start, Node aim)
		{
			//Only run a new PathSearch if we don't running currently one.
			if (myThread.ThreadState != ThreadState.Running)
			{
				this.start = start;
				this.aim = aim;
				aimglobal = aim.ToPathNode();

				//openList.Clear();
				openLinkedList.Clear();
				//If thread is never Started, start it
				if (myThread.ThreadState == ThreadState.Unstarted)
					myThread.Start();
				//If thread is Suspended from an older search, resume it
				//with the new parameters specified above.
				if (myThread.ThreadState == ThreadState.Suspended)
					myThread.Resume();

				abort = false;
			}
			//FindWay();
		}
		public void FindWay(int startX, int startY, int aimX, int aimY)
		{
			startX = (int)MathHelper.Clamp(startX, 0, AStarMap.Mapnodes.GetLength(1)-1);
			startY = (int)MathHelper.Clamp(startY, 0, AStarMap.Mapnodes.GetLength(0)-1);
			aimX = (int)MathHelper.Clamp(aimX, 0, AStarMap.Mapnodes.GetLength(1)-1);
			aimY = (int)MathHelper.Clamp(aimY, 0, AStarMap.Mapnodes.GetLength(0)-1);
			FindWay(AStarMap.Mapnodes[startY, startX], AStarMap.Mapnodes[aimY, aimX]);
		}

		public void Resume()
		{
			if (myThread.ThreadState == ThreadState.Suspended)
				myThread.Resume();
		}

		private void MapToArray(ref PathNode[,] searchmap)
		{
			Node[,] map = AStarMap.Mapnodes;
			//searchmap = new PathNode[AStarMap.GetLength(0),AStarMap.GetLength(1)];
			int ymax = searchmap.GetLength(0);
			int xmax = searchmap.GetLength(1);
			for (int y = 0; y < ymax; y++)
			{
				for (int x = 0; x < xmax; x++)
				{
					searchmap[y, x].FromNode(map[y, x]);
				}
			}

			//return searchmap;
		}

		private void FindWay()
		{
			while (!stop)
			{
				if (aim.Walkable == Walkable.Walkable && start.Walkable == Walkable.Walkable)
				{
					
					PathNode aimNode;
					PathNode currentNode;
					searching = true;
					//searchMap = MapToArray();
					starttime = DateTime.Now;
					List<PathNode> similarNodes = new List<PathNode>();
					MapToArray(ref searchMap);
					
					//openList.Add(start.ToPathNode());
					openLinkedList.AddFirst(start.ToPathNode());
					PathNodeComparer comparer = new PathNodeComparer();

					aimNode = searchMap[aimglobal.MapYPosition, aimglobal.MapXPosition];

					while (openLinkedList.Count > 0 && searching && !abort)
					{
						if (openLinkedList.First.Value == aimNode)
							searching = false;
						if (searching)
						{
							currentNode = openLinkedList.First.Value;
							try
							{
								//    openList.Remove(openList[0]);
								//    openList.Sort(comparer);
								openLinkedList.RemoveFirst();
							}
							catch (Exception)
							{
								abort = true;
								//throw;
							}
							try
							{
								while (openLinkedList.First.Value.FCost == currentNode.FCost)
								{
									similarNodes.Add(openLinkedList.First.Value);
									openLinkedList.RemoveFirst();
								}
							}
							catch (Exception)
							{
								

							}
							if (currentNode.State != NodeState.Closed && searching)
								//ExploreNode(openList[0], searchMap);
								ExploreNode(currentNode, searchMap);
							for (int i = 0; i < similarNodes.Count; i++)
							{
								ExploreNode(similarNodes[i], searchMap);
							}
							similarNodes.Clear();
							//openList.AddRange(newOpenList);
							


							//if (openList.Contains(aimNode))
							//    if (aimNode.State == NodeState.Closed)
							//        searching = false;
							if (openLinkedList.Contains(aimNode))
								if (aimNode.State == NodeState.Closed)
									searching = false;
							//myThread.Suspend();
						}

					}
					if (!searching)
					{
						PathNode lastNode;
						LinkedList<PathNode> pathnodes = new LinkedList<PathNode>();
						pathnodes.AddLast(searchMap[aimglobal.MapYPosition, aimglobal.MapXPosition]);
						lastNode = searchMap[aimglobal.MapYPosition, aimglobal.MapXPosition];
						while (lastNode.RootNode != null)
						{
							pathnodes.AddBefore(pathnodes.First, lastNode.RootNode);
							lastNode = lastNode.RootNode;
						}
						path = new Path(pathnodes.ToList());
						PathFound(path, (DateTime.Now - starttime).TotalMilliseconds);
					}
					else
						PathFound(path, -2);
				}
				else
					PathFound(path, -1);
				if (!stop)
					myThread.Suspend();
			}
		}

		

		private bool ExploreNode(PathNode node, PathNode[,] searchmap)
		{

			//newOpenList.Clear();
			//Get Left New Node
			GetNewNode(node, -1, 0, searchmap);
			//Get Right New Node
			GetNewNode(node, 1, 0, searchmap);
			//Get Up New Node
			GetNewNode(node, 0, -1, searchmap);
			//Get Down New Node
			GetNewNode(node, 0, 1, searchmap);

			////Get Left New Node
			//GetNewNode(node, -1, -1, searchmap);
			////Get Right New Node
			//GetNewNode(node, 1, 1, searchmap);
			////Get Up New Node
			//GetNewNode(node, 1, -1, searchmap);
			////Get Down New Node
			//GetNewNode(node, -1, 1, searchmap);

			node.State = NodeState.Closed;
			return false;
			//else
			//{
			//    //Sort openList if ther Are more than One Value in it
			//    if (newOpenList.Count > 1)
			//        newOpenList.Sort(new PathNodeComparer());
			//    return true;
			//}



		}

		private void GetNewNode(PathNode rootNode, int xOffset, int yOffset,PathNode[,] searchMap)
		{ 
			//Check if xOffset is in Bounds
			if(rootNode.MapXPosition + xOffset >= 0 && rootNode.MapXPosition + xOffset < searchMap.GetLength(1))
				if (rootNode.MapYPosition + yOffset >= 0 && rootNode.MapYPosition + yOffset < searchMap.GetLength(0))
				{
					PathNode newNode = searchMap[rootNode.MapYPosition + yOffset, rootNode.MapXPosition + xOffset];
					//Check if its not the RootNode
					if (newNode != rootNode)
						//Exclude Closed Nodes
						if (newNode.State == NodeState.Unknown)
						{
							if (newNode.Walkable != Walkable.Blocked)
							{
								if (!(newNode.Walkable == Walkable.NotReachable && yOffset < 0) && !(newNode.Walkable == Walkable.Platform && yOffset > 0))
								{
									newNode.RootNode = rootNode;
									newNode.State = NodeState.Known;
									newNode.CalculateFCost(aimglobal.Rectangle.CenterToVector2());
									//openList.Add(newNode);
									try
									{
										LinkedListNode<PathNode> smaller = openLinkedList.First;
										while (newNode.FCost > smaller.Value.FCost)
										{
											smaller = smaller.Next;
										}
										openLinkedList.AddBefore(smaller, newNode);
									}
									catch (Exception)
									{

										openLinkedList.AddLast(newNode);
									}
								}
							}
						}
						else if (newNode.State == NodeState.Known)
						{
							float oldFCost = newNode.FCostNoAim;
							//Calculate new F Cost
							float newFCost = newNode.CalculateFCost(aimglobal.Rectangle.CenterToVector2(), rootNode);
							//If newFcost ist smaller than the old F Cost, we found a shorter way to this node
							if (newFCost < oldFCost)
							{
								openLinkedList.Remove(newNode);
								newNode.RootNode = rootNode;
								newNode.CalculateFCost(aimglobal.Rectangle.CenterToVector2());
								try
								{
									LinkedListNode<PathNode> smaller = openLinkedList.First;
									while (newNode.FCost > smaller.Value.FCost)
									{
										smaller = smaller.Next;
									}
									openLinkedList.AddBefore(smaller, newNode);
								}
								catch (Exception)
								{

									openLinkedList.AddLast(newNode);
								}
							}
							//openList.Add(newNode);
						}
				}
				
		}

		#region IDisposable Member

		public void Dispose()
		{
			Stop();
		}

		#endregion
	}
}
