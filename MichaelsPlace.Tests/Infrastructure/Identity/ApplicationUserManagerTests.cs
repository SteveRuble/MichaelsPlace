using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using MichaelsPlace.Infrastructure.Identity;
using NUnit.Framework;

namespace MichaelsPlace.Tests.Infrastructure.Identity
{
    public class ApplicationUserManagerTests : IntegrationTestBase
    {
        public ApplicationUserManager Target { get; set; }

        [SetUp]
        public void SetUp()
        {
            Target = new ApplicationUserManager(new ApplicationUserStore(DbContext));
        }

        [Test]
        public async Task email_address_round_trips()
        {
            var user = DbContext.Users.First();
            var expected = user.UserName + "1";

            await Target.SetEmailAsync(user.Id, expected);

            var actual = await Target.GetEmailAsync(user.Id);

            actual.Should().Be(expected);

            var actualUser = await Target.FindByEmailAsync(expected);

            actualUser.Should().NotBeNull();
            actualUser.Id.Should().Be(user.Id);
        }

        [Test]
        public async Task phone_number_round_trips()
        {
            var user = DbContext.Users.First();
            var expected = "9876543210";

            await Target.SetPhoneNumberAsync(user.Id, expected);

            var actual = await Target.GetPhoneNumberAsync(user.Id);

            actual.Should().Be(expected);
        }
    }
}
