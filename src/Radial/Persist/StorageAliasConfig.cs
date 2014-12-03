using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial.Persist
{
    /// <summary>
    /// Storage alias config
    /// </summary>
    public struct StorageAliasConfig : IEquatable<StorageAliasConfig>
    {
        /// <summary>
        /// Gets the alias name.
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// Gets the alias settings.
        /// </summary>
        public string Settings { get; internal set; }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return Name.Trim().ToLower().GetHashCode();
        }

        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public bool Equals(StorageAliasConfig other)
        {
            if (ReferenceEquals(this, other))
                return true;

            return this.GetHashCode() == other.GetHashCode();
        }
    }
}
