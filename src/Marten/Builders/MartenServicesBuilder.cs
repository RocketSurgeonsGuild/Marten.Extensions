using Microsoft.Extensions.DependencyInjection;
using Rocket.Surgery.Builders;
using Rocket.Surgery.Extensions.DependencyInjection;

namespace Rocket.Surgery.Extensions.Marten.Builders
{
    /// <summary>
    /// Class MartenServicesBuilder.
    /// Implements the <see cref="Rocket.Surgery.Builders.Builder{Rocket.Surgery.Extensions.DependencyInjection.IServiceConventionContext}" />
    /// </summary>
    /// <seealso cref="Rocket.Surgery.Builders.Builder{Rocket.Surgery.Extensions.DependencyInjection.IServiceConventionContext}" />
    public class MartenServicesBuilder : Builder<IServiceConventionContext>
    {
        private readonly IServiceCollection _services;

        /// <summary>
        /// Initializes a new instance of the <see cref="MartenServicesBuilder"/> class.
        /// </summary>
        /// <param name="servicesBuilder">The services builder.</param>
        internal MartenServicesBuilder(IServiceConventionContext servicesBuilder) : base(servicesBuilder, servicesBuilder.Properties)
        {
            _services = servicesBuilder.Services;
        }

        /// <summary>
        /// Adds the delegate.
        /// </summary>
        /// <param name="delegate">The delegate.</param>
        internal void AddDelegate(MartenComponentConfigurationDelegate @delegate)
        {
            _services.AddSingleton(new MartenConfigurationDelegateContainer(@delegate));
        }

        /// <summary>
        /// Adds the delegate.
        /// </summary>
        /// <param name="delegate">The delegate.</param>
        internal void AddDelegate(MartenConfigurationDelegate @delegate)
        {
            _services.AddSingleton(new MartenConfigurationDelegateContainer(@delegate));
        }
    }
}

