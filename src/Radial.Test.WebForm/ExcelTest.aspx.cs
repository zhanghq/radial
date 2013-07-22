using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Radial.Test.WebForm
{
    public partial class ExcelTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            DataTable table = new DataTable();

            for (int i = 0; i < 8; i++)
                table.Columns.Add(i.ToString() + "CodeCodeCodeCodeCodeCodeCodeCodeCodeCodeCodeCodeCodeCodeCodeCodeCodeCode");

            table.Rows.Add("CodeCodeCodeCodeCodeCodeCodeCodeCode", "BC00023");

            table.Rows.Add(new object[] { });

            table.Rows.Add("Name", "A1", "A2", "A3", "A4", "A5");

            table.Rows.Add("test", "测试的方式发生地方我认为人反感的风格时光飞逝的歌爱上对方是乏味让我发的发二恶烷", "45%", "$19.4", "Fd09", DateTime.Now);
            table.Rows.Add("test2", "23.4", "45%", "19.4", "Fd09", DateTime.Now);
            for (int i = 0; i < 655; i++)
            {
                table.Rows.Add("test3", "23.4", "45%", "$19.4", "Fd09", DateTime.Now);
            }


            DataTable table2 = table.Copy();
            table2.TableName = "Test2";

            DataSet ds = new DataSet();
            ds.Tables.Add(table);
            ds.Tables.Add(table2);

            ExcelTools.ExportToHttp(ds, "test");
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {
                GridView1.DataSource = ExcelTools.ImportToDataSet(FileUpload1.FileContent,false);
                GridView1.DataBind();
            }
        }
    }
}