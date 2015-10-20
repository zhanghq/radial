using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using OfficeOpenXml;
using Radial.Net;
using Radial.Web;

namespace Radial
{
    /// <summary>
    /// Excel tools
    /// </summary>
    public static class ExcelTools
    {
        #region Export

        /// <summary>
        /// Exports to HTTP stream.
        /// </summary>
        /// <param name="dataTables">The export data tables.</param>
        /// <param name="downloadFileName">The download file name, use random name if set to null.</param>
        /// <param name="columnHeader">if set to <c>true</c>, column name will used as caption property on first row.</param>
        public static void ExportToHttp(IEnumerable<DataTable> dataTables, string downloadFileName = null, bool columnHeader = true)
        {
            ExportToHttp(dataTables, downloadFileName, columnHeader, null);
        }


        /// <summary>
        /// Exports to HTTP stream.
        /// </summary>
        /// <param name="dataTables">The export data tables.</param>
        /// <param name="downloadFileName">The download file name, use random name if set to null.</param>
        /// <param name="columnHeader">if set to <c>true</c>, column name will used as caption property on first row.</param>
        /// <param name="customHandler">The custom handler.</param>
        public static void ExportToHttp(IEnumerable<DataTable> dataTables, string downloadFileName, bool columnHeader, Action<ExcelWorksheet> customHandler)
        {
            if (dataTables == null || dataTables.Count() == 0 || !HttpKits.IsWebApp)
                return;

            if (!string.IsNullOrWhiteSpace(downloadFileName))
                downloadFileName = HttpUtility.UrlEncode(Path.GetFileNameWithoutExtension(downloadFileName), GlobalVariables.Encoding) + ".xlsx";
            else
                downloadFileName = Path.GetRandomFileName().Replace(".", string.Empty) + ".xlsx";

            using (ExcelPackage pck = new ExcelPackage())
            {
                for (int i = 0; i < dataTables.Count(); i++)
                {
                    DataTable table = dataTables.ElementAt(i);

                    if (table == null)
                        continue;

                    FillWorkbook(pck, table, i, columnHeader, customHandler);
                }

                HttpResponse httpResponse = HttpKits.CurrentContext.Response;
                httpResponse.Clear();
                httpResponse.Charset = GlobalVariables.Encoding.BodyName;
                httpResponse.AppendHeader("Content-Disposition", "attachment;filename=" + downloadFileName);
                httpResponse.ContentEncoding = GlobalVariables.Encoding;
                httpResponse.ContentType = ContentTypes.Excel;
                httpResponse.BinaryWrite(pck.GetAsByteArray());
                httpResponse.End();
            }
        }

        /// <summary>
        /// Exports to HTTP stream.
        /// </summary>
        /// <param name="dataTable">The export data table.</param>
        /// <param name="downloadFileName">The download file name, use random name if set to null.</param>
        /// <param name="columnHeader">if set to <c>true</c>, column name will used as caption property on first row.</param>
        public static void ExportToHttp(DataTable dataTable, string downloadFileName = null, bool columnHeader = true)
        {
            ExportToHttp(dataTable, downloadFileName, columnHeader, null);
        }

        /// <summary>
        /// Exports to HTTP stream.
        /// </summary>
        /// <param name="dataTable">The export data table.</param>
        /// <param name="downloadFileName">The download file name, use random name if set to null.</param>
        /// <param name="columnHeader">if set to <c>true</c>, column name will used as caption property on first row.</param>
        /// <param name="customHandler">The custom handler.</param>
        public static void ExportToHttp(DataTable dataTable, string downloadFileName, bool columnHeader, Action<ExcelWorksheet> customHandler)
        {
            ExportToHttp(new DataTable[] { dataTable }, downloadFileName, columnHeader, customHandler);
        }

