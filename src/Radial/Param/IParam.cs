using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace Radial.Param
{
    /// <summary>
    /// IParam interface.
    /// </summary>
    [ServiceContract]
    public interface IParam
    {
        /// <summary>
        /// Determine whether the specified param object is exists.
        /// </summary>
        /// <param name="path">The parameter path (case insensitive) or configuration name.</param>
        /// <returns>
        ///   <c>true</c> if the specified path is exists; otherwise, <c>false</c>.
        /// </returns>
        [OperationContract]
        bool Exists(string path);

        /// <summary>
        /// Get param object.
        /// </summary>
        /// <param name="path">The parameter path (case insensitive) or configuration name.</param>
        /// <returns>If path exists, return the object, otherwise return null.</returns>
        [OperationContract]
        ParamObject Get(string path);

        /// <summary>
        /// Get the param value.
        /// </summary>
        /// <param name="path">The parameter path (case insensitive) or configuration name.</param>
        /// <returns>If path exists, return its value, otherwise return string.Empty.</returns>
        [OperationContract]
        string GetValue(string path);

        /// <summary>
        /// Get next level objects.
        /// </summary>
        /// <param name="currentPath">The current parameter path (case insensitive and list all of first level objects when it sets to string.Empty or null).</param>
        /// <returns>If data exists, return an objects list, otherwise return an empty list.</returns>
        [OperationContract]
        IList<ParamObject> Next(string currentPath);

        /// <summary>
        /// Get next level objects.
        /// </summary>
        /// <param name="currentPath">The current parameter path (case insensitive and list all of first level objects when it sets to string.Empty or null).</param>
        /// <param name="pageSize">The list size per page.</param>
        /// <param name="pageIndex">The index of page.</param>
        /// <param name="objectTotal">The object total.</param>
        /// <returns>If data exists, return an objects list, otherwise return an empty list.</returns>
        [OperationContract(Name = "PagedNext")]
        IList<ParamObject> Next(string currentPath, int pageSize, int pageIndex, out int objectTotal);

        /// <summary>
        /// Search objects.
        /// </summary>
        /// <param name="path">The parameter path (case insensitive) or configuration name.</param>
        /// <returns>If path matches, return an objects list, otherwise return an empty list.</returns>
        [OperationContract]
        IList<ParamObject> Search(string path);

        /// <summary>
        /// Search objects.
        /// </summary>
        /// <param name="path">The parameter path (case insensitive) or configuration name.</param>
        /// <param name="pageSize">The list size per page.</param>
        /// <param name="pageIndex">The index of page.</param>
        /// <param name="objectTotal">The object total.</param>
        /// <returns>If path matches, return an objects list, otherwise return an empty list.</returns>
        [OperationContract(Name = "PagedSearch")]
        IList<ParamObject> Search(string path, int pageSize, int pageIndex, out int objectTotal);

        ///// <summary>
        ///// Create new param object.
        ///// </summary>
        ///// <param name="path">The parameter path (case insensitive) or configuration name.</param>
        ///// <param name="description">The description.</param>
        ///// <param name="value">The value.</param>
        //[OperationContract]
        //void Create(string path, string description, string value);


        ///// <summary>
        ///// Update param object.
        ///// </summary>
        ///// <param name="path">The parameter path (case insensitive) or configuration name.</param>
        ///// <param name="description">The new description.</param>
        ///// <param name="value">The new value.</param>
        //[OperationContract]
        //void Update(string path, string description, string value);


        /// <summary>
        /// Save param object.
        /// </summary>
        /// <param name="path">The parameter path (case insensitive) or configuration name.</param>
        /// <param name="value">The value.</param>
        [OperationContract]
        void Save(string path, string value);

        /// <summary>
        /// Save param object.
        /// </summary>
        /// <param name="path">The parameter path (case insensitive) or configuration name.</param>
        /// <param name="description">The description.</param>
        /// <param name="value">The value.</param>
        [OperationContract]
        void Save(string path, string description, string value);

        /// <summary>
        /// Delete param object.
        /// </summary>
        /// <param name="path">The parameter path (case insensitive) or configuration name.</param>
        [OperationContract]
        void Delete(string path);
    }
}
