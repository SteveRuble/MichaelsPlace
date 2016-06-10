using MichaelsPlace.Models.Persistence;
using Microsoft.AspNet.Identity;

namespace MichaelsPlace.Services.Messaging
{
    public interface ISmsService : IIdentityMessageService
    {
        void Send(SmsNotification notification);
    }
}