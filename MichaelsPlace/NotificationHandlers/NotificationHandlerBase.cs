using System;
using System.Threading.Tasks;
using MediatR;
using MichaelsPlace.Handlers;
using MichaelsPlace.Infrastructure;
using MichaelsPlace.Utilities;
using Ninject;

namespace MichaelsPlace.Listeners
{
    /// <summary>
    /// Base class for simple listeners which subscribe to a single type of message.
    /// Derived implementations can customize the subscription if needed.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public abstract class NotificationHandlerBase<TMessage> : INotificationHandler<TMessage> where TMessage : INotification
    {
        private Injected<IMediator> _mediator;

        [Inject]
        public IMediator Mediator
        {
            get { return _mediator.Value; }
            set { _mediator.Value = value; }
        }

        /// <summary>
        /// When implemented by a derived class, handles a <paramref name="notification"/> of type <typeparamref name="TMessage"/>.
        /// </summary>
        /// <param name="notification"></param>
        public abstract void Handle(TMessage notification);
    }
}