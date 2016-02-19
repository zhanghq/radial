using System;
using NHibernate;
using NHibernate.Cfg;
using System.Collections.Generic;
using System.Linq;

namespace Radial.Persist.Nhs
{

    /// <summary>
    /// SessionFactoryWrapperSet
    /// </summary>
    public sealed class SessionFactoryWrapperSet: HashSet<SessionFactoryWrapper>
    {

        /// <summary>
        /// Gets the <see cref="SessionFactoryWrapper"/> with the specified alias.
        /// </summary>
        /// <value>
        /// The <see cref="SessionFactoryWrapper"/>.
        /// </value>
        /// <param name="alias">The alias.</param>
        /// <returns></returns>
        public SessionFactoryWrapper this[string alias]
        {
            get
            {
                return this.SingleOrDefault(o => string.Compare(o.Alias, alias, true) == 0);
            }
        }


        /// <summary>
        /// Gets the <see cref="SessionFactoryWrapper"/> at the specified index.
        /// </summary>
        /// <value>
        /// The <see cref="SessionFactoryWrapper"/>.
        /// </value>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public SessionFactoryWrapper this[int index]
        {
            get
            {
                return this.ElementAt(index);
            }
        }
    }

    /// <summary>
    /// A wrapper class of session factory wrapper instance.
    /// </summary>
    public sealed class SessionFactoryWrapper : IEquatable<SessionFactoryWrapper>
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionFactoryWrapper" /> class.
        /// </summary>
        /// <param name="alias">The storage alias (case insensitive).</param>
        /// <param name="factory">The ISessionFactory instance.</param>
        public SessionFactoryWrapper(string alias, ISessionFactory factory)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(alias), "storage alias can not be empty or null");
            Checker.Parameter(factory != null, "ISessionFactory instance can not be null");

            Alias = alias.Trim();
            SessionFactory = factory;
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
        /// Gets the session factory.
        /// </summary>
        public ISessionFactory SessionFactory
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
            return Alias.ToLower().GetHashCode();
        }

        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public bool Equals(SessionFactoryWrapper other)
        {
            if (ReferenceEquals(this, other))
                return true;

            if (other == null)
                return false;

            return this.GetHashCode() == other.GetHashCode();
        }
    }
}
