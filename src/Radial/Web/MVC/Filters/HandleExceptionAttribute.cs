using System;
using System.Web.Mvc;
using System.Net;
using Radial.Net;

namespace Radial.Web.Mvc.Filters
{
    /// <summary>
    /// Represents an attribute that is used to handle and log an exception that is thrown by an action method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class HandleExceptionAttribute : HandleErrorAttribute
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
        public HandleExceptionAttribute(ExceptionOutputStyle outputStyle, int defaultErrorCode,
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
        /// <param name="filterContext">The action-filter context.</param>
        public override void OnException(ExceptionContext filterContext)
        {
            Logger.Default.Fatal(filterContext.Exception);


            if (OutputStyle == ExceptionOutputStyle.System)
            {
                base.OnException(filterContext);
                return;
            }

            filterContext.ExceptionHandled = true;

            KnownFaultException hkfe = filterContext.Exception as KnownFaultException;

            ExceptionOutputData data = new ExceptionOutputData
            {
                ErrorCode = hkfe != null ? hkfe.ErrorCode : DefaultErrorCode,
                RequestUrl = HttpKits.MakeRelativeUrl(filterContext.HttpContext.Request.RawUrl).Replace("~", string.Empty),
                ErrorMessage = hkfe != null ? hkfe.Message : DefaultErrorMessage
            };

            filterContext.HttpContext.Response.Clear();
            filterContext.HttpContext.Response.ContentEncoding = GlobalVariables.Encoding;

            string respContext = string.Empty;

            if (OutputStyle == ExceptionOutputStyle.Json)
            {
                filterContext.HttpContext.Response.ContentType = ContentTypes.Json;
                respContext = data.ToJson();
            }
            if (OutputStyle == ExceptionOutputStyle.Xml)
            {
                filterContext.HttpContext.Response.ContentType = ContentTypes.Xml;
                respContext = data.ToXml();
            }

            filterContext.HttpContext.Response.StatusCode = (int)GlobalVariables.WebExceptionHttpStatusCode;

            filterContext.HttpContext.Response.Write(respContext);

            filterContext.HttpContext.ApplicationInstance.CompleteRequest();
        }
    }
}