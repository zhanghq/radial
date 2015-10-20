using System;

namespace Radial.Persist.Nhs
{
    /// <summary>
    /// Table shard mapping.
    /// </summary>
    public struct ShardTableMapping : IEquatable<ShardTableMapping>
    {
        /// <summary>
        /// Gets or sets the full type of the object.
        /// </summary>
        /// <value>
        /// The type of the object.
        /// </value>
        public string ObjectType { get; set; }
        /// <summary>
        /// Gets or sets the name of the table.
        /// </summary>
        /// <value>
        /// The name of the table.
        /// </value>
        public string TableName { get; set; }


        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return ObjectType.Trim().ToLower().GetHashCode();
        }

        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public bool Equals(ShardTableMapping other)
        {
            if (ReferenceEquals(this, other))
                return true;

            return this.GetHashCode() == other.GetHashCode();
        }
    }
}
