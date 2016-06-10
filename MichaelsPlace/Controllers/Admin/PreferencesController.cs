using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MichaelsPlace.Extensions;
using MichaelsPlace.Models.Persistence;
using MichaelsPlace.Queries;
using MichaelsPlace.Services;

namespace MichaelsPlace.Controllers.Admin
{
    public class PreferencesViewModel
    {
        public List<SubscriptionPreferenceViewModel> SubscriptionPreferences { get; set; }
    }

    public class SubscriptionPreferenceViewModel
    {
        public int? Id { get; set; }
        public string SubscriptionName { get; set; }
        public string SubscriptionDescription { get; set; }
        public bool IsSmsRequested { get; set; }

        public bool IsEmailRequested { get; set; }
    }

    [Authorize]
    public class PreferencesController : AdminControllerBase
    {
        private readonly ReflectionService _reflectionService;
        private readonly UserQueries _userQueries;

        public PreferencesController(ReflectionService reflectionService)
        {
            _reflectionService = reflectionService;
        }

        public ActionResult Display()
        {
            var userId = User.GetUserId();
            var viewModel = BuildPreferencesViewModel(userId);

            return PartialView("Display", viewModel);
        }


        public ActionResult Edit()
        {
            var userId = User.GetUserId();
            var viewModel = BuildPreferencesViewModel(userId);

            return PartialView("Edit", viewModel);
        }

        private PreferencesViewModel BuildPreferencesViewModel(string userId)
        {
            var subscriptionPreferences = DbContext.UserPreferences.OfType<SubscriptionPreference>().Where(u => u.User.Id == userId).ToList();
            var subscriptionPreferenceViewModels = from description in _reflectionService.GetSubscriptionDescriptions()
                                                   from maybeNullPreference in subscriptionPreferences.Where(sp => sp.SubscriptionName == description.Name).DefaultIfEmpty()
                                                   orderby description.Name
                                                   select new SubscriptionPreferenceViewModel()
                                                          {
                                                              Id = maybeNullPreference?.Id,
                                                              SubscriptionName = description.Name,
                                                              SubscriptionDescription = description.Description,
                                                              IsSmsRequested = maybeNullPreference?.IsSmsRequested ?? false,
                                                              IsEmailRequested = maybeNullPreference?.IsEmailRequested ?? false,
                                                          };

            var viewModel = new PreferencesViewModel()
                            {
                                SubscriptionPreferences = subscriptionPreferenceViewModels.ToList()
                            };
            return viewModel;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PreferencesViewModel viewModel)
        {
            var userId = User.GetUserId();

            if (!ModelState.IsValid)
            {
                return PartialView("Edit", viewModel);
            }

            var user = DbContext.Users.Include(u => u.Preferences).First(u => u.Id == userId);

            var subscriptionPreferences = user.Preferences.OfType<SubscriptionPreference>()
                                              .Where(u => u.User.Id == userId)
                                              .ToDictionary(p => p.Id);

            foreach (var postedSubscriptionPreference in viewModel.SubscriptionPreferences)
            {
                SubscriptionPreference subscriptionPreference;
                if (postedSubscriptionPreference.Id == null 
                    || !subscriptionPreferences.TryGetValue(postedSubscriptionPreference.Id.Value, out subscriptionPreference))
                {
                    subscriptionPreference = DbContext.Set<SubscriptionPreference>().Add(new SubscriptionPreference()
                                                                                         {
                                                                                             SubscriptionName = postedSubscriptionPreference.SubscriptionName,
                                                                                             User = user
                                                                                         });
                }

                subscriptionPreference.IsEmailRequested = postedSubscriptionPreference.IsEmailRequested;
                subscriptionPreference.IsSmsRequested = postedSubscriptionPreference.IsSmsRequested;
            }

            DbContext.SaveChanges();

            return Display();
        }
    }
}