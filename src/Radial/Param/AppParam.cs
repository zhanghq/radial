using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;

namespace Radial.Param
{
    /// <summary>
    /// The application parameter and configuration static class.
    /// </summary>
    public static class AppParam
    {
        const string CanNotConvertExceptionFormat = "can not convert the parameter value of the specified path \"{0}\" to {1}, please ensure it has been set properly";

        /// <summary>
        /// Get the IParam instance
        /// </summary>
        private static IParam Instance
        {
            get
            {
                if (!Dependency.Container.IsRegistered<IParam>())
                    return new ConfigurationParam();
                return Dependency.Container.Resolve<IParam>();
            }
        }

        /// <summary>
        /// Determine whether the specified param object is exists.
        /// </summary>
        /// <param name="path">The parameter path (case insensitive) or configuration name.</param>
        /// <returns>
        ///   <c>true</c> if the specified path is exists; otherwise, <c>false</c>.
        /// </returns>
        public static bool Exists(string path)
        {
            return Instance.Exists(path);
        }

        /// <summary>
        /// Get param object.
        /// </summary>
        /// <param name="path">The parameter path (case insensitive) or configuration name.</param>
        /// <returns>If path exists, return the object, otherwise return null.</returns>
        public static ParamObject Get(string path)
        {
            return Instance.Get(path);
        }

        /// <summary>
        /// Get the param value.
        /// </summary>
        /// <param name="path">The parameter path (case insensitive) or configuration name.</param>
        /// <returns>If path exists, return its value, otherwise return null.</returns>
        public static string GetValue(string path)
        {
            return Instance.GetValue(path);
        }

        /// <summary>
        /// Get next level objects.
        /// </summary>
        /// <param name="currentPath">The current parameter path (case insensitive and list all of first level objects when it sets to string.Empty or null).</param>
        /// <returns>If data exists, return an objects list, otherwise return an empty list.</returns>
        public static IList<ParamObject> Next(string currentPath)
        {
            return Instance.Next(currentPath);
        }

        /// <summary>
        /// Get next level objects.
        /// </summary>
        /// <param name="currentPath">The current parameter path (case insensitive and list all of first level objects when it sets to string.Empty or null).</param>
        /// <param name="pageSize">The list size per page.</param>
        /// <param name="pageIndex">The index of page.</param>
        /// <param name="objectTotal">The object total.</param>
        /// <returns>If data exists, return an objects list, otherwise return an empty list.</returns>
        public static IList<ParamObject> Next(string currentPath, int pageSize, int pageIndex, out int objectTotal)
        {
            return Instance.Next(currentPath, pageSize, pageIndex, out objectTotal);
        }

