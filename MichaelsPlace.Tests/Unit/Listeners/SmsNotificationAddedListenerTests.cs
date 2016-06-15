using MichaelsPlace.Listeners;
using MichaelsPlace.Models.Persistence;
using MichaelsPlace.Services.Messaging;
using Moq;
using NUnit.Framework;

namespace MichaelsPlace.Tests.Unit.Listeners
{
    [TestFixture]
    public class SmsNotificationAddedListenerTests : ListenerTestBase<SmsNotificationAddedNotificationHandler, EntityAdded<SmsNotification>>
    {
        public Mock<ISmsService> MockEmailSender => Kernel.GetMock<ISmsService>();

        [Test]
        public void message_is_received()
        {
            var notification = new SmsNotification();
            WhenAnEntityIsAdded(notification);
            MockEmailSender.Verify(m => m.Send(notification));
        }

    }
}