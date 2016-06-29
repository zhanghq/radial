using Radial.UnitTest.Persist.Domain;
using Radial.UnitTest.Persist.Domain.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial.UnitTest.Persist.Nhs.Repos
{
    [RegisterInterface(typeof(IQuestionRepository), "nhs")]
    public class QuestionRepository : Radial.Persist.Nhs.BasicRepository<Question, string>, IQuestionRepository
    {
        public QuestionRepository(Radial.Persist.IUnitOfWorkEssential uow) : base(uow) {
            SetDefaultOrderBys(new Radial.Persist.ObjectOrderBy<Question>(o => o.CreateTime, false));
        }
    }
}
