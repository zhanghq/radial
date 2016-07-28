using System.Net.Http;
using System.Web.Http;
using Radial.Web.Http;
using System.Collections.Generic;

namespace Radial.Test.Mvc.Controllers
{
    public class WebApiController : ApiController
    {
        //
        // GET: /Api/

        public string GetName()
        {
            return "Goft";
        }

        [HttpGet]
        public string StdJson_Exception()
        {
            int a = 0;
            int x = 2 / a;
            return x.ToString();
        }
        [HttpGet]
        public HttpResponseMessage StdJson_Error()
        {
            return this.StdErrorJson();
        }

        [HttpGet]
        public HttpResponseMessage StdJson_Success()
        {
            return this.StdSuccessJson(1);
        }
    }
}
