using NUnit.Framework;
using Radial.Persist;
using Radial.Persist.Nhs;
using Radial.UnitTest.Persist.Nhs.Domain;
using Radial.UnitTest.Persist.Nhs.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;

namespace Radial.UnitTest.Persist.Nhs
{
    [TestFixture]
    public class SemiautoTest
    {
        [TestFixtureSetUp]
        public void SetUp()
        {
            Dependency.Container.RegisterType<IFactoryPoolInitializer, SemiautoPoolInitializer>();
        }

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
    }
}
