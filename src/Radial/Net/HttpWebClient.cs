using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Collections.Specialized;

namespace Radial.Net
{
    /// <summary>
    /// Contains HTTP web request methods.
    /// </summary>
    public static class HttpWebClient
    {

        /// <summary>
        /// Http Get method
        /// </summary>
        /// <param name="url">The request url(include query string).</param>
        /// <returns>
        /// The HttpResponseObj instance(never null).
        /// </returns>
        public static HttpResponseObj Get(string url)
        {
            return Get((HttpWebRequest)HttpWebRequest.Create(url));
        }

        /// <summary>
        /// Http Get method
        /// </summary>
        /// <param name="request">The request(include query string).</param>
        /// <returns>
        /// The HttpResponseObj instance(never null).
        /// </returns>
        public static HttpResponseObj Get(HttpWebRequest request)
        {
            Checker.Parameter(request != null, "request can not be null");

            request.Method = WebRequestMethods.Http.Get;
            request.KeepAlive = false;
            HttpWebResponse resp = null;

            try
            {
                resp = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException e)
            {
                resp = e.Response as HttpWebResponse;

                Checker.Requires(resp != null, e.Message);
            }

            return new HttpResponseObj(resp);
        }

        /// <summary>
        /// Http Post method use "application/x-www-form-urlencoded" content type.
        /// </summary>
        /// <param name="url">The request url(include query string).</param>
        /// <returns>
        /// The HttpResponseObj instance(never null).
        /// </returns>
        public static HttpResponseObj Post(string url)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(url), "url can not be null");

            Uri uri = new Uri(url);

            return Post((HttpWebRequest)HttpWebRequest.Create(uri.AbsoluteUri.Replace(uri.Query, string.Empty)), uri.Query.Trim('?', '&'));
        }

        /// <summary>
        /// Http Post method use "application/x-www-form-urlencoded" content type.
        /// </summary>
        /// <param name="request">The request(exclude query string).</param>
        /// <param name="queryString">The query string.</param>
        /// <returns>
        /// The HttpResponseObj instance(never null).
        /// </returns>
        public static HttpResponseObj Post(HttpWebRequest request,string queryString)
        {
            Checker.Parameter(request != null, "request can not be null");

            request.Method = WebRequestMethods.Http.Post;
            request.ContentType = "application/x-www-form-urlencoded";
            request.KeepAlive = false;
            HttpWebResponse resp = null;

            try
            {
                if (!string.IsNullOrWhiteSpace(queryString))
                {
                    using (StreamWriter sw = new StreamWriter(request.GetRequestStream()))
                    {
                        sw.Write(queryString);
                    }
                }
                resp = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException e)
            {
                resp = e.Response as HttpWebResponse;
                Checker.Requires(resp != null, e.Message);
            }

            return new HttpResponseObj(resp);
        }

        /// <summary>
        /// Http Post method use "multipart/form-data" content type.
        /// </summary>
        /// <param name="url">The request url(exclude query string).</param>
        /// <param name="postDatas">The post data array.</param>
        /// <returns>
        /// The HttpResponseObj instance(never null)
        /// </returns>
        public static HttpResponseObj Post(string url, IMultipartFormData[] postDatas)
        {
            return Post((HttpWebRequest)HttpWebRequest.Create(url), postDatas);
        }

        /// <summary>
        /// Http Post method use "multipart/form-data" content type.
        /// </summary>
        /// <param name="request">The request(exclude query string).</param>
        /// <param name="datas">The post data array.</param>
        /// <returns>
        /// The HttpResponseObj instance(never null)
        /// </returns>
        public static HttpResponseObj Post(HttpWebRequest request, IMultipartFormData[] datas)
        {
            Checker.Parameter(request != null, "request can not be null");

            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString();
            string separator = "--" + boundary + "\r\n";
            string footer = "--" + boundary + "--\r\n";
            byte[] separatorBytes = Encoding.UTF8.GetBytes(separator);
            byte[] footerBytes = Encoding.UTF8.GetBytes(footer);


            request.ContentType = "multipart/form-data; boundary=" + boundary;
            request.Method = WebRequestMethods.Http.Post;
            request.KeepAlive = false;
            HttpWebResponse resp = null;

            try
            {
                if (datas != null && datas.Length > 0)
                {
                    using (Stream stream = request.GetRequestStream())
                    {
                        stream.Write(separatorBytes, 0, separatorBytes.Length);
                        for (int i = 0; i < datas.Length; i++)
                        {
                            datas[i].Write(stream);
                            if (i < datas.Length - 1)
                                stream.Write(separatorBytes, 0, separatorBytes.Length);
                        }
                        stream.Write(footerBytes, 0, footerBytes.Length);
                    }
                }
                resp = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException e)
            {
                resp = e.Response as HttpWebResponse;
                Checker.Requires(resp != null, e.Message);
            }

            return new HttpResponseObj(resp);
        }
    }
}
