using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radial.Data.Nhs.Param
{
    /// <summary>
    /// IParamRepository.
    /// </summary>
    public interface IParamRepository : IRepository<ParamEntity, string>
    {
        /// <summary>
        /// Searches the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        IList<ParamEntity> Search(string path);
        /// <summary>
        /// Searches the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="objectTotal">The object total.</param>
        /// <returns></returns>
        IList<ParamEntity> Search(string path, int pageSize, int pageIndex, out int objectTotal);
    }
}
