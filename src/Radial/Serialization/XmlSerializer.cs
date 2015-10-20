using System;
using System.Text;
using System.IO;
using System.Xml;

namespace Radial.Serialization
{
    /// <summary>
    /// Xml serializer.
    /// </summary>
    public static class XmlSerializer
    {
        /// <summary>
        /// Serializes object to xml.
        /// </summary>
        /// <param name="obj">The obj instance.</param>
        /// <returns>
        /// The xml string.
        /// </returns>
        public static string Serialize(object obj)
        {
            if (obj == null)
                return null;

            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(obj.GetType());

            Encoding enc = new UTF8Encoding(false);

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.Encoding = enc;

            using (MemoryStream ms = new MemoryStream())
            {
                XmlWriter writer = XmlWriter.Create(ms, settings);
                serializer.Serialize(writer, obj);
                writer.Flush();
                return enc.GetString(ms.ToArray());
            }
        }

        /// <summary>
        /// Serializes object to xml.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="obj">The obj instance.</param>
        /// <returns>The xml string.</returns>
        public static string Serialize<T>(T obj)
        {
            if (obj == null)
                return null;

            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));

            Encoding enc = new UTF8Encoding(false);

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.Encoding = enc;

            using (MemoryStream ms = new MemoryStream())
            {
                XmlWriter writer = XmlWriter.Create(ms, settings);
                serializer.Serialize(writer, obj);
                writer.Flush();
                return enc.GetString(ms.ToArray());
            }
        }


        /// <summary>
        /// Deserializes xml to object.
        /// </summary>
        /// <param name="xml">The xml.</param>
        /// <param name="objType">The type of the object.</param>
        /// <returns>
        /// If the xml is not null or empty return object, otherwise return null.
        /// </returns>
        public static object Deserialize(string xml, Type objType)
        {
            Checker.Parameter(objType != null, "object type can not be null");

            object obj = null;

            if (!string.IsNullOrWhiteSpace(xml))
            {
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(objType);
                using (TextReader tr = new StringReader(xml.Trim()))
                {
                    obj = serializer.Deserialize(tr);
                }
            }
            return obj;
        }

        /// <summary>
        /// Deserializes xml to object.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="xml">The xml.</param>
        /// <returns>
        /// If the xml is not null or empty return object, otherwise return type default value.
        /// </returns>
        public static T Deserialize<T>(string xml)
        {
            T obj = default(T);

            if (!string.IsNullOrWhiteSpace(xml))
            {
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                using (TextReader tr = new StringReader(xml.Trim()))
                {
                    obj = (T)serializer.Deserialize(tr);
                }
            }
            return obj;
        }

        /// <summary>
        /// Deserializes xml to object.
        /// </summary>
        /// <param name="xml">The xml.</param>
        /// <param name="objType">The type of the object.</param>
        /// <param name="obj">Deserialized object instance.</param>
        /// <returns>If successful deserialized return True, otherwise return False.</returns>
        public static bool TryDeserialize(string xml, Type objType,out object obj)
        {
            Checker.Parameter(objType != null, "object type can not be null");

            bool success = false;

            obj = null;

            if (!string.IsNullOrWhiteSpace(xml))
            {
                try
                {

                    obj = Deserialize(xml, objType);
                    success = true;
                }
                catch { }
            }
            return success;
        }

        /// <summary>
        /// Deserializes xml to object.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="xml">The xml.</param>
        /// <param name="obj">Deserialized object instance.</param>
        /// <returns>If successful deserialized return True, otherwise return False.</returns>
        public static bool TryDeserialize<T>(string xml, out T obj)
        {
            bool success = false;

            obj = default(T);

            if (!string.IsNullOrWhiteSpace(xml))
            {
                try
                {
                    obj = Deserialize<T>(xml);
                    success = true;
                }
                catch { }
            }
            return success;
        }
    }
}
