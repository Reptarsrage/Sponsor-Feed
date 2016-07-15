/*
 * Justin Robb
 * 4/29/16
 * Subscriber Feed
 *  
 */

namespace SubscriberFeed
{
    using System.Drawing;
    using System.Windows.Forms;

    /// <summary>
    /// We can use a transparent text box to display text in our 
    /// transparent notifications (<see cref="NotificationForm"/>) on top of all other applications on the screen.
    /// </summary>
    public partial class TransparentTextBox : RichTextBox
    {
        private const int WM_PAINT = 15;

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            if (m.Msg == WM_PAINT)
            {
                this.Invalidate();
                base.WndProc(ref m);

                // raise the paint event
                using (Graphics graphic = base.CreateGraphics())
                    OnPaint(new PaintEventArgs(graphic,
                     base.ClientRectangle));
            }
            else {
                base.WndProc(ref m);
            }
        }

        public TransparentTextBox()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.SupportsTransparentBackColor |
                 ControlStyles.OptimizedDoubleBuffer |
                 ControlStyles.ResizeRedraw |
                 ControlStyles.UserPaint, true);
            this.BorderStyle = BorderStyle.None;
            this.BackColor = Color.Transparent;
            this.ForeColor = Color.Magenta;
            this.Multiline = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (e.ClipRectangle.Equals(this.Bounds))
            {

                TextRenderer.DrawText(e.Graphics, this.Text, this.Font, e.ClipRectangle, this.ForeColor, TextFormatFlags.WordBreak | TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                //e.Graphics.DrawRectangle(new Pen(Color.Blue, 2.0F), e.ClipRectangle);
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
        }
    }
}
