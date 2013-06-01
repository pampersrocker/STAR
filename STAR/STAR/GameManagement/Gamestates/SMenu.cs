using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using Star.Input;
using Star.Menu;
using Star.Game.Level;
using Star.Game;
using Star.Menu.OptionsMenu;

namespace Star.GameManagement.Gamestates
{

	class SMenu : IGameState
	{
		Camera camera;
		CurrentMenu newmenu;
		ContentManager content;
		MenuList[] menulists;
		CurrentMenu currentmenu = CurrentMenu.MainMenu;
		CustMapMenu custmapmenu;
		MainMenu mainmenu;
		OptionsMenu optionsMenu;
		CurrentMenu oldMenu = CurrentMenu.NoChange;
		int current_menu_list = 0;
		int current_pos = 0;
		SpriteFont font;
		Texture2D background;
		Rectangle bgrect;
		CloudLayer cloudlayer;
		string level_name;
		LevelType leveltype;
		TransitionEffect oldMenuEffect;
		TransitionEffect newMenueEffect;
		int screenWidth,screenHeight;
		Options options;
		PlayerBackground pBackground;
		

		public string GetLevelName
		{
			get { return level_name; }
		}

		public LevelType GetLevelType
		{
			get { return leveltype; }
		}

		public CurrentMenu CurrentMenu
		{
			get { return currentmenu; }
			set 
			{ 
				currentmenu = value;
				current_pos = 0;
				current_menu_list = 0;
				menulists[current_menu_list].SetActiveButton(current_pos);
			}
		}

		public SMenu(IServiceProvider ServiceProvider)
		{
			content = new ContentManager(ServiceProvider);
			content.RootDirectory = "Data";
			
			current_pos = 0;
			custmapmenu = new CustMapMenu(ServiceProvider);
			menulists = new MenuList[4];
			cloudlayer = new Star.Game.Level.CloudLayer();
			string[] names;
			names = new string[5];
			names[0] = "Story";
			names[1] = "Custom Maps";
			names[2] = "Options";
			names[3] = "Credits";
			names[4] = "Quit";
			menulists[0] = new MenuList("Main Menu",names);
			menulists[0].ChangeButtonState(0, ButtonIndicator.Button_Hover);
			names = new string[3];
			names[0] = "Continue";
			names[1] = "Start New Story";
			names[2] = "Mission Selection";
			menulists[1] = new MenuList("Story", names);
			mainmenu = new MainMenu();
			optionsMenu = new OptionsMenu();
			pBackground = new PlayerBackground(ServiceProvider);
			oldMenuEffect = new TransitionEffect(Vector2.Zero, TransitionType.Translate);

			newMenueEffect = new TransitionEffect(Vector2.Zero, TransitionType.Translate);
		}
		#region IGameState Member

		public void Initialize(Options options, GraphicsDevice graphics)
		{
			camera = new Camera(options.ScreenWidth, options.ScreenHeight, new Vector2(options.ScreenWidth / 2, options.ScreenHeight / 2), options.ScaleFactor);
			this.options = options;
			optionsMenu.Options = options;
			optionsMenu.Initialize(content.ServiceProvider, graphics, new MenuList[1]);
			MenuList[] mainmenuList = new MenuList[1];
			string[] names = new string[5];
			names[0] = "Story";
			names[1] = "Custom Maps";
			names[2] = "Options";
			names[3] = "Credits";
			names[4] = "Quit";
			mainmenuList[0] = new MenuList("S.T.A.R.", names);
			mainmenuList[0].ChangeButtonState(0, ButtonIndicator.Button_Locked);
			mainmenuList[0].SetActiveButton(0);
			font = content.Load<SpriteFont>("Stuff/Arial");
			background = content.Load<Texture2D>("Img/Menu/StdBG");
			//bgrect = new Rectangle(0, 0, options.ScreenWidth, options.ScreenHeight);
			int height = (int)(((float)options.ScreenWidth / background.Width) * background.Height);
			if (height >= options.ScreenHeight)
			{
				bgrect = new Rectangle(0, options.ScreenHeight - height, options.ScreenWidth, height);
			}
			else
			{
				int width = (int)(((float)options.ScreenHeight / background.Height) * background.Width);
				bgrect = new Rectangle(options.ScreenWidth - width, 0, width, options.ScreenHeight);
			} 
			screenWidth = options.ScreenWidth;
			screenHeight = options.ScreenHeight;
			options.InitObjectHolder.dataHolder.PutData(Layer.Data_Layer.NumLayerObjects.GetKey(),4);
			cloudlayer.Initialize(options, new LevelVariables());
			custmapmenu.Initialize(options);
			mainmenu.Initialize(content.ServiceProvider, graphics, mainmenuList);
			pBackground.Initialize(options);
		}

