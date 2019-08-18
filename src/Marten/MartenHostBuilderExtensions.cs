using System;
using Marten;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Npgsql;
using Rocket.Surgery.Conventions;
using Rocket.Surgery.Extensions.DependencyInjection;
using Rocket.Surgery.Extensions.Marten;
using Rocket.Surgery.Extensions.Marten.Conventions;

// ReSharper disable once CheckNamespace
namespace Rocket.Surgery.Conventions
{
    /// <summary>
    /// LoggingExtensions.
    /// </summary>
    public static class MartenHostBuilderExtensions
    {
        /// <summary>
        /// Uses marten.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="options">The options.</param>
        /// <returns>IConventionHostBuilder.</returns>
        public static IConventionHostBuilder UseMarten(this IConventionHostBuilder container, MartenOptions? options = null)
        {
            container.Set(options  ?? new MartenOptions());
            container.Scanner.PrependConvention<MartenCommandConvention>();
            container.Scanner.PrependConvention<MartenConvention>();
            return container;
        }

        /// <summary>
        /// Uses marten.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <returns>IConventionHostBuilder.</returns>
        public static IConventionHostBuilder UseMartenLightweightTracking(this IConventionHostBuilder container)
        {
            var options = container.Get<MartenOptions>() ?? new MartenOptions();
            options.UseSession = true;
            options.SessionTracking = DocumentTracking.None;
            container.Set(options);
            return container;
        }

        /// <summary>
        /// Uses marten.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <returns>IServiceConventionContext.</returns>
        public static IServiceConventionContext UseMartenLightweightTracking(this IServiceConventionContext container)
        {
            var options = container.Get<MartenOptions>() ?? new MartenOptions();
            options.UseSession = true;
            options.SessionTracking = DocumentTracking.None;
            container.Set(options);
            return container;
        }

        /// <summary>
        /// Uses marten.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <returns>IConventionHostBuilder.</returns>
        public static IConventionHostBuilder UseMartenWithDirtyTracking(this IConventionHostBuilder container)
        {
            var options = container.Get<MartenOptions>() ?? new MartenOptions();
            options.UseSession = true;
            options.SessionTracking = DocumentTracking.DirtyTracking;
            container.Set(options);
            return container;
        }

        /// <summary>
        /// Uses marten.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <returns>IServiceConventionContext.</returns>
        public static IServiceConventionContext UseMartenWithDirtyTracking(this IServiceConventionContext container)
        {
            var options = container.Get<MartenOptions>() ?? new MartenOptions();
            options.UseSession = true;
            options.SessionTracking = DocumentTracking.DirtyTracking;
            container.Set(options);
            return container;
        }

        /// <summary>
        /// Uses marten.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <returns>IConventionHostBuilder.</returns>
        public static IConventionHostBuilder UseMartenUnitOfWork(this IConventionHostBuilder container)
        {
            var options = container.Get<MartenOptions>() ?? new MartenOptions();
            options.AutomaticUnitOfWork= true;
            container.Set(options);
            return container;
        }

        /// <summary>
        /// Uses marten.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <returns>IServiceConventionContext.</returns>
        public static IServiceConventionContext UseMartenUnitOfWork(this IServiceConventionContext container)
        {
            var options = container.Get<MartenOptions>() ?? new MartenOptions();
            options.AutomaticUnitOfWork= true;
            container.Set(options);
            return container;
        }

        /// <summary>
        /// Uses marten.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="connectionString">The connection string</param>
        /// <returns>IConventionHostBuilder.</returns>
        public static IConventionHostBuilder UseMartenConnectionString(this IConventionHostBuilder container, string connectionString)
        {
            container.Scanner.PrependConvention(new MartenStringConnectionStringConvention(() => connectionString));
            return container;
        }

        /// <summary>
        /// Uses marten.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="connectionString">The connection string</param>
        /// <returns>IServiceConventionContext.</returns>
        public static IServiceConventionContext UseMartenConnectionString(this IServiceConventionContext container, string connectionString)
        {
            container.Services.ConfigureOptions(new MartenStringConnectionStringConvention(() => connectionString));
            return container;
        }

        /// <summary>
        /// Uses marten.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="connectionString">The factory for the connection string</param>
        /// <returns>IConventionHostBuilder.</returns>
        public static IConventionHostBuilder UseMartenConnectionString(this IConventionHostBuilder container, Func<string> connectionString)
        {
            container.Scanner.PrependConvention(new MartenStringConnectionStringConvention(connectionString));
            return container;
        }

        /// <summary>
        /// Uses marten.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="connectionString">The factory for the connection string</param>
        /// <returns>IServiceConventionContext.</returns>
        public static IServiceConventionContext UseMartenConnectionString(this IServiceConventionContext container, Func<string> connectionString)
        {
            container.Services.ConfigureOptions(new MartenStringConnectionStringConvention(connectionString));
            return container;
        }

        /// <summary>
        /// Uses marten.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="connection">The factory for the connection</param>
        /// <returns>IConventionHostBuilder.</returns>
        public static IConventionHostBuilder UseMartenConnectionString(this IConventionHostBuilder container, Func<NpgsqlConnection> connection)
        {
            container.Scanner.PrependConvention(new MartenNpgsqlConnectionConnectionStringConvention(connection));
            return container;
        }

        /// <summary>
        /// Uses marten.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="connection">The factory for the connection</param>
        /// <returns>IServiceConventionContext.</returns>
        public static IServiceConventionContext UseMartenConnectionString(this IServiceConventionContext container, Func<NpgsqlConnection> connection)
        {
            container.Services.ConfigureOptions(new MartenNpgsqlConnectionConnectionStringConvention(connection));
            return container;
        }
    }
}
