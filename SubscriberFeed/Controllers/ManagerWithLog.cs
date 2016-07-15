namespace SubscriberFeed.Controllers
{
    using SubscriberFeed.Models;
    using System.Drawing;

    public abstract class ManagerWithLog
    {
        public event LogEventHandler LogEvent;
        public delegate void LogEventHandler(object sender, LogEventArgs e);


        // ----------------------------------------
        // EVENTS
        // ----------------------------------------
        protected virtual void OnLog(LogEventArgs e)
        {
            if (LogEvent != null)
                LogEvent(this, e);
        }

        protected virtual void log(string t, Color? logColor = null)
        {
            LogEventArgs args = new LogEventArgs();
            args.Text = t;
            if (logColor != null)
            {
                args.Color = (Color)logColor;
            }
            this.OnLog(args);
        }
    }
}
