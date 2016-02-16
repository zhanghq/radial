using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial.UnitTest.Persist.Domain
{

    public class Question : ModelBase
    {
        public string Subject { get; set; }
        public string Phase { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
