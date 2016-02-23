using Radial.UnitTest.Persist.Domain;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Radial.UnitTest.Persist.Efs
{
    class DefaultDbContext: DbContext
    {
        public DefaultDbContext(string connStringName)
        {
            Database.Connection.ConnectionString = ConfigurationManager.ConnectionStrings[connStringName].ConnectionString;
            Configuration.ValidateOnSaveEnabled = false;
            Database.SetInitializer<DefaultDbContext>(null);
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