        /// <summary>
        /// Exports to HTTP stream.
        /// </summary>
        /// <param name="dataSet">The export data set.</param>
        /// <param name="downloadFileName">The download file name, use random name if set to null.</param>
        /// <param name="columnHeader">if set to <c>true</c>, column name will used as caption property on first row.</param>
        /// <param name="customHandler">The custom handler.</param>
        public static void ExportToHttp(DataSet dataSet, string downloadFileName, bool columnHeader, Action<ExcelWorksheet> customHandler)
        {
            if (dataSet == null || !HttpKits.IsWebApp)
                return;

            if (!string.IsNullOrWhiteSpace(downloadFileName))
                downloadFileName = HttpUtility.UrlEncode(Path.GetFileNameWithoutExtension(downloadFileName), GlobalVariables.Encoding) + ".xlsx";
            else
                downloadFileName = Path.GetRandomFileName().Replace(".", string.Empty) + ".xlsx";

            using (ExcelPackage pck = new ExcelPackage())
            {
                for (int i = 0; i < dataSet.Tables.Count; i++)
                {
                    DataTable table = dataSet.Tables[i];

                    if (table == null)
                        continue;

                    FillWorkbook(pck, table, i, columnHeader, customHandler);
                }

                HttpResponse httpResponse = HttpKits.CurrentContext.Response;
                httpResponse.Clear();
                httpResponse.Charset = GlobalVariables.Encoding.BodyName;
                httpResponse.AppendHeader("Content-Disposition", "attachment;filename=" + downloadFileName);
                httpResponse.ContentEncoding = GlobalVariables.Encoding;
                httpResponse.ContentType = ContentTypes.Excel;
                httpResponse.BinaryWrite(pck.GetAsByteArray());
                httpResponse.End();
            }
        }

        /// <summary>
        /// Exports to HTTP stream.
        /// </summary>
        /// <param name="dataSet">The export data set.</param>
        /// <param name="downloadFileName">The download file name, use random name if set to null.</param>
        /// <param name="columnHeader">if set to <c>true</c>, column name will used as caption property on first row.</param>
        public static void ExportToHttp(DataSet dataSet, string downloadFileName = null, bool columnHeader = true)
        {
            ExportToHttp(dataSet, downloadFileName, columnHeader, null);
        }

        /// <summary>
        /// Exports to file.
        /// </summary>
        /// <param name="dataTables">The export data tables.</param>
        /// <param name="excelFilePath">The excel file path, use random path if set to null.</param>
        /// <param name="columnHeader">if set to <c>true</c>, column name will used as caption property on first row.</param>
        public static void ExportToFile(IEnumerable<DataTable> dataTables, string excelFilePath = null, bool columnHeader = true)
        {
            ExportToFile(dataTables, excelFilePath, columnHeader, null);
        }

        /// <summary>
        /// Exports to file.
        /// </summary>
        /// <param name="dataTables">The export data tables.</param>
        /// <param name="excelFilePath">The excel file path, use random path if set to null.</param>
        /// <param name="columnHeader">if set to <c>true</c>, column name will used as caption property on first row.</param>
        /// <param name="customHandler">The custom handler.</param>
        public static void ExportToFile(IEnumerable<DataTable> dataTables, string excelFilePath, bool columnHeader, 
            Action<ExcelWorksheet> customHandler)
        {
            if (dataTables == null || dataTables.Count() == 0)
                return;

            if (!string.IsNullOrWhiteSpace(excelFilePath))
                excelFilePath = Path.Combine(Path.GetDirectoryName(excelFilePath), Path.GetFileNameWithoutExtension(excelFilePath) + ".xlsx");
            else
                excelFilePath = Path.GetRandomFileName().Replace(".", string.Empty) + ".xlsx";

            using (ExcelPackage pck = new ExcelPackage())
            {
                for (int i = 0; i < dataTables.Count(); i++)
                {
                    DataTable table = dataTables.ElementAt(i);

                    if (table == null)
                        continue;

                    FillWorkbook(pck, table, i, columnHeader, customHandler);
                }

                pck.SaveAs(new FileInfo(excelFilePath));
            }
        }

        /// <summary>
        /// Exports to file.
        /// </summary>
        /// <param name="dataTable">The export data table.</param>
        /// <param name="excelFilePath">The excel file path, use random path if set to null.</param>
        /// <param name="columnHeader">if set to <c>true</c>, column name will used as caption property on first row.</param>
        /// <param name="customHandler">The custom handler.</param>
        public static void ExportToFile(DataTable dataTable, string excelFilePath, bool columnHeader, Action<ExcelWorksheet> customHandler)
        {
            ExportToFile(new DataTable[] { dataTable }, excelFilePath, columnHeader, customHandler);
        }

