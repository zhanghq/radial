using Radial.Persist.Cfg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial.Persist
{
    /// <summary>
    /// Storage router interface
    /// </summary>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    public interface IStorageRouter<TObject> where TObject : class
    {
        /// <summary>
        /// Gets the persistence config.
        /// </summary>
        /// <value>
        /// The persistence config.
        /// </value>
        PersistenceCfg PersistenceCfg { get; }
        /// <summary>
        /// Gets the storage aliases.
        /// </summary>
        /// <returns></returns>
        string[] GetStorageAliases();
        /// <summary>
        /// Gets the storage alias.
        /// </summary>
        /// <param name="selector">The storage selector.</param>
        /// <returns></returns>
        string GetStorageAlias(object selector);
    }

    /// <summary>
    /// StorageRouter
    /// </summary>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    public abstract class StorageRouter<TObject> : IStorageRouter<TObject> where TObject : class
    {
        PersistenceCfg _cfg;

        /// <summary>
        /// Initializes a new instance of the <see cref="StorageRouter{TObject}"/> class.
        /// </summary>
        /// <param name="cfg">The CFG.</param>
        public StorageRouter(PersistenceCfg cfg)
        {
            _cfg = cfg;
        }

        /// <summary>
        /// Gets the persistence config.
        /// </summary>
        /// <value>
        /// The persistence config.
        /// </value>
        public PersistenceCfg PersistenceCfg
        {
            get
            {
                return _cfg;
            }
        }

        /// <summary>
        /// Gets the storage alias.
        /// </summary>
        /// <param name="selector">The storage selector.</param>
        /// <returns></returns>
        public abstract string GetStorageAlias(object selector);

        /// <summary>
        /// Gets the storage aliases.
        /// </summary>
        /// <returns></returns>
        public abstract string[] GetStorageAliases();
    }
}
