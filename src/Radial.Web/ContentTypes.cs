using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mime;

namespace Radial.Web
{
    /// <summary>
    /// Content types.
    /// </summary>
    public static class ContentTypes
    {
        /// <summary>
        /// Plain Text Content Type
        /// </summary>
        public const string PlainText = "text/plain; charset=UTF-8";

        /// <summary>
        /// Html Content Type
        /// </summary>
        public const string Html = "text/html; charset=UTF-8";

        /// <summary>
        /// Xml Content Type
        /// </summary>
        public const string Xml = "text/xml; charset=UTF-8";

        /// <summary>
        /// Json Content Type
        /// </summary>
        public const string Json = "application/json; charset=UTF-8";

        /// <summary>
        /// Excel Content Type
        /// </summary>
        public const string Excel = "application/vnd.ms-excel; charset=UTF-8";
    }
}
