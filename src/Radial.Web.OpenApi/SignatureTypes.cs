using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radial.Web.OpenApi
{
    /// <summary>
    /// Provides a predefined set of algorithms that are supported officially by the protocol
    /// </summary>
    public enum SignatureTypes
    {
        /// <summary>
        /// HMACSHA1
        /// </summary>
        HMACSHA1,
        /// <summary>
        /// PLAINTEXT
        /// </summary>
        PLAINTEXT,
        /// <summary>
        /// RSASHA1
        /// </summary>
        RSASHA1
    }
}
