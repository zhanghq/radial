using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radial.Data.Nhs.Key
{
    /// <summary>
    /// Entity class of sequential key. 
    /// </summary>
    public class SequentialKeyEntity
    {
        /// <summary>
        /// Gets or sets the discriminator.
        /// </summary>
        public virtual string Discriminator { get; set; }
        /// <summary>
        /// Gets or sets the current value.
        /// </summary>
        public virtual ulong Value { get; set; }

        /// <summary>
        /// Gets or sets the update time.
        /// </summary>
        public virtual DateTime UpdateTime { get; set; }
    }
}
