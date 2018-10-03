using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Clock.Wpf
{
    public class NotificationManager
    {
        DispatcherTimer _timer;
        ILookup<string, Notification> _notifications;

        public NotificationManager()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
            _timer.Start();

            _notifications = ConfigurationManager.AppSettings["Notifications"]?.Split(';').Select(n =>
           {
               string[] parts = n.Split('|');
               return new Notification
               {
                   Time = parts[0],
                   Message = parts[1]
               };
           }).ToLookup(k => k.Time);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            string time = now.ToString("HH:mm");
            StringBuilder sb = new StringBuilder();
            foreach (var n in _notifications[time].Where(t => t.LastShown.AddMinutes(2) < now))
            {
                if (sb.Length == 0)
                    sb.AppendLine(time);

                sb.AppendLine(n.Message);
                n.LastShown = now;
            }
            if (sb.Length > 0)
                MessageBox.Show(sb.ToString());
        }


    }

    public class Notification
    {
        public string Message { get; set; }
        public string Time { get; set; }
        public DateTime LastShown { get; set; }
    }
}
