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
        /// Get cache data.
        /// </summary>
        /// <param name="key">The cache key.</param>
        /// <returns>If there has matched key, return the cached value, otherwise return null.</returns>
        object Get(string key);
        /// <summary>
        /// Put cache data.
        /// </summary>
        /// <param name="key">The cache key.</param>
        /// <param name="value">The cache value.</param>
        /// <param name="cacheSeconds">The cache holding seconds.</param>
        void Put(string key, object value, int? cacheSeconds = null);
        /// <summary>
        /// Put cache data.
        /// </summary>
        /// <param name="key">The cache key.</param>
        /// <param name="region">The cache region.</param>
        /// <param name="value">The cache value.</param>
        /// <param name="cacheSeconds">The cache holding seconds.</param>
        void Put(string key, string region, object value, int? cacheSeconds = null);
        /// <summary>
        /// Drop cache data.
        /// </summary>
        /// <param name="key">The cache key.</param>
        void Drop(string key);


        /// <summary>
        /// Gets the cache regions.
        /// </summary>
        /// <returns>The cache regions.</returns>
        string[] GetRegions();

        /// <summary>
        /// Drop cache region.
        /// </summary>
        /// <param name="region">The cache region.</param>
        void DropRegion(string region);
    }
}
