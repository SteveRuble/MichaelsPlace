using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using MichaelsPlace.Models.Admin;
using MichaelsPlace.Models.Persistence;
using Ninject;
using NUnit.Framework;

namespace MichaelsPlace.Tests
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
        public void person_personModel_disabled_mapping()
        {
            var person = new Person()
                         {
                             ApplicationUser = new ApplicationUser()
                                               {
                                                   LockoutEndDateUtc = Constants.Magic.DisabledLockoutEndDate.UtcDateTime
                                               }
                         };
            person.ApplicationUser.LockoutEndDateUtc.Should().Be(Constants.Magic.DisabledLockoutEndDate.UtcDateTime);
            var personModel = Mapper.Map<PersonModel>(person);
            personModel.IsDisabled.Should().BeTrue();
            personModel.IsLockedOut.Should().BeTrue();
        }
    }
}
