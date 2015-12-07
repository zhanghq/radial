using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial.UnitTest.Persist.Efs.Domain
{
    class Article
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
