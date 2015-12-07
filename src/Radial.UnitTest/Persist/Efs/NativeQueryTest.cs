using NUnit.Framework;
using Radial.Persist;
using Radial.Persist.Efs;
using Radial.UnitTest.Persist.Efs.Domain;

namespace Radial.UnitTest.Persist.Efs
{
    [TestFixture]
    public class NativeQueryTest: EfTestBase
    {

        [Test]
        public void WithoutRepo()
        {
            using (IUnitOfWork uow = new UnitOfWork())
            { 
                uow.PrepareTransaction();
                var c = uow.NativeQuery.ExecuteDataTable("Insert into `Article`(`Id`) Values ('" + Radial.TimingSeq.Next() + "')");

                uow.Commit();
            }
        }
    }
}
