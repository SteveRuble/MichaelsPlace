using System.Threading.Tasks;
using MichaelsPlace.Models.Persistence;
using MichaelsPlace.Utilities;
using Microsoft.AspNet.Identity;
using Ninject;
using Serilog;
using Twilio;

namespace MichaelsPlace.Services.Messaging
{
    public class DevelopmentSmsService : ISmsService
    {
        private Injected<ILogger> _logger;

        [Inject]
        public ILogger Logger
        {
            get { return _logger.Value; }
            set { _logger.Value = value; }
        }


        public void Send(SmsNotification notification)
        {
            Logger.Information("Sent SMS {SMS}", notification);
        }

        public Task SendAsync(IdentityMessage message)
        {
            var notification = new SmsNotification()
                               {
                                   ToPhoneNumber = message.Destination,
                                   Content = message.Body
                               };
            Send(notification);
            return Task.CompletedTask;
        }


    }

    public class SmsService : ISmsService
    {
        private Injected<ILogger> _logger;

        [Inject]
        public ILogger Logger
        {
            get { return _logger.Value; }
            set { _logger.Value = value; }
        }


        public void Send(SmsNotification notification)
        {
            Logger.Information("Sent SMS {SMS}", notification);
        }

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