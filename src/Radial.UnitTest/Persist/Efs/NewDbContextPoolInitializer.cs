using Radial.Persist.Efs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial.UnitTest.Persist.Efs
{
    public class NewDbContextPoolInitializer : IDbContextPoolInitializer
    {
        public DbContextSet Execute()
        {
            DbContextSet set = new DbContextSet();

            set.Add(new DbContextEntry(Storages.Db0.Alias,
                () => { return new DefaultDbContext(Storages.Db0.Alias); }, Storages.Db0.IsReadOnly));
            set.Add(new DbContextEntry(Storages.Db1.Alias,
                () => { return new DefaultDbContext(Storages.Db1.Alias); }, Storages.Db1.IsReadOnly));

            return set;
        }
    }
}
