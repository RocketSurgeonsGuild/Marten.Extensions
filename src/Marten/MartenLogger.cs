using System;
using System.Linq;
using Marten;
using Marten.Services;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Rocket.Surgery.Extensions.Marten
{
    /// <summary>
    /// MartenLogger.
    /// Implements the <see cref="IMartenLogger" />
    /// Implements the <see cref="IMartenSessionLogger" />
    /// </summary>
    /// <seealso cref="IMartenLogger" />
    /// <seealso cref="IMartenSessionLogger" />
    public class MartenLogger : IMartenLogger, IMartenSessionLogger
    {
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="MartenLogger" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public MartenLogger(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Starts the session.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <returns>IMartenSessionLogger.</returns>
        public IMartenSessionLogger StartSession(IQuerySession session)
        {
            return this;
        }

        /// <summary>
        /// Schemas the change.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        public void SchemaChange(string sql)
        {
            _logger.LogTrace("Executing DDL change:\n{Sql}", sql);
        }

        /// <summary>
        /// Log a command that executed successfully
        /// </summary>
        /// <param name="command">The command.</param>
        public void LogSuccess(NpgsqlCommand command)
        {
            _logger.LogTrace(
                "Marten Query Success:\n{CommandText}\n{Parameter}",
                command.CommandText,
                string.Join(" - ", command.Parameters.OfType<NpgsqlParameter>().Select(x => $"{x.ParameterName}: {x.Value}"))
            );
        }

        /// <summary>
        /// Log a command that failed
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="ex">The ex.</param>
        public void LogFailure(NpgsqlCommand command, Exception ex)
        {
            _logger.LogTrace(
                (EventId)0,
                "Marten Query Failure:\n{CommandText}\n{Parameter}",
                ex,
                command.CommandText,
                string.Join(" - ", command.Parameters.OfType<NpgsqlParameter>().Select(x => $"{x.ParameterName}: {x.Value}"))
            );
        }

        /// <summary>
        /// Records the saved changes.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="commit">The commit.</param>
        public void RecordSavedChanges(IDocumentSession session, IChangeSet commit)
        {
            _logger.LogTrace(
                "Marten Save Changes:\n    Inserted: {Inserted}\n    Updated: {Updated}\n    Patched: {Patched}\n    Deleted: {Deleted}",
                commit.Inserted.Count(),
                commit.Updated.Count(),
                commit.Patches.Count(),
                commit.Deleted.Count()
            );
        }
    }
}
