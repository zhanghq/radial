using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using NUnit.Framework;
using Radial.Persist;
using Radial.Persist.Nhs;
using Radial.UnitTest.Persist.Nhs.Domain;
using Radial.UnitTest.Persist.Nhs.Repository;

namespace Radial.UnitTest.Persist.Nhs
{
    [TestFixture]
    public class UnitOfWorkTest
    {
        [Test]
        public void Test1()
        {
            using (IUnitOfWork uow = new NhUnitOfWork())
            {
                UserRepository userRepository = new UserRepository(uow);

                //测试是否会批量添加
                userRepository.Add(new User { Id = RandomCode.NewInstance.Next(1, int.MaxValue), Name = "测试" });
                userRepository.Add(new User { Id = RandomCode.NewInstance.Next(1, int.MaxValue), Name = "测试" });

                //测试未提交前，添加到Repository是否可以被查到
                int id3 = RandomCode.NewInstance.Next(1, int.MaxValue);
                userRepository.Add(new User { Id = id3, Name = "测试3" });

                User ut = userRepository[id3];

                Console.WriteLine(ut.Name);


                //测试是否会更新
                User u = userRepository.FindFirst(new OrderBySnippet<User>(o => o.Id));

                u.Name = RandomCode.Create(10);

                userRepository.Save(u);

                User u2 = userRepository.FindFirst(new OrderBySnippet<User>(o => o.Id, false));

                u2.Name = RandomCode.Create(10);

                userRepository.Save(u2);

                uow.Commit();
            }
        }
    }
}
