using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Star.Game
{
    public static class RectangleFunctions
    {
        public static Vector2 IntersectionDepth(Rectangle rect1, Rectangle rect2)
        {
            Vector2 depth;
            int x;
            int y;
            if (rect1.Intersects(rect2))
            {
                if (rect1.X < rect2.X)
                {
                    x = rect1.Right - rect2.Left;
                }
                else
                {
                    x = rect2.Right - rect1.Left;
                }

                if (rect1.Y < rect2.Y)
                {
                    y = rect1.Bottom - rect2.Top;
                }
                else
                {
                    y = rect2.Bottom - rect1.Top;
                }

                depth = new Vector2(x, y);
            }
            else
            {
                depth = Vector2.Zero;
            }

            return depth;
        }

        public static Rectangle TranslateRectangle(Rectangle rect, Vector2 translation)
        {
            Rectangle newRect;

            newRect = new Rectangle(rect.X + (int)translation.X, rect.Y + (int)translation.Y, rect.Width, rect.Height);

            return newRect;
        }

        public static Rectangle ReCenterSpecialRects(Star.Game.Enemy.SpecialRect rect,Point newCenter)
        {
            Rectangle temp = rect.Rectangle;
            temp.Location = new Point(
                newCenter.X + rect.OriginalRectangle.Location.X,
                newCenter.Y + rect.OriginalRectangle.Location.Y);

            return temp;
        }
    }
}
