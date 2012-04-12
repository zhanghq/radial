using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Radial.Net
{
    /// <summary>
    /// An interface to HTTP multipart/form-data request data.
    /// </summary>
    public interface IMultipartFormData
    {
        /// <summary>
        /// Gets the name of the post parameter.
        /// </summary>
        string ParamName { get; }

        /// <summary>
        /// Gets or sets the encoding of post data bytes.
        /// </summary>
        Encoding Encoding { get; set; }

        /// <summary>
        /// Write data to the request stream
        /// </summary>
        /// <param name="reqStream">The request stream.</param>
        void Write(Stream reqStream);
    }
}
