using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Linq;

namespace Radial.Param
{
    /// <summary>
    /// Xml param class.
    /// </summary>
    public class XmlParam : IParam
    {

        /// <summary>
        /// The Xml namespace.
        /// </summary>
        public const string XmlNs = "urn:jr-trc-xmlparam";

        static object SyncRoot = new object();

        static XElement S_Root;


        /// <summary>
        /// Initializes a new instance of the <see cref="XmlParam"/> class.
        /// </summary>
        static XmlParam()
        {
            Initial(ConfigurationPath);
            FileWatcher.CreateMonitor(ConfigurationPath, Initial);
        }


        /// <summary>
        /// Gets the configuration path.
        /// </summary>
        private static string ConfigurationPath
        {
            get
            {
                return GlobalVariables.GetConfigPath("XmlParam.config");
            }
        }

        /// <summary>
        /// Initials the specified config file path.
        /// </summary>
        /// <param name="configFilePath">The config file path.</param>
        private static void Initial(string configFilePath)
        {

            lock (SyncRoot)
            {
                XDocument doc = null;
                if (!File.Exists(configFilePath))
                {
                    Logger.Get<XmlParam>().Debug("create new xmlparam settings");
                    doc = new XDocument();
                    doc.Add(new XElement(BuildXName("params")));
                    doc.Save(configFilePath);
                }
                else
                {
                    Logger.Get<XmlParam>().Debug("load exists xmlparam settings");
                    doc = XDocument.Load(configFilePath);
                }

                S_Root = doc.Root;
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
            XNamespace ns = XmlNs;
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
                obj.HasNext = true;

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

            string[] pathSplits = path.Split(new string[] { ParamObject.PathSeparator }, StringSplitOptions.RemoveEmptyEntries);

            XElement e = S_Root;
            for (int i = 0; i < pathSplits.Length; i++)
            {
                var next = e.Element(BuildXName("next"));

                if (next == null)
                {
                    //no next element
                    e = e.Elements(BuildXName("item"))
                        .Where(o => string.Compare(o.Attribute("name").Value, pathSplits[i].Trim(), true) == 0).FirstOrDefault();
                }
                else
                {
                    //has next element
                    e = next.Elements(BuildXName("item"))
                        .Where(o => string.Compare(o.Attribute("name").Value, pathSplits[i].Trim(), true) == 0).FirstOrDefault();
                }
                if (e == null)
                    break;
            }

            return e;

        }

        /// <summary>
        /// Recursives the create parent.
        /// </summary>
        /// <param name="childPath">The child path.</param>
        /// <returns></returns>
        private XElement RecursiveCreateParent(string childPath)
        {
            childPath = ParamObject.NormalizePath(childPath);

            string parentPath = ParamObject.GetParentPath(childPath);

            //no parent element,top level
            if (string.IsNullOrWhiteSpace(parentPath))
                return null;

            XElement pElement = GetElement(parentPath);

            if (pElement == null)
            {
                pElement = new XElement(BuildXName("item"), new XAttribute("name", ParamObject.GetParamName(parentPath)));

                XElement ppElement = RecursiveCreateParent(parentPath);

                if (ppElement != null)
                {
                    XElement ppNext = ppElement.Element(BuildXName("next"));
                    if (ppNext != null)
                        ppNext.Add(pElement);
                    else
                        ppElement.Add(new XElement(BuildXName("next"), pElement));
                }
                else
                    S_Root.Add(pElement);
            }

            return pElement;
        }


        /// <summary>
        /// Create new param object.
        /// </summary>
        /// <param name="path">The parameter path (case insensitive) or configuration name.</param>
        /// <param name="description">The description.</param>
        /// <param name="value">The value.</param>
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

            lock (SyncRoot)
            {
                Checker.Requires(!Exists(path), "duplicated path: \"{0}\"", path);

                XElement pElement = RecursiveCreateParent(path);

                if (pElement != null)
                {
                    XElement pNext = pElement.Element(BuildXName("next"));
                    if (pNext != null)
                        pNext.Add(newElement);
                    else
                        pElement.Add(new XElement(BuildXName("next"), newElement));
                }
                else
                    S_Root.Add(newElement);

                S_Root.Save(ConfigurationPath);
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

            lock (SyncRoot)
            {
                XElement e = GetElement(path);

                Checker.Requires(e != null, "can not find path: \"{0}\"", path);

                ParamObject obj = new ParamObject { Path = path };

                if (!string.IsNullOrWhiteSpace(description))
                {
                    obj.Description = description.Trim();
                }
                else
                    obj.Description = null;

                e.SetAttributeValue("description", obj.Description);

                if (!string.IsNullOrWhiteSpace(value))
                {
                    obj.Value = value.Trim();
                }
                else
                    obj.Value = null;

                if (e.Element(BuildXName("value")) != null)
                    e.Element(BuildXName("value")).Remove();

                if (obj.Value != null)
                    e.Add(new XElement(BuildXName("value"), new XCData(obj.Value)));

                S_Root.Save(ConfigurationPath);

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
        public bool Exists(string path)
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
        /// If path exists, return its value, otherwise return null.
        /// </returns>
        public string GetValue(string path)
        {
            ParamObject o = Get(path);

            if (o == null)
                return null;

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

                if (string.IsNullOrWhiteSpace(currentPath))
                {
                    foreach (XElement e in S_Root.Elements(BuildXName("item")).OrderBy(o => o.Attribute("name").Value))
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
            if (pageSize < 0)
                pageSize = 0;
            if (pageIndex < 1)
                pageIndex = 1;

            objectTotal = 0;

            IList<ParamObject> list = new List<ParamObject>();

            lock (SyncRoot)
            {
                ParamObject obj = null;

                if (string.IsNullOrWhiteSpace(currentPath))
                {
                    objectTotal = S_Root.Elements(BuildXName("item")).Count();

                    foreach (XElement e in S_Root.Elements(BuildXName("item")).OrderBy(o => o.Attribute("name").Value).Take(pageSize).Skip(pageSize * (pageIndex - 1)))
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

                foreach (XElement e in S_Root.Descendants(BuildXName("item")).Where(o => o.Attribute("name").Value.StartsWith(path, StringComparison.OrdinalIgnoreCase)).OrderBy(o => o.Attribute("name").Value))
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

            if (pageSize < 0)
                pageSize = 0;
            if (pageIndex < 1)
                pageIndex = 1;

            path = ParamObject.NormalizePath(path);

            IList<ParamObject> list = new List<ParamObject>();

            lock (SyncRoot)
            {
                objectTotal = S_Root.Descendants(BuildXName("item")).Where(o => o.Attribute("name").Value.StartsWith(path, StringComparison.OrdinalIgnoreCase)).Count();

                foreach (XElement e in S_Root.Descendants(BuildXName("item")).Where(o => o.Attribute("name").Value.StartsWith(path)).OrderBy(o => o.Attribute("name").Value).Take(pageSize).Skip(pageSize * (pageIndex - 1)))
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
            if (!Exists(path))
                Create(path, description, value);
            else
                Update(path, description, value);
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

                    S_Root.Save(ConfigurationPath);
                }

            }
        }



        #endregion



    }
}
