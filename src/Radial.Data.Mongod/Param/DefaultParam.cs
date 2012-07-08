using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radial.Param;
using System.Xml.Linq;
using Radial.Cache;
using System.IO;
using System.Xml;

namespace Radial.Data.Mongod.Param
{
    /// <summary>
    /// Default MongoDB param implement.
    /// </summary>
    public class DefaultParam : IParam
    {

        MongoParamRepository _repository;
        static object SyncRoot = new object();
        const string Xmlns = "urn:radial-xmlparam";


        /// <summary>
        /// The cache key.
        /// </summary>
        public const string CacheKey = "MongoParamCache";

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultParam"/> class.
        /// </summary>
        public DefaultParam()
        {
            _repository = new MongoParamRepository();
        }

        /// <summary>
        /// Loads the root element.
        /// </summary>
        /// <returns></returns>
        private XElement LoadRootElement()
        {
            XDocument doc = new XDocument();

            string content = CacheStatic.Get<string>(CacheKey);

            if (string.IsNullOrWhiteSpace(content))
            {
                ItemEntity entity = _repository.Get(ItemEntity.EntityId);
                if (entity == null)
                    doc.Add(new XElement(BuildXName("params")));
                else
                    doc = XDocument.Parse(entity.Content.Trim());
            }
            else
                doc = XDocument.Parse(content);

            return doc.Root;
        }

        /// <summary>
        /// Saves the root element.
        /// </summary>
        /// <param name="root">The root.</param>
        private void SaveRootElement(XElement root)
        {
            using (MemoryStream ms = new MemoryStream())
            using (StreamReader sr = new StreamReader(ms))
            {
                root.Save(ms);
                string content = sr.ReadToEnd();

                CacheStatic.Set<string>(CacheKey, content);

                ItemEntity entity = _repository.Get(ItemEntity.EntityId);

                if (entity == null)
                    entity = new ItemEntity { Content = content };
                else
                    entity.Content = content;

                _repository.Save(entity);
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
            Checker.Parameter(e != null, "xelement can not be null");

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
        /// <param name="path">The path.</param>
        /// <returns></returns>
        private XElement GetElement(string path)
        {
            path = ParamObject.NormalizePath(path);

            lock (SyncRoot)
            {
                string[] pathSplits = path.Split(new string[] { ParamObject.PathSeparator }, StringSplitOptions.RemoveEmptyEntries);

                XElement e = LoadRootElement();
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

        #region IParam members

        /// <summary>
        /// Determine whether the specified param object is exists.
        /// </summary>
        /// <param name="path">The parameter path (case insensitive) or configuration name.</param>
        /// <returns>
        ///   <c>true</c> if the specified path is exists; otherwise, <c>false</c>.
        /// </returns>
        public bool Exist(string path)
        {
            lock (SyncRoot)
            {
                path = ParamObject.NormalizePath(path);
                return GetElement(path) != null;
            }
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

            lock (SyncRoot)
            {
                XElement e = GetElement(path);

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
        public IList<ParamObject> Next(string currentPath)
        {
            IList<ParamObject> list = new List<ParamObject>();


            lock (SyncRoot)
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

                    XElement e = GetElement(currentPath);

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
        public IList<ParamObject> Next(string currentPath, int pageSize, int pageIndex, out int objectTotal)
        {
            Checker.Parameter(pageSize >= 0, "pageSize must be greater than or equal to 0");
            Checker.Parameter(pageIndex >= 1, "pageIndex must be greater than or equal to 1");

            objectTotal = 0;

            IList<ParamObject> list = new List<ParamObject>();

            lock (SyncRoot)
            {
                ParamObject obj = null;

                if (string.IsNullOrWhiteSpace(currentPath))
                {
                    XElement root = LoadRootElement();

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


                    XElement e = GetElement(currentPath);

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
        public IList<ParamObject> Search(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return Next(path);

            path = ParamObject.NormalizePath(path);

            IList<ParamObject> list = new List<ParamObject>();

            lock (SyncRoot)
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
        public IList<ParamObject> Search(string path, int pageSize, int pageIndex, out int objectTotal)
        {
            if (string.IsNullOrWhiteSpace(path))
                return Next(path, pageSize, pageIndex, out objectTotal);

            path = ParamObject.NormalizePath(path);

            IList<ParamObject> list = new List<ParamObject>();

            lock (SyncRoot)
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
        /// Create new param object.
        /// </summary>
        /// <param name="path">The parameter path (case insensitive) or configuration name.</param>
        /// <param name="description">The description.</param>
        /// <param name="value">The value.</param>
        /// <returns>
        /// If successful created, return param object.
        /// </returns>
        public ParamObject Create(string path, string description, string value)
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

            lock (SyncRoot)
            {
                XElement root = LoadRootElement();

                Checker.Requires(!Exist(path), "duplicated path: \"{0}\"", path);

                string parentPath = ParamObject.GetParentPath(path);

                if (!string.IsNullOrWhiteSpace(parentPath))
                {
                    XElement pElement = GetElement(parentPath);

                    Checker.Requires(pElement != null, "parent path \"{0}\" does not exist", parentPath);

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

            return obj;
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
        public ParamObject Update(string path, string description, string value)
        {
            path = ParamObject.NormalizePath(path);

            lock (SyncRoot)
            {
                XElement root = LoadRootElement();

                XElement e = GetElement(path);

                Checker.Requires(e != null, "can not find path: \"{0}\"", path);

                ParamObject obj = new ParamObject { Path = path };

                if (!string.IsNullOrWhiteSpace(description))
                {
                    obj.Description = description.Trim();
                }
                else
                    obj.Description = string.Empty;

                e.SetAttributeValue("description", obj.Description);

                if (!string.IsNullOrWhiteSpace(value))
                {
                    obj.Value = value.Trim();
                }
                else
                    obj.Value = string.Empty;

                if (e.Element(BuildXName("value")) != null)
                    e.Element(BuildXName("value")).Remove();
                e.Add(new XElement(BuildXName("value"), new XCData(obj.Value)));

                SaveRootElement(root);

                return obj;
            }
        }

        /// <summary>
        /// Delete param object.
        /// </summary>
        /// <param name="path">The parameter path (case insensitive) or configuration name.</param>
        public void Delete(string path)
        {
            path = ParamObject.NormalizePath(path);

            lock (SyncRoot)
            {
                XElement root = LoadRootElement();

                XElement e = GetElement(path);

                if (e != null)
                {
                    XElement next = e.Element(BuildXName("next"));
                    Checker.Requires(next == null || next.Elements(BuildXName("item")).Count() == 0, "can not delete \"{0}\" because it contains some next level objects", path);

                    e.Remove();

                    string parentPath = ParamObject.GetParentPath(path);

                    if (!string.IsNullOrWhiteSpace(parentPath))
                    {
                        XElement pElement = GetElement(parentPath);

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
        }

        #endregion
    }
}
