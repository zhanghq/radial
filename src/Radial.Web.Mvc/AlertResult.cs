using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Radial.Web.Mvc
{
    /// <summary>
    /// Popup javascript alert window.
    /// </summary>
    public class AlertResult : ActionResult
    {
        string _message;
        string _redirect;


        /// <summary>
        /// Initializes a new instance of the <see cref="AlertResult"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public AlertResult(string message) : this(message, string.Empty) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AlertResult"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="redirect">The redirect url.</param>
        public AlertResult(string message, string redirect)
        {
            _message = message;
            _redirect = redirect;
        }

        /// <summary>
        /// Enables processing of the result of an action method by a custom type that inherits from the <see cref="T:System.Web.Mvc.ActionResult"/> class.
        /// </summary>
        /// <param name="context">The context in which the result is executed. The context information includes the controller, HTTP content, request context, and route data.</param>
        public override void ExecuteResult(ControllerContext context)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("<script type=\"text/javascript\">");

            sb.AppendLine("alert(\"" + _message + "\");");

            if (!string.IsNullOrWhiteSpace(_redirect))
                sb.AppendLine("window.location=\"" + _redirect + "\";");

            sb.AppendLine("</script>");

            context.HttpContext.Response.Write(sb.ToString());
        }
    }
}
