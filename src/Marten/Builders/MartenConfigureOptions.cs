using Marten;
using Marten.Events;
using Marten.NodaTime;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Rocket.Surgery.Extensions.Marten.Builders
{
    /// <summary>
    /// MartenConfigureOptions.
    /// Implements the <see cref="IConfigureOptions{StoreOptions}" />
    /// Implements the <see cref="IConfigureOptions{StoreOptions}" />
    /// </summary>
    /// <seealso cref="IConfigureOptions{StoreOptions}" />
    /// <seealso cref="IConfigureOptions{StoreOptions}" />
    class MartenConfigureOptions : IConfigureOptions<StoreOptions>
    {
        private readonly ILoggerFactory _loggerFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="MartenConfigureOptions" /> class.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        public MartenConfigureOptions(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }

        /// <summary>
        /// Configures the specified options.
        /// </summary>
        /// <param name="options">The options.</param>
        public void Configure(StoreOptions options)
        {
            options.UseDefaultSerialization(EnumStorage.AsString, Casing.CamelCase, CollectionStorage.Default);
            options.UseNodaTime(true);
            options.AutoCreateSchemaObjects = AutoCreate.CreateOrUpdate;
            options.Events.StreamIdentity = StreamIdentity.AsString;

            options.Logger(new MartenLogger(_loggerFactory.CreateLogger<MartenLogger>()));
        }
    }
}