		public EGameState Update(Microsoft.Xna.Framework.GameTime gameTime, Input.Inputhandler inputhandler,Options options)
		{
			List<MenuKeys> keys = inputhandler.GetMenuKeys;
			List<MenuKeys> oldkeys = inputhandler.GetOldMenuKeys;
			EGameState gamestate = EGameState.Menu;
			cloudlayer.Update(gameTime, Vector2.Zero, options,Vector2.Zero);
			newMenueEffect.Update(gameTime);
			oldMenuEffect.Update(gameTime);
			//if (keys != oldkeys)
			{
				switch (currentmenu)
				{
					case CurrentMenu.MainMenu:
						newmenu = mainmenu.Update(gameTime, inputhandler);
						DoMenuTransition(newmenu);
						if (currentmenu == CurrentMenu.Quit)
							gamestate = EGameState.Quit;
						else if (currentmenu == Menu.CurrentMenu.Credits)
						{
							gamestate = EGameState.Credits;
							currentmenu = Menu.CurrentMenu.MainMenu;
						}
						break;

					case CurrentMenu.CustomMaps:
						newmenu = custmapmenu.Update(gameTime, inputhandler);
						DoMenuTransition(newmenu);
						if (newmenu == CurrentMenu.LoadLevel)
						{
							leveltype = LevelType.CustomLevels;
							level_name = custmapmenu.GetLevelName;
							gamestate = EGameState.Loading;
						}
						break;
					case CurrentMenu.LoadLevel:
						gamestate = EGameState.Loading;
						break;
					case CurrentMenu.Options:
						newmenu = optionsMenu.Update(gameTime, inputhandler);
						DoMenuTransition(newmenu);
						break;
						
				}
			}

			return gamestate;
		}

		private void DoMenuTransition(CurrentMenu newMenu)
		{
			if (newmenu != currentmenu)
			{
				oldMenu = currentmenu;
				currentmenu = newMenu;
				ResetTransition();
			}
			else
				currentmenu = newMenu;
		}

		private void ResetTransition()
		{
			newMenueEffect = new TransitionEffect(Vector2.Zero, TransitionType.Translate);
			oldMenuEffect = new TransitionEffect(new Vector2(0,100), TransitionType.FadeOut);
		}

