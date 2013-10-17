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
        /// Gets storage alias according to the specified object key.
        /// </summary>
        /// <param name="type">Type of the object.</param>
        /// <param name="key">The object key according to.</param>
        /// <returns>
        /// The storage alias.
        /// </returns>
        string GetObjectAlias(Type type, object key);

        /// <summary>
        /// Gets storage aliases supported by the specified object type.
        /// </summary>
        /// <param name="type">Type of the object.</param>
        /// <returns>
        /// The storage alias array.
        /// </returns>
        string[] GetTypeAliases(Type type);
    }
}
