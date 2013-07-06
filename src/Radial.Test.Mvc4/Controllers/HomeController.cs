using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Radial.Web;

namespace Radial.Test.Mvc.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            GeneralUpload uploader = new GeneralUpload();
            var result = uploader.Save(file, "Uploads", true);

            if (result.State == UploadState.Succeed)
                return Content(Path.GetFileName(result.FilePath));

            return Content("0");
        }
    }
}
