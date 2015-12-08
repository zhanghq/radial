using System;
using System.Data.Entity;

namespace Radial.Persist.Efs
{
    /// <summary>
    /// DbContextWrapper
    /// </summary>
    public sealed class DbContextWrapper : IEquatable<DbContextWrapper>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DbContextWrapper"/> class.
        /// </summary>
        /// <param name="alias">The storage alias (case insensitive).</param>
        /// <param name="func">The delegate to create new DbContext instance.</param>
        public DbContextWrapper(string alias, Func<DbContext> func)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(alias), "storage alias can not be empty or null");
            Checker.Parameter(func != null, "Func<DbContext> delegate can not be null");

            Alias = alias.ToLower().Trim();
            Func = func;
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
        /// Gets the delegate to create new DbContext instance.
        /// </summary>
        private Func<DbContext> Func
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a new DbContext instance.
        /// </summary>
        public DbContext DbContext
        {
            get
            {
                return Func();
            }
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

        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public bool Equals(DbContextWrapper other)
        {
            return Equals(other);
        }

    }
}