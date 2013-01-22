using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Practices.Unity;

namespace Radial
{
    /// <summary>
    /// PerWebRequestLifetimeManager
    /// </summary>
    public sealed class PerWebRequestLifetimeManager : LifetimeManager
    {
        /// <summary>
        /// The item key
        /// </summary>
        readonly string ItemKey = Guid.NewGuid().ToString("N");

        /// <summary>
        /// Retrieve a value from the backing store associated with this Lifetime policy.
        /// </summary>
        /// <returns>
        /// the object desired, or null if no such object is currently stored.
        /// </returns>
        public override object GetValue()
        {
            return HttpContext.Current.Items[ItemKey];
        }

        /// <summary>
        /// Remove the given object from backing store.
        /// </summary>
        public override void RemoveValue()
        {
            HttpContext.Current.Items[ItemKey] = null;
        }

        /// <summary>
        /// Stores the given value into backing store for retrieval later.
        /// </summary>
        /// <param name="newValue">The object being stored.</param>
        public override void SetValue(object newValue)
        {
            HttpContext.Current.Items[ItemKey] = newValue;
        }
    }
}
