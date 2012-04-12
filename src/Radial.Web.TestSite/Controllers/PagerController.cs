using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Radial.Web.TestSite.Models;
using Radial.Web.Mvc.Pagination;

namespace Radial.Web.TestSite.Controllers
{
    public class PagerController : Controller
    {
        //
        // GET: /Pager/

        public ActionResult Index(int? pid = 1)
        {
            IList<PagedItem> olist = new List<PagedItem>();
            for (int i = 1; i <= 100; i++)
            {
                olist.Add(new PagedItem { Id = i });
            }

            PagedList<PagedItem> list = new PagedList<PagedItem>(olist.Skip(10 * (pid.Value - 1)).Take(10).ToList(), pid.Value, 10, olist.Count);

            return View(list);
        }


        public ActionResult Ajax(int? pid = 1)
        {
            IList<PagedItem> olist = new List<PagedItem>();
            for (int i = 1; i <= 100; i++)
            {
                olist.Add(new PagedItem { Id = i });
            }

            PagedList<PagedItem> list = new PagedList<PagedItem>(olist.Skip(10 * (pid.Value - 1)).Take(10).ToList(), pid.Value, 10, olist.Count);
            if (Request.IsAjaxRequest())
                return PartialView("_PartialAjax", list);
            return View(list);
        }
    }
}
