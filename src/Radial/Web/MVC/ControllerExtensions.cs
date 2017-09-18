using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Data;
using System.Web.Routing;
using OfficeOpenXml;

namespace Radial.Web.Mvc
{
    /// <summary>
    /// Controller extension class.
    /// </summary>
    public static class ControllerExtensions
    {
        /// <summary>
        /// Popup javascript alert window.
        /// </summary>
        /// <param name="c">The controller.</param>
        /// <param name="message">The message.</param>
        /// <param name="redirect">The redirect.</param>
        /// <returns>AlertResult instance.</returns>
        public static AlertResult Alert(this Controller c, string message, string redirect = null)
        {
            return new AlertResult(message, redirect);
        }

        /// <summary>
        /// Renders image to the response.
        /// </summary>
        /// <param name="c">The controller.</param>
        /// <param name="imageStream">The image stream.</param>
        /// <param name="format">The image format.</param>
        /// <returns>ImageResult instance.</returns>
        public static ImageResult Image(this Controller c, Stream imageStream, ImageFormat format)
        {
            return new ImageResult(System.Drawing.Image.FromStream(imageStream), format);
        }

        /// <summary>
        /// Renders image to the response.
        /// </summary>
        /// <param name="c">The controller.</param>
        /// <param name="imageBytes">The image bytes.</param>
        /// <param name="format">The image format.</param>
        /// <returns>ImageResult instance.</returns>
        public static ImageResult Image(this Controller c, byte[] imageBytes, ImageFormat format)
        {
            return new ImageResult(System.Drawing.Image.FromStream(new MemoryStream(imageBytes)), format);
        }

        /// <summary>
        /// Renders image to the response.
        /// </summary>
        /// <param name="c">The controller.</param>
        /// <param name="image">The image.</param>
        /// <param name="format">The image format.</param>
        /// <returns>ImageResult instance.</returns>
        public static ImageResult Image(this Controller c, Image image, ImageFormat format)
        {
            return new ImageResult(image, format);
        }

        /// <summary>
        /// Renders xml to the response.
        /// </summary>
        /// <param name="c">The controller.</param>
        /// <param name="xml">The xml.</param>
        /// <returns>
        /// XmlResult instance.
        /// </returns>
        public static XmlResult Xml(this Controller c, string xml)
        {
            return new XmlResult(xml);
        }

        /// <summary>
        /// Renders json to the response.
        /// </summary>
        /// <param name="c">The controller.</param>
        /// <param name="data">The data.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <returns>
        /// NewJsonResult instance.
        /// </returns>
        public static NewJsonResult NewJson(this Controller c, object data, string contentType = null)
        {
            return new NewJsonResult(data, contentType);
        }

        /// <summary>
        /// Renders standard Error json output to the response.
        /// </summary>
        /// <param name="c">The controller.</param>
        /// <param name="code">The status code.</param>
        /// <param name="message">The message.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <returns>
        /// NewJsonResult instance.
        /// </returns>
        public static StdJsonResult StdErrorJson(this Controller c, int code = 500, string message = null, string contentType = null)
        {
            return new StdJsonResult(new StdJsonOutput { Code = code, Message = message }, contentType);
        }

        /// <summary>
        /// Renders standard Success json output to the response.
        /// </summary>
        /// <param name="c">The controller.</param>
        /// <param name="data">The data payload.</param>
        /// <param name="message">The message.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <returns>
        /// NewJsonResult instance.
        /// </returns>
        public static StdJsonResult StdSuccessJson(this Controller c, object data = null, string message = null, string contentType = null)
        {
            return new StdJsonResult(new StdJsonOutput { Data = data }, contentType);
        }

        /// <summary>
        /// Throws a new KnownFaultException and let the system itself to decide how to deal with.
        /// </summary>
        /// <param name="c">The controller.</param>
        /// <param name="errorCode">The error code.</param>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <returns>ThrowKnownFaultResult instance.</returns>
        public static KnownFaultResult KnownFault(this Controller c, int errorCode, string message = null, Exception innerException = null)
        {
            return new KnownFaultResult(errorCode, message, innerException);
        }


