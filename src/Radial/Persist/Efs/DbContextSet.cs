using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Radial.Persist.Efs
{
    /// <summary>
    /// DbContextSet
    /// </summary>
    public sealed class DbContextSet
    {
        HashSet<DbContextEntry> _set = new HashSet<DbContextEntry>();

        /// <summary>
        /// Adds the DbContextEntry instance.
        /// </summary>
        /// <param name="item">The DbContextEntry instance.</param>
        public void Add(DbContextEntry item)
        {
            if (item == null)
                return;

            Checker.Requires(!_set.Contains(item), "duplicated DbContextEntry alias: {0}", item.StorageAlias);

            _set.Add(item);
        }

        /// <summary>
        /// Adds the DbContextEntry instances.
        /// </summary>
        /// <param name="items">The DbContextEntry instances.</param>
        public void Add(IEnumerable<DbContextEntry> items)
        {
            if (items == null)
                return;

            foreach (var item in items)
                Add(item);
        }


        /// <summary>
        /// Gets the specified DbContextEntry instance with the given storage alias, throw an exception if not found..
        /// </summary>
        /// <param name="storageAlias">The storage alias.</param>
        /// <returns>The specified DbContextEntry instance</returns>
        public DbContextEntry this[string storageAlias]
        {
            get
            {
                var ce = _set.SingleOrDefault(o => string.Compare(o.StorageAlias, storageAlias.Trim(), true) == 0);

                Checker.Requires(ce != null, "can not find the DbContextEntry instance with the given storage alias {0}", storageAlias);

                return ce;
            }
        }

        /// <summary>
        /// Gets the first DbContextEntry instance.
        /// </summary>
        /// <value>
        /// The first DbContextEntry instance.
        /// </value>
        public DbContextEntry FirstEntry
        {
            get
            {
                return _set.FirstOrDefault();
            }
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        public int Count
        {
            get
            {
                return _set.Count;
            }
        }
    }

    /// <summary>
    /// DbContextEntry
    /// </summary>
    public sealed class DbContextEntry: IEquatable<DbContextEntry>
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="DbContextEntry"/> class.
        /// </summary>
        /// <param name="storageAlias">The storage alias.</param>
        /// <param name="func">The delegate to create new DbContext instance.</param>
        /// <param name="isReadonly">if set to <c>true</c> [is readonly].</param>
        public DbContextEntry(string storageAlias, Func<DbContext> func,
            bool isReadonly = false)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(storageAlias), "storage alias can not be empty or null");
            Checker.Parameter(func != null, "Func<DbContext> delegate can not be null");

            StorageAlias = storageAlias.Trim();
            Func = func;
            IsReadOnly = isReadonly;
        }

        /// <summary>
        /// Gets the storage alias.
        /// </summary>
        public string StorageAlias
        {
            get;
            private set;
        }


        /// <summary>
        /// Gets the delegate to create new DbContext instance.
        /// </summary>
        public Func<DbContext> Func
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is readonly.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is readonly; otherwise, <c>false</c>.
        /// </value>
        public bool IsReadOnly
        {
            get;
            private set;
        }

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
            return (this.GetType().FullName + StorageAlias.ToLower()).GetHashCode();
        }

        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public bool Equals(DbContextEntry other)
        {
            if (ReferenceEquals(this, other))
                return true;

            if (other == null)
                return false;

            return this.GetHashCode() == other.GetHashCode();
        }
    }
}