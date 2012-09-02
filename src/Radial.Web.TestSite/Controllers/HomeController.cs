using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Radial.Param;
using Radial.Web.Mvc;
using System.Data;
using Radial.Web.Mvc.Filters;

namespace Radial.Web.TestSite.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Param()
        {
            return Content(AppParam.GetValue("sinaweibo.appkey"));
        }

        public ActionResult Json()
        {
            return this.NewJson(new { id = "4324234" });
        }

        public ActionResult ExportExecel()
        {
            DataTable dt=new DataTable();
            dt.Columns.Add("日期");
            dt.Columns.Add("用户名");

            dt.Rows.Add(DateTime.Now,"测试账户1");
            dt.Rows.Add(DateTime.Now,"测试账户2");
            dt.Rows.Add(DateTime.Now,"测试账户3");


            return this.Excel(dt, "测试输出");
        }

        public ActionResult Alter()
        {
            return this.Alert("你好,要跳转喽~", Url.Action("Index"));
        }

        public ActionResult FireError()
        {
            return Content(System.IO.File.ReadAllText("C:\\" + Guid.NewGuid().ToString("n") + ".txt"));
        }

        public ActionResult FireError2()
        {
            try
            {
                return Content(System.IO.File.ReadAllText("C:\\" + Guid.NewGuid().ToString("n") + ".txt"));
            }
            catch
            {
                return this.KnownFault(100, "io error!");
            }
        }
    }
}
