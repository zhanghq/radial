﻿/*
 ASP.NET MvcPager 分页组件 Thanks To Webdiyer
 */
using System;
using System.Globalization;
using System.Web;
using System.Web.Mvc.Ajax;
using System.Web.Routing;
using System.Web.Mvc;
using System.Text;
using System.Collections.Generic;

namespace Radial.Web.Mvc.Pagination
{
    /// <summary>
    /// PagerBuilder
    /// </summary>
    internal class PagerBuilder
    {
        private readonly HtmlHelper _html;
        private readonly AjaxHelper _ajax;
        private readonly string _actionName;
        private readonly string _controllerName;
        private readonly int _totalPageCount = 1;
        private readonly int _pageIndex;
        private readonly PagerOptions _pagerOptions;
        private readonly RouteValueDictionary _routeValues;
        private readonly string _routeName;
        private readonly int _startPageIndex = 1;
        private readonly int _endPageIndex = 1;
        private readonly bool _msAjaxPaging;
        private readonly AjaxOptions _ajaxOptions;
        private IDictionary<string, object> _htmlAttributes;
        private const string ScriptPageIndexName = "_MvcPager_PageIndex_";
        private const string GoToPageScript = "function _MvcPager_GoToPage(_pib,_mp){var pageIndex;if(_pib.tagName==\"SELECT\"){pageIndex=_pib.options[_pib.selectedIndex].value;}else{pageIndex=_pib.value;var r=new RegExp(\"^\\\\s*(\\\\d+)\\\\s*$\");if(!r.test(pageIndex)){alert(\"%InvalidPageIndexErrorMessage%\");return;}else if(RegExp.$1<1||RegExp.$1>_mp){alert(\"%PageIndexOutOfRangeErrorMessage%\");return;}}var _hl=document.getElementById(_pib.id+'link').childNodes[0];var _lh=_hl.href;_hl.href=_lh.replace('" + ScriptPageIndexName + "',pageIndex);if(_hl.click){_hl.click();}else{var evt=document.createEvent('MouseEvents');evt.initEvent('click',true,true);_hl.dispatchEvent(evt);}_hl.href=_lh;}";
        private const string KeyDownScript = "function _MvcPager_Keydown(e){var _kc,_pib;if(window.event){_kc=e.keyCode;_pib=e.srcElement;}else if(e.which){_kc=e.which;_pib=e.target;}var validKey=(_kc==8||_kc==46||_kc==37||_kc==39||(_kc>=48&&_kc<=57)||(_kc>=96&&_kc<=105));if(!validKey){if(_kc==13){ _MvcPager_GoToPage(_pib,%TotalPageCount%);}if(e.preventDefault){e.preventDefault();}else{event.returnValue=false;}}}";

