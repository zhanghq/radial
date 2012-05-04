using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radial.Param;
using Radial.Net;
using Radial.Serialization;
using System.Collections.Specialized;

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

        string _accessToken;
        string _openId;
        string _clientIp;
        string _scope = "all";

        /// <summary>
        /// Initializes a new instance of the <see cref="TencentWeibo"/> class.
        /// </summary>
        /// <param name="appKey">The app key.</param>
        /// <param name="appSecret">The app secret.</param>
        public TencentWeibo(string appKey, string appSecret)
            : base(appKey, appSecret)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TencentWeibo"/> class using the default parameter values.
        /// </summary>
        public TencentWeibo()
            : this(AppParam.GetValue("tencentweibo.appkey"), AppParam.GetValue("tencentweibo.appsecret"))
        {
        }

        /// <summary>
        /// Sets the access token.
        /// </summary>
        /// <param name="accessToken">The access_token.</param>
        public void SetAccessToken(string accessToken)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(accessToken), "access_token can not be empty or null");
            _accessToken = accessToken.Trim();
        }


        /// <summary>
        /// Gets the access token.
        /// </summary>
        public string AccessToken
        {
            get
            {
                Checker.Requires(!string.IsNullOrWhiteSpace(_accessToken), "cannot find access_token, please invoke SetAccessToken method first");
                return _accessToken;
            }
        }


        /// <summary>
        /// Sets the open id.
        /// </summary>
        /// <param name="openId">The open id.</param>
        public void SetOpenId(string openId)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(openId), "openid can not be empty or null");
            _openId = openId.Trim();
        }


        /// <summary>
        /// Gets the open id.
        /// </summary>
        public string OpenId
        {
            get
            {
                Checker.Requires(!string.IsNullOrWhiteSpace(_openId), "cannot find openid, please invoke SetOpenId method first");
                return _openId;
            }
        }


        /// <summary>
        /// Sets the client ip.
        /// </summary>
        /// <param name="clientIp">The client ip.</param>
        public void SetClientIp(string clientIp)
        {
            _clientIp = clientIp.Trim();
        }



        /// <summary>
        /// Gets the client ip.
        /// </summary>
        public string ClientIp
        {
            get
            {
                return _clientIp;
            }
        }


        /// <summary>
        /// Sets the scope.
        /// </summary>
        /// <param name="scope">The scope.</param>
        public void SetScope(string scope)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(scope), "scope can not be empty or null");
            _scope = scope.Trim();
        }


        /// <summary>
        /// Gets the scope.
        /// </summary>
        public string Scope
        {
            get
            {
                Checker.Requires(!string.IsNullOrWhiteSpace(_scope), "cannot find scope, please invoke SetScope method first");
                return _scope;
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
            args.Add("client_id", AppKey);
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
        /// <param name="otherResponseData">The other response data.</param>
        /// <returns></returns>
        public string GetAccessTokenWithCode(string code, string redirect_uri, out NameValueCollection otherResponseData)
        {
            return GetAccessToken("authorization_code", code, redirect_uri, string.Empty, out otherResponseData);
        }

        /// <summary>
        /// Gets the access token(grant_type="refresh_token").
        /// </summary>
        /// <param name="refresh_token">The refresh_token.</param>
        /// <param name="otherResponseData">The other response data.</param>
        /// <returns></returns>
        public string GetAccessTokenWithToken(string refresh_token, out NameValueCollection otherResponseData)
        {
            return GetAccessToken("refresh_token", string.Empty, string.Empty, refresh_token, out otherResponseData);
        }

        /// <summary>
        /// Gets the access token.
        /// </summary>
        /// <param name="grant_type">The grant_type.</param>
        /// <param name="code">The code.</param>
        /// <param name="redirect_uri">The redirect_uri.</param>
        /// <param name="refresh_token">The refresh_token.</param>
        /// <param name="otherResponseData">The other response data.</param>
        /// <returns></returns>
        private string GetAccessToken(string grant_type, string code, string redirect_uri, string refresh_token, out NameValueCollection otherResponseData)
        {

            IDictionary<string, dynamic> args = new Dictionary<string, dynamic>();
            args.Add("client_id", AppKey);
            args.Add("client_secret", AppSecret);

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

            otherResponseData = HttpKits.ResolveParameters(resp.Text);

            SetAccessToken(otherResponseData["access_token"]);

            otherResponseData.Remove("access_token");

            return AccessToken;
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
                if (args.Count(o => string.Compare(o.Key, "oauth_consumer_key", true) == 0) == 0)
                    args.Add("oauth_consumer_key", AppKey);
                if (args.Count(o => string.Compare(o.Key, "access_token", true) == 0) == 0)
                    args.Add("access_token", AccessToken);
                if (args.Count(o => string.Compare(o.Key, "openid", true) == 0) == 0)
                    args.Add("openid", OpenId);
                if (args.Count(o => string.Compare(o.Key, "clientip", true) == 0) == 0 && !string.IsNullOrWhiteSpace(ClientIp))
                    args.Add("clientip", ClientIp);
                if (args.Count(o => string.Compare(o.Key, "oauth_version", true) == 0) == 0)
                    args.Add("oauth_version", "2.a");
                if (args.Count(o => string.Compare(o.Key, "scope", true) == 0) == 0)
                    args.Add("scope", Scope);
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

            if (args == null)
                args = new Dictionary<string, dynamic>();

            if (args.Count(o => string.Compare(o.Key, "oauth_consumer_key", true) == 0) == 0)
                args.Add("oauth_consumer_key", AppKey);
            if (args.Count(o => string.Compare(o.Key, "access_token", true) == 0) == 0)
                args.Add("access_token", AccessToken);
            if (args.Count(o => string.Compare(o.Key, "openid", true) == 0) == 0)
                args.Add("openid", OpenId);
            if (args.Count(o => string.Compare(o.Key, "clientip", true) == 0) == 0 && !string.IsNullOrWhiteSpace(ClientIp))
                args.Add("clientip", ClientIp);
            if (args.Count(o => string.Compare(o.Key, "oauth_version", true) == 0) == 0)
                args.Add("oauth_version", "2.a");
            if (args.Count(o => string.Compare(o.Key, "scope", true) == 0) == 0)
                args.Add("scope", Scope);

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


            List<IMultipartFormData> postDataList = new List<IMultipartFormData>(postDatas);

            if (postDataList.FirstOrDefault(o => string.Compare(o.ParamName, "oauth_consumer_key", true) == 0) == null)
                postDataList.Add(new PlainTextFormData("oauth_consumer_key", AppKey));
            if (postDataList.FirstOrDefault(o => string.Compare(o.ParamName, "access_token", true) == 0) == null)
                postDataList.Add(new PlainTextFormData("access_token", AccessToken));
            if (postDataList.FirstOrDefault(o => string.Compare(o.ParamName, "openid", true) == 0) == null)
                postDataList.Add(new PlainTextFormData("openid", OpenId));
            if (postDataList.FirstOrDefault(o => string.Compare(o.ParamName, "clientip", true) == 0) == null && !string.IsNullOrWhiteSpace(ClientIp))
                postDataList.Add(new PlainTextFormData("clientip", ClientIp));
            if (postDataList.FirstOrDefault(o => string.Compare(o.ParamName, "oauth_version", true) == 0) == null)
                postDataList.Add(new PlainTextFormData("oauth_version", "2.a"));
            if (postDataList.FirstOrDefault(o => string.Compare(o.ParamName, "scope", true) == 0) == null)
                postDataList.Add(new PlainTextFormData("scope", Scope));

            return base.Post(apiUrl, postDataList.ToArray());
        }
    }
}
