using NUnit.Framework;
using Radial.Persist.Nhs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial.UnitTest.Persist.Nhs
{
    [TestFixture]
    public class SchemaBuilderTest
    {
        [Test]
        public void Create()
        {
            SchemaBuilder scb = new SchemaBuilder();
            scb.Create();
        }

        [Test]
        public void Drop()
        {
            SchemaBuilder scb = new SchemaBuilder();
            scb.Drop();
        }

        [Test]
        public void Validate()
        {
            SchemaBuilder scb = new SchemaBuilder();
            scb.Drop();
            Assert.Throws<NHibernate.HibernateException>(() => scb.Validate());
            //scb.Create();
            //Assert.DoesNotThrow(() => scb.Validate());
            //scb.Drop();
        }

        [Test]
        public void Update()
        {
            SchemaBuilder scb = new SchemaBuilder();
            scb.Update();
        }

        [Test]
        public void Export()
        {
            SchemaBuilder scb = new SchemaBuilder();

            Console.WriteLine(scb.Export());
        }
    }
}
