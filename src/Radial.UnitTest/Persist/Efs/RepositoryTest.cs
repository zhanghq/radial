using NUnit.Framework;
using Radial.Persist;
using Radial.Persist.Efs;
using Radial.UnitTest.Persist.Efs.Domain;
using Radial.UnitTest.Persist.Efs.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial.UnitTest.Persist.Efs
{
    [TestFixture]
    public class RepositoryTest: EfTestBase
    {
        [Test]
        public void Exist()
        {
            using (IUnitOfWork uow = new UnitOfWork())
            {
                IArticleRepository repo = new ArticleRepository(uow);

                Assert.IsFalse(repo.Exist("abc"));
                Assert.IsFalse(repo.Exist(o => o.Title == "2342"));
            }
        }

        [Test]
        public void Find()
        {
            var id = Radial.TimingSeq.Next();
            using (IUnitOfWork uow = new UnitOfWork())
            {
                uow.RegisterNew<Article>(new Article
                {
                    Id = id,
                    Content = "test"
                });

                uow.Commit();
            }
            using (IUnitOfWork uow = new UnitOfWork())
            {
                IArticleRepository repo = new ArticleRepository(uow);

                var a = repo.Find(id);

                Assert.NotNull(a);

                a = repo[id];

                Assert.NotNull(a);

                a = repo.Find(o => o.Id == id);

                Assert.NotNull(a);
            }
        }
    }
}
