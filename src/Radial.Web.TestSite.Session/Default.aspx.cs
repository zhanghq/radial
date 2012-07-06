using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Radial.Web.TestSite.Session
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnSet_Click(object sender, EventArgs e)
        {
            Session["test1"] = Guid.NewGuid().ToString("n");
            Session["test2"] = Guid.NewGuid().ToString("n");

            litValue.Text = "已设置:<br/>" + "SessionTestSite@" + Session.SessionID + "<br/>" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }

        protected void btnRead_Click(object sender, EventArgs e)
        {
            if (Session["test1"] == null && Session["test2"] == null)
                litValue.Text = "空值";
            else
                litValue.Text = "<p>Test1:" + Session["test1"].ToString() + "<br/>Test1:" + Session["test2"].ToString() + "</p><p>" + "SessionTestSite@" + Session.SessionID + "</p>";
        }
    }
}