		private void UpdateMainMenu(List<MenuKeys> keys,List<MenuKeys> oldkeys,ref EGameState gamestate,ref MenuAction action)
		{
			if (keys.Contains(Star.Input.MenuKeys.Up) && !oldkeys.Contains(Star.Input.MenuKeys.Up))
			{
				int temp = current_pos;
				temp -= 1;
				if (temp < 0)
				{
					temp = menulists[current_menu_list].Buttons.Length - 1;
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
			action = checkactions(keys, oldkeys);
			if (action == MenuAction.Quit)
			{
				gamestate = EGameState.Quit;
			}
		}

		public void Draw(Microsoft.Xna.Framework.GameTime gametime,SpriteBatch spritebatch,GraphicsDevice graphics)
		{
			spritebatch.Begin();
			spritebatch.Draw(background, bgrect, Color.White);
			spritebatch.End();
			
			cloudlayer.Draw(spritebatch);
			pBackground.Draw(spritebatch);
			switch (currentmenu)
			{
				case CurrentMenu.MainMenu:
					mainmenu.Draw(font, spritebatch, graphics,1,newMenueEffect.Matrix * Matrix.CreateScale(options.ScaleFactor));
					break;
				case CurrentMenu.CustomMaps:
					custmapmenu.Draw(font, spritebatch, graphics, 1, newMenueEffect.Matrix * Matrix.CreateScale(options.ScaleFactor));
					break;
				case CurrentMenu.Options:
					optionsMenu.Draw(font, spritebatch, graphics, 1, newMenueEffect.Matrix * Matrix.CreateScale(options.ScaleFactor));
					break;
			}

			switch (oldMenu)
			{ 
				case CurrentMenu.MainMenu:
					mainmenu.Draw(font, spritebatch, graphics, oldMenuEffect.AlphaFloat, oldMenuEffect.Matrix * Matrix.CreateScale(options.ScaleFactor));
					break;
				case CurrentMenu.CustomMaps:
					custmapmenu.Draw(font, spritebatch, graphics, oldMenuEffect.AlphaFloat, oldMenuEffect.Matrix * Matrix.CreateScale(options.ScaleFactor));
					break;
				case CurrentMenu.Options:
					optionsMenu.Draw(font, spritebatch, graphics, oldMenuEffect.AlphaFloat, oldMenuEffect.Matrix * Matrix.CreateScale(options.ScaleFactor));
					break;
			}
		}

		public void Unload()
		{
			Dispose();
		}

		#endregion

		public MenuAction checkactions(List<Input.MenuKeys> keys, List<Input.MenuKeys> oldkeys)
		{
			MenuAction action = MenuAction.Nothing;
			if (keys.Contains(Input.MenuKeys.Enter) && !oldkeys.Contains(Input.MenuKeys.Enter))
			{
				if (current_menu_list == 0 && currentmenu == CurrentMenu.MainMenu)
				{
					if (current_pos == 0)
					{
						current_menu_list = 1;
						current_pos = 0;
						menulists[current_menu_list].SetActiveButton(0);
					}
					else if (current_pos == 1)
					{
						currentmenu = CurrentMenu.CustomMaps;
					}
					else if (current_pos == 4)
					{
						action = MenuAction.Quit;
					}
					else if (current_pos == 3)
					{
						//action = MenuAction.ShowCredits;
					}
				}
			}
			else if (keys.Contains(Star.Input.MenuKeys.Back) && !oldkeys.Contains(Star.Input.MenuKeys.Back))
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

		#region IDisposable Member

		public void Dispose()
		{
			custmapmenu.Dispose();
			mainmenu.Dispose();
			UnloadGraphicsChanged();
		}

		#endregion

		#region IGraphicsChange Member

		public void GraphicsChanged(GraphicsDevice device, Options options)
		{
			camera = new Camera(options.ScreenWidth, options.ScreenHeight, new Vector2(options.ScreenWidth / 2, options.ScreenHeight / 2), options.ScaleFactor);
			cloudlayer.GraphicsChanged(device, options);
			int height = (int)(((float)options.ScreenWidth / background.Width) * background.Height);
			if (height>=options.ScreenHeight)
			{
				bgrect = new Rectangle(0, options.ScreenHeight - height, options.ScreenWidth, height);
			}
			else
			{
				int width = (int)(((float)options.ScreenHeight / background.Height) * background.Width);
				bgrect = new Rectangle(options.ScreenWidth - width, 0, width, options.ScreenHeight);
			}
			//bgrect = new Rectangle(0, 0, options.ScreenWidth, options.ScreenHeight);
		}

		#endregion

		#region IGraphicsChange Member


		public void UnloadGraphicsChanged()
		{
			GraphicsManager.RemoveItem(this);
		}

		#endregion

		#region IInitializeable Member

		bool initialized;

		public bool Initialized
		{
			get { return initialized; }
		}

		public void Initialize(Options options)
		{
			Initialize(options, options.InitObjectHolder.graphics);
			initialized = true;
		}

		#endregion
	}
}
