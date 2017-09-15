using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.Unity;

namespace Radial
{
    /// <summary>
    /// Extension methods
    /// </summary>
    public static class ExtensionMethods
    {
        #region IDataReader

        /// <summary>
        /// To the result set.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        public static DbResultSet ToResultSet(this IDataReader reader)
        {
            if (reader == null)
                return new DbResultSet();

            IList<DbResultRow> rows = new List<DbResultRow>();


            while (reader.Read())
            {
                IList<DbResultField> fields = new List<DbResultField>();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    fields.Add(new DbResultField
                    {
                        Name = reader.GetName(i),
                        Type = reader.GetFieldType(i),
                        Value = reader[i]
                    });
                }

                rows.Add(new DbResultRow(fields));
            }

            return new DbResultSet(rows);
        }

        #endregion

        #region IEnumerable
        /// <summary>
        /// Determines whether a sequence contains a specified element by using a specified predicate.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">A sequence in which to locate a value.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>true if the source sequence contains an element that has the specified value; otherwise, false.</returns>
        /// <exception cref="System.ArgumentNullException">source or predicate is null.</exception>
        public static bool Contains<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            return source.Any<TSource>(predicate);
        }
        #endregion

        #region ICollection
        /// <summary>
        /// Removes specified elements from collection by using a specified predicate.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">A sequence in which to locate a value.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        public static void Remove<TSource>(this ICollection<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw new System.ArgumentNullException("source");
            if (predicate == null)
                throw new System.ArgumentNullException("predicate");


            for (int i = 0; i < source.Count; i++)
            {
                var o = source.ElementAt(i);
                if (predicate(o))
                {
                    source.Remove(o);
                    i--;
                }
            }
        }

        /// <summary>
        /// Adds objects to the specified source.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="objs">The objs.</param>
        public static void Add<TSource>(this ICollection<TSource> source, IEnumerable<TSource> objs)
        {
            foreach (TSource o in objs)
                source.Add(o);
        }
        #endregion

        #region Stream

        /// <summary>
        /// Read all bytes.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public static byte[] ReadBytes(this Stream stream)
        {
            IList<byte> bytes = new List<byte>();

            if (stream != null && stream.CanRead)
            {
                stream.Position = 0;
                int b = 0;
                while ((b = stream.ReadByte()) > -1)
                    bytes.Add((byte)b);
            }
            return bytes.ToArray();
        }

        #endregion

        #region IUnityContainer
        /// <summary>
        /// Register all interface implementations, with RegisterInterface attribute.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="implAssembly">The implementation assembly.</param>
        /// <param name="symbol">The symbol</param>
        /// <param name="lifetimeManagerFunc">The lifetime manager function.</param>
        /// <param name="injectionMembers">The injection members.</param>
        public static void RegisterInterfaces(this IUnityContainer container, Assembly implAssembly, string symbol = null,
            Func<LifetimeManager> lifetimeManagerFunc = null, params InjectionMember[] injectionMembers)
        {
            var intyps = implAssembly.GetTypes().Where(o =>
            o.CustomAttributes.Contains(x => x.AttributeType == typeof(RegisterInterfaceAttribute))).ToArray();
            RegisterInterfaces(container, intyps, symbol, lifetimeManagerFunc, injectionMembers);

        }

        /// <summary>
        /// Register all interface implementations, with RegisterInterface attribute.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="implTypes">The implementation types.</param>
        /// <param name="symbol">The symbol</param>
        /// <param name="lifetimeManagerFunc">The lifetime manager function.</param>
        /// <param name="injectionMembers">The injection members.</param>
        public static void RegisterInterfaces(this IUnityContainer container, Type[] implTypes, string symbol = null,
            Func<LifetimeManager> lifetimeManagerFunc = null, params InjectionMember[] injectionMembers)
        {
            if (container == null || implTypes == null)
                return;

            foreach (var type in implTypes)
            {
                var attrObjs = type.GetCustomAttributes(typeof(RegisterInterfaceAttribute), false);

                if (attrObjs == null)
                    continue;

                foreach (var attrObj in attrObjs)
                {
                    var attr = attrObj as RegisterInterfaceAttribute;

                    if (attr == null)
                        continue;

                    LifetimeManager fm = null;
                    if (lifetimeManagerFunc != null)
                        fm = lifetimeManagerFunc();

                    //if symbol not null , types which its [attr.Symbol] equals to symbol will be registered only
                    if (!string.IsNullOrWhiteSpace(symbol))
                    {
                        if (!string.IsNullOrWhiteSpace(attr.Symbol) && attr.Symbol == symbol)
                            container.RegisterType(attr.InterfaceType, type, fm, injectionMembers);
                        continue;
                    }
                    container.RegisterType(attr.InterfaceType, type, fm, injectionMembers);
                }
            }
        }
        #endregion

        #region GUID
        /// <summary>
        /// To the long value.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns></returns>
        public static long ToLong(this Guid guid)
        {
            return BitConverter.ToInt64(guid.ToByteArray(), 0);
        }
        #endregion
    }
}
