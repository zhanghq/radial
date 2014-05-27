using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Cfg;
using NHibernate.Util;

namespace Radial.Persist.Nhs.NamingStrategy
{
    /// <summary>
    /// Sqlite naming strategy.
    /// </summary>
    public sealed class SqliteNamingStrategy : INamingStrategy
    {
        /// <summary>
        /// Return a table name for an entity class
        /// </summary>
        /// <param name="className">the fully-qualified class name</param>
        /// <returns>
        /// a table name
        /// </returns>
        public string ClassToTableName(string className)
        {
            return string.Format("'{0}'", StringHelper.Unqualify(className));
        }

        /// <summary>
        /// Alter the column name given in the mapping document
        /// </summary>
        /// <param name="columnName">a column name</param>
        /// <returns>
        /// a column name
        /// </returns>
        public string ColumnName(string columnName)
        {
            columnName = columnName.Trim('\'');
            return string.Format("'{0}'", columnName);
        }

        /// <summary>
        /// Return the logical column name used to refer to a column in the metadata
        /// (like index, unique constraints etc)
        /// A full bijection is required between logicalNames and physical ones
        /// logicalName have to be case insersitively unique for a given table
        /// </summary>
        /// <param name="columnName">given column name if any</param>
        /// <param name="propertyName">property name of this column</param>
        /// <returns></returns>
        public string LogicalColumnName(string columnName, string propertyName)
        {
            columnName = columnName.Trim('\'');
            return string.Format("'{0}'", StringHelper.IsNotEmpty(columnName) ? columnName : StringHelper.Unqualify(propertyName));
        }

        /// <summary>
        /// Return a column name for a property path expression
        /// </summary>
        /// <param name="propertyName">a property path</param>
        /// <returns>
        /// a column name
        /// </returns>
        public string PropertyToColumnName(string propertyName)
        {
            return string.Format("'{0}'", StringHelper.Unqualify(propertyName));
        }

        /// <summary>
        /// Return a table name for a collection
        /// </summary>
        /// <param name="className">the fully-qualified name of the owning entity class</param>
        /// <param name="propertyName">a property path</param>
        /// <returns>
        /// a table name
        /// </returns>
        public string PropertyToTableName(string className, string propertyName)
        {
            return string.Format("'{0}'", StringHelper.Unqualify(propertyName));
        }

        /// <summary>
        /// Alter the table name given in the mapping document
        /// </summary>
        /// <param name="tableName">a table name</param>
        /// <returns>
        /// a table name
        /// </returns>
        public string TableName(string tableName)
        {
            tableName = tableName.Trim('\'');

            return string.Format("'{0}'", tableName);
        }
    }
}
