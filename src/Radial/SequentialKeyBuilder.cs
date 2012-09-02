using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radial
{
    /// <summary>
    /// The base class of sequential key builder.
    /// </summary>
    public abstract class SequentialKeyBuilder
    {
        /// <summary>
        /// Gets the next sequential UInt64 key based on the specified type.
        /// </summary>
        /// <typeparam name="T">The specified type.</typeparam>
        /// <returns>
        /// The next sequential key value.
        /// </returns>
        public ulong Next<T>()
        {
            return Next<T>(1);
        }


        /// <summary>
        /// Gets the next sequential UInt64 key based on the specified type.
        /// </summary>
        /// <typeparam name="T">The specified type.</typeparam>
        /// <param name="increaseStep">The increase step.</param>
        /// <returns>
        /// The next sequential key value.
        /// </returns>
        public ulong Next<T>(uint increaseStep)
        {
            return Next(typeof(T), increaseStep);
        }

        /// <summary>
        /// Gets the next sequential UInt64 key based on the specified type.
        /// </summary>
        /// <param name="type">The specified type.</param>
        /// <returns>The next sequential key value.</returns>
        public ulong Next(Type type)
        {
            return Next(type, 1);
        }

        /// <summary>
        /// Gets the next sequential UInt64 key based on the specified type.
        /// </summary>
        /// <param name="type">The specified type.</param>
        /// <param name="increaseStep">The increase step.</param>
        /// <returns>
        /// The next sequential key value.
        /// </returns>
        public ulong Next(Type type, uint increaseStep)
        {
            return Next(type.FullName, increaseStep);
        }

        /// <summary>
        /// Gets the next sequential UInt64 key based on the unique discriminator.
        /// </summary>
        /// <param name="discriminator">The unique discriminator.</param>
        /// <returns>The next sequential key value.</returns>
        public ulong Next(string discriminator)
        {
            return Next(discriminator, 1);
        }

        /// <summary>
        /// Gets the next sequential UInt64 key based on the unique discriminator.
        /// </summary>
        /// <param name="discriminator">The unique discriminator.</param>
        /// <param name="increaseStep">The increase step.</param>
        /// <returns>
        /// The next sequential key value.
        /// </returns>
        public abstract ulong Next(string discriminator, uint increaseStep);
    }
}
