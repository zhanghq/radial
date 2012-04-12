using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radial.Data.Nhs.Key
{
    /// <summary>
    /// ISequentialKeyRepository.
    /// </summary>
    public interface ISequentialKeyRepository : IRepository<SequentialKeyEntity, string>
    {
        /// <summary>
        /// Upserts and returns the current value of the specified discriminator.
        /// </summary>
        /// <param name="discriminator">The discriminator.</param>
        /// <param name="increaseStep">The increase step.</param>
        /// <returns>
        /// The new sequential key.
        /// </returns>
        ulong Upsert(string discriminator, uint increaseStep);
    }
}
