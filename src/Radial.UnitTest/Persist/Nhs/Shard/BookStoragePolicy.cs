using Radial.Persist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial.UnitTest.Persist.Nhs.Shard
{
    public class BookStoragePolicy : IStoragePolicy
    {
        public string GetAlias(params object[] keys)
        {
            string id = (string)keys[0];

            int last = int.Parse(id.Last().ToString());

            if (last % 2 == 0)
                return "shard1";
            return "shard2";
        }

        public string[] GetAliases()
        {
            return new string[] { "shard1", "shard2" };
        }
    }
}
