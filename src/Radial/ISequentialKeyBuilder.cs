using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radial
{
    /// <summary>
    /// Sequential key builder.
    /// </summary>
    public interface ISequentialKeyBuilder
    {
        /// <summary>
        /// Gets the next sequential UInt64 key based on the specified type.
        /// </summary>
        /// <typeparam name="T">The specified type.</typeparam>
        /// <returns>
        /// The next sequential key value.
        /// </returns>
        ulong Next<T>();

        /// <summary>
        /// Gets the next sequential UInt64 key based on the specified type.
        /// </summary>
        /// <typeparam name="T">The specified type.</typeparam>
        /// <param name="increaseStep">The increase step.</param>
        /// <returns>
        /// The next sequential key value.
        /// </returns>
        ulong Next<T>(uint increaseStep);

        /// <summary>
        /// Gets the next sequential UInt64 key based on the specified type.
        /// </summary>
        /// <param name="type">The specified type.</param>
        /// <returns>The next sequential key value.</returns>
        ulong Next(Type type);

        /// <summary>
        /// Gets the next sequential UInt64 key based on the specified type.
        /// </summary>
        /// <param name="type">The specified type.</param>
        /// <param name="increaseStep">The increase step.</param>
        /// <returns>
        /// The next sequential key value.
        /// </returns>
        ulong Next(Type type, uint increaseStep);


        /// <summary>
        /// Gets the next sequential UInt64 key based on the unique discriminator.
        /// </summary>
        /// <param name="discriminator">The unique discriminator.</param>
        /// <returns>The next sequential key value.</returns>
        ulong Next(string discriminator);

        /// <summary>
        /// Gets the next sequential UInt64 key based on the unique discriminator.
        /// </summary>
        /// <param name="discriminator">The unique discriminator.</param>
        /// <param name="increaseStep">The increase step.</param>
        /// <returns>
        /// The next sequential key value.
        /// </returns>
        ulong Next(string discriminator, uint increaseStep);
    }
}
