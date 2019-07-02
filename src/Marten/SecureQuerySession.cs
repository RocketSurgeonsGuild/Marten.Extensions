using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Marten;
using Marten.Linq;
using Marten.Services.BatchQuerying;
using Marten.Storage;
using Npgsql;
using Rocket.Surgery.Extensions.Marten.Security;

namespace Rocket.Surgery.Extensions.Marten
{
    /// <summary>
    /// SecureQuerySession.
    /// Implements the <see cref="Rocket.Surgery.Extensions.Marten.ISecureQuerySession" />
    /// </summary>
    /// <seealso cref="Rocket.Surgery.Extensions.Marten.ISecureQuerySession" />
    public class SecureQuerySession : ISecureQuerySession
    {
        private readonly IQuerySession _querySession;
        private readonly ISecurityQueryProvider _securityQueryProvider;
        private readonly IMartenContext _martenContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="SecureQuerySession" /> class.
        /// </summary>
        /// <param name="querySession">The query session.</param>
        /// <param name="securityQueryProvider">The security query provider.</param>
        /// <param name="martenContext">The marten context.</param>
        public SecureQuerySession(IQuerySession querySession, ISecurityQueryProvider securityQueryProvider, IMartenContext martenContext)
        {
            _querySession = querySession;
            _securityQueryProvider = securityQueryProvider;
            _martenContext = martenContext;
        }

        /// <summary>
        /// Disposes this instance.
        /// </summary>
        public void Dispose()
        {
            _querySession.Dispose();
        }

        /// <summary>
        /// Loads the specified identifier.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id">The identifier.</param>
        /// <returns>T.</returns>
        public T Load<T>(string id)
        {
            return _querySession.Load<T>(id);
        }

        /// <summary>
        /// Loads the asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id">The identifier.</param>
        /// <param name="token">The token.</param>
        /// <returns>Task{T}.</returns>
        public Task<T> LoadAsync<T>(string id, CancellationToken token = new CancellationToken())
        {
            return _querySession.LoadAsync<T>(id, token);
        }

        /// <summary>
        /// Loads the specified identifier.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id">The identifier.</param>
        /// <returns>T.</returns>
        public T Load<T>(int id)
        {
            return _querySession.Load<T>(id);
        }

        /// <summary>
        /// Loads the specified identifier.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id">The identifier.</param>
        /// <returns>T.</returns>
        public T Load<T>(long id)
        {
            return _querySession.Load<T>(id);
        }

        /// <summary>
        /// Loads the specified identifier.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id">The identifier.</param>
        /// <returns>T.</returns>
        public T Load<T>(Guid id)
        {
            return _querySession.Load<T>(id);
        }

        /// <summary>
        /// Loads the asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id">The identifier.</param>
        /// <param name="token">The token.</param>
        /// <returns>Task{T}urns>
        public Task<T> LoadAsync<T>(int id, CancellationToken token = new CancellationToken())
        {
            return _querySession.LoadAsync<T>(id, token);
        }

        /// <summary>
        /// Loads the asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id">The identifier.</param>
        /// <param name="token">The token.</param>
        /// <returns>Task{T}.</returns>
        public Task<T> LoadAsync<T>(long id, CancellationToken token = new CancellationToken())
        {
            return _querySession.LoadAsync<T>(id, token);
        }

        /// <summary>
        /// Loads the asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id">The identifier.</param>
        /// <param name="token">The token.</param>
        /// <returns>Task{T}.</returns>
        public Task<T> LoadAsync<T>(Guid id, CancellationToken token = new CancellationToken())
        {
            return _querySession.LoadAsync<T>(id, token);
        }

        /// <summary>
        /// Queries this instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>IMartenQueryable{T}.</returns>
        public IMartenQueryable<T> Query<T>()
        {
            return (IMartenQueryable<T>) _querySession.Query<T>()
                .OnlyItemsTheUserCanSee(_securityQueryProvider, _martenContext);
        }

        /// <summary>
        /// Queries the specified SQL.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">The SQL.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>IReadOnlyList{T}.</returns>
        public IReadOnlyList<T> Query<T>(string sql, params object[] parameters)
        {
            return _querySession.Query<T>(sql, parameters)
                .OnlyItemsTheUserCanSee(_securityQueryProvider, _martenContext)
                .ToList();
        }

