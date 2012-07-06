using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.SessionState;
using Enyim.Caching;
using System.Web.Configuration;
using System.Configuration;
using System.Web.Hosting;
using System.Web;
using System.IO;

namespace Radial.Web.Session
{
    /// <summary>
    /// Memcached session state store provider.
    /// </summary>
    public class MemcachedStateStore : SessionStateStoreProviderBase
    {

        #region State Data Class

        /// <summary>
        /// State data class.
        /// </summary>
        [Serializable]
        class StateData
        {
            public StateData()
            {
                Created = DateTime.Now;
                ActionFlag = SessionStateActions.None;
            }

            /// <summary>
            /// Gets or sets the action flag.
            /// </summary>
            /// <value>
            /// The action flag.
            /// </value>
            public SessionStateActions ActionFlag
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the lock id.
            /// </summary>
            /// <value>
            /// The lock id.
            /// </value>
            public int LockId
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the created.
            /// </summary>
            /// <value>
            /// The created.
            /// </value>
            public DateTime Created
            {
                get;
                set;
            }

            /// <summary>
            /// Gets the lock age.
            /// </summary>
            public TimeSpan LockAge
            {
                get { return DateTime.Now.Subtract(Created); }
            }

            /// <summary>
            /// Gets or sets a value indicating whether this <see cref="StateData"/> is locked.
            /// </summary>
            /// <value>
            ///   <c>true</c> if locked; otherwise, <c>false</c>.
            /// </value>
            public bool Locked
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the content.
            /// </summary>
            /// <value>
            /// The content.
            /// </value>
            public byte[] Content
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the timeout.
            /// </summary>
            /// <value>
            /// The timeout.
            /// </value>
            public int Timeout { get; set; }

            /// <summary>
            /// Toes the session state store data.
            /// </summary>
            /// <param name="httpContex">The HTTP contex.</param>
            /// <returns></returns>
            public SessionStateStoreData ToSessionStateStoreData(HttpContext httpContex)
            {
                SessionStateItemCollection items = new SessionStateItemCollection();

                if (Content != null && Content.Length > 0)
                {
                    using (MemoryStream ms = new MemoryStream(Content))
                    {
                        if (ms.Length > 0)
                        {
                            BinaryReader reader = new BinaryReader(ms);
                            items = SessionStateItemCollection.Deserialize(reader);
                        }
                    }
                }

                return new SessionStateStoreData(items, SessionStateUtility.GetSessionStaticObjects(httpContex), Timeout);
            }
        }


        #endregion

        Logger _logger;
        string _applicationName;
        MemcachedClient _memcachedClient;
        SessionStateSection _sessionStateConfig;
        static object S_SyncRoot = new object();

        /// <summary>
        /// 获取一条简短的易懂描述，它适合在管理工具或其他用户界面 (UI) 中显示。
        /// </summary>
        /// <returns>简短的易懂描述，适合在管理工具或其他 UI 中显示。</returns>
        public override string Description
        {
            get
            {
                return "Memcached session state store provider";
            }
        }

        /// <summary>
        /// 获得一个友好名称，用于在配置过程中引用提供程序。
        /// </summary>
        /// <returns>用于在配置过程中引用提供程序的友好名称。</returns>
        public override string Name
        {
            get
            {
                return "MemcachedSession";
            }
        }

        /// <summary>
        /// Builds the memcached key.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        /// <returns></returns>
        private string BuildCacheKey(string sessionId)
        {
            return string.Format("{0}@{1}", _applicationName, sessionId);
        }


        /// <summary>
        /// Writes the exception log.
        /// </summary>
        /// <param name="e">The exception.</param>
        /// <param name="methodName">Name of the method.</param>
        private void WriteExceptionLog(Exception e, string methodName)
        {
            _logger.Error(e, "An exception occurred while executing the method: {0}", methodName);
        }

        #region Members of SessionStateStoreProviderBase

