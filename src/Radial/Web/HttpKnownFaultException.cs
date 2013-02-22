using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Radial.Web
{
    /// <summary>
    /// Http known fault exception.
    /// </summary>
    public sealed class HttpKnownFaultException : KnownFaultException
    {
        HttpStatusCode? _statusCode;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpKnownFaultException"/> class.
        /// </summary>
        /// <param name="errorCode">The error code.</param>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="statusCode">The HTTP status code.</param>
        public HttpKnownFaultException(int errorCode, string message, HttpStatusCode? statusCode)
            : this(errorCode, message, null, statusCode)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpKnownFaultException"/> class.
        /// </summary>
        /// <param name="errorCode">The error code.</param>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="statusCode">The HTTP status code.</param>
        public HttpKnownFaultException(int errorCode, string message, Exception innerException, HttpStatusCode? statusCode)
            : base(errorCode, message, innerException)
        {
            _statusCode = statusCode;
        }


        /// <summary>
        /// Gets the HTTP status code.
        /// </summary>
        public HttpStatusCode? StatusCode
        {
            get
            {
                return _statusCode;
            }
        }
    }
}