        /// <summary>
        /// query as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">The SQL.</param>
        /// <param name="token">The token.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Task{IReadOnlyList{T}}.</returns>
        public async Task<IReadOnlyList<T>> QueryAsync<T>(string sql, CancellationToken token = new CancellationToken(), params object[] parameters)
        {
            return (await _querySession.QueryAsync<T>(sql, token, parameters).ConfigureAwait(false))
                .OnlyItemsTheUserCanSee(_securityQueryProvider, _martenContext)
                .ToList();
        }

        /// <summary>
        /// Creates the batch query.
        /// </summary>
        /// <returns>IBatchedQuery.</returns>
        public IBatchedQuery CreateBatchQuery()
        {
            return _querySession.CreateBatchQuery();
        }

        /// <summary>
        /// Queries the specified query.
        /// </summary>
        /// <typeparam name="TDoc">The type of the t document.</typeparam>
        /// <typeparam name="TOut">The type of the t out.</typeparam>
        /// <param name="query">The query.</param>
        /// <returns>TOut.</returns>
        public TOut Query<TDoc, TOut>(ICompiledQuery<TDoc, TOut> query)
        {
            return _querySession.Query(query);
        }

        /// <summary>
        /// Queries the asynchronous.
        /// </summary>
        /// <typeparam name="TDoc">The type of the t document.</typeparam>
        /// <typeparam name="TOut">The type of the t out.</typeparam>
        /// <param name="query">The query.</param>
        /// <param name="token">The token.</param>
        /// <returns>Task{TOut}.</returns>
        public Task<TOut> QueryAsync<TDoc, TOut>(ICompiledQuery<TDoc, TOut> query, CancellationToken token = new CancellationToken())
        {
            return _querySession.QueryAsync(query, token);
        }

        /// <summary>
        /// Loads the many.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ids">The ids.</param>
        /// <returns>IReadOnlyList{T}.</returns>
        public IReadOnlyList<T> LoadMany<T>(params string[] ids)
        {
            return _querySession.LoadMany<T>(ids)
                .OnlyItemsTheUserCanSee(_securityQueryProvider, _martenContext)
                .ToList();
        }

        /// <summary>
        /// Loads the many.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ids">The ids.</param>
        /// <returns>IReadOnlyList{T}.</returns>
        public IReadOnlyList<T> LoadMany<T>(params Guid[] ids)
        {
            return _querySession.LoadMany<T>(ids)
                .OnlyItemsTheUserCanSee(_securityQueryProvider, _martenContext)
                .ToList();
        }

        /// <summary>
        /// Loads the many.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ids">The ids.</param>
        /// <returns>IReadOnlyList{T}.</returns>
        public IReadOnlyList<T> LoadMany<T>(params int[] ids)
        {
            return _querySession.LoadMany<T>(ids)
                .OnlyItemsTheUserCanSee(_securityQueryProvider, _martenContext)
                .ToList();
        }

        /// <summary>
        /// Loads the many.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ids">The ids.</param>
        /// <returns>IReadOnlyList{T}.</returns>
        public IReadOnlyList<T> LoadMany<T>(params long[] ids)
        {
            return _querySession.LoadMany<T>(ids)
                .OnlyItemsTheUserCanSee(_securityQueryProvider, _martenContext)
                .ToList();
        }

        /// <summary>
        /// load many as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ids">The ids.</param>
        /// <returns>Task{IReadOnlyList{T}}.</returns>
        public async Task<IReadOnlyList<T>> LoadManyAsync<T>(params string[] ids)
        {
            return (await _querySession.LoadManyAsync<T>(ids).ConfigureAwait(false))
                .OnlyItemsTheUserCanSee(_securityQueryProvider, _martenContext)
                .ToList();
        }

        /// <summary>
        /// load many as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ids">The ids.</param>
        /// <returns>Task{IReadOnlyList{T}}.</returns>
        public async Task<IReadOnlyList<T>> LoadManyAsync<T>(params Guid[] ids)
        {
            return (await _querySession.LoadManyAsync<T>(ids).ConfigureAwait(false))
                .OnlyItemsTheUserCanSee(_securityQueryProvider, _martenContext)
                .ToList();
        }

