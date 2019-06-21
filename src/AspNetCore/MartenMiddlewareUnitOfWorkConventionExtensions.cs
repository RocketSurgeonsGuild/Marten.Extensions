using Rocket.Surgery.Extensions.Marten.AspNetCore;


// ReSharper disable once CheckNamespace
namespace Rocket.Surgery.Conventions
{
    public static class MartenMiddlewareUnitOfWorkConventionExtensions
    {
        public static IConventionHostBuilder AddMartenFunctionsUnitOfWork(this IConventionHostBuilder builder)
        {
            builder.Scanner.AppendConvention(new MartenMiddlewareUnitOfWorkConvention());
            return builder;
        }
    }
}
