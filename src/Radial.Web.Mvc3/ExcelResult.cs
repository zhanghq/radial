using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Radial.Web.Mvc
{
    /// <summary>
    /// Excel Result
    /// </summary>
    public class ExcelResult : ActionResult
    {
        IEnumerable<DataTable> _dataTables;
        DataSet _dataSet;
        string _downloadFileName;
        bool _columnHeader;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelResult"/> class.
        /// </summary>
        /// <param name="dataTable">The data table.</param>
        /// <param name="downloadFileName">The download file name.</param>
        /// <param name="columnHeader">if set to <c>true</c> will set column name as header.</param>
        public ExcelResult(DataTable dataTable, string downloadFileName = null, bool columnHeader = true)
            : this(new DataTable[] { dataTable }, downloadFileName, columnHeader)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelResult"/> class.
        /// </summary>
        /// <param name="dataTables">The data tables.</param>
        /// <param name="downloadFileName">The download file name.</param>
        /// <param name="columnHeader">if set to <c>true</c> will set column name as header.</param>
        public ExcelResult(IEnumerable<DataTable> dataTables, string downloadFileName = null, bool columnHeader = true)
        {
            _dataTables = new List<DataTable>(dataTables);
            _downloadFileName = downloadFileName;
            _columnHeader = columnHeader;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelResult"/> class.
        /// </summary>
        /// <param name="dataSet">The data set.</param>
        /// <param name="downloadFileName">The download file name.</param>
        /// <param name="columnHeader">if set to <c>true</c> will set column name as header.</param>
        public ExcelResult(DataSet dataSet, string downloadFileName = null, bool columnHeader = true)
        {
            _dataSet = dataSet;
            _downloadFileName = downloadFileName;
            _columnHeader = columnHeader;
        }

        /// <summary>
        /// Enables processing of the result of an action method by a custom type that inherits from the <see cref="T:System.Web.Mvc.ActionResult" /> class.
        /// </summary>
        /// <param name="context">The context in which the result is executed. The context information includes the controller, HTTP content, request context, and route data.</param>
        public override void ExecuteResult(ControllerContext context)
        {
            if (_dataTables != null)
                ExcelTools.ExportToHttp(_dataTables, _downloadFileName, _columnHeader);
            if (_dataSet != null)
                ExcelTools.ExportToHttp(_dataSet, _downloadFileName, _columnHeader);
        }
    }
}
