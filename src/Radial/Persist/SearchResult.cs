using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radial.Persist
{

    /// <summary>
    /// Search result..
    /// </summary>
    /// <typeparam name="TData">The type of the data.</typeparam>
    public class SearchResult<TData> where TData : class
    {

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        public TData Data { get; set; }

        /// <summary>
        /// Gets or sets the object total if pageable.
        /// </summary>
        public int? ObjectTotal { get; set; }

        /// <summary>
        /// Gets or sets the page total if pageable.
        /// </summary>
        public int? PageTotal { get; set; }
    }
}
