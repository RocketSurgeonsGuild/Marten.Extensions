using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Marten;
using Marten.Events;
using Marten.Events.Projections;
using Marten.Events.Projections.Async;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Rocket.Surgery.Conventions;
using Rocket.Surgery.Conventions.Reflection;
using Rocket.Surgery.Conventions.Scanners;
using Rocket.Surgery.Extensions.DependencyInjection;
using Rocket.Surgery.Extensions.Marten.Projections;
using Rocket.Surgery.Extensions.Testing;
using Xunit;
using Xunit.Abstractions;
using ProjectionType = Rocket.Surgery.Extensions.Marten.Projections.ProjectionType;

namespace Rocket.Surgery.Extensions.Marten.Tests
{
    public class InlineProjectionTests : AutoFakeTest
    {
        [Fact]
        public void ShouldContainerAllAggregatedTypes()
        {
            var options = _serviceProvider.GetRequiredService<IOptions<StoreOptions>>().Value;

            options.Events.InlineProjections.Should().Contain(
                x => x.GetType() == typeof(AggregationProjection<InlineProjectionAttributed>)
            );
            options.Events.InlineProjections.Should().Contain(x => x.GetType() == typeof(InlineProjection));
            options.Events.InlineProjections.Should()
               .Contain(x => x.GetType() == typeof(OneForOneProjection<Dto, View>));

            options.Events.AsyncProjections.Should().Contain(
                x => x.GetType() == typeof(AggregationProjection<AsyncProjectionAttributed>)
            );
            options.Events.AsyncProjections.Should().Contain(x => x.GetType() == typeof(AsyncProjection));
            options.Events.AsyncProjections.Should()
               .Contain(x => x.GetType() == typeof(OneForOneProjection<Dto, View>));
        }

        public InlineProjectionTests(ITestOutputHelper outputHelper) : base(outputHelper)
        {
            AutoFake.Provide<IServiceCollection>(new ServiceCollection());
            AutoFake.Provide<IAssemblyProvider>(new TestAssemblyProvider());
            AutoFake.Provide<IAssemblyCandidateFinder>(new TestAssemblyCandidateFinder());
            AutoFake.Provide<IConventionScanner>(
                new AggregateConventionScanner(
                    new TestAssemblyCandidateFinder(),
                    new ServiceProviderDictionary(),
                    Logger
                )
            );
            var servicesBuilder = AutoFake.Resolve<ServicesBuilder>();
            servicesBuilder.Services.AddTransient<MartenRegistry, MyMartenRegistry>();
            servicesBuilder.Services.AddTransient<IInlineProjection, InlineProjection>();
            servicesBuilder.Services.AddTransient<IAsyncProjection, AsyncProjection>();
            servicesBuilder.Services.AddTransient<IInlineProjection, InlineTransform>();
            servicesBuilder.Services.AddTransient<IAsyncProjection, AsyncTransform>();
            servicesBuilder.Services.AddSingleton(LoggerFactory);
            var martenBuilder = servicesBuilder
               .WithMarten();
            _serviceProvider = servicesBuilder.Build();
        }

        private readonly IServiceProvider _serviceProvider;

#nullable disable
        [Projection()]
        private class InlineProjectionAttributed
        {
            public string Id { get; set; }
        }

        [Projection(ProjectionType.Async)]
        private class AsyncProjectionAttributed
        {
            public string Id { get; set; }
        }

        private class Dto
        {
            public string Id { get; set; }
        }

        private class View
        {
            public string Id { get; set; }
        }
#nullable restore
        private class InlineProjection : DocumentProjection<Dto>, IDocumentProjection, IInlineProjection
        {
            public Type[] Consumes => throw new NotImplementedException();

            public AsyncOptions AsyncOptions => throw new NotImplementedException();

            public void Apply(IDocumentSession session, EventPage page) => throw new NotImplementedException();

            public Task ApplyAsync(IDocumentSession session, EventPage page, CancellationToken token)
                => throw new NotImplementedException();
        }

        private class AsyncProjection : DocumentProjection<Dto>, IDocumentProjection, IAsyncProjection
        {
            public Type[] Consumes => throw new NotImplementedException();

            public AsyncOptions AsyncOptions => throw new NotImplementedException();

            public void Apply(IDocumentSession session, EventPage page) => throw new NotImplementedException();

            public Task ApplyAsync(IDocumentSession session, EventPage page, CancellationToken token)
                => throw new NotImplementedException();
        }

        private class InlineTransform : ITransform<Dto, View>, IInlineProjection
        {
            public View Transform(EventStream stream, Event<Dto> input) => throw new NotImplementedException();
        }

        private class AsyncTransform : ITransform<Dto, View>, IAsyncProjection
        {
            public View Transform(EventStream stream, Event<Dto> input) => throw new NotImplementedException();
        }
    }
}