using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MichaelsPlace.Models.Persistence
{
    public class ApplicationUser : IdentityUser
    {
        private ICollection<UserPreference> _preferences;

        [DisplayName("Disabled")]
        public virtual bool IsDisabled { get; set; }

        public virtual ICollection<UserPreference> Preferences
        {
            get { return _preferences ?? (_preferences = new HashSet<UserPreference>()); }
            set { _preferences = value; }
        }

        public virtual Person Person { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}