using Marten;
using Microsoft.Extensions.Logging;

namespace Rocket.Surgery.Extensions.Marten.Tests
{
    internal class MyMartenRegistry : MartenRegistry
    {
        public MyMartenRegistry(ILoggerFactory factory)
        {
            var logger = factory.CreateLogger("abcd");
            logger.LogInformation($"Hello from {nameof(MyMartenRegistry)}");
        }
    }

#nullable disable
    internal class MartenContext : IMartenContext
    {
        public IMartenUser User { get; set; }
    }
#nullable restore
}