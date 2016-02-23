using Radial.UnitTest.Persist.Domain;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial.UnitTest.Persist.Efs
{
    class DefaultDbContext: DbContext
    {
        public DefaultDbContext(string connStringName)
        {
            Database.Connection.ConnectionString = ConfigurationManager.ConnectionStrings[connStringName].ConnectionString;
            Configuration.ValidateOnSaveEnabled = false;
        }

        public DbSet<Question> Questions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();


            modelBuilder.Entity<QuestionYW>().Map(c =>
            {
                c.MapInheritedProperties();
                c.ToTable("QuestionYW");
            });
            modelBuilder.Entity<QuestionSX>().Map(c =>
            {
                c.MapInheritedProperties();
                c.ToTable("QuestionSX");
            });
            modelBuilder.Entity<QuestionYY>().Map(c =>
            {
                c.MapInheritedProperties();
                c.ToTable("QuestionYY");
            });
        }
    }
}
