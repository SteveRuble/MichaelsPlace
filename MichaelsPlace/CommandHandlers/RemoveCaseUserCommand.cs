using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using MediatR;
using MichaelsPlace.CommandHandlers;
using MichaelsPlace.Models.Persistence;

namespace MichaelsPlace.CommandHandlers
{
    public class RemoveCaseUserCommand : CommandHandlerBase, IAsyncRequestHandler<RemoveCaseUserCommand.Request, ICommandResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public class Request : IAsyncRequest<ICommandResult>
        {
            public int UserId { get; set; }
            public string CaseId { get; set; }
        }

        public RemoveCaseUserCommand(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<ICommandResult> Handle(Request message)
        {
            var @case = (from cases in _dbContext.Cases
                         where cases.Id == message.CaseId
                         select cases).First();

            var casePerson = @case.CaseUsers.First(cu => cu.Id == message.UserId);
            if (casePerson == null)
            {
                return CommandResult.FailureAsync();
            }

            @case.CaseUsers.Remove(casePerson);
            _dbContext.SaveChanges();

            return CommandResult.SuccessAsync();
        }
    }
}