using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Radial.Boot;

namespace Radial.Test.Mvc3
{
    public class Startup : IBootTask
    {

        /// <summary>
        /// System initialize process.
        /// </summary>
        public void Initialize()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }

        private void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        private void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // 路由名称
                "{controller}/{action}/{id}", // 带有参数的 URL
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // 参数默认值
            );

        }

        /// <summary>
        /// Start system.
        /// </summary>
        public void Start()
        {
        }

        /// <summary>
        /// Stop system.
        /// </summary>
        public void Stop()
        {
        }
    }
}