        /// <summary>
        /// Initializes a new instance of the <see cref="PagerBuilder" /> class.(适用于PagedList为null时)
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="ajax">The ajax.</param>
        /// <param name="pagerOptions">The pager options.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        internal PagerBuilder(HtmlHelper html, AjaxHelper ajax, PagerOptions pagerOptions, IDictionary<string, object> htmlAttributes)
        {
            if (pagerOptions == null)
                pagerOptions = new PagerOptions();
            _html = html;
            _ajax = ajax;
            _pagerOptions = pagerOptions;
            _htmlAttributes = htmlAttributes;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PagerBuilder" /> class.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="ajax">The ajax.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="totalPageCount">The total page count.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pagerOptions">The pager options.</param>
        /// <param name="routeName">Name of the route.</param>
        /// <param name="routeValues">The route values.</param>
        /// <param name="ajaxOptions">The ajax options.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        internal PagerBuilder(HtmlHelper html, AjaxHelper ajax, string actionName, string controllerName,
            int totalPageCount, int pageIndex, PagerOptions pagerOptions, string routeName, RouteValueDictionary routeValues,
            AjaxOptions ajaxOptions, IDictionary<string, object> htmlAttributes)
        {
            _msAjaxPaging = (ajax != null);
            if (String.IsNullOrEmpty(actionName))
            {
                if (ajax != null)
                    actionName = (string)ajax.ViewContext.RouteData.Values["action"];
                else
                    actionName = (string)html.ViewContext.RouteData.Values["action"];
            }
            if (String.IsNullOrEmpty(controllerName))
            {
                if (ajax != null)
                    controllerName = (string)ajax.ViewContext.RouteData.Values["controller"];
                else
                    controllerName = (string)html.ViewContext.RouteData.Values["controller"];
            }
            if (pagerOptions == null)
                pagerOptions = new PagerOptions();

            _html = html;
            _ajax = ajax;
            _actionName = actionName;
            _controllerName = controllerName;
            
            //修正总页数BUG
            //if (pagerOptions.MaxPageIndex == 0 || pagerOptions.MaxPageIndex > totalPageCount)
            //    _totalPageCount = totalPageCount;
            //else
            //    _totalPageCount = pagerOptions.MaxPageIndex;
            _totalPageCount = totalPageCount;
            //如果总页数小于等于1，且不是自动隐藏，则总页数设置为1
            if (_totalPageCount < 1)
                _totalPageCount = 1;

            _pageIndex = pageIndex > 0 ? pageIndex : 1;
            _pagerOptions = pagerOptions;
            _routeName = routeName;
            _routeValues = routeValues;
            _ajaxOptions = ajaxOptions;
            _htmlAttributes = htmlAttributes;

            //修正起始页BUG
            //// start page index
            //_startPageIndex = pageIndex - (pagerOptions.NumericPagerItemCount / 2);
            //if (_startPageIndex + pagerOptions.NumericPagerItemCount > _totalPageCount)
            //    _startPageIndex = _totalPageCount + 1 - pagerOptions.NumericPagerItemCount;
            //if (_startPageIndex < 1)
            //    _startPageIndex = 1;

            //未设置MaxPageIndex
            if (pagerOptions.MaxPageIndex <= 0)
            {
                _startPageIndex = pageIndex - (pagerOptions.NumericPagerItemCount / 2);
                if (_startPageIndex + pagerOptions.NumericPagerItemCount > _totalPageCount)
                    _startPageIndex = _totalPageCount + 1 - pagerOptions.NumericPagerItemCount;
            }
            else
            {
                _startPageIndex = pageIndex - (pagerOptions.MaxPageIndex / 2);
                if (_startPageIndex + pagerOptions.MaxPageIndex > _totalPageCount)
                    _startPageIndex = _totalPageCount + 1 - pagerOptions.MaxPageIndex;
            }

            if (_startPageIndex < 1)
                _startPageIndex = 1;

            //修正最后页索引BUG
            //// end page index
            //_endPageIndex = _startPageIndex + _pagerOptions.NumericPagerItemCount - 1;
            //if (_endPageIndex > _totalPageCount)
            //    _endPageIndex = _totalPageCount;

            //未设置MaxPageIndex
            if (pagerOptions.MaxPageIndex <= 0)
            {
                _endPageIndex = _startPageIndex + _pagerOptions.NumericPagerItemCount - 1;
            }
            else
            {
                //已设置MaxPageIndex
                _endPageIndex = _startPageIndex + _pagerOptions.MaxPageIndex - 1;
            }

            if (_endPageIndex > _totalPageCount)
                _endPageIndex = _totalPageCount;
        }
        //non Ajax pager builder
        /// <summary>
        /// Initializes a new instance of the <see cref="PagerBuilder" /> class.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="totalPageCount">The total page count.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pagerOptions">The pager options.</param>
        /// <param name="routeName">Name of the route.</param>
        /// <param name="routeValues">The route values.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        internal PagerBuilder(HtmlHelper helper, string actionName, string controllerName, int totalPageCount,
            int pageIndex, PagerOptions pagerOptions, string routeName, RouteValueDictionary routeValues,
            IDictionary<string, object> htmlAttributes)
            : this(helper, null, actionName, controllerName,
                totalPageCount, pageIndex, pagerOptions, routeName, routeValues, null, htmlAttributes) { }
        //Microsoft Ajax pager builder
        /// <summary>
        /// Initializes a new instance of the <see cref="PagerBuilder" /> class.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="totalPageCount">The total page count.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pagerOptions">The pager options.</param>
        /// <param name="routeName">Name of the route.</param>
        /// <param name="routeValues">The route values.</param>
        /// <param name="ajaxOptions">The ajax options.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        internal PagerBuilder(AjaxHelper helper, string actionName, string controllerName, int totalPageCount,
            int pageIndex, PagerOptions pagerOptions, string routeName, RouteValueDictionary routeValues,
            AjaxOptions ajaxOptions, IDictionary<string, object> htmlAttributes)
            : this(null, helper, actionName,
                controllerName, totalPageCount, pageIndex, pagerOptions, routeName, routeValues, ajaxOptions, htmlAttributes) { }
        //jQuery Ajax pager builder
        /// <summary>
        /// Initializes a new instance of the <see cref="PagerBuilder" /> class.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="totalPageCount">The total page count.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pagerOptions">The pager options.</param>
        /// <param name="routeName">Name of the route.</param>
        /// <param name="routeValues">The route values.</param>
        /// <param name="ajaxOptions">The ajax options.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        internal PagerBuilder(HtmlHelper helper, string actionName, string controllerName, int totalPageCount,
            int pageIndex, PagerOptions pagerOptions, string routeName, RouteValueDictionary routeValues,
            AjaxOptions ajaxOptions, IDictionary<string, object> htmlAttributes)
            : this(helper, null, actionName,
                controllerName, totalPageCount, pageIndex, pagerOptions, routeName, routeValues, ajaxOptions, htmlAttributes) { }

        /// <summary>
        /// Adds the previous.
        /// </summary>
        /// <param name="results">The results.</param>
        private void AddPrevious(ICollection<PagerItem> results)
        {
            var item = new PagerItem(_pagerOptions.PrevPageText, _pageIndex - 1, _pageIndex == 1, PagerItemType.PrevPage);
            if (!item.Disabled || (item.Disabled && _pagerOptions.ShowDisabledPagerItems))
                results.Add(item);
        }
        /// <summary>
        /// Adds the first.
        /// </summary>
        /// <param name="results">The results.</param>
        private void AddFirst(ICollection<PagerItem> results)
        {
            var item = new PagerItem(_pagerOptions.FirstPageText, 1, _pageIndex == 1, PagerItemType.FirstPage);
            //只有导航按钮未被禁用，或导航按钮被禁用但ShowDisabledButtoms=true时才添加
            if (!item.Disabled || (item.Disabled && _pagerOptions.ShowDisabledPagerItems))
                results.Add(item);
        }


        /// <summary>
        /// Adds the more before.
        /// </summary>
        /// <param name="results">The results.</param>
        private void AddMoreBefore(ICollection<PagerItem> results)
        {
            if (_startPageIndex > 1 && _pagerOptions.ShowMorePagerItems)
            {
                var index = _startPageIndex - 1;
                if (index < 1) index = 1;
                var item = new PagerItem(_pagerOptions.MorePageText, index, false, PagerItemType.MorePage);
                results.Add(item);
            }
        }

        /// <summary>
        /// Adds the page numbers.
        /// </summary>
        /// <param name="results">The results.</param>
        private void AddPageNumbers(ICollection<PagerItem> results)
        {
            for (var pageIndex = _startPageIndex; pageIndex <= _endPageIndex; pageIndex++)
            {
                var text = pageIndex.ToString();
                if (pageIndex == _pageIndex && !string.IsNullOrEmpty(_pagerOptions.CurrentPageNumberFormatString))
                    text = String.Format(_pagerOptions.CurrentPageNumberFormatString, text);
                else if (!string.IsNullOrEmpty(_pagerOptions.PageNumberFormatString))
                    text = String.Format(_pagerOptions.PageNumberFormatString, text);
                var item = new PagerItem(text, pageIndex, false, PagerItemType.NumericPage);
                results.Add(item);
            }
        }

        /// <summary>
        /// Adds the more after.
        /// </summary>
        /// <param name="results">The results.</param>
        private void AddMoreAfter(ICollection<PagerItem> results)
        {
            //修正AddMoreAfter BUG
            //if (_endPageIndex < _totalPageCount)
            //{
            //    var index = _startPageIndex + _pagerOptions.NumericPagerItemCount;
            //    if (index > _totalPageCount) { index = _totalPageCount; }
            //    var item = new PagerItem(_pagerOptions.MorePageText, index, false, PagerItemType.MorePage);
            //    results.Add(item);
            //}

            var index = _startPageIndex + _pagerOptions.NumericPagerItemCount;

            if (_pagerOptions.MaxPageIndex > 0)
            {
                //已设置MaxPageIndex
                index = _startPageIndex + _pagerOptions.MaxPageIndex;
            }


            if (index <= _totalPageCount)
            {
                var item = new PagerItem(_pagerOptions.MorePageText, index, false, PagerItemType.MorePage);
                results.Add(item);
            }
        }

        /// <summary>
        /// Adds the next.
        /// </summary>
        /// <param name="results">The results.</param>
        private void AddNext(ICollection<PagerItem> results)
        {
            var item = new PagerItem(_pagerOptions.NextPageText, _pageIndex + 1, _pageIndex >= _totalPageCount, PagerItemType.NextPage);
            if (!item.Disabled || (item.Disabled && _pagerOptions.ShowDisabledPagerItems))
                results.Add(item);
        }

        /// <summary>
        /// Adds the last.
        /// </summary>
        /// <param name="results">The results.</param>
        private void AddLast(ICollection<PagerItem> results)
        {
            var item = new PagerItem(_pagerOptions.LastPageText, _totalPageCount, _pageIndex >= _totalPageCount, PagerItemType.LastPage);
            if (!item.Disabled || (item.Disabled && _pagerOptions.ShowDisabledPagerItems))
                results.Add(item);
        }


        /// <summary>
        /// 根据页索引生成分页导航Url
        /// </summary>
        /// <param name="pageIndex">要生成导航链接的页索引</param>
        /// <returns>分页导航链接Url</returns>
        private string GenerateUrl(int pageIndex)
        {
            //若要生成url的页索引小于1或大于总页数或等于当前页索引时，无需生成分页导航链接
            if (pageIndex > _totalPageCount || pageIndex == _pageIndex)
                return null;

            var routeValues = GetCurrentRouteValues(_html.ViewContext); // _routeValues??new RouteValueDictionary();

            // 设置Route Value中的分页导航Url参数值，pageIndex为0时生成适用于脚本的导航链接
            if (pageIndex == 0)
                routeValues[_pagerOptions.PageIndexParameterName] = ScriptPageIndexName;
            else
                routeValues[_pagerOptions.PageIndexParameterName] = pageIndex;

            // Return link
            var urlHelper = new UrlHelper(_html.ViewContext.RequestContext);
            if (!string.IsNullOrEmpty(_routeName))
                return urlHelper.RouteUrl(_routeName, routeValues);
            return urlHelper.RouteUrl(routeValues);
        }

        /// <summary>
        /// 生成最终的分页Html代码
        /// </summary>
        /// <returns></returns>
        internal MvcHtmlString RenderPager()
        {
            //总页数小于等于1，且自动隐藏，则不显示
            if (_totalPageCount <= 1 && _pagerOptions.AutoHide)
                return new MvcHtmlString(string.Empty);


            //Display error message if pageIndex out of range
            if ((_pageIndex > _totalPageCount && _totalPageCount > 0) || _pageIndex < 1)
            {
                return
                    MvcHtmlString.Create(string.Format("<div style=\"color:red;font-weight:bold\">{0}</div>",
                                          _pagerOptions.PageIndexOutOfRangeErrorMessage));
            }

            var pagerItems = new List<PagerItem>();
            //First page
            if (_pagerOptions.ShowFirstLast)
                AddFirst(pagerItems);

            // Prev page
            if (_pagerOptions.ShowPrevNext)
                AddPrevious(pagerItems);

            if (_pagerOptions.ShowNumericPagerItems)
            {
                //if (_pagerOptions.AlwaysShowFirstLastPageNumber && _startPageIndex > 1)
                //    pagerItems.Add(new PagerItem("1", 1, false, PagerItemType.NumericPage));

                // more page before numeric page buttons
                if (_pagerOptions.ShowMorePagerItems)
                    AddMoreBefore(pagerItems);

                // numeric page
                AddPageNumbers(pagerItems);

                // more page after numeric page buttons
                if (_pagerOptions.ShowMorePagerItems)
                    AddMoreAfter(pagerItems);

                //if (_pagerOptions.AlwaysShowFirstLastPageNumber && _endPageIndex < _totalPageCount)
                //    pagerItems.Add(new PagerItem(_totalPageCount.ToString(), _totalPageCount, false,
                //                                 PagerItemType.NumericPage));
            }

            // Next page
            if (_pagerOptions.ShowPrevNext)
                AddNext(pagerItems);

            //Last page
            if (_pagerOptions.ShowFirstLast)
                AddLast(pagerItems);

            var sb = new StringBuilder();
            if (_msAjaxPaging)
            {
                foreach (PagerItem item in pagerItems)
                {
                    sb.Append(GenerateMsAjaxPagerElement(item));
                }
            }
            else if (_pagerOptions.UseJqueryAjax)
            {
                foreach (PagerItem item in pagerItems)
                {
                    sb.Append(GenerateJqAjaxPagerElement(item));
                }
            }
            else
            {
                foreach (PagerItem item in pagerItems)
                {
                    sb.Append(GeneratePagerElement(item));
                }
            }
            var tb = new TagBuilder(_pagerOptions.ContainerTagName);
            if (!string.IsNullOrEmpty(_pagerOptions.Id))
                tb.GenerateId(_pagerOptions.Id);
            if (!string.IsNullOrEmpty(_pagerOptions.CssClass))
                tb.AddCssClass(_pagerOptions.CssClass);
            if (!string.IsNullOrEmpty(_pagerOptions.HorizontalAlign))
            {
                string strAlign = "text-align:" + _pagerOptions.HorizontalAlign.ToLower();
                if (_htmlAttributes == null)
                    _htmlAttributes = new RouteValueDictionary { { "style", strAlign } };
                else
                {
                    if (_htmlAttributes.Keys.Contains("style"))
                        _htmlAttributes["style"] += ";" + strAlign;
                }
            }
            tb.MergeAttributes(_htmlAttributes, true);
            string pagerScript = string.Empty;
            if (_pagerOptions.ShowPageIndexBox)
            {
                sb.Append(BuildGoToPageSection(ref pagerScript));
            }
            else
                sb.Length -= _pagerOptions.SeparatorHtml.Length;
            tb.InnerHtml = sb.ToString();
            if (!string.IsNullOrEmpty(pagerScript))
                pagerScript = "<script type=\"text/javascript\">//<![CDATA[\r\n" + pagerScript + "\r\n//]]>\r\n</script>";
            return MvcHtmlString.Create(pagerScript + tb.ToString(TagRenderMode.Normal));
        }

        /// <summary>
        /// Builds the go to page section.
        /// </summary>
        /// <param name="pagerScript">The pager script.</param>
        /// <returns></returns>
        private string BuildGoToPageSection(ref string pagerScript)
        {
            const string ctrlIndexName = "_MvcPager_ControlIndex";
            ViewContext viewCtx = _msAjaxPaging
                                      ? _ajax.ViewContext
                                      : _html.ViewContext;
            int ctrlIndex;
            if (int.TryParse((string)viewCtx.HttpContext.Items[ctrlIndexName], out ctrlIndex))
                ++ctrlIndex;
            viewCtx.HttpContext.Items[ctrlIndexName] = ctrlIndex.ToString();

            string controlId = "_MvcPager_Ctrl" + ctrlIndex;
            string scriptLink = GenerateAnchor(new PagerItem("0", 0, false, PagerItemType.NumericPage));
            
            if (ctrlIndex == 0)
            {
                pagerScript += KeyDownScript.Replace("%TotalPageCount%", _totalPageCount.ToString()) + GoToPageScript.Replace("%InvalidPageIndexErrorMessage%",
                                                                      _pagerOptions.InvalidPageIndexErrorMessage).Replace("%PageIndexOutOfRangeErrorMessage%",
                                                       _pagerOptions.PageIndexOutOfRangeErrorMessage);
            }
            string onChangeScript = null;
            if (!_pagerOptions.ShowGoButton)
                onChangeScript = " onchange=\"_MvcPager_GoToPage(this," + _totalPageCount + ")\"";
            var piBuilder = new StringBuilder();
            if (_pagerOptions.PageIndexBoxType == PageIndexBoxType.DropDownList)
            {
                // start page index
                int startIndex = _pageIndex - (_pagerOptions.MaximumPageIndexItems / 2);
                if (startIndex + _pagerOptions.MaximumPageIndexItems > _totalPageCount)
                    startIndex = _totalPageCount + 1 - _pagerOptions.MaximumPageIndexItems;
                if (startIndex < 1)
                    startIndex = 1;

                // end page index
                int endIndex = startIndex + _pagerOptions.MaximumPageIndexItems - 1;
                if (endIndex > _totalPageCount)
                    endIndex = _totalPageCount;
                piBuilder.AppendFormat("<select id=\"{0}\"{1}>", controlId + "_pib", onChangeScript);
                for (int i = startIndex; i <= endIndex; i++)
                {
                    piBuilder.AppendFormat("<option value=\"{0}\"", i);
                    if (i == _pageIndex)
                        piBuilder.Append(" selected=\"selected\"");
                    piBuilder.AppendFormat(">{0}</option>", i);
                }
                piBuilder.Append("</select>");
            }
            else
                piBuilder.AppendFormat(
                    "<input type=\"text\" id=\"{0}\" value=\"{1}\" onkeydown=\"_MvcPager_Keydown(event)\"{2}/>",
                    controlId + "_pib", _pageIndex, onChangeScript);
            string outHtml;
            if (!string.IsNullOrEmpty(_pagerOptions.PageIndexBoxWrapperFormatString))
            {
                outHtml = string.Format(_pagerOptions.PageIndexBoxWrapperFormatString, piBuilder);
                piBuilder = new StringBuilder(outHtml);
            }

            if (_pagerOptions.ShowGoButton)
                piBuilder.AppendFormat(
                    "<input type=\"button\" value=\"{0}\" style=\"margin-left: 2px;\" onclick=\"_MvcPager_GoToPage(document.getElementById('{1}')," + _totalPageCount + ")\"/>",
                    _pagerOptions.GoButtonText
                    , controlId + "_pib");
            piBuilder.AppendFormat("<span id=\"{0}\" style=\"display:none;width:0px;height:0px\">{1}</span>",
                                   controlId + "_piblink", scriptLink);
            if (!string.IsNullOrEmpty(_pagerOptions.GoToPageSectionWrapperFormatString) ||
                !string.IsNullOrEmpty(_pagerOptions.PagerItemWrapperFormatString))
            {
                outHtml = string.Format(
                    _pagerOptions.GoToPageSectionWrapperFormatString ?? _pagerOptions.PagerItemWrapperFormatString,
                    piBuilder);
            }
            else
                outHtml = piBuilder.ToString();
            return outHtml;
        }


        /// <summary>
        /// Generates the anchor.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        private string GenerateAnchor(PagerItem item)
        {
            if (_msAjaxPaging)
            {
                var routeValues = GetCurrentRouteValues(_ajax.ViewContext);
                if (item.PageIndex == 0)
                    routeValues[_pagerOptions.PageIndexParameterName] = ScriptPageIndexName;
                else
                    routeValues[_pagerOptions.PageIndexParameterName] = item.PageIndex;
                if (!string.IsNullOrEmpty(_routeName))
                    return _ajax.RouteLink(item.Text, _routeName, routeValues, _ajaxOptions).ToString();
                return _ajax.RouteLink(item.Text, routeValues, _ajaxOptions).ToString();
            }
            string url = GenerateUrl(item.PageIndex);
            if (_pagerOptions.UseJqueryAjax)
            {
                if (_html.ViewContext.UnobtrusiveJavaScriptEnabled)
                {
                    var tag = new TagBuilder("a") { InnerHtml = item.Text };
                    tag.MergeAttribute("href", url);
                    tag.MergeAttributes(_ajaxOptions.ToUnobtrusiveHtmlAttributes());
                    return string.IsNullOrEmpty(url)
                        ? _html.Encode(item.Text) : tag.ToString(TagRenderMode.Normal);
                }
                var scriptBuilder = new StringBuilder();

                if (!string.IsNullOrEmpty(_ajaxOptions.OnFailure) || !string.IsNullOrEmpty(_ajaxOptions.OnBegin) || (!string.IsNullOrEmpty(_ajaxOptions.OnComplete) && _ajaxOptions.HttpMethod.ToUpper() != "GET"))
                {
                    scriptBuilder.Append("$.ajax({type:\'").Append(_ajaxOptions.HttpMethod.ToUpper() == "GET" ? "get" : "post");
                    scriptBuilder.Append("\',url:$(this).attr(\'href\'),success:function(data,status,xhr){$(\'#");
                    scriptBuilder.Append(_ajaxOptions.UpdateTargetId).Append("\').html(data);}");
                    if (!string.IsNullOrEmpty(_ajaxOptions.OnFailure))
                        scriptBuilder.Append(",error:").Append(HttpUtility.HtmlAttributeEncode(_ajaxOptions.OnFailure));
                    if (!string.IsNullOrEmpty(_ajaxOptions.OnBegin))
                        scriptBuilder.Append(",beforeSend:").Append(HttpUtility.HtmlAttributeEncode(_ajaxOptions.OnBegin));
                    if (!string.IsNullOrEmpty(_ajaxOptions.OnComplete))
                        scriptBuilder.Append(",complete:").Append(
                            HttpUtility.HtmlAttributeEncode(_ajaxOptions.OnComplete));
                    scriptBuilder.Append("});return false;");
                }
                else
                {
                    if (_ajaxOptions.HttpMethod.ToUpper() == "GET")
                    {
                        scriptBuilder.Append("$(\'#").Append(_ajaxOptions.UpdateTargetId);
                        scriptBuilder.Append("\').load($(this).attr(\'href\')");
                        if (!string.IsNullOrEmpty(_ajaxOptions.OnComplete))
                            scriptBuilder.Append(",").Append(HttpUtility.HtmlAttributeEncode(_ajaxOptions.OnComplete));
                        scriptBuilder.Append(");return false;");
                    }
                    else
                    {
                        scriptBuilder.Append("$.post($(this).attr(\'href\'), function(data) {$(\'#");
                        scriptBuilder.Append(_ajaxOptions.UpdateTargetId);
                        scriptBuilder.Append("\').html(data);});return false;");
                    }
                }
                return string.IsNullOrEmpty(url)
                           ? _html.Encode(item.Text)
                           : String.Format(CultureInfo.InvariantCulture,
                                           "<a href=\"{0}\" onclick=\"{1}\">{2}</a>",
                                           url, scriptBuilder, item.Text);
            }
            return "<a href=\"" + url +
                   "\" onclick=\"window.open(this.attributes.getNamedItem('href').value,'_self')\"></a>";
        }

        /// <summary>
        /// Generates the pager element.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        private MvcHtmlString GeneratePagerElement(PagerItem item)
        {
            //pager item link
            string url = GenerateUrl(item.PageIndex);
            if (item.Disabled) //first,last,next or previous page
                return CreateWrappedPagerElement(item, String.Format("<a disabled=\"disabled\">{0}</a>", item.Text));
            return CreateWrappedPagerElement(item,
                                             string.IsNullOrEmpty(url)
                                                 ? _html.Encode(item.Text)
                                                 : String.Format("<a href='{0}'>{1}</a>", url, item.Text));
        }

        /// <summary>
        /// Generates the jq ajax pager element.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        private MvcHtmlString GenerateJqAjaxPagerElement(PagerItem item)
        {
            if (item.Disabled)
                return CreateWrappedPagerElement(item, String.Format("<a disabled=\"disabled\">{0}</a>", item.Text));
            return CreateWrappedPagerElement(item, GenerateAnchor(item));
        }

        /// <summary>
        /// Generates the ms ajax pager element.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        private MvcHtmlString GenerateMsAjaxPagerElement(PagerItem item)
        {
            if (item.PageIndex == _pageIndex && !item.Disabled) //current page index
                return CreateWrappedPagerElement(item, item.Text);
            if (item.Disabled)
                return CreateWrappedPagerElement(item, string.Format("<a disabled=\"disabled\">{0}</a>", item.Text));

            // return null if current page index less than 1 or large than total page count
            if (item.PageIndex < 1 || item.PageIndex > _totalPageCount)
                return null;

            return CreateWrappedPagerElement(item, GenerateAnchor(item));
        }

        /// <summary>
        /// Creates the wrapped pager element.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="el">The el.</param>
        /// <returns></returns>
        private MvcHtmlString CreateWrappedPagerElement(PagerItem item, string el)
        {
            string navStr = el;
            switch (item.Type)
            {
                case PagerItemType.FirstPage:
                case PagerItemType.LastPage:
                case PagerItemType.NextPage:
                case PagerItemType.PrevPage:
                    if ((!string.IsNullOrEmpty(_pagerOptions.NavigationPagerItemWrapperFormatString) ||
                         !string.IsNullOrEmpty(_pagerOptions.PagerItemWrapperFormatString)))
                        navStr =
                            string.Format(
                                _pagerOptions.NavigationPagerItemWrapperFormatString ??
                                _pagerOptions.PagerItemWrapperFormatString, el);
                    break;
                case PagerItemType.MorePage:
                    if ((!string.IsNullOrEmpty(_pagerOptions.MorePagerItemWrapperFormatString) ||
                         !string.IsNullOrEmpty(_pagerOptions.PagerItemWrapperFormatString)))
                        navStr =
                            string.Format(
                                _pagerOptions.MorePagerItemWrapperFormatString ??
                                _pagerOptions.PagerItemWrapperFormatString, el);
                    break;
                case PagerItemType.NumericPage:
                    if (item.PageIndex == _pageIndex &&
                        (!string.IsNullOrEmpty(_pagerOptions.CurrentPagerItemWrapperFormatString) ||
                         !string.IsNullOrEmpty(_pagerOptions.PagerItemWrapperFormatString))) //current page
                        navStr =
                            string.Format(
                                _pagerOptions.CurrentPagerItemWrapperFormatString ??
                                _pagerOptions.PagerItemWrapperFormatString, el);
                    else if (!string.IsNullOrEmpty(_pagerOptions.NumericPagerItemWrapperFormatString) ||
                             !string.IsNullOrEmpty(_pagerOptions.PagerItemWrapperFormatString))
                        navStr =
                            string.Format(
                                _pagerOptions.NumericPagerItemWrapperFormatString ??
                                _pagerOptions.PagerItemWrapperFormatString, el);
                    break;
            }
            return MvcHtmlString.Create(navStr + _pagerOptions.SeparatorHtml);
        }


        /// <summary>
        /// Gets the current route values.
        /// </summary>
        /// <param name="viewContext">The view context.</param>
        /// <returns></returns>
        private RouteValueDictionary GetCurrentRouteValues(ViewContext viewContext)
        {
            var routeValues = _routeValues ?? new RouteValueDictionary();
            var rq = viewContext.HttpContext.Request.QueryString;
            if (rq != null && rq.Count > 0)
            {
                var invalidParams = new[] { "x-requested-with", "xmlhttprequest", _pagerOptions.PageIndexParameterName.ToLower() };
                foreach (string key in rq.Keys)
                {
                    // 添加url参数到路由中
                    if (!string.IsNullOrEmpty(key) && Array.IndexOf(invalidParams, key.ToLower()) < 0)
                    {
                        routeValues[key] = rq[key];
                    }
                }
            }
            // action
            routeValues["action"] = _actionName;
            // controller
            routeValues["controller"] = _controllerName;
            return routeValues;
        }
    }
}