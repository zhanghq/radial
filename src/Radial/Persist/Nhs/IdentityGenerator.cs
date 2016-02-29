using System;
using System.Collections.Generic;
using NHibernate.Dialect;
using NHibernate.Type;

namespace Radial.Persist.Nhs
{

    /// <summary>
    /// Identity generator.
    /// </summary>
    public class IdentityGenerator : NHibernate.Id.IIdentifierGenerator, NHibernate.Id.IConfigurable
    {

        /// <summary>
        /// Gets the prefix.
        /// </summary>
        public string Prefix { get; private set; }

        /// <summary>
        /// Configures the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="parms">The parms.</param>
        /// <param name="dialect">The dialect.</param>
        public void Configure(IType type, IDictionary<string, string> parms, Dialect dialect)
        {
            if (parms != null)
            {
                if (parms.ContainsKey("prefix") && !string.IsNullOrWhiteSpace(parms["prefix"]))
                    Prefix = parms["prefix"].Trim().ToUpper();
            }
        }

        /// <summary>
        /// Generate a new identifier
        /// </summary>
        /// <param name="session">The <see cref="T:NHibernate.Engine.ISessionImplementor" /> this id is being generated in.</param>
        /// <param name="obj">The entity for which the id is being generated.</param>
        /// <returns>
        /// The new identifier
        /// </returns>
        public object Generate(NHibernate.Engine.ISessionImplementor session, object obj)
        {
            Checker.Parameter(obj != null, "object can not be null");

            Type objType = obj.GetType();

            var metadata = session.Factory.GetClassMetadata(objType);

            Checker.Requires(metadata.HasIdentifierProperty, "{0} does not has identifier property", objType.FullName);

            var p = objType.GetProperty(metadata.IdentifierPropertyName);

            Checker.Requires(p != null, "can not find object identifier property {0} in {1}", metadata.IdentifierPropertyName, objType.FullName);
            Checker.Requires(p.PropertyType == typeof(string), "not support identifier type {0}, please use System.String instead", objType.FullName);

            string objIdVal = p.GetValue(obj) as string;

            if (string.IsNullOrWhiteSpace(objIdVal))
                return string.Format("{0}{1}", Prefix, RandomCode.GetIdentityKey());

            return objIdVal;
        }
    }
}
