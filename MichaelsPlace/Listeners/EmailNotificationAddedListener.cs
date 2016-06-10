using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MichaelsPlace.Infrastructure.Messaging;
using MichaelsPlace.Models.Persistence;
using MichaelsPlace.Utilities;
using Ninject;

namespace MichaelsPlace.Listeners
{
    [UsedImplicitly]
    public class EmailNotificationAddedListener : ListenerBase<EntityAdded<EmailNotification>>
    {
        private Injected<IEmailSender> _emailSender;

        [Inject]
        public IEmailSender EmailSender
        {
            get { return _emailSender.Value; }
            set { _emailSender.Value = value; }
        }

        public override void Handle(EntityAdded<EmailNotification> message)
        {
            EmailSender.Send(message.Entity);
        }
    }
}
