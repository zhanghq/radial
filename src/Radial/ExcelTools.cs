using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using System.IO;
using Radial.Net;

namespace Radial
{
    /// <summary>
    /// Excel tools
    /// </summary>
    public static class ExcelTools
    {
        /// <summary>
        /// Exports to HTTP stream.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="fileName"> The download file name.</param>
        public static void ExportToHttp(DataTable data, string fileName)
        {
            Checker.Parameter(fileName != null, "download file name can not be empty or null");

            fileName=fileName.Trim();

            IWorkbook book = BuildHSSFWorkbook(data);

            if (fileName.LastIndexOf('.') > 0)
            {
                string ext = fileName.Substring(fileName.LastIndexOf('.'));

                if (ext.ToLower() == ".xls" || ext.ToLower() == ".xlsx")
                    fileName.Replace(ext, string.Empty);
            }


            HttpResponse httpResponse = HttpContext.Current.Response;

            httpResponse.Clear();
            httpResponse.Buffer = true;
            httpResponse.Charset = Encoding.UTF8.BodyName;
            httpResponse.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName, Encoding.UTF8) + ".xls");
            httpResponse.ContentEncoding = Encoding.UTF8;
            httpResponse.ContentType = ContentTypes.Excel;
            book.Write(httpResponse.OutputStream);
            httpResponse.End();
        }

        /// <summary>
        /// Exports to file.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="savePath">The export file save path</param>
        public static void ExportToFile(DataTable data, string savePath)
        {
            Checker.Parameter(savePath != null, "export file save path can not be empty or null");

            savePath = savePath.Trim();

            IWorkbook book = BuildHSSFWorkbook(data);

            if (savePath.LastIndexOf('.') > 0)
            {
                string ext = savePath.Substring(savePath.LastIndexOf('.'));

                if (ext.ToLower() == ".xls" || ext.ToLower() == ".xlsx")
                    savePath.Replace(ext, string.Empty);
            }

            savePath += ".xls";

            using (FileStream fs = new FileStream(savePath, FileMode.Create))
            {
                book.Write(fs);
            }
        }

        /// <summary>
        /// Builds the workbook.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        private static HSSFWorkbook BuildHSSFWorkbook(DataTable data)
        {
            Checker.Parameter(data != null, "export data can not be null");

            HSSFWorkbook book = new HSSFWorkbook();

            ISheet sheet = book.CreateSheet(string.IsNullOrWhiteSpace(data.TableName) ? "Sheet1" : data.TableName);

            //Title Row
            IRow row0 = sheet.CreateRow(0);
            for (int i = 0; i < data.Columns.Count; i++)
                row0.CreateCell(i, CellType.STRING).SetCellValue(data.Columns[i].ColumnName);

            //Data Rows
            for (int i = 0; i < data.Rows.Count; i++)
            {
                IRow drow = sheet.CreateRow(i + 1);
                for (int j = 0; j < data.Columns.Count; j++)
                    drow.CreateCell(j, CellType.STRING).SetCellValue(data.Rows[i][j].ToString());
            }

            return book;
        }
    }
}
