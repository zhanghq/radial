using System;
using Newtonsoft.Json;

namespace Radial.Serialization.Converters
{
    /// <summary>
    /// Converter boolean value to 0 or 1
    /// </summary>
    public sealed class BooleanJsonConverter : JsonConverter
    {

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>
        ///   <c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.
        /// </returns>
        public override bool CanConvert(Type objectType)
        {
            if (objectType == typeof(bool) || objectType == typeof(Nullable) || objectType.IsGenericType && objectType.GetGenericTypeDefinition() == typeof(Nullable<>))
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
            if (reader.ValueType == typeof(string) || reader.ValueType == typeof(int) || reader.ValueType == typeof(long))
            {
                string str = reader.Value == null ? string.Empty : reader.Value.ToString().Trim();

                if (!string.IsNullOrWhiteSpace(str) && (str == "1" || string.Compare(str, bool.TrueString, true) == 0))
                    return true;
            }


            if (reader.ValueType == typeof(bool))
                return reader.Value;

            if (reader.ValueType == null && (objectType == typeof(Nullable) || objectType.IsGenericType && objectType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                return null;

            return false;
        }

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter"/> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            writer.WriteValue(((bool)value) ? 1 : 0);
        }
    }
}
