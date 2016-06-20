using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using FluentAssertions.Mvc;
using MediatR;
using MichaelsPlace.CommandHandlers;
using MichaelsPlace.Controllers.Admin;
using MichaelsPlace.Infrastructure;
using MichaelsPlace.Infrastructure.Identity;
using MichaelsPlace.Models.Admin;
using MichaelsPlace.Tests.TestHelpers;
using Microsoft.AspNet.Identity.EntityFramework;
using Moq;
using Ninject;
using NUnit.Framework;

namespace MichaelsPlace.Tests.Integration.Controllers
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
            Target.Edit(id).Should().BeViewResult()
                  .ModelAs<PersonEditViewModel>().Should().NotBeNull();
        }

        [Test]
        public async Task post_edit()
        {
            var mockMediator = MockingKernel.GetMock<IMediator>();
            mockMediator.Setup(m => m.SendAsync(It.IsAny<AddOrEditPersonCommand>())).ReturnsAsync(CommandResult.Success());
            mockMediator.Setup(m => m.SendAsync(It.IsAny<UpdateUserCommand>())).ReturnsAsync(CommandResult.Success());

            var model = new PersonModel
                        {
                            Id = TestConstants.IdA,
                        };

            var result = await Target.Edit(model.Id, new PersonEditViewModel() {Person = model}, null);

            result.Should().BeHttpStatusResult(HttpStatusCode.Accepted);

            mockMediator.Verify(m => m.SendAsync(It.Is<AddOrEditPersonCommand>(c => c.Person == model)));
        }
    }
}
