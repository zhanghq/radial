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
using Radial.Web;

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
        /// <param name="table">The export data table.</param>
        /// <param name="downloadFileName">The download file name.</param>
        /// <param name="columnHeader">if set to <c>true</c> will set column name as header.</param>
        /// <param name="headerCellStyleFormater">The header cell style formater.</param>
        /// <param name="dataCellStyleFormater">The data cell style formater.</param>
        public static void ExportToHttp(DataTable table, string downloadFileName, bool columnHeader = true, Func<ICell, ICellStyle> headerCellStyleFormater = null, Func<ICell, ICellStyle> dataCellStyleFormater = null)
        {
            ExportToHttp(new DataTable[] { table }, downloadFileName, columnHeader, headerCellStyleFormater, dataCellStyleFormater);
        }

        /// <summary>
        /// Exports to HTTP stream.
        /// </summary>
        /// <param name="dataSet">The export data set.</param>
        /// <param name="downloadFileName">The download file name.</param>
        /// <param name="columnHeader">if set to <c>true</c> will set column name as header.</param>
        /// <param name="headerCellStyleFormater">The header cell style formater.</param>
        /// <param name="dataCellStyleFormater">The data cell style formater.</param>
        public static void ExportToHttp(DataSet dataSet, string downloadFileName, bool columnHeader = true, Func<ICell, ICellStyle> headerCellStyleFormater = null, Func<ICell, ICellStyle> dataCellStyleFormater = null)
        {
            Checker.Parameter(dataSet != null, "export data set can not be null");
            DataTable[] tables = new DataTable[dataSet.Tables.Count];
            dataSet.Tables.CopyTo(tables, 0);

            ExportToHttp(tables, downloadFileName, columnHeader, headerCellStyleFormater, dataCellStyleFormater);
        }

        /// <summary>
        /// Exports to HTTP stream.
        /// </summary>
        /// <param name="tables">The export data tables.</param>
        /// <param name="downloadFileName">The download file name.</param>
        /// <param name="columnHeader">if set to <c>true</c> will set column name as header.</param>
        /// <param name="headerCellStyleFormater">The header cell style formater.</param>
        /// <param name="dataCellStyleFormater">The data cell style formater.</param>
        public static void ExportToHttp(IEnumerable<DataTable> tables, string downloadFileName, bool columnHeader = true, Func<ICell, ICellStyle> headerCellStyleFormater = null, Func<ICell, ICellStyle> dataCellStyleFormater = null)
        {
            if (!HttpKits.IsWebApp)
                return;

            Checker.Parameter(downloadFileName != null, "download file name can not be empty or null");

            IWorkbook book = BuildHSSFWorkbook(tables, columnHeader, headerCellStyleFormater, dataCellStyleFormater);

            string ext = Path.GetExtension(downloadFileName.Trim());

            if (ext.ToLower() == ".xls" || ext.ToLower() == ".xlsx")
                downloadFileName = downloadFileName.Replace(ext, string.Empty);


            HttpResponse httpResponse = HttpKits.CurrentContext.Response;

            httpResponse.Clear();
            httpResponse.Charset = Encoding.UTF8.BodyName;
            httpResponse.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(downloadFileName, Encoding.UTF8) + ".xls");
            httpResponse.ContentEncoding = Encoding.UTF8;
            httpResponse.ContentType = ContentTypes.Excel;
            book.Write(httpResponse.OutputStream);
            httpResponse.End();
        }

        /// <summary>
        /// Exports to file.
        /// </summary>
        /// <param name="table">The export data table.</param>
        /// <param name="exportFilePath">The export XLS file full path.</param>
        /// <param name="columnHeader">if set to <c>true</c> will set column name as header.</param>
        /// <param name="headerCellStyleFormater">The header cell style formater.</param>
        /// <param name="dataCellStyleFormater">The data cell style formater.</param>
        public static void ExportToFile(DataTable table, string exportFilePath, bool columnHeader = true, Func<ICell, ICellStyle> headerCellStyleFormater = null, Func<ICell, ICellStyle> dataCellStyleFormater = null)
        {
            ExportToFile(new DataTable[] { table }, exportFilePath, columnHeader, headerCellStyleFormater, dataCellStyleFormater);
        }

        /// <summary>
        /// Exports to file.
        /// </summary>
        /// <param name="dataSet">The export data set.</param>
        /// <param name="exportFilePath">The export XLS file full path.</param>
        /// <param name="columnHeader">if set to <c>true</c> will set column name as header.</param>
        /// <param name="headerCellStyleFormater">The header cell style formater.</param>
        /// <param name="dataCellStyleFormater">The data cell style formater.</param>
        public static void ExportToFile(DataSet dataSet, string exportFilePath, bool columnHeader = true, Func<ICell, ICellStyle> headerCellStyleFormater = null, Func<ICell, ICellStyle> dataCellStyleFormater = null)
        {
            Checker.Parameter(dataSet != null, "export data set can not be null");
            DataTable[] tables = new DataTable[dataSet.Tables.Count];
            dataSet.Tables.CopyTo(tables, 0);

            ExportToFile(tables, exportFilePath, columnHeader, headerCellStyleFormater, dataCellStyleFormater);
        }

        /// <summary>
        /// Exports to file.
        /// </summary>
        /// <param name="tables">The export data tables.</param>
        /// <param name="exportFilePath">The export XLS file full path.</param>
        /// <param name="columnHeader">if set to <c>true</c> will set column name as header.</param>
        /// <param name="headerCellStyleFormater">The header cell style formater.</param>
        /// <param name="dataCellStyleFormater">The data cell style formater.</param>
        public static void ExportToFile(IEnumerable<DataTable> tables, string exportFilePath, bool columnHeader = true, Func<ICell, ICellStyle> headerCellStyleFormater = null, Func<ICell, ICellStyle> dataCellStyleFormater = null)
        {
            Checker.Parameter(exportFilePath != null, "export file path can not be empty or null");

            IWorkbook book = BuildHSSFWorkbook(tables, columnHeader, headerCellStyleFormater, dataCellStyleFormater);

            string ext = Path.GetExtension(exportFilePath.Trim());

            if (ext.ToLower() == ".xls" || ext.ToLower() == ".xlsx")
                exportFilePath = exportFilePath.Replace(ext, string.Empty);

            exportFilePath += ".xls";

            using (FileStream fs = new FileStream(exportFilePath, FileMode.Create))
            {
                book.Write(fs);
            }
        }

        /// <summary>
        /// Builds the workbook.
        /// </summary>
        /// <param name="tables">The export data tables.</param>
        /// <param name="columnHeader">if set to <c>true</c> will set column name as header.</param>
        /// <param name="headerCellStyleFormater">The header cell style formater.</param>
        /// <param name="dataCellStyleFormater">The data cell style formater.</param>
        /// <returns></returns>
        private static HSSFWorkbook BuildHSSFWorkbook(IEnumerable<DataTable> tables, bool columnHeader, Func<ICell, ICellStyle> headerCellStyleFormater, Func<ICell, ICellStyle> dataCellStyleFormater)
        {
            Checker.Parameter(tables != null || tables.Count() > 0 || tables.All(o => o != null), "export data tables can not be empty or contains null value");

            HSSFWorkbook book = new HSSFWorkbook();

            for (int t = 0; t < tables.Count(); t++)
            {
                DataTable table = tables.ElementAt(t);

                ISheet sheet = book.CreateSheet(string.IsNullOrWhiteSpace(table.TableName) ? "Sheet" + (t + 1) : table.TableName);

                int firstRowNum = 0;

                if (columnHeader)
                {
                    //header row
                    IRow row0 = sheet.CreateRow(0);
                    for (int i = 0; i < table.Columns.Count; i++)
                    {
                        ICell cell = row0.CreateCell(i, CellType.String);
                        cell.SetCellValue(table.Columns[i].ColumnName);
                        if (headerCellStyleFormater != null)
                        {
                            var style = headerCellStyleFormater(cell);
                            if (style != null)
                                cell.CellStyle = style;
                        }
                    }

                    firstRowNum = 1;
                }

                //Data Rows
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    IRow drow = sheet.CreateRow(i + firstRowNum);
                    for (int j = 0; j < table.Columns.Count; j++)
                    {
                        ICell cell = drow.CreateCell(j, CellType.String);
                        cell.SetCellValue(table.Rows[i][j].ToString());
                        if (dataCellStyleFormater != null)
                        {
                            var style = dataCellStyleFormater(cell);
                            if (style != null)
                                cell.CellStyle = style;
                        }
                    }
                }
            }

            return book;
        }

        /// <summary>
        /// Imports to data table.
        /// </summary>
        /// <param name="excelFilePath">The excel file path.</param>
        /// <param name="sheetIndex">The zero-based index of the sheet.</param>
        /// <param name="firstRowHeader">if set to <c>true</c> [first row header].</param>
        /// <param name="cellValueInterpreter">The cell value interpreter.</param>
        /// <returns></returns>
        public static DataTable ImportToDataTable(string excelFilePath, int sheetIndex, bool firstRowHeader = true, Func<ICell, object> cellValueInterpreter = null)
        {
            if (!File.Exists(excelFilePath))
                throw new FileNotFoundException(excelFilePath);

            IWorkbook workbook = WorkbookFactory.Create(excelFilePath);
            ISheet sheet = workbook.GetSheetAt(sheetIndex);

            return BuildDataTableFromSheet(sheet, firstRowHeader, cellValueInterpreter);
        }

        /// <summary>
        /// Imports to data table.
        /// </summary>
        /// <param name="excelFilePath">The excel file path.</param>
        /// <param name="sheetName">Name of the sheet.</param>
        /// <param name="firstRowHeader">if set to <c>true</c> [first row header].</param>
        /// <param name="cellValueInterpreter">The cell value interpreter.</param>
        /// <returns></returns>
        public static DataTable ImportToDataTable(string excelFilePath, string sheetName, bool firstRowHeader = true, Func<ICell, object> cellValueInterpreter = null)
        {
            if (!File.Exists(excelFilePath))
                throw new FileNotFoundException(excelFilePath);

            IWorkbook workbook = WorkbookFactory.Create(excelFilePath);
            ISheet sheet = workbook.GetSheet(sheetName);

            return BuildDataTableFromSheet(sheet, firstRowHeader, cellValueInterpreter);
        }

        /// <summary>
        /// Imports to data table.
        /// </summary>
        /// <param name="excelStream">The excel stream.</param>
        /// <param name="sheetIndex">The zero-based index of the sheet.</param>
        /// <param name="firstRowHeader">if set to <c>true</c> [first row header].</param>
        /// <param name="cellValueInterpreter">The cell value interpreter.</param>
        /// <returns></returns>
        public static DataTable ImportToDataTable(Stream excelStream, int sheetIndex, bool firstRowHeader = true, Func<ICell, object> cellValueInterpreter = null)
        {
            IWorkbook workbook = WorkbookFactory.Create(excelStream);
            ISheet sheet = workbook.GetSheetAt(sheetIndex);

            return BuildDataTableFromSheet(sheet, firstRowHeader, cellValueInterpreter);
        }

        /// <summary>
        /// Imports to data table.
        /// </summary>
        /// <param name="excelStream">The excel stream.</param>
        /// <param name="sheetName">Name of the sheet.</param>
        /// <param name="firstRowHeader">if set to <c>true</c> [first row header].</param>
        /// <param name="cellValueInterpreter">The cell value interpreter.</param>
        /// <returns></returns>
        public static DataTable ImportToDataTable(Stream excelStream, string sheetName, bool firstRowHeader = true, Func<ICell, object> cellValueInterpreter = null)
        {
            IWorkbook workbook = WorkbookFactory.Create(excelStream);
            ISheet sheet = workbook.GetSheet(sheetName);

            return BuildDataTableFromSheet(sheet, firstRowHeader, cellValueInterpreter);
        }


        /// <summary>
        /// Imports to data set.
        /// </summary>
        /// <param name="excelStream">The excel stream.</param>
        /// <param name="firstRowHeader">if set to <c>true</c> [first row header].</param>
        /// <param name="cellValueInterpreter">The cell value interpreter.</param>
        /// <returns></returns>
        public static DataSet ImportToDataSet(Stream excelStream,  bool firstRowHeader = true, Func<ICell, object> cellValueInterpreter = null)
        {
            IWorkbook workbook = WorkbookFactory.Create(excelStream);

            DataSet ds = new DataSet();

            for (int i = 0; i < workbook.NumberOfSheets; i++)
            {
                ds.Tables.Add(BuildDataTableFromSheet(workbook.GetSheetAt(i), firstRowHeader, cellValueInterpreter));
            }

            return ds;
        }

        /// <summary>
        /// Imports to data set.
        /// </summary>
        /// <param name="excelFilePath">The excel file path.</param>
        /// <param name="firstRowHeader">if set to <c>true</c> [first row header].</param>
        /// <param name="cellValueInterpreter">The cell value interpreter.</param>
        /// <returns></returns>
        public static DataSet ImportToDataTable(string excelFilePath, bool firstRowHeader = true, Func<ICell, object> cellValueInterpreter = null)
        {
            if (!File.Exists(excelFilePath))
                throw new FileNotFoundException(excelFilePath);

            IWorkbook workbook = WorkbookFactory.Create(excelFilePath);

            DataSet ds = new DataSet();

            for (int i = 0; i < workbook.NumberOfSheets; i++)
            {
                ds.Tables.Add(BuildDataTableFromSheet(workbook.GetSheetAt(i), firstRowHeader, cellValueInterpreter));
            }

            return ds;
        }


        /// <summary>
        /// Builds the data table from sheet.
        /// </summary>
        /// <param name="sheet">The sheet.</param>
        /// <param name="firstRowHeader">if set to <c>true</c> [first row header].</param>
        /// <param name="cellValueInterpreter">The cell value interpreter.</param>
        /// <returns></returns>
        private static DataTable BuildDataTableFromSheet(ISheet sheet, bool firstRowHeader, Func<ICell, object> cellValueInterpreter)
        {
            DataTable table = new DataTable();

            //空表格
            if (sheet.LastRowNum == 0)
                return table;

            //最大的有效单元格编号
            int maxCellsNum = 0;

            for (int i = sheet.FirstRowNum; i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);

                if (row != null)
                {
                    foreach (var c in row.Cells)
                    {
                        //跳过空列
                        if (!string.IsNullOrWhiteSpace(c.ToString()) && c.ColumnIndex + 1 > maxCellsNum)
                            maxCellsNum = c.ColumnIndex;
                    }
                }
            }


            //构建表头
            IRow firstRow = sheet.GetRow(sheet.FirstRowNum);

            IDictionary<string, int> sameColumns = new Dictionary<string, int>();

            for (int i = 0; i <= maxCellsNum; i++)
            {
                if (!firstRowHeader)
                {
                    table.Columns.Add(new DataColumn(i.ToString()));
                    continue;
                }

                ICell hc = firstRow.GetCell(i);

                if (hc == null)
                {
                    table.Columns.Add(new DataColumn(i.ToString()));
                    continue;
                }

                //检查重名
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


            //读取数据行
            int dataRowNum = firstRowHeader ? sheet.FirstRowNum + 1 : sheet.FirstRowNum;//数据行开始编号

            for (int i = dataRowNum; i <= sheet.LastRowNum; i++)
            {
                DataRow dataRow = table.NewRow();

                IRow row = sheet.GetRow(i);

                if (row == null)
                {
                    //添加空行，后面再删除空行
                    table.Rows.Add(dataRow);
                    continue;
                }

                for (int j = 0; j <= maxCellsNum; j++)
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
                                    if (DateUtil.IsCellDateFormatted(cell))
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


            //删除头尾的空行
            while (true)
            {
                if (table.Rows[0].ItemArray.All(o => o == null || string.IsNullOrWhiteSpace(o.ToString())))
                {
                    table.Rows.RemoveAt(0);
                    continue;
                }

                if (table.Rows[table.Rows.Count - 1].ItemArray.All(o => o == null || string.IsNullOrWhiteSpace(o.ToString())))
                {
                    table.Rows.RemoveAt(table.Rows.Count - 1);
                    continue;
                }

                break;
            }

            return table;
        }
    }
}