        /// <summary>
        /// Search objects.
        /// </summary>
        /// <param name="path">The parameter path (case insensitive) or configuration name.</param>
        /// <returns>
        /// If path matches, return an objects list, otherwise return an empty list.
        /// </returns>
        public static IList<ParamObject> Search(string path)
        {
            return Instance.Search(path);
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
        public static IList<ParamObject> Search(string path, int pageSize, int pageIndex, out int objectTotal)
        {
            return Instance.Search(path, pageSize, pageIndex, out objectTotal);
        }

        /// <summary>
        /// Save param object.
        /// </summary>
        /// <param name="path">The parameter path (case insensitive) or configuration name.</param>
        /// <param name="value">The value.</param>
        public static void Save(string path, object value)
        {
            Instance.Save(path, GetObjectString(value));
        }

        /// <summary>
        /// Save param object.
        /// </summary>
        /// <param name="path">The parameter path (case insensitive) or configuration name.</param>
        /// <param name="description">The description.</param>
        /// <param name="value">The value.</param>
        public static void Save(string path, string description, object value)
        {
            Instance.Save(path, description, GetObjectString(value));
        }

        /// <summary>
        /// Delete param object.
        /// </summary>
        /// <param name="path">The parameter path (case insensitive) or configuration name.</param>
        public static void Delete(string path)
        {
            Instance.Delete(path);
        }

        #region All GetValues

        /// <summary>
        /// Get the param value.
        /// </summary>
        /// <param name="path">The parameter path (case insensitive) or configuration name.</param>
        /// <returns>
        /// If path exists, return its value, otherwise throw an exception.
        /// </returns>
        public static bool GetValueBoolean(string path)
        {
            string paramValue = GetValue(path);

            bool returnValue = false;

            if (!bool.TryParse(paramValue, out returnValue))
            {
                if (paramValue == "1" || string.Compare(paramValue, "y", true) == 0 || string.Compare(paramValue, "yes", true) == 0)
                    returnValue = true;
                else if (paramValue == "0" || string.Compare(paramValue, "n", true) == 0 || string.Compare(paramValue, "no", true) == 0)
                    returnValue = false;
                else
                    throw new NotSupportedException(string.Format(CanNotConvertExceptionFormat, path, typeof(bool).FullName));
            }

            return returnValue;
        }

        /// <summary>
        /// Get the param value.
        /// </summary>
        /// <param name="path">The parameter path (case insensitive) or configuration name.</param>
        /// <param name="defaultValue">The default value when param value empty or can not convert.</param>
        /// <returns>
        /// If path exists, return its value, otherwise return default value.
        /// </returns>
        public static bool GetValueBoolean(string path, bool defaultValue)
        {
            string paramValue = GetValue(path);
            if (string.IsNullOrWhiteSpace(paramValue))
                return defaultValue;

            bool returnValue = false;

            if (!bool.TryParse(paramValue, out returnValue))
            {
                paramValue = paramValue.Trim();

                if (paramValue == "1" || string.Compare(paramValue, "y", true) == 0 || string.Compare(paramValue, "yes", true) == 0)
                {
                    returnValue = true;
                }
                else
                {
                    if (paramValue == "0" || string.Compare(paramValue, "n", true) == 0 || string.Compare(paramValue, "no", true) == 0)
                        returnValue = false;
                    else
                        returnValue = defaultValue;
                }
            }

            return returnValue;
        }

        /// <summary>
        /// Get the param value.
        /// </summary>
        /// <param name="path">The parameter path (case insensitive) or configuration name.</param>
        /// <returns>
        /// If path exists, return its value, otherwise throw an exception.
        /// </returns>
        public static decimal GetValueDecimal(string path)
        {
            string paramValue = GetValue(path);

            decimal returnValue = 0;

            if (!string.IsNullOrWhiteSpace(paramValue))
                Checker.Requires(decimal.TryParse(paramValue.Trim(), out returnValue),
                    CanNotConvertExceptionFormat, path, typeof(decimal).FullName);

            return returnValue;
        }


        /// <summary>
        /// Get the param value.
        /// </summary>
        /// <param name="path">The parameter path (case insensitive) or configuration name.</param>
        /// <param name="defaultValue">The default value when param value empty or can not convert.</param>
        /// <returns>
        /// If path exists, return its value, otherwise return default value.
        /// </returns>
        public static decimal GetValueDecimal(string path, decimal defaultValue)
        {
            string paramValue = GetValue(path);
            if (string.IsNullOrWhiteSpace(paramValue))
                return defaultValue;

            decimal returnValue = 0;

            if (!decimal.TryParse(paramValue.Trim(), out returnValue))
            {
                returnValue = defaultValue;
            }

            return returnValue;
        }

        /// <summary>
        /// Get the param value.
        /// </summary>
        /// <param name="path">The parameter path (case insensitive) or configuration name.</param>
        /// <returns>
        /// If path exists, return its value, otherwise throw an exception.
        /// </returns>
        public static double GetValueDouble(string path)
        {
            string paramValue = GetValue(path);

            double returnValue = 0;

            if (!string.IsNullOrWhiteSpace(paramValue))
                Checker.Requires(double.TryParse(paramValue.Trim(), out returnValue), 
                    CanNotConvertExceptionFormat, path, typeof(double).FullName);

            return returnValue;
        }

        /// <summary>
        /// Get the param value.
        /// </summary>
        /// <param name="path">The parameter path (case insensitive) or configuration name.</param>
        /// <param name="defaultValue">The default value when param value empty or can not convert.</param>
        /// <returns>
        /// If path exists, return its value, otherwise return default value.
        /// </returns>
        public static double GetValueDouble(string path, double defaultValue)
        {
            string paramValue = GetValue(path);
            if (string.IsNullOrWhiteSpace(paramValue))
                return defaultValue;

            double returnValue = 0;

            if (!double.TryParse(paramValue.Trim(), out returnValue))
            {
                returnValue = defaultValue;
            }

            return returnValue;
        }

        /// <summary>
        /// Get the param value.
        /// </summary>
        /// <param name="path">The parameter path (case insensitive) or configuration name.</param>
        /// <returns>
        /// If path exists, return its value, otherwise throw an exception.
        /// </returns>
        public static float GetValueFloat(string path)
        {
            string paramValue = GetValue(path);

            float returnValue = 0;

            if (!string.IsNullOrWhiteSpace(paramValue))
                Checker.Requires(float.TryParse(paramValue.Trim(), out returnValue),
                    CanNotConvertExceptionFormat, path, typeof(float).FullName);

            return returnValue;
        }

        /// <summary>
        /// Get the param value.
        /// </summary>
        /// <param name="path">The parameter path (case insensitive) or configuration name.</param>
        /// <param name="defaultValue">The default value when param value empty or can not convert.</param>
        /// <returns>
        /// If path exists, return its value, otherwise return default value.
        /// </returns>
        public static float GetValueFloat(string path, float defaultValue)
        {
            string paramValue = GetValue(path);
            if (string.IsNullOrWhiteSpace(paramValue))
                return defaultValue;

            float returnValue = 0;

            if (!float.TryParse(paramValue.Trim(), out returnValue))
            {
                returnValue = defaultValue;
            }

            return returnValue;
        }

        /// <summary>
        /// Get the param value.
        /// </summary>
        /// <param name="path">The parameter path (case insensitive) or configuration name.</param>
        /// <returns>
        /// If path exists, return its value, otherwise throw an exception.
        /// </returns>
        public static int GetValueInt32(string path)
        {
            string paramValue = GetValue(path);

            int returnValue = 0;

            if (!string.IsNullOrWhiteSpace(paramValue))
                Checker.Requires(int.TryParse(paramValue.Trim(), out returnValue),
                    CanNotConvertExceptionFormat, path, typeof(int).FullName);

            return returnValue;
        }

        /// <summary>
        /// Get the param value.
        /// </summary>
        /// <param name="path">The parameter path (case insensitive) or configuration name.</param>
        /// <param name="defaultValue">The default value when param value empty or can not convert.</param>
        /// <returns>
        /// If path exists, return its value, otherwise return default value.
        /// </returns>
        public static int GetValueInt32(string path, int defaultValue)
        {
            string paramValue = GetValue(path);
            if (string.IsNullOrWhiteSpace(paramValue))
                return defaultValue;

            int returnValue = 0;

            if (!int.TryParse(paramValue.Trim(), out returnValue))
            {
                returnValue = defaultValue;
            }

            return returnValue;
        }

        /// <summary>
        /// Get the param value.
        /// </summary>
        /// <param name="path">The parameter path (case insensitive) or configuration name.</param>
        /// <returns>
        /// If path exists, return its value, otherwise throw an exception.
        /// </returns>
        public static long GetValueInt64(string path)
        {
            string paramValue = GetValue(path);

            long returnValue = 0;

            if (!string.IsNullOrWhiteSpace(paramValue))
                Checker.Requires(long.TryParse(paramValue.Trim(), out returnValue),
                    CanNotConvertExceptionFormat, path, typeof(long).FullName);

            return returnValue;
        }

        /// <summary>
        /// Get the param value.
        /// </summary>
        /// <param name="path">The parameter path (case insensitive) or configuration name.</param>
        /// <param name="defaultValue">The default value when param value empty or can not convert.</param>
        /// <returns>
        /// If path exists, return its value, otherwise return default value.
        /// </returns>
        public static long GetValueInt64(string path, long defaultValue)
        {
            string paramValue = GetValue(path);
            if (string.IsNullOrWhiteSpace(paramValue))
                return defaultValue;

            long returnValue = 0;

            if (!long.TryParse(paramValue.Trim(), out returnValue))
            {
                returnValue = defaultValue;
            }

            return returnValue;
        }

        /// <summary>
        /// Get the param value.
        /// </summary>
        /// <param name="path">The parameter path (case insensitive) or configuration name.</param>
        /// <returns>
        /// If path exists, return its value, otherwise throw an exception.
        /// </returns>
        public static float GetValueSingle(string path)
        {
            string paramValue = GetValue(path);

            float returnValue = 0;

            if (!string.IsNullOrWhiteSpace(paramValue))
                Checker.Requires(float.TryParse(paramValue.Trim(), out returnValue),
                    CanNotConvertExceptionFormat, path, typeof(float).FullName);

            return returnValue;
        }

        /// <summary>
        /// Get the param value.
        /// </summary>
        /// <param name="path">The parameter path (case insensitive) or configuration name.</param>
        /// <param name="defaultValue">The default value when param value empty or can not convert.</param>
        /// <returns>
        /// If path exists, return its value, otherwise return default value.
        /// </returns>
        public static float GetValueSingle(string path, float defaultValue)
        {
            string paramValue = GetValue(path);
            if (string.IsNullOrWhiteSpace(paramValue))
                return defaultValue;

            float returnValue = 0;

            if (!float.TryParse(paramValue.Trim(), out returnValue))
            {
                returnValue = defaultValue;
            }

            return returnValue;
        }

        #endregion


        /// <summary>
        /// Gets the object string.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>If object is null return string.Empty, otherwise return the string value of object.</returns>
        private static string GetObjectString(object obj)
        {
            if (obj == null)
                return null;

            Type objType = obj.GetType();

            if (objType.IsEnum)
                return ((int)obj).ToString();

            if (objType == typeof(Boolean) || objType == typeof(Byte) || objType == typeof(SByte)
                || objType == typeof(Int16) || objType == typeof(UInt16) || objType == typeof(Int32) || objType == typeof(UInt32)
                || objType == typeof(Int64) || objType == typeof(UInt64) || objType == typeof(Char) || objType == typeof(String)
                || objType == typeof(Double) || objType == typeof(Single) || objType == typeof(Decimal))
                return obj.ToString();

            throw new NotSupportedException(string.Format("Object type {0} was not supported in AppParam", objType.FullName));
        }
    }
}
