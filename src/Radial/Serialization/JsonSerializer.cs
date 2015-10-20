﻿namespace Radial.Serialization
{
    /// <summary>
    /// Json serializer.
    /// </summary>
    public static class JsonSerializer
    {

        /// <summary>
        /// Serialize to json string.
        /// </summary>
        /// <typeparam name="T">The object type.</typeparam>
        /// <param name="o">The object.</param>
        /// <returns>
        /// Json string.
        /// </returns>
        public static string Serialize<T>(T o)
        {
            if (o == null)
                return null;

            return Newtonsoft.Json.JsonConvert.SerializeObject(o);
        }

        /// <summary>
        /// Serialize to json string.
        /// </summary>
        /// <param name="o">The object.</param>
        /// <returns>Json string.</returns>
        public static string Serialize(object o)
        {
            if (o == null)
                return null;

            return Newtonsoft.Json.JsonConvert.SerializeObject(o);
        }

        /// <summary>
        /// Deserialize from json string.
        /// </summary>
        /// <typeparam name="T">The object type.</typeparam>
        /// <param name="json">The json string.</param>
        /// <returns>
        /// Object instance.
        /// </returns>
        public static T Deserialize<T>(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return default(T);

            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// Deserialize from json string.
        /// </summary>
        /// <param name="json">The json string.</param>
        /// <returns>Object instance.</returns>
        public static object Deserialize(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return null;

            return Newtonsoft.Json.JsonConvert.DeserializeObject(json);
        }

        /// <summary>
        /// Deserialize from json string.
        /// </summary>
        /// <typeparam name="T">The object type.</typeparam>
        /// <param name="json">The json string.</param>
        /// <param name="obj">Deserialized object instance.</param>
        /// <returns>If successful deserialized return True, otherwise return False.</returns>
        public static bool TryDeserialize<T>(string json, out T obj)
        {
            bool success = false;

            obj = default(T);

            if (!string.IsNullOrWhiteSpace(json))
            {
                try
                {
                    obj = Deserialize<T>(json);
                    success = true;
                }
                catch { }
            }

            return success;
        }

        /// <summary>
        /// Deserialize from json string.
        /// </summary>
        /// <param name="json">The json string.</param>
        /// <param name="obj">Deserialized object instance.</param>
        /// <returns>If successful deserialized return True, otherwise return False.</returns>
        public static bool TryDeserialize(string json,out object obj)
        {
            bool success = false;

            obj = null;

            if (!string.IsNullOrWhiteSpace(json))
            {
                try
                {
                    obj = Deserialize(json);
                    success = true;
                }
                catch { }
            }

            return success;
        }
    }
}
