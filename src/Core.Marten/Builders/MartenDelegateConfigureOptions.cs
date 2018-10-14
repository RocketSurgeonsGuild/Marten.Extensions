using System;
using System.Collections.Generic;
using System.Linq;
using Marten;
using Microsoft.Extensions.Options;

namespace Rocket.Surgery.Core.Marten.Builders
{
    class MartenDelegateConfigureOptions : IConfigureOptions<StoreOptions>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IEnumerable<MartenConfigurationDelegateContainer> _delegates;

        public MartenDelegateConfigureOptions(
            IServiceProvider serviceProvider,
            IEnumerable<MartenConfigurationDelegateContainer> delegates)
        {
            _serviceProvider = serviceProvider;
            _delegates = delegates;
        }

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