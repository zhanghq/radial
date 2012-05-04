﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Radial.Web.OpenApi;
using Radial.Web.OpenApi.SDK;
using Radial.Web.Mvc;
using Radial.Net;
using Radial.Data.Nhs.Mvc;

namespace Radial.Web.TestSite.Controllers
{
    public class SinaController : Controller
    {
        SinaWeibo2 _client;

        public SinaController()
        {
            _client = new SinaWeibo2();
        }

        //
        // GET: /Sina/
        //[HandleDataSession]
        public ActionResult Index()
        {
            ViewBag.AuthUrl = _client.GetAuthorizationUrlWithCode(HttpKits.MakeAbsoluteUrl("~/sina/callback"), string.Empty, string.Empty);
            return View();
        }

        //[HandleDataSession]
        public ActionResult Callback(string code)
        {
            dynamic respData;

            string access_token = _client.GetAccessTokenWithCode(code, HttpKits.MakeAbsoluteUrl("~/sina/callback"), out respData);
            _client.SetAccessToken(access_token);

            IDictionary<string, dynamic> args = new Dictionary<string, dynamic>();
            args.Add("status", "sdf你好!@#$%……~&*（）-=+" + Guid.NewGuid().ToString("n"));

            HttpResponseObj obj = _client.Post("https://api.weibo.com/2/statuses/update.json", args);

            List<IMultipartFormData> postdatas = new List<IMultipartFormData>();
            postdatas.Add(new PlainTextFormData("status", "sdafsdf你好!@#$%……~&*（）-=+" + Guid.NewGuid().ToString("n")));
            postdatas.Add(new FileFormData(@"D:\Pictures\460.jpg", "pic"));

            obj = _client.Post("https://api.weibo.com/2/statuses/upload.json", postdatas.ToArray());
            return this.NewJson(new { uid = respData.uid });
        }
    }
}
