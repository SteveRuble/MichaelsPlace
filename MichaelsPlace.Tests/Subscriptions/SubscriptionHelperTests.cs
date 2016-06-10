using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using MichaelsPlace.Services;
using NUnit.Framework;

namespace MichaelsPlace.Tests.Subscriptions
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
