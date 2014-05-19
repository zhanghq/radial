using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radial.Cache
{
    /// <summary>
    /// Cache interface.
    /// </summary>
    public interface ICache
    {
        /// <summary>
        /// Retrieve cached data.
        /// </summary>
        /// <param name="key">The cache key.</param>
        /// <returns>If there has matched key, return the cached value, otherwise return null.</returns>
        object Get(string key);
        /// <summary>
        /// Set cache data.
        /// </summary>
        /// <param name="key">The cache key.</param>
        /// <param name="value">The cache value.</param>
        /// <param name="cacheSeconds">The cache holding seconds.</param>
        void Set(string key, object value, int? cacheSeconds = null);
        /// <summary>
        /// Remove cache data.
        /// </summary>
        /// <param name="key">The cache key.</param>
        void Remove(string key);
    }
}
