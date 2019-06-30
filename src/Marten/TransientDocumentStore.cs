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
    /// <summary>
    /// Class TransientDocumentStore.
    /// Implements the <see cref="Marten.IDocumentStore" />
    /// </summary>
    /// <seealso cref="Marten.IDocumentStore" />
    internal class TransientDocumentStore : IDocumentStore
    {
        private readonly DocumentStore _documentStore;
        private readonly IEnumerable<IDocumentSessionListener> _documentSessionListeners;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransientDocumentStore"/> class.
        /// </summary>
        /// <param name="documentStore">The document store.</param>
        /// <param name="documentSessionListeners">The document session listeners.</param>
        public TransientDocumentStore(DocumentStore documentStore, IEnumerable<IDocumentSessionListener> documentSessionListeners)
        {
            _documentStore = documentStore;
            _documentSessionListeners = documentSessionListeners;
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
            return _documentStore.OpenSession(tracking, isolationLevel).RegisterListeners(_documentSessionListeners);
        }

        IDocumentSession IDocumentStore.OpenSession(string tenantId, DocumentTracking tracking,
            IsolationLevel isolationLevel)
        {
            return _documentStore.OpenSession(tenantId, tracking, isolationLevel).RegisterListeners(_documentSessionListeners);
        }

        IDocumentSession IDocumentStore.OpenSession(SessionOptions options)
        {
            return _documentStore.OpenSession(options).RegisterListeners(_documentSessionListeners);
        }

        IDocumentSession IDocumentStore.LightweightSession(IsolationLevel isolationLevel)
        {
            return _documentStore.LightweightSession(isolationLevel).RegisterListeners(_documentSessionListeners);
        }

        IDocumentSession IDocumentStore.LightweightSession(string tenantId, IsolationLevel isolationLevel)
        {
            return _documentStore.LightweightSession(tenantId, isolationLevel).RegisterListeners(_documentSessionListeners);
        }

        IDocumentSession IDocumentStore.DirtyTrackedSession(IsolationLevel isolationLevel)
        {
            return _documentStore.DirtyTrackedSession(isolationLevel).RegisterListeners(_documentSessionListeners);
        }

        IDocumentSession IDocumentStore.DirtyTrackedSession(string tenantId, IsolationLevel isolationLevel)
        {
            return _documentStore.DirtyTrackedSession(tenantId, isolationLevel).RegisterListeners(_documentSessionListeners);
        }

        IQuerySession IDocumentStore.QuerySession()
        {
            return _documentStore.QuerySession();
        }

        IQuerySession IDocumentStore.QuerySession(string tenantId)
        {
            return _documentStore.QuerySession(tenantId);
        }

        IQuerySession IDocumentStore.QuerySession(SessionOptions options)
        {
            return _documentStore.QuerySession(options);
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

    /// <summary>
    /// Class SecureQuerySessionExtensions.
    /// </summary>
    public static class SecureQuerySessionExtensions
    {
        /// <summary>
        /// Secures the query session.
        /// </summary>
        /// <param name="store">The store.</param>
        /// <param name="securityQueryProvider">The security query provider.</param>
        /// <param name="martenContext">The marten context.</param>
        /// <returns>ISecureQuerySession.</returns>
        public static ISecureQuerySession SecureQuerySession(this IDocumentStore store, ISecurityQueryProvider securityQueryProvider, IMartenContext martenContext)
        {
            return new SecureQuerySession(store.QuerySession(), securityQueryProvider, martenContext);
        }

        /// <summary>
        /// Secures the query session.
        /// </summary>
        /// <param name="store">The store.</param>
        /// <param name="tenantId">The tenant identifier.</param>
        /// <param name="securityQueryProvider">The security query provider.</param>
        /// <param name="martenContext">The marten context.</param>
        /// <returns>ISecureQuerySession.</returns>
        public static ISecureQuerySession SecureQuerySession(this IDocumentStore store, string tenantId, ISecurityQueryProvider securityQueryProvider, IMartenContext martenContext)
        {
            return new SecureQuerySession(store.QuerySession(tenantId), securityQueryProvider, martenContext);
        }

        /// <summary>
        /// Secures the query session.
        /// </summary>
        /// <param name="store">The store.</param>
        /// <param name="options">The options.</param>
        /// <param name="securityQueryProvider">The security query provider.</param>
        /// <param name="martenContext">The marten context.</param>
        /// <returns>ISecureQuerySession.</returns>
        public static ISecureQuerySession SecureQuerySession(this IDocumentStore store, SessionOptions options, ISecurityQueryProvider securityQueryProvider, IMartenContext martenContext)
        {
            return new SecureQuerySession(store.QuerySession(options), securityQueryProvider, martenContext);
        }
    }
}
