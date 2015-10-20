using System.Web.Http;

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
