using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Star.Components
{


    public struct DrawableText
    {
        public SpriteFont font;
        public Vector2 pos;
		public Color color;
        public string Text;

        public void CenterText(Vector2 center)
        {
            Vector2 size = font.MeasureString(Text);
            pos = center - size/2;
        }

    }
}
