using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using Radial.Param;
using Radial.Net;

namespace Radial.Web.OpenApi.SDK
{
    /// <summary>
    /// Sina weibo sdk.
    /// </summary>
    public sealed class SinaWeibo : BasicSDK
    {
        /// <summary>
        /// SingleWeiboMessageUrlTemplate
        /// </summary>
        const string SingleWeiboMessageUrlTemplate = "http://weibo.com/:userid/:mid";
        /// <summary>
        /// RequestApiUrl
        /// </summary>
        const string RequestApiUrl = "http://api.t.sina.com.cn/oauth/request_token";
        /// <summary>
        /// AuthApiUrl
        /// </summary>
        const string AuthApiUrl = "http://api.t.sina.com.cn/oauth/authorize";
        /// <summary>
        /// AccessTokenApiUrl
        /// </summary>
        const string AccessTokenApiUrl = "http://api.t.sina.com.cn/oauth/access_token";

        /// <summary>
        /// Initializes a new instance of the <see cref="SinaWeibo"/> class.
        /// </summary>
        /// <param name="oauth">The OAuth object.</param>
        /// <param name="tokenPersistence">The ITokenPersistence instance.</param>
        public SinaWeibo(OAuth oauth, ITokenPersistence tokenPersistence) : base(oauth, tokenPersistence) { }

        /// <summary>
        /// Gets the default SinaWeibo instance.
        /// </summary>
        public static SinaWeibo Default
        {
            get
            {
                KeySecretPair pair = new KeySecretPair { Key = AppParam.GetValue("sinaweibo.appkey"), Secret = AppParam.GetValue("sinaweibo.appsecret") };
                return new SinaWeibo(new OAuth(pair), new HttpSessionTokenPersistence("sinaweibo"));
            }
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
        protected override string AppendQuery(string url, string pName, dynamic pValue)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(url), "url can not be empty or null");
            Checker.Parameter(!string.IsNullOrWhiteSpace(pName), "parameter name can not be empty or null");
            Checker.Parameter(pValue != null, "parameter value can not be null");

            string query = string.Format("{0}={1}", pName, UrlEncode(UrlEncode(pValue.ToString())));
            if (url.Contains('?'))
                return string.Join("&", url, query);
            return string.Join("?", url, query);
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
        /// Gets the access token.
        /// </summary>
        /// <param name="verifier">The verifier string.</param>
        /// <param name="userId">The login user id.</param>
        /// <returns>
        /// The access token.
        /// </returns>
        public KeySecretPair GetAccessToken(string verifier, out string userId)
        {
            NameValueCollection additionalParams;
            KeySecretPair ks = GetAccessToken(verifier, AccessTokenApiUrl, out additionalParams);
            userId = additionalParams["user_id"];
            return ks;
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
        /// Appends the source key.
        /// </summary>
        /// <param name="apiUrl">The API URL.</param>
        /// <returns></returns>
        private string AppendSourceKey(string apiUrl)
        {
            return AppendQuery(apiUrl, "source", OAuth.AppKeySecret.Key);
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
            apiUrl = AppendSourceKey(apiUrl);
            return base.Get(apiUrl, args, needAuth);
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
            apiUrl = AppendSourceKey(apiUrl);
            return base.Post(apiUrl, args);
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
            apiUrl = AppendSourceKey(apiUrl);
            return base.Post(apiUrl, args, fileData);
        }
    }
}
