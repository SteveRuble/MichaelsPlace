using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using MichaelsPlace.Subscriptions;
using NUnit.Framework;

namespace MichaelsPlace.Tests.Subscriptions
{
    [TestFixture]
    public class SubscriptionHelperTests
    {
        public SubscriptionHelper Target = new SubscriptionHelper();

        [Test]
        public void attributed_listeners_are_found()
        {
            Target.SubscriptionDescriptions.Should().Contain(d => d.Name == Constants.Subscriptions.UserCaseCreated);
        }
    }
}
