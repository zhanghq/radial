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

        [Test]
        public void FindAll()
        {
            using (IUnitOfWork uow = new UnitOfWork())
            {
                IArticleRepository repo = new ArticleRepository(uow);
                for (int i = 0; i < 10; i++)
                {
                    repo.Add(new Article
                    {
                        Id = Radial.TimingSeq.Next(),
                        Content = "test"
                    });
                }

                uow.Commit();
            }

            using (IUnitOfWork uow = new UnitOfWork())
            {
                IArticleRepository repo = new ArticleRepository(uow);

                Assert.DoesNotThrow(() =>
                {
                    var objs = repo.FindAll(new OrderBySnippet<Article>(o => o.Id, false));

                    objs = repo.FindAll(o => o.Content == "test", new OrderBySnippet<Article>(o => o.Id, false));
                    objs = repo.FindAll(o => o.Content == "test", new OrderBySnippet<Article>[] { new OrderBySnippet<Article>(o => o.Id, false) }, 10);

                    int objectTotal;

                    objs = repo.FindAll(o => o.Content == "test", new OrderBySnippet<Article>[] { new OrderBySnippet<Article>(o => o.Id, false) }, 10, 1, out objectTotal);

                });
            }
        }

        [Test]
        public void FindFirst()
        {
            using (IUnitOfWork uow = new UnitOfWork())
            {
                IArticleRepository repo = new ArticleRepository(uow);
                repo.FindFirst(new OrderBySnippet<Article>(o => o.Id, false));
            }
        }
    }
}
