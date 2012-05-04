using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Collections.Specialized;
using Radial.Net;

namespace Radial.Web.OpenApi
{
    /// <summary>
    /// Basic sdk functions for OAuth 2.0
    /// </summary>
    public class BasicSDK2
    {


        /// <summary>
        /// Initializes a new instance of the <see cref="BasicSDK2"/> class.
        /// </summary>
        /// <param name="appKey">The app key.</param>
        /// <param name="appSecret">The app secret.</param>
        public BasicSDK2(string appKey, string appSecret)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(appKey), "app key can not be empty or null");
            Checker.Parameter(!string.IsNullOrWhiteSpace(appSecret), "app secret can not be empty or null");

            AppKey = appKey;
            AppSecret = appSecret;
        }

        /// <summary>
        /// Gets the app key.
        /// </summary>
        public string AppKey
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the app secret.
        /// </summary>
        public string AppSecret
        {
            get;
            internal set;
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
        /// Builds the request url.
        /// </summary>
        /// <param name="apiUrl">The API URL.</param>
        /// <param name="args">The args.</param>
        /// <returns></returns>
        protected string BuildRequestUrl(string apiUrl,IDictionary<string, dynamic> args)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(apiUrl), "apiUrl can not be empty or null");

            if (args != null)
            {
                foreach (KeyValuePair<string, dynamic> item in args)
                {
                    apiUrl = AppendQuery(apiUrl, item.Key, item.Value);
                }
            }

            return apiUrl;

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
        /// Get the api response use HTTP GET
        /// </summary>
        /// <param name="apiUrl">The api url(include query string).</param>
        /// <returns>
        /// The HttpResponseObj instance(never null).
        /// </returns>
        public HttpResponseObj Get(string apiUrl)
        {
            return Get(apiUrl, null, true);
        }

        /// <summary>
        /// Gets the specified API URL.
        /// </summary>
        /// <param name="apiUrl">The API URL.</param>
        /// <param name="useAuth">if set to <c>true</c> [use auth].</param>
        /// <returns></returns>
        public HttpResponseObj Get(string apiUrl, bool useAuth)
        {
            return Get(apiUrl, null, useAuth);
        }

        /// <summary>
        /// Gets the specified API URL.
        /// </summary>
        /// <param name="apiUrl">The API URL.</param>
        /// <param name="args">The args.</param>
        /// <returns></returns>
        public virtual HttpResponseObj Get(string apiUrl, IDictionary<string, dynamic> args)
        {
            return Get(apiUrl, args, true);
        }

        /// <summary>
        /// Get the api response use HTTP GET
        /// </summary>
        /// <param name="apiUrl">The api url(exclude query string).</param>
        /// <param name="args">The args.</param>
        /// <param name="useAuth">if set to <c>true</c> [use auth].</param>
        /// <returns>
        /// The HttpResponseObj instance(never null).
        /// </returns>
        public virtual HttpResponseObj Get(string apiUrl, IDictionary<string, dynamic> args, bool useAuth)
        {
            return HttpWebHost.Get(BuildRequestUrl(apiUrl, args));
        }

        /// <summary>
        /// Get the api response use HTTP POST(ContentType="application/x-www-form-urlencoded").
        /// </summary>
        /// <param name="apiUrl">The api url(exclude query string).</param>
        /// <returns>
        /// The HttpResponseObj instance(never null).
        /// </returns>
        public HttpResponseObj Post(string apiUrl)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(apiUrl), "apiUrl can not be empty or null");

            Uri uri = new Uri(apiUrl);
            NameValueCollection nvc = HttpKits.ResolveParameters(uri);
            IDictionary<string, dynamic> args = new Dictionary<string, dynamic>();
            foreach (string name in nvc)
                args.Add(new KeyValuePair<string, dynamic>(name, nvc[name]));

            return Post(uri.AbsoluteUri.Replace(uri.Query, string.Empty), args);

        }

        /// <summary>
        /// Get the api response use HTTP POST(ContentType="application/x-www-form-urlencoded").
        /// </summary>
        /// <param name="apiUrl">The api url(exclude query string).</param>
        /// <param name="args">The args.</param>
        /// <returns>
        /// The HttpResponseObj instance(never null).
        /// </returns>
        public virtual HttpResponseObj Post(string apiUrl, IDictionary<string, dynamic> args)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(apiUrl), "apiUrl can not be empty or null");

            return HttpWebHost.Post(BuildRequestUrl(apiUrl, args));

        }

        /// <summary>
        /// Get the api response use HTTP POST(ContentType="multipart/form-data").
        /// </summary>
        /// <param name="apiUrl">The api url(exclude query string).</param>
        /// <param name="postDatas">The post datas.</param>
        /// <returns>
        /// The HttpResponseObj instance(never null).
        /// </returns>
        public virtual HttpResponseObj Post(string apiUrl, IMultipartFormData[] postDatas)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(apiUrl), "apiUrl can not be empty or null");

            return HttpWebHost.Post(apiUrl, postDatas);
        }
    }
}
