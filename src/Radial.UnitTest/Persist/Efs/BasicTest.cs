using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Radial.Persist;
using Radial.UnitTest.Persist.Domain;
using Radial.UnitTest.Persist.Domain.Repos;

namespace Radial.UnitTest.Persist.Efs
{
    [TestFixture]
    public class BasicTest
    {
        [Test]
        public void Insert()
        {
            using (var uow = Dependency.Container.ResolveUnitOfWork(Storages.Db0.Alias))
            {
                IList<Question> qs = new List<Question>();

                for (int i = 0; i < 20; i++)
                {
                    qs.Add(new QuestionYW
                    {
                        Id=Radial.RandomCode.GetIdentityKey(),
                        Subject = "语文",
                        Phase = "初中",
                        CreateTime = DateTime.Now
                    });
                    qs.Add(new QuestionSX
                    {
                        Id = Radial.RandomCode.GetIdentityKey(),
                        Subject = "数学",
                        Phase = "初中",
                        CreateTime = DateTime.Now
                    });
                    qs.Add(new QuestionYY
                    {
                        Id = Radial.RandomCode.GetIdentityKey(),
                        Subject = "英语",
                        Phase = "初中",
                        CreateTime = DateTime.Now
                    });
                }
                uow.RegisterNew<Question>(qs);
                uow.Commit();
            }
        }

        [Test]
        public void Select()
        {
            using (var uow = Dependency.Container.ResolveUnitOfWork(Storages.Db0.Alias))
            {
                var repo = uow.ResolveRepository<IQuestionRepository>();

                var qs = repo.FindAll(o => o.Subject == "数学");

                Assert.True(qs.All(o => o.GetType() == typeof(QuestionSX)));
            }
        }

        [Test]
        public void Update()
        {
            using (var uow = Dependency.Container.ResolveUnitOfWork(Storages.Db0.Alias))
            {
                var repo = uow.ResolveRepository<IQuestionRepository>();

                var q = repo.FindFirst(o => o.Subject == "数学");

                q.Phase = "高中";

                uow.RegisterUpdate(q);

                uow.Commit();

                var q2 = repo[q.Id];

                Assert.AreEqual(q2.Phase, q.Phase);
            }
        }

        [Test]
        public void Delete()
        {
            using (var uow = Dependency.Container.ResolveUnitOfWork(Storages.Db0.Alias))
            {
                var repo = uow.ResolveRepository<IQuestionRepository>();
                var q = repo.FindFirst(o => o.Subject == "语文");
                //uow.RegisterDelete(q);
                uow.RegisterDelete<Question, string>(q.Id);
                uow.Commit();

                var q2 = repo[q.Id];
                Assert.IsNull(q2);
            }
        }
    }
}
