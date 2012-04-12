using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Context;

namespace Radial.Data.Nhs
{
    /// <summary>
    /// Context bound session class.
    /// </summary>
    public sealed class ContextBoundSession : ISession
    {
        ISession _innerSession;


        /// <summary>
        /// Initializes a new instance of the <see cref="ContextBoundSession"/> class.
        /// </summary>
        public ContextBoundSession()
        {
            _innerSession = HibernateEngine.SessionFactory.OpenSession();
            CurrentSessionContext.Bind(_innerSession);
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="ContextBoundSession"/> class.
        /// </summary>
        /// <param name="session">The session.</param>
        public ContextBoundSession(ISession session)
        {
            Checker.Parameter(session != null, "ISession instance can not be null");

            _innerSession = session;
            HibernateEngine.BindSession(session);
        }

        #region ISession 成员

        /// <summary>
        /// The entity mode in effect for this session.
        /// </summary>
        /// <value></value>
        public EntityMode ActiveEntityMode
        {
            get { return _innerSession.ActiveEntityMode; }
        }

        /// <summary>
        /// Begin a transaction with the specified <c>isolationLevel</c>
        /// </summary>
        /// <param name="isolationLevel">Isolation level for the new transaction</param>
        /// <returns>
        /// A transaction instance having the specified isolation level
        /// </returns>
        public ITransaction BeginTransaction(System.Data.IsolationLevel isolationLevel)
        {
            return _innerSession.BeginTransaction(isolationLevel);
        }

        /// <summary>
        /// Begin a unit of work and return the associated <c>ITransaction</c> object.
        /// </summary>
        /// <returns>A transaction instance</returns>
        /// <remarks>
        /// If a new underlying transaction is required, begin the transaction. Otherwise
        /// continue the new work in the context of the existing underlying transaction.
        /// The class of the returned <see cref="T:NHibernate.ITransaction"/> object is determined by
        /// the property <c>transaction_factory</c>
        /// </remarks>
        public ITransaction BeginTransaction()
        {
            return _innerSession.BeginTransaction();
        }

        /// <summary>
        /// The current cache mode.
        /// </summary>
        /// <value></value>
        /// <remarks>
        /// Cache mode determines the manner in which this session can interact with
        /// the second level cache.
        /// </remarks>
        public CacheMode CacheMode
        {
            get
            {
                return _innerSession.CacheMode;
            }
            set
            {
                _innerSession.CacheMode = value;
            }
        }

        /// <summary>
        /// Cancel execution of the current query.
        /// </summary>
        /// <remarks>
        /// May be called from one thread to stop execution of a query in another thread.
        /// Use with care!
        /// </remarks>
        public void CancelQuery()
        {
            _innerSession.CancelQuery();
        }

        /// <summary>
        /// Completely clear the session. Evict all loaded instances and cancel all pending
        /// saves, updates and deletions. Do not close open enumerables or instances of
        /// <c>ScrollableResults</c>.
        /// </summary>
        public void Clear()
        {
            _innerSession.Clear();
        }

        /// <summary>
        /// End the <c>ISession</c> by disconnecting from the ADO.NET connection and cleaning up.(not unbind session)
        /// </summary>
        /// <returns>
        /// The connection provided by the application or <see langword="null"/>
        /// </returns>
        /// <remarks>
        /// It is not strictly necessary to <c>Close()</c> the <c>ISession</c> but you must
        /// at least <c>Disconnect()</c> it.
        /// </remarks>
        public System.Data.IDbConnection Close()
        {
            return _innerSession.Close();
        }

        /// <summary>
        /// Gets the ADO.NET connection.
        /// </summary>
        /// <value></value>
        /// <remarks>
        /// Applications are responsible for calling commit/rollback upon the connection before
        /// closing the <c>ISession</c>.
        /// </remarks>
        public System.Data.IDbConnection Connection
        {
            get { return _innerSession.Connection; }
        }

        /// <summary>
        /// Is this instance associated with this Session?
        /// </summary>
        /// <param name="obj">an instance of a persistent class</param>
        /// <returns>
        /// true if the given instance is associated with this Session
        /// </returns>
        public bool Contains(object obj)
        {
            return _innerSession.Contains(obj);
        }

        /// <summary>
        /// Create a new <c>Criteria</c> instance, for the given entity name,
        /// with the given alias.
        /// </summary>
        /// <param name="entityName">The name of the entity to Query</param>
        /// <param name="alias">The alias of the entity</param>
        /// <returns>An ICriteria object</returns>
        public ICriteria CreateCriteria(string entityName, string alias)
        {
            return _innerSession.CreateCriteria(entityName, alias);
        }

        /// <summary>
        /// Create a new <c>Criteria</c> instance, for the given entity name.
        /// </summary>
        /// <param name="entityName">The name of the entity to Query</param>
        /// <returns>An ICriteria object</returns>
        public ICriteria CreateCriteria(string entityName)
        {
            return _innerSession.CreateCriteria(entityName);
        }

        /// <summary>
        /// Creates a new <c>Criteria</c> for the entity class with a specific alias
        /// </summary>
        /// <param name="persistentClass">The class to Query</param>
        /// <param name="alias">The alias of the entity</param>
        /// <returns>An ICriteria object</returns>
        public ICriteria CreateCriteria(Type persistentClass, string alias)
        {
            return _innerSession.CreateCriteria(persistentClass, alias);
        }

        /// <summary>
        /// Creates a new <c>Criteria</c> for the entity class.
        /// </summary>
        /// <param name="persistentClass">The class to Query</param>
        /// <returns>An ICriteria object</returns>
        public ICriteria CreateCriteria(Type persistentClass)
        {
            return _innerSession.CreateCriteria(persistentClass);
        }

        /// <summary>
        /// Creates the criteria.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="alias">The alias.</param>
        /// <returns></returns>
        public ICriteria CreateCriteria<T>(string alias) where T : class
        {
            return _innerSession.CreateCriteria<T>(alias);
        }

        /// <summary>
        /// Creates the criteria.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public ICriteria CreateCriteria<T>() where T : class
        {
            return _innerSession.CreateCriteria<T>();
        }

        /// <summary>
        /// Create a new instance of <c>Query</c> for the given collection and filter string
        /// </summary>
        /// <param name="collection">A persistent collection</param>
        /// <param name="queryString">A hibernate query</param>
        /// <returns>A query</returns>
        public IQuery CreateFilter(object collection, string queryString)
        {
            return _innerSession.CreateFilter(collection, queryString);
        }

        /// <summary>
        /// An <see cref="T:NHibernate.IMultiCriteria"/> that can return a list of all the results
        /// of all the criterias.
        /// </summary>
        /// <returns></returns>
        public IMultiCriteria CreateMultiCriteria()
        {
            return _innerSession.CreateMultiCriteria();
        }

        /// <summary>
        /// Create a multi query, a query that can send several
        /// queries to the server, and return all their results in a single
        /// call.
        /// </summary>
        /// <returns>
        /// An <see cref="T:NHibernate.IMultiQuery"/> that can return
        /// a list of all the results of all the queries.
        /// Note that each query result is itself usually a list.
        /// </returns>
        public IMultiQuery CreateMultiQuery()
        {
            return _innerSession.CreateMultiQuery();
        }

        /// <summary>
        /// Create a new instance of <c>Query</c> for the given query string
        /// </summary>
        /// <param name="queryString">A hibernate query string</param>
        /// <returns>The query</returns>
        public IQuery CreateQuery(string queryString)
        {
            return _innerSession.CreateQuery(queryString);
        }

        /// <summary>
        /// Create a new instance of <see cref="T:NHibernate.ISQLQuery"/> for the given SQL query string.
        /// </summary>
        /// <param name="queryString">a query expressed in SQL</param>
        /// <returns>
        /// An <see cref="T:NHibernate.ISQLQuery"/> from the SQL string
        /// </returns>
        public ISQLQuery CreateSQLQuery(string queryString)
        {
            return _innerSession.CreateSQLQuery(queryString);
        }

        /// <summary>
        /// Delete all objects returned by the query.
        /// </summary>
        /// <param name="query">The query string</param>
        /// <param name="values">A list of values to be written to "?" placeholders in the query</param>
        /// <param name="types">A list of Hibernate types of the values</param>
        /// <returns>The number of instances deleted</returns>
        public int Delete(string query, object[] values, NHibernate.Type.IType[] types)
        {
            return _innerSession.Delete(query, values, types);
        }

        /// <summary>
        /// Delete all objects returned by the query.
        /// </summary>
        /// <param name="query">The query string</param>
        /// <param name="value">A value to be written to a "?" placeholer in the query</param>
        /// <param name="type">The hibernate type of value.</param>
        /// <returns>The number of instances deleted</returns>
        public int Delete(string query, object value, NHibernate.Type.IType type)
        {
            return _innerSession.Delete(query, value, type);
        }

        /// <summary>
        /// Delete all objects returned by the query.
        /// </summary>
        /// <param name="query">The query string</param>
        /// <returns>Returns the number of objects deleted.</returns>
        public int Delete(string query)
        {
            return _innerSession.Delete(query);
        }

        /// <summary>
        /// Remove a persistent instance from the datastore. The <b>object</b> argument may be
        /// an instance associated with the receiving <see cref="T:NHibernate.ISession"/> or a transient
        /// instance with an identifier associated with existing persistent state.
        /// This operation cascades to associated instances if the association is mapped
        /// with <tt>cascade="delete"</tt>.
        /// </summary>
        /// <param name="entityName">The entity name for the instance to be removed.</param>
        /// <param name="obj">the instance to be removed</param>
        public void Delete(string entityName, object obj)
        {
            _innerSession.Delete(entityName, obj);
        }

        /// <summary>
        /// Remove a persistent instance from the datastore.
        /// </summary>
        /// <param name="obj">The instance to be removed</param>
        /// <remarks>
        /// The argument may be an instance associated with the receiving <c>ISession</c> or a
        /// transient instance with an identifier associated with existing persistent state.
        /// </remarks>
        public void Delete(object obj)
        {
            _innerSession.Delete(obj);
        }

        /// <summary>
        /// Disable the named filter for the current session.
        /// </summary>
        /// <param name="filterName">The name of the filter to be disabled.</param>
        public void DisableFilter(string filterName)
        {
            _innerSession.DisableFilter(filterName);
        }

        /// <summary>
        /// Disconnect the <c>ISession</c> from the current ADO.NET connection.
        /// </summary>
        /// <returns>
        /// The connection provided by the application or <see langword="null"/>
        /// </returns>
        /// <remarks>
        /// If the connection was obtained by Hibernate, close it or return it to the connection
        /// pool. Otherwise return it to the application. This is used by applications which require
        /// long transactions.
        /// </remarks>
        public System.Data.IDbConnection Disconnect()
        {
            return _innerSession.Disconnect();
        }

        /// <summary>
        /// Enable the named filter for this current session.
        /// </summary>
        /// <param name="filterName">The name of the filter to be enabled.</param>
        /// <returns>
        /// The Filter instance representing the enabled filter.
        /// </returns>
        public IFilter EnableFilter(string filterName)
        {
            return _innerSession.EnableFilter(filterName);
        }


        /// <summary>
        /// Remove this instance from the session cache.
        /// </summary>
        /// <param name="obj">a persistent instance</param>
        /// <remarks>
        /// Changes to the instance will not be synchronized with the database.
        /// This operation cascades to associated instances if the association is mapped
        /// with <c>cascade="all"</c> or <c>cascade="all-delete-orphan"</c>.
        /// </remarks>
        public void Evict(object obj)
        {
            _innerSession.Evict(obj);
        }

        /// <summary>
        /// Force the <c>ISession</c> to flush.
        /// </summary>
        /// <remarks>
        /// Must be called at the end of a unit of work, before commiting the transaction and closing
        /// the session (<c>Transaction.Commit()</c> calls this method). <i>Flushing</i> if the process
        /// of synchronising the underlying persistent store with persistable state held in memory.
        /// </remarks>
        public void Flush()
        {
            _innerSession.Flush();
        }

        /// <summary>
        /// Determines at which points Hibernate automatically flushes the session.
        /// </summary>
        /// <value></value>
        /// <remarks>
        /// For a readonly session, it is reasonable to set the flush mode to <c>FlushMode.Never</c>
        /// at the start of the session (in order to achieve some extra performance).
        /// </remarks>
        public FlushMode FlushMode
        {
            get
            {
                return _innerSession.FlushMode;
            }
            set
            {
                _innerSession.FlushMode = value;
            }
        }

        /// <summary>
        /// Gets the specified id.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id">The id.</param>
        /// <param name="lockMode">The lock mode.</param>
        /// <returns></returns>
        public T Get<T>(object id, LockMode lockMode)
        {
            return _innerSession.Get<T>(id, lockMode);
        }

        /// <summary>
        /// Gets the specified id.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public T Get<T>(object id)
        {
            return _innerSession.Get<T>(id);
        }

        /// <summary>
        /// Return the persistent instance of the given named entity with the given identifier,
        /// or null if there is no such persistent instance. (If the instance, or a proxy for the
        /// instance, is already associated with the session, return that instance or proxy.)
        /// </summary>
        /// <param name="entityName">the entity name</param>
        /// <param name="id">an identifier</param>
        /// <returns>a persistent instance or null</returns>
        public object Get(string entityName, object id)
        {
            return _innerSession.Get(entityName, id);
        }

        /// <summary>
        /// Return the persistent instance of the given entity class with the given identifier, or null
        /// if there is no such persistent instance. Obtain the specified lock mode if the instance
        /// exists.
        /// </summary>
        /// <param name="clazz">a persistent class</param>
        /// <param name="id">an identifier</param>
        /// <param name="lockMode">the lock mode</param>
        /// <returns>a persistent instance or null</returns>
        public object Get(Type clazz, object id, LockMode lockMode)
        {
            return _innerSession.Get(clazz, id, lockMode);
        }

        /// <summary>
        /// Return the persistent instance of the given entity class with the given identifier, or null
        /// if there is no such persistent instance. (If the instance, or a proxy for the instance, is
        /// already associated with the session, return that instance or proxy.)
        /// </summary>
        /// <param name="clazz">a persistent class</param>
        /// <param name="id">an identifier</param>
        /// <returns>a persistent instance or null</returns>
        public object Get(Type clazz, object id)
        {
            return _innerSession.Get(clazz, id);
        }

        /// <summary>
        /// Determine the current lock mode of the given object
        /// </summary>
        /// <param name="obj">A persistent instance</param>
        /// <returns>The current lock mode</returns>
        public LockMode GetCurrentLockMode(object obj)
        {
            return _innerSession.GetCurrentLockMode(obj); ;
        }

        /// <summary>
        /// Retrieve a currently enabled filter by name.
        /// </summary>
        /// <param name="filterName">The name of the filter to be retrieved.</param>
        /// <returns>
        /// The Filter instance representing the enabled filter.
        /// </returns>
        public IFilter GetEnabledFilter(string filterName)
        {
            return _innerSession.GetEnabledFilter(filterName);
        }

        /// <summary>
        /// Return the entity name for a persistent entity
        /// </summary>
        /// <param name="obj">a persistent entity</param>
        /// <returns>the entity name</returns>
        public string GetEntityName(object obj)
        {
            return _innerSession.GetEntityName(obj);
        }

        /// <summary>
        /// Return the identifier of an entity instance cached by the <c>ISession</c>
        /// </summary>
        /// <param name="obj">a persistent instance</param>
        /// <returns>the identifier</returns>
        /// <remarks>
        /// Throws an exception if the instance is transient or associated with a different
        /// <c>ISession</c>
        /// </remarks>
        public object GetIdentifier(object obj)
        {
            return _innerSession.GetIdentifier(obj);
        }

        /// <summary>
        /// Obtain an instance of <see cref="T:NHibernate.IQuery"/> for a named query string defined in the
        /// mapping file.
        /// </summary>
        /// <param name="queryName">The name of a query defined externally.</param>
        /// <returns>
        /// An <see cref="T:NHibernate.IQuery"/> from a named query string.
        /// </returns>
        /// <remarks>
        /// The query can be either in <c>HQL</c> or <c>SQL</c> format.
        /// </remarks>
        public IQuery GetNamedQuery(string queryName)
        {
            return _innerSession.GetNamedQuery(queryName);
        }

        /// <summary>
        /// Starts a new Session with the given entity mode in effect. This secondary
        /// Session inherits the connection, transaction, and other context
        /// information from the primary Session. It doesn't need to be flushed
        /// or closed by the developer.
        /// </summary>
        /// <param name="entityMode">The entity mode to use for the new session.</param>
        /// <returns>The new session</returns>
        public ISession GetSession(EntityMode entityMode)
        {
            return _innerSession.GetSession(entityMode);
        }

        /// <summary>
        /// Gets the session implementation.
        /// </summary>
        /// <returns>
        /// An NHibernate implementation of the <seealso cref="T:NHibernate.Engine.ISessionImplementor"/> interface
        /// </returns>
        /// <remarks>
        /// This method is provided in order to get the <b>NHibernate</b> implementation of the session from wrapper implementions.
        /// Implementors of the <seealso cref="T:NHibernate.ISession"/> interface should return the NHibernate implementation of this method.
        /// </remarks>
        public NHibernate.Engine.ISessionImplementor GetSessionImplementation()
        {
            return _innerSession.GetSessionImplementation();
        }

        /// <summary>
        /// Is the <c>ISession</c> currently connected?
        /// </summary>
        /// <value></value>
        public bool IsConnected
        {
            get { return _innerSession.IsConnected; }
        }

        /// <summary>
        /// Does this <c>ISession</c> contain any changes which must be
        /// synchronized with the database? Would any SQL be executed if
        /// we flushed this session?
        /// </summary>
        /// <returns></returns>
        public bool IsDirty()
        {
            return _innerSession.IsDirty();
        }

        /// <summary>
        /// Is the <c>ISession</c> still open?
        /// </summary>
        /// <value></value>
        public bool IsOpen
        {
            get { return _innerSession.IsOpen; }
        }

        /// <summary>
        /// Read the persistent state associated with the given identifier into the given transient
        /// instance.
        /// </summary>
        /// <param name="obj">An "empty" instance of the persistent class</param>
        /// <param name="id">A valid identifier of an existing persistent instance of the class</param>
        public void Load(object obj, object id)
        {
            _innerSession.Load(obj, id);
        }

        /// <summary>
        /// Return the persistent instance of the given <paramref name="entityName"/> with the given identifier,
        /// assuming that the instance exists.
        /// </summary>
        /// <param name="entityName">The entity-name of a persistent class</param>
        /// <param name="id">a valid identifier of an existing persistent instance of the class</param>
        /// <returns>The persistent instance or proxy</returns>
        /// <remarks>
        /// You should not use this method to determine if an instance exists (use <see cref="M:NHibernate.ISession.Get(System.String,System.Object)"/>
        /// instead). Use this only to retrieve an instance that you assume exists, where non-existence
        /// would be an actual error.
        /// </remarks>
        public object Load(string entityName, object id)
        {
            return _innerSession.Load(entityName, id);
        }

        /// <summary>
        /// Loads the specified id.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public T Load<T>(object id)
        {
            return _innerSession.Load<T>(id);
        }

        /// <summary>
        /// Loads the specified id.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id">The id.</param>
        /// <param name="lockMode">The lock mode.</param>
        /// <returns></returns>
        public T Load<T>(object id, LockMode lockMode)
        {
            return _innerSession.Load<T>(id, lockMode);
        }

        /// <summary>
        /// Return the persistent instance of the given entity class with the given identifier,
        /// assuming that the instance exists.
        /// </summary>
        /// <param name="theType">A persistent class</param>
        /// <param name="id">A valid identifier of an existing persistent instance of the class</param>
        /// <returns>The persistent instance or proxy</returns>
        /// <remarks>
        /// You should not use this method to determine if an instance exists (use a query or
        /// <see cref="M:NHibernate.ISession.Get(System.Type,System.Object)"/> instead). Use this only to retrieve an instance
        /// that you assume exists, where non-existence would be an actual error.
        /// </remarks>
        public object Load(Type theType, object id)
        {
            return _innerSession.Load(theType, id);
        }

        /// <summary>
        /// Return the persistent instance of the given entity class with the given identifier,
        /// obtaining the specified lock mode, assuming the instance exists.
        /// </summary>
        /// <param name="entityName">The entity-name of a persistent class</param>
        /// <param name="id">a valid identifier of an existing persistent instance of the class</param>
        /// <param name="lockMode">the lock level</param>
        /// <returns>the persistent instance or proxy</returns>
        public object Load(string entityName, object id, LockMode lockMode)
        {
            return _innerSession.Load(entityName, id, lockMode);
        }

        /// <summary>
        /// Return the persistent instance of the given entity class with the given identifier,
        /// obtaining the specified lock mode.
        /// </summary>
        /// <param name="theType">A persistent class</param>
        /// <param name="id">A valid identifier of an existing persistent instance of the class</param>
        /// <param name="lockMode">The lock level</param>
        /// <returns>the persistent instance</returns>
        public object Load(Type theType, object id, LockMode lockMode)
        {
            return _innerSession.Load(theType, id, lockMode);
        }

        /// <summary>
        /// Obtain the specified lock level upon the given object.
        /// </summary>
        /// <param name="entityName">The Entity name.</param>
        /// <param name="obj">a persistent or transient instance</param>
        /// <param name="lockMode">the lock level</param>
        /// <remarks>
        /// This may be used to perform a version check (<see cref="F:NHibernate.LockMode.Read"/>), to upgrade to a pessimistic
        /// lock (<see cref="F:NHibernate.LockMode.Upgrade"/>), or to simply reassociate a transient instance
        /// with a session (<see cref="F:NHibernate.LockMode.None"/>). This operation cascades to associated
        /// instances if the association is mapped with <tt>cascade="lock"</tt>.
        /// </remarks>
        public void Lock(string entityName, object obj, LockMode lockMode)
        {
            _innerSession.Lock(entityName, obj, lockMode);
        }

        /// <summary>
        /// Obtain the specified lock level upon the given object.
        /// </summary>
        /// <param name="obj">A persistent instance</param>
        /// <param name="lockMode">The lock level</param>
        public void Lock(object obj, LockMode lockMode)
        {
            _innerSession.Lock(obj, lockMode);
        }

        /// <summary>
        /// Copy the state of the given object onto the persistent object with the same
        /// identifier. If there is no persistent instance currently associated with
        /// the session, it will be loaded. Return the persistent instance. If the
        /// given instance is unsaved, save a copy of and return it as a newly persistent
        /// instance. The given instance does not become associated with the session.
        /// This operation cascades to associated instances if the association is mapped
        /// with <tt>cascade="merge"</tt>.<br/>
        /// The semantics of this method are defined by JSR-220.
        /// <param name="entityName">Name of the entity.</param>
        /// 	<param name="obj">a detached instance with state to be copied </param>
        /// 	<returns> an updated persistent instance </returns>
        /// </summary>
        public object Merge(string entityName, object obj)
        {
            return _innerSession.Merge(entityName, obj);
        }

        /// <summary>
        /// Copy the state of the given object onto the persistent object with the same
        /// identifier. If there is no persistent instance currently associated with
        /// the session, it will be loaded. Return the persistent instance. If the
        /// given instance is unsaved, save a copy of and return it as a newly persistent
        /// instance. The given instance does not become associated with the session.
        /// This operation cascades to associated instances if the association is mapped
        /// with <tt>cascade="merge"</tt>.<br/>
        /// The semantics of this method are defined by JSR-220.
        /// </summary>
        /// <param name="obj">a detached instance with state to be copied</param>
        /// <returns>an updated persistent instance</returns>
        public object Merge(object obj)
        {
            return _innerSession.Merge(obj);
        }

        /// <summary>
        /// Make a transient instance persistent. This operation cascades to associated
        /// instances if the association is mapped with <tt>cascade="persist"</tt>.<br/>
        /// The semantics of this method are defined by JSR-220.
        /// </summary>
        /// <param name="entityName">Name of the entity.</param>
        /// <param name="obj">a transient instance to be made persistent</param>
        public void Persist(string entityName, object obj)
        {
            _innerSession.Persist(entityName, obj);
        }

        /// <summary>
        /// Make a transient instance persistent. This operation cascades to associated
        /// instances if the association is mapped with <tt>cascade="persist"</tt>.<br/>
        /// The semantics of this method are defined by JSR-220.
        /// </summary>
        /// <param name="obj">a transient instance to be made persistent</param>
        public void Persist(object obj)
        {
            _innerSession.Persist(obj);
        }

        /// <summary>
        /// Reconnect to the given ADO.NET connection.
        /// </summary>
        /// <param name="connection">An ADO.NET connection</param>
        /// <remarks>This is used by applications which require long transactions</remarks>
        public void Reconnect(System.Data.IDbConnection connection)
        {
            _innerSession.Reconnect(connection);
        }

        /// <summary>
        /// Obtain a new ADO.NET connection.
        /// </summary>
        /// <remarks>
        /// This is used by applications which require long transactions
        /// </remarks>
        public void Reconnect()
        {
            _innerSession.Reconnect();
        }

        /// <summary>
        /// Re-read the state of the given instance from the underlying database, with
        /// the given <c>LockMode</c>.
        /// </summary>
        /// <param name="obj">a persistent or transient instance</param>
        /// <param name="lockMode">the lock mode to use</param>
        /// <remarks>
        /// It is inadvisable to use this to implement long-running sessions that span many
        /// business tasks. This method is, however, useful in certain special circumstances.
        /// </remarks>
        public void Refresh(object obj, LockMode lockMode)
        {
            _innerSession.Refresh(obj, lockMode);
        }

        /// <summary>
        /// Re-read the state of the given instance from the underlying database.
        /// </summary>
        /// <param name="obj">A persistent instance</param>
        /// <remarks>
        /// 	<para>
        /// It is inadvisable to use this to implement long-running sessions that span many
        /// business tasks. This method is, however, useful in certain special circumstances.
        /// </para>
        /// 	<para>
        /// For example,
        /// <list>
        /// 			<item>Where a database trigger alters the object state upon insert or update</item>
        /// 			<item>After executing direct SQL (eg. a mass update) in the same session</item>
        /// 			<item>After inserting a <c>Blob</c> or <c>Clob</c></item>
        /// 		</list>
        /// 	</para>
        /// </remarks>
        public void Refresh(object obj)
        {
            _innerSession.Refresh(obj);
        }

        /// <summary>
        /// Persist the state of the given detached instance, reusing the current
        /// identifier value.  This operation cascades to associated instances if
        /// the association is mapped with <tt>cascade="replicate"</tt>.
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="obj">a detached instance of a persistent class</param>
        /// <param name="replicationMode"></param>
        public void Replicate(string entityName, object obj, ReplicationMode replicationMode)
        {
            _innerSession.Replicate(entityName, obj, replicationMode);
        }

        /// <summary>
        /// Persist all reachable transient objects, reusing the current identifier
        /// values. Note that this will not trigger the Interceptor of the Session.
        /// </summary>
        /// <param name="obj">a detached instance of a persistent class</param>
        /// <param name="replicationMode"></param>
        public void Replicate(object obj, ReplicationMode replicationMode)
        {
            _innerSession.Replicate(obj, replicationMode);
        }

        /// <summary>
        /// Persist the given transient instance, first assigning a generated identifier. (Or
        /// using the current value of the identifier property if the <tt>assigned</tt>
        /// generator is used.)
        /// </summary>
        /// <param name="entityName">The Entity name.</param>
        /// <param name="obj">a transient instance of a persistent class</param>
        /// <returns>the generated identifier</returns>
        /// <remarks>
        /// This operation cascades to associated instances if the
        /// association is mapped with <tt>cascade="save-update"</tt>.
        /// </remarks>
        public object Save(string entityName, object obj)
        {
            return Save(entityName, obj);
        }

        /// <summary>
        /// Persist the given transient instance, using the given identifier.
        /// </summary>
        /// <param name="obj">A transient instance of a persistent class</param>
        /// <param name="id">An unused valid identifier</param>
        public void Save(object obj, object id)
        {
            _innerSession.Save(obj, id);
        }

        /// <summary>
        /// Persist the given transient instance, first assigning a generated identifier.
        /// </summary>
        /// <param name="obj">A transient instance of a persistent class</param>
        /// <returns>The generated identifier</returns>
        /// <remarks>
        /// Save will use the current value of the identifier property if the <c>Assigned</c>
        /// generator is used.
        /// </remarks>
        public object Save(object obj)
        {
            return _innerSession.Save(obj);
        }

        /// <summary>
        /// Either <see cref="M:NHibernate.ISession.Save(System.String,System.Object)"/> or <see cref="M:NHibernate.ISession.Update(System.String,System.Object)"/>
        /// the given instance, depending upon resolution of the unsaved-value checks
        /// (see the manual for discussion of unsaved-value checking).
        /// </summary>
        /// <param name="entityName">The name of the entity</param>
        /// <param name="obj">a transient or detached instance containing new or updated state</param>
        /// <seealso cref="M:NHibernate.ISession.Save(System.String,System.Object)"/>
        /// <seealso cref="M:NHibernate.ISession.Update(System.String,System.Object)"/>
        /// <remarks>
        /// This operation cascades to associated instances if the association is mapped
        /// with <tt>cascade="save-update"</tt>.
        /// </remarks>
        public void SaveOrUpdate(string entityName, object obj)
        {
            _innerSession.SaveOrUpdate(entityName, obj);
        }

        /// <summary>
        /// Either <c>Save()</c> or <c>Update()</c> the given instance, depending upon the value of
        /// its identifier property.
        /// </summary>
        /// <param name="obj">A transient instance containing new or updated state</param>
        /// <remarks>
        /// By default the instance is always saved. This behaviour may be adjusted by specifying
        /// an <c>unsaved-value</c> attribute of the identifier property mapping
        /// </remarks>
        public void SaveOrUpdate(object obj)
        {
            _innerSession.SaveOrUpdate(obj);
        }


        /// <summary>
        /// Get the <see cref="T:NHibernate.ISessionFactory"/> that created this instance.
        /// </summary>
        /// <value></value>
        public ISessionFactory SessionFactory
        {
            get { return _innerSession.SessionFactory; }
        }

        /// <summary>
        /// Sets the batch size of the session
        /// </summary>
        /// <param name="batchSize"></param>
        /// <returns></returns>
        public ISession SetBatchSize(int batchSize)
        {
            return _innerSession.SetBatchSize(batchSize);
        }

        /// <summary>
        /// Get the statistics for this session.
        /// </summary>
        /// <value></value>
        public NHibernate.Stat.ISessionStatistics Statistics
        {
            get { return _innerSession.Statistics; }
        }

        /// <summary>
        /// Get the current Unit of Work and return the associated <c>ITransaction</c> object.
        /// </summary>
        /// <value></value>
        public ITransaction Transaction
        {
            get { return _innerSession.Transaction; }
        }

        /// <summary>
        /// Update the persistent instance with the identifier of the given detached
        /// instance.
        /// </summary>
        /// <param name="entityName">The Entity name.</param>
        /// <param name="obj">a detached instance containing updated state</param>
        /// <remarks>
        /// If there is a persistent instance with the same identifier,
        /// an exception is thrown. This operation cascades to associated instances
        /// if the association is mapped with <tt>cascade="save-update"</tt>.
        /// </remarks>
        public void Update(string entityName, object obj)
        {
            _innerSession.Update(entityName, obj);
        }

        /// <summary>
        /// Update the persistent state associated with the given identifier.
        /// </summary>
        /// <param name="obj">A transient instance containing updated state</param>
        /// <param name="id">Identifier of persistent instance</param>
        /// <remarks>
        /// An exception is thrown if there is a persistent instance with the same identifier
        /// in the current session.
        /// </remarks>
        public void Update(object obj, object id)
        {
            _innerSession.Update(obj, id);
        }

        /// <summary>
        /// Update the persistent instance with the identifier of the given transient instance.
        /// </summary>
        /// <param name="obj">A transient instance containing updated state</param>
        /// <remarks>
        /// If there is a persistent instance with the same identifier, an exception is thrown. If
        /// the given transient instance has a <see langword="null"/> identifier, an exception will be thrown.
        /// </remarks>
        public void Update(object obj)
        {
            _innerSession.Update(obj);
        }



        /// <summary>
        /// Queries the over.
        /// </summary>
        /// <typeparam name="T">The entity class</typeparam>
        /// <param name="alias">The alias.</param>
        /// <returns>
        /// <![CDATA[An ICriteria<T> object]]>
        /// </returns>
        public IQueryOver<T, T> QueryOver<T>(System.Linq.Expressions.Expression<Func<T>> alias) where T : class
        {
            return _innerSession.QueryOver<T>(alias);
        }

        /// <summary>
        /// Queries the over.
        /// </summary>
        /// <typeparam name="T">The entity class</typeparam>
        /// <returns><![CDATA[An ICriteria<T> object]]></returns>
        public IQueryOver<T, T> QueryOver<T>() where T : class
        {
            return _innerSession.QueryOver<T>();
        }


        /// <summary>
        /// The read-only status for entities (and proxies) loaded into this Session.
        /// </summary>
        /// <seealso cref="M:NHibernate.ISession.IsReadOnly(System.Object)"/>
        ///   
        /// <seealso cref="M:NHibernate.ISession.SetReadOnly(System.Object,System.Boolean)"/>
        public bool DefaultReadOnly
        {
            get
            {
                return _innerSession.DefaultReadOnly;
            }
            set
            {
                _innerSession.DefaultReadOnly = value;
            }
        }

        /// <summary>
        /// Is the specified entity (or proxy) read-only?
        /// </summary>
        /// <param name="entityOrProxy">An entity (or <see cref="T:NHibernate.Proxy.INHibernateProxy"/>)</param>
        /// <returns>
        ///   <c>true</c> if the entity (or proxy) is read-only, otherwise <c>false</c>.
        /// </returns>
        /// <seealso cref="P:NHibernate.ISession.DefaultReadOnly"/>
        ///   
        /// <seealso cref="M:NHibernate.ISession.SetReadOnly(System.Object,System.Boolean)"/>
        public bool IsReadOnly(object entityOrProxy)
        {
            return _innerSession.IsReadOnly(entityOrProxy);
        }

        /// <summary>
        /// Change the read-only status of an entity (or proxy).
        /// </summary>
        /// <param name="entityOrProxy">An entity (or <see cref="T:NHibernate.Proxy.INHibernateProxy"/>).</param>
        /// <param name="readOnly">If <c>true</c>, the entity or proxy is made read-only; if <c>false</c>, it is made modifiable.</param>
        /// <seealso cref="P:NHibernate.ISession.DefaultReadOnly"/>
        ///   
        /// <seealso cref="M:NHibernate.ISession.IsReadOnly(System.Object)"/>
        public void SetReadOnly(object entityOrProxy, bool readOnly)
        {
            _innerSession.SetReadOnly(entityOrProxy, readOnly);
        }

        /// <summary>
        /// Copy the state of the given object onto the persistent object with the
        /// given identifier. If there is no persistent instance currently associated
        /// with the session, it will be loaded. Return the persistent instance. If
        /// there is no database row with the given identifier, save the given instance
        /// and return it as a newly persistent instance. Otherwise, the given instance
        /// does not become associated with the session.
        /// </summary>
        /// <param name="obj">a persistent or transient instance with state to be copied</param>
        /// <param name="id">the identifier of the instance to copy to</param>
        /// <returns>
        /// an updated persistent instance
        /// </returns>
        [Obsolete("No direct replacement. Use Merge instead.")]
        public object SaveOrUpdateCopy(object obj, object id)
        {
            return _innerSession.SaveOrUpdateCopy(obj, id);
        }

        /// <summary>
        /// Copy the state of the given object onto the persistent object with the same
        /// identifier. If there is no persistent instance currently associated with
        /// the session, it will be loaded. Return the persistent instance. If the
        /// given instance is unsaved or does not exist in the database, save it and
        /// return it as a newly persistent instance. Otherwise, the given instance
        /// does not become associated with the session.
        /// </summary>
        /// <param name="obj">a transient instance with state to be copied</param>
        /// <returns>
        /// an updated persistent instance
        /// </returns>
        [Obsolete("Use Merge(object) instead")]
        public object SaveOrUpdateCopy(object obj)
        {
            return _innerSession.SaveOrUpdateCopy(obj);
        }

        /// <summary>
        /// Copy the state of the given object onto the persistent object with the same
        /// identifier. If there is no persistent instance currently associated with
        /// the session, it will be loaded. Return the persistent instance. If the given
        /// instance is unsaved, save a copy of and return it as a newly persistent instance.
        /// The given instance does not become associated with the session.  This operation
        /// cascades to associated instances if the association is mapped with cascade="merge".
        /// The semantics of this method are defined by JSR-220.  Name of the entity.
        /// a detached instance with state to be copied an updated persistent instance
        /// </summary>
        /// <typeparam name="T">The entity class</typeparam>
        /// <param name="entityName">Name of the entity.</param>
        /// <param name="entity">a detached instance with state to be copied</param>
        /// <returns>an updated persistent instance</returns>
        public T Merge<T>(string entityName, T entity) where T : class
        {
            return _innerSession.Merge<T>(entityName, entity);
        }

        /// <summary>
        /// Copy the state of the given object onto the persistent object with the same
        /// identifier. If there is no persistent instance currently associated with
        /// the session, it will be loaded. Return the persistent instance. If the given
        /// instance is unsaved, save a copy of and return it as a newly persistent instance.
        /// The given instance does not become associated with the session.  This operation
        /// cascades to associated instances if the association is mapped with cascade="merge".
        /// The semantics of this method are defined by JSR-220.  Name of the entity.
        /// a detached instance with state to be copied an updated persistent instance
        /// </summary>
        /// <typeparam name="T">The entity class</typeparam>
        /// <param name="entity">a detached instance with state to be copied</param>
        /// <returns>an updated persistent instance</returns>
        public T Merge<T>(T entity) where T : class
        {
            return _innerSession.Merge<T>(entity);
        }

        /// <summary>
        ///  Creates a new IQueryOver{T} for the entity class.
        /// </summary>
        /// <typeparam name="T">The entity class</typeparam>
        /// <param name="entityName">The name of the entity to Query</param>
        /// <param name="alias">The alias of the entity</param>
        /// <returns>An IQueryOver{T} object</returns>
        public IQueryOver<T, T> QueryOver<T>(string entityName, System.Linq.Expressions.Expression<Func<T>> alias) where T : class
        {
            return _innerSession.QueryOver<T>(entityName, alias);
        }

        /// <summary>
        /// Creates a new IQueryOver{T} for the entity class.
        /// </summary>
        /// <typeparam name="T">The entity class</typeparam>
        /// <param name="entityName">The name of the entity to Query</param>
        /// <returns>An IQueryOver{T} object</returns>
        public IQueryOver<T, T> QueryOver<T>(string entityName) where T : class
        {
            return _innerSession.QueryOver<T>(entityName);
        }

        #endregion

        #region IDisposable 成员


        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _innerSession.Dispose();
            HibernateEngine.UnbindSession();
        }

        #endregion

    }
}
