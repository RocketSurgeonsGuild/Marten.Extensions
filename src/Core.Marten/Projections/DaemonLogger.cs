using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Marten.Events.Projections.Async;
using Microsoft.Extensions.Logging;

namespace Rocket.Surgery.Core.Marten.Projections
{
    /// <summary>
    /// A default logger for any Daemons
    /// </summary>
    public class DaemonLogger : IDaemonLogger
    {
        private readonly ILogger _logger;

        /// <summary>
        /// Constructor for fun
        /// </summary>
        /// <param name="loggerFactory"></param>
        /// <param name="type"></param>
        public DaemonLogger(ILoggerFactory loggerFactory, Type type)
        {
            _logger = loggerFactory.CreateLogger($"[Marten Daemon]:{type.Name}");
        }

        /// <summary>
        /// Constructor for fun
        /// </summary>
        /// <param name="logger"></param>
        public DaemonLogger(ILogger logger)
        {
            _logger = logger;
        }

        /// <inheritdoc cref="IDaemonLogger" />
        public void BeginStartAll(IEnumerable<IProjectionTrack> values)
        {
            _logger.LogInformation("Starting all tracks: {@Tracks}", values);
        }

        /// <inheritdoc cref="IDaemonLogger" />
        public void DeterminedStartingPosition(IProjectionTrack track)
        {
            _logger.LogInformation("Projection {@Track} is starting > {LastEncountered}", new { track.LastEncountered, track.IsRunning, track.ViewType }, track.LastEncountered);
        }

        /// <inheritdoc cref="IDaemonLogger" />
        public void FinishedStartingAll()
        {
            _logger.LogInformation("Finished starting the async daemon");
        }

        /// <inheritdoc cref="IDaemonLogger" />
        public void BeginRebuildAll(IEnumerable<IProjectionTrack> values)
        {
            _logger.LogInformation("Beginning a Rebuild of {@Tracks}", values);
        }

        /// <inheritdoc cref="IDaemonLogger" />
        public void FinishRebuildAll(TaskStatus status, AggregateException exception)
        {
            if (exception is null)
            {
                _logger.LogInformation("Finished RebuildAll with status {Status}", status);
            }
            else
            {
                _logger.LogError(0, exception.Flatten(), "Finished RebuildAll with status {Status}", status);
            }
        }

        /// <inheritdoc cref="IDaemonLogger" />
        public void BeginStopAll()
        {
            _logger.LogInformation("Beginning to stop the Async Daemon");
        }

        /// <inheritdoc cref="IDaemonLogger" />
        public void AllStopped()
        {
            _logger.LogInformation("Daemon stopped successfully");
        }

        /// <inheritdoc cref="IDaemonLogger" />
        public void PausingFetching(IProjectionTrack track, long lastEncountered)
        {
            _logger.LogInformation("Pausing fetching for {@Track}, last encountered {LastEncountered}", new { track.LastEncountered, track.IsRunning, track.ViewType }, track.LastEncountered);
        }

        /// <inheritdoc cref="IDaemonLogger" />
        public void FetchStarted(IProjectionTrack track)
        {
            _logger.LogInformation("Starting fetching for {@Track}", new { track.LastEncountered, track.IsRunning, track.ViewType }, track.LastEncountered);
        }

        /// <inheritdoc cref="IDaemonLogger" />
        public void FetchingIsAtEndOfEvents(IProjectionTrack track)
        {
            _logger.LogInformation("Fetching is at the end of the event log for {@Track}", new { track.LastEncountered, track.IsRunning, track.ViewType });
        }

        /// <inheritdoc cref="IDaemonLogger" />
        public void FetchingStopped(IProjectionTrack track)
        {
            _logger.LogInformation("Stopped event fetching for {@Track}", new { track.LastEncountered, track.IsRunning, track.ViewType });
        }

        /// <inheritdoc cref="IDaemonLogger" />
        public void PageExecuted(EventPage page, IProjectionTrack track)
        {
            _logger.LogInformation("{@Page} executed for {@Track}", page, new { track.LastEncountered, track.IsRunning, track.ViewType });
        }

        /// <inheritdoc cref="IDaemonLogger" />
        public void FetchingFinished(IProjectionTrack track, long lastEncountered)
        {
            _logger.LogInformation("Fetching finished for {@Track} at event {LastEncountered}", new { track.LastEncountered, track.IsRunning, track.ViewType }, track.LastEncountered);
        }

        /// <inheritdoc cref="IDaemonLogger" />
        public void StartingProjection(IProjectionTrack track, DaemonLifecycle lifecycle)
        {
            _logger.LogInformation("Starting projection {@Track} running as {Lifecycle}", new { track.LastEncountered, track.IsRunning, track.ViewType }, lifecycle);
        }

        /// <inheritdoc cref="IDaemonLogger" />
        public void Stopping(IProjectionTrack track)
        {
            _logger.LogInformation("Stopping projection {@Track}", new { track.LastEncountered, track.IsRunning, track.ViewType });
        }

        /// <inheritdoc cref="IDaemonLogger" />
        public void Stopped(IProjectionTrack track)
        {
            _logger.LogInformation("Stopped projection {@Track}", new { track.LastEncountered, track.IsRunning, track.ViewType });
        }

        /// <inheritdoc cref="IDaemonLogger" />
        public void ProjectionBackedUp(IProjectionTrack track, int cachedEventCount, EventPage page)
        {
            _logger.LogInformation("Projection {@Track} is backed up with {CachedEventCount} events in memory, last page fetched was {@Page}", new { track.LastEncountered, track.IsRunning, track.ViewType }, cachedEventCount, page);
        }

        /// <inheritdoc cref="IDaemonLogger" />
        public void ClearingExistingState(IProjectionTrack track)
        {
            _logger.LogInformation("Clearing the existing state for projection {@Track}", new { track.LastEncountered, track.IsRunning, track.ViewType });
        }

        /// <inheritdoc cref="IDaemonLogger" />
        public void Error(Exception exception)
        {
            _logger.LogError(new EventId(), exception, "Error");
        }
    }

    /// <summary>
    /// A default logger that can be resolved by convention
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DaemonLogger<T> : DaemonLogger
    {
        /// <param name="loggerFactory"></param>
        public DaemonLogger(ILoggerFactory loggerFactory) : base(loggerFactory, typeof(T)){}
    }
}
