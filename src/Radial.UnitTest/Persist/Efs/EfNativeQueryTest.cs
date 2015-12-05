using NUnit.Framework;
using Radial.Persist;
using Radial.Persist.Efs;


namespace Radial.UnitTest.Persist.Efs
{
    [TestFixture]
    public class EfNativeQueryTest: EfTestBase
    {


        [Test]
        public void WithoutRepo()
        {
            using(IUnitOfWork uow=new EfUnitOfWork())
            {
                var c=uow.NativeQuery.ExecuteDataTable("Select * From `Article`");
            }
        }
    }
}
