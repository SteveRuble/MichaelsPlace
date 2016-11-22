using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using MediatR;
using MichaelsPlace.Models.Persistence;

namespace MichaelsPlace.CommandHandlers
{
    public class DeleteOrganizationCommand : CommandHandlerBase, IAsyncRequestHandler<DeleteOrganizationCommand.Request, ICommandResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public class Request : IAsyncRequest<ICommandResult>
        {
            public int OrganizationId { get; set; }
        }

        public DeleteOrganizationCommand(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<ICommandResult> Handle(Request message)
        {
            var organization = (from organizations in _dbContext.Organizations
                         where organizations.Id == message.OrganizationId
                         select organizations).First();

            organization.IsDeleted = true;

            _dbContext.SaveChanges();

            return CommandResult.SuccessAsync(organization.Id);
        }
    }
}