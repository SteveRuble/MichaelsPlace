using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using MichaelsPlace.Models;
using MichaelsPlace.Models.Admin;
using MichaelsPlace.Models.Persistence;

namespace MichaelsPlace.CommandHandlers
{
    public class CreateOrganizationCommand : CommandHandlerBase, IAsyncRequestHandler<CreateOrganizationCommand.Request, ICommandResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public class Request : IAsyncRequest<ICommandResult>
        {
            public string Name { get; set; }
            public string PhoneNumber { get; set; }
            public string FaxNumber { get; set; }
            public string Notes { get; set; }
            public Address Address { get; set; }
            public string UserId { get; set; }
        }

        public CreateOrganizationCommand(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<ICommandResult> Handle(Request message)
        {
            var person = (from people in _dbContext.People
                          where people.Id == message.UserId
                          select people).First();

            var organization = new Organization();
            organization.Name = message.Name;
            organization.PhoneNumber = message.PhoneNumber;
            organization.FaxNumber = message.FaxNumber;
            organization.Notes = message.Notes;
            organization.OrganizationPeople.Add(new OrganizationPerson()
            {
                Person = person,
                Organization = organization
            });

            organization.Address = message.Address;

            _dbContext.Organizations.Add(organization);
            _dbContext.SaveChanges();

            return CommandResult.SuccessAsync(organization.Id);
        }
    }
}