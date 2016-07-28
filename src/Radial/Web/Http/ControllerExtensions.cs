using Radial.Net;
using Radial.Serialization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Radial.Web.Http
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
        /// <param name="httpCode">The http code.</param>
        /// <returns></returns>
        public static HttpResponseMessage StatusCode(this ApiController c, HttpStatusCode httpCode = HttpStatusCode.OK)
        {
            var resp = new HttpResponseMessage(httpCode);
            return resp;
        }

        /// <summary>
        /// Writes the plain text to HttpResponseMessage.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <param name="text">The text.</param>
        /// <param name="httpCode">The http code.</param>
        /// <returns></returns>
        public static HttpResponseMessage PlainText(this ApiController c, string text, HttpStatusCode httpCode = HttpStatusCode.OK)
        {
            if (string.IsNullOrWhiteSpace(text))
                text = string.Empty;

            var resp = new HttpResponseMessage(httpCode);
            resp.Content = new StringContent(text, GlobalVariables.Encoding, ContentTypes.PlainText);
            return resp;
        }



        /// <summary>
        /// Writes the HTML to HttpResponseMessage.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <param name="html">The HTML.</param>
        /// <param name="httpCode">The http code.</param>
        /// <returns></returns>
        public static HttpResponseMessage Html(this ApiController c, string html, HttpStatusCode httpCode = HttpStatusCode.OK)
        {
            if (string.IsNullOrWhiteSpace(html))
                html = string.Empty;

            var resp = new HttpResponseMessage(httpCode);
            resp.Content = new StringContent(html, GlobalVariables.Encoding, ContentTypes.Html);
            return resp;
        }

        /// <summary>
        /// Writes the json to HttpResponseMessage.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <param name="obj">The object.</param>
        /// <param name="httpCode">The http code.</param>
        /// <returns></returns>
        public static HttpResponseMessage NewJson(this ApiController c, object obj, HttpStatusCode httpCode = HttpStatusCode.OK)
        {
            return NewJson(c, JsonSerializer.Serialize(obj), httpCode);
        }

        /// <summary>
        /// Writes the json to HttpResponseMessage.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <param name="json">The json.</param>
        /// <param name="httpCode">The http code.</param>
        /// <returns></returns>
        public static HttpResponseMessage NewJson(this ApiController c, string json, HttpStatusCode httpCode = HttpStatusCode.OK)
        {
            if (string.IsNullOrWhiteSpace(json))
                json = string.Empty;

            var resp = new HttpResponseMessage(httpCode);
            resp.Content = new StringContent(json, GlobalVariables.Encoding, ContentTypes.Json);
            return resp;
        }

        /// <summary>
        /// Writes the standard Error json to HttpResponseMessage.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <param name="errorCode">The error code.</param>
        /// <param name="message">The message.</param>
        /// <param name="httpCode">The http code.</param>
        /// <returns></returns>
        public static HttpResponseMessage StdErrorJson(this ApiController c, int errorCode = -9999, string message = null, HttpStatusCode httpCode = HttpStatusCode.InternalServerError)
        {
            var resp = new HttpResponseMessage(httpCode);
            resp.Content = new StringContent((new StdJsonOutput { Error = errorCode, Message = message }).ToJson(),
                GlobalVariables.Encoding, ContentTypes.Json);
            return resp;
        }

        /// <summary>
        /// Writes the standard Success json to HttpResponseMessage.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <param name="payload">The payload data.</param>
        /// <param name="message">The message.</param>
        /// <param name="httpCode">The http code.</param>
        /// <returns></returns>
        public static HttpResponseMessage StdSuccessJson(this ApiController c, object payload = null, string message = null, HttpStatusCode httpCode = HttpStatusCode.OK)
        {
            var resp = new HttpResponseMessage(httpCode);
            resp.Content = new StringContent((new StdJsonOutput { Payload= payload, Message = message }).ToJson(),
                GlobalVariables.Encoding, ContentTypes.Json);
            return resp;
        }

        /// <summary>
        /// Writes the XML to HttpResponseMessage.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <param name="obj">The object.</param>
        /// <param name="httpCode">The http code.</param>
        /// <returns></returns>
        public static HttpResponseMessage Xml(this ApiController c, object obj, HttpStatusCode httpCode = HttpStatusCode.OK)
        {
            return Xml(c, XmlSerializer.Serialize(obj), httpCode);
        }

        /// <summary>
        /// Writes the XML to HttpResponseMessage.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <param name="xml">The XML.</param>
        /// <param name="httpCode">The http code.</param>
        /// <returns></returns>
        public static HttpResponseMessage Xml(this ApiController c, string xml, HttpStatusCode httpCode = HttpStatusCode.OK)
        {
            if (string.IsNullOrWhiteSpace(xml))
                xml = string.Empty;

            var resp = new HttpResponseMessage(httpCode);
            resp.Content = new StringContent(xml, GlobalVariables.Encoding, ContentTypes.Xml);
            return resp;
        }

        /// <summary>
        /// Writes the stream to HttpResponseMessage.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <param name="bytes">The bytes.</param>
        /// <param name="httpCode">The http code.</param>
        /// <returns></returns>
        public static HttpResponseMessage Stream(this ApiController c, byte[] bytes, HttpStatusCode httpCode = HttpStatusCode.OK)
        {
            var resp = new HttpResponseMessage(httpCode);
            resp.Content = new StreamContent(new MemoryStream(bytes));
            return resp;
        }

        /// <summary>
        /// Writes the stream to HttpResponseMessage.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="httpCode">The http code.</param>
        /// <returns></returns>
        public static HttpResponseMessage Stream(this ApiController c, Stream stream, HttpStatusCode httpCode = HttpStatusCode.OK)
        {
            var resp = new HttpResponseMessage(httpCode);
            resp.Content = new StreamContent(stream);
            return resp;
        }

        /// <summary>
        /// Writes the stream to HttpResponseMessage.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <param name="obj">The object.</param>
        /// <param name="httpCode">The http code.</param>
        /// <returns></returns>
        public static HttpResponseMessage Stream(this ApiController c, object obj, HttpStatusCode httpCode = HttpStatusCode.OK)
        {
            return Stream(c, BinarySerializer.Serialize(obj), httpCode);
        }
    }
}
