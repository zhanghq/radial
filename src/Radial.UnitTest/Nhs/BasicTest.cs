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

namespace Radial.UnitTest.Nhs
{
    [TestFixture]
    public class BasicTest
    {
        [TestFixtureSetUp]
        public void SetUp()
        {
            HibernateEngine.OpenAndBindSession();
            CleanUp();
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            CleanUp();

            ISession session = HibernateEngine.UnbindSession();
            if (session != null)
                session.Dispose();

        }

        public void CleanUp()
        {
            AutoTransaction.Complete(() =>
            {
                UserRepository repository = new UserRepository();

                repository.Clear();

            });
        }


        [Test]
        public void Save()
        {
            UserRepository userRepository = new UserRepository();
            AutoTransaction.Complete(() =>
            {
                userRepository.Save(new User { Id = 1, Name = "测试" });
            });

        }


    }
}
