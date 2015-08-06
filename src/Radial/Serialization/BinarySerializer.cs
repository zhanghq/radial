using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Radial.Serialization
{
    /// <summary>
    /// Binary serializer.
    /// </summary>
    public class BinarySerializer
    {
        /// <summary>
        /// Serializes object to binary array.
        /// </summary>
        /// <param name="obj">The obj instance.</param>
        /// <returns>The binary array.</returns>
        public static byte[] Serialize(object obj)
        {
            if (obj == null)
                return null;

            IFormatter formatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream())
            {
                formatter.Serialize(stream, obj);
                return stream.ToArray();
            }
        }


        /// <summary>
        /// Serializes object to binary array.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="obj">The obj instance.</param>
        /// <returns>The binary array.</returns>
        public static byte[] Serialize<T>(T obj)
        {
            if (obj == null)
                return null;

            IFormatter formatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream())
            {
                formatter.Serialize(stream, obj);
                return stream.ToArray();
            }
        }

        /// <summary>
        /// Deserializes binary array to object.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="binary">The binary array.</param>
        /// <returns>
        /// If the binary array is not null return the object, otherwise return type default value.
        /// </returns>
        public static T Deserialize<T>(byte[] binary)
        {

            T o = default(T);

            if (binary != null)
            {
                IFormatter formatter = new BinaryFormatter();
                using (Stream stream = new MemoryStream(binary))
                {
                    o = (T)formatter.Deserialize(stream);
                }
            }

            return o;
        }

        /// <summary>
        /// Deserializes binary array to object.
        /// </summary>
        /// <param name="binary">The binary array.</param>
        /// <returns>
        /// If the binary array is not null return the object, otherwise return null.
        /// </returns>
        public static object Deserialize(byte[] binary)
        {
            object o = null;

            if (binary != null)
            {
                IFormatter formatter = new BinaryFormatter();
                using (Stream stream = new MemoryStream(binary))
                {
                    o = formatter.Deserialize(stream);
                }
            }

            return o;
        }

        /// <summary>
        /// Deserializes binary array to object.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="binary">The binary array.</param>
        /// <param name="obj">Deserialized object instance.</param>
        /// <returns>If successful deserialized return True, otherwise return False.</returns>
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
        /// Deserializes binary array to object.
        /// </summary>
        /// <param name="binary">The binary array.</param>
        /// <param name="obj">Deserialized object instance.</param>
        /// <returns>If successful deserialized return True, otherwise return False.</returns>
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
