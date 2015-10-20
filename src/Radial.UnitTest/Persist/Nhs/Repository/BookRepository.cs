using Radial.Persist;
using Radial.Persist.Nhs;
using Radial.UnitTest.Persist.Nhs.Domain;

namespace Radial.UnitTest.Persist.Nhs.Repository
{
    class BookRepository : BasicRepository<Book,int>
    {
        public BookRepository(IUnitOfWork uow)
            : base(uow)
        {
        }
    }
}
