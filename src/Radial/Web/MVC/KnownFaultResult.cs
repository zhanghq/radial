using System;
using System.Web.Mvc;

namespace Radial.Web.Mvc
{
    /// <summary>
    /// Throw a new KnownFaultException and let the system itself to decide how to deal with.
    /// </summary>
    public class KnownFaultResult : ActionResult
    {
        int _errorCode;
        string _message;
        Exception _innerException;

        /// <summary>
        /// Initializes a new instance of the <see cref="KnownFaultResult" /> class.
        /// </summary>
        /// <param name="errorCode">The error code.</param>
        /// <param name="message">The message.</param>
        public KnownFaultResult(int errorCode, string message)
            : this(errorCode, message, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KnownFaultResult" /> class.
        /// </summary>
        /// <param name="errorCode">The error code.</param>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public KnownFaultResult(int errorCode, string message, Exception innerException)
        {
            _errorCode = errorCode;
            _message = message;
            _innerException = innerException;
        }


        /// <summary>
        /// Enables processing of the result of an action method by a custom type that inherits from the <see cref="T:System.Web.Mvc.ActionResult"/> class.
        /// </summary>
        /// <param name="context">The context in which the result is executed. The context information includes the controller, HTTP content, request context, and route data.</param>
        public override void ExecuteResult(ControllerContext context)
        {
            throw new KnownFaultException(_errorCode, _message, _innerException);
        }
    }
}
