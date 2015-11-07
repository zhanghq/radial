using System;
using System.Collections.Generic;
using NUnit.Framework;
using Radial.Persist;
using Radial.Persist.Nhs;
using Radial.UnitTest.Persist.Nhs.Domain;
using Radial.UnitTest.Persist.Nhs.Repository;
using System.Data;

namespace Radial.UnitTest.Persist.Nhs
{
    [TestFixture]
    public class RepositoryTest
    {
        [Test]
        public void Exist()
        {
            User u = new User { Id = RandomCode.NewInstance.Next(1, int.MaxValue), Name = "Name" };
            using (IUnitOfWork uow = new NhUnitOfWork())
            {
                uow.RegisterNew<User>(u);
                uow.Commit();
            }

            using (IUnitOfWork uow = new NhUnitOfWork())
            {
                UserRepository userRepository = new UserRepository(uow);

                Assert.IsTrue(userRepository.Exist(u.Id));
            }
        }

        [Test]
        public void FindAll()
        {
            using (IUnitOfWork uow = new NhUnitOfWork())
            {
                UserRepository userRepository = new UserRepository(uow);

                int total;

                IList<User> users = userRepository.FindAll(null, new OrderBySnippet<User>[] { new OrderBySnippet<User>(o => o.Id, false) }, 5, 1, out total);

                Assert.IsNotEmpty(users);
            }
        }

        [Test]
        public void FindByKeys()
        {
            int id1 = RandomCode.NewInstance.Next(1, int.MaxValue);
            int id2 = RandomCode.NewInstance.Next(1, int.MaxValue);

            using (IUnitOfWork uow = new NhUnitOfWork())
            {
                uow.RegisterNew<User>(new User { Id = id1, Name = "Name" });
                uow.RegisterNew<User>(new User { Id = id2, Name = "Name" });
                uow.Commit();
            }

            using (IUnitOfWork uow = new NhUnitOfWork())
            {
                UserRepository userRepository = new UserRepository(uow);
                var uss = userRepository.FindByKeys(new int[] { id1, id2 });
                Assert.AreEqual(2, uss.Count);
            }
        }

        [Test]
        public void FindDataTable()
        {
            using (IUnitOfWork uow = new NhUnitOfWork())
            {
                UserRepository userRepository = new UserRepository(uow);

                DataTable dt = userRepository.FindAllDataTable();

                Assert.NotNull(dt);

                Console.WriteLine(string.Join(",", dt.Columns[0].ColumnName, dt.Columns[1].ColumnName));

                foreach (DataRow row in dt.Rows)
                {
                    Console.WriteLine(string.Join(",", row.ItemArray));
                }
            }
        }

        [Test]
        public void ExtraCondition()
        {
            using (IUnitOfWork uow = new NhUnitOfWork())
            {
                OrderRepository repo = new OrderRepository(uow);
                //观察输出的Sql
                var objs = repo.FindAll();
            }
        }

        [Test]
        public void TimingIdGenerator()
        {
            using (IUnitOfWork uow = new NhUnitOfWork())
            {
                Book b = new Book { Name = "342" };
                uow.RegisterNew<Book>(b);
                uow.Commit();
            }
        }
    }
}
