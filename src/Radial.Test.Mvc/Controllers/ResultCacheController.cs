using Radial.Web.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;

namespace Radial.Test.Mvc.Controllers
{
    public class ResultCacheController : Controller
    {
        //
        // GET: /ResultCache/
        [ResultCache]
        public ActionResult Index()
        {
            System.Net.WebClient c = new System.Net.WebClient();

            ViewBag.Html = c.DownloadString("http://www.sina.com.cn/");
            return View();
        }

        public ActionResult Remove()
        {
            IResultCacheable c = Components.Container.Resolve<IResultCacheable>();

            c.BatchRemove("b");

            return Content("ok");
        }

    }
}
