using NHibernate;
using NHibernate.Cfg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial.Persist.Nhs
{
    /// <summary>
    /// SessionFactorySet
    /// </summary>
    public class SessionFactorySet : HashSet<SessionFactoryEntry>
    {

        /// <summary>
        /// Gets the specified SessionFactoryEntry instance with given storage alias, throw an exception if not found or equals to null.
        /// </summary>
        /// <param name="storageAlias">The storage alias.</param>
        /// <returns>The specified SessionFactoryEntry instance</returns>
        public SessionFactoryEntry this[string storageAlias]
        {
            get
            {
                var ce = this.SingleOrDefault(o => string.Compare(o.StorageAlias, storageAlias.Trim(), true) == 0);

                Checker.Requires(ce != null, "can not find the storage alias: {0}", storageAlias);

                return ce;
            }
        }

        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public new bool Add(SessionFactoryEntry item)
        {
            if (item == null)
                return false;

            return base.Add(item);
        }

        /// <summary>
        /// Gets the first session factory entry, throw an exception if equals to null.
        /// </summary>
        /// <value>
        /// The first session factory entry.
        /// </value>
        public SessionFactoryEntry FirstEntry
        {
            get
            {
                var f= this.FirstOrDefault();

                Checker.Requires(f != null, "first SessionFactoryEntry instance can not be null");

                return f;
            }
        }
    }

    /// <summary>
    /// SessionFactoryEntry
    /// </summary>
    public class SessionFactoryEntry : IEquatable<SessionFactoryEntry>
    {
        /// <summary>
        /// The default storage alias
        /// </summary>
        public const string DefaultStorageAlias = "default";

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionFactoryEntry" /> class.
        /// </summary>
        /// <param name="storageAlias">The storage alias.</param>
        /// <param name="sessionFactory">The session factory.</param>
        /// <param name="isReadonly">if set to <c>true</c> [is readonly].</param>
        public SessionFactoryEntry(string storageAlias, ISessionFactory sessionFactory,
            bool isReadonly = false)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(storageAlias), "storage alias can not be empty or null");
            Checker.Parameter(sessionFactory != null, "session factory instance can not be null");

            StorageAlias = storageAlias.Trim();
            SessionFactory = sessionFactory;
            IsReadonly = isReadonly;
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
        /// Gets the session factory.
        /// </summary>
        /// <value>
        /// The session factory.
        /// </value>
        public ISessionFactory SessionFactory
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
        public bool IsReadonly
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
        public bool Equals(SessionFactoryEntry other)
        {
            if (ReferenceEquals(this, other))
                return true;

            if (other == null)
                return false;

            return this.GetHashCode() == other.GetHashCode();
        }
    }

}
