using System.Collections.Generic;
using JetBrains.Annotations;
using Marten;
using Microsoft.Extensions.Options;

namespace Rocket.Surgery.Extensions.Marten.Builders
{
    /// <summary>
    /// MartenRegistryConfigureOptions.
    /// Implements the <see cref="IConfigureOptions{TOptions}" />
    /// </summary>
    /// <seealso cref="IConfigureOptions{StoreOptions}" />
    [UsedImplicitly]
    internal class MartenRegistryConfigureOptions : IConfigureOptions<StoreOptions>
    {
        private readonly IEnumerable<MartenRegistry> _martenRegistries;

        /// <summary>
        /// Initializes a new instance of the <see cref="MartenRegistryConfigureOptions" /> class.
        /// </summary>
        /// <param name="martenRegistries">The marten registries.</param>
        public MartenRegistryConfigureOptions(IEnumerable<MartenRegistry> martenRegistries)
            => _martenRegistries = martenRegistries;

        /// <summary>
        /// Invoked to configure a <cref name="StoreOptions" /> instance.
        /// </summary>
        /// <param name="options">The options instance to configure.</param>
        public void Configure(StoreOptions options)
        {
            foreach (var registry in _martenRegistries)
            {
                options.Schema.Include(registry);
            }
        }
    }
}