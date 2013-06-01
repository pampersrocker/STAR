using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Star.Game.Level
{
    class QuadTreeNode
    {
        Rectangle ownRect;
        QuadTreeNode[] childTreeNodes;
        List<Tile> tiles;
        bool isLastLayer = false;

        #region Properties

        public Rectangle GetRect
        {
            get { return ownRect; }
        }

        public QuadTreeNode[] ChildNodes
        {
            get { return childTreeNodes; }
        }

        public List<Tile> LevelTiles
        {
            get { return tiles; }
        }

        public bool IsLastLayer
        {
            get { return isLastLayer; }
        }

        #endregion

        /// <summary>
        /// Generates the RootNode of the Quadtree
        /// </summary>
        /// <param name="parentRect">The Rectangle of the RootNode 
        /// (normally the Bounding Box of the Level)</param>
        /// <param name="maxLayer">The Maximum count of Layers in the Quadtree</param>
        /// <param name="leveltiles">The Tiles from the Level which should be included into the Quadtree</param>
        public QuadTreeNode(Rectangle parentRect, int maxLayer, Tile[,] leveltiles)
        {
            ownRect = parentRect;
            childTreeNodes = GenerateChildNodes(ownRect, 0, maxLayer, leveltiles);
        }

        /// <summary>
        /// Generates a normal node of the Quadtree.
        /// This one contains no Tiles, but conatins 4 ChildNodes
        /// </summary>
        /// <param name="parentRect">The Rectangle of the Parent Node.
        /// The Node will generate his own rectangle depending on the Indicator</param>
        /// <param name="indicator">The Indicator controlls which Position this Node will have in the Parent Node</param>
        /// <param name="layernumber">The current Layernumber</param>
        /// <param name="maxLayer">The Maximum count of Layers in the Quadtree</param>
        /// <param name="leveltiles">The Tiles from the Level which should be included into the Quadtree</param>
        public QuadTreeNode(Rectangle parentRect,QTI indicator,int layernumber,int maxLayer,Tile[,] leveltiles)
        {
            ownRect = GenerateRect(parentRect, indicator);
            childTreeNodes = GenerateChildNodes(ownRect, layernumber, maxLayer, leveltiles);
        }

        /// <summary>
        /// This Generates a last node in the Quadtree.
        /// It contains the Leveltiles which are important for the collision.
        /// </summary>
        /// <param name="parentRect">The Rectangle of the Parent Node</param>
        /// <param name="indicator"></param>
        /// <param name="levelTiles">The Tiles from the Level which should be included into the Quadtree</param>
        public QuadTreeNode(Rectangle parentRect, Tile[,] levelTiles)
        {
            Rectangle intersectsRect;
            isLastLayer = true;
            ownRect = parentRect;
            intersectsRect = new Rectangle(
                ownRect.X ,
                ownRect.Y ,
                ownRect.Width,
                ownRect.Height);
            ownRect = intersectsRect;
            tiles = new List<Tile>();
            foreach (Tile tile in levelTiles)
            {
                if (intersectsRect.Intersects(tile.get_rect) &&
                    (tile.TileColission_Type != TileCollision.Passable))
                {
                    tiles.Add(tile);
                }
            }

        }

        private QuadTreeNode[] GenerateChildNodes(Rectangle parentRect,int layernumber,int maxLayer,Tile[,] leveltiles)
        {
            QuadTreeNode[] childs;

            layernumber++;
            if (layernumber + 1 >= maxLayer)
            {
                childs = new QuadTreeNode[1];
                childs[0] = new QuadTreeNode(parentRect, leveltiles);
            }
            else
            {
                childs = new QuadTreeNode[4];
                childs[0] = new QuadTreeNode(parentRect, QTI.LeftTop, layernumber, maxLayer, leveltiles);
                childs[1] = new QuadTreeNode(parentRect, QTI.RightTop, layernumber, maxLayer, leveltiles);
                childs[2] = new QuadTreeNode(parentRect, QTI.LeftBottom, layernumber, maxLayer, leveltiles);
                childs[3] = new QuadTreeNode(parentRect, QTI.RightBottom, layernumber, maxLayer, leveltiles);
            }

            return childs;
        }

        private Rectangle GenerateRect(Rectangle sourceRectangle, QTI indicator)
        {
            Point temppos;
            Point tempsize;
            tempsize = new Point(
                sourceRectangle.Width / 2,
                sourceRectangle.Height / 2);
            switch (indicator)
            {
                case QTI.LeftBottom:
                    temppos = new Point(
                        sourceRectangle.X,
                        sourceRectangle.Y + sourceRectangle.Height / 2);
                    break;
                case QTI.LeftTop:
                    temppos = new Point(
                        sourceRectangle.X,
                        sourceRectangle.Y);
                    break;
                case QTI.RightBottom:
                    temppos = new Point(
                        sourceRectangle.X + sourceRectangle.Width / 2,
                        sourceRectangle.Y + sourceRectangle.Height / 2);
                    break;
                case QTI.RightTop:
                    temppos = new Point(
                        sourceRectangle.X + sourceRectangle.Width / 2,
                        sourceRectangle.Y);
                    break;
                default:
                    temppos = new Point(
                        sourceRectangle.X,
                        sourceRectangle.Y);
                    break;
            }

            return new Rectangle(temppos.X, temppos.Y, tempsize.X, tempsize.Y);
        }
    }
}
