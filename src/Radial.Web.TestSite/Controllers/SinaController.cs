using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Radial.Web.OpenApi.Sina;
using Radial.Web.OpenApi;
using Radial.Web.Mvc;
using Radial.Net;
using Radial.Data.Nhs.Mvc;

namespace Radial.Web.TestSite.Controllers
{
    public class SinaController : Controller
    {
        //
        // GET: /Sina/
        [HandleDataSession]
        public ActionResult Index()
        {
            ViewBag.AuthUrl = SinaWeiboSDK.Default.GetAuthorizationUrlWithCode(HttpKits.MakeAbsoluteUrl("~/sina/callback"), string.Empty, string.Empty);
            return View();
        }

        [HandleDataSession]
        public ActionResult Callback(string code)
        {
            int expires_in;
            int remind_in;
            long uid;

            string access_token= SinaWeiboSDK.Default.GetAccessTokenWithCode(code, HttpKits.MakeAbsoluteUrl("~/sina/callback"), out expires_in, out remind_in, out uid);

            //SinaWeiboSDK.Default.SetAccessToken(access_token);


            //IDictionary<string, dynamic> args = new Dictionary<string, dynamic>();
            //args.Add("status", "sdf你好!@#$%……~&*（）-=+" + Guid.NewGuid().ToString("n"));

            //HttpResponseObj obj = SinaWeibo2.Default.Post("https://api.weibo.com/2/statuses/update.json", args);

            //List<IMultipartFormData> postdatas = new List<IMultipartFormData>();
            //postdatas.Add(new PlainTextFormData("status", "sdafsdf你好!@#$%……~&*（）-=+" + Guid.NewGuid().ToString("n")));
            //postdatas.Add(new FileFormData(@"D:\Pictures\460.jpg", "pic"));

            //obj = SinaWeibo2.Default.Post("https://api.weibo.com/2/statuses/upload.json", postdatas.ToArray());
            return this.NewJson(new { uid = uid });
        }
    }
}
