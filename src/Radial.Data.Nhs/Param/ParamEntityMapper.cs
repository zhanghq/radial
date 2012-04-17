using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode;
using NHibernate;

namespace Radial.Data.Nhs.Param
{
    /// <summary>
    /// ParamEntityMapper
    /// </summary>
    public class SequentialKeyEntityMapper : ClassMapping<ParamEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SequentialKeyEntityMapper"/> class.
        /// </summary>
        public SequentialKeyEntityMapper()
        {
            Table("Param");
            Id<string>(o => o.Path, m =>
            {
                m.Generator(new NHibernate.Mapping.ByCode.AssignedGeneratorDef());
                m.Access(Accessor.NoSetter);
            });
            Property<string>(o => o.Name, m =>
            {
                m.NotNullable(true);
                m.Access(Accessor.NoSetter);
                m.Update(false);
            });

            Property<string>(o => o.Description);

            Property<string>(o => o.Value, m =>
            {
                m.Type(NHibernateUtil.StringClob);
            });

            ManyToOne(o => o.Parent, m =>
            {
                m.Column("ParentPath");
                m.Update(false);
            });
            Bag(o => o.Children, m =>
            {
                m.Access(Accessor.NoSetter);
                m.Cascade(NHibernate.Mapping.ByCode.Cascade.All | NHibernate.Mapping.ByCode.Cascade.DeleteOrphans);
                m.Inverse(true);
                m.OrderBy("Name ASC");
                m.Key(k => k.Column("ParentPath"));
            }, a => a.OneToMany());

        }
    }
}
