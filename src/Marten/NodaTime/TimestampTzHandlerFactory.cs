using Npgsql;
using Npgsql.TypeHandling;
using NodaTime;

namespace Rocket.Surgery.Extensions.Marten.NodaTime
{
    public class TimestampTzHandlerFactory : NpgsqlTypeHandlerFactory<Instant>
    {
        // Check for the legacy floating point timestamps feature
        protected override NpgsqlTypeHandler<Instant> Create(NpgsqlConnection conn)
            => new TimestampTzHandler(conn);
    }
}
