using System;
using MichaelsPlace.Handlers;
using MichaelsPlace.Infrastructure;

namespace MichaelsPlace.Listeners
{
    /// <summary>
    /// Base class for simple listeners which subscribe to a single type of message.
    /// Derived implementations can customize the subscription if needed.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public abstract class ListenerBase<TMessage> : IListener
    {
        /// <summary>
        /// Subscribes to messages of type <typeparamref name="TMessage"/>.
        /// When overridden in a derived class, additional filtering of the subscription can be applied.
        /// </summary>
        /// <param name="bus"></param>
        /// <returns></returns>
        public virtual IDisposable SubscribeTo(IMessageBus bus)
        {
            return bus.Observe<TMessage>().Subscribe(Handle);
        }

        /// <summary>
        /// When implemented by a derived class, handles a <paramref name="message"/> of type <typeparamref name="TMessage"/>.
        /// </summary>
        /// <param name="message"></param>
        public abstract void Handle(TMessage message);
    }
}