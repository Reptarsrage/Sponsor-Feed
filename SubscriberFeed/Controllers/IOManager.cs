
namespace SubscriberFeed
{
    using Controllers;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml.Serialization;
    public class IOManager : ManagerWithLog
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

        public bool LoadLocalCookie(ref Dictionary<string, SubscriberSnippet> subscribers)
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
                        if (subscribers.ContainsKey(s.ChannelId)) subscribers.Add(s.ChannelId, s);
                    }

                    log("Loaded local file, renewing credentials.");

                    if (!APICommunicationManager.Instance.RenewCredentials())
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

        public bool SaveLocalCookie(ref Dictionary<string, SubscriberSnippet> subscribers)
        {
            try
            {
                LocalApplicationCookie cookie = new LocalApplicationCookie();
                cookie.Subscribers = new List<SubscriberSnippet>();
                foreach (SubscriberSnippet s in subscribers.Values)
                {
                    cookie.Subscribers.Add(s);
                }
                cookie.Count = subscribers.Values.Count;
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
