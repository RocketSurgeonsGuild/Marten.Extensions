using JetBrains.Annotations;
using McMaster.Extensions.CommandLineUtils;

namespace Rocket.Surgery.Extensions.Marten.Commands
{
    /// <summary>
    /// MartenCommand.
    /// </summary>
    [UsedImplicitly]
    [Command("marten")]
    [Subcommand(typeof(PatchCommand))]
    [Subcommand(typeof(AssertCommand))]
    [Subcommand(typeof(ApplyCommand))]
    [Subcommand(typeof(DumpCommand))]
    internal class MartenCommand
    {
        private readonly CommandLineApplication _application;

        /// <summary>
        /// Initializes a new instance of the <see cref="MartenCommand" /> class.
        /// </summary>
        /// <param name="application">The application.</param>
        public MartenCommand(CommandLineApplication application) => _application = application;

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