/*
 * Justin Robb
 * 4/29/16
 * Subscriber Feed
 *  
 *  TODO: Notification customization
 *  TODO: Status Bar
 *  TODO: Collapsable log
 *  TODO: Resizing events
 *  TODO: Menu and menu items
 *  TODO: Collpase to taskbar
 *  
 */

namespace SubscriberFeed
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using System.ComponentModel;
    using System.Drawing;
    using Models;

    /// <summary>
    /// Controls the main display.
    /// </summary>
    public partial class MainForm : Form
    {
        private Dictionary<string, SubscriberSnippet> Subscribers;
        private Dictionary<string, SponsorSnippet> Sponsors;
        private string fetchPageToken;
        private bool suppressNotifications;
        NotificationForm notifyForm;
        bool bootstrapped;

        private void showNotification(string msg)
        {
            if (this.suppressNotifications)
            {
                return;
            }

            notifyForm.EnqueueNotification(msg);
        }

        public MainForm()
        {
            InitializeComponent();
            this.StopButton.Enabled = false;
            this.StartButton.Enabled = true;
            this.Subscribers = new Dictionary<string, SubscriberSnippet>();
            this.Sponsors = new Dictionary<string, SponsorSnippet>();
            fetchPageToken = null;
            this.QueryTimer.Interval = 5000;
            suppressNotifications = false;
            notifyForm = new NotificationForm();
            notifyForm.Location = this.Location;
            bootstrapped = false;
            APICommunicationManager.Instance.LogEvent += this.LogEvent;
        }

        private void LogEvent(object sender, LogEventArgs e)
        {
            this.log(e.Text, e.Color);
        }

        private void log(string t, Color? logColor = null) {
            this.LogTextBox.SelectionColor = logColor ?? Color.Black;
            this.LogTextBox.AppendText(t + "\r\n");
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (bootstrapped)
            {
                IOManager.Instance.SaveLocalCookie(ref this.Subscribers, ref this.Sponsors);
                base.OnClosing(e);
            }
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            this.StartButton.Enabled = false;
            if (bootstrapped)
            {
                this.StopButton.Enabled = true;
                this.QueryTimer.Start();
                return;
            }


            if (IOManager.Instance.LoadLocalCookie(ref this.Subscribers, ref this.Sponsors))
            {
                log("Loaded from local storage.");
                this.StopButton.Enabled = true;
                this.QueryTimer.Start();
                bootstrapped = true;
                return;
            }

            global::SubscriberFeed.Properties.Settings.Default.Code = null;
            global::SubscriberFeed.Properties.Settings.Default.RefreshToken = null;
            global::SubscriberFeed.Properties.Settings.Default.Token = null;

            if (APICommunicationManager.Instance.Authorize())
            {
                this.StopButton.Enabled = true;
                log("Got user token. " + global::SubscriberFeed.Properties.Settings.Default.Code);
                if (!APICommunicationManager.Instance.getAccessToken())
                {
                    log("Error getting user's token");
                    this.StartButton.Enabled = true;
                    return;
                }

                // get, initially, all recent subs and sponsors
                try
                {
                    log("Initializing subscribers");
                    SubscriberPage page = new SubscriberPage();
                    page.NextPageToken = null;
                    do
                    {
                        page = APICommunicationManager.Instance.FetchNewSubscribers(25, page.NextPageToken);

                        ParseSubscribers(page);

                    } while (page != null && page.NextPageToken != null);

                    log("Subscribers loaded.");
                }
                catch (Exception ex)
                {
                    log("Unable to load subscribers.", Color.Red);
                    log(ex.Message, Color.Red);
                    this.StartButton.Enabled = true;
                    this.StopButton.Enabled = false;
                    return;
                }

                try
                {
                    log("Initializing sponsors");
                    SponsorPage pageSponsor = new SponsorPage();
                    pageSponsor.NextPageToken = null;
                    do
                    {
                        pageSponsor = APICommunicationManager.Instance.FetchNewSponsors(25, pageSponsor.NextPageToken);

                        ParseSponsors(pageSponsor);

                    } while (pageSponsor != null && pageSponsor.NextPageToken != null);

                    log("Sponsors loaded.");
                }
                catch (Exception ex)
                {
                    log("Unable to load sponsors.", Color.Red);
                    log(ex.Message, Color.Red);
                    this.StartButton.Enabled = true;
                    this.StopButton.Enabled = false;
                    return;
                }

                log("Started queries");
                bootstrapped = true;
                this.QueryTimer.Start();
            }
            else {
                log("Error getting user's token");
                this.StartButton.Enabled = true;
                this.StopButton.Enabled = false;
                return;
            }
        }

        private void ParseSubscribers(SubscriberPage page)
        {
            if (page != null)
            {
                foreach (Subscriber sub in page.Items)
                {
                    if (!this.Subscribers.ContainsKey(sub.SubscriberSnippet.ChannelId))
                    {
                        this.Subscribers.Add(sub.SubscriberSnippet.ChannelId, sub.SubscriberSnippet);
                        this.showNotification("New subscriber " + sub.SubscriberSnippet.Title + "!");
                    }
                }

                this.fetchPageToken = page.NextPageToken;
            }
        }

        private void ParseSponsors(SponsorPage page)
        {
            if (page != null)
            {
                foreach (Sponsor sponsor in page.Items)
                {
                    if (!this.Sponsors.ContainsKey(sponsor.SponsorSnippet.ChannelId))
                    {
                        this.Sponsors.Add(sponsor.SponsorSnippet.ChannelId, sponsor.SponsorSnippet);
                        this.showNotification("New sponsor " + sponsor.SponsorSnippet.SponsorDetails.DisplayName + "!");
                    }
                }

                this.fetchPageToken = page.NextPageToken;
            }
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            this.QueryTimer.Stop();
            this.StartButton.Enabled = true;
            this.StopButton.Enabled = false;
        }

        private void QueryTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                ParseSubscribers(APICommunicationManager.Instance.FetchNewSubscribers(25, this.fetchPageToken));
                ParseSponsors(APICommunicationManager.Instance.FetchNewSponsors(25, this.fetchPageToken));
            }
            catch (Exception ex)
            {
                log("An error occured.", Color.Red);
                log(ex.Message, Color.Red);
                this.QueryTimer.Stop();
                this.StartButton.Enabled = true;
                this.StopButton.Enabled = false;
                return;
            }
        }

        private void DeleteLocalButton_Click(object sender, EventArgs e)
        {
            IOManager.Instance.DeleteLocalCookie();
        }

        private void TestButton_Click(object sender, EventArgs e)
        {
            this.showNotification("Test!");
        }
    }
}
