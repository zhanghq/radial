using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radial.Web.OpenApi
{
    /// <summary>
    /// The Key/Secret pair struct.
    /// </summary>
    public struct KeySecretPair
    {
        /// <summary>
        /// Gets ort sets the key(not used in OAuth 2.0).
        /// </summary>
        public string Key;
        /// <summary>
        /// Gets ort sets the secret.
        /// </summary>
        public string Secret;
    }
}
