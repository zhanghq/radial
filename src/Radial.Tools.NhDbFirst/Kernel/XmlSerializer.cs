using System;
using System.Text;
using System.IO;
using System.Xml;

namespace Radial.Tools.NhDbFirst.Kernel
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
        /// <param name="objType">The type of the object.</param>
        /// <returns>The xml string.</returns>
        public static string Serialize(object obj, Type objType)
        {
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(objType);

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
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="obj">The obj instance.</param>
        /// <returns>The xml string.</returns>
        public static string Serialize<TObject>(TObject obj)
        {
            return Serialize(obj, typeof(TObject));
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
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="xml">The xml.</param>
        /// <returns>
        /// If the xml is not null or empty return object, otherwise return type default value.
        /// </returns>
        public static TObject Deserialize<TObject>(string xml)
        {
            TObject obj = default(TObject);

            if (!string.IsNullOrWhiteSpace(xml))
            {
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(TObject));
                using (TextReader tr = new StringReader(xml.Trim()))
                {
                    obj = (TObject)serializer.Deserialize(tr);
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
        public static bool TryDeserialize(string xml, Type objType, out object obj)
        {
            bool success = false;

            obj = null;

            if (!string.IsNullOrWhiteSpace(xml))
            {
                try
                {
                    System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(objType);
                    using (TextReader tr = new StringReader(xml.Trim()))
                    {
                        obj = serializer.Deserialize(tr);
                        success = true;
                    }
                }
                catch { }
            }
            return success;
        }

        /// <summary>
        /// Deserializes xml to object.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="xml">The xml.</param>
        /// <param name="obj">Deserialized object instance.</param>
        /// <returns>If successful deserialized return True, otherwise return False.</returns>
        public static bool TryDeserialize<TObject>(string xml, out TObject obj)
        {
            bool success = false;

            obj = default(TObject);

            if (!string.IsNullOrWhiteSpace(xml))
            {
                try
                {
                    System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(TObject));
                    using (TextReader tr = new StringReader(xml.Trim()))
                    {
                        obj = (TObject)serializer.Deserialize(tr);
                        success = true;
                    }
                }
                catch { }
            }
            return success;
        }
    }
}
