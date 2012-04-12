using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Radial.Web.Mvc.Models;

namespace Radial.Web.Mvc
{
    /// <summary>
    /// Throw a new KnownFaultException and let the system itself to decide how to deal with.
    /// </summary>
    public class KnownFaultResult : ActionResult
    {
        KnownFaultModel _model;

        /// <summary>
        /// Initializes a new instance of the <see cref="KnownFaultResult"/> class.
        /// </summary>
        /// <param name="errorCode">The error code.</param>
        /// <param name="message">The message.</param>
        public KnownFaultResult(int errorCode, string message)
            : this(errorCode, message, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KnownFaultResult"/> class.
        /// </summary>
        /// <param name="errorCode">The error code.</param>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public KnownFaultResult(int errorCode, string message, Exception innerException)
            : this(new KnownFaultModel
                {
                    ErrorCode = errorCode,
                    Message = message,
                    InnerException = innerException
                })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KnownFaultResult"/> class.
        /// </summary>
        /// <param name="model">The known fault model.</param>
        public KnownFaultResult(KnownFaultModel model)
        {
            Checker.Parameter(model != null, "known fault model can not be null");
            _model = model;
        }

        /// <summary>
        /// Enables processing of the result of an action method by a custom type that inherits from the <see cref="T:System.Web.Mvc.ActionResult"/> class.
        /// </summary>
        /// <param name="context">The context in which the result is executed. The context information includes the controller, HTTP content, request context, and route data.</param>
        public override void ExecuteResult(ControllerContext context)
        {
            throw new KnownFaultException(_model.ErrorCode, _model.Message, _model.InnerException);
        }
    }
}
