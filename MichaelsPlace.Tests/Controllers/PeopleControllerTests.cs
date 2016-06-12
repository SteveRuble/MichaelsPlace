using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using FluentAssertions;
using FluentAssertions.Mvc;
using MichaelsPlace.Controllers.Admin;
using MichaelsPlace.Infrastructure;
using MichaelsPlace.Infrastructure.Identity;
using MichaelsPlace.Models.Admin;
using MichaelsPlace.Models.Persistence;
using Microsoft.AspNet.Identity.EntityFramework;
using Ninject;
using NUnit.Framework;

namespace MichaelsPlace.Tests.Controllers
{
    [TestFixture]
    public class PeopleControllerTests : ControllerIntegrationTestBase<PeopleController>
    {
        [Test]
        public void get_index()
        {
            Target.Index().Should().BeViewResult()
                  .ModelAs<PeopleIndexViewModel> ().People.Should().NotBeEmpty();
        }

        [Test]
        public void get_edit()
        {
            var id = DbContext.People.First().Id;
            Target.Edit(id).Should().BePartialViewResult()
                  .ModelAs<PersonEditViewModel>().Should().NotBeNull();
        }

        [Test]
        public async Task post_edit()
        {
            var person = DbContext.People.First();
            await MockingKernel.Get<ApplicationUserManager>().SetLockoutEndDateAsync(person.Id, DateTimeOffset.Now.AddHours(1));
            var model = MockingKernel.Get<IMapper>().Map<PersonModel>(person);
            model.IsLockedOut = false;
            model.FirstName = "first";
            model.LastName = "last";
            model.EmailAddress = "email@example.com";
            var result = await Target.Edit(person.Id, new PersonEditViewModel() {Person = model}, null);
            result.Should().BePartialViewResult().WithViewName("EditCompleted");

            var actual = DbContext.People.ProjectTo<PersonModel>(Target.Mapper).First();
            actual.ShouldBeEquivalentTo(model);
        }

        [Test]
        public async Task post_edit_make_disabled()
        {
            var person = DbContext.People.First();
            await MockingKernel.Get<ApplicationUserManager>().SetLockoutEndDateAsync(person.Id, DateTimeOffset.Now.AddHours(1));
            var model = MockingKernel.Get<IMapper>().Map<PersonModel>(person);
            model.IsDisabled = true;
            var result = await Target.Edit(person.Id, new PersonEditViewModel() { Person = model }, null);
            result.Should().BePartialViewResult().WithViewName("EditCompleted");

            var actualPerson = DbContext.People.First();
            var actual = DbContext.People.ProjectTo<PersonModel>(Target.Mapper).First();
            actual.IsDisabled.Should().BeTrue();
        }

        [Test]
        public async Task post_edit_roles()
        {
            var person = DbContext.People.First();
            var applicationUserManager = MockingKernel.Get<ApplicationUserManager>();
            var applicationRoleManager = MockingKernel.Get<ApplicationRoleManager>();
            await applicationRoleManager.CreateAsync(new IdentityRole(TestConstants.IdA));
            await applicationRoleManager.CreateAsync(new IdentityRole(TestConstants.IdB));
            await applicationRoleManager.CreateAsync(new IdentityRole(TestConstants.IdC));
            await applicationUserManager.AddToRoleAsync(person.Id, TestConstants.IdA);
            var expectedRoles = new List<string>() {TestConstants.IdB, TestConstants.IdC};

            var model = MockingKernel.Get<IMapper>().Map<PersonModel>(person);

            var result = await Target.Edit(person.Id, new PersonEditViewModel() { Person = model }, expectedRoles);

            result.Should().BePartialViewResult().WithViewName("EditCompleted");

            var roles = await applicationUserManager.GetRolesAsync(person.Id);

            roles.Should().BeEquivalentTo(expectedRoles);
        }
    }
}
