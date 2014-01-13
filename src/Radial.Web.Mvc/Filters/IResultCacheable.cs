using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;

namespace Radial.Web.Mvc.Filters
{
    /// <summary>
    /// An interface declares how to deal with the action result.
    /// </summary>
    public interface IResultCacheable
    {
        /// <summary>
        /// Gets the data encoding.
        /// </summary>
        Encoding Encoding { get; }

        /// <summary>
        /// Determines whether the request context matched the conditions which generate cache data.
        /// </summary>
        /// <param name="context">The request context.</param>
        /// <returns>If matched return true, otherwise return false.</returns>
        bool IsMatched(RequestContext context);

        /// <summary>
        /// Retrieves the HTML content based on the request context.
        /// </summary>
        /// <param name="context">The request context.</param>
        /// <param name="html">The retrived HTML content.</param>
        /// <returns>
        /// If successfully retrieved return true, otherwise return false.
        /// </returns>
        bool Get(RequestContext context, out string html);


        /// <summary>
        /// Writes the HTML content to cache.
        /// </summary>
        /// <param name="context">The request context.</param>
        /// <param name="html">The HTML content.</param>
        void Set(RequestContext context, string html);


        /// <summary>
        /// Removes the cache data by the specified url.
        /// </summary>
        /// <param name="url">The specified url.</param>
        void Remove(string url);

        /// <summary>
        /// Batch removes the cache data by group names, or clear all if no name is given.
        /// </summary>
        /// <param name="groups">The group names.</param>
        void BatchRemove(params string[] groups);
    }
}
