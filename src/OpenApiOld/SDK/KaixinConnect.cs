using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radial.Param;
using System.Collections.Specialized;

namespace Radial.Web.OpenApi.SDK
{
    /// <summary>
    /// Kaixin open api sdk.
    /// </summary>
    public class KaixinConnect : BasicSDK
    {
                /// <summary>
        /// RequestApiUrl
        /// </summary>
        const string RequestApiUrl = "http://api.kaixin001.com/oauth/request_token";
        /// <summary>
        /// AuthApiUrl
        /// </summary>
        const string AuthApiUrl = "http://api.kaixin001.com/oauth/authorize";
        /// <summary>
        /// AccessTokenApiUrl
        /// </summary>
        const string AccessTokenApiUrl = "http://api.kaixin001.com/oauth/access_token";

        /// <summary>
        /// Initializes a new instance of the <see cref="QQConnect"/> class.
        /// </summary>
        /// <param name="oauth">The OAuth object.</param>
        /// <param name="tokenPersistence">The ITokenPersistence instance.</param>
        public KaixinConnect(OAuth oauth, ITokenPersistence tokenPersistence) : base(oauth, tokenPersistence) { }

        /// <summary>
        /// Gets the default KaixinConnect instance
        /// </summary>
        public static KaixinConnect Default
        {
            get
            {
                KeySecretPair pair = new KeySecretPair { Key = AppParam.GetValue("kaixinconnect.appkey"), Secret = AppParam.GetValue("kaixinconnect.secretkey") };
                return new KaixinConnect(new OAuth(pair), new HttpSessionTokenPersistence("kaixinconnect"));
            }
        }


        /// <summary>
        /// Gets the authorization url.
        /// </summary>
        /// <param name="callbackUrl">The callback url.</param>
        /// <returns>
        /// The authorization url.
        /// </returns>
        public string GetAuthorizationUrl(string callbackUrl)
        {
            return GetAuthorizationUrl(callbackUrl,string.Empty);
        }

        /// <summary>
        /// Gets the authorization url.
        /// </summary>
        /// <param name="callbackUrl">The callback url.</param>
        /// <param name="scope">The scope.</param>
        /// <returns>
        /// The authorization url.
        /// </returns>
        public string GetAuthorizationUrl(string callbackUrl, string scope)
        {
            NameValueCollection additionalParams;
            if (string.IsNullOrWhiteSpace(scope))
                return GetAuthorizationUrl(string.Format(RequestApiUrl + "?{0}={1}", OAuth.OAuthCallbackKey, UrlEncode(callbackUrl)), AuthApiUrl, callbackUrl, out additionalParams);
            else
                return GetAuthorizationUrl(string.Format(RequestApiUrl + "?{0}={1}&scope={2}", OAuth.OAuthCallbackKey, UrlEncode(callbackUrl), UrlEncode(scope)), AuthApiUrl, callbackUrl, out additionalParams);
        }


        /// <summary>
        /// Gets the access token.
        /// </summary>
        /// <param name="verifier">The verifier string.</param>
        /// <returns>
        /// The access token.
        /// </returns>
        public KeySecretPair GetAccessToken(string verifier)
        {
            NameValueCollection additionalParams;
            return GetAccessToken(verifier, AccessTokenApiUrl, out additionalParams);
        }
    }
}