        /// <summary>
        /// Exports to file.
        /// </summary>
        /// <param name="dataTable">The export data table.</param>
        /// <param name="excelFilePath">The excel file path, use random path if set to null.</param>
        /// <param name="columnHeader">if set to <c>true</c>, column name will used as caption property on first row.</param>
        public static void ExportToFile(DataTable dataTable, string excelFilePath = null, bool columnHeader = true)
        {
            ExportToFile(dataTable, excelFilePath, columnHeader, null);
        }

        /// <summary>
        /// Exports to file.
        /// </summary>
        /// <param name="dataSet">The export data set.</param>
        /// <param name="excelFilePath">The excel file path, use random path if set to null.</param>
        /// <param name="columnHeader">if set to <c>true</c>, column name will used as caption property on first row.</param>
        public static void ExportToFile(DataSet dataSet, string excelFilePath = null, bool columnHeader = true)
        {
            ExportToFile(dataSet, excelFilePath, columnHeader, null);
        }


        /// <summary>
        /// Exports to file.
        /// </summary>
        /// <param name="dataSet">The export data set.</param>
        /// <param name="excelFilePath">The excel file path, use random path if set to null.</param>
        /// <param name="columnHeader">if set to <c>true</c>, column name will used as caption property on first row.</param>
        /// <param name="customHandler">The custom handler.</param>
        public static void ExportToFile(DataSet dataSet, string excelFilePath, bool columnHeader, Action<ExcelWorksheet> customHandler)
        {
            if (dataSet == null)
                return;

            if (!string.IsNullOrWhiteSpace(excelFilePath))
                excelFilePath = Path.Combine(Path.GetDirectoryName(excelFilePath), Path.GetFileNameWithoutExtension(excelFilePath) + ".xlsx");
            else
                excelFilePath = Path.GetRandomFileName().Replace(".", string.Empty) + ".xlsx";

            using (ExcelPackage pck = new ExcelPackage())
            {
                for (int i = 0; i < dataSet.Tables.Count; i++)
                {
                    DataTable table = dataSet.Tables[i];

                    if (table == null)
                        continue;

                    FillWorkbook(pck, table, i, columnHeader, customHandler);
                }

                pck.SaveAs(new FileInfo(excelFilePath));
            }
        }

        /// <summary>
        /// Fills the workbook.
        /// </summary>
        /// <param name="pck">The PCK.</param>
        /// <param name="table">The table.</param>
        /// <param name="sheetIndex">Index of the sheet (zero-base).</param>
        /// <param name="columnHeader">if set to <c>true</c>, column name will used as caption property on first row.</param>
        /// <param name="customHandler">The custom handler.</param>
        private static void FillWorkbook(ExcelPackage pck, DataTable table, int sheetIndex, bool columnHeader, Action<ExcelWorksheet> customHandler)
        {
            if (sheetIndex < 0)
                sheetIndex = 0;

            ExcelWorksheet sheet = pck.Workbook.Worksheets.Add(string.IsNullOrWhiteSpace(table.TableName) ? "Sheet" + (sheetIndex + 1) : table.TableName);

            if (table.Rows.Count > 0)
                sheet.Cells["A1"].LoadFromDataTable(table, columnHeader);

            foreach (var cell in sheet.Cells)
                cell.AutoFitColumns();

            if (customHandler != null)
                customHandler(sheet);
        }

        #endregion

        #region Import

        /// <summary>
        /// Imports to data table.
        /// </summary>
        /// <param name="excelFilePath">The excel file path.</param>
        /// <param name="sheetName">Name of the sheet.</param>
        /// <param name="firstRowHeader">if set to <c>true</c> [first row header].</param>
        /// <returns></returns>
        public static DataTable ImportToDataTable(string excelFilePath, string sheetName, bool firstRowHeader = true)
        {
            using (FileStream fs = File.OpenRead(excelFilePath))
            {
                return ImportToDataTable(fs, sheetName, firstRowHeader);
            }
        }

