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
    /// A ErrorResponse is used to communicate with YouTube v3 API.
    /// <para>See: https://developers.google.com/youtube/v3/live/docs/sponsors/list </para>
    /// </summary>
    [Serializable]
    public class ErrorResponse
    {
        [JsonProperty("error")]
        public YoutubeError YoutubeError { get; set; }
    }
}
