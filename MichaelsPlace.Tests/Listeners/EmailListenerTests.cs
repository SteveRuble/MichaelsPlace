using MichaelsPlace.Infrastructure.Messaging;
using MichaelsPlace.Listeners;
using MichaelsPlace.Models.Persistence;
using Moq;
using NUnit.Framework;

namespace MichaelsPlace.Tests
{
    [TestFixture]
    public class EmailListenerTests : ListenerTestBase<EmailNotificationAddedListener>
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