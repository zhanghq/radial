using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Radial.Net;

namespace Radial.Web.Mvc
{
    /// <summary>
    /// Renders xml to the response.
    /// </summary>
    public class XmlResult : ActionResult
    {
        string _xml;

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlResult"/> class.
        /// </summary>
        /// <param name="xml">The xml.</param>
        public XmlResult(string xml)
        {
            if (xml == null)
                xml = "<?xml version=\"1.0\"?>";

            _xml = xml;
        }

        /// <summary>
        /// Enables processing of the result of an action method by a custom type that inherits from the <see cref="T:System.Web.Mvc.ActionResult"/> class.
        /// </summary>
        /// <param name="context">The context in which the result is executed. The context information includes the controller, HTTP content, request context, and route data.</param>
        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.ContentType = ContentTypes.Xml;
            context.HttpContext.Response.ContentEncoding = StaticVariables.Encoding;
            context.HttpContext.Response.Write(_xml);
        }
    }
}
