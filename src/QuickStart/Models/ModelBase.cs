using Radial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickStart.Models
{
    /// <summary>
    /// ModelBase
    /// </summary>
    public abstract class ModelBase
    {
        public ModelBase()
        {
            Id = Guid.NewGuid().ToString("n").ToUpper().Substring(0, 20);
        }

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
