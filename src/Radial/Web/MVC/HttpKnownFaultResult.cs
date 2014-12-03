using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Net;

namespace Radial.Web.Mvc
{
    /// <summary>
    /// Throw a new HttpKnownFaultException and let the system itself to decide how to deal with.
    /// </summary>
    public class HttpKnownFaultResult : ActionResult
    {
        int _errorCode;
        string _message;
        Exception _innerException;
        HttpStatusCode? _httpStatusCode;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpKnownFaultResult"/> class.
        /// </summary>
        /// <param name="errorCode">The error code.</param>
        /// <param name="message">The message.</param>
        /// <param name="httpStatusCode">The HTTP status code.</param>
        public HttpKnownFaultResult(int errorCode, string message, HttpStatusCode? httpStatusCode)
            : this(errorCode, message, null, httpStatusCode)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpKnownFaultResult"/> class.
        /// </summary>
        /// <param name="errorCode">The error code.</param>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="httpStatusCode">The HTTP status code.</param>
        public HttpKnownFaultResult(int errorCode, string message, Exception innerException, HttpStatusCode? httpStatusCode)
        {
            _errorCode = errorCode;
            _message = message;
            _innerException = innerException;
            _httpStatusCode = httpStatusCode;
        }


        /// <summary>
        /// Enables processing of the result of an action method by a custom type that inherits from the <see cref="T:System.Web.Mvc.ActionResult"/> class.
        /// </summary>
        /// <param name="context">The context in which the result is executed. The context information includes the controller, HTTP content, request context, and route data.</param>
        public override void ExecuteResult(ControllerContext context)
        {
            throw new HttpKnownFaultException(_errorCode, _message, _innerException, _httpStatusCode);
        }
    }
}
