using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Radial.Web.Mvc
{
    /// <summary>
    /// Transfer Result.
    /// </summary>
    public class TransferResult : ActionResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TransferResult"/> class.
        /// </summary>
        /// <param name="url">The transfer URL.</param>
        public TransferResult(string url)
        {
            Url = url;
        }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        private string Url { get; set; }

        /// <summary>
        /// Enables processing of the result of an action method by a custom type that inherits from the <see cref="T:System.Web.Mvc.ActionResult" /> class.
        /// </summary>
        /// <param name="context">The context in which the result is executed. The context information includes the controller, HTTP content, request context, and route data.</param>
        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Server.TransferRequest(Url, true);
        }
    }

    /// <summary>
    /// Transfer to route result
    /// </summary>
    public class TransferToRouteResult : ActionResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TransferToRouteResult"/> class.
        /// </summary>
        /// <param name="routeValues">The route values.</param>
        public TransferToRouteResult(RouteValueDictionary routeValues)
            : this(null, routeValues)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransferToRouteResult"/> class.
        /// </summary>
        /// <param name="routeName">Name of the route.</param>
        /// <param name="routeValues">The route values.</param>
        public TransferToRouteResult(string routeName, RouteValueDictionary routeValues)
        {
            this.RouteName = routeName ?? string.Empty;
            this.RouteValues = routeValues ?? new RouteValueDictionary();
        }

        /// <summary>
        /// Gets or sets the name of the route.
        /// </summary>
        /// <value>
        /// The name of the route.
        /// </value>
        private string RouteName { get; set; }
        /// <summary>
        /// Gets or sets the route values.
        /// </summary>
        /// <value>
        /// The route values.
        /// </value>
        private RouteValueDictionary RouteValues { get; set; }

        /// <summary>
        /// Enables processing of the result of an action method by a custom type that inherits from the <see cref="T:System.Web.Mvc.ActionResult" /> class.
        /// </summary>
        /// <param name="context">The context in which the result is executed. The context information includes the controller, HTTP content, request context, and route data.</param>
        public override void ExecuteResult(ControllerContext context)
        {
            var urlHelper = new UrlHelper(context.RequestContext);
            var url = urlHelper.RouteUrl(this.RouteName, this.RouteValues);

            var actualResult = new TransferResult(url);
            actualResult.ExecuteResult(context);
        }
    }
}
