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
        TValue? _min;
        TValue? _max;

        /// <summary>
        /// Initializes a new instance of the <see cref="RangeValue&lt;TValue&gt;"/> class.
        /// </summary>
        /// <param name="min">The range minimum value.</param>
        /// <param name="max">The range maximum value.</param>
        public RangeValue(TValue? min, TValue? max)
        {
            _min = min;
            _max = max;
        }

        /// <summary>
        /// Gets or sets the range minimum value.
        /// </summary>
        public TValue? Min { get { return _min; } set { _min = value; } }
        /// <summary>
        /// Gets or sets the range maximum value.
        /// </summary>
        public TValue? Max { get { return _max; } set { _max = value; } }
    }
}
