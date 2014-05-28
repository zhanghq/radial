using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Radial;

namespace $safeprojectname$.Models
{
    /// <summary>
    /// ModelBase
    /// </summary>
    public abstract class ModelBase
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        public int Version { get; set; }
    }
}
