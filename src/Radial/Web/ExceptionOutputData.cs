﻿using System.Text;
using Newtonsoft.Json;

namespace Radial.Web
{
    /// <summary>
    /// A struct represents exception data that will display on screen.
    /// </summary>
    public struct ExceptionOutputData
    {
        /// <summary>
        /// Error code.
        /// </summary>
        [JsonProperty("error", Order = 0)]
        public int ErrorCode;
        /// <summary>
        /// Request url.
        /// </summary>
        [JsonProperty("request", Order = 1)]
        public string RequestUrl;
        /// <summary>
        /// Error message.
        /// </summary>
        [JsonProperty("message", Order = 2)]
        public string ErrorMessage;

        /// <summary>
        /// Toes the XML.
        /// </summary>
        /// <returns>XML string.</returns>
        public string ToXml()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            sb.Append("<exception>");
            sb.AppendFormat("<error>{0}</error>", ErrorCode);
            sb.AppendFormat("<request>{0}</request>", RequestUrl);
            sb.AppendFormat("<message>{0}</message>", ErrorMessage);
            sb.Append("</exception>");
            return sb.ToString();
        }

        /// <summary>
        /// To the Json.
        /// </summary>
        /// <returns>Json string.</returns>
        public string ToJson()
        {
            return Serialization.JsonSerializer.Serialize(this);
        }
    }
}
