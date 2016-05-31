using System;
using MichaelsPlace.Models.Persistence;
using Newtonsoft.Json;

namespace MichaelsPlace.Infrastructure
{
    public class EventStore : IEventStore
    {
        private readonly object _sync = new object();
        private readonly ApplicationDbContext _dbContext;

        public EventStore(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Save(IDurableEvent durableEvent)
        {
            var json = JsonConvert.SerializeObject(durableEvent, GlobalSettings.EventSerializerSettings);
            var @event = new Event()
                         {
                             Id = durableEvent.Id.ToString(),
                             ContentJson = json,
                             TimestampUtc = DateTimeOffset.UtcNow
                         };

            lock (_sync)
            {
                _dbContext.Events.Add(@event);
                _dbContext.SaveChanges();
            }
        }
    }
}