using Marten;
using Marten.Events;
using Marten.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Rocket.Surgery.Extensions.Marten.Builders
{
    class MartenConfigureOptions : IConfigureOptions<StoreOptions>
    {
        private readonly ILoggerFactory _loggerFactory;

        public MartenConfigureOptions(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }

        public void Configure(StoreOptions options)
        {
            options.Serializer(new CustomJsonNetSerializer()
            {
                Casing = Casing.CamelCase,
                EnumStorage = EnumStorage.AsString
            });
            options.AutoCreateSchemaObjects = AutoCreate.CreateOrUpdate;
            options.Events.StreamIdentity = StreamIdentity.AsString;

            options.Logger(new MartenLogger(_loggerFactory.CreateLogger<MartenLogger>()));
        }
    }
}
