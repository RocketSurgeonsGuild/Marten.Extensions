using System;
using Autofac.Extras.FakeItEasy;
using FakeItEasy;
using FluentAssertions;
using Marten;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NodaTime;
using NodaTime.Testing;
using Npgsql;
using Rocket.Surgery.Core.Marten;
using Rocket.Surgery.Core.Marten.Builders;
using Rocket.Surgery.Conventions.Reflection;
using Rocket.Surgery.Conventions.Scanners;
using Rocket.Surgery.Extensions.DependencyInjection;
using Rocket.Surgery.Extensions.Testing;
using Xunit;
using Xunit.Abstractions;

namespace Rocket.Surgery.Marten.Tests
{
    public class MartenServicesBuilderTests : AutoTestBase
    {
        public MartenServicesBuilderTests(ITestOutputHelper outputHelper) : base(outputHelper) { }

        [Fact]
        public void MustCallDelegatesOnBuild()
        {
            var configurationDelegate = A.Fake<MartenConfigurationDelegate>();
            var componentConfigurationDelegate = A.Fake<MartenComponentConfigurationDelegate>();

            AutoFake.Provide<IServiceCollection>(new ServiceCollection());
            var servicesBuilder = AutoFake.Resolve<ServicesBuilder>();
            servicesBuilder.Services.AddTransient<MartenRegistry, MyMartenRegistry>();
            servicesBuilder.Services.AddSingleton<ILoggerFactory>(LoggerFactory);
            servicesBuilder.Services.AddSingleton<IClock>(
                new FakeClock(Instant.FromDateTimeOffset(DateTimeOffset.Now),
                    Duration.FromSeconds(1))
            );
            var martenBuilder = servicesBuilder
                .WithMarten()
                .AddStartupFilter()
                .Configure(configurationDelegate)
                .Configure(componentConfigurationDelegate);

            martenBuilder.UseDirtyTrackedSession();

            var serviceProvider = servicesBuilder.Build();
            var options = serviceProvider.GetRequiredService<IOptions<StoreOptions>>().Value;

            A.CallTo(() => configurationDelegate(options))
                .MustHaveHappened(Repeated.Exactly.Once)
                .Then(A.CallTo(() => componentConfigurationDelegate(A<IServiceProvider>._, options))
                    .MustHaveHappened(Repeated.Exactly.Once));
        }

        [Fact]
        public void MustRegisterListeners_Implicitly()
        {
            var configurationDelegate = A.Fake<MartenConfigurationDelegate>();
            var componentConfigurationDelegate = A.Fake<MartenComponentConfigurationDelegate>();

            AutoFake.Provide<IServiceCollection>(new ServiceCollection());
            var servicesBuilder = AutoFake.Resolve<ServicesBuilder>();
            servicesBuilder.Services.AddTransient<MartenRegistry, MyMartenRegistry>();
            servicesBuilder.Services.AddSingleton<ILoggerFactory>(LoggerFactory);
            servicesBuilder.Services.AddSingleton<IClock>(
                new FakeClock(Instant.FromDateTimeOffset(DateTimeOffset.Now),
                Duration.FromSeconds(1))
            );
            var martenBuilder = servicesBuilder
                .WithMarten()
                .AddStartupFilter()
                .Configure(configurationDelegate)
                .Configure(componentConfigurationDelegate);
            servicesBuilder.Services.AddScoped<IMartenUser>(_ => new MartenUser<string>(() => "abc123"));

            martenBuilder.UseDirtyTrackedSession();

            var serviceProvider = servicesBuilder.Build();
            var options = serviceProvider.GetRequiredService<IOptions<StoreOptions>>().Value;
            options.Connection(() => new NpgsqlConnection());

            var session = serviceProvider.GetService<IDocumentSession>();

            session.Listeners.Count.Should().Be(1);
        }

        [Fact]
        public void MustRegisterListeners_Explicitly()
        {
            var configurationDelegate = A.Fake<MartenConfigurationDelegate>();
            var componentConfigurationDelegate = A.Fake<MartenComponentConfigurationDelegate>();

            AutoFake.Provide<IServiceCollection>(new ServiceCollection());
            var servicesBuilder = AutoFake.Resolve<ServicesBuilder>();
            servicesBuilder.Services.AddTransient<MartenRegistry, MyMartenRegistry>();
            servicesBuilder.Services.AddSingleton<ILoggerFactory>(LoggerFactory);
            servicesBuilder.Services.AddSingleton<IClock>(
                new FakeClock(Instant.FromDateTimeOffset(DateTimeOffset.Now),
                    Duration.FromSeconds(1))
            );
            var martenBuilder = servicesBuilder
                .WithMarten()
                .AddStartupFilter()
                .Configure(configurationDelegate)
                .Configure(componentConfigurationDelegate);
            servicesBuilder.Services.AddScoped<IMartenUser>(_ => new MartenUser<string>(() => "abc123"));

            martenBuilder.UseDirtyTrackedSession();

            var serviceProvider = servicesBuilder.Build();
            var options = serviceProvider.GetRequiredService<IOptions<StoreOptions>>().Value;
            options.Connection(() => new NpgsqlConnection());

            using (var scope = serviceProvider.GetService<IServiceScopeFactory>().CreateScope())
            {
                var session = scope.ServiceProvider.GetService<IDocumentStore>().LightweightSession();
                session.Listeners.Count.Should().Be(1);
            }
        }
    }
}
