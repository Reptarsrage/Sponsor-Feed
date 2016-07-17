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

        private const int timerInterval = 100;

        private int showForMSecs = 2000;
        private int fadeDuration = 250;
        private int fadesteps = 100;
        private int timeBetweenAlerts = 500;
        private bool allowFade = true;

        protected override bool ShowWithoutActivation
        {
            get { return true; }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams baseParams = base.CreateParams;

                const int WS_EX_NOACTIVATE = 0x08000000;
                const int WS_EX_TOOLWINDOW = 0x00000080;
                baseParams.ExStyle |= (int)(WS_EX_NOACTIVATE | WS_EX_TOOLWINDOW);

                return baseParams;
            }
        }

        public NotificationForm()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
            
            notifications = new Queue<string>();
            shown = false;
            this.BackColor = Color.Black;
            //this.TransparencyKey = Color.Black;
            this.textBox1.Text = "";
            this.textBox1.ForeColor = Color.Magenta;
            this.textBox1.WordWrap = true;
            this.textBox1.Enabled = false;
            this.TopMost = true;
            this.Show();
            this.timer1.Interval = timerInterval;
            this.timer1.Start();
        }

        private void fade(bool fadein) {
            if (!allowFade)
            {
                if (fadein)
                {
                    this.shown = true;
                    Opacity = 1.00;
                    this.timer1.Interval = showForMSecs; // time shown
                    this.timer1.Start();
                }
                else
                {
                    this.shown = false;
                    Opacity = 0.00;
                    this.timer1.Interval = timeBetweenAlerts; // time between
                    this.timer1.Start();
                }
                return;
            }


            int duration = fadeDuration;//in milliseconds
            int steps = fadesteps;
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
                        this.timer1.Interval = showForMSecs; // time shown
                        this.timer1.Start();
                    }
                    else {
                        this.shown = false;
                        this.timer1.Interval = timeBetweenAlerts; // time between
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
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!this.shown && this.notifications.Count == 0)
            {
                this.timer1.Interval = timerInterval;
                return; // wait
            }

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
