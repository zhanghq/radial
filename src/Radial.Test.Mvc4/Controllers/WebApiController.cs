using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

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

    }
}
