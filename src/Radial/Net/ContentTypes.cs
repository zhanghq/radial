using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mime;

namespace Radial.Net
{
    /// <summary>
    /// Content types.
    /// </summary>
    public static class ContentTypes
    {
        /// <summary>
        /// Plain Text Content Type
        /// </summary>
        public const string PlainText = "text/plain; charset=utf-8";

        /// <summary>
        /// Html Content Type
        /// </summary>
        public const string Html = "text/html; charset=utf-8";

        /// <summary>
        /// Xml Content Type
        /// </summary>
        public const string Xml = "text/xml; charset=utf-8";

        /// <summary>
        /// Json Content Type
        /// </summary>
        public const string Json = "application/json; charset=utf-8";

        /// <summary>
        /// Excel Content Type
        /// </summary>
        public const string Excel = "application/vnd.ms-excel; charset=utf-8";

        /// <summary>
        /// Binary Stream Content Type
        /// </summary>
        public const string BinaryStream = "application/octet-stream; charset=utf-8";
    }
}
