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
    /// Stellt eine Liste im Menü dar
    /// </summary>
    public class MenuList
    {
        MenuButton[] buttons;
        string Title;
        int current_position = 0;

        public MenuList(string new_title, string[] button_names)
        {
			Random rand = new Random((int)DateTime.Now.Ticks);
            Title = new_title;
            buttons = new MenuButton[button_names.Length];
            float x_pos = 100;
            float y_pos = 150;
            for (int i = 0; i < button_names.Length; i++)
            {
                buttons[i] = new MenuButton(new Vector2(x_pos, y_pos), button_names[i],rand);
                y_pos += (MenuButton.BUTTON_HEIGHT * 1.5f);
            }
        }
        public MenuList(string new_title, string[] button_names,float x_pos,float y_pos,float distance_between)
        {
			Random rand = new Random((int)DateTime.Now.Ticks);
            Title = new_title;
            buttons = new MenuButton[button_names.Length];
            for (int i = 0; i < button_names.Length; i++)
            {
                buttons[i] = new MenuButton(new Vector2(x_pos, y_pos), button_names[i],rand);
                y_pos += (MenuButton.BUTTON_HEIGHT + distance_between);
            }
        }

        public string GetTitle
        {
            get { return Title; }
        }

        public MenuButton[] Buttons
        {
            get { return buttons; }
        }
        public int GetCurrentPos
        {
            get { return current_position; }
        }

		public void SetButtonStrings(string[] buttons)
		{
			for (int i = 0; i < buttons.Length && i < this.buttons.Length; i++)
				this.buttons[i].GetText = buttons[i];
		}

		public MenuButton ActiveButton
        {
            get
            {
				return buttons[current_position];
            }
        }
    
        public void SetActiveButton(int number)
        {
            if (number >= 0 && number < buttons.Length)
            {
                foreach (MenuButton button in buttons)
                {
                    if (button.TextureIndicator != ButtonIndicator.Button_Locked)
                    {
                        button.TextureIndicator = ButtonIndicator.Button_Normal;
                    }
                }
				for (int i = number; i < buttons.Length; i++)
				{
					if (buttons[i].TextureIndicator != ButtonIndicator.Button_Locked)
					{
						buttons[i].TextureIndicator = ButtonIndicator.Button_Hover;

						current_position = i;
						break;
					}
				}
            }
        }

        public void IncreseDecreasePosition(bool increase)
        {
            if (increase)
            {
                do
                {
                    current_position += 1;
                    if (current_position >= buttons.Length)
                    {
                        current_position = 0;
                    }
                }while (buttons[current_position].TextureIndicator == ButtonIndicator.Button_Locked);
            }
            else
            {
                do
                {
                    current_position -= 1;
                    if (current_position < 0)
                    {
                        current_position = buttons.Length - 1;
                    }
                } while (buttons[current_position].TextureIndicator == ButtonIndicator.Button_Locked);
            }
            SetActiveButton(current_position);
        }

        public void ChangeButtonState(int number, ButtonIndicator state)
        {
            buttons[number].TextureIndicator = state;
        }
    }
}
