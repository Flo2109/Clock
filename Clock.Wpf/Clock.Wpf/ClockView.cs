using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Clock.Wpf
{
    public class ClockView : Window
    {
        private readonly TextBlock _output;

        private readonly Brush _defaultForeground = Brushes.DimGray;
        private readonly Brush _hoverForeground = Brushes.White;
        private bool _isMouseOver;

        public System.Windows.Forms.Screen Screen { get; set; }

        public ClockView()
        {
            Icon = new BitmapImage(new Uri("pack://application:,,,/clock.ico"));
            Title = "Clock";

            _output = new TextBlock();
            _output.VerticalAlignment = VerticalAlignment.Center;
            _output.HorizontalAlignment = HorizontalAlignment.Center;
            Content = _output;
            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Tick += Timer_Tick;
            timer.Start();

            WindowStyle = WindowStyle.None;
            ShowInTaskbar = false;
            AllowsTransparency = true;
            var color = Brushes.Black;
            Background = color;
            _defaultForeground = color;
            _output.Foreground = _defaultForeground;
            Width = 35;
            Height = 15;
            MaxHeight = 15;
            MaxWidth = 35;
            MouseDoubleClick += ClockView_MouseDoubleClick;
            MouseDown += ClockView_MouseDown;
            MouseEnter += ClockView_MouseEnter;
            MouseLeave += ClockView_MouseLeave;
            UpdateTime();
        }

        private void ClockView_MouseLeave(object sender, MouseEventArgs e)
        {
            _isMouseOver = false;
            _output.Foreground = _defaultForeground;
        }

        private void ClockView_MouseEnter(object sender, MouseEventArgs e)
        {
            _isMouseOver = true;
            _output.Foreground = _hoverForeground;
        }

        private void ClockView_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            UpdateTime();
        }

        private void UpdateTime()
        {
            _output.Text = DateTime.Now.ToString("HH:mm");
            if (!_isMouseOver)
            {
                Topmost = false;
                Topmost = true;
            }

            UpdatePosition();
        }

        private void UpdatePosition()
        {
            try
            {
                if (Screen != null)
                {
                    double heightOffset = 37;
                    double widthOffset = 53;
                    if (Screen.Primary)
                        widthOffset = 105;
                    Top = Screen.Bounds.Y + Screen.Bounds.Height - heightOffset;
                    Left = Screen.Bounds.X + Screen.Bounds.Width - widthOffset;
                }
            }
            catch { }
        }

        private void ClockView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
