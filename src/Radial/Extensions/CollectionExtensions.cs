using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radial.Extensions
{
    /// <summary>
    /// Collection extensions
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Removes specified elements from collection by using a specified predicate.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">A sequence in which to locate a value.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        public static void Remove<TSource>(this ICollection<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw new System.ArgumentNullException("source");
            if (predicate == null)
                throw new System.ArgumentNullException("predicate");

            IEnumerable<TSource> temps = source.TakeWhile(predicate);

            for (int i = 0; i < temps.Count(); i++)
                source.Remove(temps.ElementAt(i));
        }
    }
}
