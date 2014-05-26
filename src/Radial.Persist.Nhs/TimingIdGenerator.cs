using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return TimingSeq.Next();
        }
    }
}
