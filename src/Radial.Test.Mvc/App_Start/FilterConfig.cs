using System.Web.Mvc;

namespace Radial.Test.Mvc
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new Web.Mvc.Filters.HandleExceptionAttribute());
        }
    }
}