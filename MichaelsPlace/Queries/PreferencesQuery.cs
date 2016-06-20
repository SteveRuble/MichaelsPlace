using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Threading.Tasks;
using AutoMapper;
using JetBrains.Annotations;
using MichaelsPlace.Infrastructure;
using MichaelsPlace.Models.Persistence;

namespace MichaelsPlace.Queries
{
    public class UserPreferencesModel
    {
        public ApplicationUser User { get; set; }

        public Person Person { get; set; }

        public IEnumerable<SubscriptionPreference> SubscriptionPreferences { get; set; } = Enumerable.Empty<SubscriptionPreference>();
    }

    public class SubscriptionPreferenceDetails
    {
        public string UserId { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsEmailRequested { get; set; }
        public bool IsSmsRequested { get; set; }
    }
    
    public class PreferencesQuery : QueryBase
    {
        public virtual IQueryable<SubscriptionPreferenceDetails> GetSubscriptionPreferenceDetails([NotNull] string subscriptionName)
        {
            if (subscriptionName == null) throw new ArgumentNullException(nameof(subscriptionName));

            return DbSets.Set<SubscriptionPreference>()
                            .Where(sp => sp.SubscriptionName == subscriptionName)
                            .Select(sp => new SubscriptionPreferenceDetails()
                                          {
                                              UserId = sp.User.Id,
                                              EmailAddress = sp.User.Person.EmailAddress,
                                              PhoneNumber = sp.User.Person.PhoneNumber,
                                              IsSmsRequested = sp.IsSmsRequested,
                                              IsEmailRequested = sp.IsEmailRequested,
                                          });
        }

        /// <summary>
        /// Gets preferences of the specified <typeparamref name="TPreference"/> type,
        /// including the associated user.
        /// </summary>
        /// <typeparam name="TPreference"></typeparam>
        /// <returns></returns>
        public virtual IQueryable<UserPreferencesModel> Execute<TPreference>() where TPreference : UserPreference
        {
            return DbSets.Set<ApplicationUser>().Select(u => new UserPreferencesModel()
                                               {
                                                   User = u,
                                                   Person = u.Person,
                                                   SubscriptionPreferences = u.Preferences.OfType<SubscriptionPreference>()
                                               });
        }
    }
}
