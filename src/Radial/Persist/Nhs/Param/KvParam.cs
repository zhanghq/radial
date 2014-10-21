using NHibernate;
using Radial.Param;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
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
            path = ParamObject.NormalizePath(path);

            using (var uow = new NhUnitOfWork(StorageAlias))
            {
                ISession session = uow.UnderlyingContext as ISession;
                var query = session.CreateSQLQuery("SELECT Path,Description,Value,HasNext FROM KvParam WHERE Path=:Path");
                query.SetString("Path", path);
                var lines = query.List();

                if (lines.Count == 0)
                    return null;

                var fields = (object[])lines[0];

                return new ParamObject
                {
                    Path = fields[0].ToString().Trim(),
                    Description = fields[1] != null ? fields[1].ToString() : null,
                    Value = fields[2] != null ? fields[2].ToString().Trim() : null,
                    HasNext = bool.Parse(fields[3].ToString())
                };
            }
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
            currentPath = ParamObject.NormalizePath(currentPath);

            IList<ParamObject> list = new List<ParamObject>();

            using (var uow = new NhUnitOfWork(StorageAlias))
            {
                ISession session = uow.UnderlyingContext as ISession;
                var query = session.CreateSQLQuery("SELECT Path,Description,Value,HasNext FROM KvParam WHERE Parent=:Parent");
                query.SetString("Parent", currentPath);
                var lines = query.List();

                foreach (var line in lines)
                {
                    var fields = (object[])line;

                    list.Add(new ParamObject
                    {
                        Path = fields[0].ToString().Trim(),
                        Description = fields[1] != null ? fields[1].ToString() : null,
                        Value = fields[2] != null ? fields[2].ToString().Trim() : null,
                        HasNext = bool.Parse(fields[3].ToString())
                    });
                }
            }
            return list;
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
            if (pageSize < 0)
                pageSize = 0;
            if (pageIndex < 1)
                pageIndex = 1;

            objectTotal = 0;

            currentPath = ParamObject.NormalizePath(currentPath);

            IList<ParamObject> list = new List<ParamObject>();

            using (var uow = new NhUnitOfWork(StorageAlias))
            {
                ISession session = uow.UnderlyingContext as ISession;
                var query = session.CreateSQLQuery("SELECT Path,Description,Value,HasNext FROM KvParam WHERE Parent=:Parent");
                query.SetString("Parent", currentPath);
                query.SetMaxResults(pageSize);
                query.SetFirstResult(pageSize * (pageIndex - 1));
                var lines = query.List();

                foreach (var line in lines)
                {
                    var fields = (object[])line;

                    list.Add(new ParamObject
                    {
                        Path = fields[0].ToString().Trim(),
                        Description = fields[1] != null ? fields[1].ToString() : null,
                        Value = fields[2] != null ? fields[2].ToString().Trim() : null,
                        HasNext = bool.Parse(fields[3].ToString())
                    });
                }

                query = session.CreateSQLQuery("SELECT COUNT(*) FROM KvParam WHERE Parent=:Parent");
                query.SetString("Parent", currentPath);
                objectTotal = int.Parse(query.UniqueResult().ToString());
            }
            return list;
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
            if (string.IsNullOrWhiteSpace(path))
                return Next(path);

            path = ParamObject.NormalizePath(path);

            IList<ParamObject> list = new List<ParamObject>();

            using (var uow = new NhUnitOfWork(StorageAlias))
            {
                ISession session = uow.UnderlyingContext as ISession;
                var query = session.CreateSQLQuery("SELECT Path,Description,Value,HasNext FROM KvParam WHERE Path LIKE :Path ORDER BY Parent");
                query.SetString("Path", path + "%");
                var lines = query.List();

                foreach (var line in lines)
                {
                    var fields = (object[])line;

                    list.Add(new ParamObject
                    {
                        Path = fields[0].ToString().Trim(),
                        Description = fields[1] != null ? fields[1].ToString() : null,
                        Value = fields[2] != null ? fields[2].ToString().Trim() : null,
                        HasNext = bool.Parse(fields[3].ToString())
                    });
                }
            }

            return list;
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
            if (string.IsNullOrWhiteSpace(path))
                return Next(path, pageSize, pageIndex, out objectTotal);

            if (pageSize < 0)
                pageSize = 0;
            if (pageIndex < 1)
                pageIndex = 1;

            objectTotal = 0;

            path = ParamObject.NormalizePath(path);

            IList<ParamObject> list = new List<ParamObject>();

            using (var uow = new NhUnitOfWork(StorageAlias))
            {
                ISession session = uow.UnderlyingContext as ISession;
                var query = session.CreateSQLQuery("SELECT Path,Description,Value,HasNext FROM KvParam WHERE Path LIKE :Path ORDER BY Parent");
                query.SetString("Path", path + "%");
                query.SetMaxResults(pageSize);
                query.SetFirstResult(pageSize * (pageIndex - 1));

                var lines = query.List();

                foreach (var line in lines)
                {
                    var fields = (object[])line;

                    list.Add(new ParamObject
                    {
                        Path = fields[0].ToString().Trim(),
                        Description = fields[1] != null ? fields[1].ToString() : null,
                        Value = fields[2] != null ? fields[2].ToString().Trim() : null,
                        HasNext = bool.Parse(fields[3].ToString())
                    });
                }

                query = session.CreateSQLQuery("SELECT COUNT(*) FROM KvParam WHERE Path LIKE :Path");
                query.SetString("Path", path + "%");
                objectTotal = int.Parse(query.UniqueResult().ToString());
            }

            return list;
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
            path = ParamObject.NormalizePath(path);

            using (var uow = new NhUnitOfWork(StorageAlias))
            {
                uow.PrepareTransaction();

                ISession session = uow.UnderlyingContext as ISession;

                var query = session.CreateSQLQuery("DELETE FROM KvParam WHERE Path=:Path");
                query.SetString("Path", path);
                query.ExecuteUpdate();

                var parentPath = ParamObject.GetParentPath(path);

                if(!string.IsNullOrWhiteSpace(parentPath))
                {
                    query = session.CreateSQLQuery("SELECT COUNT(*) FROM KvParam WHERE Parent=:Parent");
                    query.SetString("Parent", parentPath);
                    if(int.Parse(query.UniqueResult().ToString())==0)
                    {
                        query = session.CreateSQLQuery("UPDATE KvParam SET HasNext=0 WHERE Path=:Path");
                        query.SetString("Path", parentPath);
                        query.ExecuteUpdate();
                    }
                }

                uow.Commit();
            }
        }
    }
}
