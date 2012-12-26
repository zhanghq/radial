using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Radial.Data
{
    /// <summary>
    /// The order by snippet
    /// </summary>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    public sealed class OrderBySnippet<TObject> where TObject : class
    {
        Expression<Func<TObject, object>> _property;
        bool _isAscending;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderBySnippet&lt;TObject&gt;"/> class.
        /// </summary>
        /// <param name="property">The sort property.</param>
        /// <param name="isAscending">if set to <c>true</c> [the property will sort in ascending].</param>
        public OrderBySnippet(Expression<Func<TObject, object>> property, bool isAscending = true)
        {
            Checker.Parameter(property != null, "the sort property can not be null");
            _property = property;
            _isAscending = isAscending;
        }


        /// <summary>
        /// Gets the sort property.
        /// </summary>
        public Expression<Func<TObject, object>> Property { get { return _property; } }


        /// <summary>
        /// Gets a value indicating whether the property will sort in ascending.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the property will sort in ascending; otherwise, <c>false</c>.
        /// </value>
        public bool IsAscending { get { return _isAscending; } }
    }
}
