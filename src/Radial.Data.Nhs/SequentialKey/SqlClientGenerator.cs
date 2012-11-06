using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radial.Data;
using Radial;
using NHibernate;

namespace Radial.Data.Nhs.SequentialKey
{
    /// <summary>
    /// SqlClient sequential key generator.
    /// </summary>
    public sealed class SqlClientGenerator : SequentialKeyGenerator
    {
        const string QUERY = "BEGIN TRANSACTION "
                        + "IF EXISTS(SELECT * FROM [SequentialKey] WHERE [Discriminator]=:Discriminator) "
                        + "UPDATE [SequentialKey] SET [Value]=[Value]+:IncreaseStep,[UpdateTime]=GETDATE() WHERE [Discriminator]=:Discriminator "
                        + "ELSE "
                        + "INSERT INTO [SequentialKey] ([Discriminator],[Value],[UpdateTime]) VALUES (:Discriminator,:IncreaseStep,GETDATE()) "
                        + "SELECT [Value] FROM [SequentialKey] WHERE [Discriminator]=:Discriminator "
                        + "COMMIT TRANSACTION";

        IUnitOfWork _uow;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlClientGenerator"/> class.
        /// </summary>
        /// <param name="uow">IUnitOfWork instance.</param>
        public SqlClientGenerator(IUnitOfWork uow)
        {
            _uow = uow;
        }

        /// <summary>
        /// Gets the next sequential UInt64 key based on the unique discriminator.
        /// </summary>
        /// <param name="discriminator">The unique discriminator.</param>
        /// <param name="increaseStep">The increase step.</param>
        /// <returns>
        /// The next sequential key value.
        /// </returns>
        public override ulong Next(string discriminator, uint increaseStep)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(discriminator), "discriminator can not be empty or null");

            ISQLQuery query = ((ISession)_uow.UnderlyingContext).CreateSQLQuery(QUERY);
            query.SetString("Discriminator", discriminator);
            query.SetParameter<int>("IncreaseStep", (int)increaseStep);

            return (ulong)query.UniqueResult<long>();
        }
    }
}
