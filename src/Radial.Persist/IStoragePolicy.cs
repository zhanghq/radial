using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radial.Persist
{
    /// <summary>
    /// Storage policy.
    /// </summary>
    public interface IStoragePolicy
    {
        /// <summary>
        /// Gets the storage alias.
        /// </summary>
        /// <param name="keys">The keys according to.</param>
        /// <returns>The storage alias.</returns>
        string GetAlias(params object[] keys);


        /// <summary>
        /// Get all available storage aliases.
        /// </summary>
        /// <returns>The storage alias array.</returns>
        string[] GetAliases();
    }
}
