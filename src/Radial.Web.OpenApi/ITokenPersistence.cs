using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radial.Web.OpenApi
{
    /// <summary>
    /// Token persistence interface.
    /// </summary>
    public interface ITokenPersistence
    {
        /// <summary>
        /// Sets the request token.
        /// </summary>
        /// <param name="requestToken">The request token.</param>
        void SetRequestToken(KeySecretPair requestToken);

        /// <summary>
        /// Gets the request token.
        /// </summary>
        /// <returns>The request token.</returns>
        KeySecretPair GetRequestToken();

        /// <summary>
        /// Clear the request token.
        /// </summary>
        void ClearRequestToken();

        /// <summary>
        /// Sets the access token.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        void SetAccessToken(KeySecretPair accessToken);

        /// <summary>
        /// Gets the access token.
        /// </summary>
        /// <returns>The access token.</returns>
        KeySecretPair GetAccessToken();

        /// <summary>
        /// Clear the access token.
        /// </summary>
        void ClearAccessToken();
    }
}
