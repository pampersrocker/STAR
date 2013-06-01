using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StarEdit.Editor
{
    public partial class NewMapForm : Form
    {
        int x = 100;
        int y = 100;

        public int XSize
        {
            get { return x; }
        }

        public int YSize
        {
            get { return y; }
        }

        public NewMapForm()
        {
            InitializeComponent();
        }

        private void NewButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void XUpDownNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            x = (int)XUpDownNumericUpDown.Value;
        }

        private void YUpDown2_ValueChanged(object sender, EventArgs e)
        {
            y = (int)YUpDown2.Value;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void NewMapForm_Load(object sender, EventArgs e)
        {

        }
    }
}
