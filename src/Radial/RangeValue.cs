using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radial
{
    /// <summary>
    /// Represents a range of value.
    /// </summary>
    public struct RangeValue<TValue> where TValue : struct
    {
        /// <summary>
        /// Gets or sets the range minimum value.
        /// </summary>
        public TValue? Min { get; set; }
        /// <summary>
        /// Gets or sets the range maximum value.
        /// </summary>
        public TValue? Max { get; set; }
    }
}
