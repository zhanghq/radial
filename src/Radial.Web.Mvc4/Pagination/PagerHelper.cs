/*
 ASP.NET MvcPager 分页组件 Thanks To Webdiyer
 */
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Routing;

namespace Radial.Web.Mvc.Pagination
{
    /// <summary>
    /// PagerHelper
    /// </summary>
    public static class PagerHelper
    {
        #region Html Pager
        /// <summary>
        /// Pagers the specified helper.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="totalItemCount">The total item count.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="pagerOptions">The pager options.</param>
        /// <param name="routeName">Name of the route.</param>
        /// <param name="routeValues">The route values.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public static MvcHtmlString Pager(this HtmlHelper helper, int totalItemCount, int pageSize, int pageIndex, string actionName, string controllerName,
            PagerOptions pagerOptions, string routeName, object routeValues, object htmlAttributes)
        {
            var totalPageCount = (int)Math.Ceiling(totalItemCount / (double)pageSize);
            var builder = new PagerBuilder
                (
                    helper,
                    actionName,
                    controllerName,
                    totalPageCount,
                    pageIndex,
                    pagerOptions,
                    routeName,
                    new RouteValueDictionary(routeValues),
                    new RouteValueDictionary(htmlAttributes)
                );
            return builder.RenderPager();
        }

