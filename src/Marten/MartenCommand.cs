using JetBrains.Annotations;
using Marten;
using Marten.CommandLine;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Options;

namespace Rocket.Surgery.Extensions.Marten
{
    /// <summary>
    /// MartenCommand.
    /// </summary>
    [Command("marten", ThrowOnUnexpectedArgument = false)]
    [UsedImplicitly]
    public class MartenCommand
    {
        private readonly IOptions<StoreOptions> _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="MartenCommand" /> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public MartenCommand(IOptions<StoreOptions> options)
        {
            _options = options;
        }

        /// <summary>
        /// Gets the remaining arguments.
        /// </summary>
        /// <value>The remaining arguments.</value>
        [UsedImplicitly] public string[] RemainingArguments { get; }

        /// <summary>
        /// Called when [execute].
        /// </summary>
        /// <returns>System.Int32.</returns>
        public int OnExecute()
        {
            return MartenCommands.Execute(_options.Value, RemainingArguments);
        }
    }
}
