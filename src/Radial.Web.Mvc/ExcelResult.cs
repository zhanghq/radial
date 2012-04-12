using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Data;

namespace Radial.Web.Mvc
{
    /// <summary>
    /// Renders Excel file to the response.
    /// </summary>
    public class ExcelResult : ActionResult
    {
        DataSet _ds;
        string _fn;
        Encoding _encoding;


        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelResult"/> class.
        /// </summary>
        /// <param name="dt">The DataTable.</param>
        /// <param name="fileName">The file name(not contains extension).</param>
        public ExcelResult(DataTable dt, string fileName)
            : this(dt, fileName, Encoding.UTF8)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelResult"/> class.
        /// </summary>
        /// <param name="dt">The DataTable.</param>
        /// <param name="fileName">The file name(not contains extension).</param>
        /// <param name="encoding">The encoding.</param>
        public ExcelResult(DataTable dt, string fileName, Encoding encoding)
        {
            Checker.Parameter(dt != null, "DataTable object can not be null");
            Checker.Parameter(!string.IsNullOrWhiteSpace(fileName), "fileName can not be empty or null");

            _ds = new DataSet();
            _ds.Tables.Add(dt);

            _fn = fileName.Trim();

            if (encoding != null)
                _encoding = encoding;
            else
                _encoding = Encoding.UTF8;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelResult"/> class.
        /// </summary>
        /// <param name="ds">The DataSet.</param>
        /// <param name="fileName">The file name(not contains extension).</param>
        public ExcelResult(DataSet ds, string fileName)
            : this(ds, fileName, Encoding.UTF8)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelResult"/> class.
        /// </summary>
        /// <param name="ds">The DataSet.</param>
        /// <param name="fileName">The file name(not contains extension).</param>
        /// <param name="encoding">The encoding.</param>
        public ExcelResult(DataSet ds, string fileName, Encoding encoding)
        {
            Checker.Parameter(ds != null, "DataSet object can not be null");
            Checker.Parameter(!string.IsNullOrWhiteSpace(fileName), "fileName can not be empty or null");

            _ds = ds;
            _fn = fileName.Trim();

            if (encoding != null)
                _encoding = encoding;
            else
                _encoding = Encoding.UTF8;
        }


        /// <summary>
        /// Enables processing of the result of an action method by a custom type that inherits from the <see cref="T:System.Web.Mvc.ActionResult"/> class.
        /// </summary>
        /// <param name="context">The context in which the result is executed. The context information includes the controller, HTTP content, request context, and route data.</param>
        public override void ExecuteResult(ControllerContext context)
        {
            HttpKits.ExportToExcel(_ds, _fn, _encoding);
        }
    }
}
