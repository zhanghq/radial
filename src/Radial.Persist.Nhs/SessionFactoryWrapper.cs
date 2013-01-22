using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;

namespace Radial.Persist.Nhs
{
    /// <summary>
    /// A wrapper class of ISessionFactory instance.
    /// </summary>
    public sealed class SessionFactoryWrapper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SessionFactoryWrapper"/> class.
        /// </summary>
        /// <param name="alias">The storage alias (case insensitive).</param>
        /// <param name="factory">The factory instance.</param>
        public SessionFactoryWrapper(string alias, ISessionFactory factory)
            : this(alias, factory, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionFactoryWrapper"/> class.
        /// </summary>
        /// <param name="alias">The storage alias (case insensitive).</param>
        /// <param name="factory">The factory instance.</param>
        /// <param name="group">The factory group, if equal to null means the factory not included in any group.</param>
        public SessionFactoryWrapper(string alias, ISessionFactory factory, int? group)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(alias), "storage alias can not be empty or null");
            Checker.Parameter(factory != null, "factory instance can not be null");

            Alias = alias.ToLower().Trim();
            Factory = factory;
            Group = group;
        }

        /// <summary>
        /// Gets the storage alias.
        /// </summary>
        public string Alias
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the factory instance.
        /// </summary>
        public ISessionFactory Factory
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the storage group, if equal to null means the factory not included in any group.
        /// </summary>
        public int? Group
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
            return Alias.GetHashCode();
        }
    }
}
