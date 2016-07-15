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
    /// A PageInfo is used to communicate with YouTube v3 API.
    /// <para>See: https://developers.google.com/youtube/v3/docs/subscriptions </para>
    /// </summary>
    [Serializable]
    public class PageInfo
    {
        [JsonProperty("resultsPerPage")]
        public int ResultsPerPage { get; set; }

        [JsonProperty("totalResults")]
        public int TotalResults { get; set; }
    }
}
