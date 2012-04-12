using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radial.Param;

namespace Radial.Data.Nhs.Param
{
    /// <summary>
    /// ParamEntityExtension.
    /// </summary>
    public static class ParamEntityExtension
    {
        /// <summary>
        /// Toes the ParamObject.
        /// </summary>
        /// <returns>ParamObject instance.</returns>
        public static ParamObject ToObject(this ParamEntity e)
        {
            if (e == null)
                return null;

            return new ParamObject
            {
                Path = e.Path,
                Description = e.Description,
                Value = e.Value,
                ContainsNext = e.Children.Count > 0 ? true : false
            };
        }

        /// <summary>
        /// Toes the objects.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <returns>ParamObject instances.</returns>
        public static IList<ParamObject> ToObjects(this IList<ParamEntity> list)
        {
            IList<ParamObject> objects = new List<ParamObject>();

            if (list != null)
            {
                foreach (ParamEntity e in list)
                    objects.Add(e.ToObject());
            }

            return objects;
        }
    }
}
