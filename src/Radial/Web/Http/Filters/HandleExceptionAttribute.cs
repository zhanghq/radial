using System;
using Radial.Net;
using System.Web.Http.Filters;
using System.Net.Http;
using System.Net;

namespace Radial.Web.Http.Filters
{
    /// <summary>
    /// Represents an attribute that is used to handle and log an exception that is thrown by an action method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class HandleExceptionAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HandleExceptionAttribute" /> class.
        /// </summary>
        /// <param name="defaultErrorCode">The default error code, if unknown exception occurs.</param>
        /// <param name="defaultErrorMessage">The default error message, if unknown exception occurs.</param>
        /// <param name="httpStatusCode">The HTTP status code.</param>
        public HandleExceptionAttribute(int defaultErrorCode = 500, string defaultErrorMessage = null, HttpStatusCode httpStatusCode = HttpStatusCode.OK)
        {
            DefaultErrorCode = defaultErrorCode;
            HttpStatusCode = HttpStatusCode;

            if (!string.IsNullOrWhiteSpace(defaultErrorMessage))
                DefaultErrorMessage = defaultErrorMessage.Trim();
        }

        /// <summary>
        /// Gets the HTTP status code.
        /// </summary>
        /// <value>
        /// The HTTP status code.
        /// </value>
        public HttpStatusCode HttpStatusCode
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the default error code.
        /// </summary>
        public int DefaultErrorCode
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the default error message.
        /// </summary>
        public string DefaultErrorMessage
        {
            get;
            private set;
        }

        /// <summary>
        /// Called when an exception occurs.
        /// </summary>
        /// <param name="actionExecutedContext">The HttpActionExecutedContext object.</param>
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            Logger.Fatal(actionExecutedContext.Exception);


            KnownFaultException hkfe = actionExecutedContext.Exception as KnownFaultException;

            var stdJsonOutput = new StdJsonOutput();
            stdJsonOutput.Code = hkfe != null ? hkfe.ErrorCode : DefaultErrorCode;
            stdJsonOutput.Message = hkfe != null ? hkfe.Message : (string.IsNullOrWhiteSpace(DefaultErrorMessage) ? actionExecutedContext.Exception.Message : DefaultErrorMessage);

            HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode);

            resp.Content = new StringContent(stdJsonOutput.ToJson(), GlobalVariables.Encoding, ContentTypes.Json);

            actionExecutedContext.Response = resp;
        }
    }
}