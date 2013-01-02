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
using Autofac;
using System.Transactions;

namespace Radial.UnitTest.Nhs
{
    [TestFixture]
    public class ContextualMappingByCodeTest
    {

        [TestFixtureSetUp]
        public void SetUp()
        {
            ComponentContainer.RegisterPerThread<IFactoryPoolInitializer, MappingByCodeFactoryPoolInitializer>();
            HibernateEngine.OpenAndBindSession();
            CleanUp(false);
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            CleanUp(true);
        }

        public void CleanUp(bool dispose)
        {
            using (IUnitOfWork uow = new ContextualUnitOfWork())
            {
                uow.RegisterClear<User>();
                uow.Commit();
            }

            if (dispose)
            {
                ISession session = HibernateEngine.UnbindSession();
                session.Dispose();
            }
        }


        [Test]
        public void Save()
        {
            using (IUnitOfWork uow = new ContextualUnitOfWork())
            {
                UserRepository userRepository = new UserRepository(uow);

                uow.RegisterSave<User>(new User { Id = 1, Name = "测试" });

                //uow.RegisterDelete<User, int>(1);

                uow.Commit();

            }
        }
    }
}
