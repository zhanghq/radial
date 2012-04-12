using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radial.Param;
using Radial.Serialization;
using Radial.Net;

namespace Radial.Web.OpenApi.SDK
{
    /// <summary>
    /// Sina weibo sdk for OAuth 2.0
    /// </summary>
    public sealed class SinaWeibo2 : BasicSDK2
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

        /// <summary>
        /// GetUidApiUrl
        /// </summary>
        const string GetUidApiUrl = "https://api.weibo.com/2/account/get_uid.json";

        long _current_uid = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="SinaWeibo2"/> class.
        /// </summary>
        /// <param name="initialPair">The initial app key/secret pair allocated by the provider.</param>
        /// <param name="tokenPersistence">The ITokenPersistence instance.</param>
        public SinaWeibo2(KeySecretPair initialPair, ITokenPersistence tokenPersistence) : base(initialPair, tokenPersistence) { }

        /// <summary>
        /// Gets the default SinaWeibo2 instance.
        /// </summary>
        public static SinaWeibo2 Default
        {
            get
            {
                KeySecretPair pair = new KeySecretPair { Key = AppParam.GetValue("sinaweibo.appkey"), Secret = AppParam.GetValue("sinaweibo.appsecret") };
                return new SinaWeibo2(pair,new HttpSessionTokenPersistence("sinaweibo"));
            }
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
        public string GetAuthorizationUrl(string redirect_uri, string response_type, string state, string display)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(redirect_uri), "redirect_uri can not be empty or null");

            IDictionary<string, dynamic> args = new Dictionary<string, dynamic>();
            args.Add("client_id", InitialPair.Key);
            args.Add("redirect_uri", redirect_uri);

            if (!string.IsNullOrWhiteSpace(response_type))
                args.Add("response_type", response_type);
            if (!string.IsNullOrWhiteSpace(state))
                args.Add("state", state);
            if (!string.IsNullOrWhiteSpace(display))
                args.Add("display", display);

