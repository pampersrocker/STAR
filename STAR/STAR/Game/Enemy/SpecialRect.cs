using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Star.Game.Enemy
{
    public class SpecialRect
    {
        Rectangle rect;
        Rectangle orgRect;

        public SpecialRect(Rectangle originalRect)
        {
            orgRect = originalRect;
            rect = originalRect;
        }

        public Rectangle Rectangle
        {
            get { return rect; }
            set { rect = value; }
        }

        public Rectangle OriginalRectangle
        {
            get { return orgRect; }
        }

        public string DataString(string seperator)
        {
            string data = "";

            data += orgRect.X + seperator + orgRect.Y + seperator + orgRect.Width + seperator + orgRect.Height;

            return data;
        }
    }
}
