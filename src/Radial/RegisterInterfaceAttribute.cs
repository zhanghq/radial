using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial
{
    /// <summary>
    /// RegisterInterfaceAttribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class RegisterInterfaceAttribute : Attribute
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterInterfaceAttribute"/> class.
        /// </summary>
        /// <param name="interfaceType">Type of the interface.</param>
        public RegisterInterfaceAttribute(Type interfaceType)
            : this(interfaceType, null)
        { }

		/// <summary>
		/// Initializes a new instance of the <see cref="Radial.RegisterInterfaceAttribute"/> class.
		/// </summary>
		/// <param name="interfaceType">Interface type.</param>
		/// <param name="symbol">Symbol.</param>
        public RegisterInterfaceAttribute(Type interfaceType, string symbol)
        {
            InterfaceType = interfaceType;
            Symbol = symbol;
        }

        /// <summary>
        /// Gets the type of the interface.
        /// </summary>
        /// <value>
        /// The type of the interface.
        /// </value>
        public Type InterfaceType { get; private set; }

        /// <summary>
        /// Gets the symbol.
        /// </summary>
        public string Symbol { get; private set; }
    }
}
