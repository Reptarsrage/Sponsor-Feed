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
    /// A Subscriber is used to communicate with YouTube v3 API.
    /// <para>See: https://developers.google.com/youtube/v3/docs/subscriptions </para>
    /// </summary>
    [Serializable]
    public class Subscriber
    {
        [JsonProperty("kind")]
        public string Kind { get; set; }

        [JsonProperty("etag")]
        public string Etag { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("subscriberSnippet")]
        public SubscriberSnippet SubscriberSnippet { get; set; }
    }
}
