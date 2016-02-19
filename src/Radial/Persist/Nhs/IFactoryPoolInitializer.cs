using System.Collections.Generic;

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
        SessionFactoryWrapperSet Initialize();
    }
}
