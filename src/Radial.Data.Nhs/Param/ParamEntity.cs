using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radial.Param;

namespace Radial.Data.Nhs.Param
{
    /// <summary>
    /// Entity class of param object.
    /// </summary>
    public class ParamEntity
    {
        string _path;
        string _name;
        IList<ParamEntity> _children;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParamEntity"/> class.
        /// </summary>
        public ParamEntity() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParamEntity"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        public ParamEntity(string path)
        {
            _path = ParamObject.NormalizePath(path);
            _name = ParamObject.GetParamName(_path);
            _children = new List<ParamEntity>();
        }

        /// <summary>
        /// Gets the path.
        /// </summary>
        public virtual string Path { get { return _path; } }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public virtual string Name { get { return _name; } }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public virtual string Value { get; set; }

        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        public virtual ParamEntity Parent { get; set; }

        /// <summary>
        /// Gets the children.
        /// </summary>
        public virtual IList<ParamEntity> Children { get { return _children; } }
    }
}
