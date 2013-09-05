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
        /// Retrieve cached data.
        /// </summary>
        /// <param name="key">The cache key(case insensitive).</param>
        /// <returns>
        /// If there has matched key, return the cached value, otherwise return null.
        /// </returns>
        public byte[] Get(string key)
        {
            return null;
        }

        /// <summary>
        /// Set cache data.
        /// </summary>
        /// <param name="key">The cache key(case insensitive).</param>
        /// <param name="value">The cache value.</param>
        /// <param name="cacheSeconds">The cache holding seconds.</param>
        public void Set(string key, byte[] value, int? cacheSeconds = null)
        {

        }


        /// <summary>
        /// Remove cache data.
        /// </summary>
        /// <param name="key">The cache key(case insensitive).</param>
        public void Remove(string key)
        {

        }

    }
}
