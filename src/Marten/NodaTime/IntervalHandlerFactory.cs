using Npgsql;
using NodaTime;
using Npgsql.TypeHandling;
using NpgsqlTypes;

namespace Rocket.Surgery.Extensions.Marten.NodaTime
{
    public class IntervalHandlerFactory : NpgsqlTypeHandlerFactory<Period>
    {
        // Check for the legacy floating point timestamps feature
        protected override NpgsqlTypeHandler<Period> Create(NpgsqlConnection conn)
            => new IntervalHandler(conn.HasIntegerDateTimes);
    }
}
