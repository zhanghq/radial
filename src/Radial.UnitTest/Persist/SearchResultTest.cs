using NUnit.Framework;
using Radial.Persist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial.UnitTest.Persist
{
    [TestFixture]
    public class SearchResultTest
    {
        [Test]
        public void Serialization()
        {
            var srt = new SearchResult<IList<int>>();
            srt.Data = new List<int>();
            srt.Data.Add(100);
            srt.ObjectTotal = 100;
            srt.PageSize = 15;

            string json = Radial.Serialization.JsonSerializer.Serialize(srt);

            var srt1 = Radial.Serialization.JsonSerializer.Deserialize<SearchResult<IList<int>>>(json);

            Assert.AreEqual(srt.PageTotal, srt1.PageTotal);
        }
    }
}
