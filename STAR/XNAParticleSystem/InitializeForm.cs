using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace XNAParticleSystem
{
	public partial class InitializeForm : Form
	{
		Game1 game;
		public InitializeForm(Game1 game)
		{
			this.game = game;
			InitializeComponent();
			listBox1.Items.Add("Newton");
			listBox1.Items.Add("InitializeNewtonAtom");
			listBox1.Items.Add("InitializeSnow");
			listBox1.Items.Add("InitializeExplosion");
			listBox1.Items.Add("InitializeBloodCollision");
			listBox1.Items.Add("InitializeMASSCollision");

			listBox1.Items.Add("InitializeCircle");
			listBox1.Items.Add("InitializeWaterfall");
			listBox1.Items.Add("InitializeNewtonComplete");
			listBox1.Items.Add("InitializeMassCollisionNOCUDA");
		}

		private void button1_Click(object sender, EventArgs e)
		{
			switch (listBox1.Items[listBox1.SelectedIndices[0]].ToString())
			{
				case "Newton":
					game.InitializeNEW(checkBox1.Checked);
					game.InitializeNewton();
					break;
				case "InitializeNewtonAtom":
					game.InitializeNEW(checkBox1.Checked);
					game.InitializeNewtonAtom();
					break;
				case "InitializeSnow":
					game.InitializeNEW(checkBox1.Checked);
					game.InitializeSnow();
					break;

				case "InitializeExplosion":
					game.InitializeNEW(checkBox1.Checked);
					game.InitializeExplosion();
					break;
				case "InitializeBloodCollision":
					game.InitializeNEW(checkBox1.Checked);
					game.InitializeBloodCollision();
					break;
				case "InitializeMASSCollision":
					game.InitializeNEW(checkBox1.Checked);
					game.InitializeMASSCollision();
					break;
				case "InitializeCircle":
					game.InitializeNEW(checkBox1.Checked);
					game.InitializeCircle();
					break;
				case "InitializeWaterfall":
					game.InitializeNEW(checkBox1.Checked);
					game.InitializeWaterfall();
					break;
				case "InitializeNewtonComplete":
					game.InitializeNEW(checkBox1.Checked);
					game.InitializeNewtonComplete();
					break;
				case "InitializeMassCollisionNOCUDA":
					game.InitializeNEW(checkBox1.Checked);
					game.InitializeMASSCollisionNOCUDA();
					break;
				default:
					break;
			}
			game.AddSystemsToManager();
		}

		private void buttonUpdate_Click(object sender, EventArgs e)
		{
			listBoxDebug.Items.Clear();
			lock (DebugForm.debugText)
			{
				for (int i = 0; i < DebugForm.debugText.Count; i++)
				{
					listBoxDebug.Items.Add(DebugForm.debugText[i]);
				}
			}
		}
	}
}
