using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Marten;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NodaTime;
using NodaTime.Testing;
using Npgsql;
using Rocket.Surgery.Conventions;
using Rocket.Surgery.Domain;
using Rocket.Surgery.Extensions.DependencyInjection;
using Rocket.Surgery.Extensions.Testing;
using Xunit;
using Xunit.Abstractions;

namespace Rocket.Surgery.Extensions.Marten.Tests
{
    public class SecurityQueryProviderTests : AutoTestBase, IAsyncLifetime
    {
        private readonly PostgresAutomation _fixture;

        public SecurityQueryProviderTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            _fixture = PostgresAutomation.ForUnitTesting(typeof(SecurityQueryProviderTests));
        }

        public async Task InitializeAsync()
        {
            await _fixture.Start();
        }

        public async Task DisposeAsync()
        {
            await Task.Yield();
            await _fixture.DropDatabase();
        }

        class HaveOwner : IHaveOwner<string>
        {
            public string Id { get; set; }
            public OwnerData<string> Owner { get; set; }
        }

        class CanBeAssigned : ICanBeAssigned<long>
        {
            public string Id { get; set; }
            public AssignedUsersData<long> AssignedUsers { get; set; }
        }

        class UserLike
        {
            public string Id { get; set; }
            public HashSet<string> Identities { get; } = new HashSet<string>();
        }

        class OwnerAndCanBeAssigned : IHaveOwner<Guid>, ICanBeAssigned<Guid>
        {
            public string Id { get; set; }
            public OwnerData<Guid> Owner { get; set; }

            public AssignedUsersData<Guid> AssignedUsers { get; set; }
        }

        [Fact]
        public void Should_Work_With_Owner_Document()
        {
            AutoFake.Provide<IServiceCollection>(new ServiceCollection());
            var serviceProviderDictionary = new ServiceProviderDictionary();
            AutoFake.Provide<IServiceProviderDictionary>(serviceProviderDictionary);
            AutoFake.Provide<IServiceProvider>(serviceProviderDictionary);
            AutoFake.Provide<IDictionary<object, object>>(serviceProviderDictionary);
            serviceProviderDictionary.Set(new MartenOptions()
            {
                SessionTracking = DocumentTracking.DirtyTracking,
                UseSession = true
            });
            var servicesBuilder = AutoFake.Resolve<ServicesBuilder>();
            servicesBuilder.Services.AddTransient<MartenRegistry, MyMartenRegistry>();
            servicesBuilder.Services.AddSingleton(LoggerFactory);
            servicesBuilder.Services.AddSingleton<IClock>(
                new FakeClock(Instant.FromDateTimeOffset(DateTimeOffset.Now),
                    Duration.FromSeconds(1))
            );
            var martenBuilder = servicesBuilder.WithMarten();
            servicesBuilder.Services.AddScoped<IMartenContext>(_ => new MartenContext() { User = new MartenUser<string>(() => Guid.NewGuid().ToString()) });

            var serviceProvider = servicesBuilder.Build();
            var options = serviceProvider.GetRequiredService<IOptions<StoreOptions>>().Value;
            options.Connection(() => new NpgsqlConnection(_fixture.ConnectionString.ToString()));

            using (var scope = serviceProvider.CreateScope())
            {
                using (var session = scope.ServiceProvider.GetService<ISecureQuerySession>())
                {
                    var c = session.Query<HaveOwner>()
                        .ToCommand();
                    c.CommandText.Should().Be("select d.data, d.id, d.mt_version from public.mt_doc_securityqueryprovidertests_haveowner as d where d.data -> 'owner' ->> 'id' LIKE :arg0");
                }
            }
        }

        [Fact]
        public void Should_Work_With_OwnerAndCanBeAssigned_Document()
        {
            AutoFake.Provide<IServiceCollection>(new ServiceCollection());
            var serviceProviderDictionary = new ServiceProviderDictionary();
            AutoFake.Provide<IServiceProviderDictionary>(serviceProviderDictionary);
            AutoFake.Provide<IServiceProvider>(serviceProviderDictionary);
            AutoFake.Provide<IDictionary<object, object>>(serviceProviderDictionary);
            serviceProviderDictionary.Set(new MartenOptions()
            {
                SessionTracking = DocumentTracking.DirtyTracking,
                UseSession = true
            });
            var servicesBuilder = AutoFake.Resolve<ServicesBuilder>();
            servicesBuilder.Services.AddTransient<MartenRegistry, MyMartenRegistry>();
            servicesBuilder.Services.AddSingleton(LoggerFactory);
            servicesBuilder.Services.AddSingleton<IClock>(
                new FakeClock(Instant.FromDateTimeOffset(DateTimeOffset.Now),
                    Duration.FromSeconds(1))
            );
            var martenBuilder = servicesBuilder.WithMarten();
            servicesBuilder.Services.AddScoped<IMartenContext>(_ => new MartenContext() { User = new MartenUser<Guid>(() => Guid.NewGuid()) });

            var serviceProvider = servicesBuilder.Build();
            var options = serviceProvider.GetRequiredService<IOptions<StoreOptions>>().Value;
            options.Connection(() => new NpgsqlConnection(_fixture.ConnectionString.ToString()));

            using (var scope = serviceProvider.CreateScope())
            {
                using (var session = scope.ServiceProvider.GetService<ISecureQuerySession>())
                {
                    var c = session.Query<OwnerAndCanBeAssigned>()
                        .ToCommand();

                    c.CommandText.Should().Be("select d.data, d.id, d.mt_version from public.mt_doc_securityqueryprovidertests_ownerandcanbeassigned as d where (CAST(d.data -> 'owner' ->> 'id' as uuid) = :arg0 or d.data @> :arg1)");
                }
            }
        }

