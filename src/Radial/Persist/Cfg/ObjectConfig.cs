using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial.Persist.Cfg
{
    /// <summary>
    /// ObjectConfig
    /// </summary>
    public sealed class ObjectConfig
    {
        /// <summary>
        /// Gets or sets the class type.
        /// </summary>
        public string Class { get; set; }

        /// <summary>
        /// Gets or sets the mapping.
        /// </summary>
        public ObjectMappingConfig Mapping { get; set; }
    }

    /// <summary>
    /// ObjectMappingConfig
    /// </summary>
    public sealed class ObjectMappingConfig
    {
        /// <summary>
        /// Gets or sets the mapping template value.
        /// </summary>
        public string Template { get; set; }
        /// <summary>
        /// Gets or sets the type of the mapping template value.
        /// </summary>
        public ConfigValueType TemplateType { get; set; }
        ///// <summary>
        ///// Gets or sets the parameters of the mapping template.
        ///// </summary>
        //public ISet<string> Parameters { get; set; }
    }
}
