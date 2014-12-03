using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Configuration;
using NHibernate;
using System.IO;

namespace Radial.Persist.Nhs
{
    /// <summary>
    /// Application startup http module.
    /// </summary>
    public class AppStartModule : IHttpModule
    {
        static string[] NotNeedNhSessionFileExtensions = new string[] { "jpg", "jpeg", "gif", "png", "bmp", "rar", "zip", "doc", "docx", "dot", "dotx", "xls", "xlsx", "xlt", "xltx", "ppt", "pptx", "pot", "potx", "pdf", "xps", "iso", "txt", "tiff", "htm", "html", "asp", "php", "jsp", "jspa", "cgi", "do", "js", "css", "swf", "flv", "ico" };

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

            string notneedExts = ConfigurationManager.AppSettings["NoNhFileExt"];

            if (!string.IsNullOrWhiteSpace(notneedExts))
            {
                List<string> tempList = new List<string>(NotNeedNhSessionFileExtensions);

                string[] splits = notneedExts.Split(',');
                foreach (string t in splits)
                {
                    string ext = t.Trim().ToLower();

                    if (!tempList.Contains(ext))
                        tempList.Add(ext);
                }

                NotNeedNhSessionFileExtensions = tempList.ToArray();
            }
        }

        /// <summary>
        /// Handles the BeginRequest event of the context control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void context_BeginRequest(object sender, EventArgs e)
        {
            if (NeedSession(sender as HttpApplication))
                HibernateEngine.OpenAndBindSession();
        }

        /// <summary>
        /// Handles the EndRequest event of the context control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void context_EndRequest(object sender, EventArgs e)
        {
            if (NeedSession(sender as HttpApplication))
                HibernateEngine.CloseAndUnbindSession();
        }

        /// <summary>
        /// Handles the Error event of the context control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void context_Error(object sender, EventArgs e)
        {
            if (NeedSession(sender as HttpApplication))
                HibernateEngine.CloseAndUnbindSession();
        }


        /// <summary>
        /// Needs the session.
        /// </summary>
        /// <param name="app">The app.</param>
        /// <returns></returns>
        bool NeedSession(HttpApplication app)
        {
            Checker.Parameter(app != null, "HttpApplication instance can not be null");

            string fileExt = Path.GetExtension(HttpContext.Current.Request.Path).Trim('.', ' ').ToLower();

            return !NotNeedNhSessionFileExtensions.Contains(fileExt);

        }


        #endregion
    }
}
