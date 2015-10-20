﻿using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Collections.Specialized;

namespace Radial.Param
{
    /// <summary>
    /// ConfigurationManager param class.
    /// </summary>
    public sealed class ConfigurationParam : IParam
    {
        /// <summary>
        /// Normalizes the app settings.
        /// </summary>
        /// <returns></returns>
        private NameValueCollection NormalizeAppSettings()
        {
            NameValueCollection collection = new NameValueCollection();

            foreach (string key in ConfigurationManager.AppSettings.AllKeys)
            {
                string newKey = key.Trim().ToLower();
                Checker.Requires(!collection.AllKeys.Contains(newKey), "{0} already in app settings", key.Trim());
                collection.Add(newKey, ConfigurationManager.AppSettings[key]);
            }

            return collection;
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
            Checker.Parameter(!string.IsNullOrWhiteSpace(path), "path can not be empty or null");

            path = path.Trim().ToLower();

            NameValueCollection settings = NormalizeAppSettings();

            return settings.AllKeys.Contains(path);
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
            Checker.Parameter(!string.IsNullOrWhiteSpace(path), "path can not be empty or null");

            path = path.Trim().ToLower();

            NameValueCollection settings = NormalizeAppSettings();

            foreach (string key in settings.AllKeys)
            {
                if (string.Compare(key.Trim(), path, true) == 0)
                {
                    return new ParamObject { Path = path, Value = settings[path] };
                }
            }

            return null;
        }

        /// <summary>
        /// Get the param value.
        /// </summary>
        /// <param name="path">The parameter path (case insensitive) or configuration name.</param>
        /// <returns>
        /// /// <returns>If path exists, return its value, otherwise return null.</returns>
        /// </returns>
        public string GetValue(string path)
        {
            ParamObject p = Get(path);
            if (p != null)
                return null;
            return string.Empty;
        }

        /// <summary>
        /// Create new param object.
        /// </summary>
        /// <param name="path">The parameter path (case insensitive) or configuration name.</param>
        /// <param name="description">The description.</param>
        /// <param name="value">The value.</param>
        private void Create(string path, string description, string value)
        {
        }

        /// <summary>
        /// Update param object.
        /// </summary>
        /// <param name="path">The parameter path (case insensitive) or configuration name.</param>
        /// <param name="description">The new description.</param>
        /// <param name="value">The new value.</param>
        private void Update(string path, string description, string value)
        {
        }

        /// <summary>
        /// Delete param object.
        /// </summary>
        /// <param name="path">The parameter path (case insensitive) or configuration name.</param>
        public void Delete(string path)
        {
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
            return new List<ParamObject>();
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
            return new List<ParamObject>();
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
            return new List<ParamObject>();
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
            return new List<ParamObject>();
        }

        /// <summary>
        /// Save param object.
        /// </summary>
        /// <param name="path">The parameter path (case insensitive) or configuration name.</param>
        /// <param name="value">The value.</param>
        public void Save(string path, string value)
        {
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
    }
}
