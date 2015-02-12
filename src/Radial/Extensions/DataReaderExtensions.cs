using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial.Extensions
{
    /// <summary>
    /// DataReaderExtensions
    /// </summary>
    public static class DataReaderExtensions
    {
        /// <summary>
        /// To the row list.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        public static IList<IList<object>> ToRowList(this IDataReader reader)
        {
            IList<IList<object>> list = new List<IList<object>>();

            if (reader == null)
                return list;


            while (reader.Read())
            {
                List<object> alist = new List<object>(reader.FieldCount);
                for (int i = 0; i < reader.FieldCount; i++)
                    alist.Add(reader[i]);
                list.Add(alist);
            }

            return list;
        }

        /// <summary>
        /// To the row list.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="topRowCount">The top row count.</param>
        /// <returns></returns>
        public static IList<IList<object>> ToRowList(this IDataReader reader, int topRowCount)
        {
            IList<IList<object>> list = new List<IList<object>>();

            if (reader == null)
                return list;

            while (reader.Read())
            {
                List<object> alist = new List<object>(reader.FieldCount);
                for (int i = 0; i < reader.FieldCount; i++)
                    alist.Add(reader[i]);
                list.Add(alist);

                if (list.Count >= topRowCount)
                    break;
            }

            return list;
        }
    }
}
