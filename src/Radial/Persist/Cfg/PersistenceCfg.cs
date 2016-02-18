using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Radial.Persist.Cfg
{
    /// <summary>
    /// PersistenceCfg.
    /// </summary>
    public sealed class PersistenceCfg
    {
        const string Xmlns = "urn:radial-persistence";

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
        /// Gets or sets the databases.
        /// </summary>
        public DatabaseCfgSet Databases { get; set; }

        /// <summary>
        /// Gets or sets the objects.
        /// </summary>
        public ObjectCfgSet Objects { get; set; }

        /// <summary>
        /// Gets or sets the storages.
        /// </summary>
        public StorageCfgSet Storages { get; set; }


        /// <summary>
        /// Loads the specified file path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        public static PersistenceCfg Load(string filePath)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(filePath), "file path can not be empty or null");

            PersistenceCfg cfg = new PersistenceCfg();
            cfg.Databases = new DatabaseCfgSet();
            cfg.Objects = new ObjectCfgSet();
            cfg.Storages = new StorageCfgSet();

            XDocument doc = XDocument.Load(filePath);
            XElement root = doc.Element(BuildXName("persistence"));

            #region databases

            var dbroot = root.Element(BuildXName("databases"));

            Checker.Requires(dbroot!=null, "<databases> element is required.");

            var dbxs = dbroot.Elements(BuildXName("database"));

            Checker.Requires(dbxs.Count() > 0, "can not find any <database> element.");

            foreach (var dbx in dbxs)
            {
                DatabaseCfgItem item = new DatabaseCfgItem();

                Checker.Requires(dbx.Attribute("name") != null && !string.IsNullOrWhiteSpace(dbx.Attribute("name").Value),
                    "<database> must set name attribute value");
                item.Name = dbx.Attribute("name").Value.Trim();
                var dbsx = dbx.Element(BuildXName("setting"));
                Checker.Requires(dbsx != null, "<database> must contains <setting> element");
                Checker.Requires(dbsx.Attribute("type") != null && !string.IsNullOrWhiteSpace(dbsx.Attribute("type").Value),
                    "<setting> must set type attribute value");
                Checker.Requires(!string.IsNullOrWhiteSpace(dbsx.Value), "<setting> element must set value");
                switch (dbsx.Attribute("type").Value.Trim().ToLower())
                {
                    case "file":
                        item.SettingType = DatabaseSettingType.File;
                        if (Radial.Web.HttpKits.IsWebApp)
                            item.SettingValue = Radial.Web.HttpKits.MakeAbsoluteUrl(dbsx.Value);
                        else
                            item.SettingValue = GlobalVariables.GetPath(dbsx.Value);
                        break;
                    case "text": item.SettingType = DatabaseSettingType.Text; item.SettingValue = dbsx.Value; break;
                    default: throw new NotSupportedException("unknown type string: " + dbsx.Attribute("type").Value.Trim());
                }

                cfg.Databases.Add(item);
            }

            #endregion

            #region objects

            var obroot = root.Element(BuildXName("objects"));

            Checker.Requires(obroot!=null, "<objects> element is required.");

            var clxs = obroot.Elements(BuildXName("class"));

            Checker.Requires(clxs.Count() > 0, "can not find any <class> element.");

            foreach (var clx in clxs)
            {
                ObjectCfgItem item = new ObjectCfgItem();

                Checker.Requires(clx.Attribute("name") != null && !string.IsNullOrWhiteSpace(clx.Attribute("name").Value),
                    "<class> must set name attribute value");
                item.ClassName = clx.Attribute("name").Value.Trim();
                Checker.Requires(clx.Attribute("type") != null && !string.IsNullOrWhiteSpace(clx.Attribute("type").Value),
                    "<class> must set type attribute value");
                item.ClassType = Type.GetType(clx.Attribute("type").Value);

                if (clx.Attribute("routing") != null && !string.IsNullOrWhiteSpace(clx.Attribute("routing").Value))
                    item.RouterType = Type.GetType(clx.Attribute("routing").Value);

                var mpsx = clx.Element(BuildXName("mapping"));
                Checker.Requires(mpsx != null, "<class> must contains <mapping> element");
                Checker.Requires(mpsx.Attribute("type") != null && !string.IsNullOrWhiteSpace(mpsx.Attribute("type").Value),
                    "<mapping> must set type attribute value");
                Checker.Requires(!string.IsNullOrWhiteSpace(mpsx.Value), "<mapping> element must set value");
                switch (mpsx.Attribute("type").Value.Trim().ToLower())
                {
                    case "file":
                        item.MappingType = ClassMappingType.File;
                        if (Radial.Web.HttpKits.IsWebApp)
                            item.MappingValue = Radial.Web.HttpKits.MakeAbsoluteUrl(mpsx.Value);
                        else
                            item.MappingValue = GlobalVariables.GetPath(mpsx.Value);
                        break;
                    case "text": item.MappingType = ClassMappingType.Text; item.MappingValue = mpsx.Value; break;
                    default: throw new NotSupportedException("unknown type string: " + mpsx.Attribute("type").Value.Trim());
                }

                cfg.Objects.Add(item);
            }

            #endregion

            #region storages

            var stroot = root.Element(BuildXName("storages"));

            Checker.Requires(stroot!=null, "<storages> element is required.");

            var stxs = stroot.Elements(BuildXName("storage"));

            Checker.Requires(stxs.Count() > 0, "can not find any <storage> element.");

            foreach (var stx in stxs)
            {
                StorageCfgItem item = new StorageCfgItem();
                Checker.Requires(stx.Attribute("alias") != null && !string.IsNullOrWhiteSpace(stx.Attribute("alias").Value),
                "<storage> must set alias attribute value");
                item.Alias = stx.Attribute("alias").Value.Trim();
                Checker.Requires(stx.Attribute("database") != null && !string.IsNullOrWhiteSpace(stx.Attribute("database").Value),
                "<storage> must set database attribute value");
                item.DatabaseName = stx.Attribute("database").Value.Trim();

                Checker.Requires(cfg.Databases[item.DatabaseName] != null, "can not find database config name: {0}", item.DatabaseName);

                var cox = stx.Element(BuildXName("contains"));

                Checker.Requires(cox!=null, "<contains> element is required.");

                var obxs = cox.Elements(BuildXName("object"));

                Checker.Requires(obxs.Count() > 0, "can not find any <object> element.");

                item.ContainedObjects = new ContainedObjectCfgSet();

                foreach(var obx in obxs)
                {
                    ContainedObjectCfgItem citem = new ContainedObjectCfgItem();
                    citem.MappingReplaces = new Dictionary<string, string>();
                    Checker.Requires(obx.Attribute("class") != null && !string.IsNullOrWhiteSpace(obx.Attribute("class").Value),
                        "<object> must set class attribute value");
                    citem.ClassName = obx.Attribute("class").Value.Trim();

                    Checker.Requires(cfg.Objects[citem.ClassName] != null, "can not find object config name: {0}", citem.ClassName);

                    var mpx = obx.Element(BuildXName("mapping"));
                    if (mpx != null)
                    {
                        var rpxs = mpx.Elements(BuildXName("replace"));

                        foreach (var rpx in rpxs)
                        {
                            Checker.Requires(rpx.Attribute("key") != null && !string.IsNullOrWhiteSpace(rpx.Attribute("key").Value),
                                "<replace> must set key attribute value");
                            Checker.Requires(rpx.Attribute("value") != null && !string.IsNullOrWhiteSpace(rpx.Attribute("value").Value),
                                "<replace> must set value attribute value");

                            citem.MappingReplaces.Add(rpx.Attribute("key").Value.Trim(), rpx.Attribute("value").Value.Trim());
                        }
                    }

                    item.ContainedObjects.Add(citem);
                }

                cfg.Storages.Add(item);
            }

            #endregion

            Checker.Requires(cfg.Databases.Count > 0 && cfg.Objects.Count > 0 && cfg.Storages.Count > 0, "persistence config error");

            return cfg;
        }
    }
}
