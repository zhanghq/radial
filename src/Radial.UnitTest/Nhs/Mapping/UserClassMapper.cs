using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radial.UnitTest.Nhs.Domain;
using NHibernate.Mapping.ByCode.Conformist;

namespace Radial.UnitTest.Nhs.Mapping
{
    class UserClassMapper : ClassMapping<User>
    {
        public UserClassMapper()
        {
            Id<int>(o => o.Id, m =>
            {
                m.Column("Id");
                m.Generator(new NHibernate.Mapping.ByCode.AssignedGeneratorDef());
            });
            Property<string>(o => o.Name, m =>
            {
                m.NotNullable(true); m.Length(50);
            });
        }
    }
}
