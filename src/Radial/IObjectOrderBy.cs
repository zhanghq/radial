using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial
{
    /// <summary>
    /// Object order by interface.
    /// </summary>
    public interface IObjectOrderBy
    {
        /// <summary>
        /// Gets the name of the sort property.
        /// </summary>
        string PropertyName { get; }

        /// <summary>
        /// Gets a value indicating whether the property will sort in ascending.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the property will sort in ascending; otherwise, <c>false</c>.
        /// </value>
        bool IsAscending { get; }
    }
}
