using System;
using JetBrains.Annotations;
using Marten;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;

namespace Rocket.Surgery.Extensions.Marten.Commands
{
    /// <summary>
    /// DumpCommand.
    /// </summary>
    [UsedImplicitly]
    [Command("apply", Description = "Applies all outstanding changes to the database based on the current configuration")]
    public class ApplyCommand
    {
        private readonly IDocumentStore _store;
        private readonly ILogger<ApplyCommand> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplyCommand" /> class.
        /// </summary>
        /// <param name="store">The store.</param>
        /// <param name="logger">logger.</param>
        public ApplyCommand(IDocumentStore store, ILogger<ApplyCommand> logger)
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
                _store.Schema.ApplyAllConfiguredChangesToDatabase();
                _logger.LogInformation("Successfully applied outstanding database changes");
                return 0;
            }
            catch (Exception e)
            {
                _logger.LogCritical("Failed to apply outstanding database changes!");
                _logger.LogWarning(e.ToString());

                throw;
            }
        }
    }
}
