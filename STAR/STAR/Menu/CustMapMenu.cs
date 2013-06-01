using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Star.Input;
using Star.Menu;
using Star.GameManagement;

namespace Star.Menu
{
    public class CustMapMenu : Menu
    {
		Vector2 offset;
        ContentManager content;
        MenuList list;
        MenuList levels;
        string[] level_names;
        Texture2D levelstex;
        Rectangle levels_rect;
        string level_to_load;

        public string GetLevelName
        {
            get { return level_to_load; }
        }

        public CustMapMenu(IServiceProvider ServiceProvider)
        {
            content = new ContentManager(ServiceProvider);
            content.RootDirectory = "Data";
			base.lists = new MenuList[2];
			ReScanFolder();
            //levels = new MenuList("Levels", level_names);
            string[] temp = new string[2];
            temp[0] = "Refresh";
            temp[1] = "Back";
            list = new MenuList("Custom Maps", temp);
            list.SetActiveButton(0);
			
			lists[0] = list;
        }

        public void Initialize(Options options)
        {
			levels_rect = new Rectangle((int)((options.ScreenWidth / 100.0f) * 20f / options.ScaleFactor), (int)((options.ScreenHeight / 100f) * 20f / options.ScaleFactor), (int)((options.ScreenWidth / 100f) * 78f / options.ScaleFactor), (int)((options.ScreenHeight / 100f) * 70f / options.ScaleFactor));
            levels = new MenuList("Levels", level_names, levels_rect.X + 10, levels_rect.Y + 10, 0);
            levelstex = content.Load<Texture2D>("Stuff/Blank");
			lists[1] = levels;
        }

        public void ReScanFolder()
        {
            level_names = Directory.GetFiles("Data/Levels/CustomLevels/", "*.map");
            for (int i = 0; i < level_names.Length;i++)
            {
                level_names[i] = level_names[i].Replace("Data/Levels/CustomLevels/", "");
                level_names[i] = level_names[i].Replace(".map", "");
                level_names[i] = level_names[i].Trim();
            }
            levels = new MenuList("Levels", level_names,levels_rect.X +10,levels_rect.Y+10,0);
			lists[1] = levels;
        }

		public override CurrentMenu Update(GameTime gametime, Inputhandler inputhandler)
		{
			CurrentMenu currentmenu = CurrentMenu.CustomMaps;
			//DoUpDownMovement(gametime, inputhandler);
			//DoLeftRightMovement(gametime, inputhandler);
			base.Update(gametime, inputhandler);
			List<MenuKeys> keys = inputhandler.GetNewPressedMenuKeys;
			if (keys.Contains(MenuKeys.Enter))
			{
				switch (currentList)
				{
					case 0:
						switch (current_position)
						{
							case 0:
								ReScanFolder();
								break;
							case 1:
								currentmenu = CurrentMenu.MainMenu;
								break;
						}
						break;
					case 1:
						currentmenu = CurrentMenu.LoadLevel;
						level_to_load = levels.Buttons[current_position].GetText;
						break;
				}

			}
			if (keys.Contains(MenuKeys.Back))
			{
				currentmenu = CurrentMenu.MainMenu;
			}
			if (lists[currentList].ActiveButton.Rectangle.Bottom > levels_rect.Bottom-10)
			{
				offset.Y += -(lists[currentList].ActiveButton.Rectangle.Bottom+offset.Y - levels_rect.Bottom) * gametime.GetElapsedTotalSecondsFloat()*10;
			}
			if (lists[currentList].ActiveButton.Rectangle.Top + offset.Y < levels_rect.Top + 10)
			{
				offset.Y -= (lists[currentList].ActiveButton.Rectangle.Top+offset.Y - levels_rect.Top) * gametime.GetElapsedTotalSecondsFloat() *10;
			}

			return currentmenu;
		}

        public void Draw(GameTime gametime, SpriteBatch spritebatch, GraphicsDeviceManager graphics,SpriteFont font)
        {
            spritebatch.Begin();
            spritebatch.Draw(levelstex,levels_rect,new Color(1f,1f,1f,0.1f));
            foreach (MenuButton button in list.Buttons)
            {
                spritebatch.DrawString(font, button.GetText, button.Position, button.GetColor, button.Rotation,new Vector2(button.Rectangle.Center.X - button.Rectangle.X, button.Rectangle.Center.Y - button.Rectangle.Y),button.Scale, SpriteEffects.None,0 );
            }
            foreach (MenuButton button in levels.Buttons)
            {
                spritebatch.DrawString(font, button.GetText, button.Position, button.GetColor);
            }
            spritebatch.End();
        }

		public override void Draw(SpriteFont font, SpriteBatch spritebatch, GraphicsDevice graphics, float Alpha, Matrix matrix)
		{
			spritebatch.Begin(SpriteSortMode.Immediate,
				BlendState.NonPremultiplied,
				null,
				null,
				null,
				null,
				matrix);
			//spritebatch.Draw(levelstex, levels_rect, new Color(1f, 1f, 1f, 0.1f*Alpha));
			foreach (MenuButton button in list.Buttons)
			{
				spritebatch.DrawString(font, button.GetText, button.Position, new Color(button.GetColor.R, button.GetColor.G, button.GetColor.B, (int)(Alpha * 255)), button.Rotation, new Vector2(button.Rectangle.Center.X - button.Rectangle.X, button.Rectangle.Center.Y - button.Rectangle.Y), button.Scale, SpriteEffects.None, 0);
			}
			foreach (MenuButton button in levels.Buttons)
			{
				float AlphaPos = (button.Rectangle.Bottom+ offset.Y > levels_rect.Bottom) ? 1f - ((button.Rectangle.Bottom+ offset.Y - levels_rect.Bottom) / 50f) : 1f;
				AlphaPos = (button.Rectangle.Top + offset.Y < levels_rect.Top + 10) ? 1f - (-(button.Rectangle.Top + offset.Y - levels_rect.Top) / 50f) : AlphaPos;
				
				AlphaPos = MathHelper.Clamp(AlphaPos, 0, 1);
				spritebatch.DrawString(font, button.GetText, button.Position + offset, new Color(button.GetColor.R, button.GetColor.G, button.GetColor.B, (int)(Alpha * AlphaPos * 255)));
			}
			spritebatch.End();
		}
    }
}
