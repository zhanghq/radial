using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;

namespace Radial.Web.Mvc.Filters
{
    /// <summary>
    /// Indicates the action result can be cached
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited=false)]
    public sealed class ResultCacheAttribute : ActionFilterAttribute
    {
        IResultCacheable _cacheImpl;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResultCacheAttribute"/> class.
        /// </summary>
        public ResultCacheAttribute()
        {
            _cacheImpl = Components.Container.Resolve<IResultCacheable>();
        }


        /// <summary>
        /// Called by the ASP.NET MVC framework before the action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (_cacheImpl == null)
                return;

            if (_cacheImpl.IsMatched(filterContext.RequestContext))
            {
                string html;

                if (_cacheImpl.Get(filterContext.RequestContext, out html))
                {
                    var crs = new ContentResult();
                    crs.Content = html;
                    crs.ContentEncoding = _cacheImpl.Encoding;
                    filterContext.Result = crs;
                }
            }
        }


        /// <summary>
        /// Called by the ASP.NET MVC framework after the action result executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            if (_cacheImpl == null)
                return;

            var vr = filterContext.Result as ViewResult;

            if (vr == null)
                return;

            if (_cacheImpl.IsMatched(filterContext.RequestContext))
            {
                using (var sw = new StringWriter())
                {
                    var vc = new ViewContext(filterContext.Controller.ControllerContext, vr.View, vr.ViewData, vr.TempData, sw);
                    vr.View.Render(vc, sw);
                    _cacheImpl.Set(filterContext.RequestContext, sw.ToString());
                }
            }
        }
    }
}
