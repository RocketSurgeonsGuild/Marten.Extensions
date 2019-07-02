using System;
using System.Collections.Generic;
using FluentAssertions;
using Marten;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NodaTime;
using NodaTime.Testing;
using Npgsql;
using Rocket.Surgery.Conventions;
using Rocket.Surgery.Extensions.DependencyInjection;
using Rocket.Surgery.Extensions.Testing;
using Xunit;
using Xunit.Abstractions;

namespace Rocket.Surgery.Extensions.Marten.Tests
{
    public class MartenServicesBuilderTests : AutoTestBase
    {
        public MartenServicesBuilderTests(ITestOutputHelper outputHelper) : base(outputHelper) { }

        [Fact]
        public void MustRegisterListeners_Implicitly()
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
            servicesBuilder.Services.AddSingleton<ILoggerFactory>(LoggerFactory);
            servicesBuilder.Services.AddSingleton<IClock>(
                new FakeClock(Instant.FromDateTimeOffset(DateTimeOffset.Now),
                Duration.FromSeconds(1))
            );
            var martenBuilder = servicesBuilder.WithMarten();
            servicesBuilder.Services.AddScoped<IMartenContext>(_ => new MartenContext() { User = new MartenUser<string>(() => "abc123") });

            var serviceProvider = servicesBuilder.Build();
            var options = serviceProvider.GetRequiredService<IOptions<StoreOptions>>().Value;
            options.Connection(() => new NpgsqlConnection());

            var session = serviceProvider.GetService<IDocumentSession>();

            session.Listeners.Count.Should().Be(1);
        }

        [Fact]
        public void MustRegisterListeners_Explicitly()
        {
            AutoFake.Provide<IServiceCollection>(new ServiceCollection());
            var serviceProviderDictionary = new ServiceProviderDictionary();
            AutoFake.Provide<IServiceProviderDictionary>(serviceProviderDictionary);
            AutoFake.Provide<IServiceProvider>(serviceProviderDictionary);
            AutoFake.Provide<IDictionary<object, object>>(serviceProviderDictionary);
            var servicesBuilder = AutoFake.Resolve<ServicesBuilder>();
            servicesBuilder.Services.AddTransient<MartenRegistry, MyMartenRegistry>();
            servicesBuilder.Services.AddSingleton<ILoggerFactory>(LoggerFactory);
            servicesBuilder.Services.AddSingleton<IClock>(
                new FakeClock(Instant.FromDateTimeOffset(DateTimeOffset.Now),
                    Duration.FromSeconds(1))
            );
            var martenBuilder = servicesBuilder.WithMarten();
            servicesBuilder.Services.AddScoped<IMartenContext>(_ => new MartenContext() { User = new MartenUser<string>(() => "abc123") });

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
