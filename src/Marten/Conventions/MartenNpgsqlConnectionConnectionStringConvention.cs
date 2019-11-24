using System;
using Marten;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Npgsql;
using Rocket.Surgery.Extensions.DependencyInjection;

namespace Rocket.Surgery.Extensions.Marten.Conventions
{
    /// <summary>
    /// MartenNpgsqlConnectionConnectionStringConvention.
    /// Implements the <see cref="IConfigureOptions{TOptions}" />
    /// Implements the <see cref="IServiceConvention" />
    /// </summary>
    /// <seealso cref="IConfigureOptions{StoreOptions}" />
    /// <seealso cref="IServiceConvention" />
    internal class MartenNpgsqlConnectionConnectionStringConvention : IConfigureOptions<StoreOptions>,
                                                                      IServiceConvention
    {
        private readonly Func<NpgsqlConnection> _connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="MartenNpgsqlConnectionConnectionStringConvention" /> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public MartenNpgsqlConnectionConnectionStringConvention(Func<NpgsqlConnection> connectionString)
            => _connectionString = connectionString;

        /// <summary>
        /// Invoked to configure a <cref name="StoreOptions" /> instance.
        /// </summary>
        /// <param name="options">The options instance to configure.</param>
        public void Configure(StoreOptions options) => options.Connection(_connectionString);

        /// <summary>
        /// Registers the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public void Register(IServiceConventionContext context) => context.Services.ConfigureOptions(this);
    }
}