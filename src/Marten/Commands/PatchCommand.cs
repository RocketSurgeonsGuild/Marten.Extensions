using JetBrains.Annotations;
using Marten;
using Marten.Schema;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;

namespace Rocket.Surgery.Extensions.Marten.Commands
{
    /// <summary>
    /// PatchCommand.
    /// </summary>
    [UsedImplicitly]
    [Command(
        "patch",
        Description =
            "Evaluates the current configuration against the database and writes a patch and drop file if there are any differences"
    )]
    public class PatchCommand
    {
        private readonly IDocumentStore _store;
        private readonly ILogger<PatchCommand> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="PatchCommand" /> class.
        /// </summary>
        /// <param name="store">The store.</param>
        /// <param name="logger">logger.</param>
        public PatchCommand(IDocumentStore store, ILogger<PatchCommand> logger)
        {
            _store = store;
            _logger = logger;
        }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        [Argument(0, Description = "File (or folder) location to write the DDL file")]
        public string? FileName { get; set; }

        /// <summary>
        /// Opt-into schema creation
        /// </summary>
        [Option(Description = "Opt into also writing out any missing schema creation scripts")]
        public bool Schema { get; set; }

        /// <summary>
        /// Gets or sets the drop location for the files.
        /// </summary>
        [Option(Description = "Override the location of the drop file")]
        public string? Drop { get; set; }

        /// <summary>
        /// Use transactions
        /// </summary>
        [Option(Description = "Option to create scripts as transactional script")]
        public bool TransactionalScript { get; set; }

        /// <summary>
        /// Enable auto create all objects
        /// </summary>
        [Option(Description = "Drop and re-create things if required")]
        public bool AutoCreateAll { get; set; }

        /// <summary>
        /// Called when [execute].
        /// </summary>
        /// <returns>System.Int32.</returns>
        public int OnExecute()
        {
            try
            {
                _store.Schema.AssertDatabaseMatchesConfiguration();
                _logger.LogInformation(
                    "No differences were detected between the Marten configuration and the database"
                );

                return 0;
            }
            catch (SchemaValidationException)
            {
                var patch = _store.Schema.ToPatch(Schema, AutoCreateAll);

                _logger.LogInformation("Wrote a patch file to {FileName}", FileName);
                patch.WriteUpdateFile(FileName, TransactionalScript);

                var dropFile = Drop ?? SchemaPatch.ToDropFileName(FileName);

                _logger.LogInformation("Wrote the drop file to {DropFile}", dropFile);
                patch.WriteRollbackFile(dropFile, TransactionalScript);

                return 0;
            }
        }
    }
}