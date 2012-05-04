using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Radial.Data.Nhs.Mvc;
using Radial.Web.OpenApi.SDK;
using Radial.Web.OpenApi;
using Radial.Web.Mvc;

namespace Radial.Web.TestSite.Controllers
{
    public class TencentController : Controller
    {
        //
        // GET: /Tencent/

        [HandleDataSession]
        public ActionResult Index()
        {
            ViewBag.AuthUrl = TencentWeibo.Default.GetAuthorizationUrlWithCode(HttpKits.MakeAbsoluteUrl("~/tencent/callback"), null);
            return View();
        }

        [HandleDataSession]
        public ActionResult Callback(string code, string openid, string openkey)
        {
            int expires_in;

            KeySecretPair pair = TencentWeibo.Default.GetAccessTokenWithCode(code, HttpKits.MakeAbsoluteUrl("~/tencent/callback"), out expires_in);
            TencentWeibo.Default.SetAccessToken(pair);

            return this.NewJson(new { openid = openid, openkey = openkey });
        }

    }
}
