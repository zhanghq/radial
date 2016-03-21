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
        /// To the object context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public static ObjectContext ToObjectContext(this DbContext context)
        {
            return ((IObjectContextAdapter)context).ObjectContext;
        }


        /// <summary>
        /// Gets the logger.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public static LogWriter GetLogger(this DbContext context)
        {
            return Logger.New("EntityFramework");
        }
    }
}
