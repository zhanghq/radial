using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radial.Data.Nhs
{
    /// <summary>
    /// Represent the routing item in alias routing configuration.
    /// </summary>
    public sealed class RoutingClass
    {
        /// <summary>
        /// Gets the type of the entity.
        /// </summary>
        public Type EntityType { get; internal set; }

        /// <summary>
        /// Gets the routing instance.
        /// </summary>
        public IAliasRouting RoutingInstance { get; internal set; }
    }
}