            return BuildRequestUrl(AuthApiUrl,args);

        }

        /// <summary>
        /// Gets the access token.
        /// </summary>
        /// <param name="grant_type">The grant_type.</param>
        /// <param name="code">The code.</param>
        /// <param name="redirect_uri">The redirect_uri.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="refresh_token">The refresh_token.</param>
        /// <returns></returns>
        public KeySecretPair GetAccessToken(string grant_type, string code, string redirect_uri, string username, string password, string refresh_token)
        {
            int expires_in;
            string refresh_token_callback;
            return GetAccessToken(grant_type, code, redirect_uri, username, password, refresh_token, out expires_in, out refresh_token_callback);
        }

        /// <summary>
        /// Gets the access token.
        /// </summary>
        /// <param name="grant_type">The grant_type.</param>
        /// <param name="code">The code.</param>
        /// <param name="redirect_uri">The redirect_uri.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="refresh_token">The refresh_token.</param>
        /// <param name="expires_in">The expires_in.</param>
        /// <returns></returns>
        public KeySecretPair GetAccessToken(string grant_type, string code, string redirect_uri, string username, string password, string refresh_token, out int expires_in)
        {
            string refresh_token_callback;
            return GetAccessToken(grant_type, code, redirect_uri, username, password, refresh_token, out expires_in, out refresh_token_callback);
        }

        /// <summary>
        /// Gets the access token.
        /// </summary>
        /// <param name="grant_type">The grant_type.</param>
        /// <param name="code">The code.</param>
        /// <param name="redirect_uri">The redirect_uri.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="refresh_token">The refresh_token.</param>
        /// <param name="expires_in">The expires_in.</param>
        /// <param name="refresh_token_callback">The refresh_token_callback.</param>
        /// <returns></returns>
        public KeySecretPair GetAccessToken(string grant_type, string code, string redirect_uri, string username, string password, string refresh_token,out int expires_in,out string refresh_token_callback)
        {
            expires_in = 0;
            refresh_token_callback = null;

            IDictionary<string, dynamic> args = new Dictionary<string, dynamic>();
            args.Add("client_id", InitialPair.Key);
            args.Add("client_secret", InitialPair.Secret);

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
            if (!string.IsNullOrWhiteSpace(refresh_token))
                args.Add("refresh_token", refresh_token);

            //use base method in order to bypass append access token to request url.
            HttpResponseObj resp = base.Post(AccessTokenApiUrl, args);

            Checker.Requires(resp.Code == System.Net.HttpStatusCode.OK, "request access token error, code: {0} text: {1}", resp.Code, resp.Text);

            dynamic o = JsonSerializer.Deserialize<dynamic>(resp.Text);

            if (o.expires_in != null)
                expires_in = o.expires_in;
            if (o.refresh_token != null)
                refresh_token_callback = o.refresh_token;
            return new KeySecretPair { Secret = o.access_token };
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
        /// Gets the current user id.
        /// </summary>
        /// <returns></returns>
        public long CurrentUserId
        {
            get
            {
                if (_current_uid == 0)
                {
                    HttpResponseObj resp = Get(GetUidApiUrl);

                    Checker.Requires(resp.Code == System.Net.HttpStatusCode.OK, "request user id error, code: {0} text: {1}", resp.Code, resp.Text);

                    dynamic o = JsonSerializer.Deserialize<dynamic>(resp.Text);

                    _current_uid = o.uid;
                }

                return _current_uid;
            }
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
        public override HttpResponseObj Get(string apiUrl, IDictionary<string, dynamic> args, bool useAuth)
        {
            if (args == null)
                args = new Dictionary<string, dynamic>();

            if (useAuth)
            {
                KeySecretPair access = this.TokenPersistence.GetAccessToken();

                Checker.Requires(!string.IsNullOrWhiteSpace(access.Secret), "access token can not be empty or null");

                if (args.Count(o => string.Compare(o.Key, "access_token", true) == 0) == 0)
                    args.Add("access_token", access.Secret);
            }


            return base.Get(apiUrl, args, useAuth);
        }

        /// <summary>
        /// Get the api response use HTTP POST(ContentType="application/x-www-form-urlencoded").
        /// </summary>
        /// <param name="apiUrl">The api url(exclude query string).</param>
        /// <param name="args">The args(append access_token automatically).</param>
        /// <returns>
        /// The HttpResponseObj instance(never null).
        /// </returns>
        public override HttpResponseObj Post(string apiUrl, IDictionary<string, dynamic> args)
        {
            KeySecretPair access = this.TokenPersistence.GetAccessToken();

            Checker.Requires(!string.IsNullOrWhiteSpace(access.Secret), "access token can not be empty or null");

            if (args == null)
                args = new Dictionary<string, dynamic>();

            if (args.Count(o => string.Compare(o.Key, "access_token", true) == 0) == 0)
                args.Add("access_token", access.Secret);

            return base.Post(apiUrl, args);
        }

        /// <summary>
        /// Get the api response use HTTP POST(ContentType="multipart/form-data").
        /// </summary>
        /// <param name="apiUrl">The api url(exclude query string).</param>
        /// <param name="postDatas">The post datas(append access_token automatically).</param>
        /// <returns>
        /// The HttpResponseObj instance(never null).
        /// </returns>
        public override HttpResponseObj Post(string apiUrl, IMultipartFormData[] postDatas)
        {
            Checker.Parameter(postDatas != null, "post data can not be null");

            KeySecretPair access = this.TokenPersistence.GetAccessToken();

            Checker.Requires(!string.IsNullOrWhiteSpace(access.Secret), "access token can not be empty or null");

            List<IMultipartFormData> postDataList = new List<IMultipartFormData>(postDatas);

            if (postDataList.FirstOrDefault(o => string.Compare(o.ParamName, "access_token", true) == 0) == null)
                postDataList.Add(new PlainTextFormData("access_token", access.Secret));

            return base.Post(apiUrl, postDataList.ToArray());
        }
    }
}
