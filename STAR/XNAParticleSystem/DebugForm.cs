using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XNAParticleSystem
{
	static class DebugForm
	{
		public static List<string> debugText;

		public static void Initialize()
		{

			debugText = new List<string>();

		}

		public static void AddText(string text)
		{
			lock (debugText)
			{
				debugText.Add(text);
			}
		}
	}
}
