using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MichaelsPlace.Models.Persistence;
using Serilog;

namespace MichaelsPlace.Infrastructure
{
    public class MessageBus : IMessageBus
    {
        private readonly ILogger _logger;
        private readonly ConcurrentDictionary<Type, object> _subjects = new ConcurrentDictionary<Type, object>();

        public MessageBus(ILogger logger)
        {
            _logger = logger;
        }

        public IObservable<TEvent> Observe<TEvent>()
        {
            var subject = (ISubject<TEvent>)_subjects.GetOrAdd(typeof(TEvent), t => new Subject<TEvent>());
            return subject.AsObservable();
        }

        public void Publish<TEvent>(TEvent @event)
        {
            object subject;
            if (_subjects.TryGetValue(typeof(TEvent), out subject))
            {
                ((ISubject<TEvent>) subject).OnNext(@event);
            }
            else
            {
                _logger.Debug("No subscribers for event {@event}", @event);
            }
        }
    }
}
