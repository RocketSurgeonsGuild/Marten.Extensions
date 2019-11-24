using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Marten.Events.Projections.Async;
using Microsoft.Extensions.Logging;

namespace Rocket.Surgery.Extensions.Marten.Projections
{
    /// <summary>
    /// A default logger for any Daemons
    /// Implements the <see cref="IDaemonLogger" />
    /// </summary>
    /// <seealso cref="IDaemonLogger" />
    public class DaemonLogger : IDaemonLogger
    {
        private readonly ILogger _logger;

        /// <summary>
        /// Constructor for fun
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="type">The type.</param>
        public DaemonLogger(ILoggerFactory loggerFactory, Type type)
            => _logger = loggerFactory.CreateLogger($"[Marten Daemon]:{type.Name}");

        /// <summary>
        /// Constructor for fun
        /// </summary>
        /// <param name="logger">The logger.</param>
        public DaemonLogger(ILogger logger) => _logger = logger;

        /// <summary>
        /// Begins the start all.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <inheritdoc cref="IDaemonLogger" />
        public void BeginStartAll(IEnumerable<IProjectionTrack> values)
            => _logger.LogInformation("Starting all tracks: {@Tracks}", values);

        /// <summary>
        /// Determineds the starting position.
        /// </summary>
        /// <param name="track">The track.</param>
        /// <inheritdoc cref="IDaemonLogger" />
        public void DeterminedStartingPosition(IProjectionTrack track) => _logger.LogInformation(
            "Projection {@Track} is starting > {LastEncountered}",
            new { track.LastEncountered, track.IsRunning, track.ViewType },
            track.LastEncountered
        );

        /// <summary>
        /// Finisheds the starting all.
        /// </summary>
        /// <inheritdoc cref="IDaemonLogger" />
        public void FinishedStartingAll() => _logger.LogInformation("Finished starting the async daemon");

        /// <summary>
        /// Begins the rebuild all.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <inheritdoc cref="IDaemonLogger" />
        public void BeginRebuildAll(IEnumerable<IProjectionTrack> values)
            => _logger.LogInformation("Beginning a Rebuild of {@Tracks}", values);

        /// <summary>
        /// Finishes the rebuild all.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <param name="exception">The exception.</param>
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

        /// <summary>
        /// Begins the stop all.
        /// </summary>
        /// <inheritdoc cref="IDaemonLogger" />
        public void BeginStopAll() => _logger.LogInformation("Beginning to stop the Async Daemon");

        /// <summary>
        /// Alls the stopped.
        /// </summary>
        /// <inheritdoc cref="IDaemonLogger" />
        public void AllStopped() => _logger.LogInformation("Daemon stopped successfully");

        /// <summary>
        /// Pausings the fetching.
        /// </summary>
        /// <param name="track">The track.</param>
        /// <param name="lastEncountered">The last encountered.</param>
        /// <inheritdoc cref="IDaemonLogger" />
        public void PausingFetching(IProjectionTrack track, long lastEncountered) => _logger.LogInformation(
            "Pausing fetching for {@Track}, last encountered {LastEncountered}",
            new { track.LastEncountered, track.IsRunning, track.ViewType },
            track.LastEncountered
        );

        /// <summary>
        /// Fetches the started.
        /// </summary>
        /// <param name="track">The track.</param>
        /// <inheritdoc cref="IDaemonLogger" />
        public void FetchStarted(IProjectionTrack track) => _logger.LogInformation(
            "Starting fetching for {@Track}",
            new { track.LastEncountered, track.IsRunning, track.ViewType },
            track.LastEncountered
        );

        /// <summary>
        /// Fetchings the is at end of events.
        /// </summary>
        /// <param name="track">The track.</param>
        /// <inheritdoc cref="IDaemonLogger" />
        public void FetchingIsAtEndOfEvents(IProjectionTrack track) => _logger.LogInformation(
            "Fetching is at the end of the event log for {@Track}",
            new { track.LastEncountered, track.IsRunning, track.ViewType }
        );

