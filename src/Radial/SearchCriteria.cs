using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radial.Persist
{

    /// <summary>
    /// Search criteria.
    /// </summary>
    /// <typeparam name="TFilter">The type of the filter.</typeparam>
    /// <typeparam name="TOrderBy">The type of the order by.</typeparam>
    public sealed class SearchCriteria<TFilter, TOrderBy>
        where TFilter : class
        where TOrderBy : class
    {

        /// <summary>
        /// Gets or sets the filter.
        /// </summary>
        public TFilter Filter { get; set; }


        /// <summary>
        /// Gets or sets the order bys.
        /// </summary>
        public TOrderBy[] OrderBys { get; set; }


        /// <summary>
        /// Gets or sets the size of the page.
        /// </summary>
        public int? PageSize { get; set; }

        /// <summary>
        /// Gets or sets the index of the page.
        /// </summary>
        public int? PageIndex { get; set; }
    }
}