        /// <summary>
        /// Imports to data table.
        /// </summary>
        /// <param name="excelFilePath">The excel file path.</param>
        /// <param name="sheetIndex">Index of the sheet (zero-base).</param>
        /// <param name="firstRowHeader">if set to <c>true</c> [first row header].</param>
        /// <returns></returns>
        public static DataTable ImportToDataTable(string excelFilePath, int sheetIndex = 0, bool firstRowHeader = true)
        {
            using (FileStream fs = File.OpenRead(excelFilePath))
            {
                return ImportToDataTable(fs, sheetIndex, firstRowHeader);
            }
        }

        /// <summary>
        /// Imports to data table.
        /// </summary>
        /// <param name="excelStream">The excel stream.</param>
        /// <param name="sheetName">Name of the sheet.</param>
        /// <param name="firstRowHeader">if set to <c>true</c> [first row header].</param>
        /// <returns></returns>
        public static DataTable ImportToDataTable(Stream excelStream, string sheetName, bool firstRowHeader = true)
        {
            using (ExcelPackage pck = new ExcelPackage())
            {
                pck.Load(excelStream);

                var sheet = pck.Workbook.Worksheets[sheetName];

                if (sheet != null && sheet.Dimension != null)
                    return CreateDataTable(sheet, firstRowHeader);
            }

            return null;
        }

        /// <summary>
        /// Imports to data table.
        /// </summary>
        /// <param name="excelStream">The excel stream.</param>
        /// <param name="sheetIndex">Index of the sheet (zero-base).</param>
        /// <param name="firstRowHeader">if set to <c>true</c> [first row header].</param>
        /// <returns></returns>
        public static DataTable ImportToDataTable(Stream excelStream, int sheetIndex = 0, bool firstRowHeader = true)
        {

            using (ExcelPackage pck = new ExcelPackage())
            {
                pck.Load(excelStream);

                var sheet = pck.Workbook.Worksheets[sheetIndex + 1];

                if (sheet != null && sheet.Dimension != null)
                    return CreateDataTable(sheet, firstRowHeader);
            }

            return null;
        }

        /// <summary>
        /// Imports to data tables.
        /// </summary>
        /// <param name="excelFilePath">The excel file path.</param>
        /// <param name="firstRowHeader">if set to <c>true</c> [first row header].</param>
        /// <returns></returns>
        public static IEnumerable<DataTable> ImportToDataTables(string excelFilePath, bool firstRowHeader = true)
        {
            using (FileStream fs = File.OpenRead(excelFilePath))
            {
                return ImportToDataTables(fs, firstRowHeader);
            }
        }

        /// <summary>
        /// Imports to data tables.
        /// </summary>
        /// <param name="excelStream">The excel stream.</param>
        /// <param name="firstRowHeader">if set to <c>true</c> [first row header].</param>
        /// <returns></returns>
        public static IEnumerable<DataTable> ImportToDataTables(Stream excelStream, bool firstRowHeader = true)
        {
            IList<DataTable> tables = new List<DataTable>();

            using (ExcelPackage pck = new ExcelPackage())
            {
                pck.Load(excelStream);

                for (int i = 0; i < pck.Workbook.Worksheets.Count; i++)
                {
                    var sheet = pck.Workbook.Worksheets[i + 1];

                    if (sheet == null || sheet.Dimension == null)
                        continue;

                    tables.Add(CreateDataTable(sheet, firstRowHeader));
                }
            }

            return tables;
        }

        /// <summary>
        /// Imports to data set.
        /// </summary>
        /// <param name="excelFilePath">The excel file path.</param>
        /// <param name="firstRowHeader">if set to <c>true</c> [first row header].</param>
        /// <returns></returns>
        public static DataSet ImportToDataSet(string excelFilePath, bool firstRowHeader = true)
        {
            using (FileStream fs = File.OpenRead(excelFilePath))
            {
                return ImportToDataSet(fs, firstRowHeader);
            }
        }

