using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Radial.Data.Nhs.Mvc;
using Radial.Web.OpenApi.SDK;
using Radial.Web.OpenApi;
using Radial.Web.Mvc;
using System.Collections.Specialized;
using Radial.Net;

namespace Radial.Web.TestSite.Controllers
{
    public class TencentController : Controller
    {
        //
        // GET: /Tencent/
        TencentWeibo _client;

        public TencentController()
        {
            _client = new TencentWeibo();
        }

        //[HandleDataSession]
        public ActionResult Index()
        {
            ViewBag.AuthUrl = _client.GetAuthorizationUrlWithCode(HttpKits.MakeAbsoluteUrl("~/tencent/callback"), null);
            return View();
        }

        //[HandleDataSession]
        public ActionResult Callback(string code, string openid, string openkey)
        {
            NameValueCollection nvc = new NameValueCollection();
            string access_token = _client.GetAccessTokenWithCode(code, HttpKits.MakeAbsoluteUrl("~/tencent/callback"), out nvc);
            _client.SetAccessToken(access_token);
            _client.SetOpenId(openid);
            //_client.SetClientIp("218.81.82.138");

            IDictionary<string, dynamic> args = new Dictionary<string, dynamic>();
            //args.Add("format", "json");
            args.Add("content", "sdafsdf你好!@#$%……~&*（）-=+" + Guid.NewGuid().ToString("n"));

            HttpResponseObj obj = _client.Get("http://open.t.qq.com/api/t/add", args);

            return this.NewJson(new { openid = openid, openkey = openkey, resp = obj.Text });
        }

    }
}
