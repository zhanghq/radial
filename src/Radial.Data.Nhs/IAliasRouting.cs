using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radial.Data.Nhs
{
    /// <summary>
    /// Session factory alias routing.
    /// </summary>
    public interface IAliasRouting
    {
        /// <summary>
        /// Gets the session factory alias.
        /// </summary>
        /// <param name="keys">The keys according to.</param>
        /// <returns>The session factory alias</returns>
        string GetAlias(params object[] keys);


        /// <summary>
        /// Get all available aliases to this instance.
        /// </summary>
        /// <returns>The session factory alias array.</returns>
        string[] GetAliases();
    }
}
