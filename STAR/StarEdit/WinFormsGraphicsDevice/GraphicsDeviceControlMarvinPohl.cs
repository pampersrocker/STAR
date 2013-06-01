using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace WinFormsGraphicsDevice
{
	public abstract partial class GraphicsDeviceControl : Control
	{
		Timer DrawTimer;
		bool ignoreFocus = false;
		bool activated = true;
		bool initialized;

		public bool Activated
		{
			get { return activated; }
			set { activated = value; }
		}

		public bool Initialized
		{
			get { return initialized; }
		}


		public bool IgnoreFocus
		{
			get { return ignoreFocus; }
			set { ignoreFocus = value; }
		}

		/// <summary>
		/// Updates The GameTime and Calls the protected abstract Update(GameTime gameTime) method.
		/// </summary>
		public new void Update()
		{
			if ((Focused || ignoreFocus) && activated)
			{
				UpdateGameTime();
				Update(gametime);
				//Draw(gametime);
				base.Refresh();
			}
		}

		public void DrawTimer_Tick(object sender, EventArgs e)
		{
			if (initialized)
				Update();
		}

		public void Redraw()
		{
			OnPaint(new PaintEventArgs(CreateGraphics(),DisplayRectangle));
		}

		private void UpdateGameTime()
		{
			lastframetime = currentframetime;
			currentframetime = DateTime.Now;
			timesincelastframe = currentframetime - lastframetime;
			timesincestart = currentframetime - starttime;
			//gametime = new GameTime(timesincestart, timesincelastframe, timesincestart, timesincelastframe);
			gametime = new GameTime(timesincestart, timesincelastframe);
		}



		protected abstract void Update(GameTime gameTime);
	}
}
