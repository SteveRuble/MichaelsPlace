using MichaelsPlace.Models.Persistence;
using Microsoft.AspNet.Identity;

namespace MichaelsPlace.Infrastructure.Messaging
{
    public interface IEmailSender : IIdentityMessageService
    {
        void Send(EmailNotification notification);
    }
}