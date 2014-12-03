using System;
using System.Collections;
using System.Collections.Generic;

namespace Radial.Tools.NhDbFirst.Data
{
    /// <summary>
    /// 表示数据行的集合
    /// </summary>
    public sealed class RowDataCollection : ReadOnlyCollectionBase,IEnumerable<RowData>
    {
        private readonly IList<RowData> _genericList;

        /// <summary>
        /// 初始化一个 <see cref="RowDataCollection"/> 类的对象.
        /// </summary>
        public RowDataCollection()
        {
            _genericList = new List<RowData>();
        }

        /// <summary>
        /// 将数据行添加到集合中的内部方法
        /// </summary>
        /// <param name="row">数据行</param>
        internal void Add(RowData row)
        {
            _genericList.Add(row);
        }

        /// <summary>
        /// 按索引获取数据行
        /// </summary>
        /// <param name="index">从0开始的数据行索引</param>
        public RowData this[int index]
        {
            get { return _genericList[index]; }
        }

        /// <summary>
        /// 按字段名获取数据行
        /// </summary>
        /// <param name="fieldName">数据行的字段名</param>
        public RowData this[string fieldName]
        {
            get
            {
                foreach (var r in _genericList)
                {
                    if (string.Compare(r.FieldName, fieldName, StringComparison.OrdinalIgnoreCase) == 0)
                        return r;
                }

                return null;
            }
        }

        /// <summary>
        /// 获取包含在 <see cref="T:System.Collections.ReadOnlyCollectionBase" /> 实例中的元素数。
        /// </summary>
        /// <returns>包含在 <see cref="T:System.Collections.ReadOnlyCollectionBase" /> 实例中的元素数。检索此属性的值的运算复杂度为 O(1)。</returns>
        public override int Count
        {
            get
            {
                return _genericList.Count;
            }
        }


        /// <summary>
        /// 返回一个循环访问集合的枚举数。.
        /// </summary>
        /// <returns>可用于循环访问集合的 System.Collections.Generic.IEnumerator&lt;RowData&gt;</returns>
        public new IEnumerator<RowData> GetEnumerator()
        {
            return _genericList.GetEnumerator();
        }
    }
}
