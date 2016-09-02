using System;

namespace Radial
{
    /// <summary>
    /// Search result.
    /// </summary>
    /// <typeparam name="TData">The type of the data.</typeparam>
    public class SearchResult<TData> where TData : class
    {
        int? _objectTotal;
        int? _pageSize;
        int? _pageTotal;

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        public TData Data { get; set; }

        /// <summary>
        /// Gets or sets the object total if pageable.
        /// </summary>
        public int? ObjectTotal
        {
            get
            {
                return _objectTotal;
            }
            set
            {
                _objectTotal = value;
                SetPageTotal();
            }
        }

        /// <summary>
        /// Gets or sets the size of the page if pageable.
        /// </summary>
        public int? PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = value;
                SetPageTotal();
            }
        }

        /// <summary>
        /// Gets the page total if pageable.
        /// </summary>
        public int? PageTotal
        {
            get
            {
                return _pageTotal;
            }
        }

        /// <summary>
        /// Set page total.
        /// </summary>
        private void SetPageTotal()
        {
            _pageTotal = null;

            if (_objectTotal.HasValue && _pageSize.HasValue)
                _pageTotal = (int)Math.Ceiling((double)_objectTotal.Value / _pageSize.Value);
        }
    }
}
