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
            byte[] bytes = null;
            IFormatter formatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream())
            {
                formatter.Serialize(stream, obj);
                bytes = stream.ToArray();
            }
            return bytes;
        }


        /// <summary>
        /// Serializes object to binary array.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="obj">The obj instance.</param>
        /// <returns>The binary array.</returns>
        public static byte[] Serialize<TObject>(TObject obj)
        {
            byte[] bytes = null;
            IFormatter formatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream())
            {
                formatter.Serialize(stream, obj);
                bytes = stream.ToArray();
            }
            return bytes;
        }

        /// <summary>
        /// Deserializes binary array to object.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="binary">The binary array.</param>
        /// <returns>
        /// If the binary array is not null return the object, otherwise return type default value.
        /// </returns>
        public static TObject Deserialize<TObject>(byte[] binary)
        {

            TObject o = default(TObject);

            if (binary != null)
            {
                IFormatter formatter = new BinaryFormatter();
                using (Stream stream = new MemoryStream(binary))
                {
                    o = (TObject)formatter.Deserialize(stream);
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
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="binary">The binary array.</param>
        /// <param name="obj">Deserialized object instance.</param>
        /// <returns>If successful deserialized return True, otherwise return False.</returns>
        public static bool TryDeserialize<TObject>(byte[] binary, out TObject obj)
        {
            bool success = false;

            obj = default(TObject);

            if (binary != null)
            {
                try
                {
                    IFormatter formatter = new BinaryFormatter();
                    using (Stream stream = new MemoryStream(binary))
                    {
                        obj = (TObject)formatter.Deserialize(stream);
                        success = true;
                    }
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
                    IFormatter formatter = new BinaryFormatter();
                    using (Stream stream = new MemoryStream(binary))
                    {
                        obj = formatter.Deserialize(stream);
                        success = true;
                    }
                }
                catch { }
            }

            return success;
        }
    }
}
