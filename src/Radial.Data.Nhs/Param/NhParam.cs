using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radial.Param;
using System.Xml.Linq;
using Radial.Cache;
using System.IO;

namespace Radial.Data.Nhs.Param
{
    /// <summary>
    /// IParam implement using NHibernate. 
    /// </summary>
    public class NhParam : IParam
    {
        static object S_SyncRoot = new object();
        const string Xmlns = "urn:radial-xmlparam";

        /// <summary>
        /// The cache key.
        /// </summary>
        public const string CacheKey = "NhParamCache";

        /// <summary>
        /// Loads the root element.
        /// </summary>
        private XElement LoadRootElement()
        {
            string sha1;
            return LoadRootElement(out sha1);
        }

        /// <summary>
        /// Loads the root element.
        /// </summary>
        /// <param name="originalSHA1">the original sha1.</param>
        /// <returns></returns>
        private XElement LoadRootElement(out string originalSHA1)
        {
            lock (S_SyncRoot)
            {
                XDocument doc = new XDocument();

                ParamEntity entity = ParamEntity.FromCacheString(CacheStatic.Get<string>(CacheKey));

                if (entity == null)
                {
                    using (IUnitOfWork uow = new NhUnitOfWork())
                    {
                        ParamRepository paramRepo = new ParamRepository(uow);
                        entity = paramRepo.Find(ParamEntity.EntityId);
                        if (entity == null || string.IsNullOrWhiteSpace(entity.XmlContent))
                        {
                            doc.Add(new XElement(BuildXName("params")));

                            string xmlContent = string.Empty;
                            using (MemoryStream ms = new MemoryStream())
                            using (StreamReader sr = new StreamReader(ms))
                            {
                                doc.Root.Save(ms);
                                ms.Position = 0;
                                xmlContent = sr.ReadToEnd().Trim();
                            }

                            originalSHA1 = Radial.Security.CryptoProvider.SHA1Encrypt(xmlContent);

                            entity = new ParamEntity { XmlContent = xmlContent, Sha1 = originalSHA1 };
                        }
                        else
                        {
                            originalSHA1 = entity.Sha1;
                            doc = XDocument.Parse(entity.XmlContent);
                        }

                        //set entity cache
                        CacheStatic.Set<string>(CacheKey, entity.ToCacheString());
                    }
                }
                else
                {
                    originalSHA1 = entity.Sha1;
                    doc = XDocument.Parse(entity.XmlContent);
                }

                return doc.Root;
            }
        }

        /// <summary>
        /// Saves the root element.
        /// </summary>
        /// <param name="root">The root.</param>
        /// <param name="originalSHA1">The original sha1.</param>
        private void SaveRootElement(XElement root, string originalSHA1)
        {
            lock (S_SyncRoot)
            {
                string xmlContent = string.Empty;
                using (MemoryStream ms = new MemoryStream())
                using (StreamReader sr = new StreamReader(ms))
                {
                    root.Save(ms);
                    ms.Position = 0;
                    xmlContent = sr.ReadToEnd().Trim();
                }

                string newSHA1 = Radial.Security.CryptoProvider.SHA1Encrypt(xmlContent);

                ParamEntity entity;

                using (IUnitOfWork uow = new NhUnitOfWork())
                {
                    ParamRepository paramRepo = new ParamRepository(uow);

                    entity = paramRepo.Find(ParamEntity.EntityId);

                    if (entity != null)
                    {
                        Checker.Requires(string.Compare(entity.Sha1, originalSHA1, true) == 0, "parameter entity conflict, probably has been modified in another program");

                        entity.XmlContent = xmlContent;
                        entity.Sha1 = newSHA1;
                    }
                    else
                        entity = new ParamEntity { XmlContent = xmlContent, Sha1 = newSHA1 };


                    uow.RegisterSave<ParamEntity>(entity);

                    uow.Commit(true);
                }

                CacheStatic.Set<string>(CacheKey, entity.ToCacheString());
            }
        }

        /// <summary>
        /// Builds the name with xmlns.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        private static XName BuildXName(string name)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(name), "name can not be empty or null");
            XNamespace ns = Xmlns;
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

            lock (S_SyncRoot)
            {
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

            lock (S_SyncRoot)
            {
                string originalSHA1;
                XElement root = LoadRootElement(out originalSHA1);

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

                SaveRootElement(root, originalSHA1);
            }
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

            lock (S_SyncRoot)
            {
                string originalSHA1;
                XElement root = LoadRootElement(out originalSHA1);

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

                SaveRootElement(root, originalSHA1);
            }
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

            lock (S_SyncRoot)
            {
                XElement root = LoadRootElement();
                return GetElement(root, path) != null;
            }
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

            lock (S_SyncRoot)
            {
                XElement root = LoadRootElement();
                XElement e = GetElement(root, path);

                if (e != null)
                {
                    ParamObject obj = LoadObject(e);
                    return obj;
                }

                return null;

            }
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

            lock (S_SyncRoot)
            {
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
            Checker.Parameter(pageSize >= 0, "pageSize must be greater than or equal to 0");
            Checker.Parameter(pageIndex >= 1, "pageIndex must be greater than or equal to 1");

            objectTotal = 0;

            IList<ParamObject> list = new List<ParamObject>();

            lock (S_SyncRoot)
            {
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

            lock (S_SyncRoot)
            {
                XElement root = LoadRootElement();

                foreach (XElement e in root.Descendants(BuildXName("item")).Where(o => o.Attribute("name").Value.StartsWith(path, StringComparison.OrdinalIgnoreCase)).OrderBy(o => o.Attribute("name").Value))
                {
                    ParamObject obj = LoadObject(e);

                    list.Add(obj);
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
        /// <exception cref="System.NotImplementedException"></exception>
        public IList<ParamObject> Search(string path, int pageSize, int pageIndex, out int objectTotal)
        {
            if (string.IsNullOrWhiteSpace(path))
                return Next(path, pageSize, pageIndex, out objectTotal);

            Checker.Parameter(pageSize >= 0, "pageSize must be greater than or equal to 0");
            Checker.Parameter(pageIndex >= 1, "pageIndex must be greater than or equal to 1");

            path = ParamObject.NormalizePath(path);

            IList<ParamObject> list = new List<ParamObject>();

            lock (S_SyncRoot)
            {
                XElement root = LoadRootElement();

                objectTotal = root.Descendants(BuildXName("item")).Where(o => o.Attribute("name").Value.StartsWith(path, StringComparison.OrdinalIgnoreCase)).Count();

                foreach (XElement e in root.Descendants(BuildXName("item")).Where(o => o.Attribute("name").Value.StartsWith(path)).OrderBy(o => o.Attribute("name").Value).Take(pageSize).Skip(pageSize * (pageIndex - 1)))
                {
                    ParamObject obj = LoadObject(e);

                    list.Add(obj);
                }
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

            lock (S_SyncRoot)
            {
                string originalSHA1;
                XElement root = LoadRootElement(out originalSHA1);

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

                    SaveRootElement(root, originalSHA1);
                }
            }
        }

        #endregion
    }
}
