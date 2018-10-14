using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Rocket.Surgery.AspNetCore.Marten;
using Rocket.Surgery.Core.Marten;
using Rocket.Surgery.Core.Marten.Builders;
using Rocket.Surgery.Conventions;
using Rocket.Surgery.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Security.Claims;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class MartenServicesExtensions
    {
        public static MartenServicesBuilder AddStartupFilter(this MartenServicesBuilder builder)
        {
            builder.Parent.Services.TryAddEnumerable(ServiceDescriptor.Transient<IStartupFilter, MartenStartupFilter>());
            builder.Parent.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Parent.Services.TryAddScoped<IMartenUser>(_ =>
            {
                var context = _.GetRequiredService<IHttpContextAccessor>().HttpContext;
                return new MartenUser<string>(() => context.User?.Claims.GetIdFromClaims());
            });
            return builder;
        }
    }
}
