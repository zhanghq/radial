using System;
using System.Linq.Expressions;

namespace Radial.Persist
{
    /// <summary>
    /// The order by snippet
    /// </summary>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    public sealed class OrderBySnippet<TObject> where TObject : class
    {
        Expression<Func<TObject, object>> _property;
        string _propertyName;
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
            _propertyName = property.ToString().Split(new string[] { Property.Parameters[0].Name + "." },
                StringSplitOptions.RemoveEmptyEntries)[1].Trim(')', '(');
            _isAscending = isAscending;
        }


        /// <summary>
        /// Gets the sort property.
        /// </summary>
        public Expression<Func<TObject, object>> Property { get { return _property; } }

        /// <summary>
        /// Gets the name of the sort property.
        /// </summary>
        public string PropertyName { get { return _propertyName; } }

        /// <summary>
        /// Gets a value indicating whether the property will sort in ascending.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the property will sort in ascending; otherwise, <c>false</c>.
        /// </value>
        public bool IsAscending { get { return _isAscending; } }
    }
}
