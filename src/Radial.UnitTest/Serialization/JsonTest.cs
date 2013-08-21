using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial.UnitTest.Serialization
{
    [TestFixture]
    public class JsonTest
    {
        [Test]
        public void DynamicType()
        {
            string jtxt = Radial.Serialization.JsonSerializer.Serialize(new { name = "link", age = 23 });

            dynamic o = Radial.Serialization.JsonSerializer.Deserialize(jtxt);

            Console.WriteLine(o.name);
        }
    }
}
