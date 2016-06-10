using MichaelsPlace.Models.Persistence;
using Microsoft.AspNet.Identity;

namespace MichaelsPlace.Infrastructure.Messaging
{
    public interface ISmsSender : IIdentityMessageService
    {
        void Send(SmsNotification notification);
    }
}