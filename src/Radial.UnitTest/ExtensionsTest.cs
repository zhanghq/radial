using NUnit.Framework;
using System.Collections.Generic;

namespace Radial.UnitTest
{
    [TestFixture]
    public class ExtensionsTest
    {
        [Test]
        public void Remove()
        {
            IList<dynamic> list = new List<dynamic>();
            list.Add(new { a = 2, b = 3 });
            list.Add(new { a = 12, b = 13 });
            list.Add(new { a = 1, b = 4 });
            list.Add(new { a = 14, b = 15 });

            list.Remove(o => o.a > 10 && o.b > o.a);
        }
    }
}
