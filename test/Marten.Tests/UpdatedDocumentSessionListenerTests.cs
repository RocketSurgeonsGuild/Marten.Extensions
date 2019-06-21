using System;
using FluentAssertions;
using NodaTime;
using NodaTime.Testing;
using Rocket.Surgery.Extensions.Marten;
using Rocket.Surgery.Extensions.Marten.Listeners;
using Rocket.Surgery.Domain;
using Xunit;

namespace Rocket.Surgery.Marten.Tests
{
    public class UpdatedDocumentSessionListenerTests
    {
        class CreatedDocument : IHaveCreatedBy<string>
        {
            public ChangeData<string> Created { get; }
            public ChangeData<string> Updated { get; }
        }

        class UpdatedDocument : IHaveUpdatedBy<Guid>
        {
            public ChangeData<Guid> Created { get; }
            public ChangeData<Guid> Updated { get; }
        }

        class CreatedUpdatedDocument : IHaveCreatedBy<long>, IHaveUpdatedBy<long>
        {
            public ChangeData<long> Created { get; }
            public ChangeData<long> Updated { get; }
        }

        [Fact]
        public void Should_Work_With_Created_Document()
        {
            var listener = new MartenDocumentSessionListener(
                new FakeClock(Instant.FromDateTimeOffset(DateTimeOffset.Now), Duration.FromSeconds(1)),
                new MartenContext() { User = new MartenUser<string>(() => "abc123") }
            );
            var document = new CreatedDocument();

            MartenDocumentSessionListener.Apply(document, "abc123", DateTimeOffset.Now);

            document.Created.By.Should().Be("abc123");
            document.Created.At.Should().BeCloseTo(DateTimeOffset.Now, 1000);
            document.Updated?.By.Should().BeNull();
            document.Updated?.At.Should().BeNull();
        }

        [Fact]
        public void Should_Work_With_Updated_Document()
        {
            var guid = Guid.NewGuid();
            var listener = new MartenDocumentSessionListener(
                new FakeClock(Instant.FromDateTimeOffset(DateTimeOffset.Now), Duration.FromSeconds(1)),
                new MartenContext()
                {
                    User = new MartenUser<Guid>(() => guid)
                }
            );
            var document = new UpdatedDocument();

            MartenDocumentSessionListener.Apply(document, guid, DateTimeOffset.Now);

            document.Created?.By.Should().BeEmpty();
            document.Created?.At.Should().BeNull();
            document.Updated.By.Should().Be(guid);
            document.Updated.At.Should().BeCloseTo(DateTimeOffset.Now, 1000);
        }

        [Fact]
        public void Should_Work_With_Created_And_Updated_Document()
        {
            var listener = new MartenDocumentSessionListener(
                new FakeClock(Instant.FromDateTimeOffset(DateTimeOffset.Now), Duration.FromSeconds(1)),
                new MartenContext()
                {
                    User = new MartenUser<long>(() => 456789)
                        }
            );
            var document = new CreatedUpdatedDocument();

            MartenDocumentSessionListener.Apply((IHaveCreatedBy<long>) document, 456789, DateTimeOffset.Now);
            MartenDocumentSessionListener.Apply((IHaveUpdatedBy<long>) document, 456789, DateTimeOffset.Now);

            document.Created.By.Should().Be(456789);
            document.Created.At.Should().BeCloseTo(DateTimeOffset.Now, 1000);
            document.Updated.By.Should().Be(456789);
            document.Updated.At.Should().BeCloseTo(DateTimeOffset.Now, 1000);
        }
    }
}
