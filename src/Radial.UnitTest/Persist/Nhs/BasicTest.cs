using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Radial.Persist;
using Radial.UnitTest.Persist.Domain;

namespace Radial.UnitTest.Persist.Nhs
{
    [TestFixture]
    public class BasicTest
    {
        [Test]
        public void Insert()
        {
            using (var uow = Dependency.Container.ResolveUnitOfWork(Storages.Db0.Alias))
            {
                uow.RegisterNew<Question>(new QuestionYW
                {
                    Subject = "语文",
                    Phase = "初中",
                    CreateTime = DateTime.Now
                });
                uow.RegisterNew<Question>(new QuestionSX
                {
                    Subject = "数学",
                    Phase = "初中",
                    CreateTime = DateTime.Now
                });
                uow.RegisterNew<Question>(new QuestionYW
                {
                    Subject = "英语",
                    Phase = "初中",
                    CreateTime = DateTime.Now
                });
                uow.Commit();
            }
        }
    }
}
