
namespace SubscriberFeed
{
    using System;

    public class IOManager
    {
        
        private static IOManager instance;

        private IOManager() { }

        public static IOManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new IOManager();
                }
                return instance;
            }
        }

        public bool LoadLocalCookie()
        {
            string path = Path.Combine(Path.GetTempPath(), global::SubscriberFeed.Properties.Settings.Default.LocalCookie);
            if (File.Exists(path))
            {
                try
                {
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
                }
                catch (IOException)
                {
                    // failed to load
                    return false;
                }
            }

            // if file doesn't exist, go through normal auth and load process
            return false;
        }

        public bool SaveLocalCookie()
        {
            try
            {
                LocalApplicationCookie cookie = new LocalApplicationCookie();
                cookie.Subscribers = new List<SubscriberSnippet>();
                foreach (SubscriberSnippet s in this.Subscribers.Values)
                {
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
    }
}
