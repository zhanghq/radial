using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Radial.Persist;
using Radial.UnitTest.Persist.Domain;
using Radial.Persist.Nhs;

namespace Radial.UnitTest.Persist.Nhs
{
    [TestFixture]
    public class BasicTest
    {
        [Test]
        public void Insert()
        {
            IList<Question> qs = new List<Question>();
            qs.Add(new Question
            {
                Subject = "语文",
                Phase = "初中",
                CreateTime = DateTime.Now
            });
            qs.Add(new Question
            {
                Subject = "数学",
                Phase = "初中",
                CreateTime = DateTime.Now
            });
            qs.Add(new Question
            {
                Subject = "英语",
                Phase = "初中",
                CreateTime = DateTime.Now
            });

            var qsrouter = Dependency.Container.ResolveStorageRouter<Question>();
            using (System.Transactions.TransactionScope ts = new System.Transactions.TransactionScope())
            {
                foreach (var q in qs)
                {
                    using (var uow = new UnitOfWork(qsrouter.GetStorageAlias(q)))
                    {
                        uow.RegisterNew(q);
                    }
                }

                ts.Complete();
            }
        }
    }
}
