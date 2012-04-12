using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;

namespace Radial.Data.Nhs.Key
{
    /// <summary>
    /// SqlServer sequential key repository class.
    /// </summary>
    public class SqlServerSequentialKeyRepository : BasicRepository<SequentialKeyEntity, string>, ISequentialKeyRepository
    {
        const string SQLQUERY = "IF EXISTS(SELECT * FROM [SequentialKey] WHERE [Discriminator]=:Discriminator) "
                                + "BEGIN "
                                + "UPDATE [SequentialKey] SET [Value]=[Value]+:IncreaseStep,[UpdateTime]=:UpdateTime WHERE [Discriminator]=:Discriminator "
                                + "SELECT [Value] FROM [SequentialKey] WHERE [Discriminator]=:Discriminator "
                                + "END "
                                + "ELSE "
                                + "BEGIN "
                                + "INSERT INTO [SequentialKey] ([Discriminator],[Value],[UpdateTime]) VALUES (:Discriminator,:IncreaseStep,:UpdateTime) "
                                + "SELECT [Value] FROM [SequentialKey] WHERE [Discriminator]=:Discriminator "
                                + "END";

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlServerSequentialKeyRepository"/> class.
        /// </summary>
        public SqlServerSequentialKeyRepository() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlServerSequentialKeyRepository"/> class.
        /// </summary>
        /// <param name="session">The NHibernate session object.</param>
        public SqlServerSequentialKeyRepository(ISession session) : base(session) { }

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

            ISQLQuery query = Session.CreateSQLQuery(SQLQUERY);
            query.SetString("Discriminator", discriminator);
            query.SetParameter<int>("IncreaseStep", (int)increaseStep);
            query.SetDateTime("UpdateTime", DateTime.Now);

            return (ulong)query.UniqueResult<long>();
        }

        #endregion
    }
}
