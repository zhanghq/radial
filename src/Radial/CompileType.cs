using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radial
{
    /// <summary>
    /// The compile type
    /// </summary>
    public enum CompileType
    {
        /// <summary>
        /// Debug
        /// </summary>
        [EnumItem("Debug Compile Model")]
        Debug,
        /// <summary>
        /// Release
        /// </summary>
        [EnumItem("Release Compile Model")]
        Release
    }
}
