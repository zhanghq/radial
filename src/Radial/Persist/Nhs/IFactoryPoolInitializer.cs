using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radial.Persist.Nhs
{
    /// <summary>
    /// Initialize hibernate session factory pool.
    /// </summary>
    public interface IFactoryPoolInitializer
    {
        /// <summary>
        /// Execute pool initialization.
        /// </summary>
        /// <returns>The configuration wrapper set.</returns>
        ISet<ConfigurationWrapper> Execute();
    }
}
