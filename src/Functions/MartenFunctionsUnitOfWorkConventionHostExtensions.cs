using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Rocket.Surgery.Extensions.Marten;
using Rocket.Surgery.Extensions.Marten.Builders;
using Rocket.Surgery.Conventions;
using Rocket.Surgery.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Security.Claims;
using Rocket.Surgery.Extensions.Marten.Functions;

// ReSharper disable once CheckNamespace
namespace Rocket.Surgery.Conventions
{
    /// <summary>
    /// MartenFunctionsUnitOfWorkConventionHostExtensions.
    /// </summary>
    public static class MartenFunctionsUnitOfWorkConventionHostExtensions
    {
        /// <summary>
        /// Adds the marten functions unit of work.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>IConventionHostBuilder.</returns>
        public static IConventionHostBuilder AddMartenUnitOfWorkFunctionFilter(this IConventionHostBuilder builder)
        {
            var options = builder.Get<MartenOptions>() ?? new MartenOptions();
            options.AutomaticUnitOfWork = true;
            builder.Set(options);
            builder.Scanner.AppendConvention<MartenFunctionsUnitOfWorkConvention>();
            return builder;
        }
    }
}
