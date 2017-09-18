using System.Web.Mvc;
using Radial.Net;
using Radial.Serialization;

namespace Radial.Web.Mvc
{
    /// <summary>
    /// The standard JSON output result.
    /// </summary>
    public class StdJsonResult : ActionResult
    {
        StdJsonOutput _obj;
        string _contentType;

        /// <summary>
        /// Initializes a new instance of the <see cref="StdJsonResult" /> class.
        /// </summary>
        /// <param name="obj">The standard json ouput object.</param>
        public StdJsonResult(StdJsonOutput obj) : this(obj, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="StdJsonResult" /> class.
        /// </summary>
        /// <param name="obj">The standard json ouput object.</param>
        /// <param name="contentType">Type of the content.</param>
        public StdJsonResult(StdJsonOutput obj, string contentType)
        {
            Checker.Parameter(obj != null, "the standard json ouput object can not be null");
            _obj = obj;

            if (string.IsNullOrWhiteSpace(contentType))
                _contentType = ContentTypes.Json;
            else
                _contentType = contentType.Trim();
        }

        /// <summary>
        /// Enables processing of the result of an action method by a custom type that inherits from the <see cref="T:System.Web.Mvc.ActionResult"/> class.
        /// </summary>
        /// <param name="context">The context in which the result is executed. The context information includes the controller, HTTP content, request context, and route data.</param>
        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.Clear();
            context.HttpContext.Response.ContentType = _contentType;
            context.HttpContext.Response.ContentEncoding = GlobalVariables.Encoding;

            context.HttpContext.Response.Write(_obj.ToJson());

            context.HttpContext.ApplicationInstance.CompleteRequest();
        }
    }
}
