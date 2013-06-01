using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Star.Input;
using Star.Game;
using System.IO;
using Star;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Star.GameManagement
{
	public enum QualitySetting
	{ 
		Low,
		Middle,
		High,
	}

	public enum DisplayMode
	{
		Fullscreen,
		Window,
	}

	public enum OptionsID
	{ 
		ShaderQuality,
		ParticleQuality,
		PostProcessQuality,
	}

	public enum OptionsFile
	{ 
		Resolution,
		ShaderQuality,
		ParticleQuality,
		PostProcessQuality,
		DisplayMode,
		Controller,
		MusicVolume
	}

	public struct Resolution : IComparable<Resolution>, IEquatable<Resolution>
	{
		public int ScreenWidth;
		public int ScreenHeight;

		public Resolution(int width, int height)
		{
			ScreenWidth = width;
			ScreenHeight = height;
		}

		#region Override Methods
		public override string ToString()
		{
			return ScreenWidth.ToString() + "x" + ScreenHeight.ToString();
		}

		public override bool Equals(object obj)
		{
			try
			{
				return this == (Resolution)obj;
			}
			catch (Exception)
			{
				return this.GetHashCode() == obj.GetHashCode();
			}
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		} 
		#endregion

		#region Static Methods
		public static Resolution Parse(string data)
		{
			string[] resolution = data.Split('x');
			Resolution newResolution = new Resolution();
			newResolution.ScreenWidth = int.Parse(resolution[0]);
			newResolution.ScreenHeight = int.Parse(resolution[1]);
			return newResolution;
		}

		public static List<Resolution> GetAvailableResolutions
		{
			get
			{
				List<Resolution> output = new List<Resolution>();

				output.Add(new Resolution(640, 480));
				output.Add(new Resolution(800, 600));
				output.Add(new Resolution(1024, 768));
				output.Add(new Resolution(1280, 1024));
				output.Add(new Resolution(1280, 720));
				output.Add(new Resolution(1366, 768));
				output.Add(new Resolution(1920, 1080));
				output.Add(new Resolution(1920, 1200));

				return output;
			}
		} 
		#endregion

		#region Operators
		
		public static bool operator ==(Resolution a, Resolution b)
		{
			return a.ScreenHeight == b.ScreenHeight && a.ScreenWidth == b.ScreenWidth;
		}

		public static bool operator !=(Resolution a, Resolution b)
		{
			return !(a == b);
		}

		public static bool operator >(Resolution a, Resolution b)
		{
			return a.ScreenWidth > b.ScreenWidth && a.ScreenHeight > b.ScreenHeight;
		}

		public static bool operator <(Resolution a, Resolution b)
		{
			return !(a > b) && a != b;
		}

		public static bool operator <=(Resolution a, Resolution b)
		{
			return a < b || a == b;
		}

		public static bool operator >=(Resolution a, Resolution b)
		{
			return a > b || a == b;
		} 

		#endregion

		#region IComparable<Resolution> Member

		public int CompareTo(Resolution other)
		{
			try
			{
				if (this < other)
					return 1;
				else if (this > other)
					return -1;
				else
					return 0;
			}
			catch (Exception)
			{
				return 0;				
			}
		}

		#endregion

		#region IEquatable<Resolution> Member

		public bool Equals(Resolution other)
		{
			return this == other;
		}

		#endregion
	}

	public delegate void DisplayModeChangedEventHandler(Options options,DisplayMode mode);
	public delegate void ResolutionChangedEventHandler(Options options, Resolution resolution);
	public delegate void ControllerChangedEventHandler(Options options, Controller controller);
	public delegate void MusicVolumeChangedEventHandler(Options options,float vol);

	public class Options
	{

		float baseResolutionForScale =  600;
		int musicVolume;

		public int MusicVolume
		{
			get { return musicVolume; }
			set
			{
				musicVolume = (int)MathHelper.Clamp(value, 0, 100);
				MusicVolumeChanged(this, MusicVolumeFloat);
			}
		}

		public float MusicVolumeFloat
		{
			get { return ((float)musicVolume) / 100f; }
		}
		Dictionary<OptionsID, QualitySetting> settings;
		Resolution resolution;
		Controller controller;
		DisplayMode displayMode;
		public InitObjectHolder InitObjectHolder;



		public event DisplayModeChangedEventHandler DisplayModeChanged;
		public event ResolutionChangedEventHandler ResolutionChanged;
		public event ControllerChangedEventHandler ControllerChanged;
		public event MusicVolumeChangedEventHandler MusicVolumeChanged;

		public DisplayMode DisplayMode
		{
			get { return displayMode; }
			set
			{
				displayMode = value;
				DisplayModeChanged(this, displayMode);
			}
		}

		public bool IsFullScreen
		{
			get { return displayMode == DisplayMode.Fullscreen; }
		}

		public Options()
		{
			DisplayModeChanged += new DisplayModeChangedEventHandler(Options_DisplayModeChanged);
			ControllerChanged += new ControllerChangedEventHandler(Options_ControllerChanged);
			ResolutionChanged += new ResolutionChangedEventHandler(Options_ResolutionChanged);
			MusicVolumeChanged += new MusicVolumeChangedEventHandler(Options_MusicVolumeChanged);
			settings = DefaultSettings;
			resolution.ScreenWidth = 800;
			resolution.ScreenHeight = 600;
			musicVolume = 80;
			controller = Controller.Keyboard;
			InitObjectHolder = new InitObjectHolder(null,null,null);
		}

		void Options_MusicVolumeChanged(Options options, float vol)
		{
			//throw new NotImplementedException();
		}

		void Options_ResolutionChanged(Options options, Resolution resolution)
		{
			//throw new NotImplementedException();
		}

		void Options_ControllerChanged(Options options, Controller controller)
		{
			//throw new NotImplementedException();
		}

		void Options_DisplayModeChanged(Options options, DisplayMode mode)
		{
			//throw new NotImplementedException();
		}

		public static Dictionary<OptionsID, QualitySetting> DefaultSettings
		{
			get 
			{
				Dictionary<OptionsID, QualitySetting> settings = new Dictionary<OptionsID, QualitySetting>();
				settings.Add(OptionsID.ParticleQuality, QualitySetting.High);
				settings.Add(OptionsID.PostProcessQuality, QualitySetting.High);
				settings.Add(OptionsID.ShaderQuality, QualitySetting.Low);
				return settings;
			}
		}

		public static Options FromSettingsFile()
		{
			Options options;
			options = Default;

			
			Dictionary<OptionsFile, string> data;
			try
			{
				data = FileManager.GetFileDict<OptionsFile>(GameConstants.CONFIG_FILE);
				Dictionary<OptionsID, QualitySetting> qualityDict = new Dictionary<OptionsID, QualitySetting>();
				try
				{
					qualityDict.Add(
					OptionsID.ShaderQuality,
					(QualitySetting)Enum.Parse(typeof(QualitySetting), (data[OptionsFile.ShaderQuality])));
					qualityDict.Add(
					OptionsID.PostProcessQuality,
					(QualitySetting)Enum.Parse(typeof(QualitySetting), (data[OptionsFile.PostProcessQuality])));
					qualityDict.Add(
						OptionsID.ParticleQuality,
						(QualitySetting)Enum.Parse(typeof(QualitySetting), (data[OptionsFile.ParticleQuality])));
					options.QualitySettings = qualityDict;
				}
				catch (Exception e) 
				{
					FileManager.WriteInErrorLog(options, "Failed to load Settings File :\n" + e.Message, e.GetType()); 
				}
				
				options.Resolution = Resolution.Parse(data[OptionsFile.Resolution]);
				options.controller = (Controller)Enum.Parse(typeof(Controller), data[OptionsFile.Controller]);
				options.displayMode = (DisplayMode)Enum.Parse(typeof(DisplayMode), data[OptionsFile.DisplayMode]);
				options.MusicVolume = int.Parse(data[OptionsFile.MusicVolume]);
			}
			catch (FileNotFoundException)
			{
				options = Default;
				options.Resolution = new Resolution(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
			}
			catch (Exception e)
			{
				FileManager.WriteInErrorLog(options, "Failed to load Settings File :\n" + e.Message, e.GetType()); 
			}
			

			return options;
		}

		public void ToFile()
		{
			string data = "";
			data += OptionsFile.Resolution.ToString() + "=" + Resolution.ToString() + ";\n";
			data += OptionsFile.DisplayMode.ToString() + "=" + displayMode.ToString() + ";\n";
			data += OptionsFile.Controller + "=" + controller.ToString() + ";\n";
			data += QualitySettings.DictToString();
			data += OptionsFile.MusicVolume.ToString() + "=" + MusicVolume.ToString() + ";\n";
			FileManager.WriteFile(data, GameConstants.CONFIG_FILE);
		}

		public static Options Default
		{ 
			get{
				Options options = new Options();
				options.QualitySettings = DefaultSettings;
				options.Resolution = Resolution.Parse("800x600");
				return options;
			}
		}

		public int ScreenWidth
		{
			get { return resolution.ScreenWidth; }
			set { resolution.ScreenWidth = value; }
		}

		public int ScreenHeight
		{
			get { return resolution.ScreenHeight; }
			set { resolution.ScreenHeight = value; }
		}

		public Dictionary<OptionsID, QualitySetting> QualitySettings
		{
			get { return settings; }
			set { settings = value; }
		}

		public Resolution Resolution
		{
			get { return resolution; }
			set
			{
				resolution = value;
				ResolutionChanged(this, resolution);
			}
		}

		public float ScaleFactor
		{
			get { return (((float)(resolution.ScreenHeight) / (float)baseResolutionForScale)); }
		}

		public Controller Controller
		{
			get { return controller; }
			set
			{
				controller = value;
				ControllerChanged(this, controller);
			}
		}
	}
}
