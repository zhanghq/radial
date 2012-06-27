using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radial
{
    /// <summary>
    /// Known fault exception.
    /// </summary>
    public class KnownFaultException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KnownFaultException"/> class.
        /// </summary>
        /// <param name="errorCode">The error code.</param>
        /// <param name="message">The message that describes the error.</param>
        public KnownFaultException(int errorCode, string message)
            : base(message)
        {
            ErrorCode = errorCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KnownFaultException"/> class.
        /// </summary>
        /// <param name="errorCode">The error code.</param>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The inner exception.</param>
        public KnownFaultException(int errorCode, string message, Exception innerException)
            : base(message, innerException)
        {
            ErrorCode = errorCode;
        }

        /// <summary>
        /// Gets the error code.
        /// </summary>
        public int ErrorCode
        {
            get { return HResult; }
            private set { HResult = value; }
        }
    }
}
