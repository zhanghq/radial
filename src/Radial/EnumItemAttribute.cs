using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radial
{
    /// <summary>
    /// Used to describe the enumeration item.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class EnumItemAttribute : Attribute
    {
        string _description;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumItemAttribute"/> class.
        /// </summary>
        /// <param name="description">The item description.</param>
        public EnumItemAttribute(string description)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(description), "enumeration item description can not be empty or null");
            _description = description.Trim();
        }

        /// <summary>
        /// Gets the item description.
        /// </summary>
        public string Description
        {
            get
            {
                return _description;
            }
        }
    }
}
