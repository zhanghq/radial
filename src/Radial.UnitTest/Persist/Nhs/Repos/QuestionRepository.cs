using Radial.Persist.Nhs;
using Radial.UnitTest.Persist.Domain;
using Radial.UnitTest.Persist.Domain.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial.UnitTest.Persist.Nhs.Repos
{
    public class QuestionRepository : BasicRepository<Question, string>, IQuestionRepository
    {
        public QuestionRepository(Radial.Persist.IUnitOfWorkEssential uow) : base(uow) { }
    }
}
