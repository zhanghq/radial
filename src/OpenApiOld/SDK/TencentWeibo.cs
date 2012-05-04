using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radial.Param;
using Radial.Net;
using Radial.Serialization;

namespace Radial.Web.OpenApi.SDK
{
    /// <summary>
    /// Tencent Weibo SDK OAuth 2.0 
    /// </summary>
    public sealed class TencentWeibo : BasicSDK2
    {
        /// <summary>
        /// AuthApiUrl
        /// </summary>
        const string AuthApiUrl = "https://open.t.qq.com/cgi-bin/oauth2/authorize";
        /// <summary>
        /// AccessTokenApiUrl
        /// </summary>
        const string AccessTokenApiUrl = "https://open.t.qq.com/cgi-bin/oauth2/access_token";

        /// <summary>
        /// Initializes a new instance of the <see cref="TencentWeibo"/> class.
        /// </summary>
        /// <param name="initialPair">The initial app key/secret pair allocated by the provider.</param>
        /// <param name="tokenPersistence">The ITokenPersistence instance.</param>
        public TencentWeibo(KeySecretPair initialPair, ITokenPersistence tokenPersistence) : base(initialPair, tokenPersistence) { }

        /// <summary>
        /// Gets the default TencentWeibo instance.
        /// </summary>
        public static TencentWeibo Default
        {
            get
            {
                KeySecretPair pair = new KeySecretPair { Key = AppParam.GetValue("tencentweibo.appkey"), Secret = AppParam.GetValue("tencentweibo.appsecret") };
                return new TencentWeibo(pair, new HttpSessionTokenPersistence("tencentweibo"));
            }
        }

        /// <summary>
        /// Gets the authorization url(response_type="code").
        /// </summary>
        /// <param name="redirect_uri">The redirect_uri.</param>
        /// <param name="wap">The wap.</param>
        /// <returns>
        /// The authorization url.
        /// </returns>
        public string GetAuthorizationUrlWithCode(string redirect_uri, int? wap)
        {
            return GetAuthorizationUrl(redirect_uri, "code", wap);
        }

        ///// <summary>
        ///// Gets the authorization url(response_type="token").
        ///// </summary>
        ///// <param name="redirect_uri">The redirect_uri.</param>
        ///// <param name="wap">The wap.</param>
        ///// <returns>
        ///// The authorization url.
        ///// </returns>
        //private string GetAuthorizationUrlWithToken(string redirect_uri, int? wap)
        //{
        //    return GetAuthorizationUrl(redirect_uri, "token", wap);
        //}

        /// <summary>
        /// Gets the authorization url.
        /// </summary>
        /// <param name="redirect_uri">The redirect_uri.</param>
        /// <param name="response_type">The response_type.</param>
        /// <param name="wap">The wap.</param>
        /// <returns>
        /// The authorization url.
        /// </returns>
        private string GetAuthorizationUrl(string redirect_uri, string response_type, int? wap)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(redirect_uri), "redirect_uri can not be empty or null");

            IDictionary<string, dynamic> args = new Dictionary<string, dynamic>();
            args.Add("client_id", InitialPair.Key);
            args.Add("redirect_uri", redirect_uri);

            if (!string.IsNullOrWhiteSpace(response_type))
                args.Add("response_type", response_type);
            if (wap.HasValue)
                args.Add("wap", wap.Value);

            return BuildRequestUrl(AuthApiUrl, args);

        }

        /// <summary>
        /// Gets the access token(grant_type="authorization_code").
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="redirect_uri">The redirect_uri.</param>
        /// <param name="expires_in">The expires_in.</param>
        /// <returns></returns>
        public KeySecretPair GetAccessTokenWithCode(string code, string redirect_uri, out int expires_in)
        {
            string new_refresh_token;
            return GetAccessTokenWithCode(code, redirect_uri, out expires_in, out new_refresh_token);
        }

        /// <summary>
        /// Gets the access token(grant_type="authorization_code").
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="redirect_uri">The redirect_uri.</param>
        /// <param name="expires_in">The expires_in.</param>
        /// <param name="new_refresh_token">The new_refresh_token.</param>
        /// <returns></returns>
        public KeySecretPair GetAccessTokenWithCode(string code, string redirect_uri, out int expires_in, out string new_refresh_token)
        {
            return GetAccessToken("authorization_code", code, redirect_uri, string.Empty, out expires_in, out new_refresh_token);
        }

        /// <summary>
        /// Gets the access token(grant_type="refresh_token").
        /// </summary>
        /// <param name="refresh_token">The refresh_token.</param>
        /// <param name="expires_in">The expires_in.</param>
        /// <returns></returns>
        public KeySecretPair GetAccessTokenWithToken(string refresh_token, out int expires_in)
        {
            string new_refresh_token;
            return GetAccessTokenWithToken(refresh_token, out expires_in, out new_refresh_token);
        }

        /// <summary>
        /// Gets the access token(grant_type="refresh_token").
        /// </summary>
        /// <param name="refresh_token">The refresh_token.</param>
        /// <param name="expires_in">The expires_in.</param>
        /// <param name="new_refresh_token">The new_refresh_token.</param>
        /// <returns></returns>
        public KeySecretPair GetAccessTokenWithToken(string refresh_token, out int expires_in, out string new_refresh_token)
        {
            return GetAccessToken("authorization_code", string.Empty, string.Empty, refresh_token, out expires_in, out new_refresh_token);
        }

        /// <summary>
        /// Gets the access token.
        /// </summary>
        /// <param name="grant_type">The grant_type.</param>
        /// <param name="code">The code.</param>
        /// <param name="redirect_uri">The redirect_uri.</param>
        /// <param name="refresh_token">The refresh_token.</param>
        /// <param name="expires_in">The expires_in.</param>
        /// <param name="new_refresh_token">The new_refresh_token.</param>
        /// <returns></returns>
        private KeySecretPair GetAccessToken(string grant_type, string code, string redirect_uri, string refresh_token, out int expires_in, out string new_refresh_token)
        {
            expires_in = 0;
            new_refresh_token = null;

            IDictionary<string, dynamic> args = new Dictionary<string, dynamic>();
            args.Add("client_id", InitialPair.Key);
            args.Add("client_secret", InitialPair.Secret);

            if (!string.IsNullOrWhiteSpace(grant_type))
                args.Add("grant_type", grant_type);
            if (!string.IsNullOrWhiteSpace(code))
                args.Add("code", code);
            if (!string.IsNullOrWhiteSpace(redirect_uri))
                args.Add("redirect_uri", redirect_uri);
            if (!string.IsNullOrWhiteSpace(refresh_token))
                args.Add("refresh_token", refresh_token);

            //use base method in order to bypass append access token to request url.
            HttpResponseObj resp = base.Post(AccessTokenApiUrl, args);

            Checker.Requires(resp.Code == System.Net.HttpStatusCode.OK, "request access token error, code: {0} text: {1}", resp.Code, resp.Text);

            dynamic o = JsonSerializer.Deserialize<dynamic>(resp.Text);

            expires_in = o.expires_in;
            //not supported yet 
            new_refresh_token = o.refresh_token;
            return new KeySecretPair { Secret = o.access_token };
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

                if (args.Count(o => string.Compare(o.Key, "oauth_consumer_key", true) == 0) == 0)
                    args.Add("oauth_consumer_key", InitialPair.Key);
                if (args.Count(o => string.Compare(o.Key, "access_token", true) == 0) == 0)
                    args.Add("access_token", access.Secret);
                if (args.Count(o => string.Compare(o.Key, "oauth_version", true) == 0) == 0)
                    args.Add("oauth_version", "2.a");
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
