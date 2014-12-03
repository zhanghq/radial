using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Radial.UnitTest
{
    [TestFixture]
    public class ExcelToolsTest
    {
        [Test]
        public void ImportToDataTable()
        {
            DataTable table = ExcelTools.ImportToDataTable("demo.xlsx", 0, false);
            Assert.AreEqual(table.Columns.Count, 4);
            Assert.AreEqual(table.Rows.Count, 12);
        }

        [Test]
        public void ExportToFile()
        {
            DataTable table = new DataTable();

            for (int i = 0; i < 6; i++)
                table.Columns.Add(i.ToString());

            table.Rows.Add("CodeCodeCodeCodeCodeCodeCodeCodeCode", "BC00023");

            table.Rows.Add(new object[] { });

            table.Rows.Add("Name", "A1", "A2", "A3", "A4", "A5");
            table.Rows.Add("test", "测试的方式发生地方我认为人反感的风格时光飞逝的歌爱上对方是乏味让我发的发二恶烷", "45%", "$19.4", "Fd09", DateTime.Now);
            table.Rows.Add("test2", "23.4", "45%", "$19.4", "Fd09", DateTime.Now);
            table.Rows.Add("test3", "23.4", "45%", "$19.4", "Fd09", DateTime.Now);


            DataTable table2 = table.Copy();
            table2.TableName = "Test2";

            DataSet ds = new DataSet();
            ds.Tables.Add(table);
            ds.Tables.Add(table2);

            Assert.DoesNotThrow(() => ExcelTools.ExportToFile(ds, "export.xls", true));


            //Assert.DoesNotThrow(() => ExcelTools.ExportToFile(table, "export.xls", false, null, o =>
            //{
            //    ICellStyle style = o.Sheet.Workbook.CreateCellStyle();

            //    style.BorderBottom = BorderStyle.Thin;
            //    style.BorderLeft = BorderStyle.Thin;
            //    style.BorderRight = BorderStyle.Thin;
            //    style.BorderTop = BorderStyle.Thin;

            //    if (o.RowIndex == 0)
            //    {
            //        style.Alignment = HorizontalAlignment.Center;

            //        IFont font = o.Sheet.Workbook.CreateFont();
            //        font.Boldweight = (short)FontBoldWeight.Bold;
            //        font.Color = HSSFColor.Red.Index;
            //        style.SetFont(font);

            //        style.FillForegroundColor = HSSFColor.Blue.Index;
            //        style.FillPattern = FillPattern.SolidForeground;
            //    }
            //    return style;
            //}));

            DataTable dt = ExcelTools.ImportToDataTable("export.xls", 0, true);

            Assert.AreEqual(dt.Rows.Count, 6);
        }
    }
}
