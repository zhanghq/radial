using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

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
        /// <param name="objType">The type of the object.</param>
        /// <returns>The xml string.</returns>
        public static string Serialize(object obj, Type objType)
        {
            Checker.Parameter(objType != null, "object type can not be null");

            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(objType);
            using (MemoryStream stream = new MemoryStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                serializer.Serialize(stream, obj);
                stream.Position = 0;

                return reader.ReadToEnd();
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
            Checker.Parameter(objType != null, "object type can not be null");

            object obj = null;

            if (!string.IsNullOrWhiteSpace(xml))
            {
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(objType);
                using (TextReader tr = new StringReader(xml))
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
                using (TextReader tr = new StringReader(xml))
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
        public static bool TryDeserialize(string xml, Type objType,out object obj)
        {
            Checker.Parameter(objType != null, "object type can not be null");

            bool success = false;

            obj = null;

            if (!string.IsNullOrWhiteSpace(xml))
            {
                try
                {
                    System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(objType);
                    using (TextReader tr = new StringReader(xml))
                    {
                        obj = serializer.Deserialize(tr);
                        success = true;
                    }
                }
                catch (Exception e)
                {
                    Logger.Default.Error(e, "can not deserialize xml to object: {0}", xml);
                }
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
                    using (TextReader tr = new StringReader(xml))
                    {
                        obj = (TObject)serializer.Deserialize(tr);
                        success = true;
                    }
                }
                catch (Exception e)
                {
                    Logger.Default.Error(e, "can not deserialize xml to object: {0}", xml);
                }
            }
            return success;
        }
    }
}
