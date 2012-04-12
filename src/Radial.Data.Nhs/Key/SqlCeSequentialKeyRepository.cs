using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;

namespace Radial.Data.Nhs.Key
{
    /// <summary>
    /// SqlCe sequential key repository
    /// </summary>
    public class SqlCeSequentialKeyRepository : BasicRepository<SequentialKeyEntity, string>, ISequentialKeyRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlCeSequentialKeyRepository"/> class.
        /// </summary>
        public SqlCeSequentialKeyRepository() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlCeSequentialKeyRepository"/> class.
        /// </summary>
        /// <param name="session">The NHibernate session object.</param>
        public SqlCeSequentialKeyRepository(ISession session) : base(session) { }

        #region ISequentialKeyRepository Members

        /// <summary>
        /// Upserts and returns the current value of the specified discriminator.
        /// </summary>
        /// <param name="discriminator">The discriminator.</param>
        /// <param name="increaseStep">The increase step.</param>
        /// <returns>The new sequential key.</returns>
        public ulong Upsert(string discriminator, uint increaseStep)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(discriminator), "discriminator can not be empty or null");

            ulong newValue = 0;

            SequentialKeyEntity obj = Session.QueryOver<SequentialKeyEntity>().Where(o => o.Discriminator == discriminator.Trim()).SingleOrDefault();

            if (obj == null)
                obj = new SequentialKeyEntity { Discriminator = discriminator.Trim(), Value = 0 };

            obj.Value += increaseStep;
            obj.UpdateTime = DateTime.Now;

            newValue = obj.Value;

            Session.Save(obj);

            return newValue;
        }

        #endregion
    }
}
