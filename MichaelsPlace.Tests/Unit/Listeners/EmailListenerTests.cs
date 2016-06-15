using MichaelsPlace.Listeners;
using MichaelsPlace.Models.Persistence;
using MichaelsPlace.Services.Messaging;
using Moq;
using NUnit.Framework;

namespace MichaelsPlace.Tests.Unit.Listeners
{
    [TestFixture]
    public class EmailListenerTests : ListenerTestBase<EmailNotificationAddedNotificationHandler, EntityAdded<EmailNotification>>
    {
        public Mock<IEmailService> MockEmailSender => Kernel.GetMock<IEmailService>();

        [Test]
        public void message_is_received()
        {
            var notification = new EmailNotification();
            WhenAnEntityIsAdded(notification);
            MockEmailSender.Verify(m => m.Send(notification));
        }

    }
}