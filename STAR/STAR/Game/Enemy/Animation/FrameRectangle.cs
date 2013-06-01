using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Runtime.InteropServices;

namespace Star.Game.Enemy
{
    public struct FrameRectangle
    {
        public Rectangle Rect;
        public float Rotation;
        public Color Color;
        public float DrawPosition;
        public Vector2 Origin;

        public void LoadFromData(string data)
        {
            try
            {
                string[] values = data.Split(',');
                if (values.Length >= 10)
                {
                    Rect = new Rectangle(
                        int.Parse(values[0]),
                        int.Parse(values[1]),
                        int.Parse(values[2]),
                        int.Parse(values[3]));
                    Rotation = float.Parse(values[4], System.Globalization.CultureInfo.CreateSpecificCulture("en-us"));
                    Color = new Color(
                        byte.Parse(values[5]),
                        byte.Parse(values[6]),
                        byte.Parse(values[7]),
                        byte.Parse(values[8]));
					DrawPosition = float.Parse(values[9], System.Globalization.CultureInfo.CreateSpecificCulture("en-us"));
					if (values.Length >= 12)
						Origin = new Vector2(
							float.Parse(values[10], System.Globalization.CultureInfo.CreateSpecificCulture("en-us")),
							float.Parse(values[11], System.Globalization.CultureInfo.CreateSpecificCulture("en-us")));
					else
						Origin = new Vector2();
                }
                else
                {
                    throw new Exception(
                        "Der angegebene string enthält " + 
                        values.Length + 
                        " Werte, er muss aber 10 enthalten.");
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException("Der übergebene String konnte nicht verarbeitet werden:\n" + e.Message);
            }
        }

        public string GetDataString()
        {
			string data =
				Rect.X + "," +
				Rect.Y + "," +
				Rect.Width + "," +
				Rect.Height + "," +
				Rotation.ToString(System.Globalization.CultureInfo.CreateSpecificCulture("en-us")) + "," +
				Color.R + "," +
				Color.G + "," +
				Color.B + "," +
				Color.A + "," +
				DrawPosition.ToString(System.Globalization.CultureInfo.CreateSpecificCulture("en-us")) + "," +
				Origin.X.ToString(System.Globalization.CultureInfo.CreateSpecificCulture("en-us")) + "," +
				Origin.Y.ToString(System.Globalization.CultureInfo.CreateSpecificCulture("en-us"));
            return data;

        }

        public void Load(Rectangle rect, float rotation, float layer, Color color, Vector2 origin)
        {
            Rect = rect;
            Rotation = rotation;
            Color = color;
            DrawPosition = layer;
			Origin = origin;
        }

        public FrameRectangle Scale(float scale)
        {
            Rect.X = (int)(Rect.X * scale);
            Rect.Y = (int)(Rect.Y * scale);
            Rect.Width = (int)(Rect.Width * scale);
            Rect.Height = (int)(Rect.Height * scale);
            //Origin /= scale;
            return this;
        }

        public FrameRectangle Copy()
        {
            return new FrameRectangle() { Color = new Color(Color.R, Color.G, Color.B, Color.A), DrawPosition = DrawPosition, Rect = new Rectangle(Rect.X, Rect.Y, Rect.Width, Rect.Height), Rotation = Rotation };
        }

        public static FrameRectangle Default
        {
            get { return new FrameRectangle() { Color = Color.White, DrawPosition = 0, Rect = new Rectangle(0,0,100,100), Rotation = 0 }; }
        }
    }
}
