using System;
using System.Linq;
using Marten;
using Marten.Services;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Rocket.Surgery.Core.Marten
{
    public class MartenLogger : IMartenLogger, IMartenSessionLogger
    {
        private readonly ILogger _logger;

        public MartenLogger(ILogger logger)
        {
            _logger = logger;
        }

        public IMartenSessionLogger StartSession(IQuerySession session)
        {
            return this;
        }

        public void SchemaChange(string sql)
        {
            _logger.LogTrace("Executing DDL change:\n{Sql}", sql);
        }

        public void LogSuccess(NpgsqlCommand command)
        {
            _logger.LogTrace(
                "Marten Query Success:\n{CommandText}\n{Parameter}",
                command.CommandText,
                string.Join(" - ", command.Parameters.OfType<NpgsqlParameter>().Select(x => $"{x.ParameterName}: {x.Value}"))
            );
        }

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
