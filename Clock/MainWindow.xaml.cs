using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Color = System.Windows.Media.Color;
using Image = System.Windows.Controls.Image;

namespace Clock
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isStart { get; set; }

        private Timer timer { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            //设置初始背景色
            var now = DateTime.Now;
            TimeLabel.Content = now.ToString("T");
            ColorLabel.Content = TimeToColor(now.Hour, now.Minute, now.Second);
            var color = System.Drawing.ColorTranslator.FromHtml(TimeToColor(now.Hour, now.Minute, now.Second));
            BackGroundBrush.Color = Color.FromRgb(color.R, color.G, color.B);

            //设置0.5秒的透明度过渡
            var colorAnimation = new DoubleAnimation
            {
                From = 0, To = 1, Duration = new Duration(TimeSpan.FromSeconds(0.5))
            };
            var story = new Storyboard();
            story.Children.Add(colorAnimation);
            Storyboard.SetTargetName(colorAnimation, "Window");
            Storyboard.SetTargetProperty(colorAnimation, new PropertyPath("Opacity", RenderTransformProperty, TranslateTransform.XProperty));
            story.Begin(this);

            Mouse.OverrideCursor = Cursors.None;
            //UpdateLabel();
        }

        private void Init(object sender, EventArgs e)
        { 
            ColorLabel.FontSize = Width / 24;
            TimeLabel.FontSize = Width / 16;
            BackGroundBrush.Opacity = double.Parse(ConfigurationManager.AppSettings["Opacity"]);
            timer=new Timer
            {
                Interval = 1000,
                Enabled = true
            };
            timer.Elapsed += Timer_Tick;
        }

        private void Timer_Tick(object sender, ElapsedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(UpdateLabel));
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            isStart = true;
        }
        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (isStart)
            {
                Exit();
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            Exit();
        }

        private void Exit()
        {
            Environment.Exit(0);
        }

        private void UpdateLabel()
        {
            var now = DateTime.Now;
            TimeLabel.Content = now.ToString("T");
            ColorLabel.Content = TimeToColor(now.Hour, now.Minute, now.Second);
            var color = ColorTranslator.FromHtml(ColorLabel.Content.ToString());

            var colorAnimation=new ColorAnimation();
            colorAnimation.From = BackGroundBrush.Color;
            colorAnimation.To = Color.FromRgb(color.R, color.G, color.B);
            colorAnimation.Duration=new Duration(TimeSpan.FromSeconds(1));
            var story = new Storyboard();
            story.Children.Add(colorAnimation);
            Storyboard.SetTargetName(colorAnimation, "BackGroundBrush");
            Storyboard.SetTargetProperty(colorAnimation, new PropertyPath("Color", RenderTransformProperty, TranslateTransform.XProperty));

            story.Begin(this);
        }

        private string TimeToColor(int hour, int minute, int second)
        {
            
            return
                $"#{(int)(hour / 24d * 256):x2}{(int)(minute / 60d * 256):x2}{(int)(second / 60d * 256):x2}";
            /*
            if (second % 2 == 1)
                return "#66ccff";
            return "#FF6666";*/
        }
    }
}
