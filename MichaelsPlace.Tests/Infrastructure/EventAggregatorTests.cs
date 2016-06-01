using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using FluentAssertions;
using MichaelsPlace.Infrastructure;
using MichaelsPlace.Models.Persistence;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace MichaelsPlace.Tests.Infrastructure
{
    public class TestPersistedHistoricalEvent : EventBase
    {
        public string Payload { get; set; }
    }

    public class TestEvent 
    {
        public string Payload { get; set; }
    }

    [TestFixture]
    public class EventAggregatorTests
    {
        public EventAggregator Target { get; set; }
        public Mock<IEventStore> MockEventStore { get; set; }


        [SetUp]
        public void SetUp()
        {
            MockEventStore = new Mock<IEventStore>(MockBehavior.Strict);
            Target = new EventAggregator(MockEventStore.Object);
        }

        [Test]
        public void durable_events_are_stored()
        {
            var @event = new TestPersistedHistoricalEvent() {Payload = "test"};
            MockEventStore.Setup(m => m.Save(@event)).Verifiable();

            Target.Publish(@event);

            MockEventStore.Verify();
        }

        [Test]
        public void non_durable_events_are_not_stored()
        {
            var @event = new TestEvent() {Payload = "test"};

            Target.Publish(@event);
        }

        [Test]
        public async Task events_are_published_to_observer()
        {
            var subscription = Target.Observe<TestEvent>().FirstOrDefaultAsync().ToTask();

            var expected = new TestEvent() {Payload = "test"};

            Target.Publish(expected);

            var actual = await subscription;

            actual.Should().Be(expected);
        }
    }
}
