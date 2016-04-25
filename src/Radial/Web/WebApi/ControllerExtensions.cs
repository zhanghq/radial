using Radial.Net;
using Radial.Serialization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Radial.Web.WebApi
{
    /// <summary>
    /// ControllerExtensions
    /// </summary>
    public static class ControllerExtensions
    {

        /// <summary>
        /// Writes the status code to HttpResponseMessage.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        public static HttpResponseMessage StatusCode(this ApiController c, HttpStatusCode code = HttpStatusCode.OK)
        {
            var resp = new HttpResponseMessage(code);
            return resp;
        }

        /// <summary>
        /// Writes the plain text to HttpResponseMessage.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <param name="text">The text.</param>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        public static HttpResponseMessage PlainText(this ApiController c, string text, HttpStatusCode code = HttpStatusCode.OK)
        {
            if (string.IsNullOrWhiteSpace(text))
                text = string.Empty;

            var resp = new HttpResponseMessage(code);
            resp.Content = new StringContent(text, GlobalVariables.Encoding, ContentTypes.PlainText);
            return resp;
        }



        /// <summary>
        /// Writes the HTML to HttpResponseMessage.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <param name="html">The HTML.</param>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        public static HttpResponseMessage Html(this ApiController c, string html, HttpStatusCode code = HttpStatusCode.OK)
        {
            if (string.IsNullOrWhiteSpace(html))
                html = string.Empty;

            var resp = new HttpResponseMessage(code);
            resp.Content = new StringContent(html, GlobalVariables.Encoding, ContentTypes.Html);
            return resp;
        }

        /// <summary>
        /// Writes the json to HttpResponseMessage.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <param name="obj">The object.</param>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        public static HttpResponseMessage NewJson(this ApiController c, object obj, HttpStatusCode code = HttpStatusCode.OK)
        {
            return NewJson(c, JsonSerializer.Serialize(obj), code);
        }

        /// <summary>
        /// Writes the json to HttpResponseMessage.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <param name="json">The json.</param>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        public static HttpResponseMessage NewJson(this ApiController c, string json, HttpStatusCode code = HttpStatusCode.OK)
        {
            if (string.IsNullOrWhiteSpace(json))
                json = string.Empty;

            var resp = new HttpResponseMessage(code);
            resp.Content = new StringContent(json, GlobalVariables.Encoding, ContentTypes.Json);
            return resp;
        }


        /// <summary>
        /// Writes the XML to HttpResponseMessage.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <param name="obj">The object.</param>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        public static HttpResponseMessage Xml(this ApiController c, object obj, HttpStatusCode code = HttpStatusCode.OK)
        {
            return Xml(c, XmlSerializer.Serialize(obj), code);
        }

        /// <summary>
        /// Writes the XML to HttpResponseMessage.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <param name="xml">The XML.</param>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        public static HttpResponseMessage Xml(this ApiController c, string xml, HttpStatusCode code = HttpStatusCode.OK)
        {
            if (string.IsNullOrWhiteSpace(xml))
                xml = string.Empty;

            var resp = new HttpResponseMessage(code);
            resp.Content = new StringContent(xml, GlobalVariables.Encoding, ContentTypes.Xml);
            return resp;
        }

        /// <summary>
        /// Writes the stream to HttpResponseMessage.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <param name="bytes">The bytes.</param>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        public static HttpResponseMessage Stream(this ApiController c, byte[] bytes, HttpStatusCode code = HttpStatusCode.OK)
        {
            var resp = new HttpResponseMessage(code);
            resp.Content = new StreamContent(new MemoryStream(bytes));
            return resp;
        }

        /// <summary>
        /// Writes the stream to HttpResponseMessage.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        public static HttpResponseMessage Stream(this ApiController c, Stream stream, HttpStatusCode code = HttpStatusCode.OK)
        {
            var resp = new HttpResponseMessage(code);
            resp.Content = new StreamContent(stream);
            return resp;
        }

        /// <summary>
        /// Writes the stream to HttpResponseMessage.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <param name="obj">The object.</param>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        public static HttpResponseMessage Stream(this ApiController c, object obj, HttpStatusCode code = HttpStatusCode.OK)
        {
            return Stream(c, BinarySerializer.Serialize(obj), code);
        }
    }
}