        /// <summary>
        /// 初始化提供程序。
        /// </summary>
        /// <param name="name">该提供程序的友好名称。</param>
        /// <param name="config">名称/值对的集合，表示在配置中为该提供程序指定的、提供程序特定的属性。</param>
        /// <exception cref="T:System.ArgumentNullException">提供程序的名称是 null。</exception>
        ///   
        /// <exception cref="T:System.ArgumentException">提供程序的名称长度为零。</exception>
        ///   
        /// <exception cref="T:System.InvalidOperationException">提供程序初始化完成后，将尝试调用该提供程序上的 <see cref="M:System.Configuration.Provider.ProviderBase.Initialize(System.String,System.Collections.Specialized.NameValueCollection)"/>。</exception>
        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            try
            {
                _logger = Logger.GetInstance("MemcachedSession");
                _applicationName = HostingEnvironment.ApplicationVirtualPath;
                _memcachedClient = new MemcachedClient();

                //
                // Set the ApplicationName property.
                //
                if (!string.IsNullOrWhiteSpace(config["applicationName"]))
                    _applicationName = config["applicationName"].Trim();

                Configuration cfg = WebConfigurationManager.OpenWebConfiguration(HostingEnvironment.ApplicationVirtualPath);
                _sessionStateConfig = (SessionStateSection)cfg.GetSection("system.web/sessionState");

                // Initialize the abstract base class.
                base.Initialize(name, config);
            }
            catch (Exception e)
            {
                WriteExceptionLog(e, "Initialize");
            }
        }

        /// <summary>
        /// 创建要用于当前请求的新 <see cref="T:System.Web.SessionState.SessionStateStoreData"/> 对象。
        /// </summary>
        /// <param name="context">当前请求的 <see cref="T:System.Web.HttpContext"/>。</param>
        /// <param name="timeout">新 <see cref="T:System.Web.SessionState.SessionStateStoreData"/> 的会话状态 <see cref="P:System.Web.SessionState.HttpSessionState.Timeout"/> 值。</param>
        /// <returns>
        /// 当前请求的新 <see cref="T:System.Web.SessionState.SessionStateStoreData"/>。
        /// </returns>
        public override SessionStateStoreData CreateNewStoreData(System.Web.HttpContext context, int timeout)
        {
            return new SessionStateStoreData(new SessionStateItemCollection(), SessionStateUtility.GetSessionStaticObjects(context), timeout);
        }

        /// <summary>
        /// 将新的会话状态项添加到数据存储区中。
        /// </summary>
        /// <param name="context">当前请求的 <see cref="T:System.Web.HttpContext"/>。</param>
        /// <param name="id">当前请求的 <see cref="P:System.Web.SessionState.HttpSessionState.SessionID"/>。</param>
        /// <param name="timeout">当前请求的会话 <see cref="P:System.Web.SessionState.HttpSessionState.Timeout"/>。</param>
        public override void CreateUninitializedItem(System.Web.HttpContext context, string id, int timeout)
        {
            StateData data = new StateData { ActionFlag = SessionStateActions.InitializeItem, Timeout = timeout };

            string cacheKey = BuildCacheKey(id);

            try
            {
                lock (S_SyncRoot)
                    _memcachedClient.Store(Enyim.Caching.Memcached.StoreMode.Add, cacheKey, data, TimeSpan.FromMinutes(data.Timeout));
            }
            catch (Exception e)
            {
                WriteExceptionLog(e, "CreateUninitializedItem");
            }

        }

        /// <summary>
        /// 释放由 <see cref="T:System.Web.SessionState.SessionStateStoreProviderBase"/> 实现使用的所有资源。
        /// </summary>
        public override void Dispose()
        {
            _memcachedClient.Dispose();
        }

        /// <summary>
        /// 在请求结束时由 <see cref="T:System.Web.SessionState.SessionStateModule"/> 对象调用。
        /// </summary>
        /// <param name="context">当前请求的 <see cref="T:System.Web.HttpContext"/>。</param>
        public override void EndRequest(System.Web.HttpContext context)
        {
            
        }

        /// <summary>
        /// 从会话数据存储区中返回只读会话状态数据。
        /// </summary>
        /// <param name="context">当前请求的 <see cref="T:System.Web.HttpContext"/>。</param>
        /// <param name="id">当前请求的 <see cref="P:System.Web.SessionState.HttpSessionState.SessionID"/>。</param>
        /// <param name="locked">当此方法返回时，如果请求的会话项在会话数据存储区被锁定，请包含一个设置为 true 的布尔值；否则请包含一个设置为 false 的布尔值。</param>
        /// <param name="lockAge">当此方法返回时，请包含一个设置为会话数据存储区中的项锁定时间的 <see cref="T:System.TimeSpan"/> 对象。</param>
        /// <param name="lockId">当此方法返回时，请包含一个设置为当前请求的锁定标识符的对象。有关锁定标识符的详细信息，请参见 <see cref="T:System.Web.SessionState.SessionStateStoreProviderBase"/> 类摘要中的“锁定会话存储区数据”。</param>
        /// <param name="actions">当此方法返回时，请包含 <see cref="T:System.Web.SessionState.SessionStateActions"/> 值之一，指示当前会话是否为未初始化的无 Cookie 会话。</param>
        /// <returns>
        /// 使用会话数据存储区中的会话值和信息填充的 <see cref="T:System.Web.SessionState.SessionStateStoreData"/>。
        /// </returns>
        public override SessionStateStoreData GetItem(System.Web.HttpContext context, string id, out bool locked, out TimeSpan lockAge, out object lockId, out SessionStateActions actions)
        {
            locked = false;
            lockAge = TimeSpan.Zero;
            lockId = null;
            actions = SessionStateActions.None;

            SessionStateStoreData obj = null;

            string cacheKey = BuildCacheKey(id);

            try
            {
                StateData data = _memcachedClient.Get<StateData>(cacheKey);

                if (data != null)
                {
                    if (!locked)
                    {
                        actions = data.ActionFlag;

                        if (actions == SessionStateActions.InitializeItem)
                            obj = CreateNewStoreData(context, _sessionStateConfig.Timeout.Minutes);
                        else
                            obj = data.ToSessionStateStoreData(context);

                        locked = data.Locked;
                        lockId = data.LockId;
                    }
                    else
                        lockAge = data.LockAge;
                }
            }
            catch (Exception e)
            {
                WriteExceptionLog(e, "GetItem");
            }

            return obj;
        }

