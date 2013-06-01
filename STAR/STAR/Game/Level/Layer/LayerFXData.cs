using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Star.Game.Level
{
    public enum LayerFX
    { 
        BackgroundFX,
        RearParallaxFX,
        FrontParallaxFX,
        CloudsFX,
        LevelFX,
        PostFX,
    }

    public struct LayerFXData
    {
        public bool HueEnabled;
        public bool ContrastEnabled;
		public bool SaturationEnabled;
        public float Hue;
        public float Saturation;
        public float Contrast;
        public float Brightness;
		public bool Colorize;

        public static LayerFXData Default
        {
            get
            {
                return new LayerFXData()
                {
                    Brightness = 0,
                    Contrast = 1,
                    ContrastEnabled = false,
                    Hue = 0,
                    HueEnabled = false,
                    Saturation = 1,
					SaturationEnabled = false,
					Colorize = true
                };
            }

        }

		public static LayerFXData FromString(string data)
		{
			CultureInfo cInfo = System.Globalization.CultureInfo.CreateSpecificCulture("en-us");
			int position = 0;
			LayerFXData fxData;
			fxData = LayerFXData.Default;

			if (!string.IsNullOrEmpty(data))
			{
				string[] stringValues = data.Split(',');

				for (int i = 0; i < stringValues.Length; i++)
				{
					stringValues[i] = stringValues[i].Trim();
				}



				try
				{
					fxData.HueEnabled = stringValues[0] == "1" ? true : false;
					position++;
					fxData.ContrastEnabled = stringValues[1] == "1" ? true : false;
					position++;
					fxData.SaturationEnabled = stringValues[2] == "1" ? true : false;
					position++;
					fxData.Hue = float.Parse(stringValues[3],cInfo);
					position++;
					fxData.Saturation = float.Parse(stringValues[4],cInfo);
					position++;
					fxData.Contrast = float.Parse(stringValues[5],cInfo);
					position++;
					fxData.Brightness = float.Parse(stringValues[6],cInfo);
				}
				catch (Exception e)
				{

					FileManager.WriteInErrorLog(fxData, "Couldn't parse FXString at position " + position.ToString() + "\n" + e.Message, e.GetType());
				}
			}

			return fxData;
		}

		public string DataString()
		{
			CultureInfo cInfo = System.Globalization.CultureInfo.CreateSpecificCulture("en-us");
			string data = "";

			data += (HueEnabled ? 1 : 0) + ",";
			data += (ContrastEnabled ? 1 : 0) + ",";
			data += (SaturationEnabled ? 1 : 0) + ",";
			data += Hue.ToString(cInfo) + ",";
			data += Saturation.ToString(cInfo) + ",";
			data += Contrast.ToString(cInfo) + ",";
			data += Brightness.ToString(cInfo) + ",";

			return data;
		}
    }
}
