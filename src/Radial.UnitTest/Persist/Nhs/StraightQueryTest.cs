using NUnit.Framework;
using Radial.Persist;
using Radial.Persist.Nhs;
using Radial.UnitTest.Persist.Nhs.Domain;
using Radial.UnitTest.Persist.Nhs.Repository;

namespace Radial.UnitTest.Persist.Nhs
{
    [TestFixture]
    public class StraightQueryTest
    {
        [Test]
        public void WithRepo()
        {
            using (IUnitOfWork uow = new NhUnitOfWork())
            {
                //all will in trans by user explicit call
                uow.PrepareTransaction();

                Team t = new Team();
                t.Name = "adfadf";
                

                TeamRepository repo = new TeamRepository(uow);
                repo.ExecTestSelect();


                repo.ExecTestAddSP1();

                uow.RegisterNew(t);

                //rollback all
                //repo.ExecTestError();

                uow.Commit();
            }
        }

        [Test]
        public void WithRepoWithScope()
        {
            using(System.Transactions.TransactionScope tc=new System.Transactions.TransactionScope())
            using (IUnitOfWork uow = new NhUnitOfWork())
            {
                Team t = new Team();
                t.Name = "adfadf";


                TeamRepository repo = new TeamRepository(uow);
                //repo.Save(t);
                repo.ExecTestSelect();

                uow.RegisterNew(t);

                repo.ExecTestAddSP1();
                //repo.ExecTest1();

                // not affected
                uow.Commit();

                //not commit，no data
                //tc.Complete();
            }
        }

        [Test]
        public void WithoutRepo()
        {
            using (IUnitOfWork uow = new NhUnitOfWork())
            {
                StraightQuery query = new StraightQuery(uow);
                query.PrepareTransaction();
                query.ExecuteNonQuery("Insert into Team (Id,Name) Values (rand(),'ces')");
                query.SpExecuteNonQuery("TestSP1");
                uow.Commit();
            }
        }

        [Test]
        public void WithoutRepoScope()
        {
            using (System.Transactions.TransactionScope tc = new System.Transactions.TransactionScope())
            using (IUnitOfWork uow = new NhUnitOfWork())
            {
                StraightQuery query = new StraightQuery(uow);
                query.PrepareTransaction();
                query.ExecuteNonQuery("Insert into Team (Id,Name) Values (rand(),'ces')");
                query.SpExecuteNonQuery("TestSP1");
                // not affected
                uow.Commit();

                //not commit，no data
                //tc.Complete();
            }
        }


    }
}
