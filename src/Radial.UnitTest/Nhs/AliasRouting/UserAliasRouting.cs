using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radial.Data.Nhs;

namespace Radial.UnitTest.Nhs.AliasRouting
{
    public class UserAliasRouting : IAliasRouting
    {
        #region IAliasRouting Members

        /// <summary>
        /// Gets the session factory alias.
        /// </summary>
        /// <param name="keys">The keys according to.</param>
        /// <returns>
        /// The session factory alias
        /// </returns>
        public string GetAlias(params object[] keys)
        {
            Checker.Parameter(keys.Length == 1 && keys[0] is int, "input keys incorrect");

            int id = (int)keys[0];

            if (id % 2 == 0)
                return "Partition2";
            return "Partition1";

        }

        /// <summary>
        /// Get all available aliases to this instance.
        /// </summary>
        /// <returns>
        /// The session factory alias array.
        /// </returns>
        public string[] GetAliases()
        {
            return new string[] { "Partition1", "Partition2" };
        }

        #endregion
    }
}
