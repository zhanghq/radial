using NUnit.Framework;
using Radial.Persist.Nhs;
using Radial.UnitTest.Persist.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial.UnitTest.Persist.Nhs
{
    [TestFixture]
    public class SimpleTest
    {
        [Test]
        public void Insert()
        {
            using(var uow=new UnitOfWork())
            {
                uow.RegisterNew<Question>(new Question
                {
                    Subject = "语文",
                    Phase="初中",
                    CreateTime = DateTime.Now
                });
                uow.RegisterNew<Question>(new Question
                {
                    Subject = "数学",
                    Phase = "初中",
                    CreateTime = DateTime.Now
                });
                uow.RegisterNew<Question>(new Question
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
