using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Collections.Specialized;
using System.Net;
using System.Security.Cryptography;
using Radial.Net;

namespace Radial.Web.OpenApi
{
    /// <summary>
    /// Basic sdk functions
    /// </summary>
    public abstract class BasicSDK
    {
        OAuth _oauth;
        ITokenPersistence _tokenPersistence;

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicSDK"/> class.
        /// </summary>
        /// <param name="oauth">The OAuth object.</param>
        /// <param name="tokenPersistence">The ITokenPersistence instance.</param>
        public BasicSDK(OAuth oauth, ITokenPersistence tokenPersistence)
        {
            Checker.Parameter(oauth != null, "OAuth object can not be null");
            Checker.Parameter(tokenPersistence != null, "ITokenPersistence instance can not be null");

            _oauth = oauth;
            _tokenPersistence = tokenPersistence;
        }

        /// <summary>
        /// Gets the OAuth object.
        /// </summary>
        public OAuth OAuth
        {
            get
            {
                return _oauth;
            }
        }

        /// <summary>
        /// Gets the token persistence instance.
        /// </summary>
        public ITokenPersistence TokenPersistence
        {
            get
            {
                return _tokenPersistence;
            }
        }

        /// <summary>
        /// URLs the encode.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public virtual string UrlEncode(string input)
        {
            return HttpKits.EscapeUrl(input);
        }

        /// <summary>
        /// Append parameter to the end of url query string
        /// </summary>
        /// <param name="url">The url.</param>
        /// <param name="pName">The parameter name.</param>
        /// <param name="pValue">The parameter value.</param>
        /// <returns>
        /// New url with query string
        /// </returns>
        protected virtual string AppendQuery(string url, string pName, dynamic pValue)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(url), "url can not be empty or null");
            Checker.Parameter(!string.IsNullOrWhiteSpace(pName), "parameter name can not be empty or null");
            Checker.Parameter(pValue != null, "parameter value can not be null");

