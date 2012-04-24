using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Radial.Param;

namespace Radial.Serialization.Converters
{
    /// <summary>
    /// DateTime with milliseconds json converter class.
    /// </summary>
    public sealed class DateTimeMillisecondJsonConverter : JsonConverter
    {
        string _format="yyyy/MM/dd HH:mm:ss.fff";

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>
        ///   <c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.
        /// </returns>
        public override bool CanConvert(Type objectType)
        {
            if (objectType == typeof(DateTime) || objectType == typeof(Nullable) || objectType.IsGenericType && objectType.GetGenericTypeDefinition() == typeof(Nullable<>))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader"/> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>
        /// The object value.
        /// </returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            if (reader.ValueType == typeof(string))
            {
                string str = reader.Value == null ? string.Empty : reader.Value.ToString().Trim();

                if (!string.IsNullOrWhiteSpace(str))
                {
                    DateTime time;
                    if (DateTime.TryParse(str, out time))
                        return time;
                }
            }

            if (reader.ValueType == typeof(DateTime))
                return reader.Value;

            if (reader.ValueType == null && (objectType == typeof(Nullable) || objectType.IsGenericType && objectType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                return null;

            return DateTime.MinValue;
        }

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter"/> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            writer.WriteValue(((DateTime)value).ToString(_format));
        }
    }
}
