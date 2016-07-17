/*
 * Justin Robb
 * 4/29/16
 * Subscriber Feed
 *  
 */

namespace SubscriberFeed
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    /// <summary>
    /// Used in XML Serialization to save/load subscribers
    /// </summary>
    [Serializable()]
    [XmlRoot("LocalApplicationCookie")]
    public class LocalApplicationCookie
    {

        [XmlArray("Subscribers")]
        public List<SubscriberSnippet> Subscribers { get; set; }

        [XmlArray("Sponsors")]
        public List<SponsorSnippet> Sponsors { get; set; }

        [XmlElement("Code")]
        public string Code { get; set; }

        [XmlElement("RefreshToken")]
        public string RefreshToken { get; set; }

        [XmlElement("Token")]
        public string Token { get; set; }

        [XmlElement("Count")]
        public int Count { get; set; }
    }
}
