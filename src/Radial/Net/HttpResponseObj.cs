using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;

namespace Radial.Net
{
    /// <summary>
    /// The wrapper object of HTTP web response.
    /// </summary>
    public sealed class HttpResponseObj
    {
        HttpStatusCode _code;
        WebHeaderCollection _headers;
        CookieCollection _cookies;
        string _server;
        string _characterSet;
        string _contentType;
        string _contentEncoding;
        long _contentLength;
        byte[] _rawData;
        string _text;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpResponseObj"/> class.
        /// </summary>
        /// <param name="webResp">The HttpWebResponse object.</param>
        public HttpResponseObj(HttpWebResponse webResp)
        {
            Checker.Parameter(webResp != null, "HttpWebResponse object can not be null");
            _code = webResp.StatusCode;
            _headers = webResp.Headers;
            _cookies = webResp.Cookies;
            _characterSet = webResp.CharacterSet;
            _server = webResp.Server;
            _contentType = webResp.ContentType;
            _contentEncoding = webResp.ContentEncoding;
            _contentLength = webResp.ContentLength;

            using (Stream respStream = webResp.GetResponseStream())
            {
                List<byte> blist = new List<byte>();
                int b = 0;
                while ((b = respStream.ReadByte()) > -1)
                {
                    blist.Add((byte)b);
                }

                _rawData = blist.ToArray();

                using (MemoryStream ms = new MemoryStream(_rawData))
                {
                    ms.Position = 0;
                    Encoding encoding = null;

                    try
                    {
                        if (!string.IsNullOrWhiteSpace(_characterSet))
                            encoding = Encoding.GetEncoding(_characterSet);
                        if (encoding == null && !string.IsNullOrWhiteSpace(_contentEncoding))
                            encoding = Encoding.GetEncoding(_contentEncoding);
                    }
                    catch (Exception ex)
                    {
                        Logger.Warn(ex);
                    }

                    StreamReader sr = null;
                    if (encoding != null)
                        sr = new StreamReader(ms, encoding);
                    else
                        sr = new StreamReader(ms);

                    _text = sr.ReadToEnd();

                    sr.Dispose();
                }
            }
        }

        /// <summary>
        /// Gets the response status code.
        /// </summary>
        public HttpStatusCode Code
        {
            get
            {
                return _code;
            }
        }

        /// <summary>
        /// Gets the response raw data.
        /// </summary>
        public byte[] RawData
        {
            get
            {
                return _rawData;
            }
        }

        /// <summary>
        /// Gets the response character set.
        /// </summary>
        public string CharacterSet
        {
            get
            {
                return _characterSet;
            }
        }

        /// <summary>
        /// Gets the response text.
        /// </summary>
        public string Text
        {
            get
            {
                return _text;
            }
        }

        /// <summary>
        /// Gets the response headers.
        /// </summary>
        public WebHeaderCollection Headers
        {
            get
            {
                return _headers;
            }
        }

        /// <summary>
        /// Gets the cookies.
        /// </summary>
        public CookieCollection Cookies
        {
            get
            {
                return _cookies;
            }
        }

        /// <summary>
        /// Gets the server.
        /// </summary>
        public string Server
        {
            get
            {
                return _server;
            }
        }

        /// <summary>
        /// Gets the type of the content.
        /// </summary>
        public string ContentType
        {
            get
            {
                return _contentType;
            }
        }

        /// <summary>
        /// Gets the content encoding.
        /// </summary>
        public string ContentEncoding
        {
            get { return _contentEncoding; }
        }

        /// <summary>
        /// Gets the length of the content.
        /// </summary>
        public long ContentLength
        {
            get { return _contentLength; }
        }
    }
}
