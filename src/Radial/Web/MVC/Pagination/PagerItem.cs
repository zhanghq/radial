/*
 ASP.NET MvcPager 分页组件 Thanks To Webdiyer
 */
namespace Radial.Web.Mvc.Pagination
{
    /// <summary>
    /// PagerItem
    /// </summary>
    internal class PagerItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PagerItem" /> class.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="disabled">if set to <c>true</c> [disabled].</param>
        /// <param name="type">The type.</param>
        public PagerItem(string text, int pageIndex, bool disabled, PagerItemType type)
        {
            Text = text;
            PageIndex = pageIndex;
            Disabled = disabled;
            Type = type;
        }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        internal string Text { get; set; }
        /// <summary>
        /// Gets or sets the index of the page.
        /// </summary>
        /// <value>
        /// The index of the page.
        /// </value>
        internal int PageIndex { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="PagerItem" /> is disabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if disabled; otherwise, <c>false</c>.
        /// </value>
        internal bool Disabled { get; set; }
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        internal PagerItemType Type { get; set; }
    }

    /// <summary>
    /// PagerItemType
    /// </summary>
    internal enum PagerItemType:byte
    {
        /// <summary>
        /// FirstPage
        /// </summary>
        FirstPage,
        /// <summary>
        /// NextPage
        /// </summary>
        NextPage,
        /// <summary>
        /// PrevPage
        /// </summary>
        PrevPage,
        /// <summary>
        /// LastPage
        /// </summary>
        LastPage,
        /// <summary>
        /// MorePage
        /// </summary>
        MorePage,
        /// <summary>
        /// NumericPage
        /// </summary>
        NumericPage
    }
}
