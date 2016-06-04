using System;

namespace MichaelsPlace.Infrastructure
{
    public interface IMessageBus
    {
        void Publish<TEvent>(TEvent @event);
        IObservable<TEvent> Observe<TEvent>();
    }
}