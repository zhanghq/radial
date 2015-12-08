using Radial.Persist;
using Radial.Persist.Efs;
using Radial.UnitTest.Persist.Efs.Domain;

namespace Radial.UnitTest.Persist.Efs.Repos
{

    interface IArticleRepository : IRepository<Article, string>
    {

    }

    class ArticleRepository : BasicRepository<Article, string>, IArticleRepository
    {
        public ArticleRepository(IUnitOfWorkEssential uow) : base(uow)
        {

        }
    }
}
