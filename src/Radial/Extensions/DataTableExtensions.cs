using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace Radial.Extensions
{
    /// <summary>
    /// DataTableExtensions
    /// </summary>
    public static class DataTableExtensions
    {
        /// <summary>
        /// To the excel book.
        /// </summary>
        /// <param name="table">The data table.</param>
        /// <returns>
        /// IWorkbook instance.
        /// </returns>
        public static IWorkbook ToExcelBook(this DataTable table)
        {
            IWorkbook hssfworkbook = new HSSFWorkbook();

            ISheet sheet = hssfworkbook.CreateSheet();

            if (table != null)
            {
                //caption
                IRow captionRow = sheet.CreateRow(0);
                for (int columi = 0; columi < table.Columns.Count; columi++)
                {
                    captionRow.CreateCell(columi).SetCellValue(table.Columns[columi].ColumnName);
                }

                //data    
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    IRow dataRow = sheet.CreateRow(i + 1);

                    for (int j = 0; j < table.Columns.Count; j++)
                        dataRow.CreateCell(j).SetCellValue(table.Rows[i][j].ToString());
                }
            }

            return hssfworkbook;
        }
    }
}
