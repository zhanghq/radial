using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using Radial.Param;
using NHibernate.Criterion;

namespace Radial.Data.Nhs.Param
{
    /// <summary>
    /// SqlCe param repository.
    /// </summary>
    public class SqlCeParamRepository : BasicRepository<ParamEntity, string>, IParamRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlServerParamRepository"/> class.
        /// </summary>
        public SqlCeParamRepository() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlServerParamRepository"/> class.
        /// </summary>
        /// <param name="session">The NHibernate session object.</param>
        public SqlCeParamRepository(ISession session) : base(session) { }

        /// <summary>
        /// Searches the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public IList<ParamEntity> Search(string path)
        {
            path = ParamObject.NormalizePath(path);

            var q = Session.QueryOver<ParamEntity>()
                .Where(Expression.Like(Projections.Property<ParamEntity>(o => o.Path), path, MatchMode.Start))
                .OrderBy(o => o.Name).Asc;

            return q.List();
        }

        /// <summary>
        /// Searches the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="objectTotal">The object total.</param>
        /// <returns></returns>
        public IList<ParamEntity> Search(string path, int pageSize, int pageIndex, out int objectTotal)
        {
            path = ParamObject.NormalizePath(path);
            ValidatePagingParameter(pageSize, pageIndex);

            var q = Session.QueryOver<ParamEntity>()
                 .Where(Expression.Like(Projections.Property<ParamEntity>(o => o.Path), path, MatchMode.Start));

            objectTotal = q.ToRowCountQuery().RowCount();

            return q.Skip(pageSize * (pageIndex - 1)).Take(pageSize).List();
        }

        /// <summary>
        /// Getses the specified where.
        /// </summary>
        /// <param name="where">The where.</param>
        /// <param name="orderBys">The order bys.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="objectTotal">The object total.</param>
        /// <returns></returns>
        public override IList<ParamEntity> Gets(System.Linq.Expressions.Expression<Func<ParamEntity, bool>> where, OrderBySnippet<ParamEntity>[] orderBys, int pageSize, int pageIndex, out int objectTotal)
        {
            ValidatePagingParameter(pageSize, pageIndex);

            var q = Session.QueryOver<ParamEntity>();

            if (where != null)
            {
                q=q.Where(where);
            }

            objectTotal = q.ToRowCountQuery().RowCount();

            if (orderBys != null)
            {
                foreach (OrderBySnippet<ParamEntity> order in orderBys)
                {
                    if (order.IsAscending)
                        q=q.OrderBy(order.Property).Asc;
                    else
                        q = q.OrderBy(order.Property).Desc;
                }
            }

            return q.Skip(pageSize * (pageIndex - 1)).Take(pageSize).List();
        }
    }
}
