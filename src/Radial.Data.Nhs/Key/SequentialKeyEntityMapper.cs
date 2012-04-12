using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode;
using NHibernate;

namespace Radial.Data.Nhs.Key
{
    /// <summary>
    /// SequentialKeyEntityMapper
    /// </summary>
    public class SequentialKeyEntityMapper : ClassMapping<SequentialKeyEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SequentialKeyEntityMapper"/> class.
        /// </summary>
        public SequentialKeyEntityMapper()
        {
            Table("SequentialKey");
            Lazy(false);
            Id<string>(o => o.Discriminator, m =>
            {
                m.Generator(new NHibernate.Mapping.ByCode.AssignedGeneratorDef());
            });
            Property<ulong>(o => o.Value, m =>
            {
                m.NotNullable(true);
            });
            Property<DateTime>(o => o.UpdateTime, m =>
            {
                m.NotNullable(true);
            });
        }
    }
}