            string query = string.Format("{0}={1}", pName, UrlEncode(pValue.ToString()));
            if (url.Contains('?'))
                return string.Join("&", url, query);
            return string.Join("?", url, query);
        }

        /// <summary>
        /// Gets the request token.
        /// </summary>
        /// <param name="requestUrl">The request api url.</param>
        /// <param name="additionalParams">The additional parameters callback from the api service.</param>
        /// <returns>
        /// The request token.
        /// </returns>
        protected virtual KeySecretPair GetRequestTokenInternal(string requestUrl, out NameValueCollection additionalParams)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(requestUrl), "url can not be empty or null");

            string normalizedUrl;
            string normalizedRequestParameters;
            string sign = OAuth.GenerateSignature(new Uri(requestUrl), WebRequestMethods.Http.Get, out normalizedUrl, out normalizedRequestParameters);

            string actualRequestUrl = string.Format("{0}?{1}&{2}={3}", normalizedUrl, normalizedRequestParameters, OAuth.OAuthSignatureKey, UrlEncode(sign));

            HttpResponseObj respObj = HttpWebHost.Get(actualRequestUrl);

            Checker.Requires(respObj.Code == System.Net.HttpStatusCode.OK, "request error, code: {0} text: {1}", respObj.Code, respObj.Text);

            NameValueCollection paramCollection = HttpKits.ResolveParameters(respObj.Text);

            Checker.Requires(paramCollection.Count >= 2, "response not match the protocol, text: {0}", respObj.Text);

            KeySecretPair c = new KeySecretPair { Key = paramCollection[OAuth.OAuthTokenKey], Secret = paramCollection[OAuth.OAuthTokenSecretKey] };

            paramCollection.Remove(OAuth.OAuthTokenKey);
            paramCollection.Remove(OAuth.OAuthTokenSecretKey);

            additionalParams = paramCollection;

            return c;
        }

        /// <summary>
        /// Gets the authorization url.
        /// </summary>
        /// <param name="authUrl">The authorization api url.</param>
        /// <param name="requestKey">The request credential key.</param>
        /// <param name="callbackUrl">The callback url.</param>
        /// <returns></returns>
        protected virtual string GetAuthorizationUrlInternal(string authUrl, string requestKey, string callbackUrl)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(authUrl), "authUrl can not be empty or null");
            Checker.Parameter(!string.IsNullOrWhiteSpace(requestKey), "requestKey can not be empty or null");
            Checker.Parameter(!string.IsNullOrWhiteSpace(callbackUrl), "callbackUrl can not be empty or null");

            return string.Format("{0}?{1}={2}&{3}={4}", authUrl, OAuth.OAuthTokenKey, requestKey, OAuth.OAuthCallbackKey, UrlEncode(callbackUrl));

        }

        /// <summary>
        /// Gets the access token.
        /// </summary>
        /// <param name="authUrl">The authorization api url.</param>
        /// <param name="verifier">The verifier.</param>
        /// <param name="requestToken">The request token.</param>
        /// <param name="additionalParams">The additional parameters callback from the api service.</param>
        /// <returns>
        /// The access token.
        /// </returns>
        protected virtual KeySecretPair GetAccessTokenInternal(string authUrl, string verifier, KeySecretPair requestToken, out NameValueCollection additionalParams)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(authUrl), "authUrl can not be empty or null");
            Checker.Parameter(!string.IsNullOrWhiteSpace(verifier), "verifier can not be empty or null");

            string normalizedUrl;
            string normalizedRequestParameters;
            string sign = OAuth.GenerateSignature(new Uri(authUrl), requestToken.Key, requestToken.Secret, verifier, WebRequestMethods.Http.Get, SignatureTypes.HMACSHA1, out normalizedUrl, out normalizedRequestParameters);

            string requestUrl = string.Format("{0}?{1}&{2}={3}", normalizedUrl, normalizedRequestParameters, OAuth.OAuthSignatureKey, UrlEncode(sign));

            HttpResponseObj respObj = HttpWebHost.Get(requestUrl);

            Checker.Requires(respObj.Code == System.Net.HttpStatusCode.OK, "request error, code: {0} text: {1}", respObj.Code, respObj.Text);

            NameValueCollection paramCollection = HttpKits.ResolveParameters(respObj.Text);

            Checker.Requires(!string.IsNullOrWhiteSpace(paramCollection[OAuth.OAuthTokenKey]) && !string.IsNullOrWhiteSpace(paramCollection[OAuth.OAuthTokenSecretKey]), "can not retrieve auth token key and secret from server response: {0}", respObj.Text);

            KeySecretPair c = new KeySecretPair { Key = paramCollection[OAuth.OAuthTokenKey], Secret = paramCollection[OAuth.OAuthTokenSecretKey] };

            paramCollection.Remove(OAuth.OAuthTokenKey);
            paramCollection.Remove(OAuth.OAuthTokenSecretKey);

            additionalParams = paramCollection;

            return c;
        }

        /// <summary>
        /// Gets the authorization url.
        /// </summary>
        /// <param name="requestApiUrl">The request api url</param>
        /// <param name="authUrl">The authorization api url</param>
        /// <param name="callbackUrl">The callback url.</param>
        /// <returns>
        /// The authorization url.
        /// </returns>
        public string GetAuthorizationUrl(string requestApiUrl, string authUrl, string callbackUrl)
        {
            NameValueCollection additionalParams;
            return GetAuthorizationUrl(requestApiUrl, authUrl, callbackUrl, out additionalParams);

        }

        /// <summary>
        /// Gets the authorization url.
        /// </summary>
        /// <param name="requestApiUrl">The request api url</param>
        /// <param name="authApiUrl">The authorization api url</param>
        /// <param name="callbackUrl">The callback url.</param>
        /// <param name="additionalParams">The additional parameters callback from the api service.</param>
        /// <returns>
        /// The authorization url.
        /// </returns>
        public string GetAuthorizationUrl(string requestApiUrl, string authApiUrl, string callbackUrl, out NameValueCollection additionalParams)
        {
            KeySecretPair requestToken = GetRequestTokenInternal(requestApiUrl, out additionalParams);
            TokenPersistence.SetRequestToken(requestToken);
            return GetAuthorizationUrlInternal(authApiUrl, requestToken.Key, callbackUrl);

        }

        /// <summary>
        /// Gets the access token.
        /// </summary>
        /// <param name="verifier">The verifier string.</param>
        /// <param name="accessTokenApiUrl">The access token api url.</param>
        /// <param name="additionalParams">The additional parameters callback from the api service.</param>
        /// <returns>The access token.</returns>
        public KeySecretPair GetAccessToken(string verifier, string accessTokenApiUrl, out NameValueCollection additionalParams)
        {
            additionalParams = new NameValueCollection();

            KeySecretPair token = GetAccessTokenInternal(accessTokenApiUrl, verifier, TokenPersistence.GetRequestToken(), out additionalParams);

            return token;
        }


        /// <summary>
        /// Sets the access token using ITokenPersistence instance.
        /// </summary>
        /// <param name="token">The access token.</param>
        public virtual void SetAccessToken(KeySecretPair token)
        {
            TokenPersistence.SetAccessToken(token);
            TokenPersistence.ClearRequestToken();
        }

        /// <summary>
        /// Clear the access token.
        /// </summary>
        public virtual void ClearAccessToken()
        {
            TokenPersistence.ClearAccessToken();
        }

        /// <summary>
        /// Creates the authorization request header.
        /// </summary>
        /// <param name="requestUrl">The request url.</param>
        /// <param name="httpMethod">The http method.</param>
        /// <returns></returns>
        protected virtual NameValueCollection CreateAuthorizationRequestHeader(string requestUrl, string httpMethod)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(requestUrl), "requestUrl can not be empty or null");
            Checker.Parameter(httpMethod != null, "httpMethod can not be null");

            Uri reqUri = new Uri(requestUrl);

            KeySecretPair accessToken = TokenPersistence.GetAccessToken();
            string normalizedUrl = string.Empty;
            string normalizedRequestParameters = string.Empty;

            string sign = OAuth.GenerateSignature(reqUri, accessToken.Key, accessToken.Secret, httpMethod, out normalizedUrl, out normalizedRequestParameters);

            normalizedRequestParameters += string.Format("&{0}={1}", OAuth.OAuthSignatureKey, UrlEncode(sign));

            if (!string.IsNullOrWhiteSpace(reqUri.Query))
            {
                foreach (string p in reqUri.Query.Trim('?').Split('&'))
                    normalizedRequestParameters = normalizedRequestParameters.Replace("&" + p, string.Empty).Replace(p, string.Empty);
            }

            normalizedRequestParameters = normalizedRequestParameters.Replace("=", "=\"").Replace("&", "\"&") + "\"";

            NameValueCollection collection = new NameValueCollection();

            collection.Add("Authorization", "OAuth " + string.Join(",", normalizedRequestParameters.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries)));

            return collection;
        }

        /// <summary>
        /// Get the api response use HTTP GET.
        /// </summary>
        /// <param name="apiUrl">The api url.</param>
        /// <returns>
        /// The HttpResponseObj instance(never null).
        /// </returns>
        public HttpResponseObj Get(string apiUrl)
        {
            return Get(apiUrl, true);
        }

        /// <summary>
        /// Get the api response use HTTP GET.
        /// </summary>
        /// <param name="apiUrl">The api url.</param>
        /// <param name="args">The args.</param>
        /// <returns>
        /// The HttpResponseObj instance(never null).
        /// </returns>
        public HttpResponseObj Get(string apiUrl, IDictionary<string, dynamic> args)
        {
            return Get(apiUrl, args, true);
        }

        /// <summary>
        /// Get the api response use HTTP GET
        /// </summary>
        /// <param name="apiUrl">The api url.</param>
        /// <param name="needAuth">if set to <c>true</c> [need authorization].</param>
        /// <returns>
        /// The HttpResponseObj instance(never null).
        /// </returns>
        public HttpResponseObj Get(string apiUrl, bool needAuth)
        {
            return Get(apiUrl, null, needAuth);
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
        public virtual HttpResponseObj Get(string apiUrl, IDictionary<string, dynamic> args, bool needAuth)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(apiUrl), "apiUrl can not be empty or null");

            if (args != null)
            {
                foreach (KeyValuePair<string, dynamic> item in args)
                {
                    apiUrl = AppendQuery(apiUrl, item.Key, item.Value);
                }
            }

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(apiUrl);

            if (needAuth)
                request.Headers.Add(CreateAuthorizationRequestHeader(apiUrl, WebRequestMethods.Http.Get));

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
        public virtual HttpResponseObj Post(string apiUrl, IDictionary<string, dynamic> args)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(apiUrl), "apiUrl can not be empty or null");

            if (args != null)
            {
                foreach (KeyValuePair<string, dynamic> item in args)
                {
                    apiUrl = AppendQuery(apiUrl, item.Key, item.Value);
                }
            }


            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(apiUrl.Substring(0, apiUrl.IndexOf("?")));
            request.Headers.Add(CreateAuthorizationRequestHeader(apiUrl, WebRequestMethods.Http.Post));

            return HttpWebHost.Post(request, apiUrl.Substring(apiUrl.IndexOf("?") + 1));

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
        public virtual HttpResponseObj Post(string apiUrl, IDictionary<string, dynamic> args, FileFormData fileData)
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

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(apiUrl.Substring(0, apiUrl.IndexOf("?")));

            request.Headers.Add(CreateAuthorizationRequestHeader(apiUrl, WebRequestMethods.Http.Post));

            NameValueCollection queryCollection = HttpKits.ResolveParameters(apiUrl.Substring(apiUrl.IndexOf("?") + 1));

            List<IMultipartFormData> dataList = new List<IMultipartFormData>();

            foreach (string key in queryCollection.Keys)
            {
                dataList.Add(new PlainTextFormData(key, queryCollection[key]));
            }

            dataList.Add(fileData);

            return HttpWebHost.Post(request, dataList.ToArray());
        }

    }
}
