using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Marten;
using Marten.Events;
using Marten.Linq;
using Marten.Patching;
using Marten.Services;
using Marten.Services.BatchQuerying;
using Marten.Storage;
using Npgsql;
using Rocket.Surgery.Core.Marten.Security;
using Remotion.Linq;

namespace Rocket.Surgery.Core.Marten
{
    public class SecureDocumentSession : IDocumentSession
    {
        private readonly IDocumentSession _documentSession;
        private readonly ISecurityQueryProvider _securityQueryProvider;
        private readonly IMartenUser _martenUser;

        public SecureDocumentSession(IDocumentSession documentSession, ISecurityQueryProvider securityQueryProvider, IMartenUser martenUser = null)
        {
            _documentSession = documentSession;
            _securityQueryProvider = securityQueryProvider;
            _martenUser = martenUser;
        }

        public void Dispose()
        {
            _documentSession.Dispose();
        }

        public T Load<T>(string id)
        {
            return _documentSession.Load<T>(id);
        }

        public Task<T> LoadAsync<T>(string id, CancellationToken token = new CancellationToken())
        {
            return _documentSession.LoadAsync<T>(id, token);
        }

        public T Load<T>(int id)
        {
            return _documentSession.Load<T>(id);
        }

        public T Load<T>(long id)
        {
            return _documentSession.Load<T>(id);
        }

        public T Load<T>(Guid id)
        {
            return _documentSession.Load<T>(id);
        }

        public Task<T> LoadAsync<T>(int id, CancellationToken token = new CancellationToken())
        {
            return _documentSession.LoadAsync<T>(id, token);
        }

        public Task<T> LoadAsync<T>(long id, CancellationToken token = new CancellationToken())
        {
            return _documentSession.LoadAsync<T>(id, token);
        }

        public Task<T> LoadAsync<T>(Guid id, CancellationToken token = new CancellationToken())
        {
            return _documentSession.LoadAsync<T>(id, token);
        }

        public IMartenQueryable<T> Query<T>()
        {
            return (IMartenQueryable<T>) _documentSession.Query<T>()
                .OnlyItemsTheUserCanSee(_securityQueryProvider, _martenUser);
        }

        public IReadOnlyList<T> Query<T>(string sql, params object[] parameters)
        {
            return _documentSession.Query<T>(sql, parameters)
                .OnlyItemsTheUserCanSee(_securityQueryProvider, _martenUser)
                .ToList();
        }

        public async Task<IReadOnlyList<T>> QueryAsync<T>(string sql, CancellationToken token = new CancellationToken(), params object[] parameters)
        {
            return (await _documentSession.QueryAsync<T>(sql, token, parameters).ConfigureAwait(false))
                .OnlyItemsTheUserCanSee(_securityQueryProvider, _martenUser)
                .ToList();
        }

        public IBatchedQuery CreateBatchQuery()
        {
            return _documentSession.CreateBatchQuery();
        }

        public TOut Query<TDoc, TOut>(ICompiledQuery<TDoc, TOut> query)
        {
            return _documentSession.Query(query);
        }

        public Task<TOut> QueryAsync<TDoc, TOut>(ICompiledQuery<TDoc, TOut> query, CancellationToken token = new CancellationToken())
        {
            return _documentSession.QueryAsync(query, token);
        }

        public IReadOnlyList<T> LoadMany<T>(params string[] ids)
        {
            return _documentSession.LoadMany<T>(ids)
                .OnlyItemsTheUserCanSee(_securityQueryProvider, _martenUser)
                .ToList();
        }

        public IReadOnlyList<T> LoadMany<T>(params Guid[] ids)
        {
            return _documentSession.LoadMany<T>(ids)
                .OnlyItemsTheUserCanSee(_securityQueryProvider, _martenUser)
                .ToList();
        }

        public IReadOnlyList<T> LoadMany<T>(params int[] ids)
        {
            return _documentSession.LoadMany<T>(ids)
                .OnlyItemsTheUserCanSee(_securityQueryProvider, _martenUser)
                .ToList();
        }

        public IReadOnlyList<T> LoadMany<T>(params long[] ids)
        {
            return _documentSession.LoadMany<T>(ids)
                .OnlyItemsTheUserCanSee(_securityQueryProvider, _martenUser)
                .ToList();
        }

        public async Task<IReadOnlyList<T>> LoadManyAsync<T>(params string[] ids)
        {
            return (await _documentSession.LoadManyAsync<T>(ids).ConfigureAwait(false))
                .OnlyItemsTheUserCanSee(_securityQueryProvider, _martenUser)
                .ToList();
        }

        public async Task<IReadOnlyList<T>> LoadManyAsync<T>(params Guid[] ids)
        {
            return (await _documentSession.LoadManyAsync<T>(ids).ConfigureAwait(false))
                .OnlyItemsTheUserCanSee(_securityQueryProvider, _martenUser)
                .ToList();
        }

        public async Task<IReadOnlyList<T>> LoadManyAsync<T>(params int[] ids)
        {
            return (await _documentSession.LoadManyAsync<T>(ids).ConfigureAwait(false))
                .OnlyItemsTheUserCanSee(_securityQueryProvider, _martenUser)
                .ToList();
        }

