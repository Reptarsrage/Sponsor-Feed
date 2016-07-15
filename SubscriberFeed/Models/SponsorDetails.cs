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
    [XmlRoot("SponsorDetails")]
    public class SponsorDetails
    {
        [XmlElement("channelId")]
        [JsonProperty("channelId")]
        public string ChannelId { get; set; }


        [XmlElement("channelUrl")]
        [JsonProperty("channelUrl")]
        public string ChannelUrl { get; set; }

        [XmlElement("displayName")]
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [XmlElement("profileImageUrl")]
        [JsonProperty("profileImageUrl")]
        public string ProfileImageUrl { get; set; }

    }
}
