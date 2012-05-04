using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Configuration;
using System.Collections.Specialized;
using Radial.Param;
using Radial.Net;
using System.Net;

namespace Radial.Web.OpenApi
{
    /// <summary>
    /// The class specify a set of methods about OAuth.
    /// </summary>
    public sealed class OAuth
    {
        #region QueryParameter & QueryParameterComparer
        /// <summary>
        /// Provides an internal structure to sort the query parameter
        /// </summary>
        private class QueryParameter
        {
            private string name = null;
            private string value = null;

            /// <summary>
            /// Initializes a new instance of the <see cref="QueryParameter"/> class.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <param name="value">The value.</param>
            public QueryParameter(string name, string value)
            {
                this.name = name;
                this.value = value;
            }

            /// <summary>
            /// Gets the name.
            /// </summary>
            public string Name
            {
                get { return name; }
            }

            /// <summary>
            /// Gets the value.
            /// </summary>
            public string Value
            {
                get { return value; }
            }
        }

        /// <summary>
        /// Comparer class used to perform the sorting of the query parameters
        /// </summary>
        private class QueryParameterComparer : IComparer<QueryParameter>
        {

            #region IComparer<QueryParameter> Members

            /// <summary>
            /// Compares the specified x.
            /// </summary>
            /// <param name="x">The x.</param>
            /// <param name="y">The y.</param>
            /// <returns></returns>
            public int Compare(QueryParameter x, QueryParameter y)
            {
                if (x == null)
                    throw new ArgumentNullException("x");
                if (y == null)
                    throw new ArgumentNullException("y");

                if (x.Name == y.Name)
                {
                    return string.Compare(x.Value, y.Value);
                }
                else
                {
                    return string.Compare(x.Name, y.Name);
                }
            }

            #endregion
        }
        #endregion

        /// <summary>
        /// OAuthVersion
        /// </summary>
        public const string OAuthVersion = "1.0";
        /// <summary>
        /// OAuthParameterPrefix
        /// </summary>
        public const string OAuthParameterPrefix = "oauth_";

        //
        // List of know and used oauth parameters' names
        //
        /// <summary>
        /// Gets or sets the OAuthConsumerKeyKey
        /// </summary>
        public string OAuthConsumerKeyKey = "oauth_consumer_key";
        /// <summary>
        /// Gets or sets the OAuthCallbackKey
        /// </summary>
        public string OAuthCallbackKey = "oauth_callback";
        /// <summary>
        /// Gets or sets the OAuthVersionKey
        /// </summary>
        public string OAuthVersionKey = "oauth_version";
        /// <summary>
        /// Gets or sets the OAuthSignatureMethodKey
        /// </summary>
        public string OAuthSignatureMethodKey = "oauth_signature_method";
        /// <summary>
        /// Gets or sets the OAuthSignatureKey
        /// </summary>
        public string OAuthSignatureKey = "oauth_signature";
        /// <summary>
        /// Gets or sets the OAuthTimestampKey
        /// </summary>
        public string OAuthTimestampKey = "oauth_timestamp";
        /// <summary>
        /// Gets or sets the OAuthNonceKey
        /// </summary>
        public string OAuthNonceKey = "oauth_nonce";
        /// <summary>
        /// Gets or sets the OAuthTokenKey
        /// </summary>
        public string OAuthTokenKey = "oauth_token";
        /// <summary>
        /// Gets or sets the OAuthTokenSecretKey
        /// </summary>
        public string OAuthTokenSecretKey = "oauth_token_secret";
        /// <summary>
        /// Gets or sets the OAuthVerifier
        /// </summary>
        public string OAuthVerifier = "oauth_verifier";
        /// <summary>
        /// HMACSHA1SignatureType
        /// </summary>
        public const string HMACSHA1SignatureType = "HMAC-SHA1";
        /// <summary>
        /// PlainTextSignatureType
        /// </summary>
        public const string PlainTextSignatureType = "PLAINTEXT";
        /// <summary>
        /// RSASHA1SignatureType
        /// </summary>
        public const string RSASHA1SignatureType = "RSA-SHA1";
        /// <summary>
        /// UnreservedChars
        /// </summary>
        public const string UnreservedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";

