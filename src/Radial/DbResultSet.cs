using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial
{
    /// <summary>
    /// DbResultSet
    /// </summary>
    public class DbResultSet : IEnumerable<DbResultRow>
    {
        IList<DbResultRow> rows;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbResultSet"/> class.
        /// </summary>
        public DbResultSet()
        {
            rows = new List<DbResultRow>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbResultSet"/> class.
        /// </summary>
        /// <param name="rows">The rows.</param>
        public DbResultSet(IEnumerable<DbResultRow> rows)
        {
            this.rows = new List<DbResultRow>(rows);
        }

        /// <summary>
        /// Gets the <see cref="DbResultRow"/> at the specified index.
        /// </summary>
        /// <value>
        /// The <see cref="DbResultRow"/>.
        /// </value>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public DbResultRow this[int index]
        {
            get
            {
                return rows[index];
            }
        }


        /// <summary>
        /// Gets the row count.
        /// </summary>
        /// <value>
        /// The row count.
        /// </value>
        public int RowCount
        {
            get
            {
                return rows.Count; ;
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<DbResultRow> GetEnumerator()
        {
            return rows.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return rows.GetEnumerator();
        }
    }
}
