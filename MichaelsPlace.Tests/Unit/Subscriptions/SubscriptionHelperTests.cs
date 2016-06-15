using FluentAssertions;
using MichaelsPlace.Services;
using NUnit.Framework;

namespace MichaelsPlace.Tests.Unit.Subscriptions
{
    [TestFixture]
    public class SubscriptionHelperTests
    {
        public ReflectionService Target = new ReflectionService();

        [Test]
        public void attributed_listeners_are_found()
        {
            Target.GetSubscriptionDescriptions().Should().Contain(d => d.Name == Constants.Subscriptions.UserCaseCreated);
        }
    }
}
