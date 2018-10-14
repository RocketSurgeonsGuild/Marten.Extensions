using System.Threading.Tasks;
using Npgsql;
using Rocket.Surgery.LocalDevelopment;
using Xunit;
using Xunit.Abstractions;

namespace Rocket.Surgery.Marten.Tests
{
    public class PostgresFixture : IAsyncLifetime
    {
        private readonly PostgresAutomation _postgresAutomation;

        public NpgsqlConnectionStringBuilder ConnectionString => _postgresAutomation.ConnectionString;

        public PostgresFixture()
        {
            _postgresAutomation = PostgresAutomation.ForUnitTesting(typeof(PostgresFixture));
        }

        public void LogPostgres(ITestOutputHelper testOutputHelper)
        {
            foreach (var log in _postgresAutomation.Logs)
                testOutputHelper.WriteLine(log);
        }

        public async Task InitializeAsync()
        {
            await _postgresAutomation.Start();
        }

        public async Task DisposeAsync()
        {
            await Task.Yield();
            await _postgresAutomation.DropDatabase();
        }
    }
}