using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace MichaelsPlace.Infrastructure.Identity
{
    public class IdentityEmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            return Task.FromResult(0);
        }
    }
}