using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radial.Param;
using Radial.Serialization;
using Radial.Net;
using System.Collections.Specialized;

namespace Radial.Web.OpenApi.Sina
{
    /// <summary>
    /// Sina weibo sdk for OAuth 2.0
    /// </summary>
    public sealed class SinaWeiboSDK
    {
        /// <summary>
        /// SingleWeiboMessageUrlTemplate
        /// </summary>
        const string SingleWeiboMessageUrlTemplate = "http://weibo.com/:userid/:mid";
        /// <summary>
        /// WeiboUrlTemplate
        /// </summary>
        const string WeiboUrlTemplate = "http://weibo.com/{0}";
        /// <summary>
        /// AuthApiUrl
        /// </summary>
        const string AuthApiUrl = "https://api.weibo.com/oauth2/authorize";
        /// <summary>
        /// AccessTokenApiUrl
        /// </summary>
        const string AccessTokenApiUrl = "https://api.weibo.com/oauth2/access_token";

        string _accessToken;

        /// <summary>
        /// Initializes a new instance of the <see cref="SinaWeiboSDK"/> class.
        /// </summary>
        /// <param name="appKey">The app key.</param>
        /// <param name="appSecret">The app secret.</param>
        public SinaWeiboSDK(string appKey, string appSecret)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(appKey), "app key can not be empty or null");
            Checker.Parameter(!string.IsNullOrWhiteSpace(appSecret), "app secret can not be empty or null");

