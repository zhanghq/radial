using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radial.Web.OpenApi
{
    /// <summary>
    /// Persists token in HttpSessionState
    /// </summary>
    public sealed class HttpSessionTokenPersistence : ITokenPersistence
    {
        /// <summary>
        /// The request token session name.
        /// </summary>
        public readonly string RequestTokenSessionName = "openApi-requestToken";
        /// <summary>
        /// The access token session name.
        /// </summary>
        public readonly string AccessTokenSessionName = "openApi-accessToken";


        /// <summary>
        /// Initializes a new instance of the <see cref="HttpSessionTokenPersistence"/> class.
        /// </summary>
        public HttpSessionTokenPersistence()
            : this(string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpSessionTokenPersistence"/> class.
        /// </summary>
        /// <param name="sessionPrefix">The session prefix.</param>
        public HttpSessionTokenPersistence(string sessionPrefix)
        {
            if (!string.IsNullOrWhiteSpace(sessionPrefix))
            {
                RequestTokenSessionName = sessionPrefix + "-" + RequestTokenSessionName;
                AccessTokenSessionName = sessionPrefix + "-" + AccessTokenSessionName;
            }
        }


        #region ITokenPersistence Members

        /// <summary>
        /// Sets the request token.
        /// </summary>
        /// <param name="requestToken">The request token.</param>
        public void SetRequestToken(KeySecretPair requestToken)
        {
            HttpKits.SetSession<KeySecretPair>(RequestTokenSessionName, requestToken);
        }

        /// <summary>
        /// Gets the request token.
        /// </summary>
        /// <returns>
        /// The request token.
        /// </returns>
        public KeySecretPair GetRequestToken()
        {
            KeySecretPair c = HttpKits.GetSession<KeySecretPair>(RequestTokenSessionName);
            return c;
        }

        /// <summary>
        /// Clear the request token.
        /// </summary>
        public void ClearRequestToken()
        {
            HttpKits.RemoveSession(RequestTokenSessionName);
        }

        /// <summary>
        /// Sets the access token.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        public void SetAccessToken(KeySecretPair accessToken)
        {
            HttpKits.SetSession<KeySecretPair>(AccessTokenSessionName, accessToken);
        }

        /// <summary>
        /// Gets the access token.
        /// </summary>
        /// <returns>
        /// The access token.
        /// </returns>
        public KeySecretPair GetAccessToken()
        {
            return HttpKits.GetSession<KeySecretPair>(AccessTokenSessionName);
        }

        /// <summary>
        /// Clear the access token.
        /// </summary>
        public void ClearAccessToken()
        {
            HttpKits.RemoveSession(AccessTokenSessionName);
        }

        #endregion
    }
}
