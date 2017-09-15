using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial
{
    /// <summary>
    /// DbResultField
    /// </summary>
    public class DbResultField
    {
        /// <summary>
        /// Gets or sets the field name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public Type Type { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public object Value { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is database null.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is database null; otherwise, <c>false</c>.
        /// </value>
        public bool IsDbNull
        {
            get
            {
                return Value == System.DBNull.Value;
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
