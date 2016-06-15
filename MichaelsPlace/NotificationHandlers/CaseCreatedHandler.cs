using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using MichaelsPlace.Attributes;
using MichaelsPlace.Infrastructure;
using MichaelsPlace.Models.Persistence;
using MichaelsPlace.Queries;

namespace MichaelsPlace.Handlers
{
    [UsedImplicitly]
    [SubscriptionDescription(Constants.Subscriptions.UserCaseCreated, "A user has created a new case.")]
    [SubscriptionDescription(Constants.Subscriptions.OrganizationCaseCreated, "A case has been created for an organization.")]
    public class CaseAddedListener : INotificationHandler<EntityAdded<Case>>
    {
        private readonly PreferencesQuery _preferencesQuery;
        private readonly ISingleEntityService _singleEntityService;

        public CaseAddedListener(PreferencesQuery preferencesQuery, ISingleEntityService singleEntityService)
        {
            _preferencesQuery = preferencesQuery;
            _singleEntityService = singleEntityService;
        }

        public void Handle(EntityAdded<Case> notification)
        {
            var createdCase = notification.Entity;

            var isOrganizationCase = createdCase.Organization != null;
            var subscriptionName = isOrganizationCase ? Constants.Subscriptions.OrganizationCaseCreated : Constants.Subscriptions.UserCaseCreated;

            var subscribers = _preferencesQuery.GetSubscriptionPreferenceDetails(subscriptionName);

            foreach (var recipient in subscribers)
            {
                if (recipient.IsEmailRequested)
                {
                    _singleEntityService.Save(new EmailNotification()
                                      {
                                          ToAddress = recipient.EmailAddress,
                                          Content = $"New case created: {createdCase.Id}",
                                          Subject = $"New case created: {createdCase.Id}"
                                      });
                }
                if (recipient.IsSmsRequested)
                {
                    _singleEntityService.Save(new SmsNotification()
                                      {
                                          ToPhoneNumber = recipient.PhoneNumber,
                                          Content = $"New case created: {createdCase.Id}",
                                      });
                }
            }
        }
    }
}
