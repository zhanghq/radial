using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Radial.Serialization;

namespace Radial.Web.Mvc
{
    /// <summary>
    /// NewJsonResult
    /// </summary>
    public class NewJsonResult : ActionResult
    {
        object _data;
        Encoding _encoding;
        string _contentType;


        /// <summary>
        /// Initializes a new instance of the <see cref="NewJsonResult"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        public NewJsonResult(object data)
            : this(data, ContentTypes.Json, Encoding.UTF8)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="NewJsonResult"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="contentType">The content type.</param>
        public NewJsonResult(object data, string contentType)
            : this(data,contentType,Encoding.UTF8)
        { }



        /// <summary>
        /// Initializes a new instance of the <see cref="NewJsonResult"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="encoding">The encoding.</param>
        /// <param name="contentType">The content type.</param>
        public NewJsonResult(object data, string contentType,Encoding encoding)
        {
            _data = data;
            if (encoding != null)
                _encoding = encoding;
            else
                _encoding = Encoding.UTF8;
            _contentType = contentType;
        }

        /// <summary>
        /// Enables processing of the result of an action method by a custom type that inherits from the <see cref="T:System.Web.Mvc.ActionResult"/> class.
        /// </summary>
        /// <param name="context">The context in which the result is executed. The context information includes the controller, HTTP content, request context, and route data.</param>
        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.Clear();
            context.HttpContext.Response.ContentType = _contentType;
            context.HttpContext.Response.ContentEncoding = _encoding;

            context.HttpContext.Response.Write(JsonSerializer.Serialize(_data));

            context.HttpContext.ApplicationInstance.CompleteRequest();
        }
    }
}
