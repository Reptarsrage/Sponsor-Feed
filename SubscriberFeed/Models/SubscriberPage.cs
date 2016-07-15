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
    /// A SubscriberPage is used to communicate with YouTube v3 API.
    /// <para>See: https://developers.google.com/youtube/v3/docs/subscriptions </para>
    /// </summary>
    [Serializable]
    public class SubscriberPage
    {
        [JsonProperty("kind")]
        public string Kind { get; set; }

        [JsonProperty("etag")]
        public string Etag { get; set; }

        [JsonProperty("nextPageToken")]
        public string NextPageToken { get; set; }

        [JsonProperty("prevPageToken")]
        public string PrevPageToken { get; set; }

        [JsonProperty("pageInfo")]
        public PageInfo PageInfo { get; set; }

        [JsonProperty("items")]
        public Subscriber[] Items { get; set; }


    }
}
