using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Configuration;
using NHibernate;

namespace Radial.Data.Nhs
{
    /// <summary>
    /// Application startup http module.
    /// </summary>
    public class AppStartModule : IHttpModule
    {
        #region IHttpModule 成员

        /// <summary>
        /// Disposes of the resources (other than memory) used by the module that implements <see cref="T:System.Web.IHttpModule"/>.
        /// </summary>
        public void Dispose()
        {
            return;
        }

        /// <summary>
        /// Initializes a module and prepares it to handle requests.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpApplication"/> that provides access to the methods, properties, and events common to all application objects within an ASP.NET application</param>
        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(context_BeginRequest);
            context.EndRequest += new EventHandler(context_EndRequest);
            context.Error += new EventHandler(context_Error);
        }

        /// <summary>
        /// Handles the BeginRequest event of the context control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void context_BeginRequest(object sender, EventArgs e)
        {
            HibernateEngine.OpenAndBindSession();
        }

        /// <summary>
        /// Handles the EndRequest event of the context control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void context_EndRequest(object sender, EventArgs e)
        {
            HibernateEngine.UnbindAndDisposeSession();
        }

        /// <summary>
        /// Handles the Error event of the context control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void context_Error(object sender, EventArgs e)
        {
            HibernateEngine.UnbindAndDisposeSession();
        }

        #endregion
    }
}
