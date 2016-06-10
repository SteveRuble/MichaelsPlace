using JetBrains.Annotations;
using MichaelsPlace.Infrastructure.Messaging;
using MichaelsPlace.Models.Persistence;
using MichaelsPlace.Utilities;
using Ninject;

namespace MichaelsPlace.Listeners
{
    [UsedImplicitly]
    public class SmsNotificationAddedListener : ListenerBase<EntityAdded<SmsNotification>>
    {
        private Injected<ISmsSender> _smsSender;

        [Inject]
        public ISmsSender SmsSender
        {
            get { return _smsSender.Value; }
            set { _smsSender.Value = value; }
        }


        public override void Handle(EntityAdded<SmsNotification> message)
        {
            SmsSender.Send(message.Entity);
        }
    }
}