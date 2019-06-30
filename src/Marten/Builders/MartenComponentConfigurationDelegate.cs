using System;
using Marten;

namespace Rocket.Surgery.Extensions.Marten.Builders
{
    /// <summary>
    /// Delegate MartenComponentConfigurationDelegate
    /// </summary>
    /// <param name="serviceProvider">The serviceProvider.</param>
    /// <param name="options">The options.</param>
    public delegate void MartenComponentConfigurationDelegate(IServiceProvider serviceProvider, StoreOptions options);
}
