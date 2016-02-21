using Radial.Persist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial.UnitTest.Persist
{
    public static class Storages
    {
        public static StorageEntry Db { get { return new StorageEntry { Alias = "db", IsReadOnly = false }; } }

        public static StorageEntry Db0 { get { return new StorageEntry { Alias = "db0", IsReadOnly = false }; } }
    
        public static StorageEntry Db1 { get { return new StorageEntry { Alias = "db1", IsReadOnly = true }; } }

    }
}
