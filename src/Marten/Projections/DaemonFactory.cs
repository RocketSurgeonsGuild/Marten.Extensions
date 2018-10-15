using System;
using Marten;
using Marten.Events.Projections;
using Marten.Events.Projections.Async;
using Microsoft.Extensions.Logging;

namespace Rocket.Surgery.Extensions.Marten.Projections
{
    class DaemonFactory : IDaemonFactory
    {
        private readonly IDocumentStore _store;
        private readonly ILoggerFactory _factory;

        public DaemonFactory(IDocumentStore store, ILoggerFactory factory)
        {
            _store = store;
            _factory = factory;
        }

        public IDaemon CreateDaemon(Type loggerType, Type[] viewTypes = null, DaemonSettings settings = null, IProjection[] projections = null)
        {
            return _store.BuildProjectionDaemon(
                logger: new DaemonLogger(_factory, loggerType),
                viewTypes: viewTypes,
                settings: settings,
                projections: projections
            );
        }

        public IDaemon CreateDaemon(ILogger logger, Type[] viewTypes = null, DaemonSettings settings = null, IProjection[] projections = null)
        {
            return _store.BuildProjectionDaemon(
                logger: new DaemonLogger(logger),
                viewTypes: viewTypes,
                settings: settings,
                projections: projections
            );
        }
    }
}
