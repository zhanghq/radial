using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radial.Web.Mvc.Models
{
    /// <summary>
    /// KnownFaultModel.
    /// </summary>
    public sealed class KnownFaultModel
    {

        /// <summary>
        /// Gets or sets the error code.
        /// </summary>
        /// <value>
        /// The error code.
        /// </value>
        public int ErrorCode { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message { get; set; }


        /// <summary>
        /// Gets or sets the inner exception.
        /// </summary>
        /// <value>
        /// The inner exception.
        /// </value>
        public Exception InnerException { get; set; }
    }
}
