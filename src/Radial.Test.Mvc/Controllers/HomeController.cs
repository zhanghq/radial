using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Radial.Web;
using Radial.Web.Mvc;
using Radial.Web.Mvc.Pagination;
using System.Drawing;
using Radial.Param;

namespace Radial.Test.Mvc.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            ViewBag.UserIP = HttpKits.GetClientIPv4Address();
            return View();
        }

        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            GeneralUpload uploader = new GeneralUpload();
            var result = uploader.Save(file, "Uploads", true);

            if (result.State == UploadState.Succeed)
                return Content(Path.GetFileName(result.FilePath));

            return Content("0");
        }

        public ActionResult ExportExcel()
        {
            DataTable table = new DataTable();

            for (int i = 0; i < 8; i++)
                table.Columns.Add(i.ToString() + "CodeCodeCodeCodeCodeCodeCodeCodeCodeCodeCodeCodeCodeCodeCodeCodeCodeCode");

            table.Rows.Add("CodeCodeCodeCodeCodeCodeCodeCodeCode", "BC00023");

            table.Rows.Add(new object[] { });

            table.Rows.Add("Name", "A1", "A2", "A3", "A4", "A5");

            table.Rows.Add("test", "测试的方式发生地方我认为人反感的风格时光飞逝的歌爱上对方是乏味让我发的发二恶烷", "45%", "$19.4", "Fd09", DateTime.Now);
            table.Rows.Add("test2", "23.4", "45%", "19.4", "Fd09", DateTime.Now);
            for (int i = 0; i < 100; i++)
            {
                table.Rows.Add("test3", "23.4", "45%", "$19.4", "Fd09", DateTime.Now);
            }


            DataTable table2 = table.Copy();


            DataSet ds = new DataSet();
            ds.Tables.Add(table);
            ds.Tables.Add(table2);

            return this.Excel(ds);
        }


        public ActionResult TransferFrom()
        {
            return this.TransferToAction("TransferTo","Home");
        }

        public ActionResult TransferTo(int? id)
        {
            return Content("Transfer To，Id：" + id);
        }

        public ActionResult Page(int? psi = 2, int? pid = 1)
        {
            PagedList<string> list = new PagedList<string>(new string[] { "a", "b" }, pid.Value, psi.Value, 10);
            return View(list);
        }

        public ActionResult ImagePixelSize()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ImagePixelSize(HttpPostedFileBase file)
        {
            try
            {
                Image img = Image.FromStream(file.InputStream);

                ViewBag.Size = img.Size;
            }
            catch(Exception ex)
            {
                ViewBag.Error=ex.Message;
            }

            return View();
        }

        public ActionResult Param()
        {
           ViewBag.Value= AppParam.GetValue("abc");
           return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Param(string value)
        {
            AppParam.Save("abc", value);

            return RedirectToAction("Param");
        }

        public ActionResult PdfToImage()
        {
            int desired_x_dpi = 200;
            int desired_y_dpi = 200;

            var s = Environment.Is64BitProcess;

            var localGhostscriptDll = Path.Combine(@"C:\Users\Haiqing\Documents\CodeWork\radial\gs\gsdll32.dll");
            var localDllInfo = new Ghostscript.NET.GhostscriptVersionInfo(localGhostscriptDll);

            string inputPdfPath = @"D:\资料\电子书\NET设计规范-.NET约定、惯用法与模式.pdf";
            string outputPath = @"D:\";

            
            using (var rasterizer = new Ghostscript.NET.Rasterizer.GhostscriptRasterizer())
            using(Stream fs=System.IO.File.Open(inputPdfPath, FileMode.Open))
            {
                rasterizer.Open(fs, localDllInfo, false);

                for (var pageNumber = 1; pageNumber <= rasterizer.PageCount && pageNumber <= 10; pageNumber++)
                {
                    var pageFilePath = Path.Combine(outputPath, string.Format("Page-{0}.png", pageNumber));

                    var img = rasterizer.GetPage(desired_x_dpi, desired_y_dpi, pageNumber);
                    img.Save(pageFilePath, System.Drawing.Imaging.ImageFormat.Png);
                }

                rasterizer.Close();
            }

            return View();
        }
    }
}
