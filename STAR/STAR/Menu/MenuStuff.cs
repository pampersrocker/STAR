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
    public enum ButtonIndicator
    { 
        Button_Normal,
        Button_Hover,
        Button_Click,
        Button_Locked
    }

    public enum MenuAction
    { 
        Nothing,
        LoadLevel,
        ShowCredits,
        Quit,
    }

    public enum CurrentMenu
    { 
        MainMenu,
        Story,
        CustomMaps,
        Options,
        Credits,
        LoadLevel,
        NoChange,
        Quit
    }

    public enum LevelType
    { 
        Story,
        CustomLevels
    }

    /// <summary>
    /// Initialisiert, verwaltet , updatet und zeichnet das Menü
    /// </summary>
    /// 
    /*
    public class Menu
    {
        bool is_enabled = true;
        MenuList[] menulists;
        int current_menu_list = 0;
        int current_pos = 0;
        SpriteFont font;
        Texture2D[] textures;

        public Menu(ContentManager Content)
        {
            font = Content.Load<SpriteFont>("Stuff/Arial");
            current_pos = 0;
            menulists = new MenuList[4];
            
            string[] names;
            names = new string[5];
            names[0] = "Story";
            names[1] = "Custom Maps";
            names[2] = "Options";
            names[3] = "Credits";
            names[4] = "Quit";
            menulists[0] = new MenuList("Main Menu",names);
            menulists[0].ChangeButtonState(0, ButtonTextureIndicator.Button_Hover);
            names = new string[3];
            names[0] = "Continue";
            names[1] = "Start New Story";
            names[2] = "Mission Selection";
            menulists[1] = new MenuList("Story", names);
        }

        public MenuAction Update(GameTime gametime,List<Input.MenuKeys> keys,List<Input.MenuKeys> oldkeys)
        {
            MenuAction action = MenuAction.Nothing;
            //if (keys != oldkeys)
            {
                if(keys.Contains(STAR.Input.MenuKeys.Up) && !oldkeys.Contains(STAR.Input.MenuKeys.Up))
                {
                    int temp = current_pos;
                    temp -= 1;
                    if(temp < 0)
                    {
                        temp = menulists[current_menu_list].Buttons.Length -1;
                    }
                    menulists[current_menu_list].SetActiveButton(temp);
                    current_pos = temp;

                }
                if (keys.Contains(Input.MenuKeys.Down) && !oldkeys.Contains(Input.MenuKeys.Down))
                {
                    int temp = current_pos;
                    temp += 1;
                    if (temp >= menulists[current_menu_list].Buttons.Length)
                    {
                        temp = 0;
                    }
                    menulists[current_menu_list].SetActiveButton(temp);
                    current_pos = temp;
                }
                switch (current_menu_list)
                { 
                    case 0:

                        break;
                }
            }
            action = checkactions(keys,oldkeys);
            return action;
        }

        public MenuAction checkactions(List<Input.MenuKeys> keys,List<Input.MenuKeys> oldkeys)
        {
            MenuAction action = MenuAction.Nothing;
            if (keys.Contains(Input.MenuKeys.Enter) && !oldkeys.Contains(Input.MenuKeys.Enter))
            {
                if (current_menu_list == 0 && current_pos == 0)
                {
                    current_menu_list = 1;
                    current_pos = 0;
                    menulists[current_menu_list].SetActiveButton(0);
                }
                else if (current_menu_list == 0 && current_pos == 4)
                {
                    action = MenuAction.Quit;
                }
                else if (current_menu_list == 0 && current_pos == 3)
                {
                    action = MenuAction.ShowCredits;
                }
            }
            else if (keys.Contains(STAR.Input.MenuKeys.Back) && !oldkeys.Contains(STAR.Input.MenuKeys.Back))
            {
                if (current_menu_list == 1)
                {
                    current_menu_list = 0;
                    current_pos = 0;
                    menulists[current_menu_list].SetActiveButton(0);
                }
            }
            return action;
        }

        public void draw(SpriteBatch spritebatch)
        {
            spritebatch.Begin();
            foreach (MenuButton button in menulists[current_menu_list].Buttons)
            {
                //spritebatch.Draw(textures[(int)button.TextureIndicator], button.Rectangle, Color.White);
                if (button.TextureIndicator != ButtonTextureIndicator.Button_Hover)
                {
                    spritebatch.DrawString(font, button.GetText, button.Position, Color.White);
                }
                else
                {
                    spritebatch.DrawString(font, button.GetText, button.Position, Color.Red);
                }
            }
            spritebatch.End();
        }
    }
    */
}
