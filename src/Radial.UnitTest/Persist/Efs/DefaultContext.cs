using Radial.UnitTest.Persist.Efs.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial.UnitTest.Persist.Efs
{
    class DefaultContext : DbContext
    {
        public DefaultContext() : base("name=Default")
        {
            Database.SetInitializer<DefaultContext>(null);

        }
        public DbSet<Article> Articles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Article>().HasKey(o => o.Id);
        }
    }
}
