using MichaelsPlace.Models.Persistence;
using Microsoft.AspNet.Identity;

namespace MichaelsPlace.Services.Messaging
{
    public interface IEmailService : IIdentityMessageService
    {
        void Send(EmailNotification notification);
    }
}