using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial.Persist.Cfg
{
    /// <summary>
    /// DatabaseSettingType
    /// </summary>
    public enum DatabaseSettingType
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
    /// DatabaseCfgSet
    /// </summary>
    public sealed class DatabaseCfgSet : HashSet<DatabaseCfgItem>
    {
        /// <summary>
        /// Gets the <see cref="DatabaseCfgItem"/> with the specified name.
        /// </summary>
        /// <value>
        /// The <see cref="DatabaseCfgItem"/>.
        /// </value>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public DatabaseCfgItem this[string name]
        {
            get
            {
                return this.SingleOrDefault(o => string.Compare(o.Name, name, true) == 0);
            }
        }
    }

    /// <summary>
    /// DatabaseCfgItem
    /// </summary>
    public sealed class DatabaseCfgItem : IEquatable<DatabaseCfgItem>
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the type of the setting.
        /// </summary>
        public DatabaseSettingType SettingType { get; set; }

        /// <summary>
        /// Gets or sets the setting value.
        /// </summary>
        public string SettingValue { get; set; }

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
            return Name.Trim().ToLower().GetHashCode();
        }

        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public bool Equals(DatabaseCfgItem other)
        {
            if (ReferenceEquals(this, other))
                return true;

            if (other == null)
                return false;

            return this.GetHashCode() == other.GetHashCode();
        }
    }
}
