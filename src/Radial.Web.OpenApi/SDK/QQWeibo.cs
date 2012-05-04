using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Net;
using Radial.Param;
using Radial.Net;

namespace Radial.Web.OpenApi.SDK
{
    /// <summary>
    /// QQ weibo sdk.
    /// </summary>
    public class QQWeibo : BasicSDK
    {
        /// <summary>
        /// RequestApiUrl
        /// </summary>
        const string RequestApiUrl = "https://open.t.qq.com/cgi-bin/request_token";
        /// <summary>
        /// AuthApiUrl
        /// </summary>
        const string AuthApiUrl = "https://open.t.qq.com/cgi-bin/authorize";
        /// <summary>
        /// AccessTokenApiUrl
        /// </summary>
        const string AccessTokenApiUrl = "https://open.t.qq.com/cgi-bin/access_token";


        /// <summary>
        /// Initializes a new instance of the <see cref="QQWeibo"/> class.
        /// </summary>
        /// <param name="oauth">The OAuth object.</param>
        /// <param name="tokenPersistence">The ITokenPersistence instance.</param>
        public QQWeibo(OAuth oauth, ITokenPersistence tokenPersistence) : base(oauth, tokenPersistence) { }

        /// <summary>
        /// Gets the default QQWeibo instance.
        /// </summary>
        public static QQWeibo Default
        {
            get
            {
                KeySecretPair pair = new KeySecretPair { Key = AppParam.GetValue("qqweibo.appkey"), Secret = AppParam.GetValue("qqweibo.appsecret") };
                return new QQWeibo(new OAuth(pair), new HttpSessionTokenPersistence("qqweibo"));
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

            return GetAuthorizationUrl(string.Format(RequestApiUrl + "?{0}={1}", OAuth.OAuthCallbackKey, UrlEncode(callbackUrl)), AuthApiUrl, callbackUrl, out additionalParams);
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

            return string.Format("{0}?{1}={2}&{3}={4}", authUrl, OAuth.OAuthTokenKey, TokenPersistence.GetRequestToken().Key, OAuth.OAuthCallbackKey, UrlEncode(callbackUrl));
        }

        /// <summary>
        /// Gets the access token.
        /// </summary>
        /// <param name="verifier">The verifier string.</param>
        /// <param name="userName">The user name.</param>
        /// <returns>
        /// The access token.
        /// </returns>
        public KeySecretPair GetAccessToken(string verifier, out string userName)
        {
            NameValueCollection additionalParams;
            KeySecretPair ks = GetAccessToken(verifier, AccessTokenApiUrl, out additionalParams);
            userName = additionalParams["name"];
            return ks;
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

            if (needAuth)
                apiUrl = AddOAuthParams(apiUrl, WebRequestMethods.Http.Get);

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(apiUrl);

            return HttpWebHost.Get(request);
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
    }
}