        Random _random;
        KeySecretPair _appKeySecret;

        /// <summary>
        /// Initializes a new instance of the <see cref="OAuth"/> class.
        /// </summary>
        /// <param name="appKeySecret">The app key/secret struct.</param>
        public OAuth(KeySecretPair appKeySecret)
        {
            _appKeySecret = appKeySecret;

            Checker.Requires(!string.IsNullOrWhiteSpace(_appKeySecret.Key), "appkey can not be empty or null");
            Checker.Requires(!string.IsNullOrWhiteSpace(_appKeySecret.Secret), "appsecret can not be empty or null");

            _random = new Random();
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="OAuth"/> class.
        /// </summary>
        public OAuth()
            : this(new KeySecretPair { Key = AppParam.GetValue("appkey"), Secret = AppParam.GetValue("appsecret") })
        { }


        /// <summary>
        /// Gets the app key/secret.
        /// </summary>
        public KeySecretPair AppKeySecret
        {
            get
            {
                return _appKeySecret;
            }
        }

        /// <summary>
        /// Helper function to compute a hash value
        /// </summary>
        /// <param name="data">The data to hash</param>
        /// <param name="hashAlgorithm">The hashing algoirhtm used. If that algorithm needs some initialization, like HMAC and its derivatives, they should be initialized prior to passing it to this function</param>
        /// <returns>a Base64 string of the hash value</returns>
        private string ComputeHash(string data, HashAlgorithm hashAlgorithm)
        {
            if (hashAlgorithm == null)
            {
                throw new ArgumentNullException("hashAlgorithm");
            }

            if (string.IsNullOrEmpty(data))
            {
                throw new ArgumentNullException("data");
            }

            byte[] dataBuffer = System.Text.Encoding.ASCII.GetBytes(data);
            byte[] hashBytes = hashAlgorithm.ComputeHash(dataBuffer);

            return Convert.ToBase64String(hashBytes);
        }

        /// <summary>
        /// Internal function to cut out all non oauth query string parameters (all parameters not begining with "oauth_")
        /// </summary>
        /// <param name="parameters">The query string part of the Url</param>
        /// <returns>A list of QueryParameter each containing the parameter name and value</returns>
        private List<QueryParameter> GetQueryParameters(string parameters)
        {
            if (parameters.StartsWith("?"))
            {
                parameters = parameters.Remove(0, 1);
            }

            List<QueryParameter> result = new List<QueryParameter>();

            if (!string.IsNullOrEmpty(parameters))
            {
                string[] p = parameters.Split('&');
                foreach (string s in p)
                {
                    if ((!string.IsNullOrEmpty(s) && !s.StartsWith(OAuthParameterPrefix)) || s.StartsWith(OAuthCallbackKey))
                    {
                        if (s.IndexOf('=') > -1)
                        {
                            string[] temp = s.Split('=');
                            result.Add(new QueryParameter(temp[0], temp[1]));
                        }
                        else
                        {
                            result.Add(new QueryParameter(s, string.Empty));
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// This is a different Url Encode implementation since the default .NET one outputs the percent encoding in lower case.
        /// While this is not a problem with the percent encoding spec, it is used in upper case throughout OAuth
        /// </summary>
        /// <param name="value">The value to Url encode</param>
        /// <returns>Returns a Url encoded string</returns>
        public string UrlEncode(string value)
        {
            StringBuilder result = new StringBuilder();

            foreach (char symbol in value)
            {
                if (UnreservedChars.IndexOf(symbol) != -1)
                {
                    result.Append(symbol);
                }
                else
                {
                    result.Append('%' + String.Format("{0:X2}", (int)symbol));
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// Normalizes the request parameters according to the spec
        /// </summary>
        /// <param name="parameters">The list of parameters already sorted</param>
        /// <returns>a string representing the normalized parameters</returns>
        private string NormalizeRequestParameters(IList<QueryParameter> parameters)
        {
            StringBuilder sb = new StringBuilder();
            QueryParameter p = null;
            for (int i = 0; i < parameters.Count; i++)
            {
                p = parameters[i];
                sb.AppendFormat("{0}={1}", p.Name, p.Value);

                if (i < parameters.Count - 1)
                {
                    sb.Append("&");
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Generate the signature base that is used to produce the signature
        /// </summary>
        /// <param name="url">The full url that needs to be signed including its non OAuth url parameters</param>
        /// <param name="token">The token, if available. If not available pass null or an empty string</param>
        /// <param name="tokenSecret">The token secret, if available. If not available pass null or an empty string</param>
        /// <param name="verifier">The verifier, if available. If not available pass null or an empty string</param>
        /// <param name="httpMethod">The http method used. Must be a valid HTTP method verb (POST,GET,PUT, etc)</param>
        /// <param name="signatureType">The signature type. To use the default values use <see cref="SignatureTypes">SignatureTypes</see>.</param>
        /// <param name="normalizedUrl">The normalized URL.</param>
        /// <param name="normalizedRequestParameters">The normalized request parameters.</param>
        /// <returns>
        /// The signature base string
        /// </returns>
        private string GenerateSignatureBaseString(Uri url, string token, string tokenSecret, string verifier, string httpMethod, string signatureType, out string normalizedUrl, out string normalizedRequestParameters)
        {
            if (token == null)
            {
                token = string.Empty;
            }

            if (tokenSecret == null)
            {
                tokenSecret = string.Empty;
            }

            if (verifier == null)
                verifier = string.Empty;

            if (null == httpMethod)
            {
                throw new ArgumentNullException("httpMethod");
            }

            if (string.IsNullOrEmpty(signatureType))
            {
                throw new ArgumentNullException("signatureType");
            }

            normalizedUrl = null;
            normalizedRequestParameters = null;

            List<QueryParameter> parameters = GetQueryParameters(url.Query);
            parameters.Add(new QueryParameter(OAuthVersionKey, OAuthVersion));
            parameters.Add(new QueryParameter(OAuthNonceKey, GenerateNonce()));
            parameters.Add(new QueryParameter(OAuthTimestampKey, GenerateTimeStamp()));
            parameters.Add(new QueryParameter(OAuthSignatureMethodKey, signatureType));
            parameters.Add(new QueryParameter(OAuthConsumerKeyKey, AppKeySecret.Key));

            if (!string.IsNullOrEmpty(verifier) && httpMethod == WebRequestMethods.Http.Get)
            {
                parameters.Add(new QueryParameter(OAuthVerifier, verifier));
            }

            if (!string.IsNullOrEmpty(token))
            {
                parameters.Add(new QueryParameter(OAuthTokenKey, token));
            }

            parameters.Sort(new QueryParameterComparer());

            normalizedUrl = string.Format("{0}://{1}", url.Scheme, url.Host);
            if (!((url.Scheme == "http" && url.Port == 80) || (url.Scheme == "https" && url.Port == 443)))
            {
                normalizedUrl += ":" + url.Port;
            }
            normalizedUrl += url.AbsolutePath;
            normalizedRequestParameters = NormalizeRequestParameters(parameters);

            StringBuilder signatureBase = new StringBuilder();
            signatureBase.AppendFormat("{0}&", httpMethod);
            signatureBase.AppendFormat("{0}&", UrlEncode(normalizedUrl));
            signatureBase.AppendFormat("{0}", UrlEncode(normalizedRequestParameters));

            return signatureBase.ToString();
        }

        /// <summary>
        /// Generates a signature using the HMAC-SHA1 algorithm
        /// </summary>
        /// <param name="url">The full url that needs to be signed including its non OAuth url parameters</param>
        /// <param name="httpMethod">The http method used. Must be a valid HTTP method verb (POST,GET,PUT, etc)</param>
        /// <param name="normalizedUrl">The normalized URL.</param>
        /// <param name="normalizedRequestParameters">The normalized request parameters.</param>
        /// <returns>
        /// A base64 string of the hash value
        /// </returns>
        public string GenerateSignature(Uri url, string httpMethod, out string normalizedUrl, out string normalizedRequestParameters)
        {
            return GenerateSignature(url, string.Empty, string.Empty, httpMethod, out normalizedUrl, out normalizedRequestParameters);
        }

        /// <summary>
        /// Generates a signature using the HMAC-SHA1 algorithm
        /// </summary>
        /// <param name="url">The full url that needs to be signed including its non OAuth url parameters</param>
        /// <param name="token">The token, if available. If not available pass null or an empty string</param>
        /// <param name="tokenSecret">The token secret, if available. If not available pass null or an empty string</param>
        /// <param name="httpMethod">The http method used. Must be a valid HTTP method verb (POST,GET,PUT, etc)</param>
        /// <param name="normalizedUrl">The normalized URL.</param>
        /// <param name="normalizedRequestParameters">The normalized request parameters.</param>
        /// <returns>
        /// A base64 string of the hash value
        /// </returns>
        public string GenerateSignature(Uri url, string token, string tokenSecret, string httpMethod, out string normalizedUrl, out string normalizedRequestParameters)
        {
            return GenerateSignature(url, token, tokenSecret, string.Empty, httpMethod, SignatureTypes.HMACSHA1, out normalizedUrl, out normalizedRequestParameters);
        }

        /// <summary>
        /// Generates a signature using the specified signatureType
        /// </summary>
        /// <param name="url">The full url that needs to be signed including its non OAuth url parameters</param>
        /// <param name="token">The token, if available. If not available pass null or an empty string</param>
        /// <param name="tokenSecret">The token secret, if available. If not available pass null or an empty string</param>
        /// <param name="verifier">The verifier, if available. If not available pass null or an empty string</param>
        /// <param name="httpMethod">The http method used. Must be a valid HTTP method verb (POST,GET,PUT, etc)</param>
        /// <param name="signatureType">The type of signature to use</param>
        /// <param name="normalizedUrl">The normalized URL.</param>
        /// <param name="normalizedRequestParameters">The normalized request parameters.</param>
        /// <returns>
        /// A base64 string of the hash value
        /// </returns>
        public string GenerateSignature(Uri url, string token, string tokenSecret, string verifier, string httpMethod, SignatureTypes signatureType, out string normalizedUrl, out string normalizedRequestParameters)
        {
            normalizedUrl = null;
            normalizedRequestParameters = null;

            switch (signatureType)
            {
                case SignatureTypes.PLAINTEXT:
                    return UrlEncode(string.Format("{0}&{1}", AppKeySecret.Secret, tokenSecret));
                case SignatureTypes.HMACSHA1:
                    string baseString = GenerateSignatureBaseString(url, token, tokenSecret, verifier, httpMethod, HMACSHA1SignatureType, out normalizedUrl, out normalizedRequestParameters);

                    HMACSHA1 hmacsha1 = new HMACSHA1();
                    hmacsha1.Key = Encoding.ASCII.GetBytes(string.Format("{0}&{1}", UrlEncode(AppKeySecret.Secret), string.IsNullOrEmpty(tokenSecret) ? "" : UrlEncode(tokenSecret)));

                    return ComputeHash(baseString, hmacsha1);
                case SignatureTypes.RSASHA1:
                    throw new NotImplementedException();
                default:
                    throw new ArgumentException("Unknown signature type", "signatureType");
            }
        }

        /// <summary>
        /// Generate the timestamp for the signature        
        /// </summary>
        /// <returns></returns>
        private string GenerateTimeStamp()
        {
            // Default implementation of UNIX time of the current UTC time
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        /// <summary>
        /// Generate a nonce
        /// </summary>
        /// <returns></returns>
        private string GenerateNonce()
        {
            // Just a simple implementation of a random number between 123400 and 9999999
            return _random.Next(123400, 9999999).ToString();
        }
    }
}
