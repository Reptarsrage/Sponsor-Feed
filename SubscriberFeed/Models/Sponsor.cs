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

    /// <summary>
    /// Used for JSON parsing.
    /// A Sponsor is used to communicate with YouTube v3 API.
    /// <para>See: https://developers.google.com/youtube/v3/live/docs/sponsors/list </para>
    /// </summary>
    [Serializable]
    public class Sponsor
    {
        [JsonProperty("kind")]
        public string Kind { get; set; }

        [JsonProperty("etag")]
        public string Etag { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("subscriberSnippet")]
        public SponsorSnippet SponsorSnippet { get; set; }

        [JsonProperty("sponsorSince")]
        public DateTime? SponsorSinceDate { get; set; }
    }
}
