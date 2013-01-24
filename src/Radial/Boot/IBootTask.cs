using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radial.Boot
{
    /// <summary>
    /// Boot task interface.
    /// </summary>
    public interface IBootTask
    {
        /// <summary>
        /// System initialize process.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Start system.
        /// </summary>
        void Start();

        /// <summary>
        /// Stop system.
        /// </summary>
        void Stop();
    }
}
