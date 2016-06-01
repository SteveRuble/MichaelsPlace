using MichaelsPlace.Models.Persistence;

namespace MichaelsPlace.Infrastructure
{
    public interface IEventStore
    {
        void Save(EventBase @event);
        TEvent Load<TEvent>(string id) where TEvent : EventBase;
    }
}