        /// <summary>
        /// load many as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ids">The ids.</param>
        /// <returns>Task{IReadOnlyList{T}}.</returns>
        public async Task<IReadOnlyList<T>> LoadManyAsync<T>(params int[] ids)
        {
            return (await _querySession.LoadManyAsync<T>(ids).ConfigureAwait(false))
                .OnlyItemsTheUserCanSee(_securityQueryProvider, _martenContext)
                .ToList();
        }

        /// <summary>
        /// load many as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ids">The ids.</param>
        /// <returns>Task{IReadOnlyList{T}}urns>
        public async Task<IReadOnlyList<T>> LoadManyAsync<T>(params long[] ids)
        {
            return (await _querySession.LoadManyAsync<T>(ids).ConfigureAwait(false))
                .OnlyItemsTheUserCanSee(_securityQueryProvider, _martenContext)
                .ToList();
        }

        /// <summary>
        /// load many as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="token">The token.</param>
        /// <param name="ids">The ids.</param>
        /// <returns>Task{IReadOnlyList{T}}.</returns>
        public async Task<IReadOnlyList<T>> LoadManyAsync<T>(CancellationToken token, params string[] ids)
        {
            return (await _querySession.LoadManyAsync<T>(token, ids).ConfigureAwait(false))
                .OnlyItemsTheUserCanSee(_securityQueryProvider, _martenContext)
                .ToList();
        }

        /// <summary>
        /// load many as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="token">The token.</param>
        /// <param name="ids">The ids.</param>
        /// <returns>Task{IReadOnlyList{T}}.</returns>
        public async Task<IReadOnlyList<T>> LoadManyAsync<T>(CancellationToken token, params Guid[] ids)
        {
            return (await _querySession.LoadManyAsync<T>(token, ids).ConfigureAwait(false))
                .OnlyItemsTheUserCanSee(_securityQueryProvider, _martenContext)
                .ToList();
        }

        /// <summary>
        /// load many as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="token">The token.</param>
        /// <param name="ids">The ids.</param>
        /// <returns>Task{IReadOnlyList{T}}.</returns>
        public async Task<IReadOnlyList<T>> LoadManyAsync<T>(CancellationToken token, params int[] ids)
        {
            return (await _querySession.LoadManyAsync<T>(token, ids).ConfigureAwait(false))
                .OnlyItemsTheUserCanSee(_securityQueryProvider, _martenContext)
                .ToList();
        }

        /// <summary>
        /// load many as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="token">The token.</param>
        /// <param name="ids">The ids.</param>
        /// <returns>Task{IReadOnlyList{T}}.</returns>
        public async Task<IReadOnlyList<T>> LoadManyAsync<T>(CancellationToken token, params long[] ids)
        {
            return (await _querySession.LoadManyAsync<T>(token, ids).ConfigureAwait(false))
                .OnlyItemsTheUserCanSee(_securityQueryProvider, _martenContext)
                .ToList();
        }

        /// <summary>
        /// Versions for.
        /// </summary>
        /// <typeparam name="TDoc">The type of the t document.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns>System.Nullable{Guid}.</returns>
        public Guid? VersionFor<TDoc>(TDoc entity)
        {
            return _querySession.VersionFor(entity);
        }

        /// <summary>
        /// Searches the specified query text.
        /// </summary>
        /// <typeparam name="TDoc">The type of the t document.</typeparam>
        /// <param name="queryText">The query text.</param>
        /// <param name="config">The configuration.</param>
        /// <returns>IReadOnlyList{TDoc}.</returns>
        public IReadOnlyList<TDoc> Search<TDoc>(string queryText, string config = "english")
        {
            return _querySession.Search<TDoc>(queryText, config);
        }

        /// <summary>
        /// Searches the asynchronous.
        /// </summary>
        /// <typeparam name="TDoc">The type of the t document.</typeparam>
        /// <param name="queryText">The query text.</param>
        /// <param name="config">The configuration.</param>
        /// <param name="token">The token.</param>
        /// <returns>Task{IReadOnlyList{TDoc}}.</returns>
        public Task<IReadOnlyList<TDoc>> SearchAsync<TDoc>(string queryText, string config = "english", CancellationToken token = default)
        {
            return _querySession.SearchAsync<TDoc>(queryText, config);
        }

