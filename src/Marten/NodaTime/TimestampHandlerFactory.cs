using Npgsql;
using Npgsql.TypeHandling;
using NodaTime;

namespace Rocket.Surgery.Extensions.Marten.NodaTime
{
    public class TimestampHandlerFactory : NpgsqlTypeHandlerFactory<Instant>
    {
        // Check for the legacy floating point timestamps feature
        protected override NpgsqlTypeHandler<Instant> Create(NpgsqlConnection conn)
        {
            var csb = new NpgsqlConnectionStringBuilder(conn.ConnectionString);
            return new TimestampHandler(conn.HasIntegerDateTimes, csb.ConvertInfinityDateTime);
        }
    }
}
