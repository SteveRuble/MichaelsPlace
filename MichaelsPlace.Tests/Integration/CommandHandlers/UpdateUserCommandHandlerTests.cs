using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using FluentAssertions;
using MichaelsPlace.CommandHandlers;
using MichaelsPlace.Controllers.Admin;
using MichaelsPlace.Infrastructure;
using MichaelsPlace.Infrastructure.Identity;
using MichaelsPlace.Models.Admin;
using Microsoft.AspNet.Identity.EntityFramework;
using Ninject;
using NUnit.Framework;

namespace MichaelsPlace.Tests.Integration.CommandHandlers
{
    public class UpdateUserCommandHandlerTests : IntegrationTestBase
    {
        public UpdateUserCommandHandler Target => MockingKernel.Get<UpdateUserCommandHandler>();

        public ModelStateDictionary ModelState { get; set; }

        [SetUp]
        public void SetUp()
        {
            MockingKernel.Load<TestModules.Http>();
            ModelState = new ModelStateDictionary();
        }

        [Test]
        public async Task make_disabled()
        {
            var user = DbContext.Users.First();
            var model = MockingKernel.Get<IMapper>().Map<UserModel>(user);
            model.IsDisabled = true;

            var result = await Target.Handle(new UpdateUserCommand(model, Enumerable.Empty<string>(), ModelState));

            result.IsSuccess.Should().BeTrue();
            
            var actual = DbContext.Users.ProjectTo<UserModel>(Target.Mapper).First();
            actual.IsDisabled.Should().BeTrue();
        }

        [Test]
        public async Task post_edit_roles()
        {
            var user = DbContext.Users.First();
            var applicationUserManager = MockingKernel.Get<ApplicationUserManager>();
            var applicationRoleManager = MockingKernel.Get<ApplicationRoleManager>();
            await applicationRoleManager.CreateAsync(new IdentityRole(TestConstants.IdA));
            await applicationRoleManager.CreateAsync(new IdentityRole(TestConstants.IdB));
            await applicationRoleManager.CreateAsync(new IdentityRole(TestConstants.IdC));

            await applicationUserManager.AddToRoleAsync(user.Id, TestConstants.IdA);

            var expectedRoles = new List<string>() { TestConstants.IdB, TestConstants.IdC };

            var model = MockingKernel.Get<IMapper>().Map<UserModel>(user);

            var result = await Target.Handle(new UpdateUserCommand(model, expectedRoles, ModelState));

            result.IsSuccess.Should().BeTrue();

            var roles = await applicationUserManager.GetRolesAsync(user.Id);

            roles.Should().BeEquivalentTo(expectedRoles);
        }
    }
}
