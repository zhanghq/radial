using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using Radial.Net;
using System.IO;

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
                row0.CreateCell(i, CellType.String).SetCellValue(data.Columns[i].ColumnName);

            //Data Rows
            for (int i = 0; i < data.Rows.Count; i++)
            {
                IRow drow = sheet.CreateRow(i + 1);
                for (int j = 0; j < data.Columns.Count; j++)
                    drow.CreateCell(j, CellType.String).SetCellValue(data.Rows[i][j].ToString());
            }

            return book;
        }

        /// <summary>
        /// Imports to data table.
        /// </summary>
        /// <param name="excelFilePath">The excel file path.</param>
        /// <param name="sheetIndex">The zero-based index of the sheet.</param>
        /// <param name="cellValueInterpreter">The cell value interpreter.</param>
        /// <param name="firstRowHeader">if set to <c>true</c> [first row header].</param>
        /// <returns></returns>
        public static DataTable ImportToDataTable(string excelFilePath, int sheetIndex, Func<ICell, object> cellValueInterpreter = null, bool firstRowHeader = true)
        {
            IWorkbook workbook = WorkbookFactory.Create(excelFilePath);
            ISheet sheet = workbook.GetSheetAt(sheetIndex);

            return BuildDataTableFromSheet(sheet, cellValueInterpreter, firstRowHeader);
        }

        /// <summary>
        /// Imports to data table.
        /// </summary>
        /// <param name="excelFilePath">The excel file path.</param>
        /// <param name="sheetName">Name of the sheet.</param>
        /// <param name="cellValueInterpreter">The cell value interpreter.</param>
        /// <param name="firstRowHeader">if set to <c>true</c> [first row header].</param>
        /// <returns></returns>
        public static DataTable ImportToDataTable(string excelFilePath, string sheetName, Func<ICell, object> cellValueInterpreter = null, bool firstRowHeader = true)
        {
            IWorkbook workbook = WorkbookFactory.Create(excelFilePath);
            ISheet sheet = workbook.GetSheet(sheetName);

            return BuildDataTableFromSheet(sheet, cellValueInterpreter, firstRowHeader);
        }

        /// <summary>
        /// Imports to data table.
        /// </summary>
        /// <param name="excelStream">The excel stream.</param>
        /// <param name="sheetIndex">The zero-based index of the sheet.</param>
        /// <param name="cellValueInterpreter">The cell value interpreter.</param>
        /// <param name="firstRowHeader">if set to <c>true</c> [first row header].</param>
        /// <returns></returns>
        public static DataTable ImportToDataTable(Stream excelStream, int sheetIndex, Func<ICell, object> cellValueInterpreter = null, bool firstRowHeader = true)
        {
            IWorkbook workbook = WorkbookFactory.Create(excelStream);
            ISheet sheet = workbook.GetSheetAt(sheetIndex);

            return BuildDataTableFromSheet(sheet, cellValueInterpreter, firstRowHeader);
        }

        /// <summary>
        /// Imports to data table.
        /// </summary>
        /// <param name="excelStream">The excel stream.</param>
        /// <param name="sheetName">Name of the sheet.</param>
        /// <param name="cellValueInterpreter">The cell value interpreter.</param>
        /// <param name="firstRowHeader">if set to <c>true</c> [first row header].</param>
        /// <returns></returns>
        public static DataTable ImportToDataTable(Stream excelStream, string sheetName, Func<ICell, object> cellValueInterpreter = null, bool firstRowHeader = true)
        {
            IWorkbook workbook = WorkbookFactory.Create(excelStream);
            ISheet sheet = workbook.GetSheet(sheetName);

            return BuildDataTableFromSheet(sheet,cellValueInterpreter,firstRowHeader);
        }

        /// <summary>
        /// Builds the data table from sheet.
        /// </summary>
        /// <param name="sheet">The sheet.</param>
        /// <param name="cellValueInterpreter">The cell value interpreter.</param>
        /// <param name="firstRowHeader">if set to <c>true</c> [first row header].</param>
        /// <returns></returns>
        private static DataTable BuildDataTableFromSheet(ISheet sheet, Func<ICell, object> cellValueInterpreter, bool firstRowHeader)
        {
            DataTable table = new DataTable();

            IRow firstRow = sheet.GetRow(0);

            int cellCount = firstRow.LastCellNum;

            IDictionary<string, int> sameColumns = new Dictionary<string, int>();

            for (int i = firstRow.FirstCellNum; i < cellCount; i++)
            {
                if (firstRowHeader)
                {
                    ICell hc = firstRow.GetCell(i);

                    string columnName = hc.ToString().Trim();

                    if (table.Columns.Contains(columnName))
                    {
                        if (!sameColumns.ContainsKey(columnName))
                            sameColumns[columnName] = 1;
                        else
                            sameColumns[columnName]++;

                        columnName += sameColumns[columnName];
                    }

                    table.Columns.Add(new DataColumn(columnName));
                }
                else
                    table.Columns.Add(new DataColumn(i.ToString()));
            }


            //读取数据行
            for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                if (row == null) continue;
                DataRow dataRow = table.NewRow();
                for (int j = row.FirstCellNum; j < cellCount; j++)
                {
                    ICell cell = row.GetCell(j);

                    if (cell != null)
                    {
                        if (cellValueInterpreter == null)
                        {
                            switch (cell.CellType)
                            {
                                case CellType.Blank: dataRow[j] = null; break;
                                case CellType.Boolean: dataRow[j] = cell.BooleanCellValue; break;
                                case CellType.Numeric:
                                    //区分BuiltIn的数字和时间
                                    if ((cell.CellStyle.DataFormat >= 14 && cell.CellStyle.DataFormat <= 22) || (cell.CellStyle.DataFormat >= 45 && cell.CellStyle.DataFormat <= 47))
                                        dataRow[j] = cell.DateCellValue;
                                    else
                                        dataRow[j] = cell.NumericCellValue;
                                    break;
                                case CellType.String: dataRow[j] = cell.StringCellValue; break;
                                default: dataRow[j] = cell.ToString(); break;
                            }
                        }
                        else
                            dataRow[j] = cellValueInterpreter(cell);
                    }
                }
                table.Rows.Add(dataRow);
            }

            return table;
        }
    }
}
