using NUnit.Framework;
using Radial.Persist;
using Radial.Persist.Nhs;
using Radial.UnitTest.Persist.Nhs.Domain;
using System;

namespace Radial.UnitTest.Persist.Nhs
{
    [TestFixture]
    public class IdentifierTest
    {
        [Test]
        public void Create()
        {
            using (IUnitOfWork uow = new NhUnitOfWork())
            {
                Team t = new Team();
                //当Id赋值时，则从赋值，若没赋值，则自动产生Id
                t.Id = Guid.NewGuid().ToString("n").Substring(0, 15).ToLower();
                t.Name = "adfadf";
                uow.RegisterNew(t);
                uow.Commit();
            }
        }
    }
}
