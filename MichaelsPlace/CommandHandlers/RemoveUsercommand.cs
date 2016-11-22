using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using MediatR;
using MichaelsPlace.Models.Persistence;

namespace MichaelsPlace.CommandHandlers
{
    public class RemoveUserCommand : CommandHandlerBase, IAsyncRequestHandler<RemoveUserCommand.Request, ICommandResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public class Request : IAsyncRequest<ICommandResult>
        {
            public int UserId { get; set; }
            public int OrganizationId { get; set; }
        }

        public RemoveUserCommand(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<ICommandResult> Handle(Request message)
        {
            var organization = (from organizations in _dbContext.Organizations
                          where organizations.Id == message.OrganizationId
                          select organizations).First();

            var orgPerson = organization.OrganizationPeople.First(op => op.Id == message.UserId);
            if (orgPerson == null)
            {
                return CommandResult.FailureAsync();
            }

            organization.OrganizationPeople.Remove(orgPerson);

            _dbContext.SaveChanges();

            return CommandResult.SuccessAsync();
        }
    }
}