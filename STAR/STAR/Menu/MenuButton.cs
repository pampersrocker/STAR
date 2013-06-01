using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;


namespace Star.Menu
{
    /// <summary>
    /// Stellt ein Button im Menü dar
    /// </summary>
    public class MenuButton
    {
        public const int BUTTON_HEIGHT = 30;
        public const int BUTTON_WIDTH = 120;
		float rotation;
		float scale;
        string text;
        Vector2 pos;
        Rectangle rect;
        ButtonIndicator indicator = ButtonIndicator.Button_Normal;

        public Color GetColor
        {
            get
            {
                switch (indicator)
                {
                    case ButtonIndicator.Button_Normal:
                        return Color.White;
                    case ButtonIndicator.Button_Hover:
                        return Color.Red;
                    case ButtonIndicator.Button_Locked:
                        return Color.Gray;
                    case ButtonIndicator.Button_Click:
                        return Color.Yellow;
                }
                return Color.White;
            }
        }

		public float Rotation
		{
			get { return rotation; }
			set { rotation = value; }
		}

		public float Scale
		{
			get 
			{
				if (indicator == ButtonIndicator.Button_Hover)
				{
					return scale * 1.2F;
				}
				return scale; 
			}
			set { scale = value; }
		}

        public string GetText
        {
            get { return text; }
			set { text = value; }
        }
        public Vector2 Position
        {
            get { return pos; }
        }
        public Rectangle Rectangle
        {
            get { return rect; }
        }

        public ButtonIndicator TextureIndicator
        {
            get { return indicator; }
            set { indicator = value; }
        }
        /// <summary>
        /// Initialisiert Button
        /// </summary>
        /// <param name="Button_text">Text des Buttons</param>
        /// <param name="newrect">Rectangle des Button</param>
        public MenuButton(Vector2 new_pos, string Button_text,Random rand)
        {
			
			rotation = (float)(rand.NextDouble() - 0.5) * 0.3f;
			scale = 1f;
            if (Button_text != null && Button_text != "")
            {
                text = Button_text;
            }
            else
            {
                text = "Button_Text";
            }
            pos = new_pos;
            rect = new Rectangle((int)pos.X, (int)pos.Y, BUTTON_WIDTH, BUTTON_HEIGHT);
        }
        public MenuButton(Vector2 new_pos)
        {
            text = "Button_Text";
            pos = new_pos;
            rect = new Rectangle((int)pos.X, (int)pos.Y, BUTTON_WIDTH, BUTTON_HEIGHT);
        }

    }
}
