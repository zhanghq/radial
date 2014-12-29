using Radial.Net;
using Radial.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Radial.Web.Mvc.WebApi
{
    /// <summary>
    /// ControllerExtensions
    /// </summary>
    public static class ControllerExtensions
    {

        /// <summary>
        /// Writes the status code.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        public static HttpResponseMessage WriteStatusCode(this ApiController c, HttpStatusCode? code = HttpStatusCode.OK)
        {
            var resp = new HttpResponseMessage(code.Value);
            return resp;
        }

        /// <summary>
        /// Writes the plain text.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <param name="text">The text.</param>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        public static HttpResponseMessage WritePlainText(this ApiController c, string text, HttpStatusCode? code = HttpStatusCode.OK)
        {
            var resp = new HttpResponseMessage(code.Value);
            resp.Content = new StringContent(text, StaticVariables.Encoding, ContentTypes.PlainText);
            return resp;
        }



        /// <summary>
        /// Writes the HTML.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <param name="html">The HTML.</param>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        public static HttpResponseMessage WriteHtml(this ApiController c, string html, HttpStatusCode? code = HttpStatusCode.OK)
        {
            var resp = new HttpResponseMessage(code.Value);
            resp.Content = new StringContent(html, StaticVariables.Encoding, ContentTypes.Html);
            return resp;
        }

        /// <summary>
        /// Writes the json.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <param name="obj">The object.</param>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        public static HttpResponseMessage WriteJson(this ApiController c, object obj, HttpStatusCode? code = HttpStatusCode.OK)
        {
            return WriteJson(c, JsonSerializer.Serialize(obj), code);
        }

        /// <summary>
        /// Writes the json.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <param name="json">The json.</param>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        public static HttpResponseMessage WriteJson(this ApiController c, string json, HttpStatusCode? code = HttpStatusCode.OK)
        {
            var resp = new HttpResponseMessage(code.Value);
            resp.Content = new StringContent(json, StaticVariables.Encoding, ContentTypes.Json);
            return resp;
        }


        /// <summary>
        /// Writes the XML.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <param name="obj">The object.</param>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        public static HttpResponseMessage WriteXml(this ApiController c, object obj, HttpStatusCode? code = HttpStatusCode.OK)
        {
            return WriteJson(c, XmlSerializer.Serialize(obj), code);
        }

        /// <summary>
        /// Writes the XML.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <param name="xml">The XML.</param>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        public static HttpResponseMessage WriteXml(this ApiController c, string xml, HttpStatusCode? code = HttpStatusCode.OK)
        {
            var resp = new HttpResponseMessage(code.Value);
            resp.Content = new StringContent(xml, StaticVariables.Encoding, ContentTypes.Xml);
            return resp;
        }

        /// <summary>
        /// Writes the stream.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <param name="bytes">The bytes.</param>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        public static HttpResponseMessage WriteStream(this ApiController c, byte[] bytes, HttpStatusCode? code = HttpStatusCode.OK)
        {
            var resp = new HttpResponseMessage(code.Value);
            resp.Content = new StreamContent(new MemoryStream(bytes));
            return resp;
        }

        /// <summary>
        /// Writes the stream.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        public static HttpResponseMessage WriteStream(this ApiController c, Stream stream, HttpStatusCode? code = HttpStatusCode.OK)
        {
            var resp = new HttpResponseMessage(code.Value);
            resp.Content = new StreamContent(stream);
            return resp;
        }

        /// <summary>
        /// Writes the stream.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <param name="obj">The object.</param>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        public static HttpResponseMessage WriteStream(this ApiController c, object obj, HttpStatusCode? code = HttpStatusCode.OK)
        {
            return WriteStream(c, Radial.Serialization.BinarySerializer.Serialize(obj), code);
        }
    }
}
