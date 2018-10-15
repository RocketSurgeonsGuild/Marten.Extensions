using Microsoft.Extensions.DependencyInjection;
using Rocket.Surgery.Builders;
using Rocket.Surgery.Extensions.DependencyInjection;

namespace Rocket.Surgery.Extensions.Marten.Builders
{
    public class MartenServicesBuilder : Builder<IServiceConventionContext>
    {
        private readonly IServiceCollection _services;

        internal MartenServicesBuilder(IServiceConventionContext servicesBuilder) : base(servicesBuilder, servicesBuilder.Properties)
        {
            _services = servicesBuilder.Services;
        }

        internal void AddDelegate(MartenComponentConfigurationDelegate @delegate)
        {
            _services.AddSingleton(new MartenConfigurationDelegateContainer(@delegate));
        }

        internal void AddDelegate(MartenConfigurationDelegate @delegate)
        {
            _services.AddSingleton(new MartenConfigurationDelegateContainer(@delegate));
        }
    }
}

