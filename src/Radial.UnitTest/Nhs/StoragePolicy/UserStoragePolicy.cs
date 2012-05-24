using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radial.Data.Nhs;

namespace Radial.UnitTest.Nhs.StoragePolicy
{
    public class UserStoragePolicy : Radial.Data.IStoragePolicy
    {
        #region IStoragePolicy Members

        /// <summary>
        /// Gets the storage alias.
        /// </summary>
        /// <param name="keys">The keys according to.</param>
        /// <returns>
        /// The storage alias
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
        /// Get all available storage aliases.
        /// </summary>
        /// <returns>
        /// The storage alias array.
        /// </returns>
        public string[] GetAliases()
        {
            return new string[] { "Partition1", "Partition2" };
        }

        #endregion
    }
}
