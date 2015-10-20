using NHibernate.SqlCommand;
using NHibernate.Criterion;

namespace Radial.Persist.Nhs.Order
{
    /// <summary>
    /// SqlServerRandomOrder
    /// </summary>
    public class SqlServerRandomOrder : NHibernate.Criterion.Order
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlServerRandomOrder"/> class.
        /// </summary>
        public SqlServerRandomOrder() : base("", true) { }

        /// <summary>
        /// Render the SQL fragment
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="criteriaQuery"></param>
        /// <returns></returns>
        public override SqlString ToSqlString(global::NHibernate.ICriteria criteria, ICriteriaQuery criteriaQuery)
        {
            return new SqlString("newid()" + (ascending ? " asc" : " desc"));
        }
    }
}
