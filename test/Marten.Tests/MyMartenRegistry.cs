using Marten;
using Microsoft.Extensions.Logging;

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
}
