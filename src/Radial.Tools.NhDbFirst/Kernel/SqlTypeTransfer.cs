using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radial.Tools.NhDbFirst.Kernel
{
    static class SqlTypeTransfer
    {
        public static string GetPropertyTypeString(DataSource dataSource, string sqlType)
        {
            switch (dataSource)
            {
                case DataSource.SqlServer: return GetPropertyTypeStringSqlServer(sqlType);
                case DataSource.MySql: return GetPropertyTypeStringMySql(sqlType);
                default: throw new NotSupportedException("不支持的数据源类型：" + dataSource.ToString());
            }
        }

        /// <summary>
        /// Gets the property type string my SQL.
        /// </summary>
        /// <param name="sqlType">Type of the SQL.</param>
        /// <returns></returns>
        /// <exception cref="System.NotSupportedException">不支持类型： + sqlType</exception>
        private static string GetPropertyTypeStringMySql(string sqlType)
        {
            string propertyTypeString = string.Empty;

            sqlType = sqlType.ToLower();

            if (sqlType.IndexOf('(') > 0)
                sqlType = sqlType.Substring(0, sqlType.IndexOf('('));
            if (sqlType.IndexOf(' ') > 0)
                sqlType = sqlType.Substring(0, sqlType.IndexOf(' '));

            switch (sqlType)
            {
                case "tinyint":
                    propertyTypeString = "byte";
                    break;
                case "smallint":
                    propertyTypeString = "short";
                    break;
                case "mediumint":
                case "int":
                case "integer":
                    propertyTypeString = "int";
                    break;
                case "bigint":
                    propertyTypeString = "long";
                    break;
                case "bit":
                    propertyTypeString = "bool";
                    break;
                case "real":
                    propertyTypeString = "float";
                    break;
                case "double":
                    propertyTypeString = "double";
                    break;
                case "float":
                    propertyTypeString = "float";
                    break;
                case "decimal":
                    propertyTypeString = "decimal";
                    break;
                case "numeric":
                    propertyTypeString = "decimal";
                    break;
                case "char":
                case "varchar":
                case "tinytext":
                case "text":
                case "mediumtext":
                case "longtext":
                    propertyTypeString = "string";
                    break;
                case "date":
                case "time":
                case "datetime":
                case "timestamp":
                    propertyTypeString = "DateTime";
                    break;
                case "tinyblob":
                case "blob":
                case "mediumblob":
                case "longblob":
                    propertyTypeString = "byte[]";
                    break;

                default: throw new NotSupportedException("不支持类型：" + sqlType);
            }
            return propertyTypeString;
        }

        /// <summary>
        /// Gets the property type string SQL server.
        /// </summary>
        /// <param name="sqlType">Type of the SQL.</param>
        /// <returns></returns>
        /// <exception cref="System.NotSupportedException">不支持类型： + sqlType</exception>
        private static string GetPropertyTypeStringSqlServer(string sqlType)
        {
            string propertyTypeString = string.Empty;

            switch (sqlType.ToLower())
            {
                case "tinyint":
                    propertyTypeString = "byte";
                    break;
                case "smallint":
                    propertyTypeString = "short";
                    break;
                case "int":
                    propertyTypeString = "int";
                    break;
                case "text":
                case "ntext":
                case "char":
                case "nchar":
                case "varchar":
                case "nvarchar":
                    propertyTypeString = "string";
                    break;
                case "bigint":
                    propertyTypeString = "long";
                    break;
                case "binary":
                    propertyTypeString = "byte[]";
                    break;
                case "bit":
                    propertyTypeString = "bool";
                    break;
                case "datetime":
                    propertyTypeString = "DateTime";
                    break;
                case "decimal":
                    propertyTypeString = "decimal";
                    break;
                case "float":
                    propertyTypeString = "double";
                    break;
                case "image":
                    propertyTypeString = "byte[]";
                    break;
                case "money":
                    propertyTypeString = "decimal";
                    break;
                case "numeric":
                    propertyTypeString = "decimal";
                    break;
                case "real":
                    propertyTypeString = "float";
                    break;
                case "smalldatetime":
                    propertyTypeString = "DateTime";
                    break;
                case "smallmoney":
                    propertyTypeString = "decimal";
                    break;
                case "timestamp":
                    propertyTypeString = "byte[]";
                    break;
                case "uniqueidentifier":
                    propertyTypeString = "Guid";
                    break;
                case "varbinary":
                    propertyTypeString = "byte[]";
                    break;
                case "sql_variant":
                    propertyTypeString = "object";
                    break;

                default: throw new NotSupportedException("不支持类型：" + sqlType);
            }
            return propertyTypeString;
        }
    }
}
