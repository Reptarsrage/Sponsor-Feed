/*
 * Justin Robb
 * 4/29/16
 * Subscriber Feed
 *  
 *  TODO: Move hardcoded URL's to settings
 */

namespace SubscriberFeed
{
    using System;
    using System.Net;
    using System.Windows.Forms;

    /// <summary>
    /// This form is used to prompt user for an access token when the application is first started.
    /// </summary>
    public partial class EnterTokenForm : Form
    {
        private string AuthUrl;
        private string query;

        public EnterTokenForm()
        {
            InitializeComponent();

            AuthUrl = global::SubscriberFeed.Properties.Settings.Default.AuthURL;
            query = string.Format(
                "?client_id={0}&redirect_uri={1}&response_type=code&scope={2}&state=YouTube%20Subscriber%20Feed&access_type=offline",
                WebUtility.UrlEncode(global::SubscriberFeed.Properties.Settings.Default.ClientID),
                WebUtility.UrlEncode(SubscriberFeed.Properties.Settings.Default.CallbackUrl),
                global::SubscriberFeed.Properties.Settings.Default.AuthScope
                );

            this.linkLabelAuth.Text = AuthUrl;
            this.buttonDone.Enabled = false;
        }

        private void linkLabelAuth_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(AuthUrl + query);
            //global::SubscriberFeed.Properties.Settings.Default.Code = form.AuthorizationCode;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void buttonDone_Click(object sender, EventArgs e)
        {
            global::SubscriberFeed.Properties.Settings.Default.Code = this.textBox1.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (this.textBox1.Text.Length > 10)
            {
                this.buttonDone.Enabled = true;
            }
            else {
                this.buttonDone.Enabled = false;
            }
        }
    }
}
