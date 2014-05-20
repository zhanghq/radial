using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Radial.Net;
using Radial.Serialization;

namespace Radial.Web.Mvc
{
    /// <summary>
    /// NewJsonResult
    /// </summary>
    public class NewJsonResult : ActionResult
    {
        object _data;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewJsonResult" /> class.
        /// </summary>
        /// <param name="data">The data.</param>
        public NewJsonResult(object data)
        {
            _data = data;
        }

        /// <summary>
        /// Enables processing of the result of an action method by a custom type that inherits from the <see cref="T:System.Web.Mvc.ActionResult"/> class.
        /// </summary>
        /// <param name="context">The context in which the result is executed. The context information includes the controller, HTTP content, request context, and route data.</param>
        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.Clear();
            context.HttpContext.Response.ContentType = ContentTypes.Json;
            context.HttpContext.Response.ContentEncoding = StaticVariables.Encoding;

            context.HttpContext.Response.Write(JsonSerializer.Serialize(_data));

            context.HttpContext.ApplicationInstance.CompleteRequest();
        }
    }
}
