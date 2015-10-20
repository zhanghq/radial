using Radial.Web.WebApi.Formatting;
using System.Web.Http;

namespace Radial.Test.Mvc
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{action}/{id}",
                defaults: new { controller = "WebApi", id = RouteParameter.Optional }
            );

            config.Formatters.Clear();
            config.Formatters.Add(new NewJsonMediaTypeFormatter());
            config.Formatters.Add(new TextMediaTypeFormatter());
        }
    }
}
