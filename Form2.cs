using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 颜色时钟屏保
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            if (Settings1.Default.Colorrange == 0)
                radioButton1.Checked = true;
            else
                radioButton2.Checked = true;
            trackBar1.Value = (int)(Settings1.Default.Opacity * 100);
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            label3.Text = trackBar1.Value.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
                Settings1.Default.Colorrange = 0;
            else
                Settings1.Default.Colorrange = 1;
            Settings1.Default.Opacity = trackBar1.Value / 100.0F;
            Settings1.Default.Save();
            Application.Exit();
        }
    }
}
