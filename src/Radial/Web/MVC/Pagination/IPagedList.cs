/*
 ASP.NET MvcPager 分页组件 Thanks To Webdiyer
 */
namespace Radial.Web.Mvc.Pagination
{
    /// <summary>
    /// IPagedList
    /// </summary>
    public interface IPagedList
    {
        /// <summary>
        /// Gets or sets the index of the current page.
        /// </summary>
        /// <value>
        /// The index of the current page.
        /// </value>
        int CurrentPageIndex { get; set; }
        /// <summary>
        /// Gets or sets the size of the page.
        /// </summary>
        /// <value>
        /// The size of the page.
        /// </value>
        int PageSize { get; set; }
        /// <summary>
        /// Gets or sets the total item count.
        /// </summary>
        /// <value>
        /// The total item count.
        /// </value>
        int TotalItemCount { get; set; }
    }
}
