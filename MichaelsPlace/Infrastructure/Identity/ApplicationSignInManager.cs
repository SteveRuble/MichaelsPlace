using System.Security.Claims;
using System.Threading.Tasks;
using MichaelsPlace.Models.Persistence;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;

namespace MichaelsPlace.Infrastructure.Identity
{
    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.

    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }

        public override async Task SignInAsync(ApplicationUser user, bool isPersistent, bool rememberBrowser)
        {
            ClaimsIdentity userIdentity = await this.CreateUserIdentityAsync(user);
            this.AuthenticationManager.SignOut(new string[2]
                                               {
                                                   "ExternalCookie",
                                                   "TwoFactorCookie"
                                               });
            if (rememberBrowser)
            {
                ClaimsIdentity rememberBrowserIdentity = this.AuthenticationManager.CreateTwoFactorRememberBrowserIdentity(this.ConvertIdToString(user.Id));
                this.AuthenticationManager.SignIn(new AuthenticationProperties()
                                                  {
                                                      IsPersistent = isPersistent
                                                  }, userIdentity, rememberBrowserIdentity);
            }
            else
            {
                this.AuthenticationManager.SignIn(new AuthenticationProperties()
                                                  {
                                                      IsPersistent = isPersistent
                                                  }, userIdentity);
            }
        }
    }
}
