using System;

namespace Radial.Persist.Nhs
{

    /// <summary>
    /// Identifier generator based on time.
    /// </summary>
    public class TimingIdGenerator : NHibernate.Id.IIdentifierGenerator
    {
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
                return TimingSeq.Next();

            return objIdVal;
        }
    }
}
