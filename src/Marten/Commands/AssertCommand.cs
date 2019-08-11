using System;
using JetBrains.Annotations;
using Marten;
using Marten.Schema;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;

namespace Rocket.Surgery.Extensions.Marten.Commands
{
    /// <summary>
    /// DumpCommand.
    /// </summary>
    [UsedImplicitly]
    [Command("assert", Description = "Applies all outstanding changes to the database based on the current configuration")]
    public class AssertCommand
    {
        private readonly IDocumentStore _store;
        private readonly ILogger<AssertCommand> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssertCommand" /> class.
        /// </summary>
        /// <param name="store">The store.</param>
        /// <param name="logger">logger.</param>
        public AssertCommand(IDocumentStore store, ILogger<AssertCommand> logger)
        {
            _store = store;
            _logger = logger;
        }

        /// <summary>
        /// Called when [execute].
        /// </summary>
        /// <returns>System.Int32.</returns>
        public int OnExecute()
        {
            try
            {
                _store.Schema.AssertDatabaseMatchesConfiguration();
                _logger.LogInformation("No database differences detected.");

                return 0;
            }
            catch (SchemaValidationException e)
            {
                _logger.LogCritical("The database does not match the configuration!");
                _logger.LogWarning(e.ToString());

                _logger.LogInformation("The changes are the patch describing the difference between the database and the current configuration");

                return 1;
            }
            catch (Exception e)
            {
                _logger.LogCritical("The database does not match the configuration!");
                _logger.LogWarning(e.ToString());

                return 1;
            }
        }
    }
}
