using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MichaelsPlace.Infrastructure;
using MichaelsPlace.Models.Persistence;
using MichaelsPlace.Queries;
using MichaelsPlace.Subscriptions;

namespace MichaelsPlace.Handlers
{
    [UsedImplicitly]
    [SubscriptionDescription(Constants.Subscriptions.UserCaseCreated, "A user has created a new case.")]
    [SubscriptionDescription(Constants.Subscriptions.OrganizationCaseCreated, "A case has been created for an organization.")]
    public class CaseAddedListener : IListener
    {
        private readonly PreferencesQuery _preferencesQuery;
        private readonly IEntitySaver _entitySaver;

        public CaseAddedListener(PreferencesQuery preferencesQuery, IEntitySaver entitySaver)
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
            var isOrganizationCase = createdCase.Organization != null;
            var subscriptionName = isOrganizationCase ? Constants.Subscriptions.OrganizationCaseCreated : Constants.Subscriptions.UserCaseCreated;

            var subscribers = _preferencesQuery.GetSubscriptionPreferenceDetails(subscriptionName);

            foreach (var recipient in subscribers)
            {
                if (recipient.IsEmailRequested)
                {
                    _entitySaver.Save(new EmailNotification()
                                      {
                                          ToAddress = recipient.EmailAddress,
                                          Content = $"New case created: {createdCase.Id}",
                                          Subject = $"New case created: {createdCase.Id}"
                                      });
                }
                if (recipient.IsSmsRequested)
                {
                    _entitySaver.Save(new SmsNotification()
                                      {
                                          ToPhoneNumber = recipient.PhoneNumber,
                                          Content = $"New case created: {createdCase.Id}",
                                      });
                }
            }
        }

    }
}