        /// <summary>
        /// Export data to excel file.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <param name="dataTables">The data tables.</param>
        /// <param name="downloadFileName">The download file name.</param>
        /// <param name="columnHeader">if set to <c>true</c>, column name will used as caption property on first row.</param>
        /// <param name="customHandler">The custom handler.</param>
        /// <returns>ExcelResult instance.</returns>
        public static ExcelResult Excel(this Controller c, IEnumerable<DataTable> dataTables, string downloadFileName = null,
            bool columnHeader = true, Action<ExcelWorksheet> customHandler = null)
        {
            return new ExcelResult(dataTables, downloadFileName, columnHeader, customHandler);
        }

        /// <summary>
        /// Export data to excel file.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <param name="dataTable">The data table.</param>
        /// <param name="downloadFileName">The download file name.</param>
        /// <param name="columnHeader">if set to <c>true</c>, column name will used as caption property on first row.</param>
        /// <param name="customHandler">The custom handler.</param>
        /// <returns>ExcelResult instance.</returns>
        public static ExcelResult Excel(this Controller c, DataTable dataTable, string downloadFileName = null,
            bool columnHeader = true, Action<ExcelWorksheet> customHandler = null)
        {
            return new ExcelResult(dataTable, downloadFileName, columnHeader, customHandler);
        }

        /// <summary>
        /// Export data to excel file.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <param name="dataSet">The data set.</param>
        /// <param name="downloadFileName">The download file name.</param>
        /// <param name="columnHeader">if set to <c>true</c>, column name will used as caption property on first row.</param>
        /// <param name="customHandler">The custom handler.</param>
        /// <returns>ExcelResult instance.</returns>
        public static ExcelResult Excel(this Controller c, DataSet dataSet, string downloadFileName = null,
            bool columnHeader = true, Action<ExcelWorksheet> customHandler = null)
        {
            return new ExcelResult(dataSet, downloadFileName, columnHeader, customHandler);
        }

        /// <summary>
        /// Transfers the specified c.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        public static TransferResult Transfer(this Controller c, string url)
        {
            return new TransferResult(url);
        }

        /// <summary>
        /// Transfers to route.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <param name="routeName">Name of the route.</param>
        /// <param name="routeValues">The route values.</param>
        /// <returns></returns>
        public static TransferToRouteResult TransferToRoute(this Controller c, string routeName, RouteValueDictionary routeValues)
        {
            return new TransferToRouteResult(routeName, routeValues);
        }

        /// <summary>
        /// Transfers to route.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <param name="routeValues">The route values.</param>
        /// <returns></returns>
        public static TransferToRouteResult TransferToRoute(this Controller c, RouteValueDictionary routeValues)
        {
            return new TransferToRouteResult(routeValues);
        }

        /// <summary>
        /// Transfers to action.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <returns></returns>
        public static TransferToRouteResult TransferToAction(this Controller c, string actionName)
        {

            return TransferToAction(c, actionName, null, null);
        }

        /// <summary>
        /// Transfers to action.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <returns></returns>
        public static TransferToRouteResult TransferToAction(this Controller c, string actionName, string controllerName)
        {
            return TransferToAction(c, actionName, controllerName, null);
        }

        /// <summary>
        /// Transfers to action.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="routeValues">The route values.</param>
        /// <returns></returns>
        public static TransferToRouteResult TransferToAction(this Controller c, string actionName,
            string controllerName, RouteValueDictionary routeValues = null)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(actionName), "action name can not be empty or null");

            if (routeValues == null)
                routeValues = c.RouteData.Values;

            if (!routeValues.ContainsKey("action"))
                routeValues.Add("action", actionName);
            else
                routeValues["action"] = actionName;

            if (!string.IsNullOrWhiteSpace(controllerName))
            {
                if (!routeValues.ContainsKey("controller"))
                    routeValues.Add("controller", controllerName);
                else
                    routeValues["controller"] = controllerName;
            }

            return new TransferToRouteResult(routeValues);
        }
    }
}
