using System.Data.Entity;
using System.Threading.Tasks;
using MichaelsPlace.Models.Persistence;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MichaelsPlace.Infrastructure.Identity
{
    public class ApplicationUserStore : UserStore<ApplicationUser>
    {
        public ApplicationDbContext ApplicationDbContext => (ApplicationDbContext) Context;

        public ApplicationUserStore()
        {
        }

        public ApplicationUserStore(DbContext context) : base(context)
        {
        }

        public override Task CreateAsync(ApplicationUser user)
        {
            if (user.Person == null)
            {
                user.Person = new Person();
            }

            return base.CreateAsync(user);
        }

        public override Task<string> GetPhoneNumberAsync(ApplicationUser user)
            => GetPersonAsync(user).ContinueWith(u => u.Result.PhoneNumber);

        public override Task SetPhoneNumberAsync(ApplicationUser user, string phoneNumber)
            => GetPersonAsync(user).ContinueWith(u => u.Result.PhoneNumber = phoneNumber);

        public override Task<string> GetEmailAsync(ApplicationUser user)
            => GetPersonAsync(user).ContinueWith(t => t.Result.EmailAddress);

        public override Task SetEmailAsync(ApplicationUser user, string email)
            => GetPersonAsync(user).ContinueWith(t => t.Result.EmailAddress = email);

        public override Task<ApplicationUser> FindByEmailAsync(string email)
            => ApplicationDbContext.Users.FirstOrDefaultAsync(u => u.Person.EmailAddress == email);

        private Task<Person> GetPersonAsync(ApplicationUser user) => ApplicationDbContext.People.FindAsync(user.Id);
    }
}