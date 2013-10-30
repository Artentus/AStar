using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AStarTester
{
    public partial class Form1 : Form
    {
        MapControl mapControl;

        public Form1()
        {
            InitializeComponent();

            mapControl = new MapControl(30, 30);
            Controls.Add(mapControl);
            mapControl.Location = new Point(20, 20);
            ClientSize = new Size(mapControl.Width + groupBox1.Width + 60, mapControl.Height + 40);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            mapControl.FindPath();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked) mapControl.SelectionMode = SelectionMode.Wall;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked) mapControl.SelectionMode = SelectionMode.Start;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked) mapControl.SelectionMode = SelectionMode.End;
        }
    }
}
