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
    /// A SponsorSnippet is used to communicate with YouTube v3 API.
    /// <para>See: https://developers.google.com/youtube/v3/live/docs/sponsors/list </para>
    /// </summary>
    [Serializable]
    [XmlRoot("SponsorSnippet")]
    public class SponsorSnippet
    {
        [XmlElement("channelId")]
        [JsonProperty("channelId")]
        public string ChannelId { get; set; }


        [XmlElement("sponsorDetails")]
        [JsonProperty("sponsorDetails")]
        public SponsorDetails SponsorDetails { get; set; }
    }
}
