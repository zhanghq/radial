using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial.Persist.Cfg
{

    /// <summary>
    /// ClassMappingType
    /// </summary>
    public enum ClassMappingType
    {
        /// <summary>
        /// The file.
        /// </summary>
        File = 0,
        /// <summary>
        /// The text.
        /// </summary>
        Text = 1
    }

    /// <summary>
    /// ObjectCfgSet
    /// </summary>
    public sealed class ObjectCfgSet : HashSet<ObjectCfgItem>
    {
        /// <summary>
        /// Gets the <see cref="ObjectCfgItem"/> with the specified class name.
        /// </summary>
        /// <value>
        /// The <see cref="ObjectCfgItem"/>.
        /// </value>
        /// <param name="className">Name of the class.</param>
        /// <returns></returns>
        public ObjectCfgItem this[string className]
        {
            get
            {
                return this.SingleOrDefault(o => string.Compare(o.ClassName, className, true) == 0);
            }
        }
    }

    /// <summary>
    /// ObjectCfgItem
    /// </summary>
    public sealed class ObjectCfgItem : IEquatable<ObjectCfgItem>
    {
        /// <summary>
        /// Gets or sets the name of the class.
        /// </summary>
        public string ClassName { get; set; }
        /// <summary>
        /// Gets or sets the type of the class.
        /// </summary>
        public Type ClassType { get; set; }

        /// <summary>
        /// Gets or sets the type of the mapping.
        /// </summary>
        public ClassMappingType MappingType { get; set; }

        /// <summary>
        /// Gets or sets the mapping value.
        /// </summary>
        public string MappingValue { get; set; }

        /// <summary>
        /// Gets or sets the type of the router.
        /// </summary>
        public Type RouterType { get; set; }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;

            if (obj == null)
                return false;

            return this.GetHashCode() == obj.GetHashCode();
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return ClassName.Trim().ToLower().GetHashCode();
        }

        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public bool Equals(ObjectCfgItem other)
        {
            if (ReferenceEquals(this, other))
                return true;

            if (other == null)
                return false;

            return this.GetHashCode() == other.GetHashCode();
        }
    }
}
