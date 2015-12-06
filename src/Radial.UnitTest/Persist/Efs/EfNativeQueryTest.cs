using NUnit.Framework;
using Radial.Persist;
using Radial.Persist.Efs;
using Radial.UnitTest.Persist.Efs.Domain;

namespace Radial.UnitTest.Persist.Efs
{
    [TestFixture]
    public class EfNativeQueryTest: EfTestBase
    {

        [Test]
        public void WithoutRepo()
        {
            using (IUnitOfWork uow = new EfUnitOfWork())
            { 
                uow.PrepareTransaction();
                var c = uow.NativeQuery.ExecuteDataTable("Insert into `Article`(`Id`) Values ('" + Radial.TimingSeq.Next() + "')");

                uow.Commit();
            }
        }
    }
}
