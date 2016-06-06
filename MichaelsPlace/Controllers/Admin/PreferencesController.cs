using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MichaelsPlace.Extensions;
using MichaelsPlace.Models.Persistence;
using MichaelsPlace.Queries;
using MichaelsPlace.Subscriptions;

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
        private readonly SubscriptionHelper _subscriptionHelper;
        private readonly UserQueries _userQueries;

        public PreferencesController(SubscriptionHelper subscriptionHelper)
        {
            _subscriptionHelper = subscriptionHelper;
        }

        public ActionResult Edit()
        {
            var userId = User.GetUserId();
            var subscriptionPreferences = DbContext.UserPreferences.OfType<SubscriptionPreference>().Where(u => u.User.Id == userId).ToList();
            var subscriptionPreferenceViewModels = from description in _subscriptionHelper.SubscriptionDescriptions
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

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PreferencesViewModel viewModel)
        {
            var userId = User.GetUserId();

            var subscriptionPreferences = DbContext.UserPreferences.OfType<SubscriptionPreference>().Where(u => u.User.Id == userId)
                                                   .ToDictionary(p => p.Id);

            throw new NotImplementedException();





        }
    }
}