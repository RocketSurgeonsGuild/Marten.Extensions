using System;
using JetBrains.Annotations;
using Marten;
using Marten.Events.Projections;
using Marten.Events.Projections.Async;
using Microsoft.Extensions.Logging;

namespace Rocket.Surgery.Extensions.Marten.Projections
{
    /// <summary>
    /// DaemonFactory.
    /// Implements the <see cref="IDaemonFactory" />
    /// </summary>
    /// <seealso cref="IDaemonFactory" />
    [UsedImplicitly]
    class DaemonFactory : IDaemonFactory
    {
        private readonly IDocumentStore _store;
        private readonly ILoggerFactory _factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="DaemonFactory" /> class.
        /// </summary>
        /// <param name="store">The store.</param>
        /// <param name="factory">The factory.</param>
        public DaemonFactory(IDocumentStore store, ILoggerFactory factory)
        {
            _store = store;
            _factory = factory;
        }

        /// <summary>
        /// Creates the daemon.
        /// </summary>
        /// <param name="loggerType">Type of the logger.</param>
        /// <param name="viewTypes">The view types.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="projections">The projections.</param>
        /// <returns>IDaemon.</returns>
        public IDaemon CreateDaemon(Type loggerType, Type[]? viewTypes = null, DaemonSettings? settings = null, IProjection[]? projections = null)
        {
            return _store.BuildProjectionDaemon(
                logger: new DaemonLogger(_factory, loggerType),
                viewTypes: viewTypes,
                settings: settings,
                projections: projections
            );
        }

        /// <summary>
        /// Creates the daemon.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="viewTypes">The view types.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="projections">The projections.</param>
        /// <returns>IDaemon.</returns>
        public IDaemon CreateDaemon(ILogger logger, Type[]? viewTypes = null, DaemonSettings? settings = null, IProjection[]? projections = null)
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
