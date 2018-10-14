using Marten;

namespace Rocket.Surgery.Core.Marten.Builders
{
    /// <summary>
    /// Delegate MartenConfigurationDelegate
    /// </summary>
    /// <param name="options">The options.</param>
    public delegate void MartenConfigurationDelegate(StoreOptions options);
}
