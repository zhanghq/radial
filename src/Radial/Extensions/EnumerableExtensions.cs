using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radial.Extensions
{
    /// <summary>
    /// <![CDATA[
    /// Provides a set of static (Shared in Visual Basic) methods for querying objects that implement System.Collections.Generic.IEnumerable<T>
    /// ]]>
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Determines whether a sequence contains a specified element by using a specified predicate.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">A sequence in which to locate a value.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>true if the source sequence contains an element that has the specified value; otherwise, false.</returns>
        /// <exception cref="System.ArgumentNullException">source or predicate is null.</exception>
        public static bool Contains<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            return source.Any<TSource>(predicate);
        }
    }
}
