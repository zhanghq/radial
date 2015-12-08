using Radial.Persist.Efs;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial.UnitTest.Persist.Efs
{
    class DefaultPoolInitializer : IDbContextPoolInitializer
    {
        public ISet<DbContextWrapper> Execute()
        {
            HashSet<DbContextWrapper> set = new HashSet<DbContextWrapper>();

            //var objectContext = ((IObjectContextAdapter)dbcontext).ObjectContext;
            //var mappingCollection = (StorageMappingItemCollection)objectContext.MetadataWorkspace.GetItemCollection(DataSpace.CSSpace);
            //mappingCollection.GenerateViews(new List<EdmSchemaError>());

            set.Add(new DbContextWrapper("default", () => { return new DefaultContext(); }));

            return set;
        }
    }
}
