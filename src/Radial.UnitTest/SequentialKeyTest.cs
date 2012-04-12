using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Threading.Tasks;
using Radial.Data.Nhs;
using NHibernate;
using Radial.Data.Nhs.Key;

namespace Radial.UnitTest
{
    [TestFixture]
    public class SequentialKeyTest
    {

        [Test]
        public void NextKey()
        {
            using (ISession session = HibernateEngine.OpenSession())
            {
                ISequentialKeyBuilder builder = new DefaultSequentialKeyBuilder(new SqlServerSequentialKeyRepository(session));


                Parallel.For(0, 20, i =>
                {
                    Console.WriteLine("P{0}={1}", i, builder.Next<string>());
                });
            }
        }
    }
}
