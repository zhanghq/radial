using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using System.Web;
using Radial;
using Newtonsoft.Json;

namespace Radial.Web
{
    /// <summary>
    /// Integrated common functions for forms authentication.
    /// </summary>
    public static class FormsAuth
    {

        /// <summary>
        /// Gets the URL that the System.Web.Security.FormsAuthentication class willredirect to if no redirect URL is specified.
        /// </summary>
        public static string DefaultUrl
        {
            get { return FormsAuthentication.DefaultUrl; }
        }

        /// <summary>
        /// The URL for the login page that the System.Web.Security.FormsAuthentication class will redirect to.
        /// </summary>
        public static string LoginUrl
        {
            get
            {
                return FormsAuthentication.LoginUrl;
            }
        }

        /// <summary>
        /// User sign in.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <param name="createPersistentCookie">if set to <c>true</c> [create persistent cookie].</param>
        public static void SignIn(string userName, bool createPersistentCookie)
        {
            SignIn(userName, createPersistentCookie, false);
        }

        /// <summary>
        /// User sign in.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <param name="createPersistentCookie">if set to <c>true</c> [create persistent cookie].</param>
        /// <param name="redirect">whether need to redirect from login page.</param>
        public static void SignIn(string userName, bool createPersistentCookie, bool redirect)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(userName), "the user name can not be empty or null");

            FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);

            if (redirect)
                FormsAuthentication.RedirectFromLoginPage(userName, createPersistentCookie);

        }


        /// <summary>
        /// User sign in.
        /// </summary>
        /// <param name="identity">The user identity.</param>
        /// <param name="createPersistentCookie">if set to <c>true</c> [create persistent cookie].</param>
        public static void SignIn(UserIdentity identity, bool createPersistentCookie)
        {
            SignIn(identity, createPersistentCookie, false);
        }

        /// <summary>
        /// User sign in.
        /// </summary>
        /// <param name="identity">The user identity.</param>
        /// <param name="createPersistentCookie">if set to <c>true</c> [create persistent cookie].</param>
        /// <param name="redirect">if set to <c>true</c> [redirect].</param>
        public static void SignIn(UserIdentity identity, bool createPersistentCookie, bool redirect)
        {
            Checker.Parameter(identity != null, "the user identity object can not be null");


            DateTime issueDate = DateTime.Now;
            DateTime expiration = issueDate.AddMinutes(FormsAuthentication.Timeout.TotalMinutes);
            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(2, identity.Name, issueDate, expiration, createPersistentCookie, JsonConvert.SerializeObject(identity), FormsAuthentication.FormsCookiePath);

            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
            HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            authCookie.Path = FormsAuthentication.FormsCookiePath;
            authCookie.Secure = FormsAuthentication.RequireSSL;
            if (createPersistentCookie)
                authCookie.Expires = expiration;
            HttpContext.Current.Response.Cookies.Add(authCookie);

            if (redirect)
                //if not use this, userData will be null.
                HttpContext.Current.Response.Redirect(FormsAuthentication.GetRedirectUrl(identity.Name, createPersistentCookie));

        }

        /// <summary>
        /// Redirects the user to the login page.
        /// </summary>
        public static void RedirectToLoginPage()
        {
            FormsAuthentication.RedirectToLoginPage();
        }

        /// <summary>
        /// Redirects the user to the default page.
        /// </summary>
        public static void RedirectToDefaultPage()
        {
            HttpContext.Current.Response.Redirect(string.IsNullOrWhiteSpace(FormsAuthentication.DefaultUrl) ? "~/" : FormsAuthentication.DefaultUrl);
        }

        /// <summary>
        /// Redirects the browser to the login URL with the specified query string.
        /// </summary>
        /// <param name="extraQueryString">The query string to include with the redirect URL.</param>
        public static void RedirectToLoginPage(string extraQueryString)
        {
            FormsAuthentication.RedirectToLoginPage(extraQueryString);
        }

        /// <summary>
        /// User sign out.
        /// </summary>
        public static void SignOut()
        {
            SignOut(false);
        }

        /// <summary>
        /// User sign out.
        /// </summary>
        /// <param name="redirect">whether need to redirect to default page.</param>
        public static void SignOut(bool redirect)
        {
            FormsAuthentication.SignOut();
            if (redirect)
                RedirectToDefaultPage();
        }

        /// <summary>
        /// Gets the name of the cookie used to store the forms-authentication ticket.
        /// </summary>
        public static string FormsCookieName
        {
            get
            {
                return FormsAuthentication.FormsCookieName;
            }
        }

        /// <summary>
        /// Gets the path for the forms-authentication cookie.
        /// </summary>
        public static string FormsCookiePath
        {
            get
            {
                return FormsAuthentication.FormsCookiePath;
            }
        }

        /// <summary>
        /// Gets the value of the domain of the forms-authentication cookie.
        /// </summary>
        public static string CookieDomain
        {
            get
            {
                return FormsAuthentication.CookieDomain;
            }
        }

        /// <summary>
        /// Gets a value that indicates whether the application is configured for cookieless forms authentication.
        /// </summary>
        public static HttpCookieMode CookieMode
        {
            get
            {
                return FormsAuthentication.CookieMode;
            }
        }

        /// <summary>
        /// Gets a value that indicates whether the application is configured to support cookieless forms authentication.
        /// </summary>
        public static bool CookiesSupported 
        {
            get
            {
                return FormsAuthentication.CookiesSupported;
            }
        }

        /// <summary>
        /// Gets the timeout value specified in the configuration file (in minutes).
        /// </summary>
        public static TimeSpan Timeout
        {
            get
            {
                return FormsAuthentication.Timeout;
            }
        }

        /// <summary>
        /// Gets a value indicating whether sliding expiration is enabled.
        /// </summary>
        public static bool SlidingExpiration
        {
            get
            {
                return FormsAuthentication.SlidingExpiration;
            }
        }
    }
}