        /// <summary>
        /// Pagers the specified helper.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="totalItemCount">The total item count.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="pagerOptions">The pager options.</param>
        /// <param name="routeName">Name of the route.</param>
        /// <param name="routeValues">The route values.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public static MvcHtmlString Pager(this HtmlHelper helper, int totalItemCount, int pageSize, int pageIndex, string actionName, string controllerName,
            PagerOptions pagerOptions, string routeName, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            var totalPageCount = (int)Math.Ceiling(totalItemCount / (double)pageSize);
            var builder = new PagerBuilder
                (
                    helper,
                    actionName,
                    controllerName,
                    totalPageCount,
                    pageIndex,
                    pagerOptions,
                    routeName,
                    routeValues,
                    htmlAttributes
                );
            return builder.RenderPager();
        }

        /// <summary>
        /// Pagers the specified helper.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="pagerOptions">The pager options.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        private static MvcHtmlString Pager(HtmlHelper helper, PagerOptions pagerOptions, IDictionary<string, object> htmlAttributes)
        {
            return new PagerBuilder(helper, null, pagerOptions, htmlAttributes).RenderPager();
        }

        /// <summary>
        /// Pagers the specified helper.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="pagedList">The paged list.</param>
        /// <returns></returns>
        public static MvcHtmlString Pager(this HtmlHelper helper, IPagedList pagedList)
        {
            if (pagedList == null)
                return Pager(helper, (PagerOptions)null, null);
            return Pager(helper, pagedList.TotalItemCount, pagedList.PageSize, pagedList.CurrentPageIndex, null, null, null, null, null, null);
        }

        /// <summary>
        /// Pagers the specified helper.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="pagedList">The paged list.</param>
        /// <param name="pagerOptions">The pager options.</param>
        /// <returns></returns>
        public static MvcHtmlString Pager(this HtmlHelper helper, IPagedList pagedList, PagerOptions pagerOptions)
        {
            if (pagedList == null)
                return Pager(helper, pagerOptions, null);
            return Pager(helper, pagedList.TotalItemCount, pagedList.PageSize, pagedList.CurrentPageIndex, null, null, pagerOptions, null, null, null);
        }

        /// <summary>
        /// Pagers the specified helper.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="pagedList">The paged list.</param>
        /// <param name="pagerOptions">The pager options.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public static MvcHtmlString Pager(this HtmlHelper helper, IPagedList pagedList, PagerOptions pagerOptions, object htmlAttributes)
        {
            if (pagedList == null)
                return Pager(helper, pagerOptions, new RouteValueDictionary(htmlAttributes));
            return Pager(helper, pagedList.TotalItemCount, pagedList.PageSize, pagedList.CurrentPageIndex, null, null, pagerOptions, null, null, htmlAttributes);
        }

        /// <summary>
        /// Pagers the specified helper.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="pagedList">The paged list.</param>
        /// <param name="pagerOptions">The pager options.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public static MvcHtmlString Pager(this HtmlHelper helper, IPagedList pagedList, PagerOptions pagerOptions, IDictionary<string, object> htmlAttributes)
        {
            if (pagedList == null)
                return Pager(helper, pagerOptions, htmlAttributes);
            return Pager(helper, pagedList.TotalItemCount, pagedList.PageSize, pagedList.CurrentPageIndex, null, null, pagerOptions, null, null, htmlAttributes);
        }

        /// <summary>
        /// Pagers the specified helper.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="pagedList">The paged list.</param>
        /// <param name="pagerOptions">The pager options.</param>
        /// <param name="routeName">Name of the route.</param>
        /// <param name="routeValues">The route values.</param>
        /// <returns></returns>
        public static MvcHtmlString Pager(this HtmlHelper helper, IPagedList pagedList, PagerOptions pagerOptions, string routeName, object routeValues)
        {
            if (pagedList == null)
                return Pager(helper, pagerOptions, null);
            return Pager(helper, pagedList.TotalItemCount, pagedList.PageSize, pagedList.CurrentPageIndex, null, null, pagerOptions, routeName, routeValues, null);
        }

        /// <summary>
        /// Pagers the specified helper.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="pagedList">The paged list.</param>
        /// <param name="pagerOptions">The pager options.</param>
        /// <param name="routeName">Name of the route.</param>
        /// <param name="routeValues">The route values.</param>
        /// <returns></returns>
        public static MvcHtmlString Pager(this HtmlHelper helper, IPagedList pagedList, PagerOptions pagerOptions, string routeName, RouteValueDictionary routeValues)
        {
            if (pagedList == null)
                return Pager(helper, pagerOptions, null);
            return Pager(helper, pagedList.TotalItemCount, pagedList.PageSize, pagedList.CurrentPageIndex, null, null, pagerOptions, routeName, routeValues, null);
        }

        /// <summary>
        /// Pagers the specified helper.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="pagedList">The paged list.</param>
        /// <param name="pagerOptions">The pager options.</param>
        /// <param name="routeName">Name of the route.</param>
        /// <param name="routeValues">The route values.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public static MvcHtmlString Pager(this HtmlHelper helper, IPagedList pagedList, PagerOptions pagerOptions, string routeName, object routeValues, object htmlAttributes)
        {
            if (pagedList == null)
                return Pager(helper, pagerOptions, new RouteValueDictionary(htmlAttributes));
            return Pager(helper, pagedList.TotalItemCount, pagedList.PageSize, pagedList.CurrentPageIndex, null, null, pagerOptions, routeName, routeValues, htmlAttributes);
        }

        /// <summary>
        /// Pagers the specified helper.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="pagedList">The paged list.</param>
        /// <param name="pagerOptions">The pager options.</param>
        /// <param name="routeName">Name of the route.</param>
        /// <param name="routeValues">The route values.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public static MvcHtmlString Pager(this HtmlHelper helper, IPagedList pagedList, PagerOptions pagerOptions, string routeName,
            RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            if (pagedList == null)
                return Pager(helper, pagerOptions, htmlAttributes);
            return Pager(helper, pagedList.TotalItemCount, pagedList.PageSize, pagedList.CurrentPageIndex, null, null, pagerOptions, routeName, routeValues, htmlAttributes);
        }

        /// <summary>
        /// Pagers the specified helper.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="pagedList">The paged list.</param>
        /// <param name="routeName">Name of the route.</param>
        /// <param name="routeValues">The route values.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public static MvcHtmlString Pager(this HtmlHelper helper, IPagedList pagedList, string routeName, object routeValues, object htmlAttributes)
        {
            if (pagedList == null)
                return Pager(helper, null, new RouteValueDictionary(htmlAttributes));
            return Pager(helper, pagedList.TotalItemCount, pagedList.PageSize, pagedList.CurrentPageIndex, null, null, null, routeName, routeValues, htmlAttributes);
        }

        /// <summary>
        /// Pagers the specified helper.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="pagedList">The paged list.</param>
        /// <param name="routeName">Name of the route.</param>
        /// <param name="routeValues">The route values.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public static MvcHtmlString Pager(this HtmlHelper helper, IPagedList pagedList, string routeName, RouteValueDictionary routeValues,
            IDictionary<string, object> htmlAttributes)
        {
            if (pagedList == null)
                return Pager(helper, null, htmlAttributes);
            return Pager(helper, pagedList.TotalItemCount, pagedList.PageSize, pagedList.CurrentPageIndex, null, null, null, routeName, routeValues, htmlAttributes);
        }

        #endregion

        #region jQuery Ajax Pager

        /// <summary>
        /// Ajaxes the pager.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="pagerOptions">The pager options.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        private static MvcHtmlString AjaxPager(HtmlHelper html, PagerOptions pagerOptions, IDictionary<string, object> htmlAttributes)
        {
            return new PagerBuilder(html, null, pagerOptions, htmlAttributes).RenderPager();
        }

        /// <summary>
        /// Ajaxes the pager.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="totalItemCount">The total item count.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="routeName">Name of the route.</param>
        /// <param name="pagerOptions">The pager options.</param>
        /// <param name="routeValues">The route values.</param>
        /// <param name="ajaxOptions">The ajax options.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public static MvcHtmlString AjaxPager(this HtmlHelper html, int totalItemCount, int pageSize, int pageIndex, string actionName, string controllerName,
            string routeName, PagerOptions pagerOptions, object routeValues, AjaxOptions ajaxOptions, object htmlAttributes)
        {
            if (pagerOptions == null)
                pagerOptions = new PagerOptions();
            pagerOptions.UseJqueryAjax = true;

            var totalPageCount = (int)Math.Ceiling(totalItemCount / (double)pageSize);
            var builder = new PagerBuilder(html, actionName, controllerName, totalPageCount, pageIndex, pagerOptions,
                                           routeName, new RouteValueDictionary(routeValues), ajaxOptions, new RouteValueDictionary(htmlAttributes));
            return builder.RenderPager();
        }

        /// <summary>
        /// Ajaxes the pager.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="totalItemCount">The total item count.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="routeName">Name of the route.</param>
        /// <param name="pagerOptions">The pager options.</param>
        /// <param name="routeValues">The route values.</param>
        /// <param name="ajaxOptions">The ajax options.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public static MvcHtmlString AjaxPager(this HtmlHelper html, int totalItemCount, int pageSize, int pageIndex, string actionName, string controllerName,
            string routeName, PagerOptions pagerOptions, RouteValueDictionary routeValues, AjaxOptions ajaxOptions, IDictionary<string, object> htmlAttributes)
        {
            if (pagerOptions == null)
                pagerOptions = new PagerOptions();
            pagerOptions.UseJqueryAjax = true;

            var totalPageCount = (int)Math.Ceiling(totalItemCount / (double)pageSize);
            var builder = new PagerBuilder(html, actionName, controllerName, totalPageCount, pageIndex, pagerOptions,
                                           routeName, routeValues, ajaxOptions, htmlAttributes);
            return builder.RenderPager();
        }

        /// <summary>
        /// Ajaxes the pager.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="pagedList">The paged list.</param>
        /// <param name="ajaxOptions">The ajax options.</param>
        /// <returns></returns>
        public static MvcHtmlString AjaxPager(this HtmlHelper html, IPagedList pagedList, AjaxOptions ajaxOptions)
        {
            if (pagedList == null)
                return AjaxPager(html, (PagerOptions)null, null);
            return AjaxPager(html, pagedList.TotalItemCount, pagedList.PageSize, pagedList.CurrentPageIndex, null, null, null, null, null, ajaxOptions,
                             null);
        }

        /// <summary>
        /// Ajaxes the pager.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="pagedList">The paged list.</param>
        /// <param name="routeName">Name of the route.</param>
        /// <param name="ajaxOptions">The ajax options.</param>
        /// <returns></returns>
        public static MvcHtmlString AjaxPager(this HtmlHelper html, IPagedList pagedList, string routeName, AjaxOptions ajaxOptions)
        {
            if (pagedList == null)
                return AjaxPager(html, (PagerOptions)null, null);
            return AjaxPager(html, pagedList.TotalItemCount, pagedList.PageSize, pagedList.CurrentPageIndex, null, null, routeName, null, null, ajaxOptions,
                             null);
        }

        /// <summary>
        /// Ajaxes the pager.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="pagedList">The paged list.</param>
        /// <param name="pagerOptions">The pager options.</param>
        /// <param name="ajaxOptions">The ajax options.</param>
        /// <returns></returns>
        public static MvcHtmlString AjaxPager(this HtmlHelper html, IPagedList pagedList, PagerOptions pagerOptions, AjaxOptions ajaxOptions)
        {
            if (pagedList == null)
                return AjaxPager(html, pagerOptions, null);
            return AjaxPager(html, pagedList.TotalItemCount, pagedList.PageSize, pagedList.CurrentPageIndex, null, null, null, pagerOptions, null, ajaxOptions,
                             null);
        }

        /// <summary>
        /// Ajaxes the pager.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="pagedList">The paged list.</param>
        /// <param name="pagerOptions">The pager options.</param>
        /// <param name="ajaxOptions">The ajax options.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public static MvcHtmlString AjaxPager(this HtmlHelper html, IPagedList pagedList, PagerOptions pagerOptions, AjaxOptions ajaxOptions, object htmlAttributes)
        {
            if (pagedList == null)
                return AjaxPager(html, pagerOptions, new RouteValueDictionary(htmlAttributes));
            return AjaxPager(html, pagedList.TotalItemCount, pagedList.PageSize, pagedList.CurrentPageIndex, null, null, null, pagerOptions, null,
                             ajaxOptions, htmlAttributes);
        }

        /// <summary>
        /// Ajaxes the pager.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="pagedList">The paged list.</param>
        /// <param name="pagerOptions">The pager options.</param>
        /// <param name="ajaxOptions">The ajax options.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public static MvcHtmlString AjaxPager(this HtmlHelper html, IPagedList pagedList, PagerOptions pagerOptions, AjaxOptions ajaxOptions,
            IDictionary<string, object> htmlAttributes)
        {
            if (pagedList == null)
                return AjaxPager(html, pagerOptions, htmlAttributes);
            return AjaxPager(html, pagedList.TotalItemCount, pagedList.PageSize, pagedList.CurrentPageIndex, null, null, null, pagerOptions, null,
                             ajaxOptions, htmlAttributes);
        }

        /// <summary>
        /// Ajaxes the pager.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="pagedList">The paged list.</param>
        /// <param name="routeName">Name of the route.</param>
        /// <param name="routeValues">The route values.</param>
        /// <param name="pagerOptions">The pager options.</param>
        /// <param name="ajaxOptions">The ajax options.</param>
        /// <returns></returns>
        public static MvcHtmlString AjaxPager(this HtmlHelper html, IPagedList pagedList, string routeName, object routeValues, PagerOptions pagerOptions, AjaxOptions ajaxOptions)
        {
            if (pagedList == null)
                return AjaxPager(html, pagerOptions, null);
            return AjaxPager(html, pagedList.TotalItemCount, pagedList.PageSize, pagedList.CurrentPageIndex, null, null, routeName, pagerOptions, routeValues, ajaxOptions,
                             null);
        }

        /// <summary>
        /// Ajaxes the pager.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="pagedList">The paged list.</param>
        /// <param name="routeName">Name of the route.</param>
        /// <param name="routeValues">The route values.</param>
        /// <param name="pagerOptions">The pager options.</param>
        /// <param name="ajaxOptions">The ajax options.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public static MvcHtmlString AjaxPager(this HtmlHelper html, IPagedList pagedList, string routeName, object routeValues,
            PagerOptions pagerOptions, AjaxOptions ajaxOptions, object htmlAttributes)
        {
            if (pagedList == null)
                return AjaxPager(html, pagerOptions, new RouteValueDictionary(htmlAttributes));
            return AjaxPager(html, pagedList.TotalItemCount, pagedList.PageSize, pagedList.CurrentPageIndex, null, null, routeName, pagerOptions,
                             routeValues, ajaxOptions, htmlAttributes);
        }

        /// <summary>
        /// Ajaxes the pager.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="pagedList">The paged list.</param>
        /// <param name="routeName">Name of the route.</param>
        /// <param name="routeValues">The route values.</param>
        /// <param name="pagerOptions">The pager options.</param>
        /// <param name="ajaxOptions">The ajax options.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public static MvcHtmlString AjaxPager(this HtmlHelper html, IPagedList pagedList, string routeName, RouteValueDictionary routeValues,
            PagerOptions pagerOptions, AjaxOptions ajaxOptions, IDictionary<string, object> htmlAttributes)
        {
            if (pagedList == null)
                return AjaxPager(html, pagerOptions, htmlAttributes);
            return AjaxPager(html, pagedList.TotalItemCount, pagedList.PageSize, pagedList.CurrentPageIndex, null, null, routeName, pagerOptions,
                             routeValues, ajaxOptions, htmlAttributes);
        }

        /// <summary>
        /// Ajaxes the pager.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="pagedList">The paged list.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="pagerOptions">The pager options.</param>
        /// <param name="ajaxOptions">The ajax options.</param>
        /// <returns></returns>
        public static MvcHtmlString AjaxPager(this HtmlHelper html, IPagedList pagedList, string actionName, string controllerName,
            PagerOptions pagerOptions, AjaxOptions ajaxOptions)
        {
            if (pagedList == null)
                return AjaxPager(html, pagerOptions, null);
            return AjaxPager(html, pagedList.TotalItemCount, pagedList.PageSize, pagedList.CurrentPageIndex, actionName, controllerName, null, pagerOptions, null, ajaxOptions,
                             null);
        }

        #endregion

        #region Microsoft Ajax Pager

        /// <summary>
        /// Pagers the specified ajax.
        /// </summary>
        /// <param name="ajax">The ajax.</param>
        /// <param name="totalItemCount">The total item count.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="routeName">Name of the route.</param>
        /// <param name="pagerOptions">The pager options.</param>
        /// <param name="routeValues">The route values.</param>
        /// <param name="ajaxOptions">The ajax options.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public static MvcHtmlString Pager(this AjaxHelper ajax, int totalItemCount, int pageSize, int pageIndex, string actionName, string controllerName,
            string routeName, PagerOptions pagerOptions, object routeValues, AjaxOptions ajaxOptions, object htmlAttributes)
        {
            var totalPageCount = (int)Math.Ceiling(totalItemCount / (double)pageSize);
            var builder = new PagerBuilder(ajax, actionName, controllerName, totalPageCount, pageIndex, pagerOptions,
                                           routeName, new RouteValueDictionary(routeValues), ajaxOptions, new RouteValueDictionary(htmlAttributes));
            return builder.RenderPager();
        }

        /// <summary>
        /// Pagers the specified ajax.
        /// </summary>
        /// <param name="ajax">The ajax.</param>
        /// <param name="totalItemCount">The total item count.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="routeName">Name of the route.</param>
        /// <param name="pagerOptions">The pager options.</param>
        /// <param name="routeValues">The route values.</param>
        /// <param name="ajaxOptions">The ajax options.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public static MvcHtmlString Pager(this AjaxHelper ajax, int totalItemCount, int pageSize, int pageIndex, string actionName, string controllerName,
            string routeName, PagerOptions pagerOptions, RouteValueDictionary routeValues, AjaxOptions ajaxOptions, IDictionary<string, object> htmlAttributes)
        {
            var totalPageCount = (int)Math.Ceiling(totalItemCount / (double)pageSize);
            var builder = new PagerBuilder(ajax, actionName, controllerName, totalPageCount, pageIndex, pagerOptions,
                                           routeName, routeValues, ajaxOptions, htmlAttributes);
            return builder.RenderPager();
        }

        /// <summary>
        /// Pagers the specified ajax.
        /// </summary>
        /// <param name="ajax">The ajax.</param>
        /// <param name="pagerOptions">The pager options.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        private static MvcHtmlString Pager(AjaxHelper ajax, PagerOptions pagerOptions, IDictionary<string, object> htmlAttributes)
        {
            return new PagerBuilder(null, ajax, pagerOptions, htmlAttributes).RenderPager();
        }

        /// <summary>
        /// Pagers the specified ajax.
        /// </summary>
        /// <param name="ajax">The ajax.</param>
        /// <param name="pagedList">The paged list.</param>
        /// <param name="ajaxOptions">The ajax options.</param>
        /// <returns></returns>
        public static MvcHtmlString Pager(this AjaxHelper ajax, IPagedList pagedList, AjaxOptions ajaxOptions)
        {
            return pagedList == null ? Pager(ajax, (PagerOptions)null, null) : Pager(ajax, pagedList.TotalItemCount, pagedList.PageSize, pagedList.CurrentPageIndex, null, null, null, null, null, ajaxOptions, null);
        }

        /// <summary>
        /// Pagers the specified ajax.
        /// </summary>
        /// <param name="ajax">The ajax.</param>
        /// <param name="pagedList">The paged list.</param>
        /// <param name="pagerOptions">The pager options.</param>
        /// <param name="ajaxOptions">The ajax options.</param>
        /// <returns></returns>
        public static MvcHtmlString Pager(this AjaxHelper ajax, IPagedList pagedList, PagerOptions pagerOptions, AjaxOptions ajaxOptions)
        {
            return pagedList == null ? Pager(ajax, pagerOptions, null) : Pager(ajax, pagedList.TotalItemCount, pagedList.PageSize, pagedList.CurrentPageIndex,
                null, null, null, pagerOptions, null, ajaxOptions, null);
        }

        /// <summary>
        /// Pagers the specified ajax.
        /// </summary>
        /// <param name="ajax">The ajax.</param>
        /// <param name="pagedList">The paged list.</param>
        /// <param name="pagerOptions">The pager options.</param>
        /// <param name="ajaxOptions">The ajax options.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public static MvcHtmlString Pager(this AjaxHelper ajax, IPagedList pagedList, PagerOptions pagerOptions, AjaxOptions ajaxOptions, object htmlAttributes)
        {
            if (pagedList == null)
                return Pager(ajax, pagerOptions, new RouteValueDictionary(htmlAttributes));
            return Pager(ajax, pagedList.TotalItemCount, pagedList.PageSize, pagedList.CurrentPageIndex, null, null, null, pagerOptions, null, ajaxOptions, htmlAttributes);
        }

        /// <summary>
        /// Pagers the specified ajax.
        /// </summary>
        /// <param name="ajax">The ajax.</param>
        /// <param name="pagedList">The paged list.</param>
        /// <param name="pagerOptions">The pager options.</param>
        /// <param name="ajaxOptions">The ajax options.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public static MvcHtmlString Pager(this AjaxHelper ajax, IPagedList pagedList, PagerOptions pagerOptions, AjaxOptions ajaxOptions, IDictionary<string, object> htmlAttributes)
        {
            if (pagedList == null)
                return Pager(ajax, pagerOptions, htmlAttributes);
            return Pager(ajax, pagedList.TotalItemCount, pagedList.PageSize, pagedList.CurrentPageIndex, null, null, null, pagerOptions, null, ajaxOptions, htmlAttributes);
        }

        /// <summary>
        /// Pagers the specified ajax.
        /// </summary>
        /// <param name="ajax">The ajax.</param>
        /// <param name="pagedList">The paged list.</param>
        /// <param name="routeName">Name of the route.</param>
        /// <param name="routeValues">The route values.</param>
        /// <param name="ajaxOptions">The ajax options.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public static MvcHtmlString Pager(this AjaxHelper ajax, IPagedList pagedList, string routeName, object routeValues, AjaxOptions ajaxOptions, object htmlAttributes)
        {
            if (pagedList == null)
                return Pager(ajax, null, new RouteValueDictionary(htmlAttributes));
            return Pager(ajax, pagedList.TotalItemCount, pagedList.PageSize, pagedList.CurrentPageIndex, null, null, routeName, null, routeValues, ajaxOptions, htmlAttributes);
        }

        /// <summary>
        /// Pagers the specified ajax.
        /// </summary>
        /// <param name="ajax">The ajax.</param>
        /// <param name="pagedList">The paged list.</param>
        /// <param name="routeName">Name of the route.</param>
        /// <param name="routeValues">The route values.</param>
        /// <param name="ajaxOptions">The ajax options.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public static MvcHtmlString Pager(this AjaxHelper ajax, IPagedList pagedList, string routeName, RouteValueDictionary routeValues,
            AjaxOptions ajaxOptions, IDictionary<string, object> htmlAttributes)
        {
            if (pagedList == null)
                return Pager(ajax, null, htmlAttributes);
            return Pager(ajax, pagedList.TotalItemCount, pagedList.PageSize, pagedList.CurrentPageIndex, null, null, routeName, null, routeValues, ajaxOptions, htmlAttributes);
        }

        /// <summary>
        /// Pagers the specified ajax.
        /// </summary>
        /// <param name="ajax">The ajax.</param>
        /// <param name="pagedList">The paged list.</param>
        /// <param name="routeName">Name of the route.</param>
        /// <param name="routeValues">The route values.</param>
        /// <param name="pagerOptions">The pager options.</param>
        /// <param name="ajaxOptions">The ajax options.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public static MvcHtmlString Pager(this AjaxHelper ajax, IPagedList pagedList, string routeName, object routeValues, PagerOptions pagerOptions,
            AjaxOptions ajaxOptions, object htmlAttributes)
        {
            if (pagedList == null)
                return Pager(ajax, pagerOptions, new RouteValueDictionary(htmlAttributes));
            return Pager(ajax, pagedList.TotalItemCount, pagedList.PageSize, pagedList.CurrentPageIndex, null, null, routeName, pagerOptions, routeValues, ajaxOptions, htmlAttributes);
        }

        /// <summary>
        /// Pagers the specified ajax.
        /// </summary>
        /// <param name="ajax">The ajax.</param>
        /// <param name="pagedList">The paged list.</param>
        /// <param name="routeName">Name of the route.</param>
        /// <param name="routeValues">The route values.</param>
        /// <param name="pagerOptions">The pager options.</param>
        /// <param name="ajaxOptions">The ajax options.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public static MvcHtmlString Pager(this AjaxHelper ajax, IPagedList pagedList, string routeName, RouteValueDictionary routeValues,
            PagerOptions pagerOptions, AjaxOptions ajaxOptions, IDictionary<string, object> htmlAttributes)
        {
            if (pagedList == null)
                return Pager(ajax, pagerOptions, htmlAttributes);
            return Pager(ajax, pagedList.TotalItemCount, pagedList.PageSize, pagedList.CurrentPageIndex, null, null, routeName, pagerOptions, routeValues, ajaxOptions, htmlAttributes);
        }
        #endregion
    }
}