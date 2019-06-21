using Marten;
using Microsoft.Extensions.Logging;
using Rocket.Surgery.Extensions.Marten;

namespace Rocket.Surgery.Marten.Tests
{
    class MyMartenRegistry : MartenRegistry
    {
        public MyMartenRegistry(ILoggerFactory factory)
        {
            var logger = factory.CreateLogger("abcd");
            logger.LogInformation($"Hello from {nameof(MyMartenRegistry)}");
        }
    }
    class MartenContext : IMartenContext
    {
        public IMartenUser User { get; set; }
    }
}
