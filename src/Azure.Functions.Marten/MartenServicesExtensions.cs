using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Rocket.Surgery.Core.Marten.Builders;

// ReSharper disable once CheckNamespace
namespace Rocket.Surgery.Azure.Functions.Marten
{
    public static class MartenServicesExtensions
    {
        public static MartenServicesBuilder AddSaveChangesFilter(this MartenServicesBuilder builder)
        {
            builder.Parent.Services.TryAddEnumerable(ServiceDescriptor.Transient<IFunctionInvocationFilter, MartenFunctionInvocationFilter>());
            return builder;
        }
    }
}
