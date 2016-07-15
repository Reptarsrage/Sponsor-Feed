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
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Windows.Forms;
    using System.Xml.Serialization;
    using System.ComponentModel;
    using System.Drawing;
    /// <summary>
    /// Controls the main display.
    /// </summary>
    public partial class MainForm : Form
    {

        private Dictionary<string, SubscriberSnippet> Subscribers;
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
            fetchPageToken = null;
            this.QueryTimer.Interval = 5000;
            suppressNotifications = false;
            notifyForm = new NotificationForm();
            notifyForm.Location = this.Location;
            bootstrapped = false;
        }

        private bool LoadLocalCookie() {
            string path = Path.Combine(Path.GetTempPath(), global::SubscriberFeed.Properties.Settings.Default.LocalCookie);
            if (File.Exists(path))
            {
                try {
                    // check if file exists, if so, load it
                    XmlSerializer ser = new XmlSerializer(typeof(LocalApplicationCookie));
                    TextReader re = new StreamReader(path);
                    LocalApplicationCookie cookie = (LocalApplicationCookie)ser.Deserialize(re); // try
                    global::SubscriberFeed.Properties.Settings.Default.Code = cookie.Code;
                    global::SubscriberFeed.Properties.Settings.Default.RefreshToken = cookie.RefreshToken;
                    global::SubscriberFeed.Properties.Settings.Default.Token = cookie.Token;
                    foreach (SubscriberSnippet s in cookie.Subscribers)
                    {
                        if (this.Subscribers.ContainsKey(s.ChannelId)) this.Subscribers.Add(s.ChannelId, s);
                    }
                    log("Loaded local file, renewing credentials.");
                    if (!this.RenewCredentials())
                    {
                        log("Please re-enter your credentials.");
                        return false;
                    }
                    return true;
                } catch (IOException) {
                    // failed to load
                    return false;
                }
            }

            // if file doesn't exist, go through normal auth and load process
            return false;
        }

        private bool SaveLocalCookie()
        {
            try
            {
                LocalApplicationCookie cookie = new LocalApplicationCookie();
                cookie.Subscribers = new List<SubscriberSnippet>();
                foreach (SubscriberSnippet s in this.Subscribers.Values) {
                    cookie.Subscribers.Add(s);
                }
                cookie.Count = this.Subscribers.Values.Count;
                cookie.Code = global::SubscriberFeed.Properties.Settings.Default.Code;
                cookie.RefreshToken = global::SubscriberFeed.Properties.Settings.Default.RefreshToken;
                cookie.Token = global::SubscriberFeed.Properties.Settings.Default.Token;
                XmlSerializer ser = new XmlSerializer(typeof(LocalApplicationCookie));
                TextWriter writer = new StreamWriter(Path.Combine(Path.GetTempPath(), global::SubscriberFeed.Properties.Settings.Default.LocalCookie));
                ser.Serialize(writer, cookie);
                writer.Close();
                return true;
            }
            catch (IOException)
            {
                // failed to save
                return false;
            }
        }

        private void log(string t, Color? logColor = null) {
            this.LogTextBox.SelectionColor = logColor ?? Color.Black;
            this.LogTextBox.AppendText(t + "\r\n");
        }

        private bool RenewCredentials()
        {
            if (string.IsNullOrWhiteSpace(global::SubscriberFeed.Properties.Settings.Default.Code))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(global::SubscriberFeed.Properties.Settings.Default.RefreshToken))
            {
                // refresh token
                return false;
            }
            if (!string.IsNullOrWhiteSpace(global::SubscriberFeed.Properties.Settings.Default.Token) &&
                global::SubscriberFeed.Properties.Settings.Default.TokenExpiry != null && 
                DateTime.Now.CompareTo(global::SubscriberFeed.Properties.Settings.Default.TokenExpiry) < 0)
            {
                // already have a good token
                return true;
            }


            string url = SubscriberFeed.Properties.Settings.Default.CallbackUrl;
            string query = string.Format("client_id={0}&client_secret={1}&grant_type=refresh_token&refresh_token={2}&url={3}",
                WebUtility.UrlEncode(global::SubscriberFeed.Properties.Settings.Default.ClientID),
                WebUtility.UrlEncode(global::SubscriberFeed.Properties.Settings.Default.ClientSecret),
                WebUtility.UrlEncode(global::SubscriberFeed.Properties.Settings.Default.RefreshToken),
                WebUtility.UrlEncode(global::SubscriberFeed.Properties.Settings.Default.AuthTokenURL)
                );

            string resp = SendHttpWebRequest(url, HttpMethod.Post, query, "application/x-www-form-urlencoded");
            if (resp == null)
            {
                return false;
            }

            try
            {
                Token respToken = JsonConvert.DeserializeObject<Token>(resp);
                global::SubscriberFeed.Properties.Settings.Default.Token = respToken.AccessToken;
                global::SubscriberFeed.Properties.Settings.Default.TokenExpiry = DateTime.Now.AddSeconds(respToken.ExpiresIn);

                log("Access token: " + global::SubscriberFeed.Properties.Settings.Default.Token);

                return true;
            }
            catch (JsonException)
            {
                log("Error authorizing.");
                return false;
            }
        }

        private SubscriberPage FetchSubscribers(int count, string pageToken)
        {
            if (string.IsNullOrWhiteSpace(global::SubscriberFeed.Properties.Settings.Default.Code))
            {
                //QueryTimer.Stop();
                throw new SystemException("Invalid state.");
            }

            if (!string.IsNullOrWhiteSpace(global::SubscriberFeed.Properties.Settings.Default.Token) &&
                DateTime.Now.CompareTo(global::SubscriberFeed.Properties.Settings.Default.TokenExpiry) > -1)
            {
                // refresh token
                if (RenewCredentials())
                {
                    //QueryTimer.Start();
                    return null;
                }
                else {
                    this.StartButton.Enabled = true;
                    this.StopButton.Enabled = false;
                    return null;
                }
            }

            // make API call
            string url = SubscriberFeed.Properties.Settings.Default.CallbackUrl;
            string query = string.Format("url={0}&part=subscriberSnippet&mySubscribers=true&maxResults={1}&order=relevance&token={2}",
                WebUtility.UrlEncode(global::SubscriberFeed.Properties.Settings.Default.SubscriptionAPIEndpointURL),
                WebUtility.UrlEncode(count.ToString()),
                WebUtility.UrlEncode("Bearer " + global::SubscriberFeed.Properties.Settings.Default.Token)
                );

            if (!string.IsNullOrWhiteSpace(pageToken))
            {
                query += "&pageToken=" + WebUtility.UrlEncode(pageToken);
            }


            string resp = SendHttpWebRequest(url, HttpMethod.Post, query, "application/x-www-form-urlencoded");
            if (resp == null)
            {
                return null;
            }

            SubscriberPage page = JsonConvert.DeserializeObject<SubscriberPage>(resp);
            string[] subs = new string[] { };
            foreach (Subscriber sub in page.Items) {
                if (!this.Subscribers.ContainsKey(sub.SubscriberSnippet.ChannelId))
                {
                    this.Subscribers.Add(sub.SubscriberSnippet.ChannelId, sub.SubscriberSnippet);
                    this.showNotification("New subscriber " + sub.SubscriberSnippet.Title + "!");
                }
            }

            log("Response from server: " + page.Items.Length + " out of " + page.PageInfo.TotalResults + " subscribers.");

            return page;
        }


        private bool Authorize()
        {
            if (!string.IsNullOrWhiteSpace(global::SubscriberFeed.Properties.Settings.Default.Token) &&
                DateTime.Now.CompareTo(global::SubscriberFeed.Properties.Settings.Default.TokenExpiry) < 0)
            {
                // already authorized
                return true;
            }


            EnterTokenForm form = new EnterTokenForm();
            DialogResult result = form.ShowDialog();

            if (result != DialogResult.OK || string.IsNullOrWhiteSpace(global::SubscriberFeed.Properties.Settings.Default.Code))
            {
                return false;
            }

            return true;
        }

        private bool getAccessToken()
        {
            if (string.IsNullOrWhiteSpace(global::SubscriberFeed.Properties.Settings.Default.Code))
            {
                return false;
            }

            if (!string.IsNullOrWhiteSpace(global::SubscriberFeed.Properties.Settings.Default.Token) &&
                DateTime.Now.CompareTo(global::SubscriberFeed.Properties.Settings.Default.TokenExpiry) >= 0)
            {
                // TODO
                // refresh token
                return false;
            }
            if (!string.IsNullOrWhiteSpace(global::SubscriberFeed.Properties.Settings.Default.Token) &&
                DateTime.Now.CompareTo(global::SubscriberFeed.Properties.Settings.Default.TokenExpiry) < 0)
            {
                // already have a good token
                return true;
            }

            string url = SubscriberFeed.Properties.Settings.Default.CallbackUrl;
            string query = string.Format("code={0}&client_id={1}&client_secret={2}&grant_type=authorization_code&redirect_uri={3}&url={4}",
                WebUtility.UrlEncode(global::SubscriberFeed.Properties.Settings.Default.Code),
                WebUtility.UrlEncode(global::SubscriberFeed.Properties.Settings.Default.ClientID),
                WebUtility.UrlEncode(global::SubscriberFeed.Properties.Settings.Default.ClientSecret),
                WebUtility.UrlEncode(url),
                WebUtility.UrlEncode(global::SubscriberFeed.Properties.Settings.Default.AuthTokenURL)
                );

            string resp = SendHttpWebRequest(url, HttpMethod.Post, query, "application/x-www-form-urlencoded");
            if (resp == null)
            {
                return false;
            }

            try
            {
                Token respToken = JsonConvert.DeserializeObject<Token>(resp);
                global::SubscriberFeed.Properties.Settings.Default.Token = respToken.AccessToken;
                global::SubscriberFeed.Properties.Settings.Default.TokenExpiry = DateTime.Now.AddSeconds(respToken.ExpiresIn);
                global::SubscriberFeed.Properties.Settings.Default.RefreshToken = respToken.RefreshToken;

                log("Access token: " + global::SubscriberFeed.Properties.Settings.Default.Token);

                return true;
            }
            catch (JsonException)
            {
                log("Error authorizing.", Color.Red);
                log(resp, Color.Red);
                return false;
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (bootstrapped)
            {
                this.SaveLocalCookie();
                base.OnClosing(e);
            }
        }

        /// <summary>
        /// Sends an HTTP request to the given URL, and receives the response.
        /// Note: This does not do a check to see if the currently stored creds are valid.
        /// </summary>
        /// <param name="url">The destination URL.</param>
        /// <param name="method">The desired HTTP Request Method.</param>
        /// <param name="data">The data to include in the message body.</param>
        /// <param name="contentType">The Content-Type of the request. Default value is "application/json"</param>
        /// <param name="timeout">How long to wait for the request to complete. Default wait time is 3 seconds.</param>
        /// <returns>Returns the response text on success. Throws a WebException on failure.</returns>
        private string SendHttpWebRequest(string url, HttpMethod method, string data, string contentType = "application/json", int timeout = 3000)
        {
            try
            {
                // CREATE REQUEST
                using (HttpClient httpClient = new HttpClient())
                {
                    HttpRequestMessage msg = new HttpRequestMessage(method, new Uri(url));
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                    if (data != null)
                    {
                        msg.Content = new StringContent(data, Encoding.ASCII, contentType);

                    }
                    else if (!method.Equals(HttpMethod.Get))
                    {
                        msg.Content = new StringContent("", Encoding.ASCII, contentType);
                    }



                    // TODO: Does this work when creds are null?
                    // answer: no
                    //if (this.isAuthroized)
                    //{
                    //    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes("oy5ct3rjz7dob56syzaa65npbpumuvjm6d26fzdlupvighy2c4kq")));//":6cc3meg6fhnkbz22qocgetkndxrfjca76emcnanjodgiooir3xoq")));
                    //}

                    // GET RESPONSE
                    HttpResponseMessage resp = httpClient.SendAsync(msg).Result;
                    if (!resp.IsSuccessStatusCode)
                    {
                        throw new WebException("Error contacting serer. " + resp.ReasonPhrase);
                    }

                    return resp.Content.ReadAsStringAsync().Result;
                }
            }
            catch (System.Net.WebException e)
            {
                this.LogTextBox.Text += "\r\n" + e.Message;
                return null;
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


            if (this.LoadLocalCookie())
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

            if (Authorize())
            {
                this.StopButton.Enabled = true;
                log("Got user token. " + global::SubscriberFeed.Properties.Settings.Default.Code);
                if (!getAccessToken())
                {
                    log("Error getting user's token");
                    this.StartButton.Enabled = true;
                    return;
                }

                // get, initially, all subs
                log("Initializing subscribers");
                SubscriberPage page = new SubscriberPage();
                page.NextPageToken = null;
                do
                {
                    page = FetchSubscribers(25, page.NextPageToken);

                } while (page != null && page.NextPageToken != null);
                


                log("Started queries");
                bootstrapped = true;
                this.QueryTimer.Start();
            }
            else {
                log("Error getting user's token");
                this.StartButton.Enabled = true;
                return;
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
            SubscriberPage page = FetchSubscribers(25, this.fetchPageToken);
            if (page != null)
            {
                this.fetchPageToken = page.NextPageToken;
            }
        }
    }
}
