using Marten;
using Marten.CommandLine;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Options;

namespace Rocket.Surgery.Extensions.Marten.AspNetCore
{
    [Command(ThrowOnUnexpectedArgument = false)]
    public class MartenCommand
    {
        private readonly IOptions<StoreOptions> _options;

        public MartenCommand(IOptions<StoreOptions> options)
        {
            _options = options;
        }

        public string[] RemainingArguments { get; }

        public int OnExecute()
        {
            return MartenCommands.Execute(_options.Value, RemainingArguments);
        }
    }
}