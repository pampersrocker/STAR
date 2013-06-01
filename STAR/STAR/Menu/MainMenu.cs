using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Star.Input;

namespace Star.Menu
{
    public class MainMenu : Menu
    {
		protected override void InitializeMenu()
		{
			current_position = 1;
		}

        public override CurrentMenu Update(GameTime gametime, Inputhandler inputhandler)
        {
			CurrentMenu currentmenu;
			base.Update(gametime,inputhandler);
            currentmenu = checkactions(inputhandler.GetMenuKeys, inputhandler.GetOldMenuKeys);
            
            return currentmenu;
        }

        public CurrentMenu checkactions(List<Input.MenuKeys> keys, List<Input.MenuKeys> oldkeys)
        {
            CurrentMenu action = CurrentMenu.MainMenu;
            if (keys.Contains(Input.MenuKeys.Enter) && !oldkeys.Contains(Input.MenuKeys.Enter))
            {
				if (current_position == 0)
				{
					//current_position = 0;
					//lists[currentList].SetActiveButton(0);
					//action = CurrentMenu.Story;
				}
				else if (current_position == 1)
				{
					action = CurrentMenu.CustomMaps;
				}
				else if (current_position == 4)
				{
					action = CurrentMenu.Quit;
				}
				else if (current_position == 3)
				{
					action = CurrentMenu.Credits;
				}
				else if (current_position == 2)
					action = CurrentMenu.Options;
            }
            return action;
        }
    }
}
