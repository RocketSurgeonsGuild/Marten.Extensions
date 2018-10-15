using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using FakeItEasy;
using FluentAssertions;
using Marten.Events;
using Marten.Events.Projections;
using Marten.Events.Projections.Async;
using NodaTime;
using NodaTime.Testing;
using Rocket.Surgery.Extensions.Marten.AspNetCore;
using Rocket.Surgery.Extensions.Marten;
using Rocket.Surgery.Extensions.Marten.Listeners;
using Rocket.Surgery.Extensions.Marten.Projections;
using Rocket.Surgery.Extensions.Marten.Security;
using Rocket.Surgery.Conventions.Reflection;
using Rocket.Surgery.Conventions.Scanners;
using Rocket.Surgery.Domain;
using Xunit;
using ProjectionType = Rocket.Surgery.Extensions.Marten.Projections.ProjectionType;

namespace Rocket.Surgery.Marten.Tests
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
                new MartenUser<string>(() => "abc123")
            );
            var document = new OwnerDocument();

            listener.Apply(document);

            document.Owner.Id.Should().Be("abc123");
        }
    }
}
