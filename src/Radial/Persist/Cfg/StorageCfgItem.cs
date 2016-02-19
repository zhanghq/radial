using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial.Persist.Cfg
{
    /// <summary>
    /// StorageCfgSet
    /// </summary>
    public sealed class StorageCfgSet : HashSet<StorageCfgItem>
    {
        /// <summary>
        /// Gets the <see cref="StorageCfgItem"/> with the specified alias.
        /// </summary>
        /// <value>
        /// The <see cref="StorageCfgItem"/>.
        /// </value>
        /// <param name="alias">The alias.</param>
        /// <returns></returns>
        public StorageCfgItem this[string alias]
        {
            get
            {
                return this.SingleOrDefault(o => string.Compare(o.Alias, alias, true) == 0);
            }
        }

        /// <summary>
        /// Gets the <see cref="StorageCfgItem"/> at the specified index.
        /// </summary>
        /// <value>
        /// The <see cref="StorageCfgItem"/>.
        /// </value>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public StorageCfgItem this[int index]
        {
            get
            {
                return this.ElementAt(index);
            }
        }
    }

    /// <summary>
    /// StorageCfgItem
    /// </summary>
    public sealed class StorageCfgItem: IEquatable<StorageCfgItem>
    {
        /// <summary>
        /// Gets or sets the storage alias.
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// Gets or sets the name of the database.
        /// </summary>
        public string DatabaseName { get; set; }

        /// <summary>
        /// Gets or sets the contained objects.
        /// </summary>
        public ContainedObjectCfgSet ContainedObjects { get; set; }


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
            return Alias.Trim().ToLower().GetHashCode();
        }

        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public bool Equals(StorageCfgItem other)
        {
            if (ReferenceEquals(this, other))
                return true;

            if (other == null)
                return false;

            return this.GetHashCode() == other.GetHashCode();
        }
    }

    /// <summary>
    /// ContainedObjectCfgSet
    /// </summary>
    public sealed class ContainedObjectCfgSet : HashSet<ContainedObjectCfgItem>
    {
        /// <summary>
        /// Gets the <see cref="ContainedObjectCfgItem"/> with the specified class name.
        /// </summary>
        /// <value>
        /// The <see cref="ContainedObjectCfgItem"/>.
        /// </value>
        /// <param name="className">Name of the class.</param>
        /// <returns></returns>
        public ContainedObjectCfgItem this[string className]
        {
            get
            {
                return this.SingleOrDefault(o => string.Compare(o.ClassName, className, true) == 0);
            }
        }

        /// <summary>
        /// Gets the <see cref="ContainedObjectCfgItem"/> at the specified index.
        /// </summary>
        /// <value>
        /// The <see cref="ContainedObjectCfgItem"/>.
        /// </value>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public ContainedObjectCfgItem this[int index]
        {
            get
            {
                return this.ElementAt(index);
            }
        }
    }

    /// <summary>
    /// ContainedObjectCfgItem
    /// </summary>
    public sealed class ContainedObjectCfgItem: IEquatable<ContainedObjectCfgItem>
    {
        /// <summary>
        /// Gets or sets the name of the class.
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// Gets or sets the mapping replaces.
        /// </summary>
        public IDictionary<string,string> MappingReplaces { get; set; }

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
        public bool Equals(ContainedObjectCfgItem other)
        {
            if (ReferenceEquals(this, other))
                return true;

            if (other == null)
                return false;

            return this.GetHashCode() == other.GetHashCode();
        }
    }
}
