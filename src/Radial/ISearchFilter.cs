using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial
{
    /// <summary>
    /// ISearchFilter
    /// </summary>
    public interface ISearchFilter
    {
        /// <summary>
        /// Asserts whether this filter is valid, if not, an exception should be thrown.
        /// </summary>
        void AssertValid();
    }
}
