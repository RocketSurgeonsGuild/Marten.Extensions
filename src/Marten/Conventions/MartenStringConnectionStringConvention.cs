using System;
using Marten;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Rocket.Surgery.Extensions.DependencyInjection;

namespace Rocket.Surgery.Extensions.Marten.Conventions
{
    /// <summary>
    ///  MartenStringConnectionStringConvention.
    /// Implements the <see cref="Microsoft.Extensions.Options.IConfigureOptions{Marten.StoreOptions}" />
    /// Implements the <see cref="Rocket.Surgery.Extensions.DependencyInjection.IServiceConvention" />
    /// </summary>
    /// <seealso cref="Microsoft.Extensions.Options.IConfigureOptions{Marten.StoreOptions}" />
    /// <seealso cref="Rocket.Surgery.Extensions.DependencyInjection.IServiceConvention" />
    class MartenStringConnectionStringConvention : IConfigureOptions<StoreOptions>, IServiceConvention
    {
        private readonly Func<string> _connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="MartenStringConnectionStringConvention"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public MartenStringConnectionStringConvention(Func<string> connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Invoked to configure a <typeparamref name="TOptions" /> instance.
        /// </summary>
        /// <param name="options">The options instance to configure.</param>
        public void Configure(StoreOptions options)
        {
            options.Connection(_connectionString);
        }

        /// <summary>
        /// Registers the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public void Register(IServiceConventionContext context)
        {
            context.Services.ConfigureOptions(this);
        }
    }
}
