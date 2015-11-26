using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radial
{

    /// <summary>
    /// Search result..
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public class SearchResult<TResult> where TResult : class
    {

        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        public TResult Result { get; set; }

        /// <summary>
        /// Gets or sets the object total.
        /// </summary>
        public int? ObjectTotal { get; set; }

        /// <summary>
        /// Gets or sets the page total.
        /// </summary>
        public int? PageTotal { get; set; }
    }
}
