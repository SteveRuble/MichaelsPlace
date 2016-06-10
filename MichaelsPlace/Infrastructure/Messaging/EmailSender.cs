using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MichaelsPlace.Models.Persistence;
using MichaelsPlace.Utilities;
using Microsoft.AspNet.Identity;
using Ninject;
using Serilog;

namespace MichaelsPlace.Infrastructure.Messaging
{
    public class DevelopmentEmailSender : IEmailSender
    {
        private Injected<ILogger> _logger;

        [Inject]
        public ILogger Logger
        {
            get { return _logger.Value; }
            set { _logger.Value = value; }
        }


        public void Send(EmailNotification notification)
        {
            Logger.Information("Sent email {Email}", notification);
        }

        public Task SendAsync(IdentityMessage message)
        {
            var notification = new EmailNotification()
                               {
                                   Content = message.Body,
                                   Subject = message.Subject,
                                   ToAddress = message.Destination
                               };

            Logger.Information("Sent identity email {Email}", notification);

            return Task.CompletedTask;
        }
    }
}
