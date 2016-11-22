using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using MediatR;
using MichaelsPlace.Models.Persistence;

namespace MichaelsPlace.CommandHandlers
{
    public class DeleteCaseCommand : CommandHandlerBase, IAsyncRequestHandler<DeleteCaseCommand.Request, ICommandResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public class Request : IAsyncRequest<ICommandResult>
        {
            public string CaseId { get; set; }
        }

        public DeleteCaseCommand(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<ICommandResult> Handle(Request message)
        {
            var @case = (from cases in _dbContext.Cases
                         where cases.Id == message.CaseId
                         select cases).First();

            @case.IsDeleted = true;

            _dbContext.SaveChanges();

            return CommandResult.SuccessAsync(@case.Id);
        }
    }
}