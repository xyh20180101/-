using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

namespace 颜色时钟屏保
{
    public partial class Form1 : Form
    {
        FontFamily wryh = new FontFamily("微软雅黑");
        bool isLoad = false;//用于记录窗口过渡是否完成

        public Form1()
        {
            this.Opacity = 0.01;//等于0无法隐藏鼠标
            this.WindowState = FormWindowState.Maximized;
            Cursor.Hide();
            InitializeComponent();
        }

        public Form1(string[] args)
        {
            if (args[0].Substring(0, 2).Equals("/c"))//系统设置里的[设置]按钮
            {
                Application.Run(new Form2());
            }
            if (args[0].Substring(0, 2).Equals("/a"))//系统的[口令]设置
            {
                MessageBox.Show("这个屏幕保护程序没有可以设定口令的选项。", "颜色时钟屏保", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Process.GetCurrentProcess().Kill();
            }
            if (args[0].Substring(0, 2).Equals("/s"))//系统设置里的[预览]按钮
            {
                Application.Run(new Form1());
            }
            Process.GetCurrentProcess().Kill();//其他参数，包括系统设置里的[小屏幕预览]
        }

        void refresh()//刷新文字和背景颜色
        {
            int h = 0, m = 0, s = 0;
            int.TryParse(DateTime.Now.ToString("HH"), out h);
            int.TryParse(DateTime.Now.ToString("mm"), out m);
            int.TryParse(DateTime.Now.ToString("ss"), out s);
            label1.Text = DateTime.Now.ToString("HH:mm:ss");//更新时间文字
            if (Settings1.Default.Colorrange == 0)//更新颜色文字Colorrange=0是直接转换，=1是百分比转换
                label2.Text = "#" + DateTime.Now.ToString("HHmmss");
            else
            {
                int hh = 0, mm = 0, ss = 0;
                hh = (int)(h / 24.0F * 255);
                mm = (int)(m / 60.0F * 255);
                ss = (int)(s / 60.0F * 255);
                label2.Text = "#" + hh.ToString("X").PadLeft(2,'0') + mm.ToString("X").PadLeft(2, '0') + ss.ToString("X").PadLeft(2, '0');
            }
            this.BackColor = System.Drawing.ColorTranslator.FromHtml(label2.Text);//更新背景颜色
            /*if (BackColor.R * 0.3 + BackColor.G * 0.6 + BackColor.B * 0.1 > 255)//亮度大于某个值时字体颜色变化
            {
                label1.ForeColor = Color.Silver;
                label2.ForeColor = Color.Silver;
            }
            else
            {
                label1.ForeColor = Color.White;
                label2.ForeColor = Color.White;
            }
            */
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            refresh();

            /*时间文字=label1，颜色代码=label2，根据窗口大小自动设置位置和字体大小*/
            label1.Size = new Size(Width, (int)(Height * 0.5));
            label1.Location = new Point(0, 0);
            label1.Font = new Font(wryh, Width * 0.05F, FontStyle.Regular);

            label2.Size = new Size(Width, (int)(Height * 0.2));
            label2.Location = new Point(0, (int)(Height * 0.8));
            label2.Font = new Font(wryh, Width * 0.01F, FontStyle.Regular);
        }

        private void timer1_Tick(object sender, EventArgs e)//每秒进行2次刷新
        {
            refresh();
            this.BringToFront();
        }

        private void timer2_Tick(object sender, EventArgs e)//开始运行时设置0.5秒的透明度过渡
        {
            this.Opacity += Settings1.Default.Opacity / 50;
            if (this.Opacity >= Settings1.Default.Opacity)
            {
                isLoad = true;
                timer2.Enabled = false;
            }
        }

        void exit()
        {
            if (isLoad)//窗口未过渡完不允许退出
            {
                Process.GetCurrentProcess().Kill();
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            exit();
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            exit();
        }

        private void label1_MouseMove(object sender, MouseEventArgs e)
        {
            exit();
        }

        private void label2_MouseMove(object sender, MouseEventArgs e)
        {
            exit();
        }
    }
}