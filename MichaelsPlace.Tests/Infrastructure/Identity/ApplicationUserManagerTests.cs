using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using MichaelsPlace.Infrastructure.Identity;
using MichaelsPlace.Utilities;
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

        [Test]
        public async Task ensure_has_claim()
        {
            var claim = new Claim(SomeRandom.String(), SomeRandom.String());
            var user = DbContext.Users.First();

            var result = await Target.EnsureHasClaimAsync(user.Id, claim.Type, claim.Value);
            result.Succeeded.Should().BeTrue();
            await Target.EnsureHasClaimAsync(user.Id, claim.Type, claim.Value);

            var claims = await Target.GetClaimsAsync(user.Id);
            claims.Where(c => c.Type == claim.Type).Should().HaveCount(1);
        }

        [Test]
        public async Task ensure_does_not_have_claim()
        {
            var claim = new Claim(SomeRandom.String(), SomeRandom.String());
            var user = DbContext.Users.First();

            var result = await Target.AddClaimAsync(user.Id, claim);
            result.Succeeded.Should().BeTrue();

            result = await Target.EnsureDoesNotHaveClaimAsync(user.Id, claim.Type, claim.Value);
            result.Succeeded.Should().BeTrue();

            var claims = await Target.GetClaimsAsync(user.Id);
            claims.Where(c => c.Type == claim.Type).Should().HaveCount(0);
        }
    }
}