            AppKey = appKey;
            AppSecret = appSecret;
        }

        /// <summary>
        /// Gets the default SinaWeibo2 instance.
        /// </summary>
        public static SinaWeiboSDK Default
        {
            get
            {
                return new SinaWeiboSDK(AppParam.GetValue("sinaweibo.appkey"), AppParam.GetValue("sinaweibo.appsecret"));
            }
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
        /// Sets the access token.
        /// </summary>
        /// <param name="accessToken">The access_token.</param>
        public void SetAccessToken(string accessToken)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(accessToken), "access_token can not be empty or null");
            _accessToken = accessToken;
        }


        /// <summary>
        /// Gets the access token.
        /// </summary>
        public string AccessToken
        {
            get
            {
                Checker.Requires(!string.IsNullOrWhiteSpace(_accessToken), "cannot find access_token, please invoke SetAccessToken method first");
                return _accessToken.Trim();
            }
        }

        /// <summary>
        /// URLs the encode.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public string UrlEncode(string input)
        {
            return HttpKits.EscapeUrl(input);
        }

        /// <summary>
        /// Builds the request url.
        /// </summary>
        /// <param name="apiUrl">The API URL.</param>
        /// <param name="args">The args.</param>
        /// <returns></returns>
        private string BuildRequestUrl(string apiUrl, IDictionary<string, dynamic> args)
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
        private string AppendQuery(string url, string pName, dynamic pValue)
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
        /// Gets the authorization url(response_type="code").
        /// </summary>
        /// <param name="redirect_uri">The redirect_uri.</param>
        /// <param name="state">The state.</param>
        /// <param name="display">The display.</param>
        /// <returns>
        /// The authorization url.
        /// </returns>
        public string GetAuthorizationUrlWithCode(string redirect_uri, string state, string display)
        {
            return GetAuthorizationUrl(redirect_uri, "code", state, display);
        }

        /// <summary>
        /// Gets the authorization url(response_type="token").
        /// </summary>
        /// <param name="redirect_uri">The redirect_uri.</param>
        /// <param name="state">The state.</param>
        /// <param name="display">The display.</param>
        /// <returns>
        /// The authorization url.
        /// </returns>
        public string GetAuthorizationUrlWithToken(string redirect_uri, string state, string display)
        {
            return GetAuthorizationUrl(redirect_uri, "token", state, display);
        }

        /// <summary>
        /// Gets the authorization url.
        /// </summary>
        /// <param name="redirect_uri">The redirect_uri.</param>
        /// <param name="response_type">The response_type.</param>
        /// <param name="state">The state.</param>
        /// <param name="display">The display.</param>
        /// <returns>
        /// The authorization url.
        /// </returns>
        private string GetAuthorizationUrl(string redirect_uri, string response_type, string state, string display)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(redirect_uri), "redirect_uri can not be empty or null");

            IDictionary<string, dynamic> args = new Dictionary<string, dynamic>();
            args.Add("client_id", AppKey);
            args.Add("redirect_uri", redirect_uri);

            if (!string.IsNullOrWhiteSpace(response_type))
                args.Add("response_type", response_type);
            if (!string.IsNullOrWhiteSpace(state))
                args.Add("state", state);
            if (!string.IsNullOrWhiteSpace(display))
                args.Add("display", display);

            return BuildRequestUrl(AuthApiUrl, args);

        }

        /// <summary>
        /// Gets the access token(grant_type="authorization_code").
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="redirect_uri">The redirect_uri.</param>
        /// <param name="expires_in">The expires_in.</param>
        /// <param name="remind_in">The remind_in.</param>
        /// <param name="uid">The uid.</param>
        /// <returns>Access Token</returns>
        public string GetAccessTokenWithCode(string code, string redirect_uri, out int expires_in, out int remind_in, out long uid)
        {
            return GetAccessToken("authorization_code", code, redirect_uri, string.Empty, string.Empty, out expires_in, out remind_in, out uid);
        }

        /// <summary>
        /// Gets the access token(grant_type="password").
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="expires_in">The expires_in.</param>
        /// <param name="remind_in">The remind_in.</param>
        /// <param name="uid">The uid.</param>
        /// <returns>Access Token</returns>
        public string GetAccessTokenWithPassword(string username, string password, out int expires_in, out int remind_in, out long uid)
        {
            return GetAccessToken("password", string.Empty, string.Empty, username, password, out expires_in, out remind_in, out uid);
        }


        /// <summary>
        /// Gets the access token.
        /// </summary>
        /// <param name="grant_type">The grant_type.</param>
        /// <param name="code">The code.</param>
        /// <param name="redirect_uri">The redirect_uri.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="expires_in">The expires_in.</param>
        /// <param name="remind_in">The remind_in.</param>
        /// <param name="uid">The uid.</param>
        /// <returns>Access Token</returns>
        private string GetAccessToken(string grant_type, string code, string redirect_uri, string username, string password, out int expires_in, out int remind_in, out long uid)
        {
            expires_in = 0;
            remind_in = 0;
            uid = 0;

            IDictionary<string, dynamic> args = new Dictionary<string, dynamic>();
            args.Add("client_id", AppKey);
            args.Add("client_secret", AppSecret);

            if (!string.IsNullOrWhiteSpace(grant_type))
                args.Add("grant_type", grant_type);
            if (!string.IsNullOrWhiteSpace(code))
                args.Add("code", code);
            if (!string.IsNullOrWhiteSpace(redirect_uri))
                args.Add("redirect_uri", redirect_uri);
            if (!string.IsNullOrWhiteSpace(username))
                args.Add("username", username);
            if (!string.IsNullOrWhiteSpace(password))
                args.Add("password", password);

            //use HttpWebHost.Post method in order to bypass append access token to request url.
            HttpResponseObj resp = HttpWebHost.Post(BuildRequestUrl(AccessTokenApiUrl, args));

            Checker.Requires(resp.Code == System.Net.HttpStatusCode.OK, "request access token error, code: {0} text: {1}", resp.Code, resp.Text);

            dynamic o = JsonSerializer.Deserialize<dynamic>(resp.Text);

            expires_in = o.expires_in;
            remind_in = int.Parse(o.remind_in.ToString());
            uid = long.Parse(o.uid.ToString());
            SetAccessToken((string)o.access_token);

            return AccessToken;
        }

        /// <summary>
        /// Gets the base62 format of weibo message id.
        /// </summary>
        /// <param name="mid">The weibo message id.</param>
        /// <returns>The base62 format of weibo message id</returns>
        public string GetBase62Mid(long mid)
        {
            string midStr = mid.ToString();

            midStr = midStr.PadLeft(midStr.Length + (7 - midStr.Length % 7), '0');

            Base62Encoder encoder = new Base62Encoder();
            string midBase62 = string.Empty;
            for (int i = 0; i * 7 < midStr.Length; i++)
            {
                midBase62 += encoder.ToBase62String(ulong.Parse(midStr.Substring(i * 7, 7)));
            }
            return midBase62;
        }

        /// <summary>
        /// Gets the long value from weibo message id base62 format.
        /// </summary>
        /// <param name="base62Mid">The base62  weibo message id.</param>
        /// <returns>The long type value of weibo message id.</returns>
        public long GetLongMid(string base62Mid)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(base62Mid), "base62 mid string can not be empty or null");

            base62Mid = base62Mid.PadLeft(base62Mid.Length + (4 - base62Mid.Length % 4), '*');

            Base62Encoder encoder = new Base62Encoder();
            string longString = string.Empty;

            for (int i = 0; i * 4 < base62Mid.Length; i++)
            {
                longString += encoder.FromBase62String(base62Mid.Substring(i * 4, 4).TrimStart('*')).ToString();
            }

            return long.Parse(longString);
        }


        /// <summary>
        /// Get the weibo single message url from the mid.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="mid">The mid.</param>
        /// <returns>The message url.</returns>
        public string GetMessageUrlFromMid(long userId, long mid)
        {
            return SingleWeiboMessageUrlTemplate.Replace(":userid", userId.ToString()).Replace(":mid", GetBase62Mid(mid));
        }

        /// <summary>
        /// Gets the weibo URL.
        /// </summary>
        /// <param name="uid">The uid.</param>
        /// <returns></returns>
        public string GetWeiboUrl(long uid)
        {

            return string.Format(WeiboUrlTemplate, "u/" + uid);
        }


        /// <summary>
        /// Gets the weibo URL.
        /// </summary>
        /// <param name="udomain">The user domain.</param>
        /// <returns></returns>
        public string GetWeiboUrl(string udomain)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(udomain), "user domain can not be empty or null.");

            return string.Format(WeiboUrlTemplate, udomain);
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
        /// Get the api response use HTTP GET
        /// </summary>
        /// <param name="apiUrl">The api url(include query string).</param>
        /// <param name="useAuth">if set to <c>true</c> [use auth].</param>
        /// <returns>
        /// The HttpResponseObj instance(never null).
        /// </returns>
        public HttpResponseObj Get(string apiUrl, bool useAuth)
        {
            return Get(apiUrl, null, useAuth);
        }

        /// <summary>
        /// Get the api response use HTTP GET
        /// </summary>
        /// <param name="apiUrl">The api url(exclude query string).</param>
        /// <param name="args">The args(append access_token automatically).</param>
        /// <returns>
        /// The HttpResponseObj instance(never null).
        /// </returns>
        public HttpResponseObj Get(string apiUrl, IDictionary<string, dynamic> args)
        {
            return Get(apiUrl, args, true);
        }

        /// <summary>
        /// Get the api response use HTTP POST(ContentType="application/x-www-form-urlencoded").
        /// </summary>
        /// <param name="apiUrl">The api url(include query string).</param>
        /// <returns>
        /// The HttpResponseObj instance(never null).
        /// </returns>
        public HttpResponseObj Post(string apiUrl)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(apiUrl), "apiUrl can not be empty or null");

            Uri uri = new Uri(apiUrl);
            NameValueCollection nvc = HttpKits.ResolveParameters(uri.Query);
            IDictionary<string, dynamic> args = new Dictionary<string, dynamic>();
            foreach (string name in nvc)
                args.Add(new KeyValuePair<string, dynamic>(name, nvc[name]));

            return Post(uri.AbsoluteUri.Replace(uri.Query, string.Empty), args);

        }

        /// <summary>
        /// Get the api response use HTTP GET
        /// </summary>
        /// <param name="apiUrl">The api url(exclude query string).</param>
        /// <param name="args">The args(append access_token automatically).</param>
        /// <param name="useAuth">if set to <c>true</c> [use auth].</param>
        /// <returns>
        /// The HttpResponseObj instance(never null).
        /// </returns>
        public HttpResponseObj Get(string apiUrl, IDictionary<string, dynamic> args, bool useAuth)
        {
            if (args == null)
                args = new Dictionary<string, dynamic>();

            if (useAuth)
            {
                if (args.Count(o => string.Compare(o.Key, "access_token", true) == 0) == 0)
                    args.Add("access_token", AccessToken);
            }


            return HttpWebHost.Get(BuildRequestUrl(apiUrl, args));
        }

        /// <summary>
        /// Get the api response use HTTP POST(ContentType="application/x-www-form-urlencoded").
        /// </summary>
        /// <param name="apiUrl">The api url(exclude query string).</param>
        /// <param name="args">The args(append access_token automatically).</param>
        /// <returns>
        /// The HttpResponseObj instance(never null).
        /// </returns>
        public HttpResponseObj Post(string apiUrl, IDictionary<string, dynamic> args)
        {

            if (args == null)
                args = new Dictionary<string, dynamic>();

            if (args.Count(o => string.Compare(o.Key, "access_token", true) == 0) == 0)
                args.Add("access_token", AccessToken);

            return HttpWebHost.Post(BuildRequestUrl(apiUrl, args));
        }

        /// <summary>
        /// Get the api response use HTTP POST(ContentType="multipart/form-data").
        /// </summary>
        /// <param name="apiUrl">The api url(exclude query string).</param>
        /// <param name="postDatas">The post datas(append access_token automatically).</param>
        /// <returns>
        /// The HttpResponseObj instance(never null).
        /// </returns>
        public HttpResponseObj Post(string apiUrl, IMultipartFormData[] postDatas)
        {
            Checker.Parameter(postDatas != null, "post data can not be null");

            List<IMultipartFormData> postDataList = new List<IMultipartFormData>(postDatas);

            if (postDataList.FirstOrDefault(o => string.Compare(o.ParamName, "access_token", true) == 0) == null)
                postDataList.Add(new PlainTextFormData("access_token", AccessToken));

            return HttpWebHost.Post(apiUrl, postDatas);
        }
    }
}
