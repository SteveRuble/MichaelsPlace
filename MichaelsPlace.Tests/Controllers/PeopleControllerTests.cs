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
                  .ModelAs<IEnumerable<PersonModel>>().Should().NotBeEmpty();
        }

        [Test]
        public void get_edit()
        {
            var id = DbContext.People.First().Id;
            Target.Edit(id).Should().BeViewResult()
                  .ModelAs<PersonModel>().Should().NotBeNull();
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
            var result = await Target.Edit(person.Id, model);
            result.Should().BeRedirectToRouteResult().WithAction("Index");

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
            var result = await Target.Edit(person.Id, model);
            result.Should().BeRedirectToRouteResult().WithAction("Index");

            var actualPerson = DbContext.People.First();
            var actual = DbContext.People.ProjectTo<PersonModel>(Target.Mapper).First();
            actual.IsDisabled.Should().BeTrue();
        }
    }
}