        public async Task<IReadOnlyList<T>> LoadManyAsync<T>(params long[] ids)
        {
            return (await _documentSession.LoadManyAsync<T>(ids).ConfigureAwait(false))
                .OnlyItemsTheUserCanSee(_securityQueryProvider, _martenUser)
                .ToList();
        }

        public async Task<IReadOnlyList<T>> LoadManyAsync<T>(CancellationToken token, params string[] ids)
        {
            return (await _documentSession.LoadManyAsync<T>(token, ids).ConfigureAwait(false))
                .OnlyItemsTheUserCanSee(_securityQueryProvider, _martenUser)
                .ToList();
        }

        public async Task<IReadOnlyList<T>> LoadManyAsync<T>(CancellationToken token, params Guid[] ids)
        {
            return (await _documentSession.LoadManyAsync<T>(token, ids).ConfigureAwait(false))
                .OnlyItemsTheUserCanSee(_securityQueryProvider, _martenUser)
                .ToList();
        }

        public async Task<IReadOnlyList<T>> LoadManyAsync<T>(CancellationToken token, params int[] ids)
        {
            return (await _documentSession.LoadManyAsync<T>(token, ids).ConfigureAwait(false))
                .OnlyItemsTheUserCanSee(_securityQueryProvider, _martenUser)
                .ToList();
        }

        public async Task<IReadOnlyList<T>> LoadManyAsync<T>(CancellationToken token, params long[] ids)
        {
            return (await _documentSession.LoadManyAsync<T>(token, ids).ConfigureAwait(false))
                .OnlyItemsTheUserCanSee(_securityQueryProvider, _martenUser)
                .ToList();
        }

        public Guid? VersionFor<TDoc>(TDoc entity)
        {
            return _documentSession.VersionFor(entity);
        }

        public NpgsqlConnection Connection => _documentSession.Connection;

        public IMartenSessionLogger Logger
        {
            get => _documentSession.Logger;
            set => _documentSession.Logger = value;
        }

        public int RequestCount => _documentSession.RequestCount;

        public IDocumentStore DocumentStore => _documentSession.DocumentStore;

        public IJsonLoader Json => _documentSession.Json;

        public ITenant Tenant => _documentSession.Tenant;

        public ISerializer Serializer => _documentSession.Serializer;

        public void Delete<T>(T entity)
        {
            _documentSession.Delete(entity);
        }

        public void Delete<T>(int id)
        {
            _documentSession.Delete<T>(id);
        }

        public void Delete<T>(long id)
        {
            _documentSession.Delete<T>(id);
        }

        public void Delete<T>(Guid id)
        {
            _documentSession.Delete<T>(id);
        }

        public void Delete<T>(string id)
        {
            _documentSession.Delete<T>(id);
        }

        public void DeleteWhere<T>(Expression<Func<T, bool>> expression)
        {
            _documentSession.DeleteWhere(expression);
        }

        public void SaveChanges()
        {
            _documentSession.SaveChanges();
        }

        public Task SaveChangesAsync(CancellationToken token = new CancellationToken())
        {
            return _documentSession.SaveChangesAsync(token);
        }

        public void Store<T>(params T[] entities)
        {
            _documentSession.Store(entities);
        }

        public void Store<T>(string tenantId, params T[] entities)
        {
            _documentSession.Store(tenantId, entities);
        }

        public void Store<T>(T entity, Guid version)
        {
            _documentSession.Store(entity, version);
        }

        public void Insert<T>(params T[] entities)
        {
            _documentSession.Insert(entities);
        }

        public void Update<T>(params T[] entities)
        {
            _documentSession.Update(entities);
        }

        public void InsertObjects(IEnumerable<object> documents)
        {
            _documentSession.InsertObjects(documents);
        }

        public void StoreObjects(IEnumerable<object> documents)
        {
            _documentSession.StoreObjects(documents);
        }

        public IPatchExpression<T> Patch<T>(int id)
        {
            return _documentSession.Patch<T>(id);
        }

        public IPatchExpression<T> Patch<T>(long id)
        {
            return _documentSession.Patch<T>(id);
        }

        public IPatchExpression<T> Patch<T>(string id)
        {
            return _documentSession.Patch<T>(id);
        }

        public IPatchExpression<T> Patch<T>(Guid id)
        {
            return _documentSession.Patch<T>(id);
        }

        public IPatchExpression<T> Patch<T>(Expression<Func<T, bool>> @where)
        {
            return _documentSession.Patch(@where);
        }

        public IPatchExpression<T> Patch<T>(IWhereFragment fragment)
        {
            return _documentSession.Patch<T>(fragment);
        }

        public void QueueOperation(IStorageOperation storageOperation)
        {
            _documentSession.QueueOperation(storageOperation);
        }

        public void Eject<T>(T document)
        {
            _documentSession.Eject(document);
        }

        public void EjectAllOfType(Type type)
        {
            _documentSession.EjectAllOfType(type);
        }

        public IUnitOfWork PendingChanges => _documentSession.PendingChanges;

        public IEventStore Events => _documentSession.Events;

        public ConcurrencyChecks Concurrency => _documentSession.Concurrency;

        public IList<IDocumentSessionListener> Listeners => _documentSession.Listeners;
    }
}
