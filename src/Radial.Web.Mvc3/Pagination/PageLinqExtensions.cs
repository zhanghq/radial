/*
 ASP.NET MvcPager 分页组件 Thanks To Webdiyer
 */
using System.Linq;

namespace Radial.Web.Mvc.Pagination
{
    /// <summary>
    /// PageLinqExtensions
    /// </summary>
    public static class PageLinqExtensions
    {
        /// <summary>
        /// To the paged list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="allItems">All items.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        public static PagedList<T> ToPagedList<T>
            (
                this IQueryable<T> allItems,
                int pageIndex,
                int pageSize
            )
        {
            if (pageIndex < 1)
                pageIndex = 1;
            var itemIndex = (pageIndex-1) * pageSize;
            var pageOfItems = allItems.Skip(itemIndex).Take(pageSize);
            var totalItemCount = allItems.Count();
            return new PagedList<T>(pageOfItems, pageIndex, pageSize, totalItemCount);
        }
    }
}
