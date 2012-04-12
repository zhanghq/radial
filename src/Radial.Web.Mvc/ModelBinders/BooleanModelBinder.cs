using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Radial.Web.Mvc.ModelBinders
{
    /// <summary>
    /// Boolean ModelBinder.
    /// </summary>
    public class BooleanModelBinder : IModelBinder
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
            string rawValue = controllerContext.HttpContext.Request[bindingContext.ModelName];

            if (string.IsNullOrWhiteSpace(rawValue))
            {
                if (bindingContext.ModelType != typeof(bool))
                    return null;
                return false;
            }

            if (rawValue.Trim() == "1")
                return true;
            
            bool bValue=false;

            bool.TryParse(rawValue, out bValue);

            return bValue;
        }

        #endregion
    }
}
