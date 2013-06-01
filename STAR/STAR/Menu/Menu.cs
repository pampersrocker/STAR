using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Star.Input;
using Star.Game;

namespace Star.Menu
{
	public class Menu : IDisposable
	{
		public MenuList[] lists;
		protected int currentList;
		public int current_position = 0;
		ContentManager content;
		Vector2 title_pos;

		public void Initialize(IServiceProvider ServiceProvider,string title,string[] new_buttons,GraphicsDevice graphics)
		{
			Initialize(ServiceProvider, title, new_buttons, Vector2.Zero, graphics);
		}
		public void Initialize(IServiceProvider ServiceProvider, string title, string[] new_buttons,Vector2 new_title_pos, GraphicsDevice graphics)
		{
			content = new ContentManager(ServiceProvider);
			content.RootDirectory = "Data";
			lists = new MenuList[1];
			lists[0] = new MenuList(title, new_buttons);
			title_pos = new_title_pos;
			PresentationParameters pp = graphics.PresentationParameters;
			if (title_pos == Vector2.Zero)
			{
				title_pos = new Vector2(
					pp.BackBufferWidth / 20f,
					pp.BackBufferHeight / 20f);
			}
			InitializeMenu();
		}

		public void Initialize(IServiceProvider serviceProvider, GraphicsDevice graphics, MenuList[] menuLists)
		{
			content = new ContentManager(serviceProvider);
			content.RootDirectory = "Data";
			lists = menuLists;
			title_pos = Vector2.Zero;
			PresentationParameters pp = graphics.PresentationParameters;
			if (title_pos == Vector2.Zero)
			{
				title_pos = new Vector2(
					pp.BackBufferWidth / 20f,
					pp.BackBufferHeight / 20f);
			}
			InitializeMenu();
		}

		public virtual void Draw(SpriteFont font, SpriteBatch spritebatch, GraphicsDevice graphics, float Alpha)
		{
			Draw(font, spritebatch, graphics,Alpha, Matrix.CreateTranslation(Vector3.Zero));
		}

		public virtual void Draw(SpriteFont font, SpriteBatch spritebatch, GraphicsDevice graphics, float Alpha, Matrix matrix)
		{
			//3_1
			//spritebatch.Begin( SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState, matrix);
			//spritebatch.DrawString(font, lists[currentList].GetTitle, title_pos, new Color(Color.White,Alpha));
			spritebatch.Begin( SpriteSortMode.Immediate, null, null, null, null, null, matrix);
			spritebatch.DrawString(font, lists[currentList].GetTitle, title_pos, new Color(1f, 1f, 1f, Alpha));
			foreach (MenuList list in lists)
			{
				foreach (MenuButton button in list.Buttons)
				{
					spritebatch.DrawString(font, button.GetText, button.Position, new Color(button.GetColor.R,button.GetColor.G,button.GetColor.B, (int)(Alpha*255)), button.Rotation, new Vector2(button.Rectangle.Center.X - button.Rectangle.X, button.Rectangle.Center.Y - button.Rectangle.Y), button.Scale, SpriteEffects.None, 0);
				}
			}

			spritebatch.End();
		}

		public virtual CurrentMenu Update(GameTime gametime, Inputhandler inputhandler)
		{
			DoUpDownMovement(gametime, inputhandler);
			DoLeftRightMovement(gametime, inputhandler);
			return CurrentMenu.NoChange;
		}

		protected void DoUpDownMovement(GameTime gametime, Inputhandler inputhandler)
		{
			List<MenuKeys> keys = inputhandler.GetNewPressedMenuKeys;
			if (inputhandler.GetNewPressedMenuKeys.Contains(Star.Input.MenuKeys.Up))
			{
				int temp = current_position;
				temp -= 1;
				if (temp < 0)
				{
					temp = lists[currentList].Buttons.Length - 1;
				}
				lists[currentList].IncreseDecreasePosition(false);
				current_position = lists[currentList].GetCurrentPos;
			}
			if (inputhandler.GetNewPressedMenuKeys.Contains(Input.MenuKeys.Down))
			{
				int temp = current_position;
				temp += 1;
				if (temp >= lists[currentList].Buttons.Length)
				{
					temp = 0;
				}
				lists[currentList].IncreseDecreasePosition(true);
				current_position = lists[currentList].GetCurrentPos;
			}
		}

		protected void DoLeftRightMovement(GameTime gametime, Inputhandler inputhandler)
		{
			if (lists.Length > 1)
			{
				if (inputhandler.GetNewPressedMenuKeys.Contains(MenuKeys.Right))
				{
					int temp = currentList;
					temp++;
					if (temp >= lists.Length)
						temp = 0;
					lists[temp].SetActiveButton(lists[temp].GetCurrentPos);
					lists[currentList].ChangeButtonState(lists[currentList].GetCurrentPos, ButtonIndicator.Button_Normal);
					currentList = temp;
				}
				if (inputhandler.GetNewPressedMenuKeys.Contains(MenuKeys.Left))
				{
					int temp = currentList;
					temp--;
					if (temp < 0)
						temp = lists.Length - 1;
					lists[temp].SetActiveButton(lists[temp].GetCurrentPos);
					lists[currentList].ChangeButtonState(lists[currentList].GetCurrentPos, ButtonIndicator.Button_Normal);
					currentList = temp;
				}
			}
		}

		protected virtual void InitializeMenu()
		{ 
		
		}

		#region IDisposable Member

		public virtual void Dispose()
		{
			try
			{
				content.Dispose();
			}
			catch { }
		}

		#endregion
	}
}
