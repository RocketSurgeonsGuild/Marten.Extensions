using Marten;
using Marten.Events;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Rocket.Surgery.Extensions.Marten;
using Rocket.Surgery.Extensions.Marten.Builders;
using Rocket.Surgery.Extensions.Marten.Projections;
using Rocket.Surgery.Conventions.Reflection;
using Rocket.Surgery.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using Scrutor;
using Marten.Events.Projections;
using Marten.Events.Projections.Async;
using Marten.Schema;
using Marten.Services;
using Marten.Storage;
using Marten.Transforms;
using System.Collections.Generic;
using System.Data;
using Marten.Linq;
using Marten.Patching;
using Marten.Services.BatchQuerying;
using Npgsql;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Reflection;
using Rocket.Surgery.Extensions.Marten.Listeners;
using Rocket.Surgery.Extensions.Marten.Security;
using NodaTime;
using NpgsqlTypes;
using Npgsql.TypeMapping;
using Npgsql.TypeHandling;
using Npgsql.BackendMessages;
using Marten.Util;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class MartenServicesExtensions
    {
        public static MartenServicesBuilder WithMarten(this IServiceConventionContext context)
        {
            DefaultServices(context.Services);

            context.Services.AddOptions();
            context.Services.AddMemoryCache();

            context.Services.TryAddSingleton(new ProjectionDescriptorCollection(context.AssemblyCandidateFinder));

            return new MartenServicesBuilder(context)
                .UseLightweightSession();
        }

        private static void DefaultServices(IServiceCollection services)
        {
            services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<StoreOptions>, MartenConfigureOptions>());
            services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<StoreOptions>, MartenDelegateConfigureOptions>());
            services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<StoreOptions>, MartenRegistryConfigureOptions>());
            services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<StoreOptions>, MartenProjectionsConfigureOptions>());

            services.TryAddScoped(c => c.GetRequiredService<IDocumentStore>().QuerySession());
            services.AddTransient<IDocumentSessionListener>(_ => ActivatorUtilities.CreateInstance<MartenDocumentSessionListener>(_));

            services.TryAddSingleton(_ => new DocumentStore(_.GetRequiredService<IOptions<StoreOptions>>().Value));
            services.TryAddTransient<IDocumentStore, TransientDocumentStore>();
            services.TryAddTransient<ISecureQuerySession>(_ =>
            {
                return _.GetRequiredService<IDocumentStore>().SecureQuerySession(_.GetRequiredService<ISecurityQueryProvider>(), _.GetRequiredService<IMartenContext>());
            });
            services.TryAddSingleton<IDaemonFactory, DaemonFactory>();
            services.TryAddTransient(typeof(DaemonLogger<>));
            services.TryAddTransient<ISecurityQueryProvider, SecurityQueryProvider>();
            services.TryAddEnumerable(ServiceDescriptor.Transient<ISecurityQueryPart, HaveOwnerSecurityQueryPart>());
            services.TryAddEnumerable(ServiceDescriptor.Transient<ISecurityQueryPart, CanBeAssignedSecurityQueryPart>());
            services.TryAddScoped<IMartenContext, MartenContext>();
        }
    }
}
