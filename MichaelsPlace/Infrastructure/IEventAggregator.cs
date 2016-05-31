using System;

namespace MichaelsPlace.Infrastructure
{
    public interface IEventAggregator
    {
        void Publish<TEvent>(TEvent @event);
        IObservable<TEvent> Observe<TEvent>();
    }
}