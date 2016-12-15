using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using MediatR;
using MichaelsPlace.Models.Persistence;

namespace MichaelsPlace.CommandHandlers
{
    public class EditOrganizationCommand : CommandHandlerBase, IAsyncRequestHandler<EditOrganizationCommand.Request, ICommandResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public class Request : IAsyncRequest<ICommandResult>
        {
            public string Name { get; set; }
            public string PhoneNumber { get; set; }
            public string FaxNumber { get; set; }
            public string Notes { get; set; }
            public Address Address { get; set; }
            public int OrganizationId { get; set; }
        }

        public EditOrganizationCommand(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<ICommandResult> Handle(Request message)
        {
            var organization = (from organizations in _dbContext.Organizations
                                where organizations.Id == message.OrganizationId
                                select organizations).First();

            organization.Name = message.Name;
            organization.PhoneNumber = message.PhoneNumber;
            organization.FaxNumber = message.FaxNumber;
            organization.Notes = message.Notes;
            organization.Address = message.Address;

            _dbContext.SaveChanges();

            return CommandResult.SuccessAsync(organization.Id);
        }
    }
}