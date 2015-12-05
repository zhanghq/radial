using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Radial.Persist.Efs
{
    /// <summary>
    /// ExtensionMethods
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Gets the entity key names.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public static string[] GetKeyNames<T>(this DbContext context) where T : class
        {
            try
            {

                Type t = typeof(T);

                //retreive the base type
                while (t.BaseType != typeof(object))
                {
                    t = t.BaseType;
                }

                ObjectContext objectContext = ((IObjectContextAdapter)context).ObjectContext;

                //create method CreateObjectSet with the generic parameter of the base-type
                MethodInfo method = typeof(ObjectContext).GetMethod("CreateObjectSet", Type.EmptyTypes)
                                                         .MakeGenericMethod(t);
                dynamic objectSet = method.Invoke(objectContext, null);


                IEnumerable<dynamic> keyMembers = objectSet.EntitySet.ElementType.KeyMembers;
                string[] keyNames = keyMembers.Select(k => (string)k.Name).ToArray();

                return keyNames;
            }
            catch (Exception ex)
            {
                throw new NotSupportedException(string.Format("error occurs while searching for primary key property from {0}, "
                    + "or there was no primary key property be specified", typeof(T).FullName), ex);
            }
        }

        /// <summary>
        /// Gets the entity key values.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="context">The context.</param>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static object[] GetKeyValues<T>(this DbContext context, T entity) where T : class
        {
            var keyNames = GetKeyNames<T>(context);
            Type type = typeof(T);

            object[] keys = new object[keyNames.Length];
            for (int i = 0; i < keyNames.Length; i++)
            {
                keys[i] = type.GetProperty(keyNames[i]).GetValue(entity, null);
            }
            return keys;
        }


        /// <summary>
        /// Gets the logger.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public static Logger GetLogger(this DbContext context)
        {
            return Logger.GetInstance("EntityFramework");
        }
    }
}
