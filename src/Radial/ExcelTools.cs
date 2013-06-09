﻿using System;
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
        /// The known cell data formats, separated by commas.
        /// </summary>
        public const string KnownCellDataFormats = "14,15,16,17,18,19,20,21,22,30,31,32,33,45,46,47,49,55,56,57,58,176,177,"
            + "178,179,180,181,182,183,184,185,186,187,188,189,190,191,192,193,194,195,196,197,198,199,200,201,202,203,204,205,206,"
            + "207,208,209";


        /// <summary>
        /// Exports to HTTP stream.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="fileName"> The download file name.</param>
        public static void ExportToHttp(DataTable data, string fileName)
        {
            Checker.Parameter(fileName != null, "download file name can not be empty or null");

            fileName = fileName.Trim();

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
        /// <param name="skipEmptyRow">if set to <c>true</c> [skip empty row].</param>
        /// <returns></returns>
        public static DataTable ImportToDataTable(string excelFilePath, int sheetIndex, Func<ICell, object> cellValueInterpreter = null, bool firstRowHeader = true, bool skipEmptyRow = false)
        {
            IWorkbook workbook = WorkbookFactory.Create(excelFilePath);
            ISheet sheet = workbook.GetSheetAt(sheetIndex);

            return BuildDataTableFromSheet(sheet, cellValueInterpreter, firstRowHeader, skipEmptyRow);
        }

        /// <summary>
        /// Imports to data table.
        /// </summary>
        /// <param name="excelFilePath">The excel file path.</param>
        /// <param name="sheetName">Name of the sheet.</param>
        /// <param name="cellValueInterpreter">The cell value interpreter.</param>
        /// <param name="firstRowHeader">if set to <c>true</c> [first row header].</param>
        /// <param name="skipEmptyRow">if set to <c>true</c> [skip empty row].</param>
        /// <returns></returns>
        public static DataTable ImportToDataTable(string excelFilePath, string sheetName, Func<ICell, object> cellValueInterpreter = null, bool firstRowHeader = true, bool skipEmptyRow = false)
        {
            IWorkbook workbook = WorkbookFactory.Create(excelFilePath);
            ISheet sheet = workbook.GetSheet(sheetName);

            return BuildDataTableFromSheet(sheet, cellValueInterpreter, firstRowHeader, skipEmptyRow);
        }

        /// <summary>
        /// Imports to data table.
        /// </summary>
        /// <param name="excelStream">The excel stream.</param>
        /// <param name="sheetIndex">The zero-based index of the sheet.</param>
        /// <param name="cellValueInterpreter">The cell value interpreter.</param>
        /// <param name="firstRowHeader">if set to <c>true</c> [first row header].</param>
        /// <param name="skipEmptyRow">if set to <c>true</c> [skip empty row].</param>
        /// <returns></returns>
        public static DataTable ImportToDataTable(Stream excelStream, int sheetIndex, Func<ICell, object> cellValueInterpreter = null, bool firstRowHeader = true, bool skipEmptyRow = false)
        {
            IWorkbook workbook = WorkbookFactory.Create(excelStream);
            ISheet sheet = workbook.GetSheetAt(sheetIndex);

            return BuildDataTableFromSheet(sheet, cellValueInterpreter, firstRowHeader, skipEmptyRow);
        }

        /// <summary>
        /// Imports to data table.
        /// </summary>
        /// <param name="excelStream">The excel stream.</param>
        /// <param name="sheetName">Name of the sheet.</param>
        /// <param name="cellValueInterpreter">The cell value interpreter.</param>
        /// <param name="firstRowHeader">if set to <c>true</c> [first row header].</param>
        /// <param name="skipEmptyRow">if set to <c>true</c> [skip empty row].</param>
        /// <returns></returns>
        public static DataTable ImportToDataTable(Stream excelStream, string sheetName, Func<ICell, object> cellValueInterpreter = null, bool firstRowHeader = true, bool skipEmptyRow = false)
        {
            IWorkbook workbook = WorkbookFactory.Create(excelStream);
            ISheet sheet = workbook.GetSheet(sheetName);

            return BuildDataTableFromSheet(sheet, cellValueInterpreter, firstRowHeader, skipEmptyRow);
        }

        /// <summary>
        /// Builds the data table from sheet.
        /// </summary>
        /// <param name="sheet">The sheet.</param>
        /// <param name="cellValueInterpreter">The cell value interpreter.</param>
        /// <param name="firstRowHeader">if set to <c>true</c> [first row header].</param>
        /// <param name="skipEmptyRow">if set to <c>true</c> [skip empty row].</param>
        /// <returns></returns>
        private static DataTable BuildDataTableFromSheet(ISheet sheet, Func<ICell, object> cellValueInterpreter, bool firstRowHeader, bool skipEmptyRow)
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
                                    //区分BuiltIn的数字和时间
                                    if (KnownCellDataFormats.Split(',').Contains(cell.CellStyle.DataFormat.ToString()))
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

            //删除空行
            if (skipEmptyRow)
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    if (table.Rows[i].ItemArray.All(o => o == null || string.IsNullOrWhiteSpace(o.ToString())))
                    {
                        table.Rows.RemoveAt(i);
                        i--;
                    }
                }
            }

            return table;
        }
    }
}
