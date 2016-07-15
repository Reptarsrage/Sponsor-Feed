/*
 * Justin Robb
 * 4/29/16
 * Subscriber Feed
 *  
 */

namespace SubscriberFeed
{
    using Newtonsoft.Json;
    using System;
    using System.Xml.Serialization;

    /// <summary>
    /// Used for JSON parsing and XML Serialization.
    /// A SubscriberSnippet is used to communicate with YouTube v3 API.
    /// <para>See: https://developers.google.com/youtube/v3/docs/subscriptions </para>
    /// </summary>
    [Serializable]
    [XmlRoot("SubscriberSnippet")]
    public class SubscriberSnippet
    {
        [XmlElement("title")]
        [JsonProperty("title")]
        public string Title { get; set; }


        [XmlElement("description")]
        [JsonProperty("description")]
        public string Description { get; set; }

        [XmlElement("channelId")]
        [JsonProperty("channelId")]
        public string ChannelId { get; set; }

    }
}
