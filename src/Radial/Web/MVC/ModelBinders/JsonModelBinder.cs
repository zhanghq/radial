using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Radial.Serialization;

namespace Radial.Web.Mvc.ModelBinders
{
    /// <summary>
    /// Json ModelBinder.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    public class JsonModelBinder<TModel> : IModelBinder
    {
        #region IModelBinder Members

        /// <summary>
        /// Binds the model to a value by using the specified controller context and binding context.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="bindingContext">The binding context.</param>
        /// <returns>
        /// The bound value.
        /// </returns>
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            string rawJson = controllerContext.HttpContext.Request[bindingContext.ModelName];

            return JsonSerializer.Deserialize<TModel>(rawJson);
        }

        #endregion
    }
}
