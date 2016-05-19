using System;
using Radial.Net;
using System.Web.Http.Filters;
using System.Net.Http;

namespace Radial.Web.WebApi.Filters
{
    /// <summary>
    /// Represents an attribute that is used to handle and log an exception that is thrown by an action method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class HandleExceptionAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HandleExceptionAttribute" /> class with ExceptionOutputStyle.System.
        /// </summary>
        public HandleExceptionAttribute()
            : this(ExceptionOutputStyle.System, 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HandleExceptionAttribute" /> class.
        /// </summary>
        /// <param name="outputStyle">The exception output style.</param>
        /// <param name="defaultErrorCode">The default error code, if unknown exception occurs.</param>
        /// <param name="defaultErrorMessage">The default error message, if unknown exception occurs.</param>
        public HandleExceptionAttribute(ExceptionOutputStyle outputStyle, int defaultErrorCode=-9999,
            string defaultErrorMessage = null)
        {
            OutputStyle = outputStyle;
            DefaultErrorCode = defaultErrorCode;
            if (!string.IsNullOrWhiteSpace(defaultErrorMessage))
                DefaultErrorMessage = defaultErrorMessage.Trim();
        }

        /// <summary>
        /// Gets the output style.
        /// </summary>
        public ExceptionOutputStyle OutputStyle
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
            Logger.Default.Fatal(actionExecutedContext.Exception);

            if (OutputStyle == ExceptionOutputStyle.System)
            {
                base.OnException(actionExecutedContext);
                return;
            }

            KnownFaultException hkfe = actionExecutedContext.Exception as KnownFaultException;

            ExceptionOutputData data = new ExceptionOutputData
            {
                ErrorCode = hkfe != null ? hkfe.ErrorCode : DefaultErrorCode,
                RequestUrl = HttpKits.MakeRelativeUrl(actionExecutedContext.Request.RequestUri.AbsoluteUri).Replace("~", string.Empty),
                ErrorMessage = hkfe != null ? hkfe.Message : DefaultErrorMessage
            };


            HttpResponseMessage resp = new HttpResponseMessage(GlobalVariables.WebExceptionHttpStatusCode);

            if (OutputStyle == ExceptionOutputStyle.Json)
                resp.Content = new StringContent(data.ToJson(), GlobalVariables.Encoding, ContentTypes.Json);

            if (OutputStyle == ExceptionOutputStyle.Xml)
                resp.Content = new StringContent(data.ToXml(), GlobalVariables.Encoding, ContentTypes.Xml);

            actionExecutedContext.Response = resp;
        }
    }
}