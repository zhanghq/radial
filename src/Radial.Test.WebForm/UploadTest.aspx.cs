using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Radial.Web;

namespace Radial.Test.WebForm
{
    public partial class UploadTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            GeneralUpload uploader = new GeneralUpload();
            var result= uploader.Save(FileUpload1.FileName, FileUpload1.FileBytes,"~/uploads");

            Literal1.Text = string.Format("state: {0}, file path={1}", result.State, result.FilePath); 
        }
    }
}