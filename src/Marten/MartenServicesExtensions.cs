using Marten;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Rocket.Surgery.Extensions.DependencyInjection;
using Rocket.Surgery.Extensions.Marten.Builders;
using Rocket.Surgery.Extensions.Marten.Listeners;
using Rocket.Surgery.Extensions.Marten.Projections;
using Rocket.Surgery.Extensions.Marten.Security;

namespace Rocket.Surgery.Extensions.Marten
{
    /// <summary>
    /// MartenServicesExtensions.
    /// </summary>
    public static class MartenServicesExtensions
    {
        /// <summary>
        /// Withes the marten.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>MartenServicesBuilder.</returns>
        public static IServiceConventionContext WithMarten(this IServiceConventionContext context)
        {
            DefaultServices(context.Services);

            context.Services.AddOptions();
            context.Services.AddMemoryCache();

            context.Services.TryAddSingleton(new ProjectionDescriptorCollection(context.AssemblyCandidateFinder));

            return context;
        }

        private static void DefaultServices(IServiceCollection services)
        {
            services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<StoreOptions>, MartenConfigureOptions>());
            services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<StoreOptions>, MartenRegistryConfigureOptions>());
            services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<StoreOptions>, MartenProjectionsConfigureOptions>());

            services.TryAddScoped(c => c.GetRequiredService<IDocumentStore>().QuerySession());
            services.AddTransient<IDocumentSessionListener>(_ => ActivatorUtilities.CreateInstance<MartenDocumentSessionListener>(_));

            services.TryAddSingleton(_ => new DocumentStore(_.GetRequiredService<IOptions<StoreOptions>>().Value));
            services.TryAddTransient<IDocumentStore, TransientDocumentStore>();
            services.TryAddTransient(_ => _.GetRequiredService<IDocumentStore>().SecureQuerySession(_.GetRequiredService<ISecurityQueryProvider>(), _.GetRequiredService<IMartenContext>()));
            services.TryAddSingleton<IDaemonFactory, DaemonFactory>();
            services.TryAddTransient(typeof(DaemonLogger<>));
            services.TryAddTransient<ISecurityQueryProvider, SecurityQueryProvider>();
            services.TryAddEnumerable(ServiceDescriptor.Transient<ISecurityQueryPart, HaveOwnerSecurityQueryPart>());
            services.TryAddEnumerable(ServiceDescriptor.Transient<ISecurityQueryPart, CanBeAssignedSecurityQueryPart>());
            services.TryAddScoped<IMartenContext, MartenContext>();
        }
    }
}