        /// <summary>
        /// Plains the text search.
        /// </summary>
        /// <typeparam name="TDoc">The type of the t document.</typeparam>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="config">The configuration.</param>
        /// <returns>IReadOnlyList{TDoc}.</returns>
        public IReadOnlyList<TDoc> PlainTextSearch<TDoc>(string searchTerm, string config = "english")
        {
            return _querySession.PlainTextSearch<TDoc>(searchTerm, config);
        }

        /// <summary>
        /// Plains the text search asynchronous.
        /// </summary>
        /// <typeparam name="TDoc">The type of the t document.</typeparam>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="config">The configuration.</param>
        /// <param name="token">The token.</param>
        /// <returns>Task{IReadOnlyList{TDoc}}.</returns>
        public Task<IReadOnlyList<TDoc>> PlainTextSearchAsync<TDoc>(string searchTerm, string config = "english", CancellationToken token = default)
        {
            return _querySession.PlainTextSearchAsync<TDoc>(searchTerm, config);
        }

        /// <summary>
        /// Phrases the search.
        /// </summary>
        /// <typeparam name="TDoc">The type of the t document.</typeparam>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="config">The configuration.</param>
        /// <returns>IReadOnlyList{TDoc}.</returns>
        public IReadOnlyList<TDoc> PhraseSearch<TDoc>(string searchTerm, string config = "english")
        {
            return _querySession.PhraseSearch<TDoc>(searchTerm, config);
        }

        /// <summary>
        /// Phrases the search asynchronous.
        /// </summary>
        /// <typeparam name="TDoc">The type of the t document.</typeparam>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="config">The configuration.</param>
        /// <param name="token">The token.</param>
        /// <returns>Task{IReadOnlyList{TDoc}}.</returns>
        public Task<IReadOnlyList<TDoc>> PhraseSearchAsync<TDoc>(string searchTerm, string config = "english", CancellationToken token = default)
        {
            return _querySession.PhraseSearchAsync<TDoc>(searchTerm, config);
        }

        /// <summary>
        /// Webs the style search.
        /// </summary>
        /// <typeparam name="TDoc">The type of the t document.</typeparam>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="regConfig">The reg configuration.</param>
        /// <returns>IReadOnlyList{TDoc}.</returns>
        public IReadOnlyList<TDoc> WebStyleSearch<TDoc>(string searchTerm, string regConfig = "english")
        {
            return _querySession.WebStyleSearch<TDoc>(searchTerm, regConfig);
        }

        /// <summary>
        /// Webs the style search asynchronous.
        /// </summary>
        /// <typeparam name="TDoc">The type of the t document.</typeparam>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="regConfig">The reg configuration.</param>
        /// <param name="token">The token.</param>
        /// <returns>Task{IReadOnlyList{TDoc}turns>
        public Task<IReadOnlyList<TDoc>> WebStyleSearchAsync<TDoc>(string searchTerm, string regConfig = "english",
            CancellationToken token = new CancellationToken())
        {
            return _querySession.WebStyleSearchAsync<TDoc>(searchTerm, regConfig);
        }

        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <value>The connection.</value>
        public NpgsqlConnection Connection => _querySession.Connection;

        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        /// <value>The logger.</value>
        public IMartenSessionLogger Logger
        {
            get => _querySession.Logger;
            set => _querySession.Logger = value;
        }

        /// <summary>
        /// Gets the request count.
        /// </summary>
        /// <value>The request count.</value>
        public int RequestCount => _querySession.RequestCount;

        /// <summary>
        /// Gets the document store.
        /// </summary>
        /// <value>The document store.</value>
        public IDocumentStore DocumentStore => _querySession.DocumentStore;

        /// <summary>
        /// Gets the json.
        /// </summary>
        /// <value>The json.</value>
        public IJsonLoader Json => _querySession.Json;

        /// <summary>
        /// Gets the tenant.
        /// </summary>
        /// <value>The tenant.</value>
        public ITenant Tenant => _querySession.Tenant;

        /// <summary>
        /// Gets the serializer.
        /// </summary>
        /// <value>The serializer.</value>
        public ISerializer Serializer => _querySession.Serializer;
    }
}
