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
using Star.GameManagement;

namespace Star.Menu.OptionsMenu
{
	class OptionsMenu : Menu
	{
		public enum OptionsMenuPositions
		{ 
			Resolution,
			Display_Mode,
			Apply,
			Shader_Quality,
			Particle_Quality,
			Post_Process_Quality,
			Controller,
			Music_Volume,
			Back,
		}

		Options options;

		Resolution tempRes;
		int currentResPos;
		Star.GameManagement.DisplayMode tempDisplayMode;

		public Options Options
		{
			set { options = value; }
		}

		protected override void InitializeMenu()
		{
			OptionsMenuPositions[] optionsButtons = (OptionsMenuPositions[])Enum.GetValues(typeof(OptionsMenuPositions));
			string[] buttons = new string[optionsButtons.Length];
			lists = new MenuList[1];
			lists[0] = new MenuList("Options", buttons);
			lists[0].SetActiveButton(0);
			tempRes = options.Resolution;
			tempDisplayMode = options.DisplayMode;
			options.DisplayModeChanged += new DisplayModeChangedEventHandler(options_DisplayModeChanged);
			options.ResolutionChanged += new ResolutionChangedEventHandler(options_ResolutionChanged);
			RecreateStrings();
		}

		void options_ResolutionChanged(Options options, Resolution resolution)
		{
			tempRes = resolution;
		}

		void options_DisplayModeChanged(Options options, GameManagement.DisplayMode mode)
		{
			tempDisplayMode = mode;
		}

		private string GetOptionsString(OptionsMenuPositions e,OptionsID id)
		{
			
			if (options != null)
			{
				return e.ToSpacedString() + ": " + options.QualitySettings[id];
			}
			else
			{
				Dictionary<OptionsID, QualitySetting> defaultOptions = Options.DefaultSettings;
				return e.ToSpacedString() + ": " + defaultOptions[id];
			}
			
		}

		public override CurrentMenu Update(GameTime gametime, Inputhandler inputhandler)
		{
			CurrentMenu menu = CurrentMenu.Options;
			base.DoUpDownMovement(gametime, inputhandler);
			if (inputhandler.GetNewPressedMenuKeys.Contains(MenuKeys.Left) || inputhandler.GetNewPressedMenuKeys.Contains(MenuKeys.Right))
			{
				int relative = inputhandler.GetNewPressedMenuKeys.Contains(MenuKeys.Left) ? -1 : 1;
				switch (current_position)
				{ 
					case (int)OptionsMenuPositions.Resolution:
						tempRes = Resolution.GetAvailableResolutions.GetRelativeElement(currentResPos, relative);
						try
						{
							currentResPos = Resolution.GetAvailableResolutions.IndexOf(tempRes);
						}
						catch (Exception)
						{
							currentResPos = 0;
						}
						break;
					case (int)OptionsMenuPositions.Shader_Quality:
						options.QualitySettings[OptionsID.ShaderQuality] = options.QualitySettings[OptionsID.ShaderQuality].GetRelativeElement(relative);
						break;
					case (int)OptionsMenuPositions.Post_Process_Quality:
						options.QualitySettings[OptionsID.PostProcessQuality] = options.QualitySettings[OptionsID.PostProcessQuality].GetRelativeElement(relative);
						break;
					case (int)OptionsMenuPositions.Particle_Quality:
						options.QualitySettings[OptionsID.ParticleQuality] = options.QualitySettings[OptionsID.ParticleQuality].GetRelativeElement(relative);
						break;
					case (int)OptionsMenuPositions.Controller:
						options.Controller = options.Controller.GetRelativeElement(relative);
						break;
					case (int)OptionsMenuPositions.Display_Mode:
						tempDisplayMode = options.DisplayMode.GetRelativeElement(relative);
						break;
					case (int)OptionsMenuPositions.Music_Volume:
						options.MusicVolume = options.MusicVolume + relative * 5;
						break;
				}
				RecreateStrings();
			}
			if (inputhandler.GetNewPressedMenuKeys.Contains(MenuKeys.Back))
				menu = CurrentMenu.MainMenu;
			if (inputhandler.GetNewPressedMenuKeys.Contains(MenuKeys.Enter))
			{
				switch (current_position)
				{ 
					case (int)OptionsMenuPositions.Back:
						menu = CurrentMenu.MainMenu;
						break;
					case (int)OptionsMenuPositions.Apply:
						options.DisplayMode = tempDisplayMode;
						options.Resolution = tempRes;
						RecreateStrings();
						break;
				}
			}

			return menu;
		}

		private void RecreateStrings()
		{
			OptionsMenuPositions[] optionsButtons = (OptionsMenuPositions[])Enum.GetValues(typeof(OptionsMenuPositions));
			string[] buttons = new string[optionsButtons.Length];
			if (options != null)
			{
					buttons[(int)OptionsMenuPositions.Resolution] = OptionsMenuPositions.Resolution.ToSpacedString() + ": " + tempRes.ToString();
			}
			else
			{
				buttons[(int)OptionsMenuPositions.Resolution] = OptionsMenuPositions.Resolution.ToSpacedString() + ": <Unknown>";
			}
			buttons[(int)OptionsMenuPositions.Particle_Quality] = GetOptionsString(OptionsMenuPositions.Particle_Quality, OptionsID.ParticleQuality);
			buttons[(int)OptionsMenuPositions.Shader_Quality] = GetOptionsString(OptionsMenuPositions.Shader_Quality, OptionsID.ShaderQuality);
			buttons[(int)OptionsMenuPositions.Post_Process_Quality] = GetOptionsString(OptionsMenuPositions.Post_Process_Quality, OptionsID.PostProcessQuality);
			buttons[(int)OptionsMenuPositions.Controller] = OptionsMenuPositions.Controller.ToSpacedString() + ": " + options.Controller.ToSpacedString();
			buttons[(int)OptionsMenuPositions.Display_Mode] = OptionsMenuPositions.Display_Mode.ToSpacedString() + ": " + tempDisplayMode.ToSpacedString();
			buttons[(int)OptionsMenuPositions.Back] = OptionsMenuPositions.Back.ToSpacedString();
			buttons[(int)OptionsMenuPositions.Apply] = OptionsMenuPositions.Apply.ToSpacedString();
			buttons[(int)OptionsMenuPositions.Music_Volume] = OptionsMenuPositions.Music_Volume.ToSpacedString() + ": " + ((int)(options.MusicVolume));

			lists[0].SetButtonStrings(buttons);

		}
	}
}
