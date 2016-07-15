using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscriberFeed.Models
{
    public class LogEventArgs : EventArgs
    {
        private string text;
        private Color color;


        public string Text
        {
            get { return this.text; }
            set { this.text = value; }
        }

        public Color Color
        {
            get { return this.color; }
            set { this.color = value; }
        }

        // Constructor. 
        public LogEventArgs()
        {
            this.text = null;
            this.color = Color.Black;
        }

        public LogEventArgs(string text)
        {
            this.text = text;
            this.color = Color.Black;
        }

        public LogEventArgs(string text, Color color)
        {
            this.text = text;
            this.color = color;
        }
    }
}
