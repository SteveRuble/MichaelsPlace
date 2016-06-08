using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using MichaelsPlace.Infrastructure;
using MichaelsPlace.Models.Persistence;
using NUnit.Framework;

namespace MichaelsPlace.Tests.Infrastructure
{
    [TestFixture]
    [Category("Integration")]
    public class EventStoreIntegrationTests : IntegrationTestBase
    {
        public EventStore Target { get; set; }

        [SetUp]
        public void SetUp()
        {
            Target = new EventStore(DbContext);
        }

        [Test]
        public void store_event()
        {
            var expected = new CaseClosedEvent()
                         {
                             CaseId = Guid.NewGuid().ToString()
                         };

            Target.Save(expected);

            expected.Id.Should().NotBeNullOrEmpty();

            var actual = Target.Load<CaseClosedEvent>(expected.Id);

            actual.CaseId.Should().Be(expected.CaseId);

        }
    }
}
