using Rocket.Surgery.Extensions.Marten;
using Rocket.Surgery.Extensions.Marten.AspNetCore;


// ReSharper disable once CheckNamespace
namespace Rocket.Surgery.Conventions
{
    /// <summary>
    /// MartenAspNetCoreConventionHostExtensions.
    /// </summary>
    public static class MartenAspNetCoreConventionHostExtensions
    {
        /// <summary>
        /// Adds the marten functions unit of work.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>IConventionHostBuilder.</returns>
        public static IConventionHostBuilder AddMartenUnitOfWorkMiddleware(this IConventionHostBuilder builder)
        {
            var options = builder.Get<MartenOptions>() ?? new MartenOptions();
            options.AutomaticUnitOfWork = true;
            builder.Set(options);
            builder.Scanner.PrependConvention<MartenMiddlewareUnitOfWorkConvention>();
            return builder;
        }
    }
}
