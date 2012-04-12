using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;

namespace Radial.Web.Mvc.Filters
{
    /// <summary>
    /// Represents an attribute that is used to handle and log an exception that is thrown by an action method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class HandleExceptionAttribute : HandleErrorAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HandleExceptionAttribute"/> class.
        /// </summary>
        public HandleExceptionAttribute()
            : this(ExceptionOutputStyle.Default, 9999)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HandleExceptionAttribute"/> class.
        /// </summary>
        /// <param name="outputStyle">The exception output style.</param>
        /// <param name="defaultErrorCode">The default error code.</param>
        public HandleExceptionAttribute(ExceptionOutputStyle outputStyle, int defaultErrorCode)
        {
            OutputStyle = outputStyle;
            DefaultErrorCode = defaultErrorCode;
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
        /// Called when an exception occurs.
        /// </summary>
        /// <param name="filterContext">The action-filter context.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="filterContext"/> parameter is null.</exception>
        public override void OnException(ExceptionContext filterContext)
        {
            Logger.Default.Fatal(filterContext.Exception);


            if (OutputStyle == ExceptionOutputStyle.Default)
            {
                base.OnException(filterContext);
                return;
            }

            filterContext.ExceptionHandled = true;

            ExceptionOutputData data = new ExceptionOutputData
            {
                ErrorCode = filterContext.Exception is KnownFaultException ? ((KnownFaultException)filterContext.Exception).ErrorCode : DefaultErrorCode,
                RequestUrl = HttpKits.MakeRelativeUrl(filterContext.HttpContext.Request.Url.ToString()).Replace("~", string.Empty),
                ErrorMessage = filterContext.Exception.Message
            };

            if (OutputStyle == ExceptionOutputStyle.Json)
                HttpKits.WriteJson<ExceptionOutputData>(data);
            if (OutputStyle == ExceptionOutputStyle.Xml)
                HttpKits.WriteXml(data.ToXml());

            filterContext.Result = new EmptyResult();
        }
    }
}