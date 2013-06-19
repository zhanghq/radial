using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookNine.Application;
using BookNine.TransferObject;
using Radial.Web.Mvc;


namespace BookNine.Web.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            UserModel model = UserService.Create(string.Format("{0}@abc.com", Radial.RandomCode.Create(6).ToLower()), "123456");

            ViewBag.ChangePasswordResult = UserService.ChangePassword(model.Id, "123456", "1234567");

            return View(model);
        }

    }
}
