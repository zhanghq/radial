using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Radial.Web.OpenApi;
using Radial.Web.OpenApi.SDK;
using Radial.Web.Mvc;
using Radial.Net;
using Radial.Data.Nhs.Mvc;
using System.Collections.Specialized;
using System.Threading;

namespace Radial.Web.TestSite.Controllers
{
    public class SinaController : Controller
    {
        //
        // GET: /Sina/
        [HandleDataSession]
        public ActionResult Index()
        {
            SinaWeibo2 _client = new SinaWeibo2();
            ViewBag.AuthUrl = _client.GetAuthorizationUrlWithCode(HttpKits.MakeAbsoluteUrl("~/sina/callback"), string.Empty, string.Empty);
            return View();
        }

        [HandleDataSession]
        public ActionResult Callback(string code)
        {
            NameValueCollection otherResponseData;

            SinaWeibo2 _client = new SinaWeibo2();

            string access_token = _client.GetAccessTokenWithCode(code, HttpKits.MakeAbsoluteUrl("~/sina/callback"), out otherResponseData);
            //if not use GetAccessTokenWithCode, you must call SetAccessToken before request api
            //_client.SetAccessToken(access_token);

            IDictionary<string, dynamic> args = new Dictionary<string, dynamic>();
            args.Add("status", "sdf你好!@#$%……~&*（）-=+" + Guid.NewGuid().ToString("n"));

            HttpResponseObj obj1 = _client.Post("https://api.weibo.com/2/statuses/update.json", args);

            Thread.Sleep(5000);

            List<IMultipartFormData> postdatas = new List<IMultipartFormData>();
            postdatas.Add(new PlainTextFormData("status", "sdafsdf你好!@#$%……~&*（）-=+" + Guid.NewGuid().ToString("n")));
            postdatas.Add(new FileFormData(Server.MapPath("~/Images/460.jpg"), "pic"));

            HttpResponseObj obj2 = _client.Post("https://api.weibo.com/2/statuses/upload.json", postdatas.ToArray());
            return this.NewJson(new { uid = otherResponseData["uid"], resp1 = obj1.Text, resp2 = obj2.Text });
        }
    }
}
