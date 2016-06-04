using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using MichaelsPlace.Infrastructure;
using MichaelsPlace.Models.Persistence;
using MichaelsPlace.Queries;

namespace MichaelsPlace.Handlers
{
    /// <summary>
    /// Contract to listen for events.
    /// </summary>
    public interface IListener
    {
        IDisposable SubscribeTo(IMessageBus bus);
    }

    public class CaseCreatedListener : IListener
    {
        private readonly PreferencesQuery _preferencesQuery;
        private readonly IEntitySaver _entitySaver;

        public CaseCreatedListener(PreferencesQuery preferencesQuery, IEntitySaver entitySaver)
        {
            _preferencesQuery = preferencesQuery;
            _entitySaver = entitySaver;
        }

        public IDisposable SubscribeTo(IMessageBus bus)
        {
            return bus.Observe<EntityAdded<Case>>()
                      .Subscribe(e => OnCaseCreated(e.Entity));
        }

        public void OnCaseCreated(Case createdCase)
        {
            var recipients = _preferencesQuery.Execute<SubscriptionPreference>()
                                              .Where(p => p.EventType == SubscribableEventType.NewIndividualCase
                                                          || p.EventType == SubscribableEventType.NewOrganizationCase);

            foreach (var recipient in recipients)
            {
                if (recipient.IsEmailRequested)
                {
                    _entitySaver.Save(new EmailNotification()
                                      {
                                          ToAddress = recipient.User.Email,
                                          Content = $"New case created: {createdCase.Id}",
                                          Subject = $"New case created: {createdCase.Id}"
                                      });
                }
                if (recipient.IsSmsRequested)
                {
                    _entitySaver.Save(new SmsNotification()
                                      {
                                          ToPhoneNumber = recipient.User.PhoneNumber,
                                          Content = $"New case created: {createdCase.Id}",
                                      });
                }
            }
        }

    }
}
