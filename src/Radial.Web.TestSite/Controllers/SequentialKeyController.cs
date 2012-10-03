using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Radial.Web.TestSite.Controllers
{
    public class SequentialKeyController : Controller
    {
        SequentialKeyGenerator _builder;

        //
        // GET: /SequentialKey/
        public SequentialKeyController(SequentialKeyGenerator builder)
        {
            _builder = builder;
        }


        public ActionResult Index()
        {
            return Content(_builder.Next<SequentialKeyController>().ToString());
        }

    }
}
