using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Marten.Events.Projections;
using Marten.Events.Projections.Async;
using Microsoft.Extensions.Logging;
using Rocket.Surgery.Conventions.Reflection;

namespace Rocket.Surgery.Extensions.Marten.Projections
{
    /// <summary>
    ///  IDaemonFactory
    /// </summary>
    public interface IDaemonFactory
    {
        /// <summary>
        /// Creates the daemon.
        /// </summary>
        /// <param name="loggerType">Type of the logger.</param>
        /// <param name="viewTypes">The view types.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="projections">The projections.</param>
        /// <returns>IDaemon.</returns>
        IDaemon CreateDaemon(
            Type loggerType,
            Type[] viewTypes = null,
            DaemonSettings settings = null,
            IProjection[] projections = null);

        /// <summary>
        /// Creates the daemon.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="viewTypes">The view types.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="projections">The projections.</param>
        /// <returns>IDaemon.</returns>
        IDaemon CreateDaemon(
            ILogger logger,
            Type[] viewTypes = null,
            DaemonSettings settings = null,
            IProjection[] projections = null);
    }
}
