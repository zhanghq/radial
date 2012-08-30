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
    /// SqlServer param repository.
    /// </summary>
    public class SqlServerParamRepository : BasicRepository<ParamEntity, string>, IParamRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlServerParamRepository"/> class.
        /// </summary>
        /// <param name="session">The IUnitOfWork instance.</param>
        public SqlServerParamRepository(IUnitOfWork uow) : base(uow) { }

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

            var q = Session.QueryOver<ParamEntity>()
                 .Where(Expression.Like(Projections.Property<ParamEntity>(o => o.Path), path, MatchMode.Start));

            return ExecutePagingQuery(q, q.OrderBy(o => o.Name).Asc, pageSize, pageIndex, out objectTotal);
        }
    }
}
