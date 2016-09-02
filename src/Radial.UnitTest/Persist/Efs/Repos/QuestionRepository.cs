using Radial.UnitTest.Persist.Domain;
using Radial.UnitTest.Persist.Domain.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial.UnitTest.Persist.Efs.Repos
{
    //[RegisterInterface(typeof(IQuestionRepository), "efs")]
    public class QuestionRepository : Radial.Persist.Efs.BasicRepository<Question>, IQuestionRepository
    {
        public QuestionRepository(Radial.Persist.IUnitOfWorkEssential uow) : base(uow)
        {
            SetDefaultOrderBys(new ObjectOrderBy<Question>(o => o.CreateTime, false));
        }
    }
}
