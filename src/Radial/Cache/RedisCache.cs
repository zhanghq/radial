using BookSleeve;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;

namespace Radial.Cache
{
    /// <summary>
    /// Redis cache implement class
    /// </summary>
    public class RedisCache : ICache
    {
        static object SyncRoot = new object();

        static RedisConnection Conn = null;
        static int Db = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="RedisCache"/> class.
        /// </summary>
        public RedisCache()
        {
            lock (SyncRoot)
            {
                if (Conn == null)
                {
                    string host = ConfigurationManager.AppSettings["RedisCache.Conn.Host"];

                    Checker.Requires(!string.IsNullOrWhiteSpace(host), "redis server host can not be empty or null");

                    int port = 6379;

                    if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["RedisCache.Conn.Port"]))
                        port = int.Parse(ConfigurationManager.AppSettings["RedisCache.Conn.Port"]);

                    if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["RedisCache.Conn.Db"]))
                        Db = int.Parse(ConfigurationManager.AppSettings["RedisCache.Conn.Db"]);

                    Conn = new RedisConnection(host, port);
                    OpenConnection();
                }
            }
        }

        /// <summary>
        /// Opens the connection.
        /// </summary>
        private void OpenConnection()
        {
            if (Conn.State != RedisConnectionBase.ConnectionState.Open &&
                Conn.State != RedisConnectionBase.ConnectionState.Opening)
                Conn.Open();
        }

        /// <summary>
        /// Set cache data.
        /// </summary>
        /// <param name="key">The cache key(case insensitive).</param>
        /// <param name="value">The cache value.</param>
        /// <param name="cacheSeconds">The cache holding seconds.</param>
        public void SetString(string key, string value, int? cacheSeconds = null)
        {
            ThreadPool.QueueUserWorkItem(o =>
            {
                OpenConnection();

                if (cacheSeconds.HasValue)
                    Conn.Strings.Set(Db, CacheHelper.NormalizeKey(key), value, cacheSeconds.Value);
                else
                    Conn.Strings.Set(Db, CacheHelper.NormalizeKey(key), value);
            });
        }


        /// <summary>
        /// Retrieve cached data.
        /// </summary>
        /// <param name="key">The cache key(case insensitive).</param>
        /// <returns>
        /// If there has matched data, return the cached string value, otherwise return null.
        /// </returns>
        public string GetString(string key)
        {
            OpenConnection();

            return Conn.Strings.GetString(Db, CacheHelper.NormalizeKey(key)).Result;
        }

        /// <summary>
        /// Set cache data.
        /// </summary>
        /// <param name="key">The cache key(case insensitive).</param>
        /// <param name="value">The cache value.</param>
        /// <param name="cacheSeconds">The cache holding seconds.</param>
        public void SetBinary(string key, byte[] value, int? cacheSeconds = null)
        {
            ThreadPool.QueueUserWorkItem(o =>
            {
                OpenConnection();

                if (cacheSeconds.HasValue)
                    Conn.Strings.Set(Db, CacheHelper.NormalizeKey(key), value, cacheSeconds.Value);
                else
                    Conn.Strings.Set(Db, CacheHelper.NormalizeKey(key), value);
            });
        }


        /// <summary>
        /// Retrieve cached data.
        /// </summary>
        /// <param name="key">The cache key(case insensitive).</param>
        /// <returns>
        /// If there has matched data, return the cached binary value, otherwise return null.
        /// </returns>
        public byte[] GetBinary(string key)
        {
            OpenConnection();

            return Conn.Strings.Get(Db, CacheHelper.NormalizeKey(key)).Result;
        }

        /// <summary>
        /// Remove cache key and its value.
        /// </summary>
        /// <param name="key">The cache key(case insensitive).</param>
        public void Remove(string key)
        {
            ThreadPool.QueueUserWorkItem(o =>
            {
                OpenConnection();

                Conn.Keys.Remove(Db, CacheHelper.NormalizeKey(key));
            });
        }



        /// <summary>
        /// Set the permanent hash cache.
        /// </summary>
        /// <param name="key">The cache key(case insensitive).</param>
        /// <param name="value">The hash value.</param>
        public void SetHash(string key, IDictionary<string, byte[]> value)
        {
            ThreadPool.QueueUserWorkItem(o =>
            {
                OpenConnection();

                Conn.Hashes.Set(Db, CacheHelper.NormalizeKey(key), new Dictionary<string, byte[]>(value));
            });
        }


        /// <summary>
        /// Set the permanent hash cache.
        /// </summary>
        /// <param name="key">The cache key(case insensitive).</param>
        /// <param name="field">The hash field(case insensitive).</param>
        /// <param name="value">The hash value.</param>
        public void SetHashString(string key, string field, string value)
        {
            ThreadPool.QueueUserWorkItem(o =>
            {
                OpenConnection();

                Conn.Hashes.Set(Db, CacheHelper.NormalizeKey(key), CacheHelper.NormalizeKey(field), value);
            });
        }


        /// <summary>
        /// Set the permanent hash cache.
        /// </summary>
        /// <param name="key">The cache key(case insensitive).</param>
        /// <param name="field">The hash field(case insensitive).</param>
        /// <param name="value">The hash value.</param>
        public void SetHashBinary(string key, string field, byte[] value)
        {
            ThreadPool.QueueUserWorkItem(o =>
            {
                OpenConnection();
                
                Conn.Hashes.Set(Db, CacheHelper.NormalizeKey(key), CacheHelper.NormalizeKey(field), value);
            });
        }

        /// <summary>
        /// Retrieve the permanent hash cache value.
        /// </summary>
        /// <param name="key">The cache key(case insensitive).</param>
        /// <param name="field">The hash field(case insensitive).</param>
        /// <returns>
        /// If there has matched data, return the cached string value, otherwise return null.
        /// </returns>
        public string GetHashStringValue(string key, string field)
        {
            OpenConnection();

            return Conn.Hashes.GetString(Db, CacheHelper.NormalizeKey(key), CacheHelper.NormalizeKey(field)).Result;
        }

        /// <summary>
        /// Retrieve the permanent hash cache value.
        /// </summary>
        /// <param name="key">The cache key(case insensitive).</param>
        /// <param name="field">The hash field(case insensitive).</param>
        /// <returns>
        /// If there has matched data, return the cached binary value, otherwise return null.
        /// </returns>
        public byte[] GetHashBinaryValue(string key, string field)
        {
            OpenConnection();

            return Conn.Hashes.Get(Db, CacheHelper.NormalizeKey(key), field).Result;
        }

        /// <summary>
        /// Retrieve the permanent hash cache value.
        /// </summary>
        /// <param name="key">The cache key(case insensitive).</param>
        /// <returns>
        /// If there has matched data, return the cached value, otherwise return null.
        /// </returns>
        public IDictionary<string, byte[]> GetHash(string key)
        {
            OpenConnection();

            return Conn.Hashes.GetAll(Db, CacheHelper.NormalizeKey(key)).Result;
        }

        /// <summary>
        /// Remove the permanent hash cache value.
        /// </summary>
        /// <param name="key">The cache key(case insensitive).</param>
        /// <param name="field">The hash field(case insensitive).</param>
        public void RemoveHashValue(string key, string field)
        {
            ThreadPool.QueueUserWorkItem(o =>
            {
                OpenConnection();

                Conn.Hashes.Remove(Db, CacheHelper.NormalizeKey(key), CacheHelper.NormalizeKey(field));
            });
        }
    }
}