        /// <summary>
        /// 从会话数据存储区中返回只读会话状态数据。
        /// </summary>
        /// <param name="context">当前请求的 <see cref="T:System.Web.HttpContext"/>。</param>
        /// <param name="id">当前请求的 <see cref="P:System.Web.SessionState.HttpSessionState.SessionID"/>。</param>
        /// <param name="locked">当此方法返回时，如果成功获得锁定，请包含一个设置为 true 的布尔值；否则请包含一个设置为 false 的布尔值。</param>
        /// <param name="lockAge">当此方法返回时，请包含一个设置为会话数据存储区中的项锁定时间的 <see cref="T:System.TimeSpan"/> 对象。</param>
        /// <param name="lockId">当此方法返回时，请包含一个设置为当前请求的锁定标识符的对象。有关锁定标识符的详细信息，请参见 <see cref="T:System.Web.SessionState.SessionStateStoreProviderBase"/> 类摘要中的“锁定会话存储区数据”。</param>
        /// <param name="actions">当此方法返回时，请包含 <see cref="T:System.Web.SessionState.SessionStateActions"/> 值之一，指示当前会话是否为未初始化的无 Cookie 会话。</param>
        /// <returns>
        /// 使用会话数据存储区中的会话值和信息填充的 <see cref="T:System.Web.SessionState.SessionStateStoreData"/>。
        /// </returns>
        public override SessionStateStoreData GetItemExclusive(System.Web.HttpContext context, string id, out bool locked, out TimeSpan lockAge, out object lockId, out SessionStateActions actions)
        {
            locked = false;
            lockAge = TimeSpan.Zero;
            lockId = null;
            actions = SessionStateActions.None;

            SessionStateStoreData obj = null;
            try
            {
                string cacheKey = BuildCacheKey(id);
                lock (S_SyncRoot)
                {
                    StateData data = _memcachedClient.Get<StateData>(cacheKey);

                    if (data != null)
                    {
                        if (!locked)
                        {
                            actions = data.ActionFlag;

                            data.Locked = true;
                            data.LockId++;


                            _memcachedClient.Store(Enyim.Caching.Memcached.StoreMode.Replace, cacheKey, data, TimeSpan.FromMinutes(data.Timeout));

                            if (actions == SessionStateActions.InitializeItem)
                                obj = CreateNewStoreData(context, _sessionStateConfig.Timeout.Minutes);
                            else
                                obj = data.ToSessionStateStoreData(context);

                            locked = data.Locked;
                            lockId = data.LockId;
                        }
                        else
                            lockAge = data.LockAge;
                    }
                }
            }
            catch (Exception e)
            {
                WriteExceptionLog(e, "GetItem");
            }

            return obj;
        }

        /// <summary>
        /// 由 <see cref="T:System.Web.SessionState.SessionStateModule"/> 对象调用，以便进行每次请求初始化。
        /// </summary>
        /// <param name="context">当前请求的 <see cref="T:System.Web.HttpContext"/>。</param>
        public override void InitializeRequest(System.Web.HttpContext context)
        {
            
        }

