using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Star.Input;
using Star.Game;

namespace Star.Game.Level
{
    public class LayerObject
    {
        Texture2D tex;
        Rectangle rect;
        Vector2 pos;
        float specialvalue;
        float specialvalue2;
        float specialvalue3;
        Rectangle specialrect;

		ExtendedRectangle extendedRectangle;

		public ExtendedRectangle ExtendedRectangle
		{
			get { return extendedRectangle; }
			set { extendedRectangle = value; }
		}

        public LayerObject(Texture2D new_tex)
        {
            tex = new_tex;
            rect = new Rectangle(0, 0, tex.Width, tex.Height);
            pos = Vector2.Zero;
        }
        public LayerObject(Texture2D new_tex, Vector2 new_pos)
        {
            tex = new_tex;
            pos = new_pos;
            rect = new Rectangle((int)pos.X, (int)pos.Y, tex.Width, tex.Height);
        }
        public LayerObject(Texture2D new_tex, Rectangle new_rect)
        {
            tex = new_tex;
            rect = new_rect;
            pos = new Vector2(rect.X, rect.Y);
        }

		public LayerObject(Texture2D tex, ExtendedRectangle extRect)
			: this(tex, extRect.OrgRectangle)
		{
			extendedRectangle = extRect;
		}

        public Rectangle SpecialRect
        {
            get { return specialrect; }
            set { specialrect = value; }
        }

        public float SpecialValue
        {
            get { return specialvalue; }
            set { specialvalue = value; }
        }

        public float SpecialValue2
        {
            get { return specialvalue2; }
            set { specialvalue2 = value; }
        }

        public float SpecialValue3
        {
            get { return specialvalue3; }
            set { specialvalue3 = value; }
        }

        public Texture2D Texture
        {
            get { return tex; }
            set { tex = value; }
        }

        public Rectangle Rectangle
        {
            get { return rect; }
            set
            {
                rect = value;
                //pos = new Vector2(rect.X, rect.Y);
                //specialrect.X = rect.X;
            }
        }

        public Vector2 Position
        {
            get { return pos; }
            set
            {
                pos = value;
                rect.X = (int)pos.X;
                rect.Y = (int)pos.Y;
                specialrect.X = (int)pos.X;
                specialrect.Y = (int)pos.Y;
            }
        }

        public void Dispose()
        {
            tex.Dispose();
        }
    }
}
