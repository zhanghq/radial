using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial.Persist.Efs
{
    /// <summary>
    /// IDbContextPoolInitializer
    /// </summary>
    public interface IDbContextPoolInitializer
    {
        /// <summary>
        /// Execute pool initialization.
        /// </summary>
        /// <returns>The DbContext wrapper set.</returns>
        ISet<DbContextWrapper> Execute();
    }
}