        /// <summary>
        /// 释放对会话数据存储区中项的锁定。
        /// </summary>
        /// <param name="context">当前请求的 <see cref="T:System.Web.HttpContext"/>。</param>
        /// <param name="id">当前请求的会话标识符。</param>
        /// <param name="lockId">当前请求的锁定标识符。</param>
        public override void ReleaseItemExclusive(System.Web.HttpContext context, string id, object lockId)
        {
            try
            {
                string cacheKey = BuildCacheKey(id);

                lock (S_SyncRoot)
                {
                    StateData data = _memcachedClient.Get<StateData>(cacheKey);

                    if (data != null && data.LockId == (int)lockId)
                    {
                        data.Locked = false;
                        data.LockId = 0;

                        _memcachedClient.Store(Enyim.Caching.Memcached.StoreMode.Replace, cacheKey, data, TimeSpan.FromMinutes(data.Timeout));

                    }
                }
            }
            catch (Exception e)
            {
                WriteExceptionLog(e, "ReleaseItemExclusive");
            }
        }

        /// <summary>
        /// 删除会话数据存储区中的项数据。
        /// </summary>
        /// <param name="context">当前请求的 <see cref="T:System.Web.HttpContext"/>。</param>
        /// <param name="id">当前请求的会话标识符。</param>
        /// <param name="lockId">当前请求的锁定标识符。</param>
        /// <param name="item">表示将从数据存储区中删除的项的 <see cref="T:System.Web.SessionState.SessionStateStoreData"/>。</param>
        public override void RemoveItem(System.Web.HttpContext context, string id, object lockId, SessionStateStoreData item)
        {
            try
            {
                lock (S_SyncRoot)
                    _memcachedClient.Remove(BuildCacheKey(id));
            }
            catch (Exception e)
            {
                WriteExceptionLog(e, "RemoveItem");
            }
        }

        /// <summary>
        /// 更新会话数据存储区中的项的到期日期和时间。
        /// </summary>
        /// <param name="context">当前请求的 <see cref="T:System.Web.HttpContext"/>。</param>
        /// <param name="id">当前请求的会话标识符。</param>
        public override void ResetItemTimeout(System.Web.HttpContext context, string id)
        {

            try
            {
                string cacheKey = BuildCacheKey(id);

                lock (S_SyncRoot)
                {
                    StateData data = _memcachedClient.Get<StateData>(cacheKey);

                    if (data != null)
                    {
                        _memcachedClient.Store(Enyim.Caching.Memcached.StoreMode.Replace, cacheKey, data, TimeSpan.FromMinutes(data.Timeout));
                    }
                }
            }
            catch (Exception e)
            {
                WriteExceptionLog(e, "ResetItemTimeout");
            }

        }

        /// <summary>
        /// 使用当前请求中的值更新会话状态数据存储区中的会话项信息，并清除对数据的锁定。
        /// </summary>
        /// <param name="context">当前请求的 <see cref="T:System.Web.HttpContext"/>。</param>
        /// <param name="id">当前请求的会话标识符。</param>
        /// <param name="item">包含要存储的当前会话值的 <see cref="T:System.Web.SessionState.SessionStateStoreData"/> 对象。</param>
        /// <param name="lockId">当前请求的锁定标识符。</param>
        /// <param name="newItem">如果为 true，则将会话项标识为新项；如果为 false，则将会话项标识为现有的项。</param>
        public override void SetAndReleaseItemExclusive(System.Web.HttpContext context, string id, SessionStateStoreData item, object lockId, bool newItem)
        {

            try
            {
                lock (S_SyncRoot)
                {
                    string cacheKey = BuildCacheKey(id);
                    StateData data = new StateData { Content = item.Serialize(), Timeout = item.Timeout, LockId = lockId != null ? (int)lockId : 0 };

                    if (newItem)
                        _memcachedClient.Store(Enyim.Caching.Memcached.StoreMode.Add, cacheKey, data, TimeSpan.FromMinutes(data.Timeout));
                    else
                        _memcachedClient.Store(Enyim.Caching.Memcached.StoreMode.Replace, cacheKey, data, TimeSpan.FromMinutes(data.Timeout));
                }
            }
            catch (Exception e)
            {
                WriteExceptionLog(e, "SetAndReleaseItemExclusive");
            }

        }

        /// <summary>
        /// 设置对 Global.asax 文件中定义的 Session_OnEnd 事件的 <see cref="T:System.Web.SessionState.SessionStateItemExpireCallback"/> 委托的引用。
        /// </summary>
        /// <param name="expireCallback">对 Global.asax 文件中定义的 Session_OnEnd 事件的 <see cref="T:System.Web.SessionState.SessionStateItemExpireCallback"/> 委托。</param>
        /// <returns>
        /// 如果会话状态存储提供程序支持调用 Session_OnEnd 事件，则为 true；否则为 false。
        /// </returns>
        public override bool SetItemExpireCallback(SessionStateItemExpireCallback expireCallback)
        {
            return false;
        }

        #endregion
    }
}
