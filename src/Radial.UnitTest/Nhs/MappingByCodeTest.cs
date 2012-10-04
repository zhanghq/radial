using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Radial.Data.Nhs;
using Radial.UnitTest.Nhs.Repository;
using Radial.UnitTest.Nhs.Domain;
using Radial.UnitTest.Nhs.PoolInitializer;
using NHibernate;
using Radial.Data;

namespace Radial.UnitTest.Nhs
{
    [TestFixture]
    public class MappingByCodeTest
    {
        [TestFixtureSetUp]
        public void SetUp()
        {

            CleanUp();
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            CleanUp();
        }

        public void CleanUp()
        {
            using (IUnitOfWork uow = new NhUnitOfWork())
            {
                uow.RegisterClear<User>();
                uow.Commit(true);
            }
        }


        [Test]
        public void Save()
        {
            using (IUnitOfWork uow = new NhUnitOfWork())
            {
                UserRepository userRepository = new UserRepository(uow);

                uow.RegisterNew<User>(new User { Id = 1, Name = "测试" });

                //uow.RegisterDelete<User, int>(1);

                uow.Commit(true);

                User u = userRepository.Find(1);

                Console.WriteLine(u.Name);
            }
        }
    }
}
