using Radial.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial.Web.Mvc.Filters
{
    /// <summary>
    /// Result cache rule class.
    /// </summary>
    public class ResultCacheRule
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResultCacheRule" /> class.
        /// </summary>
        /// <param name="url">The request url or url pattern.</param>
        /// <param name="ignoreCase">if set to <c>true</c> to ignore url case.</param>
        /// <param name="groups">The group names.</param>
        /// <param name="expires">The expiration time, default to 10 mins if not set.</param>
        public ResultCacheRule(string url, bool ignoreCase, string[] groups, TimeSpan? expires = null)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(url), "url can not be empty or null");

            Url = url.Trim().TrimStart('~', '/');
            IgnoreCase = ignoreCase;

            if (expires == null)
                expires = new TimeSpan(0, 10, 0);

            Expires = expires.Value;

            if (groups != null && groups.All(o => !string.IsNullOrWhiteSpace(o)))
            {
                Groups = groups;
                for (int i = 0; i < Groups.Length; i++)
                    Groups[i] = Groups[i].Trim().ToLower();
            }
        }

        /// <summary>
        /// Gets the request url or url pattern.
        /// </summary>
        public string Url { get; private set; }

        /// <summary>
        /// Gets a value indicating whether to ignore url case.
        /// </summary>
        public bool IgnoreCase { get; private set; }

        /// <summary>
        /// Gets the group names.
        /// </summary>
        public string [] Groups { get; private set; }

        /// <summary>
        /// Gets the expiration time.
        /// </summary>
        public TimeSpan Expires { get; private set; }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return Url.GetHashCode();
        }


        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
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
        /// ==s the specified aggregate.
        /// </summary>
        /// <param name="a">The aggregate.</param>
        /// <param name="b">The attribute.</param>
        /// <returns></returns>
        public static bool operator ==(ResultCacheRule a, ResultCacheRule b)
        {
            if ((object)a == null)
                return (object)b == null;

            return a.Equals(b);
        }

        /// <summary>
        /// !=s the specified aggregate.
        /// </summary>
        /// <param name="a">The aggregate.</param>
        /// <param name="b">The attribute.</param>
        /// <returns></returns>
        public static bool operator !=(ResultCacheRule a, ResultCacheRule b)
        {
            return !(a == b);
        }
    }
}
