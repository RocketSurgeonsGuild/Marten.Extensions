using System;
using Npgsql;
using Npgsql.TypeHandling;
using NodaTime;
using Npgsql.BackendMessages;
using NodaTime.TimeZones;

namespace Rocket.Surgery.Extensions.Marten.NodaTime
{
    class TimestampTzHandler : NpgsqlSimpleTypeHandler<Instant>, INpgsqlSimpleTypeHandler<ZonedDateTime>,
        INpgsqlSimpleTypeHandler<OffsetDateTime>,
        INpgsqlSimpleTypeHandler<DateTimeOffset>
    {
        readonly IDateTimeZoneProvider _dateTimeZoneProvider;

        /// <summary>
        /// A deprecated compile-time option of PostgreSQL switches to a floating-point representation of some date/time
        /// fields. Npgsql (currently) does not support this mode.
        /// </summary>
        readonly bool _integerFormat;

        public TimestampTzHandler(NpgsqlConnection conn)
        {
            _integerFormat = conn.HasIntegerDateTimes;
            _dateTimeZoneProvider = DateTimeZoneProviders.Tzdb;
        }

        #region Read

        public override Instant Read(NpgsqlReadBuffer buf, int len, FieldDescription fieldDescription = null)
        {
            if (_integerFormat) {
                var value = buf.ReadInt64();
                if (value == long.MaxValue || value == long.MinValue)
                    throw new NpgsqlSafeReadException(new NotSupportedException("Infinity values not supported for timestamp with time zone"));
                return TimestampHandler.Decode(value);
            }
            else
            {
                var value = buf.ReadDouble();
                if (double.IsPositiveInfinity(value) || double.IsNegativeInfinity(value))
                    throw new NpgsqlSafeReadException(new NotSupportedException("Infinity values not supported for timestamp with time zone"));
                return TimestampHandler.Decode(value);
            }
        }

        ZonedDateTime INpgsqlSimpleTypeHandler<ZonedDateTime>.Read(NpgsqlReadBuffer buf, int len, FieldDescription fieldDescription)
        {
            try
            {
                if (_integerFormat)
                {
                    var value = buf.ReadInt64();
                    if (value == long.MaxValue || value == long.MinValue)
                        throw new NpgsqlSafeReadException(new NotSupportedException("Infinity values not supported for timestamp with time zone"));
                    return TimestampHandler.Decode(value).InZone(_dateTimeZoneProvider[buf.Connection.Timezone]);
                }
                else
                {
                    var value = buf.ReadDouble();
                    if (double.IsPositiveInfinity(value) || double.IsNegativeInfinity(value))
                        throw new NpgsqlSafeReadException(new NotSupportedException("Infinity values not supported for timestamp with time zone"));
                    return TimestampHandler.Decode(value).InZone(_dateTimeZoneProvider[buf.Connection.Timezone]);
                }
            }
            catch (DateTimeZoneNotFoundException e)
            {
                throw new NpgsqlSafeReadException(e);
            }
        }

        OffsetDateTime INpgsqlSimpleTypeHandler<OffsetDateTime>.Read(NpgsqlReadBuffer buf, int len, FieldDescription fieldDescription)
            => ((INpgsqlSimpleTypeHandler<ZonedDateTime>)this).Read(buf, len, fieldDescription).ToOffsetDateTime();

        #endregion Read

        #region Write

        public override int ValidateAndGetLength(Instant value, NpgsqlParameter parameter)
            => 8;

        int INpgsqlSimpleTypeHandler<ZonedDateTime>.ValidateAndGetLength(ZonedDateTime value, NpgsqlParameter parameter)
            => 8;

        public int ValidateAndGetLength(OffsetDateTime value, NpgsqlParameter parameter)
            => 8;

        public override void Write(Instant value, NpgsqlWriteBuffer buf, NpgsqlParameter parameter)
        {
            if (_integerFormat)
                TimestampHandler.WriteInteger(value, buf);
            else
                TimestampHandler.WriteDouble(value, buf);
        }

        void INpgsqlSimpleTypeHandler<ZonedDateTime>.Write(ZonedDateTime value, NpgsqlWriteBuffer buf, NpgsqlParameter parameter)
            => Write(value.ToInstant(), buf, parameter);

        public void Write(OffsetDateTime value, NpgsqlWriteBuffer buf, NpgsqlParameter parameter)
            => Write(value.ToInstant(), buf, parameter);

        DateTimeOffset INpgsqlSimpleTypeHandler<DateTimeOffset>.Read(NpgsqlReadBuffer buf, int len, FieldDescription fieldDescription)
        {
            return Read(buf, len, fieldDescription).ToDateTimeOffset();
        }

        public new int ValidateAndGetLength(DateTimeOffset value, NpgsqlParameter parameter)
            => 8;

        public void Write(DateTimeOffset value, NpgsqlWriteBuffer buf, NpgsqlParameter parameter)
        {
            Write(Instant.FromDateTimeOffset(value), buf, parameter);
        }

        #endregion Write
    }
}
