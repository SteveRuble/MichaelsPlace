using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace MichaelsPlace.Infrastructure
{
    public class EventAggregator : IEventAggregator
    {
        private readonly IEventStore _eventStore;
        private readonly ConcurrentDictionary<Type, object> _subjects = new ConcurrentDictionary<Type, object>();

        public EventAggregator(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public IObservable<TEvent> Observe<TEvent>()
        {
            var subject = (ISubject<TEvent>)_subjects.GetOrAdd(typeof(TEvent), t => new Subject<TEvent>());
            return subject.AsObservable();
        }

        public void Publish<TEvent>(TEvent @event)
        {
            if (@event is IDurableEvent)
            {
                _eventStore.Save(@event as IDurableEvent);
            }

            object subject;
            if (_subjects.TryGetValue(typeof(TEvent), out subject))
            {
                ((ISubject<TEvent>)subject).OnNext(@event);
            }
        }
    }
}
