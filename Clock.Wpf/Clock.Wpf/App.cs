using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using Application = System.Windows.Application;

namespace Clock.Wpf
{
    public class App : Application
    {
        private NotificationManager _notifications;

        [STAThread]
        public static void Main()
        {
            App app = new App();
            app.Run();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _notifications = new NotificationManager();

            foreach (Screen screen in Screen.AllScreens)
            {
                ClockView clock = new ClockView();
                clock.Screen = screen;
                clock.Show();
            }
        }
    }
}
