using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Twilio;

namespace MichaelsPlace.Infrastructure.Identity
{
    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Twilio Begin
            var twilio = new TwilioRestClient(
              GlobalSettings.Twilio.AccountId, GlobalSettings.Twilio.AuthToken);
            var result = twilio.SendMessage(
              GlobalSettings.Twilio.FromNumber,
              message.Destination, message.Body
            );
            //Status is one of Queued, Sending, Sent, Failed or null if the number is not valid
            // Trace.TraceInformation(result.Status);
            //Twilio doesn't currently have an async API, so return success.
             return Task.FromResult(0);
            // Twilio End
        }
    }
}