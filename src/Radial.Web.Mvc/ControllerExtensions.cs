using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Radial.Serialization;
using System.Data;
using System.Net;

namespace Radial.Web.Mvc
{
    /// <summary>
    /// Controller extension class.
    /// </summary>
    public static class ControllerExtensions
    {

        /// <summary>
        /// Popup javascript alert window.
        /// </summary>
        /// <param name="c">The controller.</param>
        /// <param name="message">The message.</param>
        /// <returns>AlertResult instance.</returns>
        public static AlertResult Alert(this Controller c, string message)
        {
            return Alert(c, message, string.Empty);
        }

        /// <summary>
        /// Popup javascript alert window.
        /// </summary>
        /// <param name="c">The controller.</param>
        /// <param name="message">The message.</param>
        /// <param name="redirect">The redirect.</param>
        /// <returns>AlertResult instance.</returns>
        public static AlertResult Alert(this Controller c, string message, string redirect)
        {
            return new AlertResult(message, redirect);
        }

        /// <summary>
        /// Renders image to the response.
        /// </summary>
        /// <param name="c">The controllerontroller.</param>
        /// <param name="imageStream">The image stream.</param>
        /// <param name="format">The image format.</param>
        /// <returns>ImageResult instance.</returns>
        public static ImageResult Image(this Controller c, Stream imageStream, ImageFormat format)
        {
            return new ImageResult(System.Drawing.Image.FromStream(imageStream), format);
        }

        /// <summary>
        /// Renders image to the response.
        /// </summary>
        /// <param name="c">The controllerontroller.</param>
        /// <param name="imageBytes">The image bytes.</param>
        /// <param name="format">The image format.</param>
        /// <returns>ImageResult instance.</returns>
        public static ImageResult Image(this Controller c, byte[] imageBytes, ImageFormat format)
        {
            return new ImageResult(System.Drawing.Image.FromStream(new MemoryStream(imageBytes)), format);
        }

        /// <summary>
        /// Renders image to the response.
        /// </summary>
        /// <param name="c">The controllerontroller.</param>
        /// <param name="image">The image.</param>
        /// <param name="format">The image format.</param>
        /// <returns>ImageResult instance.</returns>
        public static ImageResult Image(this Controller c, Image image, ImageFormat format)
        {
            return new ImageResult(image, format);
        }

        /// <summary>
        /// Renders xml to the response.
        /// </summary>
        /// <param name="c">The controllerontroller.</param>
        /// <param name="xml">The xml.</param>
        /// <returns>
        /// XmlResult instance.
        /// </returns>
        public static XmlResult Xml(this Controller c, string xml)
        {
            return new XmlResult(xml);
        }

        /// <summary>
        /// Renders xml to the response.
        /// </summary>
        /// <param name="c">The controllerontroller.</param>
        /// <param name="xml">The xml.</param>
        /// <param name="encoding">The xml encoding.</param>
        /// <returns>
        /// XmlResult instance.
        /// </returns>
        public static XmlResult Xml(this Controller c, string xml, Encoding encoding)
        {
            return new XmlResult(xml, encoding);
        }

        /// <summary>
        /// Renders json to the response.
        /// </summary>
        /// <param name="c">The controller.</param>
        /// <param name="data">The data.</param>
        /// <returns>NewJsonResult instance.</returns>
        public static NewJsonResult NewJson(this Controller c, object data)
        {
            return new NewJsonResult(data);
        }

        /// <summary>
        /// Renders json to the response.
        /// </summary>
        /// <param name="c">The controller.</param>
        /// <param name="data">The data.</param>
        /// <param name="contentType">The content type.</param>
        /// <returns>NewJsonResult instance.</returns>
        public static NewJsonResult NewJson(this Controller c, object data, string contentType)
        {
            return new NewJsonResult(data, contentType);
        }



        /// <summary>
        /// Renders json to the response.
        /// </summary>
        /// <param name="c">The controller.</param>
        /// <param name="data">The data.</param>
        /// <param name="contentType">The content type.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>
        /// JsonResult instance.
        /// </returns>
        public static NewJsonResult NewJson(this Controller c, object data, string contentType, Encoding encoding)
        {
            return new NewJsonResult(data, contentType, encoding);
        }

        /// <summary>
        /// Renders Excel file to the response.
        /// </summary>
        /// <param name="c">The controller.</param>
        /// <param name="dt">The DataTable.</param>
        /// <param name="fileName">The file name(without extension).</param>
        /// <returns>ExcelResult instance.</returns>
        public static ExcelResult Excel(this Controller c, DataTable dt, string fileName)
        {
            return new ExcelResult(dt, fileName);
        }

        /// <summary>
        /// Throws a new KnownFaultException and let the system itself to decide how to deal with.
        /// </summary>
        /// <param name="c">The controller.</param>
        /// <param name="errorCode">The error code.</param>
        /// <param name="message">The message.</param>
        /// <returns>ThrowKnownFaultResult instance.</returns>
        public static HttpKnownFaultResult KnownFault(this Controller c, int errorCode, string message)
        {
            return KnownFault(c, errorCode, message, null, null);
        }

        /// <summary>
        /// Throws a new KnownFaultException and let the system itself to decide how to deal with.
        /// </summary>
        /// <param name="c">The controller.</param>
        /// <param name="errorCode">The error code.</param>
        /// <param name="message">The message.</param>
        /// <param name="httpStatusCode">The HTTP status code.</param>
        /// <returns>ThrowKnownFaultResult instance.</returns>
        public static HttpKnownFaultResult KnownFault(this Controller c, int errorCode, string message, HttpStatusCode? httpStatusCode)
        {
            return KnownFault(c, errorCode, message, null, httpStatusCode);
        }

        /// <summary>
        /// Throws a new KnownFaultException and let the system itself to decide how to deal with.
        /// </summary>
        /// <param name="c">The controller.</param>
        /// <param name="errorCode">The error code.</param>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <returns>ThrowKnownFaultResult instance.</returns>
        public static HttpKnownFaultResult KnownFault(this Controller c, int errorCode, string message, Exception innerException)
        {
            return new HttpKnownFaultResult(errorCode, message, innerException, null);
        }

        /// <summary>
        /// Throws a new KnownFaultException and let the system itself to decide how to deal with.
        /// </summary>
        /// <param name="c">The controller.</param>
        /// <param name="errorCode">The error code.</param>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="httpStatusCode">The HTTP status code.</param>
        /// <returns>ThrowKnownFaultResult instance.</returns>
        public static HttpKnownFaultResult KnownFault(this Controller c, int errorCode, string message, Exception innerException, HttpStatusCode? httpStatusCode)
        {
            return new HttpKnownFaultResult(errorCode, message, innerException, httpStatusCode);
        }
    }
}
