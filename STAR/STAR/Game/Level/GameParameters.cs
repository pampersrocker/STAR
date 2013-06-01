using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Star.GameManagement;

namespace Star.Game.Level
{
	public static class GameParameters
	{
		public static float Gravity=981f;
		public static string CurrentGraphXPack = "Grassy";
		public static float minRunFactor = 0.00001f;
		public static float RunFactorCoefficient = 0.5f;
		public static string CurrentGraphXPackPath
		{
			get { return GameConstants.GraphXPacksPath + CurrentGraphXPack + "/"; }
		}
	}
}
