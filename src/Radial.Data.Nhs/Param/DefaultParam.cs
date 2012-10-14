using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radial.Param;
using Radial.Cache;
using Radial.DistLock;

namespace Radial.Data.Nhs.Param
{
    /// <summary>
    /// Default Nhs param implement.
    /// </summary>
    public class DefaultParam : IParam
    {
        static object SyncRoot = new object();
        IParamRepository _repository;
        IUnitOfWork _uow;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultParam"/> class.
        /// </summary>
        /// <param name="uow">The IUnitOfWork instance.</param>
        public DefaultParam(IUnitOfWork uow)
        {
            Checker.Parameter(uow != null, "the IUnitOfWork instance can not be null");
            _uow=uow;
            IDictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("uow", _uow);
            _repository=Components.Resolve<IParamRepository>(dict);
        }

        /// <summary>
        /// Determine whether the specified param object is exists.
        /// </summary>
        /// <param name="path">The parameter path (case insensitive) or configuration name.</param>
        /// <returns>
        ///   <c>true</c> if the specified path is exists; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool Exists(string path)
        {
            path = ParamObject.NormalizePath(path);

            return _repository.Exists(path);
        }

        /// <summary>
        /// Get param object.
        /// </summary>
        /// <param name="path">The parameter path (case insensitive) or configuration name.</param>
        /// <returns>
        /// If path exists, return the object, otherwise return null.
        /// </returns>
        public virtual ParamObject Get(string path)
        {
            path = ParamObject.NormalizePath(path);

            ParamObject o = CacheStatic.Get<ParamObject>(path);

            if (o == null)
            {
                o = _repository.Find(path).ToObject();

                if (o != null)
                    CacheStatic.Set<ParamObject>(o.Path, o);
            }

            return o;

        }

        /// <summary>
        /// Get the param value.
        /// </summary>
        /// <param name="path">The parameter path (case insensitive) or configuration name.</param>
        /// <returns>
        /// If path exists, return its value, otherwise return string.Empty.
        /// </returns>
        public virtual string GetValue(string path)
        {
            ParamObject o = Get(path);
            if (o == null)
                return string.Empty;
            return o.Value;
        }

        /// <summary>
        /// Get next level objects.
        /// </summary>
        /// <param name="currentPath">The current parameter path (case insensitive and list all of first level objects when it sets to string.Empty or null).</param>
        /// <returns>
        /// If data exists, return an objects list, otherwise return an empty list.
        /// </returns>
        public virtual IList<ParamObject> Next(string currentPath)
        {
            currentPath = ParamObject.NormalizePath(currentPath);
            return _repository.FindAll(o => o.Parent.Path == currentPath, new OrderBySnippet<ParamEntity>(o => o.Name)).ToObjects();
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
        public virtual IList<ParamObject> Next(string currentPath, int pageSize, int pageIndex, out int objectTotal)
        {
            currentPath = ParamObject.NormalizePath(currentPath);
            return _repository.FindAll(o => o.Parent.Path == currentPath, new OrderBySnippet<ParamEntity>[] { new OrderBySnippet<ParamEntity>(o => o.Name) }, pageSize, pageIndex, out objectTotal).ToObjects();
        }

        /// <summary>
        /// Search objects.
        /// </summary>
        /// <param name="path">The parameter path (case insensitive) or configuration name.</param>
        /// <returns>
        /// If path matches, return an objects list, otherwise return an empty list.
        /// </returns>
        public virtual IList<ParamObject> Search(string path)
        {
            path = ParamObject.NormalizePath(path);

            return _repository.Search(path).ToObjects();
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
        public virtual IList<ParamObject> Search(string path, int pageSize, int pageIndex, out int objectTotal)
        {
            path = ParamObject.NormalizePath(path);

            return _repository.Search(path, pageSize, pageIndex, out objectTotal).ToObjects();
        }

        /// <summary>
        /// Create new param object.
        /// </summary>
        /// <param name="path">The parameter path (case insensitive) or configuration name.</param>
        /// <param name="description">The description.</param>
        /// <param name="value">The value.</param>
        /// <returns>
        /// If successful created, return param object.
        /// </returns>
        public virtual ParamObject Create(string path, string description, string value)
        {
            ParamEntity e = new ParamEntity(path);

            if (!string.IsNullOrWhiteSpace(description))
                e.Description = description.Trim();
            if (!string.IsNullOrWhiteSpace(value))
                e.Value = value.Trim();

            ParamObject o = null;

            lock (SyncRoot)
            {
                Checker.Requires(!Exists(e.Path), "duplicated path: \"{0}\"", path);

                string parentPath = ParamObject.GetParentPath(path);

                if (!string.IsNullOrWhiteSpace(parentPath))
                {
                    ParamEntity p = _repository.Find(parentPath);
                    Checker.Requires(p != null, "parent path \"{0}\" does not exist", parentPath);
                    e.Parent = p;
                    p.Children.Add(e);
                }

                _uow.RegisterNew<ParamEntity>(e);
                _uow.Commit(true);

                o = e.ToObject();
                CacheStatic.Set<ParamObject>(o.Path, o);
            }

            return o;
        }

        /// <summary>
        /// Update param object.
        /// </summary>
        /// <param name="path">The parameter path (case insensitive) or configuration name.</param>
        /// <param name="description">The new description.</param>
        /// <param name="value">The new value.</param>
        /// <returns>
        /// If successful created, return param object.
        /// </returns>
        public virtual ParamObject Update(string path, string description, string value)
        {
            path = ParamObject.NormalizePath(path);


            ParamObject o = null;
            lock (SyncRoot)
            {
                ParamEntity e = _repository.Find(path);

                Checker.Requires(e != null, "can not find path: \"{0}\"", path);

                if (!string.IsNullOrWhiteSpace(description))
                    e.Description = description.Trim();
                else
                    e.Description = string.Empty;

                if (!string.IsNullOrWhiteSpace(value))
                    e.Value = value.Trim();
                else
                    e.Value = string.Empty;


                _uow.RegisterUpdate<ParamEntity>(e);

                _uow.Commit(true);
                o = e.ToObject();

                CacheStatic.Set<ParamObject>(o.Path, o);
            }
            return o;
        }

        /// <summary>
        /// Delete param object.
        /// </summary>
        /// <param name="path">The parameter path (case insensitive) or configuration name.</param>
        public virtual void Delete(string path)
        {
            path = ParamObject.NormalizePath(path);

            lock (SyncRoot)
            {
                ParamEntity e = _repository.Find(path);

                Checker.Requires(e.Children.Count == 0, "can not delete \"{0}\" because it contains some next level objects", path);

                if (e != null)
                {
                    if (e.Parent != null)
                    {
                        e.Parent.Children.Remove(e);

                        _uow.RegisterDelete<ParamEntity>(e);
                        _uow.Commit(true);

                        CacheStatic.Remove(e.Path);
                    }
                }
            }
        }
    }
}
