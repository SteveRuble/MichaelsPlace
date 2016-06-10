using System;
using MichaelsPlace.Infrastructure.Messaging;
using MichaelsPlace.Listeners;
using MichaelsPlace.Models.Persistence;
using Moq;
using NUnit.Framework;

namespace MichaelsPlace.Tests
{
    [TestFixture]
    public class SmsNotificationAddedListenerTests : ListenerTestBase<SmsNotificationAddedListener>
    {
        public Mock<ISmsSender> MockEmailSender => Kernel.GetMock<ISmsSender>();

        [Test]
        public void message_is_received()
        {
            var notification = new SmsNotification();
            WhenAnEntityIsAdded(notification);
            MockEmailSender.Verify(m => m.Send(notification));
        }

    }
}