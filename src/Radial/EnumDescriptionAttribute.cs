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
    public class EnumDescriptionAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnumDescriptionAttribute"/> class.
        /// </summary>
        /// <param name="text">The item description text.</param>
        public EnumDescriptionAttribute(string text)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(text), "enumeration item description text can not be empty or null");
            Text = text.Trim();
        }

        /// <summary>
        /// Gets the item description.
        /// </summary>
        public string Text
        {
            get;
            private set;
        }
    }
}
