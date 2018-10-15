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
    public interface IDaemonFactory
    {
        IDaemon CreateDaemon(
            Type loggerType,
            Type[] viewTypes = null,
            DaemonSettings settings = null,
            IProjection[] projections = null);

        IDaemon CreateDaemon(
            ILogger logger,
            Type[] viewTypes = null,
            DaemonSettings settings = null,
            IProjection[] projections = null);
    }
}
