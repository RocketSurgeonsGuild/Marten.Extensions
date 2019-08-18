using System;
using FluentAssertions;
using NodaTime;
using Rocket.Surgery.Domain;
using Rocket.Surgery.Extensions.Marten.Listeners;
using Xunit;

namespace Rocket.Surgery.Extensions.Marten.Tests
{
    public class UpdatedDocumentSessionListenerTests
    {
        class CreatedDocument : IHaveCreatedBy<string>
        {
#nullable disable
            public ChangeData<string> Created { get; }
#nullable restore
            public ChangeData<string>? Updated { get; }
        }

        class UpdatedDocument : IHaveUpdatedBy<Guid>
        {
#nullable disable
            public ChangeData<Guid> Created { get; }
#nullable restore
            public ChangeData<Guid>? Updated { get; }
        }

        class CreatedUpdatedDocument : IHaveCreatedBy<long>, IHaveUpdatedBy<long>
        {
#nullable disable
            public ChangeData<long> Created { get; }
#nullable restore
            public ChangeData<long>? Updated { get; }
        }

        [Fact]
        public void Should_Work_With_Created_Document()
        {
            var instant = Instant.FromUtc(2019, 1, 1, 0, 0);
            var document = new CreatedDocument();

            MartenDocumentSessionListener.Apply(document, "abc123", instant);

            document.Created.By.Should().Be("abc123");
            document.Created.At.Should().Be(instant);
        }

        [Fact]
        public void Should_Work_With_Updated_Document()
        {
            var instant = Instant.FromUtc(2019, 1, 1, 0, 0);
            var guid = Guid.NewGuid();
            var document = new UpdatedDocument();

            MartenDocumentSessionListener.Apply(document, guid, instant);

            document.Updated.By.Should().Be(guid);
            document.Updated.At.Should().Be(instant);
        }

        [Fact]
        public void Should_Work_With_Created_And_Updated_Document()
        {
            var instant = Instant.FromUtc(2019, 1, 1, 0, 0);
            var document = new CreatedUpdatedDocument();

            MartenDocumentSessionListener.Apply((IHaveCreatedBy<long>)document, 456789, instant);
            MartenDocumentSessionListener.Apply((IHaveUpdatedBy<long>)document, 456789, instant.Plus(Duration.FromSeconds(1)));

            document.Created.By.Should().Be(456789);
            document.Created.At.Should().Be(instant);
            document.Updated.By.Should().Be(456789);
            document.Updated.At.Should().Be(instant.Plus(Duration.FromSeconds(1)));
        }
    }
}
