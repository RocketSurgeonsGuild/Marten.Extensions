using System;
using System.Collections.Generic;
using System.Data;
using Marten;
using Marten.Events;
using Marten.Events.Projections;
using Marten.Events.Projections.Async;
using Marten.Schema;
using Marten.Services;
using Marten.Storage;
using Marten.Transforms;
using Rocket.Surgery.Extensions.Marten.Security;

namespace Rocket.Surgery.Extensions.Marten
{
    internal class TransientDocumentStore : IDocumentStore
    {
        private readonly DocumentStore _documentStore;
        private readonly IEnumerable<IDocumentSessionListener> _documentSessionListeners;
        private readonly ISecurityQueryProvider _securityQueryProvider;
        private readonly IMartenUser _martenUser;

        public TransientDocumentStore(DocumentStore documentStore, IEnumerable<IDocumentSessionListener> documentSessionListeners, ISecurityQueryProvider securityQueryProvider, IMartenUser martenUser = null)
        {
            _documentStore = documentStore;
            _documentSessionListeners = documentSessionListeners;
            _securityQueryProvider = securityQueryProvider;
            _martenUser = martenUser;
        }

        void IDisposable.Dispose()
        {
            _documentStore.Dispose();
        }

        void IDocumentStore.BulkInsert<T>(IReadOnlyCollection<T> documents, BulkInsertMode mode = BulkInsertMode.InsertsOnly, int batchSize = 1000)
        {
            _documentStore.BulkInsert(documents, mode, batchSize);
        }

        void IDocumentStore.BulkInsert<T>(string tenantId, IReadOnlyCollection<T> documents, BulkInsertMode mode = BulkInsertMode.InsertsOnly, int batchSize = 1000)
        {
            _documentStore.BulkInsert(tenantId, documents, mode, batchSize);
        }

        IDocumentSession IDocumentStore.OpenSession(DocumentTracking tracking, IsolationLevel isolationLevel)
        {
            return new SecureDocumentSession(_documentStore.OpenSession(tracking, isolationLevel).RegisterListeners(_documentSessionListeners), _securityQueryProvider, _martenUser);
        }

        IDocumentSession IDocumentStore.OpenSession(string tenantId, DocumentTracking tracking,
            IsolationLevel isolationLevel)
        {
            return new SecureDocumentSession(_documentStore.OpenSession(tenantId, tracking, isolationLevel).RegisterListeners(_documentSessionListeners), _securityQueryProvider, _martenUser);
        }

        IDocumentSession IDocumentStore.OpenSession(SessionOptions options)
        {
            return new SecureDocumentSession(_documentStore.OpenSession(options).RegisterListeners(_documentSessionListeners), _securityQueryProvider, _martenUser);
        }

        IDocumentSession IDocumentStore.LightweightSession(IsolationLevel isolationLevel)
        {
            return new SecureDocumentSession(_documentStore.LightweightSession(isolationLevel).RegisterListeners(_documentSessionListeners), _securityQueryProvider, _martenUser);
        }

        IDocumentSession IDocumentStore.LightweightSession(string tenantId, IsolationLevel isolationLevel)
        {
            return new SecureDocumentSession(_documentStore.LightweightSession(tenantId, isolationLevel).RegisterListeners(_documentSessionListeners), _securityQueryProvider, _martenUser);
        }

        IDocumentSession IDocumentStore.DirtyTrackedSession(IsolationLevel isolationLevel)
        {
            return new SecureDocumentSession(_documentStore.DirtyTrackedSession(isolationLevel).RegisterListeners(_documentSessionListeners), _securityQueryProvider, _martenUser);
        }

        IDocumentSession IDocumentStore.DirtyTrackedSession(string tenantId, IsolationLevel isolationLevel)
        {
            return new SecureDocumentSession(_documentStore.DirtyTrackedSession(tenantId, isolationLevel).RegisterListeners(_documentSessionListeners), _securityQueryProvider, _martenUser);
        }

        IQuerySession IDocumentStore.QuerySession()
        {
            return new SecureQuerySession(_documentStore.QuerySession(), _securityQueryProvider, _martenUser);
        }

        IQuerySession IDocumentStore.QuerySession(string tenantId)
        {
            return new SecureQuerySession(_documentStore.QuerySession(tenantId), _securityQueryProvider, _martenUser);
        }

        IQuerySession IDocumentStore.QuerySession(SessionOptions options)
        {
            return new SecureQuerySession(_documentStore.QuerySession(options), _securityQueryProvider, _martenUser);
        }

        void IDocumentStore.BulkInsertDocuments(IEnumerable<object> documents, BulkInsertMode mode, int batchSize)
        {
            _documentStore.BulkInsertDocuments(documents, mode, batchSize);
        }

        void IDocumentStore.BulkInsertDocuments(string tenantId, IEnumerable<object> documents, BulkInsertMode mode,
            int batchSize)
        {
            _documentStore.BulkInsertDocuments(tenantId, documents, mode, batchSize);
        }

        IDaemon IDocumentStore.BuildProjectionDaemon(Type[] viewTypes, IDaemonLogger logger, DaemonSettings settings,
            IProjection[] projections)
        {
            return _documentStore.BuildProjectionDaemon(viewTypes, logger, settings, projections);
        }

        IDocumentSchema IDocumentStore.Schema => _documentStore.Schema;

        AdvancedOptions IDocumentStore.Advanced => _documentStore.Advanced;

        IDiagnostics IDocumentStore.Diagnostics => _documentStore.Diagnostics;

        IDocumentTransforms IDocumentStore.Transform => _documentStore.Transform;

        EventGraph IDocumentStore.Events => _documentStore.Events;

        ITenancy IDocumentStore.Tenancy => _documentStore.Tenancy;
    }
}
