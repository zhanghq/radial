using Radial.Persist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial.UnitTest.Persist.Domain.Repos
{
    public interface IQuestionRepository : IRepository<Question, string>
    {
    }
}
