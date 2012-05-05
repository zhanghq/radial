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

        [HandleDataSession]
        public ActionResult Index()
        {
            TencentWeibo _client = new TencentWeibo();
            ViewBag.AuthUrl = _client.GetAuthorizationUrlWithCode(HttpKits.MakeAbsoluteUrl("~/tencent/callback"), null);
            return View();
        }

        [HandleDataSession]
        public ActionResult Callback(string code, string openid, string openkey)
        {
            TencentWeibo _client = new TencentWeibo();
            NameValueCollection nvc = new NameValueCollection();
            string access_token = _client.GetAccessTokenWithCode(code, HttpKits.MakeAbsoluteUrl("~/tencent/callback"), out nvc);
            //if not use GetAccessTokenWithCode, you must call SetAccessToken before request api
            //_client.SetAccessToken(access_token);
            _client.SetOpenId(openid);
            _client.SetClientIp(Request.UserHostAddress);

            IDictionary<string,dynamic> args=new Dictionary<string,dynamic>();
            args.Add("content","sdf你好!@#$%……~&*（）-=+" + Guid.NewGuid().ToString("n"));

            HttpResponseObj obj1 = _client.Post("https://open.t.qq.com/api/t/add", args);

            List<IMultipartFormData> postdatas = new List<IMultipartFormData>();
            postdatas.Add(new PlainTextFormData("content", "sdf你好!@#$%……~&*（）-=+" + Guid.NewGuid().ToString("n")));
            postdatas.Add(new FileFormData(@"D:\Pictures\460.jpg", "pic"));

            HttpResponseObj obj2 = _client.Post("https://open.t.qq.com/api/t/add_pic", postdatas.ToArray());

            return this.NewJson(new { openid = openid, openkey = openkey, resp1 = obj1.Text, resp2 = obj2.Text });
        }

    }
}
