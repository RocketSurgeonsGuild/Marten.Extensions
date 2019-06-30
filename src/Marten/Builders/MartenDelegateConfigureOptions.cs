using System;
using System.Collections.Generic;
using System.Linq;
using Marten;
using Microsoft.Extensions.Options;

namespace Rocket.Surgery.Extensions.Marten.Builders
{
    /// <summary>
    /// Class MartenDelegateConfigureOptions.
    /// Implements the <see cref="Microsoft.Extensions.Options.IConfigureOptions{Marten.StoreOptions}" />
    /// </summary>
    /// <seealso cref="Microsoft.Extensions.Options.IConfigureOptions{Marten.StoreOptions}" />
    class MartenDelegateConfigureOptions : IConfigureOptions<StoreOptions>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IEnumerable<MartenConfigurationDelegateContainer> _delegates;

        /// <summary>
        /// Initializes a new instance of the <see cref="MartenDelegateConfigureOptions"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="delegates">The delegates.</param>
        public MartenDelegateConfigureOptions(
            IServiceProvider serviceProvider,
            IEnumerable<MartenConfigurationDelegateContainer> delegates)
        {
            _serviceProvider = serviceProvider;
            _delegates = delegates;
        }

        /// <summary>
        /// Configures the specified options.
        /// </summary>
        /// <param name="options">The options.</param>
        public void Configure(StoreOptions options)
        {
            foreach (var @delegate in _delegates.Select(x => x.Delegate))
            {
                if (@delegate is MartenConfigurationDelegate configurationDelegate)
                    configurationDelegate(options);
                if (@delegate is MartenComponentConfigurationDelegate componentConfigurationDelegate)
                    componentConfigurationDelegate(_serviceProvider, options);
            }
        }
    }
}
