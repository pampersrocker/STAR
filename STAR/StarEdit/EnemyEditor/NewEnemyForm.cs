using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;
using Star.GameManagement;

namespace StarEdit.EnemyEditor
{
	public partial class NewEnemyForm : Form
	{
		readonly string invaldichars= "\\/:*?\"<>|";
		string[] existingEnemies;
		public NewEnemyForm()
		{
			InitializeComponent();
			LoadExistingEnemies();
			EnemyCreate += new EnemyCreateEventHandler(NewEnemyForm_EnemyCreate);
		}

		void NewEnemyForm_EnemyCreate(string name)
		{
			//throw new NotImplementedException();
		}

		private void LoadExistingEnemies()
		{
			existingEnemies = Directory.GetDirectories("Data/" + GameConstants.EnemiesPath, "*", SearchOption.TopDirectoryOnly);
			for (int i = 0; i < existingEnemies.Length; i++)
			{
				string[] data =(existingEnemies[i].Split('/'));
				existingEnemies[i] = data[data.Length - 1];
			}
				
		}

		public delegate void EnemyCreateEventHandler(string name);

		public event EnemyCreateEventHandler EnemyCreate;

		private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (invaldichars.Contains(e.KeyChar))
			{
				e.Handled = true;
			}
			if (e.KeyChar == (char)Keys.Return)
			{
				if (!string.IsNullOrEmpty(textBox1.Text))
				{
					if (!existingEnemies.Contains(textBox1.Text))
					{
						EnemyCreate(textBox1.Text);
						Close();
					}
					else
					{
						MessageBox.Show(this, "This name already exists.", "Fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				}
				else
				{
					MessageBox.Show(this, "You may enter a name.", "Fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
				
		}

		private void abortButton_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void okButton_Click(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(textBox1.Text))
			{
				if (!existingEnemies.Contains(textBox1.Text))
				{
					EnemyCreate(textBox1.Text);
					Close();
				}
				else
				{
					MessageBox.Show(this, "This name already exists.", "Fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
			else
			{
				MessageBox.Show(this, "You may enter a name.", "Fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}
