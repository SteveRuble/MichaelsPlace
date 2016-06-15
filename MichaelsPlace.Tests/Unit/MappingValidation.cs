using AutoMapper;
using FluentAssertions;
using MichaelsPlace.Models.Admin;
using MichaelsPlace.Models.Persistence;
using Ninject;
using NUnit.Framework;

namespace MichaelsPlace.Tests.Unit
{
    [TestFixture]
    public class MappingValidation
    {
        public IMapper Mapper { get; set; }

        [OneTimeSetUp]
        public void SetUpFixture()
        {
            var kernel = new StandardKernel();
            kernel.Load<Modules.Mapping>();
            Mapper = kernel.Get<IMapper>();
        }

        [Test]
        public void mapping_is_valid()
        {
            Mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }

        [Test]
        public void appliationUser_userModel_disabled_mapping()
        {
            var user = new ApplicationUser()
                                               {
                                                   LockoutEndDateUtc = Constants.Magic.DisabledLockoutEndDate.UtcDateTime
                                               };
            user.LockoutEndDateUtc.Should().Be(Constants.Magic.DisabledLockoutEndDate.UtcDateTime);
            var userModel = Mapper.Map<UserModel>(user);
            userModel.IsDisabled.Should().BeTrue();
            userModel.IsLockedOut.Should().BeTrue();
        }
    }
}
