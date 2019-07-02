using System;
using FluentAssertions;
using NodaTime;
using NodaTime.Testing;
using Rocket.Surgery.Domain;
using Rocket.Surgery.Extensions.Marten.Listeners;
using Xunit;

namespace Rocket.Surgery.Extensions.Marten.Tests
{

    public class OwnerDocumentSessionListenerTests
    {
        class OwnerDocument : IHaveOwner<string>
        {
            public string Id { get; set; }
            public OwnerData<string> Owner { get; }
        }

        [Fact]
        public void Should_Work_With_Owner_Document()
        {
            var listener = new MartenDocumentSessionListener(
                new FakeClock(Instant.FromDateTimeOffset(DateTimeOffset.Now), Duration.FromSeconds(1)),
                new MartenContext() { User = new MartenUser<string>(() => "abc123") }
            );
            var document = new OwnerDocument();

            MartenDocumentSessionListener.Apply(document, "abc123");

            document.Owner.Id.Should().Be("abc123");
        }
    }
}
