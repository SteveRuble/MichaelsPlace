using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MichaelsPlace.Models.Persistence;
using MichaelsPlace.Services.Messaging;
using MichaelsPlace.Utilities;
using Ninject;

namespace MichaelsPlace.Listeners
{
    [UsedImplicitly]
    public class EmailNotificationAddedNotificationHandler : NotificationHandlerBase<EntityAdded<EmailNotification>>
    {
        private Injected<IEmailService> _emailSender;

        [Inject]
        public IEmailService EmailService
        {
            get { return _emailSender.Value; }
            set { _emailSender.Value = value; }
        }
        
        public override void Handle(EntityAdded<EmailNotification> message)
        {
            EmailService.Send(message.Entity);
        }
    }
}