        /// <summary>
        /// Imports to data set.
        /// </summary>
        /// <param name="excelStream">The excel stream.</param>
        /// <param name="firstRowHeader">if set to <c>true</c> [first row header].</param>
        /// <returns></returns>
        public static DataSet ImportToDataSet(Stream excelStream, bool firstRowHeader = true)
        {
            DataSet ds = new DataSet();

            using (ExcelPackage pck = new ExcelPackage())
            {
                pck.Load(excelStream);

                for (int i = 0; i < pck.Workbook.Worksheets.Count; i++)
                {
                    var sheet = pck.Workbook.Worksheets[i + 1];

                    if (sheet == null || sheet.Dimension == null)
                        continue;

                    ds.Tables.Add(CreateDataTable(sheet, firstRowHeader));
                }
            }

            return ds;
        }

        /// <summary>
        /// Creates the data table.
        /// </summary>
        /// <param name="sheet">The sheet.</param>
        /// <param name="firstRowHeader">if set to <c>true</c> [first row header].</param>
        /// <returns></returns>
        private static DataTable CreateDataTable(ExcelWorksheet sheet, bool firstRowHeader)
        {
            DataTable table = new DataTable(string.IsNullOrWhiteSpace(sheet.Name) ? "Sheet" + sheet.Index : sheet.Name);

            IDictionary<int, IList<ExcelRangeBase>> excelRows = new Dictionary<int, IList<ExcelRangeBase>>();

            foreach (var cell in sheet.Cells)
            {
                if (!excelRows.ContainsKey(cell.Start.Row))
                    excelRows.Add(cell.Start.Row, new List<ExcelRangeBase>());

                //fill count
                int fcount = cell.Start.Column - excelRows[cell.Start.Row].Count;

                for (int f = 0; f < fcount; f++)
                    excelRows[cell.Start.Row].Add(null);

                excelRows[cell.Start.Row][cell.Start.Column - 1] = cell;
            }

            //remove last empty row
            while (true)
            {
                var t = excelRows.ElementAt(excelRows.Count - 1);

                if (t.Value.Any(o => o != null && !string.IsNullOrWhiteSpace(o.Text)))
                    break;

                excelRows.Remove(t.Key);
            }

            if (excelRows.Count > 0)
            {
                //find total column
                int totalColumn = 0;

                for (int c = 0; c < excelRows.Count; c++)
                {
                    var er = excelRows.ElementAt(c);

                    var lastNotNull = er.Value.LastOrDefault(o => o != null && !string.IsNullOrWhiteSpace(o.Text));

                    if (lastNotNull == null)
                        continue;

                    var lastNotNullIndex = er.Value.IndexOf(lastNotNull);

                    if (lastNotNullIndex + 1 > totalColumn)
                        totalColumn = lastNotNullIndex + 1;
                }

                //Add column
                var firstRowCellRanges = excelRows.ElementAt(0);

                //for ckeck duplication name
                HashSet<string> columnNames = new HashSet<string>();

                for (int c = 0; c < totalColumn; c++)
                {
                    string columnName = string.Empty;

                    if (c < firstRowCellRanges.Value.Count)
                    {
                        ExcelRangeBase cell = firstRowCellRanges.Value[c];

                        if (firstRowHeader)
                        {
                            columnName = cell.Text;

                            //ckeck duplication name
                            if (columnNames.Contains(columnName))
                                columnName += "[" + cell.Address + "]";
                            else
                                columnNames.Add(columnName);
                        }
                        else
                            columnName = cell.Address;
                    }
                    else
                        columnName = sheet.Cells[firstRowCellRanges.Key, sheet.Dimension.Start.Column + c].Address;



                    table.Columns.Add(columnName);
                }

                //add data row
                for (int c = firstRowHeader ? 1 : 0; c < excelRows.Count; c++)
                {
                    var er = excelRows.ElementAt(c);

                    IList<string> drowObjs = new List<string>();
                    for (int x = 0; x < totalColumn; x++)
                    {
                        if (x < er.Value.Count && er.Value[x] != null)
                            drowObjs.Add(er.Value[x].Text);
                        else
                            drowObjs.Add(null);
                    }

                    table.Rows.Add(drowObjs.ToArray());
                }
            }

            return table;
        }

        #endregion
    }
}
