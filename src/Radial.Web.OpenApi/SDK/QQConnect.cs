using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radial.Param;
using System.Collections.Specialized;
using System.Net;
using Radial.Net;

namespace Radial.Web.OpenApi.SDK
{
    /// <summary>
    /// QQ open api sdk
    /// </summary>
    public class QQConnect : BasicSDK
    {

        /// <summary>
        /// RequestApiUrl
        /// </summary>
        const string RequestApiUrl = "http://openapi.qzone.qq.com/oauth/qzoneoauth_request_token";
        /// <summary>
        /// AuthApiUrl
        /// </summary>
        const string AuthApiUrl = "http://openapi.qzone.qq.com/oauth/qzoneoauth_authorize";
        /// <summary>
        /// AccessTokenApiUrl
        /// </summary>
        const string AccessTokenApiUrl = "http://openapi.qzone.qq.com/oauth/qzoneoauth_access_token";

        /// <summary>
        /// Initializes a new instance of the <see cref="QQConnect"/> class.
        /// </summary>
        /// <param name="oauth">The OAuth object.</param>
        /// <param name="tokenPersistence">The ITokenPersistence instance.</param>
        public QQConnect(OAuth oauth, ITokenPersistence tokenPersistence)
            : base(oauth, tokenPersistence)
        {
            OAuth.OAuthVerifier = "oauth_vericode";
        }

        /// <summary>
        /// Gets the default QQConnect instance
        /// </summary>
        public static QQConnect Default
        {
            get
            {
                KeySecretPair pair = new KeySecretPair { Key = AppParam.GetValue("qqconnect.appid"), Secret = AppParam.GetValue("qqconnect.appkey") };
                return new QQConnect(new OAuth(pair), new HttpSessionTokenPersistence("qqconnect"));
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
            NameValueCollection additionalParams;
            return GetAuthorizationUrl(RequestApiUrl, AuthApiUrl, callbackUrl, out additionalParams);
        }

        /// <summary>
        /// Gets the authorization url.
        /// </summary>
        /// <param name="authUrl">The authorization api url.</param>
        /// <param name="requestKey">The request credential key.</param>
        /// <param name="callbackUrl">The callback url.</param>
        /// <returns></returns>
        protected override string GetAuthorizationUrlInternal(string authUrl, string requestKey, string callbackUrl)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(authUrl), "authUrl can not be empty or null");
            Checker.Parameter(!string.IsNullOrWhiteSpace(requestKey), "requestKey can not be empty or null");
            Checker.Parameter(!string.IsNullOrWhiteSpace(callbackUrl), "callbackUrl can not be empty or null");

            return string.Format("{0}?{1}={2}&{3}={4}&{5}={6}", authUrl, OAuth.OAuthTokenKey, requestKey, OAuth.OAuthCallbackKey, UrlEncode(callbackUrl), OAuth.OAuthConsumerKeyKey, this.OAuth.AppKeySecret.Key);
        }

        /// <summary>
        /// Gets the access token.
        /// </summary>
        /// <param name="vericode">The vericode.</param>
        /// <param name="openid">The open id.</param>
        /// <returns></returns>
        public KeySecretPair GetAccessToken(string vericode, out string openid)
        {
            NameValueCollection additionalParams;
            KeySecretPair pair = GetAccessTokenInternal(AccessTokenApiUrl, vericode, TokenPersistence.GetRequestToken(), out additionalParams);

            openid=additionalParams["openid"];

            return pair;
        }


        /// <summary>
        /// Get the api response use HTTP GET
        /// </summary>
        /// <param name="apiUrl">The api url.</param>
        /// <param name="args">The args.</param>
        /// <param name="needAuth">if set to <c>true</c> [need authorization].</param>
        /// <returns>
        /// The HttpResponseObj instance(never null).
        /// </returns>
        public override HttpResponseObj Get(string apiUrl, IDictionary<string, dynamic> args, bool needAuth)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(apiUrl), "apiUrl can not be empty or null");

            if (args != null)
            {
                foreach (KeyValuePair<string, dynamic> item in args)
                {
                    apiUrl = AppendQuery(apiUrl, item.Key, item.Value);
                }
            }

            apiUrl = AddOAuthParams(apiUrl, WebRequestMethods.Http.Get);


            return HttpWebHost.Get(apiUrl);
        }


        /// <summary>
        /// Get the api response use HTTP POST.
        /// </summary>
        /// <param name="apiUrl">The api url.</param>
        /// <param name="args">The args.</param>
        /// <returns>
        /// The HttpResponseObj instance(never null).
        /// </returns>
        public override HttpResponseObj Post(string apiUrl, IDictionary<string, dynamic> args)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(apiUrl), "requestUrl can not be empty or null");

            if (args != null)
            {
                foreach (KeyValuePair<string, dynamic> item in args)
                {
                    apiUrl = AppendQuery(apiUrl, item.Key, item.Value);
                }
            }

            apiUrl = AddOAuthParams(apiUrl, WebRequestMethods.Http.Post);

            return HttpWebHost.Post(apiUrl);

        }

        /// <summary>
        /// Get the api response use HTTP POST.
        /// </summary>
        /// <param name="apiUrl">The api url.</param>
        /// <param name="args">The args.</param>
        /// <param name="fileData">The post file data.</param>
        /// <returns>
        /// The HttpResponseObj instance(never null).
        /// </returns>
        public override HttpResponseObj Post(string apiUrl, IDictionary<string, dynamic> args, FileFormData fileData)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(apiUrl), "apiUrl can not be empty or null");
            Checker.Parameter(fileData != null, "fileData can not be null");

            if (args != null)
            {
                foreach (KeyValuePair<string, dynamic> item in args)
                {
                    apiUrl = AppendQuery(apiUrl, item.Key, item.Value);
                }
            }

            apiUrl = AddOAuthParams(apiUrl, WebRequestMethods.Http.Post);


            NameValueCollection queryCollection = HttpKits.ResolveParameters(apiUrl.Substring(apiUrl.IndexOf("?") + 1));

            List<IMultipartFormData> dataList = new List<IMultipartFormData>();

            foreach (string key in queryCollection.Keys)
            {
                dataList.Add(new PlainTextFormData(key, queryCollection[key]));
            }

            dataList.Add(fileData);

            return HttpWebHost.Post(apiUrl.Substring(0, apiUrl.IndexOf("?")), dataList.ToArray());
        }


        /// <summary>
        /// Adds the O auth params.
        /// </summary>
        /// <param name="apiUrl">The API URL.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <returns></returns>
        private string AddOAuthParams(string apiUrl, string httpMethod)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(apiUrl), "apiUrl can not be empty or null");
            Checker.Parameter(httpMethod != null, "httpMethod can not be null");

            Uri reqUri = new Uri(apiUrl);

            KeySecretPair accessToken = TokenPersistence.GetAccessToken();
            string normalizedUrl = string.Empty;
            string normalizedRequestParameters = string.Empty;

            string sign = OAuth.GenerateSignature(reqUri, accessToken.Key, accessToken.Secret, httpMethod, out normalizedUrl, out normalizedRequestParameters);

            normalizedRequestParameters += string.Format("&{0}={1}", OAuth.OAuthSignatureKey, UrlEncode(sign));


            return string.Format("{0}?{1}", apiUrl.Substring(0, apiUrl.IndexOf("?")), normalizedRequestParameters);
        }
    }
}
