using Radial.Persist;
using Radial.Persist.Nhs;
using Radial.UnitTest.Persist.Nhs.Domain;

namespace Radial.UnitTest.Persist.Nhs.Repository
{
    class TeamRepository : BasicRepository<Team,int>
    {
        public TeamRepository(IUnitOfWorkEssential uow)
            : base(uow)
        {
        }

        public void ExecTestAddSP1()
        {
            this.UnitOfWork.NativeQuery.SpExecuteNonQuery("TestSP1");
        }

        public void ExecTestError()
        {
            this.UnitOfWork.NativeQuery.ExecuteNonQuery("SELECT 1/0");
        }

        public void ExecTestSelect()
        {
            //call this after Session.Flush(), if not there will be a wrong execution order
            Session.Flush();
            CreateSQLQuery("Select * from [Team]").ExecuteUpdate();
        }
    }
}
