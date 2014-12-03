using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Drawing;
using System.Drawing.Imaging;

namespace Radial.Web.Mvc
{
    /// <summary>
    /// Renders image to the response.
    /// </summary>
    public class ImageResult : ActionResult
    {
        Image _image;
        ImageFormat _format;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageResult"/> class.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="format">The image format.</param>
        public ImageResult(Image image, ImageFormat format)
        {
            Checker.Parameter(image != null, "image object can not be null");
            Checker.Parameter(format != null, "image format object can not be null");
            _image = image;
            _format = format;
        }

        /// <summary>
        /// Enables processing of the result of an action method by a custom type that inherits from the <see cref="T:System.Web.Mvc.ActionResult"/> class.
        /// </summary>
        /// <param name="context">The context in which the result is executed. The context information includes the controller, HTTP content, request context, and route data.</param>
        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.Clear();

            if (_format.Equals(ImageFormat.Bmp)) 
                context.HttpContext.Response.ContentType = "image/bmp";
            if (_format.Equals(ImageFormat.Gif)) 
                context.HttpContext.Response.ContentType = "image/gif";
            if (_format.Equals(ImageFormat.Icon)) 
                context.HttpContext.Response.ContentType = "image/vnd.microsoft.icon";
            if (_format.Equals(ImageFormat.Jpeg)) 
                context.HttpContext.Response.ContentType = "image/jpeg";
            if (_format.Equals(ImageFormat.Png)) 
                context.HttpContext.Response.ContentType = "image/png";
            if (_format.Equals(ImageFormat.Tiff)) 
                context.HttpContext.Response.ContentType = "image/tiff";
            if (_format.Equals(ImageFormat.Wmf)) 
                context.HttpContext.Response.ContentType = "image/wmf";

            _image.Save(context.HttpContext.Response.OutputStream, _format);

        }
    }
}
