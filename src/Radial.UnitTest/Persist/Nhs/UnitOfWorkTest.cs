using System;
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
        [OneTimeSetUp]
        public void SetUp()
        {
            //using (IUnitOfWork uow = new NhUnitOfWork())
            //{
            //    for (int i = 0; i < 100; i++)
            //        uow.RegisterNew<User>(new User { Id = RandomCode.NewInstance.Next(1, int.MaxValue), Name = "测试" });

            //    uow.Commit();
            //}
        }

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


                ////测试是否会更新
                //User u = userRepository.FindFirst(new OrderBySnippet<User>(o => o.Id));

                //u.Name = RandomCode.Create(10);

                //userRepository.Save(u);

                //User u2 = userRepository.FindFirst(new OrderBySnippet<User>(o => o.Id, false));

                //u2.Name = RandomCode.Create(10);

                //userRepository.Save(u2);

                //测试此方法是否会在提交前执行
                uow.RegisterDelete<User, int>(2323);

                uow.Commit();
            }
        }

        [Test]
        public void Test2()
        {
            //测试Remove(System.Linq.Expressions.Expression<Func<TObject, bool>> condition)

            using (IUnitOfWork uow = new NhUnitOfWork())
            {
                UserRepository userRepository = new UserRepository(uow);

                userRepository.Remove(o => o.Id > RandomCode.NewInstance.Next(10000, int.MaxValue));

                uow.Commit();
            }
        }
        [Test]
        public void Test3()
        {
            //测试事物回滚

            using (IUnitOfWork uow = new NhUnitOfWork())
            {

                uow.RegisterNew<User>(new User { Id = RandomCode.NewInstance.Next(1, int.MaxValue), Name = "测试" });
                //order 保存出错
                uow.RegisterNew<Order>(new Order { Id = RandomCode.NewInstance.Next(1, int.MaxValue), Amount = 100, Time = DateTime.Now });

                uow.Commit();
            }
        }
    }
}
