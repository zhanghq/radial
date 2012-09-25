using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Radial.Web.Mvc
{
    /// <summary>
    /// Renders xml to the response.
    /// </summary>
    public class XmlResult : ActionResult
    {
        string _xml;
        Encoding _encoding;

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlResult"/> class.
        /// </summary>
        /// <param name="xml">The xml.</param>
        public XmlResult(string xml) : this(xml, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlResult"/> class.
        /// </summary>
        /// <param name="xml">The xml.</param>
        /// <param name="encoding">The xml encoding.</param>
        public XmlResult(string xml, Encoding encoding)
        {
            if (xml == null)
                xml = "<?xml version=\"1.0\"?>";
            if (encoding == null)
                encoding = Encoding.UTF8;

            _xml = xml;
            _encoding = encoding;
        }

        /// <summary>
        /// Enables processing of the result of an action method by a custom type that inherits from the <see cref="T:System.Web.Mvc.ActionResult"/> class.
        /// </summary>
        /// <param name="context">The context in which the result is executed. The context information includes the controller, HTTP content, request context, and route data.</param>
        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.ContentType = ContentTypes.Xml;
            context.HttpContext.Response.ContentEncoding = _encoding;
            context.HttpContext.Response.Write(_xml);
        }
    }
}
