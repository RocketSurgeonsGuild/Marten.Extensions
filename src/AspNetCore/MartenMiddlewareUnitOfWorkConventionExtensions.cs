using Rocket.Surgery.Extensions.Marten.AspNetCore;


// ReSharper disable once CheckNamespace
namespace Rocket.Surgery.Conventions
{
    /// <summary>
    ///  MartenMiddlewareUnitOfWorkConventionExtensions.
    /// </summary>
    public static class MartenMiddlewareUnitOfWorkConventionExtensions
    {
        /// <summary>
        /// Adds the marten functions unit of work.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>IConventionHostBuilder.</returns>
        public static IConventionHostBuilder AddMartenFunctionsUnitOfWork(this IConventionHostBuilder builder)
        {
            builder.Scanner.AppendConvention(new MartenMiddlewareUnitOfWorkConvention());
            return builder;
        }
    }
}
