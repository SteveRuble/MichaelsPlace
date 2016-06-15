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
using Ninject;
using NUnit.Framework;

namespace MichaelsPlace.Tests.Integration.CommandHandlers
{
    public class AddOrEditPersonCommandHandlerTests : IntegrationTestBase
    {
        public AddOrEditPersonCommandHandler Target => MockingKernel.Get<AddOrEditPersonCommandHandler>();

        public ModelStateDictionary ModelState { get; set; }

        [SetUp]
        public void SetUp()
        {
            MockingKernel.Load<TestModules.Http>();
            ModelState = new ModelStateDictionary();
        }

        [Test]
        public async Task edit()
        {
            var person = DbContext.People.First();
            var model = MockingKernel.Get<IMapper>().Map<PersonModel>(person);
            
            model.FirstName = "first";
            model.LastName = "last";
            model.EmailAddress = "email@example.com";

            var result = await Target.Handle(new AddOrEditPersonCommand(model, ModelState));

            result.IsSuccess.Should().BeTrue();

            var actual = DbContext.People.ProjectTo<PersonModel>(Target.Mapper).First();

            actual.ShouldBeEquivalentTo(model);
        }

        [Test]
        public async Task add()
        {
            var model = new PersonModel
                        {
                            FirstName = "first",
                            LastName = "last",
                            EmailAddress = "email@example.com"
                        };

            var result = await Target.Handle(new AddOrEditPersonCommand(model, ModelState));

            result.IsSuccess.Should().BeTrue();
            result.Result.Should().BeOfType<string>();
            var id = result.Result.ToString();

            var actual = DbContext.People.ProjectTo<PersonModel>(Target.Mapper).First(p => p.Id == id);

            actual.ShouldBeEquivalentTo(model, o => o.Including(p => p.FirstName).Including(p => p.LastName).Including(p => p.EmailAddress));
        }
    }
}
