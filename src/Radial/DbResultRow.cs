using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial
{
    /// <summary>
    /// DbResultRow
    /// </summary>
    public class DbResultRow : IEnumerable<DbResultField>
    {
        IList<DbResultField> fields=null;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbResultRow"/> class.
        /// </summary>
        /// <param name="fields">The fields.</param>
        public DbResultRow(IEnumerable<DbResultField> fields)
        {
            this.fields = new List<DbResultField>(fields);
        }

        /// <summary>
        /// Gets the <see cref="DbResultField"/> at the specified index.
        /// </summary>
        /// <value>
        /// The <see cref="DbResultField"/>.
        /// </value>
        /// <param name="fieldIndex">The field index.</param>
        /// <returns></returns>
        public DbResultField this[int fieldIndex]
        {
            get { return fields[fieldIndex]; }
        }

        /// <summary>
        /// Gets the <see cref="DbResultField"/> with the specified field name.
        /// </summary>
        /// <value>
        /// The <see cref="DbResultField"/>.
        /// </value>
        /// <param name="fieldName">The field name.</param>
        /// <returns></returns>
        public DbResultField this[string fieldName]
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(fieldName))
                {
                    foreach (var r in fields)
                    {
                        if (string.Compare(r.Name, fieldName.Trim(), StringComparison.OrdinalIgnoreCase) == 0)
                            return r;
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Gets the field value.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="fieldIndex">The field index.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public TValue GetFieldValue<TValue>(int fieldIndex, TValue defaultValue = default(TValue))
        {
            DbResultField field = this[fieldIndex];

            if (field == null)
                return defaultValue;

            if (field.IsDbNull)
                return defaultValue;

            return (TValue)field.Value;
        }

        /// <summary>
        /// Gets the field value.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="fieldName">The field name.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public TValue GetFieldValue<TValue>(string fieldName, TValue defaultValue = default(TValue))
        {
            DbResultField field = this[fieldName];

            if (field == null)
                return defaultValue;

            if (field.IsDbNull)
                return defaultValue;

            return (TValue)field.Value;
        }

        /// <summary>
        /// Gets the field count.
        /// </summary>
        /// <value>
        /// The field count.
        /// </value>
        public int FieldCount
        {
            get
            {
                return fields.Count;
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<DbResultField> GetEnumerator()
        {
            return fields.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return fields.GetEnumerator();
        }
    }
}
