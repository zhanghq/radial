using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radial.Cache
{
    /// <summary>
    /// The default concret class with no cache implement
    /// </summary>
    public sealed class EmptyCache : ICache
    {

        /// <summary>
        /// Get cache data.
        /// </summary>
        /// <param name="key">The cache key.</param>
        /// <returns>
        /// If there has matched key, return the cached value, otherwise return null.
        /// </returns>
        public object Get(string key)
        {
            return null;
        }

        /// <summary>
        /// Put cache data.
        /// </summary>
        /// <param name="key">The cache key.</param>
        /// <param name="value">The cache value.</param>
        /// <param name="cacheSeconds">The cache holding seconds.</param>
        public void Put(string key, object value, int? cacheSeconds = null)
        {

        }


        /// <summary>
        /// Drop cache data.
        /// </summary>
        /// <param name="key">The cache key(case insensitive).</param>
        public void Drop(string key)
        {

        }

    }
}
