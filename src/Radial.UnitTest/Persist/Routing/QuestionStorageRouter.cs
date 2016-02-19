using Radial.Persist;
using Radial.UnitTest.Persist.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Radial.Persist.Cfg;

namespace Radial.UnitTest.Persist.Routing
{
    /// <summary>
    /// QuestionStorageRouter
    /// </summary>
    public class QuestionStorageRouter : StorageRouter<Question>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionStorageRouter"/> class.
        /// </summary>
        /// <param name="cfg">The CFG.</param>
        public QuestionStorageRouter(PersistenceCfg cfg) : base(cfg) { }

        /// <summary>
        /// Gets the storage alias.
        /// </summary>
        /// <param name="selector">The storage selector.</param>
        /// <returns></returns>
        public override string GetStorageAlias(object selector)
        {
            if(selector is Question)
            {
                var tmp = selector as Question;

                if(tmp.Subject=="语文")
                    return PersistenceCfg.Storages[0].Alias;
                if (tmp.Subject == "数学")
                    return PersistenceCfg.Storages[1].Alias;
                if (tmp.Subject == "英语")
                    return PersistenceCfg.Storages[2].Alias;
            }
            return PersistenceCfg.Storages[0].Alias;
        }

        /// <summary>
        /// Gets the storage aliases.
        /// </summary>
        /// <returns></returns>
        public override string[] GetStorageAliases()
        {
            return PersistenceCfg.Storages.Select(o => o.Alias).ToArray();
        }
    }
}