        /// <summary>
        /// Fetchings the stopped.
        /// </summary>
        /// <param name="track">The track.</param>
        /// <inheritdoc cref="IDaemonLogger" />
        public void FetchingStopped(IProjectionTrack track) => _logger.LogInformation(
            "Stopped event fetching for {@Track}",
            new { track.LastEncountered, track.IsRunning, track.ViewType }
        );

        /// <summary>
        /// Pages the executed.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="track">The track.</param>
        /// <inheritdoc cref="IDaemonLogger" />
        public void PageExecuted(EventPage page, IProjectionTrack track) => _logger.LogInformation(
            "{@Page} executed for {@Track}",
            page,
            new { track.LastEncountered, track.IsRunning, track.ViewType }
        );

        /// <summary>
        /// Fetchings the finished.
        /// </summary>
        /// <param name="track">The track.</param>
        /// <param name="lastEncountered">The last encountered.</param>
        /// <inheritdoc cref="IDaemonLogger" />
        public void FetchingFinished(IProjectionTrack track, long lastEncountered) => _logger.LogInformation(
            "Fetching finished for {@Track} at event {LastEncountered}",
            new { track.LastEncountered, track.IsRunning, track.ViewType },
            track.LastEncountered
        );

        /// <summary>
        /// Startings the projection.
        /// </summary>
        /// <param name="track">The track.</param>
        /// <param name="lifecycle">The lifecycle.</param>
        /// <inheritdoc cref="IDaemonLogger" />
        public void StartingProjection(IProjectionTrack track, DaemonLifecycle lifecycle) => _logger.LogInformation(
            "Starting projection {@Track} running as {Lifecycle}",
            new { track.LastEncountered, track.IsRunning, track.ViewType },
            lifecycle
        );

        /// <summary>
        /// Stoppings the specified track.
        /// </summary>
        /// <param name="track">The track.</param>
        /// <inheritdoc cref="IDaemonLogger" />
        public void Stopping(IProjectionTrack track) => _logger.LogInformation(
            "Stopping projection {@Track}",
            new { track.LastEncountered, track.IsRunning, track.ViewType }
        );

        /// <summary>
        /// Stoppeds the specified track.
        /// </summary>
        /// <param name="track">The track.</param>
        /// <inheritdoc cref="IDaemonLogger" />
        public void Stopped(IProjectionTrack track) => _logger.LogInformation(
            "Stopped projection {@Track}",
            new { track.LastEncountered, track.IsRunning, track.ViewType }
        );

        /// <summary>
        /// Projections the backed up.
        /// </summary>
        /// <param name="track">The track.</param>
        /// <param name="cachedEventCount">The cached event count.</param>
        /// <param name="page">The page.</param>
        /// <inheritdoc cref="IDaemonLogger" />
        public void ProjectionBackedUp(IProjectionTrack track, int cachedEventCount, EventPage page)
            => _logger.LogInformation(
                "Projection {@Track} is backed up with {CachedEventCount} events in memory, last page fetched was {@Page}",
                new { track.LastEncountered, track.IsRunning, track.ViewType },
                cachedEventCount,
                page
            );

        /// <summary>
        /// Clearings the state of the existing.
        /// </summary>
        /// <param name="track">The track.</param>
        /// <inheritdoc cref="IDaemonLogger" />
        public void ClearingExistingState(IProjectionTrack track) => _logger.LogInformation(
            "Clearing the existing state for projection {@Track}",
            new { track.LastEncountered, track.IsRunning, track.ViewType }
        );

        /// <summary>
        /// Errors the specified exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <inheritdoc cref="IDaemonLogger" />
        public void Error(Exception exception) => _logger.LogError(new EventId(), exception, "Error");
    }

    /// <summary>
    /// A default logger that can be resolved by convention
    /// Implements the <see cref="DaemonLogger" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="DaemonLogger" />
    public class DaemonLogger<T> : DaemonLogger
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DaemonLogger{T}" /> class.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        public DaemonLogger(ILoggerFactory loggerFactory) : base(loggerFactory, typeof(T)) { }
    }
}