using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode;
using NHibernate;
using NHibernate.Type;

namespace Radial.Data.Nhs.Param
{
    /// <summary>
    /// ParamEntityMapper
    /// </summary>
    public class ParamEntityMapper : ClassMapping<ParamEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParamEntityMapper"/> class.
        /// </summary>
        public ParamEntityMapper()
        {
            Table("Param");

            Lazy(false);
            Cache(o =>
            {
                o.Region("NhParam");
                o.Usage(CacheUsage.ReadWrite);
            });

            Id<string>(o => o.Id, m =>
            {
                m.Length(10);
                m.Generator(new NHibernate.Mapping.ByCode.AssignedGeneratorDef());
                m.Access(Accessor.NoSetter);
            });
            Version<int>(o => o.Version, m =>
            {
                m.Type((IVersionType)NHibernateUtil.Int32);
                m.Access(Accessor.NoSetter);
                m.UnsavedValue(0);
            });
            Property<string>(o => o.XmlContent, m =>
            {
                m.Type(NHibernateUtil.StringClob);
                m.NotNullable(true);
            });
        }
    }
}
