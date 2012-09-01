using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Radial.Data.Nhs;
using NHibernate;
using Radial.UnitTest.Nhs.Domain;
using Radial.UnitTest.Nhs.Repository;
using Radial.UnitTest.Nhs.PoolInitializer;
using Radial.Data;

namespace Radial.UnitTest.Nhs
{
    [TestFixture]
    public class BasicTest
    {
        IUnitOfWork _uow;

        [TestFixtureSetUp]
        public void SetUp()
        {
            _uow = new NhUnitOfWork();
            CleanUp();
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            CleanUp();
            _uow.Dispose();
        }

        public void CleanUp()
        {
            _uow.RegisterClear<User>();
            _uow.Commit();
        }


        [Test]
        public void Save()
        {
            UserRepository userRepository = new UserRepository(_uow);

            _uow.RegisterNew<User>(new User { Id = 1, Name = "测试" });

            _uow.Commit();

            User u = userRepository.Find(1);
            Console.WriteLine(u.Name);

        }


    }
}
