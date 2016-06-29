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
        where TOrderBy : IObjectOrderBy
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

        /// <summary>
        /// To search criteria identifier string.
        /// </summary>
        /// <returns>The search criteria identifier string, if no value has been set, return null.</returns>
        public string ToIdentifier()
        {
            IList<string> ccs = new List<string>();

            string filterJson = Serialization.JsonSerializer.Serialize(Filter);
            //Consider the case sensitive characters
            if (!string.IsNullOrWhiteSpace(filterJson))
                ccs.Add("f@" + filterJson);

            if (OrderBys != null)
            {
                IList<string> ods = new List<string>();
                foreach (var o in OrderBys)
                {
                    if (!string.IsNullOrWhiteSpace(o.PropertyName))
                        ods.Add(string.Format("{0}_{1}", o.PropertyName, o.IsAscending ? 0 : 1));
                }
                if (ods.Count > 0)
                    ccs.Add("o@" + string.Join("$", ods.ToArray()).ToLower());
            }
            if (PageSize.HasValue)
                ccs.Add("s@" + PageSize.Value);
            if (PageIndex.HasValue)
                ccs.Add("i@" + PageIndex.Value);

            string cc = string.Join("+", ccs.ToArray());

            if (string.IsNullOrWhiteSpace(cc))
                return null;

            return Security.CryptoProvider.SHA1Encrypt(cc);
        }
    }
}