        [Fact]
        public void Should_Work_With_CanBeAssigned_Document()
        {
            AutoFake.Provide<IServiceCollection>(new ServiceCollection());
            var serviceProviderDictionary = new ServiceProviderDictionary();
            AutoFake.Provide<IServiceProviderDictionary>(serviceProviderDictionary);
            AutoFake.Provide<IServiceProvider>(serviceProviderDictionary);
            AutoFake.Provide<IDictionary<object, object>>(serviceProviderDictionary);
            serviceProviderDictionary.Set(new MartenOptions()
            {
                SessionTracking = DocumentTracking.DirtyTracking,
                UseSession = true
            });
            var servicesBuilder = AutoFake.Resolve<ServicesBuilder>();
            servicesBuilder.Services.AddTransient<MartenRegistry, MyMartenRegistry>();
            servicesBuilder.Services.AddSingleton(LoggerFactory);
            servicesBuilder.Services.AddSingleton<IClock>(
                new FakeClock(Instant.FromDateTimeOffset(DateTimeOffset.Now),
                    Duration.FromSeconds(1))
            );
            var martenBuilder = servicesBuilder.WithMarten();
            servicesBuilder.Services.AddScoped<IMartenContext>(_ => new MartenContext() { User = new MartenUser<long>(() => 123456) });

            var serviceProvider = servicesBuilder.Build();
            var options = serviceProvider.GetRequiredService<IOptions<StoreOptions>>().Value;
            options.Connection(() => new NpgsqlConnection(_fixture.ConnectionString.ToString()));

            using (var scope = serviceProvider.CreateScope())
            {
                using (var session = scope.ServiceProvider.GetService<ISecureQuerySession>())
                {
                    var c = session.Query<CanBeAssigned>()
                        .ToCommand();

                    c.CommandText.Should().Be("select d.data, d.id, d.mt_version from public.mt_doc_securityqueryprovidertests_canbeassigned as d where d.data @> :arg0");
                }
            }
        }

        [Fact(Skip = "disable for now")]
        public void Should_Work_With_FirstOrDefaultAsync()
        {
            AutoFake.Provide<IServiceCollection>(new ServiceCollection());
            var serviceProviderDictionary = new ServiceProviderDictionary();
            AutoFake.Provide<IServiceProviderDictionary>(serviceProviderDictionary);
            AutoFake.Provide<IServiceProvider>(serviceProviderDictionary);
            AutoFake.Provide<IDictionary<object, object>>(serviceProviderDictionary);
            serviceProviderDictionary.Set(new MartenOptions()
            {
                SessionTracking = DocumentTracking.DirtyTracking,
                UseSession = true
            });
            var servicesBuilder = AutoFake.Resolve<ServicesBuilder>();
            servicesBuilder.Services.AddTransient<MartenRegistry, MyMartenRegistry>();
            servicesBuilder.Services.AddSingleton(LoggerFactory);
            servicesBuilder.Services.AddSingleton<IClock>(
                new FakeClock(Instant.FromDateTimeOffset(DateTimeOffset.Now),
                    Duration.FromSeconds(1))
            );
            var martenBuilder = servicesBuilder.WithMarten();
            servicesBuilder.Services.AddScoped<IMartenContext>(_ => new MartenContext() { User = new MartenUser<long>(() => 123456) });

            var serviceProvider = servicesBuilder.Build();
            var options = serviceProvider.GetRequiredService<IOptions<StoreOptions>>().Value;
            options.Connection(() => new NpgsqlConnection(_fixture.ConnectionString.ToString()));

            using (var scope = serviceProvider.CreateScope())
            {
                var store = scope.ServiceProvider.GetService<IDocumentStore>();
                store.BulkInsert(new[] { new UserLike()
                {
                    Id = 123456.ToString(),
                    Identities = { "123456", "456789" }
                } });

                using (var session = scope.ServiceProvider.GetService<ISecureQuerySession>())
                {
                    var c = session.Query<UserLike>()
                        .Where(x => x.Identities.Contains("123456"))
                        .FirstOrDefaultAsync();

                    c.Should().NotBeNull();
                }

                using (var session = scope.ServiceProvider.GetService<ISecureQuerySession>())
                {
                    var c = session.Query<UserLike>()
                        .Where(x => x.Identities.Contains("456789"))
                        .FirstOrDefaultAsync();

                    c.Should().NotBeNull();
                }
            }
        }
    }
}
