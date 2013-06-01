using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Star.Game.Level
{
    public enum QTI
    { 
        LeftTop,
        RightTop,
        LeftBottom,
        RightBottom
    }

    public class Quadtree
    {
        readonly QuadTreeNode rootNode;
        List<QuadTreeNode> iterateNodes;
        readonly int MaxLayer;
        Rectangle levelBoundingBox;
        List<Rectangle> current_rects;
		List<Tile>[] enemyCollisionTiles;
		List<QuadTreeNode>[] enemyiterateNodes;

        public List<Rectangle> getCurrentRectangle
        {
            get { return current_rects; }
        }

        public Quadtree(Tile[,] leveltiles, int layers)
        {
            levelBoundingBox = new Rectangle(
                0,
                0,
                leveltiles.GetLength(1) * Tile.TILE_SIZE,
                leveltiles.GetLength(0) * Tile.TILE_SIZE);
            rootNode = new QuadTreeNode(levelBoundingBox, layers, leveltiles);
            current_rects = new List<Rectangle>();
            MaxLayer = layers;

			enemyCollisionTiles = new List<Tile>[Environment.ProcessorCount];
			enemyiterateNodes = new List<QuadTreeNode>[Environment.ProcessorCount];
        }

        public List<Tile> GetCollision(Rectangle objBoundingBox)
        {
			//lock (this)
			{
				List<Tile> collisionTiles;
				collisionTiles = new List<Tile>();
				bool lastlayer = false;
				iterateNodes = new List<QuadTreeNode>();
				iterateNodes.Add(rootNode);
				current_rects = new List<Rectangle>();
				if (objBoundingBox.Intersects(levelBoundingBox))
				{
					while (lastlayer == false)
					{
						iterateNodes = GetIntersectingChildNodes(iterateNodes, objBoundingBox);
						foreach (QuadTreeNode node in iterateNodes)
						{
							if (node.IsLastLayer)
							{
								collisionTiles = collisionTiles.Union(node.LevelTiles).ToList();
								current_rects.Add(node.GetRect);
								lastlayer = true;
							}
						}
					}
				}
				return collisionTiles;
			}
        }

		public List<Tile> GetEnemyCollision(Rectangle objBoundingBox, int numThread)
		{
			
				
				enemyCollisionTiles[numThread] = new List<Tile>();
				bool lastlayer = false;
				enemyiterateNodes[numThread] = new List<QuadTreeNode>();
				enemyiterateNodes[numThread].Add(rootNode);
				//enemycurrent_rects = new List<Rectangle>();
				if (objBoundingBox.Intersects(levelBoundingBox))
				{
					while (lastlayer == false)
					{
						//lock(this)
						enemyiterateNodes[numThread] = GetIntersectingChildNodes(enemyiterateNodes[numThread], objBoundingBox);
						foreach (QuadTreeNode node in enemyiterateNodes[numThread])
						{
							if (node.IsLastLayer)
							{
								enemyCollisionTiles[numThread] = enemyCollisionTiles[numThread].Union(node.LevelTiles).ToList();
								//current_rects.Add(node.GetRect);
								lastlayer = true;
							}
						}
					}
				}
				return enemyCollisionTiles[numThread];
			
		}

        private List<QuadTreeNode> GetIntersectingChildNodes(List<QuadTreeNode>parentNodes,Rectangle collisionObj)
        {
            List<QuadTreeNode> intersectingNodes = new List<QuadTreeNode>();
            foreach (QuadTreeNode node in parentNodes)
            {
                foreach (QuadTreeNode childNode in node.ChildNodes)
                { 
                    if(collisionObj.Intersects(childNode.GetRect))
                    {
                        intersectingNodes.Add(childNode);
                    }
                }
            }
            return intersectingNodes;
        }

    }
}
