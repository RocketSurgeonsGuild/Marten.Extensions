using JetBrains.Annotations;
using Marten;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Options;

namespace Rocket.Surgery.Extensions.Marten.Commands
{
    /// <summary>
    /// MartenCommand.
    /// </summary>
    [UsedImplicitly]
    [Command("marten", ThrowOnUnexpectedArgument = false)]
    [Subcommand(typeof(PatchCommand))]
    [Subcommand(typeof(AssertCommand))]
    [Subcommand(typeof(ApplyCommand))]
    [Subcommand(typeof(DumpCommand))]
    class MartenCommand
    {
        private readonly CommandLineApplication _application;

        /// <summary>
        /// Initializes a new instance of the <see cref="MartenCommand" /> class.
        /// </summary>
        /// <param name="application">The application.</param>
        public MartenCommand(CommandLineApplication application)
        {
            _application = application;
        }

        /// <summary>
        /// Called when [execute].
        /// </summary>
        /// <returns>System.Int32.</returns>
        public int OnExecute()
        {
            _application.ShowHelp();
            return 0;
        }
    }
}
