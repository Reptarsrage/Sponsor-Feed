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
    /// A YoutubeError is used to communicate with YouTube v3 API.
    /// <para>See: https://developers.google.com/youtube/v3/live/docs/sponsors/list </para>
    /// </summary>
    [Serializable]
    public class YoutubeError
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }


        [JsonProperty("domain")]
        public string Domain { get; set; }

        [JsonProperty("reason")]
        public string Reason { get; set; }

        [JsonProperty("errors")]
        public ErrorResponse[] InternalErrors { get; set; }
    }
}
