using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Radial.Boot
{
    /// <summary>
    /// Startup Http Module
    /// </summary>
    public class StartupHttpModule : IHttpModule
    {
        /// <summary>
        /// Disposes of the resources (other than memory) used by the module that implements <see cref="T:System.Web.IHttpModule" />.
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// Initializes a module and prepares it to handle requests.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpApplication" /> that provides access to the methods, properties, and events common to all application objects within an ASP.NET application </param>
        public void Init(HttpApplication context)
        {
            Bootstrapper.Initialize();

            context.BeginRequest += context_BeginRequest;
            context.EndRequest += context_EndRequest;
        }

        /// <summary>
        /// Handles the BeginRequest event of the context control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        void context_BeginRequest(object sender, EventArgs e)
        {
            Bootstrapper.Start();
        }

        /// <summary>
        /// Handles the EndRequest event of the context control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        void context_EndRequest(object sender, EventArgs e)
        {
            Bootstrapper.Stop();
        }
    }
}
