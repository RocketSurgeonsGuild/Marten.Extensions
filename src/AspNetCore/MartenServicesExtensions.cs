using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Rocket.Surgery.Extensions.Marten.Builders;

namespace Rocket.Surgery.Extensions.Marten.AspNetCore
{
    public static class MartenServicesExtensions
    {
        public static MartenServicesBuilder AddStartupFilter(this MartenServicesBuilder builder)
        {
            builder.Parent.Services.TryAddEnumerable(ServiceDescriptor.Transient<IStartupFilter, MartenStartupFilter>());
            builder.Parent.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            return builder;
        }
    }
}
