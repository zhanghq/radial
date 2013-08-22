using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

                Assert.AreEqual(2, userRepository.FindByKeys(new int[] { id1, id2 }).Count);
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
    }
}
