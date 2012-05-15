using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radial.Data.Nhs.Key
{
    /// <summary>
    /// Default sequential key builder.
    /// </summary>
    public sealed class DefaultSequentialKeyBuilder : ISequentialKeyBuilder
    {

        ISequentialKeyRepository _repository;
        static object S_SyncRoot = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultSequentialKeyBuilder"/> class.
        /// </summary>
        /// <param name="repository">The ISequentialKeyRepository instance.</param>
        public DefaultSequentialKeyBuilder(ISequentialKeyRepository repository)
        {
            Checker.Parameter(repository != null, "ISequentialKeyRepository instance can not be null");
            _repository = repository;
        }

        #region ISequentialKeyBuilder Members

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
        public ulong Next(string discriminator, uint increaseStep)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(discriminator), "discriminator can not be empty or null");
            ulong nextKey = 0;

            lock (S_SyncRoot)
            {
                nextKey = _repository.Upsert(discriminator, increaseStep);
            }

            return nextKey;
        }

        #endregion
    }
}
