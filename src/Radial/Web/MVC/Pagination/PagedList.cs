/*
 ASP.NET MvcPager 分页组件 Thanks To Webdiyer
 */
using System;
using System.Collections.Generic;

namespace Radial.Web.Mvc.Pagination
{
    /// <summary>
    /// PagedList
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedList<T> : List<T>,IPagedList
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PagedList{T}" /> class.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalItemCount">The total item count.</param>
        public PagedList(IEnumerable<T> items, int pageIndex, int pageSize, int totalItemCount)
        {
            AddRange(items);
            TotalItemCount = totalItemCount;
            CurrentPageIndex = pageIndex;
            PageSize = pageSize;
        }

        /// <summary>
        /// Gets or sets the index of the current page.
        /// </summary>
        /// <value>
        /// The index of the current page.
        /// </value>
        public int CurrentPageIndex { get; set; }
        /// <summary>
        /// Gets or sets the size of the page.
        /// </summary>
        /// <value>
        /// The size of the page.
        /// </value>
        public int PageSize { get; set; }
        /// <summary>
        /// Gets or sets the total item count.
        /// </summary>
        /// <value>
        /// The total item count.
        /// </value>
        public int TotalItemCount { get; set; }
        /// <summary>
        /// Gets the total page count.
        /// </summary>
        /// <value>
        /// The total page count.
        /// </value>
        public int TotalPageCount { get { return (int)Math.Ceiling(TotalItemCount / (double)PageSize); } }
        /// <summary>
        /// Gets the start index of the record.
        /// </summary>
        /// <value>
        /// The start index of the record.
        /// </value>
        public int StartRecordIndex { get { return (CurrentPageIndex - 1) * PageSize + 1; } }
        /// <summary>
        /// Gets the end index of the record.
        /// </summary>
        /// <value>
        /// The end index of the record.
        /// </value>
        public int EndRecordIndex { get { return TotalItemCount > CurrentPageIndex * PageSize ? CurrentPageIndex * PageSize : TotalItemCount; } }
    }
}
