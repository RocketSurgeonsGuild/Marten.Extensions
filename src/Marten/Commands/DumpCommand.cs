using System;
using Baseline;
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
    [Command("dump", Description = "Dumps the entire DDL for the configured Marten database")]
    public class DumpCommand
    {
        private readonly IDocumentStore _store;
        private readonly ILogger<DumpCommand> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DumpCommand" /> class.
        /// </summary>
        /// <param name="store">The store.</param>
        /// <param name="logger">logger.</param>
        public DumpCommand(IDocumentStore store, ILogger<DumpCommand> logger)
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
        /// Write out DDL by type
        /// </summary>
        [Option(Description = "Opt into writing the DDL split out by file")]
        public bool ByType { get; set; }

        /// <summary>
        /// Use transactions
        /// </summary>
        [Option(Description = "Option to create scripts as transactional script")]
        public bool TransactionalScript { get; set; }

        /// <summary>
        /// Called when [execute].
        /// </summary>
        /// <returns>System.Int32.</returns>
        public int OnExecute()
        {
            if (ByType)
            {
                _logger.LogInformation("Writing DDL files to {FileName}", FileName);
                _store.Schema.WriteDDLByType(FileName, TransactionalScript);
            }
            else
            {
                _logger.LogInformation("Writing DDL file to ", FileName);

                try
                {
                    new FileSystem().CleanDirectory(FileName);
                }
                catch (Exception)
                {
                    _logger.LogInformation(
                        "Unable to clean the directory at {FileName} before writing new files",
                        FileName
                    );
                }

                _store.Schema.WriteDDL(FileName, TransactionalScript);
            }

            return 0;
        }
    }
}