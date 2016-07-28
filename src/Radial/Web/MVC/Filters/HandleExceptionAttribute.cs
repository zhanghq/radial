﻿using System;
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
        /// Initializes a new instance of the <see cref="HandleExceptionAttribute" /> class.
        /// </summary>
        /// <param name="defaultErrorCode">The default error code, if unknown exception occurs.</param>
        /// <param name="defaultErrorMessage">The default error message, if unknown exception occurs.</param>
        /// <param name="contentType">The content type.</param>
        public HandleExceptionAttribute(int defaultErrorCode = -9999, string defaultErrorMessage = null,
            string contentType = null)
        {
            DefaultErrorCode = defaultErrorCode;
            ContentType = contentType;

            if (string.IsNullOrWhiteSpace(ContentType))
                ContentType = ContentTypes.Json;

            if (!string.IsNullOrWhiteSpace(defaultErrorMessage))
                DefaultErrorMessage = defaultErrorMessage.Trim();
        }

        /// <summary>
        /// Gets the content type.
        /// </summary>
        public string ContentType
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

            KnownFaultException hkfe = filterContext.Exception as KnownFaultException;

            var stdJsonOutput = new StdJsonOutput();
            stdJsonOutput.Error = hkfe != null ? hkfe.ErrorCode : DefaultErrorCode;
            stdJsonOutput.Message = hkfe != null ? hkfe.Message : (string.IsNullOrWhiteSpace(DefaultErrorMessage) ? filterContext.Exception.Message : DefaultErrorMessage);

            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.Clear();
            filterContext.HttpContext.Response.ContentEncoding = GlobalVariables.Encoding;

            filterContext.HttpContext.Response.ContentType = ContentType;
            filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            filterContext.HttpContext.Response.Write(stdJsonOutput.ToJson());
            filterContext.HttpContext.ApplicationInstance.CompleteRequest();

        }
    }
}