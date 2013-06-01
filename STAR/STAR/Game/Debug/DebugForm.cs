using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Star.Game.Debug
{
    public partial class DebugForm : Form
    {
        public AddItemDelegate InvokeDelegate;

        public ListView ListView
        {
            get { return listView1; }
            set { listView1 = value; }
        }

        public delegate void AddItemDelegate(ListViewItem item);

        public void AddItem(ListViewItem item)
        {
            listView1.Items.Add(item);
        }

        public DebugForm()
        {
            InitializeComponent();
            InvokeDelegate = new AddItemDelegate(AddItem);
            listView1.View = View.Details;
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            
        }
    }
}
