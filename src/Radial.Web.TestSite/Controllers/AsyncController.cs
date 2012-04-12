using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Radial.Web.TestSite.Controllers
{
    public class AsyncController : Controller
    {
        //
        // GET: /Async/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string name)
        {
            return Content("Greetings," + name);
        }

    }
}
