using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Xml.Serialization;

namespace Radial.Web.Mvc.Filters
{
    /// <summary>
    /// A struct represents exception data that will display on screen.
    /// </summary>
    struct ExceptionOutputData
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
            sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            sb.Append("<exception>");
            sb.AppendFormat("<error>{0}</error>", ErrorCode);
            sb.AppendFormat("<request>{0}</request>", HttpKits.CurrentContext.Server.HtmlEncode(RequestUrl));
            sb.AppendFormat("<message>{0}</message>", HttpKits.CurrentContext.Server.HtmlEncode(ErrorMessage));
            sb.Append("</exception>");
            return sb.ToString();
        }
    }
}
