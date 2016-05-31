namespace MichaelsPlace.Infrastructure
{
    public interface IEventStore
    {
        void Save(IDurableEvent durableEvent);
    }
}