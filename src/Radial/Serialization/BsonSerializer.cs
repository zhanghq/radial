using Newtonsoft.Json.Bson;
using System.IO;

namespace Radial.Serialization
{
    /// <summary>
    /// Bson serializer.
    /// </summary>
    public static class BsonSerializer
    {

        /// <summary>
        /// Serialize to binary array.
        /// </summary>
        /// <typeparam name="T">The object type.</typeparam>
        /// <param name="o">The object.</param>
        /// <returns>
        /// The binary array.
        /// </returns>
        public static byte[] Serialize<T>(T o)
        {
            if (o == null)
                return null;

            using (MemoryStream ms = new MemoryStream())
            using (BsonWriter writer = new BsonWriter(ms))
            {
                Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
                serializer.Serialize(writer, o);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Serialize to binary array.
        /// </summary>
        /// <param name="o">The object.</param>
        /// <returns>The binary array</returns>
        public static byte[] Serialize(object o)
        {
            if (o == null)
                return null;

            using (MemoryStream ms = new MemoryStream())
            using (BsonWriter writer = new BsonWriter(ms))
            {
                Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
                serializer.Serialize(writer, o);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Deserialize from binary array.
        /// </summary>
        /// <typeparam name="T">The object type.</typeparam>
        /// <param name="binary">The binary array.</param>
        /// <returns>
        /// Object instance.
        /// </returns>
        public static T Deserialize<T>(byte[] binary)
        {
            if (binary == null)
                return default(T);

            using (MemoryStream ms = new MemoryStream(binary))
            using (BsonReader reader = new BsonReader(ms))
            {
                Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
                return serializer.Deserialize<T>(reader);
            }
        }

        /// <summary>
        /// Deserialize from binary array.
        /// </summary>
        /// <param name="binary">The binary.</param>
        /// <returns>
        /// Object instance.
        /// </returns>
        public static object Deserialize(byte[] binary)
        {
            if (binary == null)
                return null;

            using (MemoryStream ms = new MemoryStream(binary))
            using (BsonReader reader = new BsonReader(ms))
            {
                Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
                return serializer.Deserialize(reader);
            }
        }

        /// <summary>
        /// Deserialize from binary array.
        /// </summary>
        /// <typeparam name="T">The object type.</typeparam>
        /// <param name="binary">The binary array.</param>
        /// <param name="obj">Deserialized object instance.</param>
        /// <returns>
        /// If successful deserialized return True, otherwise return False.
        /// </returns>
        public static bool TryDeserialize<T>(byte[] binary, out T obj)
        {
            bool success = false;

            obj = default(T);

            if (binary != null)
            {
                try
                {
                    obj = Deserialize<T>(binary);
                    success = true;
                }
                catch { }
            }

            return success;
        }

        /// <summary>
        /// Deserialize from binary array.
        /// </summary>
        /// <param name="binary">The binary array.</param>
        /// <param name="obj">Deserialized object instance.</param>
        /// <returns>
        /// If successful deserialized return True, otherwise return False.
        /// </returns>
        public static bool TryDeserialize(byte[] binary, out object obj)
        {
            bool success = false;

            obj = null;

            if (binary != null)
            {
                try
                {
                    obj = Deserialize(binary);
                    success = true;
                }
                catch { }
            }

            return success;
        }
    }
}
