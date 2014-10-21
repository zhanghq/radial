using Radial.Param;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial.Persist.Nhs.Param
{
    /// <summary>
    /// Key(Path)-Value Param.
    /// </summary>
    public class KvParam : IParam
    {
        /// <summary>
        /// Storage alias.
        /// </summary>
        private readonly string StorageAlias;

        /// <summary>
        /// Initializes a new instance of the <see cref="KvParam"/> class.
        /// </summary>
        public KvParam()
        {
            if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["KvParam.StorageAlias"]))
                StorageAlias = ConfigurationManager.AppSettings["KvParam.StorageAlias"].Trim().ToLower();
        }

        /// <summary>
        /// Determine whether the specified param object is exists.
        /// </summary>
        /// <param name="path">The parameter path (case insensitive) or configuration name.</param>
        /// <returns>
        ///   <c>true</c> if the specified path is exists; otherwise, <c>false</c>.
        /// </returns>
        public bool Exists(string path)
        {
            return Get(path) != null;
        }

        /// <summary>
        /// Get param object.
        /// </summary>
        /// <param name="path">The parameter path (case insensitive) or configuration name.</param>
        /// <returns>
        /// If path exists, return the object, otherwise return null.
        /// </returns>
        public ParamObject Get(string path)
        {
            return null;
        }

        /// <summary>
        /// Get the param value.
        /// </summary>
        /// <param name="path">The parameter path (case insensitive) or configuration name.</param>
        /// <returns>
        /// If path exists, return its value, otherwise return null.
        /// </returns>
        public string GetValue(string path)
        {
            var obj = Get(path);

            if (obj == null)
                return null;

            return obj.Value;
        }

        /// <summary>
        /// Get next level objects.
        /// </summary>
        /// <param name="currentPath">The current parameter path (case insensitive and list all of first level objects when it sets to string.Empty or null).</param>
        /// <returns>
        /// If data exists, return an objects list, otherwise return an empty list.
        /// </returns>
        public IList<ParamObject> Next(string currentPath)
        {
            return null;
        }

        /// <summary>
        /// Get next level objects.
        /// </summary>
        /// <param name="currentPath">The current parameter path (case insensitive and list all of first level objects when it sets to string.Empty or null).</param>
        /// <param name="pageSize">The list size per page.</param>
        /// <param name="pageIndex">The index of page.</param>
        /// <param name="objectTotal">The object total.</param>
        /// <returns>
        /// If data exists, return an objects list, otherwise return an empty list.
        /// </returns>
        public IList<ParamObject> Next(string currentPath, int pageSize, int pageIndex, out int objectTotal)
        {
            objectTotal = 0;
            return null;
        }

        /// <summary>
        /// Search objects.
        /// </summary>
        /// <param name="path">The parameter path (case insensitive) or configuration name.</param>
        /// <returns>
        /// If path matches, return an objects list, otherwise return an empty list.
        /// </returns>
        public IList<ParamObject> Search(string path)
        {
            return null;
        }

        /// <summary>
        /// Search objects.
        /// </summary>
        /// <param name="path">The parameter path (case insensitive) or configuration name.</param>
        /// <param name="pageSize">The list size per page.</param>
        /// <param name="pageIndex">The index of page.</param>
        /// <param name="objectTotal">The object total.</param>
        /// <returns>
        /// If path matches, return an objects list, otherwise return an empty list.
        /// </returns>
        public IList<ParamObject> Search(string path, int pageSize, int pageIndex, out int objectTotal)
        {
            objectTotal = 0;
            return null;
        }

        /// <summary>
        /// Save param object.
        /// </summary>
        /// <param name="path">The parameter path (case insensitive) or configuration name.</param>
        /// <param name="value">The value.</param>
        public void Save(string path, string value)
        {
            Save(path, null, value);
        }

        /// <summary>
        /// Save param object.
        /// </summary>
        /// <param name="path">The parameter path (case insensitive) or configuration name.</param>
        /// <param name="description">The description.</param>
        /// <param name="value">The value.</param>
        public void Save(string path, string description, string value)
        {
        }

        /// <summary>
        /// Delete param object.
        /// </summary>
        /// <param name="path">The parameter path (case insensitive) or configuration name.</param>
        public void Delete(string path)
        {
        }
    }
}
