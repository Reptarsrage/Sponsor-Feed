/*
 * Justin Robb
 * 4/29/16
 * Subscriber Feed
 *  
 *  TODO: Allow notification modification like font, color, pictures, border, position ect.
 */

namespace SubscriberFeed
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;

    /// <summary>
    /// This form is used to display subscriber notifications 
    /// on top of all other applications on the screen.
    /// </summary>
    public partial class NotificationForm : Form
    {

        private Queue<string> notifications;
        private bool shown;

        public NotificationForm()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
            this.timer1.Interval = 10000;
            notifications = new Queue<string>();
            shown = false;
            this.BackColor = Color.Black;
            this.TransparencyKey = Color.Black;
            this.textBox1.Text = "";
            this.textBox1.ForeColor = Color.Magenta;
            this.textBox1.WordWrap = true;
            this.textBox1.Enabled = false;
            this.TopMost = true;
            this.Show();
        }

        private void fade(bool fadein) {
            int duration = 500;//in milliseconds
            int steps = 100;
            Timer timer = new Timer();
            timer.Interval = duration / steps;

            int currentStep = 0;
            timer.Tick += (arg1, arg2) =>
            {
                if (fadein)
                {
                    Opacity = ((double)currentStep) / steps;
                }
                else {
                    Opacity = ((double)(steps - currentStep)) / steps;
                }
                currentStep++;

                if (currentStep >= steps)
                {
                    if (fadein)
                    {
                        this.shown = true;
                        this.timer1.Interval = 10000;
                        this.timer1.Start();
                    }
                    else {
                        this.shown = false;
                        this.timer1.Interval = 1000;
                        this.timer1.Start();
                    }

                    timer.Stop();
                    timer.Dispose();
                }
            };

            timer.Start();
        }

        public void EnqueueNotification(string msg)
        {
            this.notifications.Enqueue(msg);
            if (!shown && !timer1.Enabled)
            {
                fade(true);
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.TopMost = true;
            this.timer1.Stop();
            if (!this.shown && this.notifications.Count > 0)
            {
                this.textBox1.Clear();
                this.textBox1.Text = this.notifications.Dequeue();
                fade(true);
            }
            else if (this.shown)
            {
                fade(false);
            }
        }
    }
}
