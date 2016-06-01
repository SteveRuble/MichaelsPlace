using System;
using System.Collections.Generic;
using System.Linq;
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

        public void Save(EventBase @event)
        {
            if (@event == null) throw new ArgumentNullException(nameof(@event));

            var json = JsonConvert.SerializeObject(@event, GlobalSettings.EventSerializerSettings);
            var historicalEvent = new HistoricalEvent()
                                  {
                                      Id = @event.Id,
                                      TimestampUtc = @event.TimestampUtc,
                                      EventType = @event.GetType().AssemblyQualifiedName,
                                      ContentJson = json,
                                  };
            lock (_sync)
            {
                _dbContext.HistoricalEvents.Add(historicalEvent);
                _dbContext.SaveChanges();
            }
        }

        public IEnumerable<TEvent> Load<TEvent>(DateTimeOffset? fromUtc = null) where TEvent : EventBase
        {
            fromUtc = fromUtc ?? DateTimeOffset.UtcNow.AddHours(-24);
            var typeName = typeof(TEvent).AssemblyQualifiedName;

            List<HistoricalEvent> historicalEvents;
            lock (_sync)
            {
                historicalEvents = _dbContext.HistoricalEvents.Where(he => he.TimestampUtc > fromUtc.Value && he.EventType == typeName).ToList();
            }

            return historicalEvents.Select(MaterializeEvent<TEvent>);
        }

        public TEvent Load<TEvent>(string id) where TEvent : EventBase
        {
            HistoricalEvent historicalEvent;
            lock (_sync)
            {
                historicalEvent = _dbContext.HistoricalEvents.Find(id);
            }

            if (historicalEvent == null)
            {
                return null;
            }

            return MaterializeEvent<TEvent>(historicalEvent);
        }

        private static TEvent MaterializeEvent<TEvent>(HistoricalEvent historicalEvent) where TEvent : EventBase
        {
            var type = Type.GetType(historicalEvent.EventType, true);
            var @event = (TEvent) JsonConvert.DeserializeObject(historicalEvent.ContentJson, type, GlobalSettings.EventSerializerSettings);

            @event.Id = historicalEvent.Id;
            @event.TimestampUtc = historicalEvent.TimestampUtc;

            return @event;
        }
    }
}