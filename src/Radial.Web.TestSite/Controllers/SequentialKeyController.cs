using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Radial.Data.Nhs.Mvc;

namespace Radial.Web.TestSite.Controllers
{
    public class SequentialKeyController : Controller
    {
        SequentialKeyBuilderBase _builder;
        //
        // GET: /SequentialKey/
        public SequentialKeyController(SequentialKeyBuilderBase builder)
        {
            _builder = builder;
        }

        [HandleDataSession]
        public ActionResult Index()
        {
            return Content(_builder.Next<SequentialKeyController>().ToString());
        }

    }
}
