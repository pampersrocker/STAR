using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;

namespace Star.Game.Debug
{
	public static class DebugManager
	{
		static DebugForm debugForm = new DebugForm();
		static long number;
		static object lockObject = new object();

		public static void Initialize()
		{
			debugForm = new DebugForm();
			number = 0;
#if DEBUGCONSOLE
			debugForm.Show(); 
#endif
		}

		public static void Dispose()
		{
			debugForm.Close();
			
		}

		public static void AddItem(string Action, string Sender, StackTrace trace)
		{
			AddItem(Action, Sender, trace, Color.White);
		}

		public static void AddItem(string Action, string Sender, StackTrace trace, Color backColor)
		{
#if DEBUGCONSOLE
				if (debugForm.Visible)
				{
					number++;
					ListViewItem item = new ListViewItem(number.ToString());
					item.SubItems.Add(Action);

					item.SubItems.Add(Sender);
					item.SubItems.Add(trace.ToString());
					item.BackColor = backColor;
					//item.SubItems.Add(hashCode.ToString());

					debugForm.Invoke(debugForm.InvokeDelegate, new object[] { item });
				}  
#endif
		}
	}
}
