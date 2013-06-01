using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Star.Game.Level
{
    public enum TileCollision
    {
        Passable,
        Impassable,
        Platform,
        Event
    }

    public enum TileType
    {
        Empty,
        Start,
        Exit,
        Wall,
        Platform,
        Spike,
        Error
    }

    public enum GrassType
    {
        Left,
        Right,
        Top,
        Bottom,
        Empty
    }

    public struct Grass
    {
        public GrassType type;
        public Rectangle rect;
    }

    public class Tile
    {
        public const int TILE_SIZE = 32;

        //String type;
        TileCollision passable = TileCollision.Passable;
        TileType tile_type = TileType.Empty;
        Vector2 pos;
        Rectangle rect;
        int tile_x, tile_y;
        Grass[] grass = new Grass[4];
		bool standable;

		public bool Standable
		{
			get { return standable; }
			set { standable = value; }
		}

        public TileType TileType
        {
            get
            {
                return tile_type;
            }
        }

        public Rectangle get_rect
        {
            get
            {
                return rect;
            }
        }

        public Point TileCoord
        {
            get { return new Point(tile_x, tile_y); }
        }

        public TileCollision TileColission_Type
        {
            get { return passable; }
        }

        public Tile(int x, int y)
        {
            tile_x = x;
            tile_y = y;
            pos = new Vector2(x * TILE_SIZE, y * TILE_SIZE);
            rect = new Rectangle((int)pos.X, (int)pos.Y, TILE_SIZE, TILE_SIZE);
        }

        public void LoadGrass()
        {
            if (tile_type == TileType.Wall)
            {
                grass[(int)GrassType.Left].rect = new Rectangle(rect.Left, rect.Top, 10, TILE_SIZE);
                grass[(int)GrassType.Left].type = GrassType.Left;
                grass[(int)GrassType.Right].rect = new Rectangle(rect.Right - 10, rect.Top, 10, TILE_SIZE);
                grass[(int)GrassType.Right].type = GrassType.Right;
                grass[(int)GrassType.Top].rect = new Rectangle(rect.Left - 5, rect.Top - 5, TILE_SIZE + 10, 20);
                grass[(int)GrassType.Top].type = GrassType.Top;
                grass[(int)GrassType.Bottom].rect = new Rectangle(rect.Left, rect.Bottom - 10, TILE_SIZE, 10);
                grass[(int)GrassType.Bottom].type = GrassType.Bottom;
            }
        }

        public void loadGrass(Tile[,] tiles)
        {
            int max_height = tiles.GetLength(0);
            int max_width = tiles.GetLength(1);
            if (tile_x - 1 >= 0)
            {
                if (tiles[tile_y, tile_x - 1].TileType != TileType.Wall)
                {
                    grass[(int)GrassType.Left].rect = new Rectangle(rect.Left, rect.Top, 10, TILE_SIZE);
                    grass[(int)GrassType.Left].type = GrassType.Left;
                }
                else
                {
                    grass[(int)GrassType.Left].rect = Rectangle.Empty;
                    grass[(int)GrassType.Left].type = GrassType.Empty;
                }
            }
            else
            {
                grass[(int)GrassType.Left].rect = new Rectangle(rect.Left, rect.Top, 10, TILE_SIZE);
                grass[(int)GrassType.Left].type = GrassType.Left;
            }
            if (tile_x + 1 < max_width)
            {
                if (tiles[tile_y, tile_x + 1].TileType != TileType.Wall)
                {
                    grass[(int)GrassType.Right].rect = new Rectangle(rect.Right - 10, rect.Top, 10, TILE_SIZE);
                    grass[(int)GrassType.Right].type = GrassType.Right;
                }
                else
                {
                    grass[(int)GrassType.Right].rect = Rectangle.Empty;
                    grass[(int)GrassType.Right].type = GrassType.Empty;
                }
            }
            else
            {
                grass[(int)GrassType.Right].rect = new Rectangle(rect.Right - 10, rect.Top, 10, TILE_SIZE);
                grass[(int)GrassType.Right].type = GrassType.Right;
            }
            if (tile_y - 1 >= 0)
            {
                if (tiles[tile_y - 1, tile_x].TileType != TileType.Wall)
                {
                    grass[(int)GrassType.Top].rect = new Rectangle(rect.Left - 5, rect.Top - 5, TILE_SIZE + 10, 20);
                    grass[(int)GrassType.Top].type = GrassType.Top;
                }
                else
                {
                    grass[(int)GrassType.Top].rect = Rectangle.Empty;
                    grass[(int)GrassType.Top].type = GrassType.Empty;
                }
            }
            else
            {
                grass[(int)GrassType.Top].rect = new Rectangle(rect.Left - 5, rect.Top - 5, TILE_SIZE + 10, 20);
                grass[(int)GrassType.Top].type = GrassType.Top;
            }
            if (tile_y + 1 < max_height)
            {
                if (tiles[tile_y + 1, tile_x].TileType != TileType.Wall)
                {
                    grass[(int)GrassType.Bottom].rect = new Rectangle(rect.Left, rect.Bottom - 10, TILE_SIZE, 10);
                    grass[(int)GrassType.Bottom].type = GrassType.Bottom;
                }
                else
                {
                    grass[(int)GrassType.Bottom].rect = Rectangle.Empty;
                    grass[(int)GrassType.Bottom].type = GrassType.Empty;
                }
            }
            else
            {
                grass[(int)GrassType.Bottom].rect = new Rectangle(rect.Left, rect.Bottom - 10, TILE_SIZE, 10);
                grass[(int)GrassType.Bottom].type = GrassType.Bottom;
            }
        }

        public Grass[] GetGrass
        {
            get
            {
                return grass;
            }
        }

        public void Clear()
        {
            passable = TileCollision.Passable;
            tile_type = TileType.Empty;
            pos = Vector2.Zero;
            rect = Rectangle.Empty;
        }

        public void load_tile(int indicator,Tile tileAbove)
        {

            #region "PerChar"
            /*
            switch (indicator)
            {
                
                case 'S':
                    //type = TileType.Wall;
                    passable = TileColission.Passable;
                    tile_type = TileType.Start;
                    break;
                case 'X':
                    //type = "Exit";
                    passable = TileColission.Passable;
                    tile_type = TileType.Exit;
                    break;
                case 'W':
                    //type = "Wall";
                    passable = TileColission.Impassable;
                    tile_type = TileType.Wall;
                    break;
                default :
                    //type = "Empty";
                    passable = TileColission.Passable;
                    tile_type = TileType.Empty;
                    break;
            }
            */
            #endregion

            //tile_type = TileType.Empty;
            tile_type = TileType.Empty;
            passable = TileCollision.Passable;
            //indicator = indicator.Trim();

            foreach (TileType type in Enum.GetValues(typeof(TileType)))
            {
                if (((int)type) == indicator)
                {
                    tile_type = type;
                }
            }

            switch (tile_type)
            {
                case (TileType.Wall):
                    passable = TileCollision.Impassable;
                    break;
                case (TileType.Platform):
                    passable = TileCollision.Platform;
                    break;
                case TileType.Spike:
                    passable = TileCollision.Event;
                    break;
            }

			switch (tile_type)
			{ 
				case TileType.Platform:
				case TileType.Wall:
					standable = true;
					break;
				default:
					standable = false;
					break;
			}
			if (tileAbove != null)
			{
				switch (tileAbove.TileType)
				{
					case TileType.Wall:
						standable = false;
						break;
					default:
						break;
				}
			}

        }
    }
}
