using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radial.Param;
using System.Xml.Linq;
using Radial.Cache;
using System.IO;
using System.Configuration;
using System.Threading;
using NHibernate;
using System.Collections;

namespace Radial.Persist.Nhs.Param
{
    /// <summary>
    /// IParam implementation using NHibernate. 
    /// </summary>
    public class NhParam : IParam
    {
        static object S_SyncRoot = new object();

        ParamItem _itemObject;

        /// <summary>
        /// The cache key.
        /// </summary>
        private readonly string CacheKey = "nhparamcache";
        /// <summary>
        /// The cache minutes (0=do not remove cache).
        /// </summary>
        private readonly int CacheMinutes = 0;

        /// <summary>
        /// Storage alias.
        /// </summary>
        private readonly string StorageAlias;

        /// <summary>
        /// Initializes a new instance of the <see cref="NhParam" /> class.
        /// </summary>
        public NhParam()
        {
            if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["NhParam.CacheKey"]))
                CacheKey = ConfigurationManager.AppSettings["NhParam.CacheKey"].Trim().ToLower();

            if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["NhParam.CacheMins"]))
            {
                if (int.TryParse(ConfigurationManager.AppSettings["NhParam.CacheMins"], out CacheMinutes))
                {
                    if (CacheMinutes < 0)
                        CacheMinutes = 0;
                }
            }

            if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["NhParam.StorageAlias"]))
                StorageAlias = ConfigurationManager.AppSettings["NhParam.StorageAlias"].Trim().ToLower();
        }

        #region Database Helper

        /// <summary>
        /// Reads from database.
        /// </summary>
        /// <returns></returns>
        private ParamItem ReadFromDatabase()
        {
            ParamItem item = null;

            using (IUnitOfWork uow = new NhUnitOfWork(StorageAlias))
            {
                ISession session = uow.UnderlyingContext as ISession;

                ISQLQuery query = session.CreateSQLQuery("SELECT XmlContent, Version FROM Param WHERE Id=:Id");
                query.SetString("Id", ParamItem.ItemId);

                IList list = query.List();

                if (list != null && list.Count == 1)
                {
                    IList olist = (IList)list[0];
                    item = new ParamItem { XmlContent = (string)olist[0], Version = (int)olist[1] };
                }
            }

            return item;
        }

        /// <summary>
        /// Writes to database.
        /// </summary>
        /// <param name="item">The item.</param>
        private void WriteToDatabase(ParamItem item)
        {
            using (IUnitOfWork uow = new NhUnitOfWork(StorageAlias))
            {
                ISession session = uow.UnderlyingContext as ISession;
                ISQLQuery query = session.CreateSQLQuery("SELECT COUNT(*) FROM Param WHERE Id=:Id");
                query.SetString("Id", ParamItem.ItemId);

                //ToString method for compatibility with different database
                bool exist = query.UniqueResult().ToString() == "1" ? true : false;

                if (!exist)
                {
                    ISQLQuery insertQuery = session.CreateSQLQuery("INSERT INTO Param (Id, XmlContent, Version) VALUES (:Id, :XmlContent, :Version)");

                    insertQuery.SetString("Id", ParamItem.ItemId);
                    insertQuery.SetParameter("XmlContent", _itemObject.XmlContent, NHibernateUtil.StringClob);
                    insertQuery.SetInt32("Version", _itemObject.Version);

                    insertQuery.ExecuteUpdate();
                }
                else
                {
                    ISQLQuery updateQuery = session.CreateSQLQuery("UPDATE Param SET XmlContent=:XmlContent, Version=:Version WHERE Id=:Id AND Version=:OldVersion");

                    int oldVersion = _itemObject.Version;
                    _itemObject.Version++;

                    updateQuery.SetString("Id", ParamItem.ItemId);
                    updateQuery.SetParameter("XmlContent", _itemObject.XmlContent, NHibernateUtil.StringClob);
                    updateQuery.SetInt32("Version", _itemObject.Version);
                    updateQuery.SetInt32("OldVersion", oldVersion);

                    int affect = updateQuery.ExecuteUpdate();

                    Checker.Requires(affect == 1, "Row was updated or deleted by another transaction");

                }
            }
        }

        #endregion

        /// <summary>
        /// Loads the root element.
        /// </summary>
        /// <returns></returns>
        private XElement LoadRootElement()
        {
            lock (S_SyncRoot)
            {
                if (_itemObject == null)
                {
                    _itemObject = RetrieveFromCache();

                    //cache empty
                    if (_itemObject == null)
                    {
                        _itemObject = ReadFromDatabase();

                        SetToCache(_itemObject);
                    }

                    //database empty
                    if (_itemObject == null)
                    {
                        _itemObject = new ParamItem();
                        
                        XDocument doc = new XDocument();
                        doc.Add(new XElement(BuildXName("params")));
                        using (MemoryStream ms = new MemoryStream())
                        using (StreamReader sr = new StreamReader(ms))
                        {
                            doc.Save(ms);
                            ms.Position = 0;
                            _itemObject.XmlContent = sr.ReadToEnd().Trim();
                        }
                    }
                }

                return XDocument.Parse(_itemObject.XmlContent).Root;
            }
        }

        /// <summary>
        /// Saves the root element.
        /// </summary>
        /// <param name="root">The root.</param>
        private void SaveRootElement(XElement root)
        {
            lock (S_SyncRoot)
            {
                using (MemoryStream ms = new MemoryStream())
                using (StreamReader sr = new StreamReader(ms))
                {
                    root.Save(ms);
                    ms.Position = 0;
                    _itemObject.XmlContent = sr.ReadToEnd().Trim();
                }

                WriteToDatabase(_itemObject);

                SetToCache(_itemObject);
            }
        }


        #region Cache

        /// <summary>
        /// Sets ParamItem to cache.
        /// </summary>
        /// <param name="item">The ParamItem item.</param>
        private void SetToCache(ParamItem item)
        {
            if (item != null)
            {
                //set entity cache
                if (CacheMinutes > 0)
                    CacheStatic.SetString(CacheKey, item.ToCacheString(), CacheMinutes * 60);
                else
                    CacheStatic.SetString(CacheKey, item.ToCacheString());
            }
        }


        /// <summary>
        /// Retrieves ParamItem from cache.
        /// </summary>
        /// <returns>The ParamItem item</returns>
        private ParamItem RetrieveFromCache()
        {
            return ParamItem.FromCacheString(CacheStatic.GetString(CacheKey));
        }

        #endregion

        /// <summary>
        /// Builds the name with xmlns.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        private static XName BuildXName(string name)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(name), "name can not be empty or null");
            XNamespace ns = ParamObject.XmlNs;
            return ns + name;
        }

        /// <summary>
        /// Loads the param object from XElement.
        /// </summary>
        /// <param name="e">The XElement.</param>
        /// <returns></returns>
        private ParamObject LoadObject(XElement e)
        {
            Checker.Parameter(e != null, "XElement can not be null");

            IList<string> pathRoutings = new List<string>();

            pathRoutings.Insert(0, ParamObject.NormalizePath(e.Attribute("name").Value));

            XElement p = e.Parent;
            while (p != null && p.Name != BuildXName("params"))
            {
                if (p.Name == BuildXName("item"))
                {
                    pathRoutings.Insert(0, ParamObject.NormalizePath(p.Attribute("name").Value));
                }
                p = p.Parent;
                continue;
            }


            ParamObject obj = new ParamObject();

            obj.Path = string.Join<string>(ParamObject.PathSeparator, pathRoutings);
            if (e.Attribute("description") != null)
                obj.Description = e.Attribute("description").Value;
            if (e.Element(BuildXName("value")) != null)
                obj.Value = e.Element(BuildXName("value")).Value;

            XElement next = e.Element(BuildXName("next"));
            if (next != null && next.Elements(BuildXName("item")).Count() > 0)
                obj.ContainsNext = true;

            return obj;
        }

        /// <summary>
        /// Gets the element.
        /// </summary>
        /// <param name="root">The root.</param>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        private XElement GetElement(XElement root, string path)
        {
            path = ParamObject.NormalizePath(path);

            string[] pathSplits = path.Split(new string[] { ParamObject.PathSeparator }, StringSplitOptions.RemoveEmptyEntries);

            XElement e = root;
            for (int i = 0; i < pathSplits.Length; i++)
            {
                e = e.Descendants(BuildXName("item"))
                    .Where(o => string.Compare(o.Attribute("name").Value, pathSplits[i].Trim(), true) == 0)
                    .SingleOrDefault();
                if (e == null)
                    break;
            }

            return e;

        }


        /// <summary>
        /// Recursives the create parent.
        /// </summary>
        /// <param name="root">The root.</param>
        /// <param name="childPath">The child path.</param>
        /// <returns></returns>
        private XElement RecursiveCreateParent(XElement root, string childPath)
        {
            Checker.Parameter(root != null, "root XElement can not be null");
            childPath = ParamObject.NormalizePath(childPath);

            string parentPath = ParamObject.GetParentPath(childPath);

            //no parent element,top level
            if (string.IsNullOrWhiteSpace(parentPath))
                return null;

            XElement pElement = GetElement(root, parentPath);

            if (pElement == null)
            {
                pElement = new XElement(BuildXName("item"), new XAttribute("name", ParamObject.GetParamName(parentPath)));

                XElement ppElement = RecursiveCreateParent(root, parentPath);

                if (ppElement != null)
                {
                    XElement ppNext = ppElement.Element(BuildXName("next"));
                    if (ppNext != null)
                        ppNext.Add(pElement);
                    else
                        ppElement.Add(new XElement(BuildXName("next"), pElement));
                }
                else
                    root.Add(pElement);
            }

            return pElement;
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
        private void Create(string path, string description, string value)
        {
            path = ParamObject.NormalizePath(path);

            ParamObject obj = new ParamObject { Path = path };

            XElement newElement = new XElement(BuildXName("item"), new XAttribute("name", ParamObject.GetParamName(path)));

            if (!string.IsNullOrWhiteSpace(description))
            {
                obj.Description = description.Trim();
                newElement.Add(new XAttribute("description", obj.Description));

            }
            if (!string.IsNullOrWhiteSpace(value))
            {
                obj.Value = value.Trim();
                newElement.Add(new XElement(BuildXName("value"), new XCData(obj.Value)));
            }


            XElement root = LoadRootElement();

            Checker.Requires(GetElement(root, path) == null, "duplicated path: \"{0}\"", path);

            XElement pElement = RecursiveCreateParent(root, path);

            if (pElement != null)
            {
                XElement pNext = pElement.Element(BuildXName("next"));
                if (pNext != null)
                    pNext.Add(newElement);
                else
                    pElement.Add(new XElement(BuildXName("next"), newElement));
            }
            else
                root.Add(newElement);

            SaveRootElement(root);

        }

        /// <summary>
        /// Update param object.
        /// </summary>
        /// <param name="path">The parameter path (case insensitive) or configuration name.</param>
        /// <param name="description">The new description.</param>
        /// <param name="value">The new value.</param>
        private void Update(string path, string description, string value)
        {
            path = ParamObject.NormalizePath(path);

            XElement root = LoadRootElement();

            XElement e = GetElement(root, path);

            Checker.Requires(e != null, "can not find path: \"{0}\"", path);

            ParamObject obj = new ParamObject { Path = path };

            if (!string.IsNullOrWhiteSpace(description))
                obj.Description = description.Trim();
            else
                obj.Description = string.Empty;

            e.SetAttributeValue("description", obj.Description);

            if (!string.IsNullOrWhiteSpace(value))
                obj.Value = value.Trim();
            else
                obj.Value = string.Empty;

            if (e.Element(BuildXName("value")) != null)
                e.Element(BuildXName("value")).Remove();
            e.Add(new XElement(BuildXName("value"), new XCData(obj.Value)));

            SaveRootElement(root);

        }

        #region IParam Members

        /// <summary>
        /// Determine whether the specified param object is exists.
        /// </summary>
        /// <param name="path">The parameter path (case insensitive) or configuration name.</param>
        /// <returns>
        ///   <c>true</c> if the specified path is exists; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool Exists(string path)
        {
            path = ParamObject.NormalizePath(path);

            XElement root = LoadRootElement();
            return GetElement(root, path) != null;
        }

        /// <summary>
        /// Get param object.
        /// </summary>
        /// <param name="path">The parameter path (case insensitive) or configuration name.</param>
        /// <returns>
        /// If path exists, return the object, otherwise return null.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public ParamObject Get(string path)
        {
            path = ParamObject.NormalizePath(path);

            XElement root = LoadRootElement();
            XElement e = GetElement(root, path);

            if (e != null)
            {
                ParamObject obj = LoadObject(e);
                return obj;
            }

            return null;


        }

        /// <summary>
        /// Get the param value.
        /// </summary>
        /// <param name="path">The parameter path (case insensitive) or configuration name.</param>
        /// <returns>
        /// If path exists, return its value, otherwise return string.Empty.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public string GetValue(string path)
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
        /// <exception cref="System.NotImplementedException"></exception>
        public IList<ParamObject> Next(string currentPath)
        {
            IList<ParamObject> list = new List<ParamObject>();

            ParamObject obj = null;

            XElement root = LoadRootElement();

            if (string.IsNullOrWhiteSpace(currentPath))
            {
                foreach (XElement e in root.Elements(BuildXName("item")).OrderBy(o => o.Attribute("name").Value))
                {
                    obj = LoadObject(e);

                    list.Add(obj);
                }
            }
            else
            {
                currentPath = ParamObject.NormalizePath(currentPath);

                XElement e = GetElement(root, currentPath);

                if (e != null)
                {
                    XElement next = e.Element(BuildXName("next"));

                    if (next != null)
                    {
                        foreach (XElement c in next.Elements(BuildXName("item")).OrderBy(o => o.Attribute("name").Value))
                        {
                            obj = LoadObject(c);

                            list.Add(obj);
                        }
                    }
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
        /// <exception cref="System.NotImplementedException"></exception>
        public IList<ParamObject> Next(string currentPath, int pageSize, int pageIndex, out int objectTotal)
        {
            if (pageSize < 0)
                pageSize = 0;
            if (pageIndex < 1)
                pageIndex = 1;

            objectTotal = 0;

            IList<ParamObject> list = new List<ParamObject>();

            ParamObject obj = null;

            XElement root = LoadRootElement();

            if (string.IsNullOrWhiteSpace(currentPath))
            {

                objectTotal = root.Elements(BuildXName("item")).Count();

                foreach (XElement e in root.Elements(BuildXName("item")).OrderBy(o => o.Attribute("name").Value).Take(pageSize).Skip(pageSize * (pageIndex - 1)))
                {
                    obj = LoadObject(e);

                    list.Add(obj);
                }
            }
            else
            {
                currentPath = ParamObject.NormalizePath(currentPath);


                XElement e = GetElement(root, currentPath);

                if (e != null)
                {
                    XElement next = e.Element(BuildXName("next"));

                    if (next != null)
                    {
                        objectTotal = next.Elements(BuildXName("item")).Count();

                        foreach (XElement c in next.Elements(BuildXName("item")).OrderBy(o => o.Attribute("name").Value).Take(pageSize).Skip(pageSize * (pageIndex - 1)))
                        {
                            obj = LoadObject(c);

                            list.Add(obj);
                        }
                    }
                }
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
        /// <exception cref="System.NotImplementedException"></exception>
        public IList<ParamObject> Search(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return Next(path);

            path = ParamObject.NormalizePath(path);

            IList<ParamObject> list = new List<ParamObject>();

            XElement root = LoadRootElement();

            foreach (XElement e in root.Descendants(BuildXName("item")).Where(o => o.Attribute("name").Value.StartsWith(path, StringComparison.OrdinalIgnoreCase)).OrderBy(o => o.Attribute("name").Value))
            {
                ParamObject obj = LoadObject(e);

                list.Add(obj);
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
        /// <exception cref="System.NotImplementedException"></exception>
        public IList<ParamObject> Search(string path, int pageSize, int pageIndex, out int objectTotal)
        {
            if (string.IsNullOrWhiteSpace(path))
                return Next(path, pageSize, pageIndex, out objectTotal);

            if (pageSize < 0)
                pageSize = 0;
            if (pageIndex < 1)
                pageIndex = 1;

            path = ParamObject.NormalizePath(path);

            IList<ParamObject> list = new List<ParamObject>();

            XElement root = LoadRootElement();

            objectTotal = root.Descendants(BuildXName("item")).Where(o => o.Attribute("name").Value.StartsWith(path, StringComparison.OrdinalIgnoreCase)).Count();

            foreach (XElement e in root.Descendants(BuildXName("item")).Where(o => o.Attribute("name").Value.StartsWith(path)).OrderBy(o => o.Attribute("name").Value).Take(pageSize).Skip(pageSize * (pageIndex - 1)))
            {
                ParamObject obj = LoadObject(e);

                list.Add(obj);
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
            Save(path, string.Empty, value);
        }

        /// <summary>
        /// Save param object.
        /// </summary>
        /// <param name="path">The parameter path (case insensitive) or configuration name.</param>
        /// <param name="description">The description.</param>
        /// <param name="value">The value.</param>
        public void Save(string path, string description, string value)
        {
            if (!Exists(path))
                Create(path, description, value);
            else
                Update(path, description, value);
        }

        /// <summary>
        /// Delete param object.
        /// </summary>
        /// <param name="path">The parameter path (case insensitive) or configuration name.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Delete(string path)
        {
            path = ParamObject.NormalizePath(path);

            XElement root = LoadRootElement();

            XElement e = GetElement(root, path);

            if (e != null)
            {
                XElement next = e.Element(BuildXName("next"));
                Checker.Requires(next == null || next.Elements(BuildXName("item")).Count() == 0, "can not delete \"{0}\" because it contains some next level objects", path);

                e.Remove();

                string parentPath = ParamObject.GetParentPath(path);

                if (!string.IsNullOrWhiteSpace(parentPath))
                {
                    XElement pElement = GetElement(root, parentPath);

                    if (pElement != null)
                    {
                        next = pElement.Element(BuildXName("next"));

                        if (next != null && next.Elements(BuildXName("item")).Count() == 0)
                            next.Remove();
                    }
                }

                SaveRootElement(root);
            }
        }

        #endregion
    }
}
